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
    public class UserController : ApiController
    {
        public User Get(string id)
        {
            var search = new SearchRepository<User>("user", "fyp");
            var result = search.GetById(1);

            return result.Documents.FirstOrDefault();
        }

        public void Post([FromBody]User user)
        {
            // Arrange
            var search = new SearchRepository<User>("user", "fyp");
            search.AddMappings(user);

            // Act
            search.Add(user);
        }

        public void Put([FromBody]User user)
        {
            // Arrange
            var search = new SearchRepository<User>("user", "fyp");
            search.AddMappings(user);

            // Act
            search.Update(user);
        }
        public void Delete(int id)
        {
            var search = new SearchRepository<User>("user", "fyp");
            var user = search.GetById(id);
            //search.Delete(user.);
        }
    }
}
