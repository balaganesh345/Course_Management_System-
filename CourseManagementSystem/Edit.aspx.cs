using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class EditPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Admin privileges required to edit courses."));
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out int courseId))
                {
                    Response.Redirect("Index.aspx");
                    return;
                }

                hfCourseId.Value = courseId.ToString();
                lnkDetails.NavigateUrl = "Details.aspx?id=" + courseId;
                LoadCourseDetails(courseId);
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
                txtCourseName.Text = row["CourseName"].ToString();
                txtCourseCode.Text = row["CourseCode"].ToString();
                txtCategory.Text = row["Category"].ToString();
                txtInstructor.Text = row["InstructorName"].ToString();
                txtDuration.Text = row["DurationInWeeks"].ToString();
                txtFee.Text = Convert.ToDecimal(row["CourseFee"]).ToString("F2");
                txtStartDate.Text = Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd");
                txtEndDate.Text = Convert.ToDateTime(row["EndDate"]).ToString("yyyy-MM-dd");
                txtDescription.Text = row["Description"] != DBNull.Value ? row["Description"].ToString() : string.Empty;
                chkIsActive.Checked = Convert.ToBoolean(row["IsActive"]);
            }
            catch (Exception ex)
            {
                ShowError("Error loading course details: " + ex.Message);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                int courseId = int.Parse(hfCourseId.Value);
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

                // Ensure unique CourseCode among other records
                string checkSql = "SELECT COUNT(1) FROM Courses WHERE CourseCode = @CourseCode AND CourseId <> @CourseId";
                var checkParams = new[] 
                { 
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@CourseId", courseId)
                };
                int existingCount = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));

                if (existingCount > 0)
                {
                    ShowError("Another course is already registered with code '" + courseCode + "'.");
                    return;
                }

                // Update via ADO.NET
                string updateSql = @"
                    UPDATE Courses 
                    SET 
                        CourseName = @CourseName,
                        CourseCode = @CourseCode,
                        Description = @Description,
                        InstructorName = @InstructorName,
                        Category = @Category,
                        DurationInWeeks = @DurationInWeeks,
                        CourseFee = @CourseFee,
                        StartDate = @StartDate,
                        EndDate = @EndDate,
                        IsActive = @IsActive
                    WHERE CourseId = @CourseId";

                var updateParams = new[]
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
                    new SqlParameter("@IsActive", isActive),
                    new SqlParameter("@CourseId", courseId)
                };

                int rows = DBHelper.ExecuteNonQuery(updateSql, updateParams);

                if (rows > 0)
                {
                    Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Course '" + courseName + "' updated successfully!"));
                }
                else
                {
                    ShowError("Course record could not be updated or was not found.");
                }
            }
            catch (Exception ex)
            {
                ShowError("An unexpected database error occurred: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
        }
    }
}
