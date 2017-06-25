using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Search
{
    public class OfflineSearchReository : SearchRepository<OfflineStore>
    {
        public OfflineSearchReository(string type, string index) : base(type, index)
        {
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

        public ISearchResponse<OfflineStore> SearchLocationsBySearchTermOffline(FypSearchRequest search)
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
                ElasticClient.Search<OfflineStore>(
                    s =>
                    s.Type(Type)
                        .Query(
                            q => searchQuery)
                        .From(search.Page)
                        .Size(search.Size)
                        );

            return response;
        }
    }
}