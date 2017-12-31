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
    public class GetUserListBySearchCompanyIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [ResponseType(typeof(List<UserListItem>))]
        public IHttpActionResult Get(string searchuser, int compID)
        {
            List<UserListItem> Result = db.GetUserListBySearchCompany(searchuser, compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
