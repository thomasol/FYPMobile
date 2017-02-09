//using ChannelAdvisor.WhereToBuy.Domain.Api.Search;
//using ChannelAdvisor.WhereToBuy.Domain.Mapping.Schema;
//using ChannelAdvisor.WhereToBuy.Infra.Search.BaseClasses;
//using Nest;

//namespace ChannelAdvisor.WhereToBuy.Infra.Search
//{
//    public class CountryProductSearchRepository : SearchRepository<SchemaCountryProduct>
//    {
//        public CountryProductSearchRepository(string type, string index)
//            : base(type, index)
//        {
//        }

//        public ISearchResponse<SchemaCountryProduct> Search(OnlineProductSearchRequest search)
//            //If we are not including dismissed, define a filter which
//            //removes all dismissed alerts, otherwise leave blank
//            QueryContainer brandQuery = new QueryContainer();
//            QueryContainer productIdQuery = new QueryContainer();
//            QueryContainer countryQuery = new QueryContainer();
//            if (search.Brand != null)
//            {
//                brandQuery =
//                    Query<SchemaCountryProduct>.Bool(
//                        x =>
//                        x.Must(
//                            m =>
//                            m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Product.Brand.Description).Query(search.Brand))));
//            }

//            if (search.ProductId != null)
//            {
//                productIdQuery =
//                    Query<SchemaCountryProduct>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Product.Model).Query(search.ProductId))));
//            }

//            if (search.Country != null)
//            {
//                countryQuery =
//                    Query<SchemaCountryProduct>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Country.Name).Query(search.Country))));
//            }

//            if (!search.PageSize.HasValue)
//            {
//                search.PageSize = 24;
//            }

//            if (!search.Page.HasValue)
//            {
//                search.Page = 0;
//            }

//            var response =
//                ElasticClient.Search<SchemaCountryProduct>(
//                    s =>
//                    s.Type(Type)
//                    .Index(Index)
//                        .Query(
//                            q =>
//                            q.QueryString(
//                                x =>
//                                x.Fields(
//                                    fs =>
//                                    fs.Field(f => f.Description)
//                                        .Field(f => f.MarketingText)
//                                        .Field(f => f.Product.MarketingText).Field(f => f.Product.Brand.Description)
//                                        .Field(f => f.Product.Model)
//                                        .Field(f => f.Product.Gtin)
//                                        .Field(f => f.Product.Ean)
//                                        .Field(f => f.Url)
//                                        .Field(f => f.Product.Url)
//                                        .Field(f => f.Product.Upc)
//                                        .Field(f => f.Product.Mpn)).Query(search.Keywords))
//                            && brandQuery && productIdQuery && countryQuery)
//                        .From(search.Page.Value)
//                        .Size(search.PageSize.Value));

//            return response;
//        }
//    }
//}