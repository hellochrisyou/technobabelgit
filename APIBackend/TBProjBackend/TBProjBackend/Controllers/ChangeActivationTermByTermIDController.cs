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
    public class ChangeActivationTermByTermIDController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(int ID, string email)
        {
            SingleIntegerResult Result = db.ChangeActivationTermByTermID(ID, email).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
