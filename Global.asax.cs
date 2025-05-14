using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace CountryCodeApplication
{
    public class Global : HttpApplication
    {
        private readonly string connStr_A = ConfigurationManager.ConnectionStrings["MyConnectionDb"]?.ConnectionString;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Log logout when session ends
            if (Session["LogId"] != null)
            {
              
            }
        }

 
    }
}