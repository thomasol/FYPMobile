//using ChannelAdvisor.WhereToBuy.Domain.Api.Search;
//using ChannelAdvisor.WhereToBuy.Domain.Search;
//using ChannelAdvisor.WhereToBuy.Infra.Search.BaseClasses;
//using Nest;
//using System.Collections.Generic;
//using System.Linq;

//namespace ChannelAdvisor.WhereToBuy.Infra.Search
//{
//    public class OfferLocationSearchRepository : SearchRepository<OfferLocation>
//    {
//        //public OfferLocationSearchRepository(string type, string index)
//            : base(type, index)
//        {
//        }

//        public ISearchResponse<Product> Search(OnlineLocationSearchRequest search)
//        {
//            QueryContainer productIdQuery = new QueryContainer();
//            QueryContainer storeQuery = new QueryContainer();
//            QueryContainer geoQuery = new QueryContainer();
//            var unit = DistanceUnit.Miles;

//            if (search.ProductId != null)
//            {
//                productIdQuery =
//                    Query<Product>.Bool(
//                        x =>
//                        x.Must(m => m.MatchPhrase(
//                                descriptor => descriptor.Field(fz => fz.Model).Query(search.ProductId))));
//            }

//            if (search.StoreId != null)
//            {
//                storeQuery =
//                    Query<OfferLocation>.Bool(
//                        x =>
//                        x.Must(m => m.Term(
//                                descriptor => descriptor.Field(fz => fz.Location.StoreId).Value(search.StoreId.Value))));
//            }

//            if (search.Lat != null && search.Lon != null && search.Distance != null)
//            {
//                if (search.DistanceType.ToLower().Equals("km"))
//                {
//                    unit = DistanceUnit.Kilometers;
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce()
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Kilometers(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("yd"))
//                {
//                    unit = DistanceUnit.Yards;
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce()
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Miles(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("cm"))
//                {
//                    unit = DistanceUnit.Centimeters;
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce()
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Centimeters(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("inch"))
//                {
//                    unit = DistanceUnit.Inch;
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce()
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.Inches(search.Distance.Value)));
//                }
//                else if (search.DistanceType.ToLower().Equals("nm"))
//                {
//                    unit = DistanceUnit.NauticalMiles;
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                    x =>
//                    x.Field(y => y.Location.StoreCoordinates)
//                    .DistanceType(GeoDistanceType.Arc)
//                    .Name("Distance")
//                    .Coerce()
//                    .Location(new GeoLocation(search.Lat.Value, search.Lon.Value))
//                    .Distance(Distance.NauticalMiles(search.Distance.Value)));
//                }
//                else
//                {
//                    geoQuery = Query<OfferLocation>.GeoDistance(
//                        x =>
//                        x.Field(y => y.Location.StoreCoordinates)
//                        .DistanceType(GeoDistanceType.Arc)
//                        .Name("Distance")
//                        .Coerce()
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
//                ElasticClient.Search<Product>(
//                    s =>
//                    s.Type(Type)
//                        .Query(q => productIdQuery && storeQuery && q.Nested(n => n
//                        .Path(p => p.OfferLocations).Query(nq => geoQuery)))
//                        .From(search.Page.Value)
//                        .Size(search.PageSize.Value)
//                        .Sort(x => x.GeoDistance(y => y.NestedPath(p => p.OfferLocations).Ascending().Unit(unit).PinTo(new GeoLocation(search.Lat.Value, search.Lon.Value)).DistanceType(GeoDistanceType.Arc).Field(p => p.OfferLocations.Select(ol => ol.Location.StoreCoordinates))))

//                        );

//            return response;
//        }

//        public ISearchResponse<Location> GetByRetailer(int retailerId, int page, int size)
//        {
//            var idQuery = Query<Location>.Term(
//                        descriptor => descriptor.Field(fz => fz.RetailerId).Value(retailerId.ToString()));

//            var response =
//                ElasticClient.Search<Location>(
//                    s =>
//                    s.Index(Index).Type(Types.Type<Location>())

//                        .Query(
//                            q => idQuery)
//                        .From(page)
//                        .Size(size)
//                        .Sort(x => x.Descending(y => y.Id))

//                        );

//            return response;
//        }

//        public void CreateMap()
//        {
//            var res = ElasticClient.Map<OfferLocation>(x => x.Index(Index).AutoMap(4)
//            .Properties(p => p
//                .GeoPoint(g => g.Name(n => n.Location.StoreCoordinates).LatLon())
//              )
//            );
//        }

//        public void CreateLocationMap()
//        {
//            ElasticClient.CreateIndex(Index);
//            var res = ElasticClient.Map<Location>(x => x.Index(Index).AutoMap(4)
//            .Properties(p => p
//                .GeoPoint(g => g.Name(n => n.StoreCoordinates).LatLon())
//              )
//            );
//        }

//        public void CreateProductMap()
//        {
//            var res = ElasticClient.Map<Product>(x => x.Index(Index)
//                .Properties(p => p.Nested<OfferLocation>(n => n.Name(y => y.OfferLocations).IncludeInParent().Properties(pl => pl.GeoPoint(g => g.Name(y => y.Location.StoreCoordinates).LatLon()))))
//                .Properties(p => p.Nested<Offer>(n => n.Name(c => c.Offers).IncludeInParent())));
//        }

//        public void IndexBulkItems(List<OfferLocation> entities)
//        {
//            var response = ElasticClient.IndexMany(entities, Index);
//        }

//        public void IndexLocationItems(List<Location> entities)
//        {
//            var response = ElasticClient.IndexMany(entities, Index);
//        }

//        public void IndexSearchProductItems(List<Product> entities)
//        {
//            var response = ElasticClient.IndexMany(entities, Index);
//        }
//    }
//}