using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class DetailsPage : Page
    {
        private int _courseId;
        private string UserRole => Session["UserRole"] != null ? Session["UserRole"].ToString() : "Admin";
        private int CurrentUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

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

                if (UserRole == "Admin")
                {
                    phAdminDetailsActions.Visible = true;
                    phStudentDetailsActions.Visible = false;
                    lnkEdit.NavigateUrl = "Edit.aspx?id=" + courseId;
                    lnkDelete.NavigateUrl = "Delete.aspx?id=" + courseId;

                    // Load enrolled students list for this course
                    phAdminEnrolledStudents.Visible = true;
                    string studentsSql = @"
                        SELECT e.EnrollmentId, e.EnrollmentDate, e.Status,
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

                    // Check if current student is already enrolled
                    string checkSql = "SELECT COUNT(1) FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId";
                    var checkParams = new[]
                    {
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", courseId)
                    };
                    int isEnrolled = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));

                    if (isEnrolled > 0)
                    {
                        btnEnrollDetails.Visible = false;
                        lblEnrolledBadge.Visible = true;
                    }
                    else
                    {
                        btnEnrollDetails.Visible = isActive;
                        lblEnrolledBadge.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                lblMessage.Text = "Error retrieving course details: " + ex.Message;
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
                    INSERT INTO Enrollments (UserId, CourseId, EnrollmentDate, Status)
                    VALUES (@UserId, @CourseId, GETDATE(), 'Enrolled');";

                var insertParams = new[]
                {
                    new SqlParameter("@UserId", CurrentUserId),
                    new SqlParameter("@CourseId", _courseId)
                };

                DBHelper.ExecuteNonQuery(insertSql, insertParams);

                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-success";
                lblMessage.Text = "Successfully enrolled in this course!";

                btnEnrollDetails.Visible = false;
                lblEnrolledBadge.Visible = true;
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger";
                lblMessage.Text = "Enrollment error: " + ex.Message;
            }
        }
    }
}
