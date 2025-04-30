using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CountryCodeApplication
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbl.Text = "";
                Txt1.Text = "";
                Txt2.Text = "";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Lbl.Text = "User data inserted successfully";
        }
    }
}