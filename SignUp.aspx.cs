using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class SignUp : System.Web.UI.Page
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

                if (string.IsNullOrEmpty(connStr))
                {
                    lblError.Text = "Database connection string is not configured.";
                    lblError.CssClass = "error";
                }
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblError.Text = "Please correct the errors in the form.";
                lblError.CssClass = "error";
                return;
            }

            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Please enter both username and password.";
                    lblError.CssClass = "error";
                    return;
                }

                InsertUser(username, password);
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Response.Redirect("Dashboard.aspx", false);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    lblError.Text = "Username already exists. Please choose a different username.";
                }
                else
                {
                    lblError.Text = $"Database error: {ex.Message}";
                }
                lblError.CssClass = "error";
            }
            catch (Exception ex)
            {
                lblError.Text = $"An error occurred: {ex.Message}";
                lblError.CssClass = "error";
            }
        }

        private void InsertUser(string username, string password)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                lblError.Text = "Database connection is not configured.";
                lblError.CssClass = "error";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "INSERT INTO AppUsers (Username, Password, IsActive) VALUES (@Username, @Password, 1)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw; // Re-throw to handle in btnSignUp_Click
            }
        }

        protected void page_redirect(object sender, EventArgs e)
        {
            try
            {
                // Debugging: Display a message to confirm the method is called
                lblError.Text = "Redirecting to Login.aspx...";
                lblError.CssClass = "error";
                Response.Redirect("~/Login.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = $"Redirect failed: {ex.Message}";
                lblError.CssClass = "error";
            }
        }
    }
}