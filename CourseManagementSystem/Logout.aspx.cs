using System;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class LogoutPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("You have been signed out successfully."));
        }
    }
}
