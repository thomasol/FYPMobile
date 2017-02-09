using System;
using System.Collections.Generic;
using Nest;

namespace FinalYearProject.Search.BaseClasses
{
    public interface ISearchRepository<T> : IDisposable
        where T : class
    {
        ISearchResponse<T> GetAll(int page, int size);

        ISearchResponse<T> GetById(int id);

        ISearchResponse<T> GetUpdated(int page, int size);

        ISearchResponse<T> GetCreated(int page, int size);

        IIndexResponse Add(T entity);

        void IndexBulk(IEnumerable<T> items);

        IUpdateResponse<T> Update(T entity);

        void Delete(T entity);

        void DeleteIndex();

        void DeleteIndex(string index);
    }
}