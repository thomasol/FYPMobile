using System.Collections.Generic;

namespace FinalYearProject.Domain
{
    public class Product
    {
        public string Description { get; set; }
        public int ModelId { get; set; }
        public string ModelNo { get; set; }
        public string Mpn { get; set; }
        public string Ean { get; set; }
        public string Upc { get; set; }
        public int BrandId { get; set; }
        public List<OnlineStore> OnlineStores { get; set; }
        public List<OfflineStore> LocalStores { get; set; }
        public string Session { get; set; }
    }
    
}