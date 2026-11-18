using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class CourseStudyPage : Page
    {
        private int _courseId;
        private string UserRole => Session["UserRole"] != null ? Session["UserRole"].ToString() : "";
        private int CurrentUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("Please log in to access course study materials and quizzes."));
                return;
            }

            if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out _courseId))
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseHeader(_courseId);
                VerifyEnrollmentAndLoad(_courseId);
            }
        }

        private void LoadCourseHeader(int courseId)
        {
            try
            {
                string sql = "SELECT CourseId, CourseName, CourseCode, Category, InstructorName, DurationInWeeks FROM Courses WHERE CourseId = @CourseId";
                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("Index.aspx");
                    return;
                }

                DataRow row = dt.Rows[0];
                litCourseName.Text = Server.HtmlEncode(row["CourseName"].ToString());
                litCourseCode.Text = Server.HtmlEncode(row["CourseCode"].ToString());
                litCategory.Text = Server.HtmlEncode(row["Category"].ToString());
                litInstructor.Text = Server.HtmlEncode(row["InstructorName"].ToString());
                litDuration.Text = row["DurationInWeeks"].ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading course details: " + ex.Message, "alert-danger");
            }
        }

        private void VerifyEnrollmentAndLoad(int courseId)
        {
            try
            {
                // Check enrollment if student
                if (UserRole == "Student")
                {
                    string checkSql = "SELECT EnrollmentId, IsCompleted, CompletionDate, QuizScore, QuizTotal FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId";
                    var checkParams = new[]
                    {
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", courseId)
                    };
                    DataTable dtEnroll = DBHelper.ExecuteSelect(checkSql, checkParams);

                    if (dtEnroll.Rows.Count == 0)
                    {
                        // Student is not enrolled in this course: block access
                        pnlRestricted.Visible = true;
                        pnlStudyContent.Visible = false;
                        return;
                    }

                    // Enrolled!
                    pnlRestricted.Visible = false;
                    pnlStudyContent.Visible = true;

                    DataRow row = dtEnroll.Rows[0];
                    bool isCompleted = row["IsCompleted"] != DBNull.Value && Convert.ToBoolean(row["IsCompleted"]);
                    if (isCompleted)
                    {
                        pnlCompletedBadge.Visible = true;
                        litCompletedScore.Text = row["QuizScore"] != DBNull.Value ? row["QuizScore"].ToString() : "2";
                        litCompletedDate.Text = row["CompletionDate"] != DBNull.Value 
                            ? Convert.ToDateTime(row["CompletionDate"]).ToString("MMMM dd, yyyy HH:mm") 
                            : DateTime.Now.ToString("MMMM dd, yyyy");
                        btnSubmitQuiz.Text = "Retake Quiz & Update Score";
                    }
                    else
                    {
                        pnlCompletedBadge.Visible = false;
                        btnSubmitQuiz.Text = "Submit Quiz & Complete Course";
                    }
                }
                else
                {
                    // Admin or Instructor preview
                    pnlRestricted.Visible = false;
                    pnlStudyContent.Visible = true;
                    pnlCompletedBadge.Visible = false;
                }

                LoadMaterials(courseId);
                LoadQuiz(courseId);
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading study materials: " + ex.Message, "alert-danger");
            }
        }

        private void LoadMaterials(int courseId)
        {
            try
            {
                string sql = "SELECT * FROM CourseMaterials WHERE CourseId = @CourseId ORDER BY CreatedAt ASC";
                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                DataView dvText = new DataView(dt);
                dvText.RowFilter = "MaterialType = 'Text'";
                rptTextMaterials.DataSource = dvText;
                rptTextMaterials.DataBind();
                lblNoTextMaterials.Visible = (dvText.Count == 0);

                DataView dvFile = new DataView(dt);
                dvFile.RowFilter = "MaterialType = 'File'";
                rptFileMaterials.DataSource = dvFile;
                rptFileMaterials.DataBind();
                lblNoFileMaterials.Visible = (dvFile.Count == 0);

                litMaterialsCount.Text = dt.Rows.Count.ToString() + " item(s)";
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to retrieve materials: " + ex.Message, "alert-danger");
            }
        }

        private void LoadQuiz(int courseId)
        {
            try
            {
                string sql = "SELECT * FROM CourseQuizzes WHERE CourseId = @CourseId ORDER BY QuestionNumber ASC";
                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count < 2)
                {
                    // Seed fallback 2 questions if none configured yet
                    EnsureDefaultQuizQuestions(courseId);
                    dt = DBHelper.ExecuteSelect(sql, parameters);
                }

                if (dt.Rows.Count >= 1)
                {
                    DataRow q1 = dt.Rows[0];
                    litQ1Text.Text = Server.HtmlEncode(q1["QuestionText"].ToString());
                    rblQ1.Items.Clear();
                    rblQ1.Items.Add(new ListItem("A) " + Server.HtmlEncode(q1["OptionA"].ToString()), "A"));
                    rblQ1.Items.Add(new ListItem("B) " + Server.HtmlEncode(q1["OptionB"].ToString()), "B"));
                    rblQ1.Items.Add(new ListItem("C) " + Server.HtmlEncode(q1["OptionC"].ToString()), "C"));
                    rblQ1.Items.Add(new ListItem("D) " + Server.HtmlEncode(q1["OptionD"].ToString()), "D"));
                }

                if (dt.Rows.Count >= 2)
                {
                    DataRow q2 = dt.Rows[1];
                    litQ2Text.Text = Server.HtmlEncode(q2["QuestionText"].ToString());
                    rblQ2.Items.Clear();
                    rblQ2.Items.Add(new ListItem("A) " + Server.HtmlEncode(q2["OptionA"].ToString()), "A"));
                    rblQ2.Items.Add(new ListItem("B) " + Server.HtmlEncode(q2["OptionB"].ToString()), "B"));
                    rblQ2.Items.Add(new ListItem("C) " + Server.HtmlEncode(q2["OptionC"].ToString()), "C"));
                    rblQ2.Items.Add(new ListItem("D) " + Server.HtmlEncode(q2["OptionD"].ToString()), "D"));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading quiz questions: " + ex.Message, "alert-danger");
            }
        }

        private void EnsureDefaultQuizQuestions(int courseId)
        {
            try
            {
                string insertSql = @"
                    IF NOT EXISTS (SELECT 1 FROM CourseQuizzes WHERE CourseId = @CourseId AND QuestionNumber = 1)
                    BEGIN
                        INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                        VALUES (@CourseId, 1, 'What is the primary objective and core focus of this course?', 
                                'Theoretical foundations and practical implementation', 'Random memorization without practice', 'Bypassing academic standards', 'Outdated legacy patterns', 'A');
                    END

                    IF NOT EXISTS (SELECT 1 FROM CourseQuizzes WHERE CourseId = @CourseId AND QuestionNumber = 2)
                    BEGIN
                        INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                        VALUES (@CourseId, 2, 'Which development practice ensures maximum code reliability, maintainability, and security?', 
                                'Hardcoding sensitive database passwords', 'Applying industry best practices, testing, and parameterized queries', 'Ignoring error handling entirely', 'Disabling input validation', 'B');
                    END";

                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DBHelper.ExecuteNonQuery(insertSql, parameters);
            }
            catch (Exception)
            {
            }
        }

        protected void btnSubmitQuiz_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string ans1 = rblQ1.SelectedValue;
            string ans2 = rblQ2.SelectedValue;

            if (string.IsNullOrEmpty(ans1) || string.IsNullOrEmpty(ans2))
            {
                ShowMessage("Please answer both questions before submitting.", "alert-warning");
                return;
            }

            try
            {
                // Retrieve correct answers
                string sql = "SELECT QuestionNumber, CorrectOption FROM CourseQuizzes WHERE CourseId = @CourseId ORDER BY QuestionNumber ASC";
                var parameters = new[] { new SqlParameter("@CourseId", _courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                string corr1 = "B";
                string corr2 = "B";

                if (dt.Rows.Count >= 1) corr1 = dt.Rows[0]["CorrectOption"].ToString().Trim().ToUpper();
                if (dt.Rows.Count >= 2) corr2 = dt.Rows[1]["CorrectOption"].ToString().Trim().ToUpper();

                int score = 0;
                if (ans1.Equals(corr1, StringComparison.OrdinalIgnoreCase)) score++;
                if (ans2.Equals(corr2, StringComparison.OrdinalIgnoreCase)) score++;

                if (UserRole == "Student" && CurrentUserId > 0)
                {
                    // Update enrollment record with completion and quiz score
                    string updateSql = @"
                        UPDATE Enrollments 
                        SET IsCompleted = 1, 
                            Status = 'Completed', 
                            CompletionDate = GETDATE(), 
                            QuizScore = @Score, 
                            QuizTotal = 2
                        WHERE UserId = @UserId AND CourseId = @CourseId";

                    var updateParams = new[]
                    {
                        new SqlParameter("@Score", score),
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", _courseId)
                    };

                    DBHelper.ExecuteNonQuery(updateSql, updateParams);

                    pnlCompletedBadge.Visible = true;
                    litCompletedScore.Text = score.ToString();
                    litCompletedDate.Text = DateTime.Now.ToString("MMMM dd, yyyy HH:mm");

                    ShowMessage("Quiz submitted successfully! Your score: " + score + " / 2. Course marked as Completed for Admin and Instructor review.", "alert-success");
                }
                else
                {
                    ShowMessage("Preview Quiz evaluation: Score = " + score + " / 2. (Logged in as " + UserRole + ")", "alert-info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error grading quiz: " + ex.Message, "alert-danger");
            }
        }

        protected void btnEnrollNow_Click(object sender, EventArgs e)
        {
            if (UserRole != "Student" || CurrentUserId == 0)
            {
                Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("Please log in as a student to enroll."));
                return;
            }

            try
            {
                string insertSql = @"
                    IF NOT EXISTS (SELECT 1 FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId)
                    BEGIN
                        INSERT INTO Enrollments (UserId, CourseId, EnrollmentDate, Status, IsCompleted)
                        VALUES (@UserId, @CourseId, GETDATE(), 'Enrolled', 0);
                    END";

                var insertParams = new[]
                {
                    new SqlParameter("@UserId", CurrentUserId),
                    new SqlParameter("@CourseId", _courseId)
                };

                DBHelper.ExecuteNonQuery(insertSql, insertParams);

                ShowMessage("You have successfully enrolled in this course! All study materials and the course quiz are now unlocked.", "alert-success");
                VerifyEnrollmentAndLoad(_courseId);
            }
            catch (Exception ex)
            {
                ShowMessage("Enrollment failed: " + ex.Message, "alert-danger");
            }
        }

        private void ShowMessage(string message, string cssClass)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert " + cssClass + " alert-dismissible fade show shadow-sm";
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
