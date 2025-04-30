using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class CountryLayering : CountryCodeApplication.AuthorizationRoute
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountry();
                BindUserSelectionsGridView();
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

                // Check if UserId already exists in UserSelections_A
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserSelections_A WHERE UserId = @UserId", con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", parsedUserId);
                        con.Open();
                        int userCount = (int)cmd.ExecuteScalar();
                        if (userCount > 0)
                        {
                            lblError.Text = "User data already exists, please update it.";
                            lblError.CssClass = "error";
                            lblError.Visible = true;
                            return;
                        }
                    }
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

                // Display success message and refresh GridView
                lblError.Text = "Selection saved successfully!";
                lblError.CssClass = "success";
                lblError.Visible = true;

                // Reset dropdowns
                ddlCountry.SelectedValue = "0";
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("Select State", "0"));
                ddlDistrict.Items.Clear();
                ddlDistrict.Items.Add(new ListItem("Select District", "0"));

                // Refresh GridView
                BindUserSelectionsGridView();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error saving selection: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }

        protected void gvUserSelections_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserSelections.PageIndex = e.NewPageIndex;
            BindUserSelectionsGridView();
        }

        protected void gvUserSelections_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUserSelections.EditIndex = e.NewEditIndex;
            BindUserSelectionsGridView();

            // Populate dropdowns in edit mode
            GridViewRow row = gvUserSelections.Rows[e.NewEditIndex];
            DropDownList ddlCountryEdit = (DropDownList)row.FindControl("ddlCountryEdit");
            DropDownList ddlStateEdit = (DropDownList)row.FindControl("ddlStateEdit");
            DropDownList ddlDistrictEdit = (DropDownList)row.FindControl("ddlDistrictEdit");

            // Store current values
            string currentCountryId = DataBinder.Eval(row.DataItem, "CountryId")?.ToString();
            string currentStateId = DataBinder.Eval(row.DataItem, "StateId")?.ToString();
            string currentDistrictId = DataBinder.Eval(row.DataItem, "DistrictId")?.ToString();

            // Populate countries
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT CountryId, CountryName FROM Countries_A";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlCountryEdit.DataSource = dt;
                ddlCountryEdit.DataTextField = "CountryName";
                ddlCountryEdit.DataValueField = "CountryId";
                ddlCountryEdit.DataBind();
                ddlCountryEdit.Items.Insert(0, new ListItem("Select Country", "0"));
                if (!string.IsNullOrEmpty(currentCountryId))
                    ddlCountryEdit.SelectedValue = currentCountryId;
            }

            // Populate states
            if (!string.IsNullOrEmpty(currentCountryId))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT StateId, StateName FROM States_A WHERE CountryId = @CountryId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CountryId", currentCountryId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlStateEdit.DataSource = dt;
                    ddlStateEdit.DataTextField = "StateName";
                    ddlStateEdit.DataValueField = "StateId";
                    ddlStateEdit.DataBind();
                    ddlStateEdit.Items.Insert(0, new ListItem("Select State", "0"));
                    if (!string.IsNullOrEmpty(currentStateId))
                        ddlStateEdit.SelectedValue = currentStateId;
                }
            }

            // Populate districts
            if (!string.IsNullOrEmpty(currentStateId))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT DistrictId, DistrictName FROM Districts_A WHERE StateId = @StateId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@StateId", currentStateId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlDistrictEdit.DataSource = dt;
                    ddlDistrictEdit.DataTextField = "DistrictName";
                    ddlDistrictEdit.DataValueField = "DistrictId";
                    ddlDistrictEdit.DataBind();
                    ddlDistrictEdit.Items.Insert(0, new ListItem("Select District", "0"));
                    if (!string.IsNullOrEmpty(currentDistrictId))
                        ddlDistrictEdit.SelectedValue = currentDistrictId;
                }
            }
        }

        protected void ddlCountryEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCountryEdit = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlCountryEdit.NamingContainer;
            DropDownList ddlStateEdit = (DropDownList)row.FindControl("ddlStateEdit");
            DropDownList ddlDistrictEdit = (DropDownList)row.FindControl("ddlDistrictEdit");

            ddlStateEdit.Items.Clear();
            ddlStateEdit.Items.Add(new ListItem("Select State", "0"));
            ddlDistrictEdit.Items.Clear();
            ddlDistrictEdit.Items.Add(new ListItem("Select District", "0"));

            if (ddlCountryEdit.SelectedValue != "0")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT StateId, StateName FROM States_A WHERE CountryId = @CountryId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CountryId", ddlCountryEdit.SelectedValue);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlStateEdit.DataSource = dt;
                    ddlStateEdit.DataTextField = "StateName";
                    ddlStateEdit.DataValueField = "StateId";
                    ddlStateEdit.DataBind();
                    ddlStateEdit.Items.Insert(0, new ListItem("Select State", "0"));
                }
            }
        }

        protected void ddlStateEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlStateEdit = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlStateEdit.NamingContainer;
            DropDownList ddlDistrictEdit = (DropDownList)row.FindControl("ddlDistrictEdit");

            ddlDistrictEdit.Items.Clear();
            ddlDistrictEdit.Items.Add(new ListItem("Select District", "0"));

            if (ddlStateEdit.SelectedValue != "0")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT DistrictId, DistrictName FROM Districts_A WHERE StateId = @StateId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@StateId", ddlStateEdit.SelectedValue);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlDistrictEdit.DataSource = dt;
                    ddlDistrictEdit.DataTextField = "DistrictName";
                    ddlDistrictEdit.DataValueField = "DistrictId";
                    ddlDistrictEdit.DataBind();
                    ddlDistrictEdit.Items.Insert(0, new ListItem("Select District", "0"));
                }
            }
        }

        protected void gvUserSelections_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUserSelections.EditIndex = -1;
            BindUserSelectionsGridView();
        }

        protected void gvUserSelections_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                lblError.Visible = false;

                int userId = Convert.ToInt32(gvUserSelections.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvUserSelections.Rows[e.RowIndex];
                string username = ((TextBox)row.Cells[1].Controls[0]).Text;
                string countryId = ((DropDownList)row.FindControl("ddlCountryEdit")).SelectedValue;
                string stateId = ((DropDownList)row.FindControl("ddlStateEdit")).SelectedValue;
                string districtId = ((DropDownList)row.FindControl("ddlDistrictEdit")).SelectedValue;

                // Validate inputs
                if (string.IsNullOrEmpty(username))
                {
                    lblError.Text = "Username cannot be empty.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    return;
                }
                if (countryId == "0" || stateId == "0" || districtId == "0")
                {
                    lblError.Text = "Please select a valid Country, State, and District.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    return;
                }

                // Update database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE UserSelections_A 
                                SET CountryId = @CountryId, 
                                    StateId = @StateId, DistrictId = @DistrictId 
                                WHERE UserId = @UserId" ;
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@CountryId", countryId);
                    cmd.Parameters.AddWithValue("@StateId", stateId);
                    cmd.Parameters.AddWithValue("@DistrictId", districtId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblError.Text = "Selection updated successfully!";
                lblError.CssClass = "success";
                lblError.Visible = true;

                gvUserSelections.EditIndex = -1;
                BindUserSelectionsGridView();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error updating selection: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }

        private void BindUserSelectionsGridView()
        {
            try
            {
                HttpCookie userIdCookie = Request.Cookies["UserId"];
                string userId = userIdCookie?.Value;

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
                {
                    lblError.Text = "User not authenticated or invalid user ID.";
                    lblError.CssClass = "error";
                    lblError.Visible = true;
                    gvUserSelections.DataSource = null;
                    gvUserSelections.DataBind();
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                    u.UserId,
                                    u.Username,
                                    c.CountryName,
                                    s.StateName,
                                    d.DistrictName,
                                    u.CountryId,
                                    u.StateId,
                                    u.DistrictId
                                FROM 
                                    UserSelections_A u
                                    INNER JOIN Countries_A c ON u.CountryId = c.CountryId
                                    INNER JOIN States_A s ON u.StateId = s.StateId
                                    INNER JOIN Districts_A d ON u.DistrictId = d.DistrictId
                                WHERE 
                                    u.UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserId", parsedUserId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvUserSelections.DataSource = dt;
                    gvUserSelections.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading selections: " + ex.Message;
                lblError.CssClass = "error";
                lblError.Visible = true;
            }
        }
    }
}