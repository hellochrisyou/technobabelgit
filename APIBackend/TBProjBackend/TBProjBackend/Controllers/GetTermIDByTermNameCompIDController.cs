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
    public class GetTermIDByTermNameCompIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(TermIDResult))]
        public IHttpActionResult Get(string term, int compID)
        {
            TermIDResult Result = db.GetTermIDByTermNameCompID(term, compID).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
