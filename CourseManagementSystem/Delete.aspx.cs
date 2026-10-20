using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class DeletePage : Page
    {
        private int _courseId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Admin privileges required to delete courses."));
                return;
            }

            if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out _courseId))
            {
                Response.Redirect("Index.aspx");
                return;
            }

            hfCourseId.Value = _courseId.ToString();

            if (!IsPostBack)
            {
                LoadCourse(_courseId);
            }
        }

        private void LoadCourse(int courseId)
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
                litFee.Text = string.Format("{0:C2}", row["CourseFee"]);
                litDates.Text = Convert.ToDateTime(row["StartDate"]).ToString("MMM dd, yyyy") + " to " +
                               Convert.ToDateTime(row["EndDate"]).ToString("MMM dd, yyyy");
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                lblMessage.Text = "Error retrieving course record: " + ex.Message;
            }
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfCourseId.Value);
                string sql = "DELETE FROM Courses WHERE CourseId = @CourseId";
                var parameters = new[] { new SqlParameter("@CourseId", id) };

                int rows = DBHelper.ExecuteNonQuery(sql, parameters);

                if (rows > 0)
                {
                    Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Course was successfully removed."));
                }
                else
                {
                    pnlMessage.Visible = true;
                    lblMessage.Text = "Course could not be found or was already deleted.";
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                lblMessage.Text = "Error during deletion: " + ex.Message;
            }
        }
    }
}
