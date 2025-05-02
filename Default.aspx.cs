
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{ 
    public partial class Default : CountryCodeApplication.AuthorizationRoute
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCountries();
                LoadPhoneNumbers();
            }
        }

        private void LoadCountries()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllCountries", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlCountries.DataSource = reader;
                ddlCountries.DataTextField = "CountryName";
                ddlCountries.DataValueField = "CountryCode";
                ddlCountries.DataBind();
                conn.Close();
            }
        }

        private void LoadPhoneNumbers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllPhoneNumbers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPhoneNumbers.DataSource = dt;
                gvPhoneNumbers.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlCountries.SelectedValue != "" && !string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertPhoneNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@CountryCode", ddlCountries.SelectedValue);
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Phone number added successfully!";
                        
                    }
                    catch(Exception ex)
                    {
                        Response.Write("Please enter the valid phone number of 10 digits only!.");
                    }
                    finally
                    {
                        conn.Close();
                        txtPhoneNumber.Text = "";
                        ddlCountries.ClearSelection();
                        LoadPhoneNumbers();
                    }
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please select a country and enter a phone number.";
            }
        }

        protected void gvPhoneNumbers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPhoneNumbers.PageIndex = e.NewPageIndex;
            LoadPhoneNumbers();
        }

        protected void gvPhoneNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}