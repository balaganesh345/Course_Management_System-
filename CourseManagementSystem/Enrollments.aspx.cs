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
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Access Denied: Admin privileges required."));
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

                BindAllEnrollments();
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
            BindAllEnrollments();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtStudentSearch.Text = string.Empty;
            ddlCourseFilter.SelectedIndex = 0;
            gvAllEnrollments.PageIndex = 0;
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
