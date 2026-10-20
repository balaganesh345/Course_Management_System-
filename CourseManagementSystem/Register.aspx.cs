using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class RegisterPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                // Check if username already exists
                string checkSql = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
                var checkParams = new[] { new SqlParameter("@Username", username) };
                int exists = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));

                if (exists > 0)
                {
                    pnlMessage.Visible = true;
                    pnlMessage.CssClass = "alert alert-danger py-2 small";
                    lblMessage.Text = "Username '" + Server.HtmlEncode(username) + "' is already taken. Please choose another.";
                    return;
                }

                // Insert student user
                string insertSql = @"
                    INSERT INTO Users (Username, Password, FullName, Email, Role, CreatedAt)
                    VALUES (@Username, @Password, @FullName, @Email, 'Student', GETDATE());
                    SELECT SCOPE_IDENTITY();
                ";

                var insertParams = new[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password),
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Email", email)
                };

                object newId = DBHelper.ExecuteScalar(insertSql, insertParams);

                if (newId != null && int.TryParse(newId.ToString(), out int userId))
                {
                    // Auto login the newly registered student
                    Session["UserId"] = userId;
                    Session["Username"] = username;
                    Session["FullName"] = fullName;
                    Session["UserRole"] = "Student";

                    Response.Redirect("Index.aspx?msg=" + Server.UrlEncode("Welcome " + fullName + "! Your student account has been created."));
                }
                else
                {
                    pnlMessage.Visible = true;
                    pnlMessage.CssClass = "alert alert-danger py-2 small";
                    lblMessage.Text = "Registration failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger py-2 small";
                lblMessage.Text = "Error during registration: " + ex.Message;
            }
        }
    }
}
