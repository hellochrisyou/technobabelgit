using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBProj
{
    class JsonSingleResult
    {
        public int Result { get; set; }
    }

    class SingleCompID
    {
        public int CompanyID { get; set; }
    }

    class Spice
    {
        public string Salt { get; set; }
    }

    class Rights
    {
        public Right[] Information { get; set; }
    }

    class Right
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string RightName { get; set; }

        public bool Active { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    class topTenTerm
    {
        public int TermID { get; set; }

        public string TermName { get; set; }

        public int ListenCount { get; set; }

        public int ClickCount { get; set; }
    }

    class TermNames
    {
        public string TermName { get; set; } 
    }

    class UpdateTermItem
    {
        public string TermName { get; set; }
        public string TermDescription { get; set; }
        public bool Active { get; set; }
    }
    
    class TermListView
    {
        public string TermName { get; set; }
        public string TermDescription { get; set; }
    }

    public class TermListItem
    {
        public int    TermId { get; set; }
        public string TermName { get; set; }
        public string TermDescription { get; set; } 
        public bool   Active { get; set; }
    }

    class TermIDResult
    {
        public int TermID { get; set; }
    }

    class UserListItem
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
       
    }

    class UserListView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }
    }

    class AdministrateListItem
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool Locked { get; set; }
        public bool NeedsPasswordReset { get; set; }

    }
    class FullTermList
    {
        public int TermID { get; set; }
        public string TermName { get; set; }
        public bool Active { get; set; }
    }
    class FullUserList
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
    }

    class graphapiList
    {
        [JsonProperty("value")]
        public Value[] Value { get; set; }
    }
    [Serializable]
    class graphapiUser
    {
        [JsonProperty("objectId")]
        public string objectId { get; set; }
        [JsonProperty("employeeId")]
        public string employeeId { get; set; }
        [JsonProperty("userPrincipalName")]
        public string userPrincipalName { get; set; }
        [JsonProperty("accountEnabled")]
        public bool accountEnabled { get; set; }
        [JsonProperty("department")]
        public string department { get; set; }
        [JsonProperty("givenName")]
        public string givenName { get; set; }
        [JsonProperty("surname")]
        public string surname { get; set; }
        [JsonProperty("passwordProfile")]
        public string passwordProfile { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("forceChangePasswordNextLogin")]
        public string forceChangePasswordNextLogin { get; set; }

    }
    class Value
    {
        [JsonProperty("employeeId")]
        public string employeeId { get; set; }
        [JsonProperty("userPrincipalName")]
        public string userPrincipalName { get; set; }
        [JsonProperty("accountEnabled")]
        public bool accountEnabled { get; set; }
        [JsonProperty("department")]
        public string department { get; set; }
        [JsonProperty("givenName")]
        public string givenName { get; set; }
        [JsonProperty("surname")]
        public string surname { get; set; }
    }
}
