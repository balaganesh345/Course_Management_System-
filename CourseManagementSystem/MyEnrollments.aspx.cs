using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class MyEnrollmentsPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Student")
            {
                Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("Please log in as a student to view your enrollments."));
                return;
            }

            if (!IsPostBack)
            {
                BindEnrollments();
            }
        }

        private void BindEnrollments()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                string sql = @"
                    SELECT e.EnrollmentId, e.UserId, e.CourseId, e.EnrollmentDate, e.Status,
                           c.CourseName, c.CourseCode, c.InstructorName, c.Category, 
                           c.DurationInWeeks, c.CourseFee, c.StartDate, c.EndDate
                    FROM Enrollments e
                    INNER JOIN Courses c ON e.CourseId = c.CourseId
                    WHERE e.UserId = @UserId
                    ORDER BY e.EnrollmentDate DESC";

                var parameters = new[] { new SqlParameter("@UserId", userId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                gvEnrollments.DataSource = dt;
                gvEnrollments.DataBind();
                litCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to load your enrolled courses: " + ex.Message, "alert-danger");
            }
        }

        protected void gvEnrollments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DropCourse")
            {
                int enrollmentId = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(Session["UserId"]);

                try
                {
                    string sql = "DELETE FROM Enrollments WHERE EnrollmentId = @EnrollmentId AND UserId = @UserId";
                    var parameters = new[]
                    {
                        new SqlParameter("@EnrollmentId", enrollmentId),
                        new SqlParameter("@UserId", userId)
                    };

                    int rows = DBHelper.ExecuteNonQuery(sql, parameters);
                    if (rows > 0)
                    {
                        ShowMessage("Successfully dropped the course enrollment.", "alert-success");
                        BindEnrollments();
                    }
                    else
                    {
                        ShowMessage("Enrollment could not be found or dropped.", "alert-warning");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error dropping course: " + ex.Message, "alert-danger");
                }
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
