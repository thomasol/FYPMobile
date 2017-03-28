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
        public bool CheckNewUser(int googleId)
        {
            var search = new SearchRepository<User>("user", "user");
            var result = search.GetById(googleId);
            if (result.Total > 0)
            {
                return false;
            }
            return true;
        }
        
    }
}
