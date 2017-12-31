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
    public class GetTermListByPageAndCompanyIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        //[Authorize]
        [ResponseType(typeof(List<TermList>))]
        public IHttpActionResult Get(int page, int compid)
        {
            List<TermList> Result = db.GetTermListByPageAndCompanyID(page, compid).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
