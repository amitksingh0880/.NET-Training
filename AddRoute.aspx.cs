using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class AddRoute : System.Web.UI.Page
    {
        protected int? InsertedRouteID
        {
            get { return ViewState["InsertedRouteID"] as int?; }
            set { ViewState["InsertedRouteID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string routeName = txtRouteName.Text.Trim();
            string routeURL = txtRouteURL.Text.Trim();
            string pageHandler = txtPageHandler.Text.Trim();
            string parentRouteIDText = txtParentRouteID.Text.Trim();
            int? parentRouteID = string.IsNullOrEmpty(parentRouteIDText) ? (int?)null : int.Parse(parentRouteIDText);
            DateTime updatedDate = DateTime.Now;

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionDb"].ConnectionString;

            string query = @"
                INSERT INTO [dbo].[RouteMaster] 
                    ([RouteName], [RouteURL], [PageHandler], [ParentRouteID], [UpdatedDate])
                OUTPUT INSERTED.RouteID
                VALUES 
                    (@RouteName, @RouteURL, @PageHandler, @ParentRouteID, @UpdatedDate)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RouteName", routeName);
                cmd.Parameters.AddWithValue("@RouteURL", routeURL);
                cmd.Parameters.AddWithValue("@PageHandler", pageHandler);
                cmd.Parameters.AddWithValue("@ParentRouteID", (object)parentRouteID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedDate", updatedDate);

                try
                {
                    conn.Open();
                    InsertedRouteID = (int)cmd.ExecuteScalar();

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Route added. Now assign roles.";

                    chkRoles.Visible = true;
                    lblRoles.Visible = true;
                    btnAssignRoles.Enabled = true;
                    btnAssignRoles.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void btnAssignRoles_Click(object sender, EventArgs e)
        {
            if (InsertedRouteID == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Insert a route first.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (ListItem item in chkRoles.Items)
                {
                    if (item.Selected)
                    {
                        int roleId = int.Parse(item.Value);
                        string query = "INSERT INTO [dbo].[RouteRole] ([RouteID], [RoleID]) VALUES (@RouteID, @RoleID)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@RouteID", InsertedRouteID.Value);
                            cmd.Parameters.AddWithValue("@RoleID", roleId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Roles assigned successfully.";
            chkRoles.ClearSelection();
            btnAssignRoles.Enabled = false;
        }
    }
}
