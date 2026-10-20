using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class CreatePage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Admin privileges required to add courses."));
                return;
            }

            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.AddDays(60).ToString("yyyy-MM-dd");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                string courseCode = txtCourseCode.Text.Trim().ToUpper();
                string courseName = txtCourseName.Text.Trim();
                string category = txtCategory.Text.Trim();
                string instructor = txtInstructor.Text.Trim();
                string description = txtDescription.Text.Trim();

                int duration = int.Parse(txtDuration.Text.Trim());
                decimal fee = decimal.Parse(txtFee.Text.Trim());
                DateTime startDate = DateTime.Parse(txtStartDate.Text.Trim());
                DateTime endDate = DateTime.Parse(txtEndDate.Text.Trim());
                bool isActive = chkIsActive.Checked;

                if (endDate <= startDate)
                {
                    ShowError("End Date must be strictly after the Start Date.");
                    return;
                }

                // Check for duplicate course code
                string checkSql = "SELECT COUNT(1) FROM Courses WHERE CourseCode = @CourseCode";
                var checkParams = new[] { new SqlParameter("@CourseCode", courseCode) };
                int existingCount = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));

                if (existingCount > 0)
                {
                    ShowError("A course with code '" + courseCode + "' already exists. Course codes must be unique.");
                    return;
                }

                // Insert into database via ADO.NET
                string insertSql = @"
                    INSERT INTO Courses 
                        (CourseName, CourseCode, Description, InstructorName, Category, DurationInWeeks, CourseFee, StartDate, EndDate, IsActive, CreatedAt)
                    VALUES 
                        (@CourseName, @CourseCode, @Description, @InstructorName, @Category, @DurationInWeeks, @CourseFee, @StartDate, @EndDate, @IsActive, GETDATE())";

                var insertParams = new[]
                {
                    new SqlParameter("@CourseName", courseName),
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                    new SqlParameter("@InstructorName", instructor),
                    new SqlParameter("@Category", category),
                    new SqlParameter("@DurationInWeeks", duration),
                    new SqlParameter("@CourseFee", fee),
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate),
                    new SqlParameter("@IsActive", isActive)
                };

                int rows = DBHelper.ExecuteNonQuery(insertSql, insertParams);

                if (rows > 0)
                {
                    Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Course '" + courseName + "' created successfully!"));
                }
                else
                {
                    ShowError("Failed to save the course record into the database.");
                }
            }
            catch (Exception ex)
            {
                ShowError("An unexpected error occurred: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
        }
    }
}
