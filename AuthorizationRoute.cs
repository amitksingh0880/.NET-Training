using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace CountryCodeApplication
{
    public class AuthorizationRoute : Page
    {
        private readonly string connStr_A = ConfigurationManager.ConnectionStrings["MyConnectionDb"]?.ConnectionString;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Redirect if not authenticated
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Read role ID from cookie
            HttpCookie roleIdCookie = Request.Cookies["RoleId"];
            if (roleIdCookie == null || !int.TryParse(roleIdCookie.Value, out int roleId))
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Check if the user is authorized for the current page
            if (!IsAuthorized(roleId))
            {
                Response.Redirect("~/Unauthorized.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Optional: Set user role name in Context or Session for later use
            string roleName = GetUserRoleFromDb(roleId);
            HttpContext.Current.Items["UserRoleName"] = roleName;
        }

        private string GetUserRoleFromDb(int roleId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr_A))
                {
                    string query = "SELECT RoleName FROM RoleMaster WHERE RoleID = @RoleId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoleId", roleId);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private bool IsAuthorized(int roleId)
        {
            try
            {
                string currentPage = "/" + Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "").ToLower();

                using (SqlConnection conn = new SqlConnection(connStr_A))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM RouteMaster rm 
                        INNER JOIN RouteRole rr ON rm.RouteID = rr.RouteID 
                        WHERE LOWER(rm.RouteURL) = @RouteURL AND rr.RoleID = @RoleId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RouteURL", currentPage);
                        cmd.Parameters.AddWithValue("@RoleId", roleId);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
