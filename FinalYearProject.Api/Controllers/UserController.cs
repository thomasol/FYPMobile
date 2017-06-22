using FinalYearProject.Domain;
using FinalYearProject.Search;
using FinalYearProject.Search.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FinalYearProject.Api.Controllers
{
    public class UserController : ApiController
    {
        private string _indexName = "fyp";
        public string Get(string id)
        {
            var search = new SearchRepository<User>("user", _indexName);
            var result = search.GetById(id);
            if(result.Documents.Count() > 0)
            {
                return result.Documents.FirstOrDefault().Id;
            }
            return "-1";
        }

        public void Post([FromBody]User user)
        {
            // Arrange
            var search = new SearchRepository<User>("user", _indexName);
            //search.AddMappings(user);

            // Act
            search.Add(user);
        }

        public void Put([FromBody]User user)
        {
            // Arrange
            var search = new SearchRepository<User>("user", _indexName);
            //search.AddMappings(user);
            // Act
            search.Update(user);
        }
        public void Delete(int id)
        {
            var search = new SearchRepository<User>("user", _indexName);
            var user = search.GetById(id);
            //search.Delete(user.);
        }
    }
}
