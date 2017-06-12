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

        public ISearchResponse<OnlineStore> SearchLocationsByEan(FypSearchRequest search)
        {
            QueryContainer eanQuery = new QueryContainer();

            if (search.ProductId != null)
            {
                eanQuery =
                    Query<OnlineStore>.Bool(
                        x =>
                        x.Must(m => m.MatchPhrase(
                                descriptor => descriptor.Field("ean").Query(search.ProductId))));
            }
            

            var response =
                ElasticClient.Search<OnlineStore>(
                    s =>
                    s.Type(Type)
                        .Query(
                            q => eanQuery)
                        .From(search.Page)
                        .Size(search.Size)
                        );

            return response;
        }

        public ISearchResponse<OnlineStore> SearchLocationsBySearchTerm(FypSearchRequest search)
        {
            QueryContainer searchTermQuery = new QueryContainer();
            //QueryContainer storeQuery = new QueryContainer();
            //QueryContainer geoQuery = new QueryContainer();

            if (search.SearchTerm != null)
            {
                searchTermQuery =
                    Query<OnlineStore>.Bool(
                        x =>
                        x.Must(m => m.MatchPhrase(
                                descriptor => descriptor.Field("description").Field("retailer").Query(search.SearchTerm))
                                ));
            }


            var response =
                ElasticClient.Search<OnlineStore>(
                    s =>
                    s.Type(Type)
                        .Query(
                            q => searchTermQuery)
                        .From(search.Page)
                        .Size(search.Size)
                        );

            return response;
        }

        public void CreateMap()
        {
            var res = ElasticClient.Map<Product>(x => x.Index(Index).AutoMap(4));
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