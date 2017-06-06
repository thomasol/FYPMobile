using System.Linq;
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
            req.Page = 0;
            req.Size = 1;
            var onlinesearch = new Search.ProductSearchRepository("product", "fyp");
            var product = onlinesearch.SearchLocations(req);
            
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
