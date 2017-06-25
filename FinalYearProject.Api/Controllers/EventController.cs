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
    public class EventController : ApiController
    {
        private string _indexName = "fyp";

        // GET: api/Events
        public List<Event> Get()
        {
            var esRepo = new SearchRepository<Event>("event", _indexName);
            var res = esRepo.GetAll(0, 10000);
            return res.Documents.ToList();
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
