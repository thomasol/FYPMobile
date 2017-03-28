using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FinalYearProject.Api.Controllers
{
    public class EventsController : ApiController
    {
        // GET: api/Events
        public string Get()
        {
            return "-1";
        }
        

        // POST: api/Events
        public void Post([FromBody]Event value)
        {
            var esRepo = new SearchRepository<Event>("event", "fyp");
            var res= esRepo.Add(value);

        }

        // PUT: api/Events/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Events/5
        public void Delete(int id)
        {
        }
    }
}
