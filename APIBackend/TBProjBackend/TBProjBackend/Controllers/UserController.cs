using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TBProjBackend.Models;

namespace TBProjBackend.Controllers
{
    public class UserController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();
        
        [ResponseType(typeof(UserListView))]
        public IHttpActionResult Get(int usersID)
        {
            UserListView Result = db.SelectUserByUserID(usersID).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
        
        [Route("api/GetUserListByCompanyID")]
        [ResponseType(typeof(List<UserListItem>))]
        public IHttpActionResult GetUserListByCompanyID(int compID)
        {
            List<UserListItem> Result = db.SelectUsersByCompanyID(compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetUserExist")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(string email, string password)
        {
            SingleIntegerResult Result = db.DoesUserExist(email, password).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Post(string cUser, int comID, string email, string password, string salt, string firstN, string LastN, string salttype, bool admin)
        {
            SingleIntegerResult Result = db.InsertNewUser(cUser, comID, email, password, salt, firstN, LastN, salttype, admin).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Put(int userID, string pass, string spice, string encrypttype, string moduser, string firstN, string lastN, bool isAdmin)
        {
            SingleIntegerResult Result = db.UpdateUsersByUserID(userID, pass, spice, encrypttype, moduser, firstN, lastN, isAdmin).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/PutUserActivationChange")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult PutActivation(int ID, string modUser)
        {
            SingleIntegerResult Result = db.ChangeActivationUserByUserID(ID, modUser).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

    }
}
