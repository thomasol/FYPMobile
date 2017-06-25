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

        public ISearchResponse<OfflineStore> SearchLocationsByEanOffline(FypSearchRequest search)
        {
            QueryContainer eanQuery = new QueryContainer();

            if (search.ProductId != null)
            {
                eanQuery =
                    Query<OfflineStore>.Bool(
                        x =>
                        x.Must(m => m.MatchPhrase(
                                descriptor => descriptor.Field("ean").Query(search.ProductId))));
            }

            var response =
                ElasticClient.Search<OfflineStore>(
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
            QueryContainer searchQuery = new QueryContainer();

            if (search.SearchTerm != null)
            {
                searchQuery =
                    Query<OfflineStore>.Bool(
                        x =>
                        x.Must(m => m.QueryString(qs => qs.
                            DefaultField("_all").Query(search.SearchTerm))));
            }


            var response =
                ElasticClient.Search<OnlineStore>(
                    s =>
                    s.Type(Type)
                        .Query(
                            q => searchQuery)
                        .From(search.Page)
                        .Size(search.Size)
                        );

            return response;
        }

        public void CreateMap()
        {
            var res = ElasticClient.Map<Domain.Event>(x => x.Index(Index).AutoMap(4));
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