using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class Event
    {
        public int EventId { get; set; }
        public string Description { get; set; }
        public enum Type { Search, Impression, Click, Hit }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public Guid GUID { get; set; }
    }
}
