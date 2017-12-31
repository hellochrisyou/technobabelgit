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
    public class EmailController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [Route("api/GetSaltByEmail")]
        [ResponseType(typeof(Spices))]
        public IHttpActionResult GetSaltByEmail(string email)
        {
            Spices Result = db.GetSaltByEmail(email).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetLoginCredentialsByEmail")]
        [ResponseType(typeof(List<LoginCredentials>))]
        public IHttpActionResult Get(string email)
        {
            List<LoginCredentials> Result = db.GetLoginCredentialsByEmail(email).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
