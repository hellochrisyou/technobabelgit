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
    public class GetSaltByEmailController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(Spices))]
        public IHttpActionResult Get(string email)
        {
            Spices Result = db.GetSaltByEmail(email).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
