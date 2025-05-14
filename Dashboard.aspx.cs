


using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class Dashboard : CountryCodeApplication.AuthorizationRoute
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;
        private readonly string connStr_A = ConfigurationManager.ConnectionStrings["MyConnectionDb"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }

            // Check RoleId cookie
            HttpCookie roleIdCookie = Request.Cookies["RoleId"];
            if (roleIdCookie == null || !int.TryParse(roleIdCookie.Value, out int roleId))
            {
                lblError.Text = "Error: Role information not found in cookie. Please log in again.";
                lblError.Visible = true;
                Response.Redirect("Login.aspx", false);
                return;
            }

            // Debug: Display RoleId
            lblError.Text = $"Debug: RoleID = {roleId}";
            lblError.Visible = true;

            // Check authorization
            if (!IsAuthorized(roleId))
            {
                lblError.Text = "Error: You do not have permission to access.";
                lblError.Visible = true;
                Response.Redirect("Unauthorized.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                lblUsername.Text = User.Identity.Name;

                // Retrieve RoleName
                string roleName = GetUserRoleFromDb(roleId);
                if (!string.IsNullOrEmpty(roleName))
                {
                    lblRoleName.Text = roleName;
                }
                else
                {
                    lblError.Text = "Error: Unable to retrieve role information.";
                    lblError.Visible = true;
                }

                // Load navigation menu
                LoadNavigationMenu(roleId);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (Session["LogId"] != null)
            {
                int logId = Convert.ToInt32(Session["LogId"]);
                //LogUserLogout(logId);
            }
            Response.Redirect("Logout.aspx", false);
        }

    

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                RegisterClientAlert("User not authenticated.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "UPDATE AppUsers SET IsActive = 0 WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            RegisterClientAlert("Your account has been deactivated successfully.");
                            Response.Redirect("Logout.aspx", false);
                        }
                        else
                        {
                            RegisterClientAlert("Account not found or already deactivated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegisterClientAlert($"Error deactivating account: {ex.Message}");
            }
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
            catch (Exception ex)
            {
                lblError.Text = $"Error fetching role: {ex.Message}";
                lblError.Visible = true;
                return null;
            }
        }

        private void LoadNavigationMenu(int roleId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr_A))
                {
                    string query = "SELECT rm.RouteName, rm.RouteURL " +
                    "FROM RouteMaster rm " +
                    "INNER JOIN RouteRole rr ON rm.RouteID = rr.RouteID " +
                    "WHERE rr.RoleID = @RoleId " +
                    "AND rm.RouteURL NOT IN ('/login', '/signup', '/userdetails','/AddUserDetails' )";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoleId", roleId);
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            rptMenu.DataSource = dt;
                            rptMenu.DataBind();
                            lblError.Text = $"Debug: Loaded {dt.Rows.Count} routes for RoleID = {roleId}";
                        }
                        else
                        {
                            lblError.Text = $"No routes found for RoleID = {roleId}. Please contact the administrator.";
                        }
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error loading routes: {ex.Message}";
                lblError.Visible = true;
            }
        }

        private bool IsAuthorized(int roleId)
        {
            try
            {
                string currentPage = "/" + Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "").ToLower();
               // CurrentUrl.Text = currentPage;
                using (SqlConnection conn = new SqlConnection(connStr_A))
                {
                    string query = "SELECT COUNT(*) FROM RouteMaster rm INNER JOIN RouteRole rr ON rm.RouteID = rr.RouteID WHERE rm.RouteURL = @RouteURL AND rr.RoleID = @RoleId ";
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
            catch (Exception ex)
            {
                lblError.Text = $"Error checking authorization: {ex.Message}";
                lblError.Visible = true;
                return false;
            }
        }

        private void RegisterClientAlert(string message)
        {
            string script = $"showAlert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", script, true);
        }
    }
}