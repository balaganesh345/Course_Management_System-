using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class DetailsPage : Page
    {
        protected int _courseId;
        protected string UserRole => Session["UserRole"] != null ? Session["UserRole"].ToString() : "Admin";
        protected int CurrentUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out _courseId))
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseDetails(_courseId);
                LoadMaterials(_courseId);
                LoadQuizManagement(_courseId);
            }
        }

        private void LoadCourseDetails(int courseId)
        {
            try
            {
                string sql = "SELECT * FROM Courses WHERE CourseId = @CourseId";
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
                litFee.Text = string.Format("{0:C2}", row["CourseFee"]);
                litStartDate.Text = Convert.ToDateTime(row["StartDate"]).ToString("MMMM dd, yyyy");
                litEndDate.Text = Convert.ToDateTime(row["EndDate"]).ToString("MMMM dd, yyyy");
                litCreatedAt.Text = Convert.ToDateTime(row["CreatedAt"]).ToString("MMM dd, yyyy HH:mm");

                string desc = row["Description"] != DBNull.Value ? row["Description"].ToString() : "";
                litDescription.Text = string.IsNullOrWhiteSpace(desc) 
                    ? "<em class='text-muted'>No description provided for this course.</em>" 
                    : Server.HtmlEncode(desc).Replace("\n", "<br/>");

                bool isActive = Convert.ToBoolean(row["IsActive"]);
                litStatusBadge.Text = isActive 
                    ? "<span class='badge-status-active'><i class='bi bi-check-circle-fill me-1'></i>Active</span>" 
                    : "<span class='badge-status-inactive'><i class='bi bi-x-circle-fill me-1'></i>Inactive</span>";

                if (UserRole == "Admin" || UserRole == "Instructor")
                {
                    phAdminDetailsActions.Visible = true;
                    phStudentDetailsActions.Visible = false;
                    lnkEdit.NavigateUrl = "Edit.aspx?id=" + courseId;
                    lnkDelete.NavigateUrl = "Delete.aspx?id=" + courseId;
                    lnkStudyPreview.NavigateUrl = "CourseStudy.aspx?id=" + courseId;

                    phAddMaterialAdmin.Visible = true;
                    phNotEnrolledNotice.Visible = false;
                    phQuizManagement.Visible = true;
                    phStudentQuizLink.Visible = false;

                    // Load enrolled students & completion list for this course
                    phAdminEnrolledStudents.Visible = true;
                    string studentsSql = @"
                        SELECT e.EnrollmentId, e.EnrollmentDate, e.Status, e.IsCompleted, e.CompletionDate, e.QuizScore,
                               u.FullName, u.Username, u.Email
                        FROM Enrollments e
                        INNER JOIN Users u ON e.UserId = u.UserId
                        WHERE e.CourseId = @CourseId
                        ORDER BY e.EnrollmentDate DESC";
                    var studentParams = new[] { new SqlParameter("@CourseId", courseId) };
                    DataTable dtStudents = DBHelper.ExecuteSelect(studentsSql, studentParams);
                    gvCourseStudents.DataSource = dtStudents;
                    gvCourseStudents.DataBind();
                    litCourseEnrolledCount.Text = dtStudents.Rows.Count.ToString();
                }
                else
                {
                    phAdminDetailsActions.Visible = false;
                    phStudentDetailsActions.Visible = true;
                    phAdminEnrolledStudents.Visible = false;
                    phAddMaterialAdmin.Visible = false;
                    phQuizManagement.Visible = false;

                    // Check if current student is already enrolled
                    string checkSql = "SELECT EnrollmentId, IsCompleted, QuizScore, CompletionDate FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId";
                    var checkParams = new[]
                    {
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", courseId)
                    };
                    DataTable dtEnroll = DBHelper.ExecuteSelect(checkSql, checkParams);

                    if (dtEnroll.Rows.Count > 0)
                    {
                        btnEnrollDetails.Visible = false;
                        lblEnrolledBadge.Visible = true;
                        lnkStudentStudy.Visible = true;
                        lnkStudentStudy.NavigateUrl = "CourseStudy.aspx?id=" + courseId;

                        phNotEnrolledNotice.Visible = false;
                        phStudentQuizLink.Visible = true;

                        DataRow enrollRow = dtEnroll.Rows[0];
                        bool isDone = enrollRow["IsCompleted"] != DBNull.Value && Convert.ToBoolean(enrollRow["IsCompleted"]);
                        if (isDone)
                        {
                            litStudentQuizStatus.Text = "<span class='badge bg-success me-1'><i class='bi bi-check-circle-fill me-1'></i>Course Completed!</span> Quiz Score: " + enrollRow["QuizScore"] + " / 2";
                        }
                        else
                        {
                            litStudentQuizStatus.Text = "<span class='badge bg-warning-subtle text-warning-emphasis me-1'><i class='bi bi-hourglass-split me-1'></i>Enrolled</span> Quiz pending completion";
                        }
                    }
                    else
                    {
                        btnEnrollDetails.Visible = isActive;
                        lblEnrolledBadge.Visible = false;
                        lnkStudentStudy.Visible = false;
                        phNotEnrolledNotice.Visible = true;
                        phStudentQuizLink.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error retrieving course details: " + ex.Message, "alert-danger");
            }
        }

        private void LoadMaterials(int courseId)
        {
            try
            {
                string sql = "SELECT * FROM CourseMaterials WHERE CourseId = @CourseId ORDER BY CreatedAt DESC";
                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                rptDetailsMaterials.DataSource = dt;
                rptDetailsMaterials.DataBind();
                lblNoMaterials.Visible = (dt.Rows.Count == 0);
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading course materials: " + ex.Message, "alert-danger");
            }
        }

        protected void rblAddMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblAddMaterialType.SelectedValue == "File")
            {
                divTextContent.Style["display"] = "none";
                divFileUpload.Style["display"] = "block";
            }
            else
            {
                divTextContent.Style["display"] = "block";
                divFileUpload.Style["display"] = "none";
            }
        }

        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (UserRole != "Admin" && UserRole != "Instructor") return;

            string title = txtAddMaterialTitle.Text.Trim();
            string type = rblAddMaterialType.SelectedValue;

            if (string.IsNullOrEmpty(title))
            {
                ShowMessage("Material title is required.", "alert-danger");
                return;
            }

            try
            {
                string uploader = Session["FullName"] != null ? Session["FullName"].ToString() : Session["Username"].ToString();

                if (type == "File")
                {
                    if (!fuAddMaterialFile.HasFile)
                    {
                        ShowMessage("Please choose a file to upload.", "alert-warning");
                        return;
                    }

                    string uploadDir = Server.MapPath("~/Uploads/Materials/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string originalFileName = Path.GetFileName(fuAddMaterialFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString("N") + "_" + originalFileName;
                    string targetFilePath = Path.Combine(uploadDir, uniqueFileName);
                    fuAddMaterialFile.SaveAs(targetFilePath);

                    string relativePath = "~/Uploads/Materials/" + uniqueFileName;
                    int fileSize = fuAddMaterialFile.PostedFile.ContentLength;

                    string insertSql = @"
                        INSERT INTO CourseMaterials (CourseId, MaterialType, Title, TextContent, FileName, FilePath, FileSize, UploadedBy, CreatedAt)
                        VALUES (@CourseId, 'File', @Title, NULL, @FileName, @FilePath, @FileSize, @UploadedBy, GETDATE())";

                    var parameters = new[]
                    {
                        new SqlParameter("@CourseId", _courseId),
                        new SqlParameter("@Title", title),
                        new SqlParameter("@FileName", originalFileName),
                        new SqlParameter("@FilePath", relativePath),
                        new SqlParameter("@FileSize", fileSize),
                        new SqlParameter("@UploadedBy", uploader)
                    };

                    DBHelper.ExecuteNonQuery(insertSql, parameters);
                }
                else
                {
                    string textContent = txtAddMaterialText.Text.Trim();
                    if (string.IsNullOrEmpty(textContent))
                    {
                        ShowMessage("Please enter the lesson / text content.", "alert-warning");
                        return;
                    }

                    string insertSql = @"
                        INSERT INTO CourseMaterials (CourseId, MaterialType, Title, TextContent, FileName, FilePath, FileSize, UploadedBy, CreatedAt)
                        VALUES (@CourseId, 'Text', @Title, @TextContent, NULL, NULL, NULL, @UploadedBy, GETDATE())";

                    var parameters = new[]
                    {
                        new SqlParameter("@CourseId", _courseId),
                        new SqlParameter("@Title", title),
                        new SqlParameter("@TextContent", textContent),
                        new SqlParameter("@UploadedBy", uploader)
                    };

                    DBHelper.ExecuteNonQuery(insertSql, parameters);
                }

                txtAddMaterialTitle.Text = string.Empty;
                txtAddMaterialText.Text = string.Empty;

                ShowMessage("Course material '" + title + "' added successfully!", "alert-success");
                LoadMaterials(_courseId);
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to save course material: " + ex.Message, "alert-danger");
            }
        }

        protected void rptDetailsMaterials_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteMaterial")
            {
                if (UserRole != "Admin" && UserRole != "Instructor") return;

                int materialId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    string deleteSql = "DELETE FROM CourseMaterials WHERE MaterialId = @MaterialId AND CourseId = @CourseId";
                    var parameters = new[]
                    {
                        new SqlParameter("@MaterialId", materialId),
                        new SqlParameter("@CourseId", _courseId)
                    };
                    DBHelper.ExecuteNonQuery(deleteSql, parameters);

                    ShowMessage("Course material removed successfully.", "alert-success");
                    LoadMaterials(_courseId);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error removing course material: " + ex.Message, "alert-danger");
                }
            }
        }

        private void LoadQuizManagement(int courseId)
        {
            if (UserRole != "Admin" && UserRole != "Instructor") return;

            try
            {
                string sql = "SELECT * FROM CourseQuizzes WHERE CourseId = @CourseId ORDER BY QuestionNumber ASC";
                var parameters = new[] { new SqlParameter("@CourseId", courseId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count >= 1)
                {
                    DataRow q1 = dt.Rows[0];
                    txtQuizQ1.Text = q1["QuestionText"].ToString();
                    txtQuizQ1A.Text = q1["OptionA"].ToString();
                    txtQuizQ1B.Text = q1["OptionB"].ToString();
                    txtQuizQ1C.Text = q1["OptionC"].ToString();
                    txtQuizQ1D.Text = q1["OptionD"].ToString();
                    ddlQuizQ1Ans.SelectedValue = q1["CorrectOption"].ToString().Trim();
                }

                if (dt.Rows.Count >= 2)
                {
                    DataRow q2 = dt.Rows[1];
                    txtQuizQ2.Text = q2["QuestionText"].ToString();
                    txtQuizQ2A.Text = q2["OptionA"].ToString();
                    txtQuizQ2B.Text = q2["OptionB"].ToString();
                    txtQuizQ2C.Text = q2["OptionC"].ToString();
                    txtQuizQ2D.Text = q2["OptionD"].ToString();
                    ddlQuizQ2Ans.SelectedValue = q2["CorrectOption"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading quiz configuration: " + ex.Message, "alert-danger");
            }
        }

        protected void btnSaveQuizSettings_Click(object sender, EventArgs e)
        {
            if (UserRole != "Admin" && UserRole != "Instructor") return;

            string q1Text = txtQuizQ1.Text.Trim();
            string q1A = txtQuizQ1A.Text.Trim();
            string q1B = txtQuizQ1B.Text.Trim();
            string q1C = txtQuizQ1C.Text.Trim();
            string q1D = txtQuizQ1D.Text.Trim();
            string q1Ans = ddlQuizQ1Ans.SelectedValue;

            string q2Text = txtQuizQ2.Text.Trim();
            string q2A = txtQuizQ2A.Text.Trim();
            string q2B = txtQuizQ2B.Text.Trim();
            string q2C = txtQuizQ2C.Text.Trim();
            string q2D = txtQuizQ2D.Text.Trim();
            string q2Ans = ddlQuizQ2Ans.SelectedValue;

            if (string.IsNullOrEmpty(q1Text) || string.IsNullOrEmpty(q2Text))
            {
                ShowMessage("Both Question 1 and Question 2 text must be provided.", "alert-danger");
                return;
            }

            try
            {
                // Upsert Question 1
                string upsertSql = @"
                    IF EXISTS (SELECT 1 FROM CourseQuizzes WHERE CourseId = @CourseId AND QuestionNumber = 1)
                    BEGIN
                        UPDATE CourseQuizzes 
                        SET QuestionText = @Q1Text, OptionA = @Q1A, OptionB = @Q1B, OptionC = @Q1C, OptionD = @Q1D, CorrectOption = @Q1Ans
                        WHERE CourseId = @CourseId AND QuestionNumber = 1;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                        VALUES (@CourseId, 1, @Q1Text, @Q1A, @Q1B, @Q1C, @Q1D, @Q1Ans);
                    END

                    IF EXISTS (SELECT 1 FROM CourseQuizzes WHERE CourseId = @CourseId AND QuestionNumber = 2)
                    BEGIN
                        UPDATE CourseQuizzes 
                        SET QuestionText = @Q2Text, OptionA = @Q2A, OptionB = @Q2B, OptionC = @Q2C, OptionD = @Q2D, CorrectOption = @Q2Ans
                        WHERE CourseId = @CourseId AND QuestionNumber = 2;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO CourseQuizzes (CourseId, QuestionNumber, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                        VALUES (@CourseId, 2, @Q2Text, @Q2A, @Q2B, @Q2C, @Q2D, @Q2Ans);
                    END";

                var parameters = new[]
                {
                    new SqlParameter("@CourseId", _courseId),
                    new SqlParameter("@Q1Text", q1Text),
                    new SqlParameter("@Q1A", string.IsNullOrEmpty(q1A) ? "Option A" : q1A),
                    new SqlParameter("@Q1B", string.IsNullOrEmpty(q1B) ? "Option B" : q1B),
                    new SqlParameter("@Q1C", string.IsNullOrEmpty(q1C) ? "Option C" : q1C),
                    new SqlParameter("@Q1D", string.IsNullOrEmpty(q1D) ? "Option D" : q1D),
                    new SqlParameter("@Q1Ans", q1Ans),
                    new SqlParameter("@Q2Text", q2Text),
                    new SqlParameter("@Q2A", string.IsNullOrEmpty(q2A) ? "Option A" : q2A),
                    new SqlParameter("@Q2B", string.IsNullOrEmpty(q2B) ? "Option B" : q2B),
                    new SqlParameter("@Q2C", string.IsNullOrEmpty(q2C) ? "Option C" : q2C),
                    new SqlParameter("@Q2D", string.IsNullOrEmpty(q2D) ? "Option D" : q2D),
                    new SqlParameter("@Q2Ans", q2Ans)
                };

                DBHelper.ExecuteNonQuery(upsertSql, parameters);
                ShowMessage("2-Question Quiz updated successfully for this course!", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating quiz: " + ex.Message, "alert-danger");
            }
        }

        protected void btnEnrollDetails_Click(object sender, EventArgs e)
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

                ShowMessage("Successfully enrolled in this course! Learning materials and the 2-question quiz are now unlocked.", "alert-success");

                btnEnrollDetails.Visible = false;
                lblEnrolledBadge.Visible = true;
                lnkStudentStudy.Visible = true;
                lnkStudentStudy.NavigateUrl = "CourseStudy.aspx?id=" + _courseId;
                phNotEnrolledNotice.Visible = false;
                phStudentQuizLink.Visible = true;
                litStudentQuizStatus.Text = "<span class='badge bg-warning-subtle text-warning-emphasis me-1'><i class='bi bi-hourglass-split me-1'></i>Enrolled</span> Quiz pending completion";

                LoadMaterials(_courseId);
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
