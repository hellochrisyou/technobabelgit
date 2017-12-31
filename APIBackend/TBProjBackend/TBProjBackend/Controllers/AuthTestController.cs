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
    public class AuthTestController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [Authorize]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get()
        {
            SingleIntegerResult Result = db.authorizationTest().First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
