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
    public class ChangeNeedsPassResetByUserIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(int userID, string email)
        {
            SingleIntegerResult Result = db.ChangeNeedsPassResetByUserID(userID, email).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
