using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace TBProjBackend.Controllers
{
    public class graphGetCompanyNameController : ApiController
    {
        public IHttpActionResult Get(string accesstoken, string login)
        {
            //admin@technobabel.onmicrosoft.com

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://graph.windows.net/technobabel.onmicrosoft.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var Result = client.GetAsync("users/" + login +"/getMemberGroups?api-version=1.6");

            if (Result == null)
            {
                return NotFound();
            }

            return Ok(Result);
        }
    }
}
