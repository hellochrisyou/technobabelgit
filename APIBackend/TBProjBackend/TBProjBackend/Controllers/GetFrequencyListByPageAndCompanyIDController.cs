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
    public class GetFrequencyListByPageAndCompanyIDController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();


        [ResponseType(typeof(List<FrequencyCombinedListItem>))]
        public IHttpActionResult Get(int page, int compid, bool listened)
        {
            List<FrequencyCombinedListItem> Result = db.GetFrequencyListByPageAndCompanyID(page, compid, listened).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
