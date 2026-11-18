using System;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] != null)
            {
                string role = Session["UserRole"].ToString();
                string fullName = Session["FullName"] != null ? Session["FullName"].ToString() : Session["Username"].ToString();

                phLoggedIn.Visible = true;
                phAnonymous.Visible = false;

                if (role == "Admin")
                {
                    phAdminNav.Visible = true;
                    phInstructorNav.Visible = false;
                    phStudentNav.Visible = false;
                    phAdminAddBtn.Visible = true;
                    litUserBadge.Text = "<i class='bi bi-person-gear text-primary me-1'></i> Admin: " + Server.HtmlEncode(fullName);
                }
                else if (role == "Instructor")
                {
                    phAdminNav.Visible = false;
                    phInstructorNav.Visible = true;
                    phStudentNav.Visible = false;
                    phAdminAddBtn.Visible = false;
                    litUserBadge.Text = "<i class='bi bi-person-video3 text-warning-emphasis me-1'></i> Instructor: " + Server.HtmlEncode(fullName);
                }
                else if (role == "Student")
                {
                    phAdminNav.Visible = false;
                    phInstructorNav.Visible = false;
                    phStudentNav.Visible = true;
                    phAdminAddBtn.Visible = false;
                    litUserBadge.Text = "<i class='bi bi-backpack text-success me-1'></i> Student: " + Server.HtmlEncode(fullName);
                }
            }
            else
            {
                phLoggedIn.Visible = false;
                phAnonymous.Visible = true;
                phAdminNav.Visible = false;
                phInstructorNav.Visible = false;
                phStudentNav.Visible = false;
            }
        }
    }
}
