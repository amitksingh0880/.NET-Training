using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class CountryLayering : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountry();
            }
        }

        private void BindCountry()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CountryId, CountryName FROM Countries_A", con))
                    {
                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            ddlCountry.DataSource = dt;
                            ddlCountry.DataTextField = "CountryName";
                            ddlCountry.DataValueField = "CountryId";
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading countries: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("Select State", "0"));
                ddlDistrict.Items.Clear();
                ddlDistrict.Items.Add(new ListItem("Select District", "0"));

                if (ddlCountry.SelectedValue != "0")
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT StateId, StateName FROM States_A WHERE CountryId = @CountryId", con))
                        {
                            cmd.Parameters.AddWithValue("@CountryId", ddlCountry.SelectedValue);
                            con.Open();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                ddlState.DataSource = dt;
                                ddlState.DataTextField = "StateName";
                                ddlState.DataValueField = "StateId";
                                ddlState.DataBind();
                                ddlState.Items.Insert(0, new ListItem("Select State", "0"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading states: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                ddlDistrict.Items.Clear();
                ddlDistrict.Items.Add(new ListItem("Select District", "0"));

                if (ddlState.SelectedValue != "0")
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT DistrictId, DistrictName FROM Districts_A WHERE StateId = @StateId", con))
                        {
                            cmd.Parameters.AddWithValue("@StateId", ddlState.SelectedValue);
                            con.Open();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                ddlDistrict.DataSource = dt;
                                ddlDistrict.DataTextField = "DistrictName";
                                ddlDistrict.DataValueField = "DistrictId";
                                ddlDistrict.DataBind();
                                ddlDistrict.Items.Insert(0, new ListItem("Select District", "0"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading districts: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;

                // Validate selections
                if (ddlCountry.SelectedValue == "0" || ddlState.SelectedValue == "0" || ddlDistrict.SelectedValue == "0")
                {
                    lblError.Text = "Please select a valid Country, State, and District.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    return;
                }

                HttpCookie usernameCookie = Request.Cookies["Username"];
                HttpCookie userIdCookie = Request.Cookies["UserId"];
                string username = usernameCookie?.Value;
                string userId = userIdCookie?.Value;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
                {
                    lblError.Text = "User not authenticated. Please log in.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                // Validate user ID is an integer
                if (!int.TryParse(userId, out int parsedUserId))
                {
                    lblError.Text = "Invalid user ID.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    return;
                }

                // Save to database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserSelections_A (UserId, Username, CountryId, StateId, DistrictId) VALUES (@UserId, @Username, @CountryId, @StateId, @DistrictId)", con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", parsedUserId);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@CountryId", ddlCountry.SelectedValue);
                        cmd.Parameters.AddWithValue("@StateId", ddlState.SelectedValue);
                        cmd.Parameters.AddWithValue("@DistrictId", ddlDistrict.SelectedValue);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Display success message
                lblError.Text = "Selection saved successfully!";
                lblError.CssClass = "success";
                lblError.Visible = true;

                // Reset dropdowns
                ddlCountry.SelectedValue = "0";
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("Select State", "0"));
                ddlDistrict.Items.Clear();
                ddlDistrict.Items.Add(new ListItem("Select District", "0"));
            }
            catch (Exception ex)
            {
                lblError.Text = "Error saving selection: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }
    }
}