using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class EnrollmentsPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Instructor"))
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Instructor or Admin privileges required."));
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseFilterOptions();

                if (Request.QueryString["courseId"] != null && int.TryParse(Request.QueryString["courseId"], out int cId))
                {
                    if (ddlCourseFilter.Items.FindByValue(cId.ToString()) != null)
                    {
                        ddlCourseFilter.SelectedValue = cId.ToString();
                    }
                }

                LoadStatistics();
                BindAllEnrollments();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                string statsSql = @"
                    SELECT 
                        COUNT(1) AS TotalCount,
                        ISNULL(SUM(CASE WHEN IsCompleted = 1 THEN 1 ELSE 0 END), 0) AS CompletedCount,
                        ISNULL(SUM(CASE WHEN IsCompleted = 0 OR IsCompleted IS NULL THEN 1 ELSE 0 END), 0) AS InProgressCount
                    FROM Enrollments";

                DataTable dtStats = DBHelper.ExecuteSelect(statsSql);
                if (dtStats.Rows.Count > 0)
                {
                    int total = Convert.ToInt32(dtStats.Rows[0]["TotalCount"]);
                    int completed = Convert.ToInt32(dtStats.Rows[0]["CompletedCount"]);
                    int inProgress = Convert.ToInt32(dtStats.Rows[0]["InProgressCount"]);

                    litTotalCount.Text = total.ToString();
                    litCompletedCount.Text = completed.ToString();
                    litInProgressCount.Text = inProgress.ToString();

                    double rate = total > 0 ? ((double)completed / total) * 100.0 : 0.0;
                    litCompletionRate.Text = rate.ToString("F0") + "%";
                }
            }
            catch (Exception)
            {
                // Fallback to defaults
            }
        }

        private void LoadCourseFilterOptions()
        {
            try
            {
                string sql = "SELECT CourseId, CourseCode, CourseName FROM Courses ORDER BY CourseName ASC";
                DataTable dt = DBHelper.ExecuteSelect(sql);

                ddlCourseFilter.Items.Clear();
                ddlCourseFilter.Items.Add(new ListItem("All Courses", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string text = row["CourseCode"].ToString() + " - " + row["CourseName"].ToString();
                    string value = row["CourseId"].ToString();
                    ddlCourseFilter.Items.Add(new ListItem(text, value));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading course list: " + ex.Message, "alert-danger");
            }
        }

        private void BindAllEnrollments()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                string sql = @"
                    SELECT e.EnrollmentId, e.UserId, e.CourseId, e.EnrollmentDate, e.Status,
                           e.IsCompleted, e.CompletionDate, e.QuizScore, e.QuizTotal,
                           u.Username, u.FullName, u.Email,
                           c.CourseName, c.CourseCode, c.InstructorName, c.Category, c.CourseFee
                    FROM Enrollments e
                    INNER JOIN Users u ON e.UserId = u.UserId
                    INNER JOIN Courses c ON e.CourseId = c.CourseId
                    WHERE 1=1";

                string search = txtStudentSearch.Text.Trim();
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (u.FullName LIKE @Search OR u.Username LIKE @Search OR u.Email LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
                }

                string courseFilter = ddlCourseFilter.SelectedValue;
                if (!string.IsNullOrEmpty(courseFilter))
                {
                    sql += " AND e.CourseId = @CourseId";
                    parameters.Add(new SqlParameter("@CourseId", int.Parse(courseFilter)));
                }

                string completionFilter = ddlCompletionFilter.SelectedValue;
                if (completionFilter == "Completed")
                {
                    sql += " AND e.IsCompleted = 1";
                }
                else if (completionFilter == "InProgress")
                {
                    sql += " AND (e.IsCompleted = 0 OR e.IsCompleted IS NULL)";
                }

                sql += " ORDER BY e.EnrollmentDate DESC";

                DataTable dt = DBHelper.ExecuteSelect(sql, parameters.ToArray());
                gvAllEnrollments.DataSource = dt;
                gvAllEnrollments.DataBind();
                litCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Error retrieving student enrollments: " + ex.Message, "alert-danger");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvAllEnrollments.PageIndex = 0;
            LoadStatistics();
            BindAllEnrollments();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtStudentSearch.Text = string.Empty;
            ddlCourseFilter.SelectedIndex = 0;
            if (ddlCompletionFilter != null) ddlCompletionFilter.SelectedIndex = 0;
            gvAllEnrollments.PageIndex = 0;
            LoadStatistics();
            BindAllEnrollments();
        }

        protected void gvAllEnrollments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllEnrollments.PageIndex = e.NewPageIndex;
            BindAllEnrollments();
        }

        private void ShowMessage(string message, string cssClass)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert " + cssClass + " alert-dismissible fade show shadow-sm";
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
