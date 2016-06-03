using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * @author: Fei Wang
 * @date: May 26, 2016
 * @version: 0.0.1
 */

namespace COMP2007_Lab3
{
    public partial class Navbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddActiveClass();
        }

        /**
         * This method add active class to current corresponding list item
         * in the navbar
         * @method AddActiveClass
         * @return {void}
         */
        private void AddActiveClass()
        {
            switch (Page.Title)
            {
                case "Home Page":
                    home.Attributes.Add("class", "active");
                    break;
                case "Contact":
                    contact.Attributes.Add("class", "active");
                    break;
            }
        }
    }
}