using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class LoginPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["msg"] != null)
                {
                    pnlMessage.Visible = true;
                    pnlMessage.CssClass = "alert alert-success py-2 small";
                    lblMessage.Text = Server.HtmlEncode(Request.QueryString["msg"]);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string selectedRole = rblRole.SelectedValue;

            try
            {
                string sql = "SELECT UserId, Username, FullName, Email, Role FROM Users WHERE Username = @Username AND Password = @Password AND Role = @Role";
                var parameters = new[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password),
                    new SqlParameter("@Role", selectedRole)
                };

                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Session["UserId"] = Convert.ToInt32(row["UserId"]);
                    Session["Username"] = row["Username"].ToString();
                    Session["FullName"] = row["FullName"].ToString();
                    Session["UserRole"] = row["Role"].ToString();

                    if (selectedRole == "Admin")
                    {
                        Response.Redirect("Index.aspx");
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
                else
                {
                    pnlMessage.Visible = true;
                    pnlMessage.CssClass = "alert alert-danger py-2 small";
                    lblMessage.Text = "Invalid username, password, or role selection. Please check your credentials.";
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger py-2 small";
                lblMessage.Text = "Error during login: " + ex.Message;
            }
        }

        protected void btnDemoAdmin_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "admin";
            txtPassword.Text = "admin123";
            rblRole.SelectedValue = "Admin";
            pnlMessage.Visible = false;
        }

        protected void btnDemoStudent_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "student";
            txtPassword.Text = "student123";
            rblRole.SelectedValue = "Student";
            pnlMessage.Visible = false;
        }
    }
}
