using System.Linq;
using System.Web.Http;
using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;
using System.Collections.Generic;

namespace FinalYearProject.Api.Controllers
{
    public class ProductController : ApiController
    {
        private string _indexName = "prod-offers";
        // GET api/values/5
        public dynamic Put(FypSearchRequest req)
        {
            req.Page = 0;
            req.Size = 25;
            if(req.SearchType == "text")
            {
                if (req.Mapping == "offlinestore")
                {
                    var offlineSearch = new Search.OfflineSearchReository(req.Mapping, _indexName);
                    var offlineStores = offlineSearch.SearchLocationsBySearchTermOffline(req);
                    return offlineStores.Documents.ToList();
                }
                var onlinesearch = new Search.ProductSearchRepository(req.Mapping, _indexName);
                var onlineStores = onlinesearch.SearchLocationsBySearchTerm(req);
                return onlineStores.Documents.ToList();
            }
            else
            {
                if (req.Mapping == "offlinestore")
                {
                    var offlineSearch = new Search.OfflineSearchReository(req.Mapping, _indexName);
                    var offlineStores = offlineSearch.SearchLocationsByEanOffline(req);
                    return offlineStores.Documents.ToList();
                }
                var onlinesearch = new Search.ProductSearchRepository(req.Mapping, _indexName);
                var onlineStores = onlinesearch.SearchLocationsByEan(req);
                return onlineStores.Documents.ToList();
            }
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
