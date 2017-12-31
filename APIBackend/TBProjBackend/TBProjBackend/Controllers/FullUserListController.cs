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
    public class FullUserListController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(List<FullUserList>))]
        public IHttpActionResult Get(int compid)
        {
            List<FullUserList> Result = db.GetFullUserListByCompanyID(compid).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
