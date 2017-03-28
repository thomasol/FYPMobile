using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;
using Nest;
using System.Collections.Generic;

namespace FinalYearProject.Search
{
    public class ProductSearchRepository : SearchRepository<OnlineStore>
    {
        public ProductSearchRepository(string type, string index) : base(type, index)
        {
        }

        public ISearchResponse<Product> SearchLocations(FypSearchRequest search)
        {
            QueryContainer productIdQuery = new QueryContainer();
            QueryContainer storeQuery = new QueryContainer();
            QueryContainer geoQuery = new QueryContainer();

            if (search.ProductId != null)
            {
                productIdQuery =
                    Query<Product>.Bool(
                        x =>
                        x.Must(m => m.MatchPhrase(
                                descriptor => descriptor.Field("ean").Query(search.ProductId))));
            }
            

            var response =
                ElasticClient.Search<Product>(
                    s =>
                    s.Type(Type)
                        .Query(
                            q => productIdQuery)
                        .From(search.Page)
                        .Size(search.Size)
                        );

            return response;
        }

        public void CreateMap()
        {
            var res = ElasticClient.Map<OnlineStore>(x => x.Index(Index).AutoMap(4)

            );
        }

        public void CreateIndex()
        {
            var res = ElasticClient.CreateIndex(Index);
        }

        public void IndexBulkItems(List<OnlineStore> entities, int size)
        {
            var response = ElasticClient.IndexMany(entities, Index);
        }
    }
}