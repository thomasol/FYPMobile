using System.Linq;
using System.Web.Http;
using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;
using System.Collections.Generic;

namespace FinalYearProject.Api.Controllers
{
    public class ProductsController : ApiController
    {
        private string _indexName = "prod-offers";
        // GET api/values/5
        public List<OnlineStore> Put( FypSearchRequest req)
        {
            req.Page = 0;
            req.Size = 10;
            var onlinesearch = new Search.ProductSearchRepository("onlinestore", _indexName);
            var onlineStores = onlinesearch.SearchLocationsByEan(req);
            //var offlineSearch = new Search.ProductSearchRepository("localstore", _indexName);
            //var offlineStores = offlineSearch.SearchLocations(req);
            return onlineStores.Documents.ToList();
        }

        // POST api/values
        public void Post([FromBody]Product item)
        {
            // Arrange
            var search = new SearchRepository<Product>("product", _indexName);
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
