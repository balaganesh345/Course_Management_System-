using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourseManagementSystem
{
    public partial class IndexPage : Page
    {
        private string UserRole => Session["UserRole"] != null ? Session["UserRole"].ToString() : "Admin";
        private int CurrentUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Default role is Admin if not explicitly set, or prompt login
            if (Session["UserRole"] == null)
            {
                // Default to Admin as requested: "in this project have the courses defualt itis set it to the Admin"
                Session["UserId"] = 1;
                Session["Username"] = "admin";
                Session["FullName"] = "Administrator";
                Session["UserRole"] = "Admin";
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["msg"] != null)
                {
                    ShowMessage(Request.QueryString["msg"], "alert-success");
                }

                ConfigureRoleView();
                LoadCategories();
                BindCoursesGrid();
            }
        }

        private void ConfigureRoleView()
        {
            if (UserRole == "Admin")
            {
                phAdminActions.Visible = true;
                phStudentActions.Visible = false;
                litSubHeader.Text = "Administrative Course Management &mdash; Create, update, delete, and view courses.";
            }
            else
            {
                phAdminActions.Visible = false;
                phStudentActions.Visible = true;
                divStatusFilter.Visible = false;
                string studentName = Session["FullName"] != null ? Session["FullName"].ToString() : "Student";
                litSubHeader.Text = "Welcome, " + Server.HtmlEncode(studentName) + "! Select any active course below to enroll.";
            }
        }

        private void LoadCategories()
        {
            try
            {
                string sql = "SELECT DISTINCT Category FROM Courses WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' ORDER BY Category ASC";
                DataTable dt = DBHelper.ExecuteSelect(sql);

                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new ListItem("All Categories", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string cat = row["Category"].ToString();
                    ddlCategory.Items.Add(new ListItem(cat, cat));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to load course categories: " + ex.Message, "alert-danger");
            }
        }

        private void BindCoursesGrid()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                string sql = @"
                    SELECT c.CourseId, c.CourseName, c.CourseCode, c.Description, c.InstructorName, 
                           c.Category, c.DurationInWeeks, c.CourseFee, c.StartDate, c.EndDate, c.IsActive, c.CreatedAt,
                           CASE WHEN EXISTS (
                               SELECT 1 FROM Enrollments e WHERE e.UserId = @CurrentUserId AND e.CourseId = c.CourseId
                           ) THEN 1 ELSE 0 END AS IsEnrolled
                    FROM Courses c
                    WHERE 1=1";

                parameters.Add(new SqlParameter("@CurrentUserId", CurrentUserId));

                string search = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (c.CourseName LIKE @Search OR c.CourseCode LIKE @Search OR c.InstructorName LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
                }

                string category = ddlCategory.SelectedValue;
                if (!string.IsNullOrEmpty(category))
                {
                    sql += " AND c.Category = @Category";
                    parameters.Add(new SqlParameter("@Category", category));
                }

                if (UserRole == "Admin")
                {
                    string status = ddlStatus.SelectedValue;
                    if (!string.IsNullOrEmpty(status))
                    {
                        sql += " AND c.IsActive = @IsActive";
                        parameters.Add(new SqlParameter("@IsActive", status == "1"));
                    }
                }
                else
                {
                    // Students only see active courses
                    sql += " AND c.IsActive = 1";
                }

                string sort = ddlSort.SelectedValue;
                switch (sort)
                {
                    case "NameDesc":
                        sql += " ORDER BY c.CourseName DESC";
                        break;
                    case "FeeAsc":
                        sql += " ORDER BY c.CourseFee ASC";
                        break;
                    case "FeeDesc":
                        sql += " ORDER BY c.CourseFee DESC";
                        break;
                    case "DateAsc":
                        sql += " ORDER BY c.StartDate ASC";
                        break;
                    case "DateDesc":
                        sql += " ORDER BY c.StartDate DESC";
                        break;
                    case "NameAsc":
                    default:
                        sql += " ORDER BY c.CourseName ASC";
                        break;
                }

                DataTable dt = DBHelper.ExecuteSelect(sql, parameters.ToArray());
                gvCourses.DataSource = dt;
                gvCourses.DataBind();

                litCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to query course database: " + ex.Message, "alert-danger");
            }
        }

        protected void gvCourses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var phAdmin = e.Row.FindControl("phAdminGridActions") as PlaceHolder;
                var phStudent = e.Row.FindControl("phStudentGridActions") as PlaceHolder;

                if (phAdmin != null) phAdmin.Visible = (UserRole == "Admin");
                if (phStudent != null) phStudent.Visible = (UserRole == "Student");
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                var phEmptyCreate = e.Row.FindControl("phEmptyCreate") as PlaceHolder;
                if (phEmptyCreate != null) phEmptyCreate.Visible = (UserRole == "Admin");
            }
        }

        protected void gvCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EnrollCourse")
            {
                if (UserRole != "Student" || CurrentUserId == 0)
                {
                    Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("Please log in as a student to enroll in courses."));
                    return;
                }

                int courseId = Convert.ToInt32(e.CommandArgument);

                try
                {
                    // Check if already enrolled
                    string checkSql = "SELECT COUNT(1) FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId";
                    var checkParams = new[]
                    {
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", courseId)
                    };
                    int count = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));

                    if (count > 0)
                    {
                        ShowMessage("You are already enrolled in this course.", "alert-info");
                        return;
                    }

                    // Insert enrollment
                    string insertSql = @"
                        INSERT INTO Enrollments (UserId, CourseId, EnrollmentDate, Status)
                        VALUES (@UserId, @CourseId, GETDATE(), 'Enrolled');";

                    var insertParams = new[]
                    {
                        new SqlParameter("@UserId", CurrentUserId),
                        new SqlParameter("@CourseId", courseId)
                    };

                    DBHelper.ExecuteNonQuery(insertSql, insertParams);

                    ShowMessage("Congratulations! You have successfully enrolled in this course.", "alert-success");
                    BindCoursesGrid();
                }
                catch (Exception ex)
                {
                    ShowMessage("Enrollment error: " + ex.Message, "alert-danger");
                }
            }
        }

        protected void gvCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCourses.PageIndex = e.NewPageIndex;
            BindCoursesGrid();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvCourses.PageIndex = 0;
            BindCoursesGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCategory.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlSort.SelectedIndex = 0;
            gvCourses.PageIndex = 0;
            BindCoursesGrid();
        }

        private void ShowMessage(string message, string cssClass)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert " + cssClass + " alert-dismissible fade show shadow-sm";
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
