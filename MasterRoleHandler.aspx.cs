using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class MasterRoleHandler : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRoles();
            }
        }

        private void BindRoles()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT RoleID, RoleName FROM RoleMaster";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                ddlRoles.DataSource = cmd.ExecuteReader();
                ddlRoles.DataTextField = "RoleName";
                ddlRoles.DataValueField = "RoleID";
                ddlRoles.DataBind();
                ddlRoles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Role --", ""));
            }
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlRoles.SelectedValue))
            {
                BindRoutesByRole(Convert.ToInt32(ddlRoles.SelectedValue));
            }
            else
            {
                gvRoutes.DataSource = null;
                gvRoutes.DataBind();
            }
        }

        private void Navigation_btn(object sender, EventArgs e)
        {
            Response.Redirect("dashboard.aspx");
        }

        private void BindRoutesByRole(int roleId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT rm.RouteName, rm.RouteURL, rm.PageHandler
                    FROM RouteMaster rm
                    INNER JOIN RouteRole rr ON rm.RouteID = rr.RouteID
                    WHERE rr.RoleID = @RoleId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRoutes.DataSource = dt;
                gvRoutes.DataBind();
            }
        }

        protected void AddNewRoute_btn(object sender, EventArgs e)
        {
            Response.Redirect("AddRoute.aspx");
        }
    }
}