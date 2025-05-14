using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class Logout : System.Web.UI.Page
    {
        private readonly string MyConnectionLog = ConfigurationManager.ConnectionStrings["MyConnectionDb"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Log logout time
                if (Session["LogId"] != null)
                {
                    int logId = Convert.ToInt32(Session["LogId"]);
                    Console.Write("LogId" + logId);
                    LogUserLogout(logId);
                }

                // Clear session and cookies
                FormsAuthentication.SignOut();
                Session.Abandon();

                HttpCookie usernameCookie = new HttpCookie("Username") { Expires = DateTime.Now.AddDays(-1) };
                HttpCookie userIdCookie = new HttpCookie("UserId") { Expires = DateTime.Now.AddDays(-1) };
                HttpCookie roleIdCookie = new HttpCookie("RoleId") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(usernameCookie);
                Response.Cookies.Add(userIdCookie);
                Response.Cookies.Add(roleIdCookie);

                // Redirect to login page
                Response.Redirect("Login.aspx", false);
            }
        }

        private void LogUserLogout(int logId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MyConnectionLog))
                {
                    string query = @"UPDATE [amit_singh].[dbo].[UserLog]
                     SET [loginOutTime] = @loginOutTime    
                     WHERE [Id] = @logId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@logId", logId);
                        cmd.Parameters.AddWithValue("@loginOutTime", DateTime.Now);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log error to a file or event log if needed
            }
        }
    }
}