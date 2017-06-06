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
        public Guid SearchGuid { get; set; }
        public Guid ImpressionGuid { get; set; }
        public Guid ClickGuid { get; set; }
        public int StoreCode { get; set; }
    }
}
