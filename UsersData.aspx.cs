using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class UsersData : CountryCodeApplication.AuthorizationRoute
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
                BindUserData();
            }
        }

        private void BindUserData()
        {
            if (string.IsNullOrEmpty(connStr))
            {
                lblError.Text = "Database connection string is not configured.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT Id, Username, IsActive FROM AppUsers";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            userDatas.DataSource = dt;
                            userDatas.DataBind();

                            if (dt.Rows.Count == 0)
                            {
                                lblError.Text = "No users found in the database.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = $"Database error: {ex.Message}";
            }
            catch (Exception ex)
            {
                lblError.Text = $"An error occurred: {ex.Message}";
            }
        }
    }
}