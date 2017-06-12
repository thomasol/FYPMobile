using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class OnlineStore
    {
        public int StoreCode { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Stock { get; set; }
        public double? Price { get; set; }
        public string Url { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public string BrandLogo { get; set; }
        public string ImageUrl { get; set; }
        public string MPN { get; set; }
        public string Retailer { get; set; }
        public string RetailerLogo { get; set; }
        public string Sku { get; set; }
        public string UPC { get; set; }
    }
}
