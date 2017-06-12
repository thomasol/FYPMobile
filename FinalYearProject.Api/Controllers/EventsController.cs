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
        private string _indexName = "fyp";

        //change this
        // GET: api/Events
        public Nest.ISearchResponse<Event> Get()
        {
            var esRepo = new SearchRepository<Event>("event", _indexName);
            var res = esRepo.GetAll(0, 100);
            return res;
        }
        

        // POST: api/Events
        public void Post([FromBody]Event value)
        {
            var esRepo = new SearchRepository<Event>("event", _indexName);
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
