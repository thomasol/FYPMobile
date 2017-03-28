using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class LocalStore
    {
        public string Ean { get; set; }
        public int StoreCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string AreaCode { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string Stock { get; set; }
        public double? Price { get; set; }
        public Gps Gps { get; set; }
    }
}
