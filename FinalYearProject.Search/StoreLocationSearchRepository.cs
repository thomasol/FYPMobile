//using ChannelAdvisor.WhereToBuy.Entities.Api.Search;
//using ChannelAdvisor.WhereToBuy.Entities.Elastic;
//using ChannelAdvisor.WhereToBuy.Search.Repositories.BaseClasses;
//using Nest;
//using System.Collections.Generic;

//namespace ChannelAdvisor.WhereToBuy.Search.Repositories
//{
//    public class StoreLocationSearchRepository : SearchRepository<SchemaLocation>
//    {
//        public StoreLocationSearchRepository(string type, string index)
//            : base(type, index)
//        {
//        }

//        public ISearchResponse<SchemaLocation> Search(OnlineLocationSearchRequest search)
//        {
//            QueryContainer storeQuery = new QueryContainer();

//            if (search.StoreId != null)
//            {
//                storeQuery =
//                    Query<SchemaRetailerProductLocation>.Bool(
//                        x =>
//                        x.Must(m => m.Term(
//                                descriptor => descriptor.Field(fz => fz.StoredId).Value(search.StoreId.Value))));
//            }

//            if (!search.PageSize.HasValue)
//            {
//                search.PageSize = 10;
//            }

//            if (!search.Page.HasValue)
//            {
//                search.Page = 0;
//            }

//            var response =
//                ElasticClient.Search<SchemaLocation>(
//                    s =>
//                    s.Type(Type)
//                        .Query(q => storeQuery)

//                        .From(search.Page.Value)
//                        .Size(search.PageSize.Value)
//                );

//            return response;
//        }

//        public void CreateMap()
//        {
//            var res = ElasticClient.Map<SchemaLocation>(x => x.Index(Index).AutoMap(4));
//        }


//        public void IndexBulkItems(List<SchemaLocation> entities, int size)
//        {
//            var response = ElasticClient.IndexMany(entities, Index);
//        }

//    }
//}