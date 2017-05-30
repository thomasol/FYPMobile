using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace FinalYearProject.Domain
{
    public class Event
    {
        public enum Types { Search, Impression, Click, Hit }
        [JsonConverter(typeof(StringEnumConverter))]
        public Types Type { get; set;}
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Url { get; set; }
        public string StoreCode { get; set; }
        public Guid SearchGUID { get; set; }
        public Guid ImpressionGUID { get; set; }
        public Guid ClickGUID { get; set; }
    }
}
