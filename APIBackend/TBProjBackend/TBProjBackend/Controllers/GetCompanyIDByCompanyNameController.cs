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
    public class GetCompanyIDByCompanyNameController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(SingleCompanyIDResult))]
        public IHttpActionResult Get(string compname)
        {
            SingleCompanyIDResult Result = db.GetCompanyIDByCompanyName(compname).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
