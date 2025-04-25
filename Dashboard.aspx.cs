using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                lblUsername.Text = User.Identity.Name;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("Login.aspx", false);
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
                        conn.Close();

                        if (rowsAffected > 0)
                        {
                            RegisterClientAlert("Your account has been deactivated successfully.");
                            FormsAuthentication.SignOut();
                            Session.Abandon();
                            Response.Redirect("Login.aspx", false);
                        }
                        else
                        {
                            RegisterClientAlert("Account not found or already deactivated.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                RegisterClientAlert($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                RegisterClientAlert($"An error occurred: {ex.Message}");
            }
        }

        private void RegisterClientAlert(string message)
        {
            string script = $"showAlert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", script, true);
        }
    }
}