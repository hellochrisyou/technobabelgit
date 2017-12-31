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
    public class TermController : ApiController
    {
        private technobabeldatabaseEntities5 db = new technobabeldatabaseEntities5();

        [Route("api/GetTermListSearch")]
        [ResponseType(typeof(List<TermList>))]
        public IHttpActionResult GetTermListSearch(string term, int compid)
        {
            List<TermList> Result = db.GetTermListBySearchCompany(term, compid).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetTermListCompID")]
        [ResponseType(typeof(List<SingleTerm>))]
        public IHttpActionResult GetTermList(int comID)
        {
            List<SingleTerm> Result = db.GetTermListByCompanyID(comID).ToList();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [ResponseType(typeof(SingleTerm))]
        public IHttpActionResult Get(int ID)
        {
            SingleTerm Result = db.SelectTermByTermID(ID).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetTermByTermNameCompID")]
        [ResponseType(typeof(TermIDResult))]
        public IHttpActionResult Get(string term, int compID)
        {
            TermIDResult Result = db.GetTermIDByTermNameCompID(term, compID).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/GetTermExist")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult GetExist(string term, int compid)
        {
            SingleIntegerResult Result = db.TermExistByCompanyID(term, compid).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Push(string createUser, int comID, string Term, String Description)
        {
            SingleIntegerResult Result = db.InsertNewTerm(createUser, comID, Term, Description).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult Put(int ID, string Descript, string modUser)
        {
            SingleIntegerResult Result = db.UpdateTermByTermID(ID, Descript, modUser).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }

        [Route("api/PutTermActivationChange")]
        [ResponseType(typeof(SingleIntegerResult))]
        public IHttpActionResult PutActivation(int ID, string email)
        {
            SingleIntegerResult Result = db.ChangeActivationTermByTermID(ID, email).First();
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
