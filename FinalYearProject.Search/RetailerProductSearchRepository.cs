//using ChannelAdvisor.WhereToBuy.Domain.Api.Search;
//using ChannelAdvisor.WhereToBuy.Domain.Mapping.Schema;
//using ChannelAdvisor.WhereToBuy.Infra.Search.BaseClasses;
//using Nest;

//namespace ChannelAdvisor.WhereToBuy.Infra.Search
//{
//    public class RetailerProductSearchRepository : SearchRepository<SchemaRetailerProduct>
//    {
//        public RetailerProductSearchRepository(string type, string index)
//            : base(type, index)
//        {
//        }

//        public ISearchResponse<SchemaRetailerProduct> Search(OnlineProductSearchRequest search)
//        {
//            //If we are not including dismissed, define a filter which
//            //removes all dismissed alerts, otherwise leave blank
//            QueryContainer brandQuery = new QueryContainer();
//            QueryContainer productIdQuery = new QueryContainer();
//            QueryContainer countryQuery = new QueryContainer();
//            QueryContainer retailerQuery = new QueryContainer();
//            if (search.Brand != null)
//            {
//                brandQuery =
//                    Query<SchemaRetailerProduct>.Bool(
//                        x =>
//                        x.Must(
//                            m =>
//                            m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Product.Brand.Description).Query(search.Brand))));
//            }

//            if (search.Retailer != null)
//            {
//                retailerQuery =
//                    Query<SchemaRetailerProduct>.Bool(
//                        x =>
//                        x.Must(
//                            m =>
//                            m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Retailer.Description).Query(search.Retailer))));
//            }

//            if (search.ProductId != null)
//            {
//                productIdQuery =
//                    Query<SchemaRetailerProduct>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Product.Mpn).Query(search.ProductId))));
//            }

//            if (search.Country != null)
//            {
//                countryQuery =
//                    Query<SchemaRetailerProduct>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Retailer.Country.Name).Query(search.Country))));
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
//                ElasticClient.Search<SchemaRetailerProduct>(
//                    s =>
//                    s.Type(Type)
//                        .Query(
//                            q =>
//                            q.QueryString(
//                                x =>
//                                x.Fields(
//                                    fs =>
//                                    fs.Field(f => f.Description)
//                                        .Field(f => f.Sku)
//                                        .Field(f => f.MarketingText)
//                                        .Field(f => f.Product.Ean)
//                                        .Field(f => f.Product.Mpn)).Query(search.Keywords))
//                            && brandQuery && productIdQuery && countryQuery && retailerQuery)
//                        .From(search.Page.Value)
//                        .Size(search.PageSize.Value));

//            return response;
//        }
//    }
//}