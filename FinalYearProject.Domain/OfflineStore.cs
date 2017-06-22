using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class OfflineStore
    {
        public string Ean { get; set; }
        public int StoreCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AreaCode { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string Stock { get; set; }
        public double? Price { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public string BrandLogo { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string ImageUrl { get; set; }
        public string Mpn { get; set; }
        public string Retailer { get; set; }
        public string RetailerLogo { get; set; }
        public string Sku { get; set; }
        public string Upc { get; set; }
        public Gps StoreCoordinates { get; set; }
    }
}
