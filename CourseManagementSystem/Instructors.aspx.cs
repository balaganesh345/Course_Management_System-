using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class InstructorsPage : Page
    {
        private string UserRole => Session["UserRole"] != null ? Session["UserRole"].ToString() : "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only Administrator can access the full instructors directory
            if (UserRole != "Admin")
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Only Administrators can view the instructors directory."));
                return;
            }

            if (!IsPostBack)
            {
                LoadKPIs();
                BindInstructorsGrid();
            }
        }

        private void LoadKPIs()
        {
            try
            {
                // Total Instructors
                string sqlInstructors = "SELECT COUNT(1) FROM Users WHERE Role = 'Instructor'";
                int totalInstructors = Convert.ToInt32(DBHelper.ExecuteScalar(sqlInstructors));
                litTotalInstructors.Text = totalInstructors.ToString();

                // Total Assigned Courses & Students & Completions across instructor courses
                string sqlMetrics = @"
                    SELECT 
                        COUNT(DISTINCT c.CourseId) AS TotalCourses,
                        COUNT(e.EnrollmentId) AS TotalStudents,
                        SUM(CASE WHEN e.IsCompleted = 1 THEN 1 ELSE 0 END) AS TotalCompletions
                    FROM Courses c
                    INNER JOIN Users u ON (LOWER(LTRIM(RTRIM(c.InstructorName))) = LOWER(LTRIM(RTRIM(u.FullName))) 
                                        OR LOWER(LTRIM(RTRIM(c.InstructorName))) = LOWER(LTRIM(RTRIM(u.Username))))
                    LEFT JOIN Enrollments e ON c.CourseId = e.CourseId
                    WHERE u.Role = 'Instructor'";

                DataTable dtMetrics = DBHelper.ExecuteSelect(sqlMetrics);
                if (dtMetrics.Rows.Count > 0)
                {
                    DataRow r = dtMetrics.Rows[0];
                    litTotalAssignedCourses.Text = r["TotalCourses"] != DBNull.Value ? r["TotalCourses"].ToString() : "0";
                    litTotalStudents.Text = r["TotalStudents"] != DBNull.Value ? r["TotalStudents"].ToString() : "0";
                    litTotalCompletions.Text = r["TotalCompletions"] != DBNull.Value ? r["TotalCompletions"].ToString() : "0";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading overview metrics: " + ex.Message, "alert-danger");
            }
        }

        private void BindInstructorsGrid()
        {
            try
            {
                string search = txtSearch.Text.Trim();

                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        u.UserId,
                        u.Username,
                        u.FullName,
                        u.Email,
                        u.CreatedAt,
                        ISNULL(stats.TotalCourses, 0) AS TotalCourses,
                        ISNULL(stats.ActiveCourses, 0) AS ActiveCourses,
                        ISNULL(stats.TotalStudents, 0) AS TotalStudents,
                        ISNULL(stats.TotalCompletions, 0) AS TotalCompletions
                    FROM Users u
                    OUTER APPLY (
                        SELECT 
                            COUNT(DISTINCT c.CourseId) AS TotalCourses,
                            COUNT(DISTINCT CASE WHEN c.IsActive = 1 THEN c.CourseId END) AS ActiveCourses,
                            COUNT(e.EnrollmentId) AS TotalStudents,
                            SUM(CASE WHEN e.IsCompleted = 1 THEN 1 ELSE 0 END) AS TotalCompletions
                        FROM Courses c
                        LEFT JOIN Enrollments e ON c.CourseId = e.CourseId
                        WHERE LOWER(LTRIM(RTRIM(c.InstructorName))) = LOWER(LTRIM(RTRIM(u.FullName)))
                           OR LOWER(LTRIM(RTRIM(c.InstructorName))) = LOWER(LTRIM(RTRIM(u.Username)))
                    ) stats
                    WHERE u.Role = 'Instructor'");

                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(search))
                {
                    sql.Append(@" AND (
                        u.FullName LIKE @Search OR 
                        u.Username LIKE @Search OR 
                        u.Email LIKE @Search OR
                        EXISTS (
                            SELECT 1 FROM Courses c2 
                            WHERE (LOWER(LTRIM(RTRIM(c2.InstructorName))) = LOWER(LTRIM(RTRIM(u.FullName))) 
                                OR LOWER(LTRIM(RTRIM(c2.InstructorName))) = LOWER(LTRIM(RTRIM(u.Username))))
                              AND (c2.CourseName LIKE @Search OR c2.CourseCode LIKE @Search)
                        )
                    )");
                    parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
                }

                sql.Append(" ORDER BY u.FullName ASC");

                DataTable dt = DBHelper.ExecuteSelect(sql.ToString(), parameters.ToArray());
                gvInstructors.DataSource = dt;
                gvInstructors.DataBind();

                litCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading instructors list: " + ex.Message, "alert-danger");
            }
        }

        protected void gvInstructors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                string fullName = rowView["FullName"].ToString();
                string username = rowView["Username"].ToString();

                PlaceHolder phCoursesTaught = (PlaceHolder)e.Row.FindControl("phCoursesTaught");
                if (phCoursesTaught != null)
                {
                    // Fetch courses taught by this instructor
                    string sql = @"
                        SELECT CourseId, CourseCode, CourseName, IsActive 
                        FROM Courses 
                        WHERE LOWER(LTRIM(RTRIM(InstructorName))) = LOWER(LTRIM(RTRIM(@FullName)))
                           OR LOWER(LTRIM(RTRIM(InstructorName))) = LOWER(LTRIM(RTRIM(@Username)))
                        ORDER BY CourseCode ASC";

                    var parameters = new[]
                    {
                        new SqlParameter("@FullName", fullName),
                        new SqlParameter("@Username", username)
                    };

                    DataTable dtCourses = DBHelper.ExecuteSelect(sql, parameters);
                    if (dtCourses.Rows.Count > 0)
                    {
                        var sb = new StringBuilder();
                        sb.Append("<div class='d-flex flex-wrap gap-1'>");
                        foreach (DataRow cr in dtCourses.Rows)
                        {
                            int courseId = Convert.ToInt32(cr["CourseId"]);
                            string code = cr["CourseCode"].ToString();
                            string name = cr["CourseName"].ToString();
                            bool isActive = Convert.ToBoolean(cr["IsActive"]);

                            string activeIcon = isActive ? "text-success" : "text-muted";
                            sb.AppendFormat("<a href='Details.aspx?id={0}' class='course-pill' title='{1} - {2} ({3})'>",
                                courseId,
                                Server.HtmlEncode(code),
                                Server.HtmlEncode(name),
                                isActive ? "Active" : "Inactive");
                            sb.AppendFormat("<i class='bi bi-circle-fill {0}' style='font-size:0.5rem;'></i> ", activeIcon);
                            sb.AppendFormat("<strong>{0}</strong>: {1}</a>",
                                Server.HtmlEncode(code),
                                Server.HtmlEncode(name.Length > 20 ? name.Substring(0, 20) + "..." : name));
                        }
                        sb.Append("</div>");
                        phCoursesTaught.Controls.Add(new LiteralControl(sb.ToString()));
                    }
                    else
                    {
                        phCoursesTaught.Controls.Add(new LiteralControl("<span class='text-muted small fst-italic'>No assigned courses yet</span>"));
                    }
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindInstructorsGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindInstructorsGrid();
        }

        public string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "IN";
            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = "alert " + cssClass + " alert-dismissible fade show shadow-sm";
            pnlMessage.Visible = true;
        }
    }
}
