//using ChannelAdvisor.WhereToBuy.Domain.Api.Search;
//using ChannelAdvisor.WhereToBuy.Domain.Mapping.Schema;
//using ChannelAdvisor.WhereToBuy.Infra.Search.BaseClasses;
//using Nest;
//using System.Collections.Generic;

//namespace ChannelAdvisor.WhereToBuy.Infra.Search
//{
//    public class LocationSearchRepository : SearchRepository<SchemaRetailerProduct>
//    {
//        public LocationSearchRepository(string type, string index)
//            //: base(type, index)
//        {
//        }

//        public ISearchResponse<SchemaRetailerProductLocation> Search(OnlineLocationSearchRequest search)
//        {
//            QueryContainer productIdQuery = new QueryContainer();
//            QueryContainer storeQuery = new QueryContainer();
//            QueryContainer geoQuery = new QueryContainer();
//            var unit = DistanceUnit.Miles;

//            if (search.ProductId != null)
//            {
//                productIdQuery =
//                    Query<SchemaRetailerProductLocation>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Product.Gtin).Query(search.ProductId))));
//            }

//            if (search.StoreId != null)
//            {
//                storeQuery =
//                    Query<SchemaRetailerProductLocation>.Bool(
//                        x =>
//                        x.Must(m => m.Term(
//                                descriptor => descriptor.Field(fz => fz.Location.StoreId).Value(search.StoreId.Value))));
//            }

//            if (search.Lat != null && search.Lon != null && search.Distance != null)
//            {
//                if (search.DistanceType.ToLower().Equals("km"))
//                {
//                    unit = DistanceUnit.Kilometers;
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce(true)
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Kilometers(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("yd"))
//                {
//                    unit = DistanceUnit.Yards;
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce(true)
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Miles(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("cm"))
//                {
//                    unit = DistanceUnit.Centimeters;
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce(true)
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Centimeters(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("inch"))
//                {
//                    unit = DistanceUnit.Inch;
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce(true)
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Inches(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("nm"))
//                {
//                    unit = DistanceUnit.NauticalMiles;
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce(true)
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.NauticalMiles(search.Distance.Value)));
//                }
//                else
//                {
//                    geoQuery = Query<SchemaRetailerProductLocation>.GeoDistance(
//                        x =>
//                        x.Field(y => y.Location.StoreCoordinates)
//                        .DistanceType(GeoDistanceType.Arc)
//                        .Name("Distance")
//                        .Coerce(true)
//                        .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                        .Distance(Distance.Miles(search.Distance.Value)));
//                }
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
//                ElasticClient.Search<SchemaRetailerProductLocation>(
//                    s =>
//                    s.Type(Type)
//                        .Query(
//                            q => geoQuery && productIdQuery && storeQuery)

//                        .From(search.Page.Value)
//                        .Size(search.PageSize.Value)
//                        .Sort(x => x.GeoDistance(
//                            y => y.Ascending().Unit(unit).PinTo(new GeoLocation(search.Lat.Value, search.Lon.Value)).DistanceType(GeoDistanceType.Arc).Field(b => b.Location.StoreCoordinates)))

//                        );

//            return response;
//        }

//        public void CreateMap()
//        {
//            var res = ElasticClient.Map<SchemaRetailerProductLocation>(x => x.Index(Index).AutoMap(4)
//            .Properties(p => p
//                .GeoPoint(g => g.Name(n => n.Location.StoreCoordinates).LatLon(true))
//              )
//            );
//        }

//        public void CreateIndex()
//        {
//            var res = ElasticClient.CreateIndex(Index);
//        }

//        public void IndexBulkItems(List<SchemaRetailerProductLocation> entities, int size)
//        {
//            var response = ElasticClient.IndexMany(entities, Index);
//        }
//    }
//}