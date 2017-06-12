using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class FypSearchRequest
    {
        public string UserName { get; set; }
        public string ProductId { get; set; }
        public string SearchTerm { get; set; }
        public int Distance { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
