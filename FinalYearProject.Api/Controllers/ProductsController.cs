using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;

namespace FinalYearProject.Api.Controllers
{
    public class ProductsController : ApiController
    {
        // GET api/values/5
        public Product Put( FypSearchRequest req)
        {
            var onlinesearch = new Search.ProductSearchRepository("product", "dev-fyp");
            //var id = Convert.ToInt32(req.ProductId);
            var product = onlinesearch.SearchLocations(req);
            
            //product.LocalStores = localsearch.SearchLocations(req);
            return product.Documents.FirstOrDefault();
        }

        // POST api/values
        public void Post([FromBody]Product item)
        {
            // Arrange
            var search = new SearchRepository<Product>("product", "fyp");
            search.AddMappings(item);

            // Act
            search.Add(item);
        }
        

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
