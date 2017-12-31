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
    public class UpdateUsersByUserIDController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(int userID, string pass, string spice, string encrypttype, string moduser, string firstN, string lastN, bool isAdmin)
        {
            SingleIntegerResult Result = db.UpdateUsersByUserID(userID, pass, spice, encrypttype, moduser, firstN, lastN, isAdmin).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
