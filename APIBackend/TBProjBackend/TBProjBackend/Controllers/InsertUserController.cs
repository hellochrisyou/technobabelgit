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
    public class InsertUserController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(string cUser, int comID, string email, string password, string salt, string firstN, string LastN, string salttype, bool admin)
        {
            SingleIntegerResult Result = db.InsertNewUser(cUser, comID, email, password, salt, firstN, LastN, salttype, admin).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
