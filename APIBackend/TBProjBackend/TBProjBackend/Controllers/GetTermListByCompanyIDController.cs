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
    public class GetTermListByCompanyIDController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();
        
        [ResponseType(typeof(List<SingleTerm>))]
        public IHttpActionResult Get(int comID)
        {
            List<SingleTerm> Result = db.GetTermListByCompanyID(comID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
