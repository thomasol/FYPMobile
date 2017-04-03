using System;
using System.Collections.Generic;
using System.Configuration;
using FinalYearProject.Domain;
using FinalYearProject.Search.Client;
using Nest;

namespace FinalYearProject.Search.BaseClasses
{
    public class SearchRepository<T> : ISearchRepository<T>
        where T : class
    {
        #region lazyLoads

        private ElasticClientWrapper elasticClient;

        public ElasticClientWrapper ElasticClient
        {
            get
            {
                return elasticClient ?? (elasticClient = new ElasticClientWrapper(Url));
            }
            set
            {
                elasticClient = value;
            }
        }

        private string environment;
        private string url;

        public string Environment
        {
            get
            {
                return environment ?? (environment = ConfigurationManager.AppSettings["Env"]);
            }
            set
            {
                environment = value;
            }
        }

        public string Url
        {
            get
            {
                return url ?? (url = ConfigurationManager.AppSettings["Search"]);
            }
            set
            {
                url = value;
            }
        }

        #endregion lazyLoads

        public readonly string Type;

        public readonly string Index;

        public SearchRepository(string type, string index)
        {
            Type = type;
            Index = Environment + "-" + index;
        }

        public virtual IIndexResponse Add(T entity)
        {
            var response = ElasticClient.Index(entity, i => i.Index(Index).Refresh());
            return response;
        }

        public IPutMappingResponse AddMappings(T entity)
        {
            return ElasticClient.Map<T>(x => x.AutoMap(2));
        }

        public virtual void IndexBulk(IEnumerable<T> entities)
        {
            var response = ElasticClient.IndexManyAsync(entities, Index);
        }

        public virtual ISearchResponse<T> GetAll(int page, int size)
        {
            var response = ElasticClient.Search<T>(s => s.Index(Index).Type(Type).From(page).Size(size));
            return response;
        }

        public virtual ISearchResponse<T> GetById(int id)
        {
            ISearchResponse<T> response =
                ElasticClient.Search<T>(s => s.Index(Index).Type(Type).Query(query => query.Term("_id", id)));
            return response;
        }

        public virtual ISearchResponse<T> GetById(string id)
        {
            ISearchResponse<T> response =
                ElasticClient.Search<T>(s => s.Index(Index).Type(Type).Query(query => query.Term("id", id)));
            return response;
        }

        public virtual ISearchResponse<T> GetUpdated(int page, int size)
        {
            var response =
                ElasticClient.Search<T>(
                    s =>
                    s.Index(Index)
                        .Type(Type)
                        .From(page)
                        .Size(size)
                        .Query(
                            q =>
                            q.DateRange(
                                r =>
                                r.GreaterThanOrEquals(DateMath.Now.Subtract(TimeSpan.FromDays(1)))
                                    .Format("MM/dd/yyyy")
                                    .Field("updatedAt"))));
            return response;
        }

        public virtual ISearchResponse<T> GetCreated(int page, int size)
        {
            var response =
                ElasticClient.Search<T>(
                    s =>
                    s.Index(Index)
                        .Type(Type)
                        .From(page)
                        .Size(size)
                        .Query(
                            q =>
                            q.DateRange(
                                r =>
                                r.GreaterThanOrEquals(DateMath.Now.Subtract(TimeSpan.FromDays(1)))
                                    .Format("MM/dd/yyyy")
                                    .Field("createdAt"))));
            return response;
        }

        public IUpdateResponse<T> Update(T entity)
        {
            var response = ElasticClient.Update<T>(entity, u => u.Index(Index).Type(Type));
            return response;
        }

        public virtual void Delete(T entity)
        {
            ElasticClient.Delete<T>(entity, u => u.Index(Index).Type(Type));
        }

        public void DeleteIndex()
        {
            ElasticClient.DeleteIndex(Index);
        }

        public void DeleteIndex(string index)
        {
            ElasticClient.DeleteIndex(index);
        }

        public ISearchResponse<dynamic> SearchAll(string query, int page, int size, string type)
        {
            var response =
                ElasticClient.Search<object>(
                    s =>
                    s.AllIndices()
                        .Type(Types.Type(typeof(Event)))
                        .From(page * size)
                        .Take(size)
                        .Query(
                            qry => qry.Bool(b => b.Must(m => m.QueryString(qs => qs.DefaultField("_all").Query(query))))));

            return response;
        }

        public bool Existing()
        {
            if (ElasticClient.IndexExists(Index).Exists)
            {
                return true;
            }
            ElasticClient.CreateIndex(Index);
            return false;
        }

        public void Dispose()
        {
            ElasticClient.ConnectionSettings.Dispose();
        }
    }
}