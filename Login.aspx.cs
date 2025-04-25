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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Dashboard.aspx", false);
                    return;
                }

                // Validate connection string
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

                int? userId = AuthenticateUser(username, password);
                if (userId.HasValue)
                {
                    // Save username in cookie
                    HttpCookie usernameCookie = new HttpCookie("Username");
                    usernameCookie.Value = username;
                    usernameCookie.Expires = DateTime.Now.AddDays(30); // Persist for 30 days
                    usernameCookie.HttpOnly = true; // Prevent JavaScript access
                    usernameCookie.Secure = Request.IsSecureConnection; // Secure for HTTPS
                    Response.Cookies.Add(usernameCookie);

                    // Save UserId in cookie
                    HttpCookie userIdCookie = new HttpCookie("UserId");
                    userIdCookie.Value = userId.Value.ToString();
                    userIdCookie.Expires = DateTime.Now.AddDays(30); // Persist for 30 days
                    userIdCookie.HttpOnly = true; // Prevent JavaScript access
                    userIdCookie.Secure = Request.IsSecureConnection; // Secure for HTTPS
                    Response.Cookies.Add(userIdCookie);

                    // Set authentication and session
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session["Username"] = username;
                    Session["UserId"] = userId.Value;

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

        private int? AuthenticateUser(string username, string password)
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
                    string query = "SELECT Id FROM AppUsers WHERE Username = @Username AND Password = @Password AND IsActive = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : (int?)null;
                    }
                }
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