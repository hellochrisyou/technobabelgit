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
    public class FullTermListController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();
        
        [ResponseType(typeof(List<FullTermList>))]
        public IHttpActionResult Get(int compid)
        {
            List<FullTermList> Result = db.GetFullTermListByCompanyID(compid).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
