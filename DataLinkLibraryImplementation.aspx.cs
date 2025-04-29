using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AgeCalculator;

namespace CountryCodeApplication
{
    public partial class DataLinkLibraryImplementation : CountryCodeApplication.AuthorizationRoute
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
               
                DateTime Date = Convert.ToDateTime(BirthDate.Text);
                string output = ClassAge.Agecalculator(Date, UserName.Text);

               
                ldlName.Text = $"Username: {UserName.Text}";
                lblAge.Text = $"{output}";

               
                string connString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    
                    string query = "INSERT INTO Users (Username, BirthDate, Age) VALUES (@Username, @BirthDate, @Age)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@Username", UserName.Text);
                        cmd.Parameters.AddWithValue("@BirthDate", Date);
                        cmd.Parameters.AddWithValue("@Age", output);

                       
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                       
                        lblMessage.Text = "Data saved successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }

        }
    }
}