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
    public class TermExistByCompanyIDController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Get(string term, int compid)
        {
            SingleIntegerResult Result = db.TermExistByCompanyID(term, compid).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
