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
    public class SelectUsersByCompanyIDController : ApiController
    {

        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(List<UserListItem>))]
        public IHttpActionResult Get(int compID)
        {
            List<UserListItem> Result = db.SelectUsersByCompanyID(compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
