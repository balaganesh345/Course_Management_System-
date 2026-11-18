using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace CourseManagementSystem
{
    public partial class DownloadMaterialPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("Please log in to download course materials."));
                return;
            }

            if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out int materialId))
            {
                lblError.Text = "Invalid material ID.";
                return;
            }

            string userRole = Session["UserRole"].ToString();
            int userId = Convert.ToInt32(Session["UserId"]);

            try
            {
                // Fetch material record
                string sql = "SELECT MaterialId, CourseId, MaterialType, Title, TextContent, FileName, FilePath FROM CourseMaterials WHERE MaterialId = @MaterialId";
                var parameters = new[] { new SqlParameter("@MaterialId", materialId) };
                DataTable dt = DBHelper.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count == 0)
                {
                    lblError.Text = "Material not found.";
                    return;
                }

                DataRow row = dt.Rows[0];
                int courseId = Convert.ToInt32(row["CourseId"]);
                string materialType = row["MaterialType"].ToString();
                string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Material";
                string fileName = row["FileName"] != DBNull.Value ? row["FileName"].ToString() : "document.txt";
                string relativePath = row["FilePath"] != DBNull.Value ? row["FilePath"].ToString() : "";
                string textContent = row["TextContent"] != DBNull.Value ? row["TextContent"].ToString() : "";

                // If user is a student, verify enrollment
                if (userRole == "Student")
                {
                    string checkSql = "SELECT COUNT(1) FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId";
                    var checkParams = new[]
                    {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@CourseId", courseId)
                    };
                    int isEnrolled = Convert.ToInt32(DBHelper.ExecuteScalar(checkSql, checkParams));
                    if (isEnrolled == 0)
                    {
                        lblError.Text = "Access Denied: You must be enrolled in this course to view its materials.";
                        return;
                    }
                }

                // If material is a text note, display it inline as a text file
                if (materialType == "Text")
                {
                    Response.Clear();
                    Response.ContentType = "text/plain; charset=utf-8";
                    string safeFileName = title.Replace(" ", "_") + ".txt";
                    Response.AddHeader("Content-Disposition", "inline; filename=\"" + Server.UrlEncode(safeFileName) + "\"");
                    Response.Write(textContent);
                    Response.End();
                    return;
                }

                if (string.IsNullOrEmpty(relativePath))
                {
                    lblError.Text = "No file path associated with this material item.";
                    return;
                }

                string physicalPath = Server.MapPath(relativePath);
                if (!File.Exists(physicalPath))
                {
                    lblError.Text = "The requested file could not be found on the server.";
                    return;
                }

                string ext = Path.GetExtension(physicalPath).ToLower();
                string contentType = "application/octet-stream";
                if (ext == ".pdf") contentType = "application/pdf";
                else if (ext == ".txt") contentType = "text/plain; charset=utf-8";
                else if (ext == ".png") contentType = "image/png";
                else if (ext == ".jpg" || ext == ".jpeg") contentType = "image/jpeg";
                else if (ext == ".gif") contentType = "image/gif";
                else if (ext == ".webp") contentType = "image/webp";
                else if (ext == ".svg") contentType = "image/svg+xml";
                else if (ext == ".html" || ext == ".htm") contentType = "text/html; charset=utf-8";
                else if (ext == ".docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                else if (ext == ".doc") contentType = "application/msword";
                else if (ext == ".xlsx") contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                else if (ext == ".xls") contentType = "application/vnd.ms-excel";
                else if (ext == ".zip") contentType = "application/zip";

                Response.Clear();
                Response.ContentType = contentType;
                // Use 'inline' so browser opens PDF, TXT, images directly in browser rather than downloading
                Response.AddHeader("Content-Disposition", "inline; filename=\"" + Server.UrlEncode(fileName) + "\"");
                Response.TransmitFile(physicalPath);
                Response.End();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading material: " + ex.Message;
            }
        }
    }
}
