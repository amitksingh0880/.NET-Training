
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace CountryCodeApplication
    {
        public partial class Login : System.Web.UI.Page
        {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;
        private readonly string MyConnectionLog = ConfigurationManager.ConnectionStrings["MyConnectionDb"]?.ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Dashboard.aspx", false);
                    return;
                }

                if (string.IsNullOrEmpty(connStr))
                {
                    lblError.Text = "Database connection string is not configured.";
                    lblError.Visible = true;
                    return;
                }

                // Load username from cookie
                HttpCookie usernameCookie = Request.Cookies["Username"];
                if (usernameCookie != null && !string.IsNullOrEmpty(usernameCookie.Value))
                {
                    txtUsername.Text = usernameCookie.Value;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Please enter both username and password.";
                    lblError.Visible = true;
                    return;
                }

                var authResult = AuthenticateUser(username, password);
                if (authResult.HasValue)
                {
                    var (userId, roleId) = authResult.Value;

                    // Save username in cookie
                    HttpCookie usernameCookie = new HttpCookie("Username")
                    {
                        Value = username,
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(usernameCookie);

                    // Save UserId in cookie
                    HttpCookie userIdCookie = new HttpCookie("UserId")
                    {
                        Value = userId.ToString(),
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(userIdCookie);

                    HttpCookie RoleIdCookie = new HttpCookie("RoleId")
                    {
                        Value = roleId.ToString(),
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(RoleIdCookie);

                    HttpCookie logIdCookie = new HttpCookie("logId")
                    {
                        Value = LogUserLogin(userId).ToString(),
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(logIdCookie);

                    // Set authentication and session
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session["Username"] = username;
                    Session["UserId"] = userId;
                    Session["UserID"] = userId;
                    Session["RoleID"] = roleId;
                    Session["LogId"] = LogUserLogin(userId);

                    Response.Redirect("Dashboard.aspx", false);
                }
                else
                {
                    lblError.Text = "Invalid username, password, or account is inactive.";
                    lblError.Visible = true;
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = $"Database error: {ex.Message}";
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = $"An error occurred: {ex.Message}";
                lblError.Visible = true;
            }
        }

        private int LogUserLogin(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MyConnectionLog))
                {
                    string query = @"INSERT INTO [amit_singh].[dbo].[UserLog] 
                                ([userId], [loginTime], [IpAddress]) 
                                OUTPUT INSERTED.Id
                                VALUES (@userId, @loginTime, @IpAddress)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@loginTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IpAddress", HttpContext.Current.Request.UserHostAddress);

                        conn.Open();
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = $"Logging error: {ex.Message}";
                lblError.Visible = true;
                return 0;
            }
        }

        private void LogUserLogout(int logId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MyConnectionLog))
                {
                    string query = @"UPDATE [amit_singh].[dbo].[UserLog] 
                                SET [loginOutTime] = @loginOutTime, 
                                    [activeMinutes] = DATEDIFF(MINUTE, [loginTime], @loginOutTime)
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

        private (int userId, int roleId)? AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                lblError.Text = "Database connection is not configured.";
                lblError.Visible = true;
                return null;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT Id, RoleId FROM AppUsers WHERE Username = @Username AND Password = @Password AND IsActive = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0); // Id
                                int roleId = reader.GetInt32(1); // RoleId
                                return (userId, roleId);
                            }
                        }
                    }
                }
                lblError.Text = "Invalid credentials or inactive user.";
                lblError.Visible = true;
                return null;
            }
            catch (SqlException ex)
            {
                lblError.Text = $"Database connection error: {ex.Message}";
                lblError.Visible = true;
                return null;
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SignUp", false);
        }
    }
}
