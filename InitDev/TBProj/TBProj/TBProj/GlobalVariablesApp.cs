using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBProj
{
    public class GlobalVariablesApp
    {
        public GlobalVariablesApp()
        {
            this.isAdmin = false;
            this.compName = "";
            this.compID = -1;
            this.loggedUserEmail = "";
            this.lastClickedID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Token = null;
        }

        public void ResetGlobals()
        {
            this.isAdmin = false;
            this.compName = "";
            this.compID = -1;
            this.loggedUserEmail = "";
            this.lastClickedID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Token = null;
        }

        public bool isAdmin { get; set; }

        public string compName { get; set; }

        public int compID { get; set; }

        public AuthenticationResult Token { get; set; }

        public string loggedUserEmail { get; set; }

        public int lastClickedID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
