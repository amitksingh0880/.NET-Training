using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class UserDetail : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;
        string userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Request.QueryString["username"];
                if (!string.IsNullOrEmpty(username))
                {
                    ViewState["username"] = username;
                    LoadUserData(username);
                }
                else
                {
                    lblError.Text = "No username provided.";
                    lblError.CssClass = "error";
                }
            }
        }

        private void LoadUserData(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Query to fetch user profile details
                    string query = @"SELECT a.Id, a.Username, p.UserId, p.FullName, p.Address, p.PhoneNumber, p.Gender, p.ProfilePicture
                                  FROM UserProfile p INNER JOIN AppUsers a 
                                  ON a.Id = p.UserId
                                  WHERE a.Username = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", username);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                // Ensure ProfilePicture is not null; set a default image if necessary
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (string.IsNullOrEmpty(row["ProfilePicture"]?.ToString()))
                                    {
                                        row["ProfilePicture"] = "~/Images/default-profile.png";
                                    }
                                }
                                GridViewUsers.DataSource = dt;
                                GridViewUsers.DataBind();
                            }
                            else
                            {
                                string userIdQuery = "SELECT Id FROM AppUsers WHERE Username = @Username";
                                using (SqlCommand userIdCmd = new SqlCommand(userIdQuery, conn))
                                {
                                    userIdCmd.Parameters.AddWithValue("@Username", username);
                                    conn.Open();
                                    object result = userIdCmd.ExecuteScalar();
                                    conn.Close();

                                    if (result != null)
                                    {
                                        userId = result.ToString();
                                        lblError.Text = "User profile not found. Redirecting to add details.";
                                        Response.Redirect($"AddUserDetails.aspx?userId={userId}");
                                    }
                                    else
                                    {
                                        lblError.Text = "User not found in the system.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading user data: " + ex.Message;
                lblError.CssClass = "error";
            }
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            userId = e.CommandArgument.ToString();
            Response.Redirect($"AddUserDetails.aspx?userId={userId}");
        }
    }
}