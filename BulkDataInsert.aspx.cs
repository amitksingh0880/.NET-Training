using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace CountryCodeApplication
{
    public partial class BulkDataInsert : System.Web.UI.Page
    {
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string filePath = Server.MapPath("~/Uploads/" + Path.GetFileName(FileUpload1.FileName));
                    FileUpload1.SaveAs(filePath);

                    string[] csvLines = File.ReadAllLines(filePath);

                    string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionDb"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        for (int i = 1; i < csvLines.Length; i++)
                        {
                            string[] values = csvLines[i].Split(',');

                            if (values.Length == 4)
                            {
                                using (SqlCommand cmd = new SqlCommand("InsertBulkDataThroughCSV", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Name", values[0].Trim());
                                    cmd.Parameters.AddWithValue("@City", values[1].Trim());
                                    cmd.Parameters.AddWithValue("@PhoneNo", values[2].Trim());
                                    cmd.Parameters.AddWithValue("@Designation", values[3].Trim());

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        con.Close();
                    }

                    lblMessage.Text = "Data inserted successfully via stored procedure.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Please upload a CSV file.";
            }
        }
    }
}