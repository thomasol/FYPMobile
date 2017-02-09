using System;
using Nest;

namespace FinalYearProject.Search.Client
{
    
    public class ElasticClientWrapper : ElasticClient
    {
        public ElasticClientWrapper(string url) : base(GetConnectionSettings(url)) { }

        public static ConnectionSettings GetConnectionSettings(string url)
        {
            var node = new Uri(url);
           return new ConnectionSettings(
                node
            );
        }
    }
}
