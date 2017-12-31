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
    public class GetTopTenClickedByCompIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(List<TopTenTerm>))]
        public IHttpActionResult Get(int compID)
        {
            List<TopTenTerm> Result = db.GetTopTenClickedTermsByCompID(compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
