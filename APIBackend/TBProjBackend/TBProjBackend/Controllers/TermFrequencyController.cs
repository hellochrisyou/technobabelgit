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
    public class TermFrequencyController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();



        [Route("api/GetPagesByCompanyIDListName")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult GetPagesByCompanyIDListName(int compID, string listname)
        {
            SingleIntegerResult Result = db.GetPagesByCompanyIDListName(compID, listname).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }


        [Route("api/GetTopTenClickedTerms")]
        [ResponseType(typeof(List<TopTenTerm>))]
        public IHttpActionResult GetClicked(int compID)
        {
            List<TopTenTerm> Result = db.GetTopTenClickedTermsByCompID(compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetTopTenListenedTerms")]
        [ResponseType(typeof(List<TopTenTerm>))]
        public IHttpActionResult GetListened(int compID)
        {
            List<TopTenTerm> Result = db.GetTopTenListenedTermsByCompID(compID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/IncreaseClickedTerm")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult PutClicked(int ID, string Term)
        {
            SingleIntegerResult Result = db.IncreaseClickCountByCompIDTermName(ID, Term).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/IncreaseListenedTerm")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult PutListened(int ID, string Term)
        {
            SingleIntegerResult Result = db.IncreaseListenCountByCompIDTermName(ID, Term).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
