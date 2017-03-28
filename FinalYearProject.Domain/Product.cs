using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class Product
    {
        public int ModelId { get; set; }
        public string ModelNo { get; set; }
        public string Mpn { get; set; }
        public string Ean { get; set; }
        public string Upc { get; set; }
        public int BrandId { get; set; }
        public List<OnlineStore> OnlineStores { get; set; }
        public List<LocalStore> LocalStores { get; set; }
        public string Session { get; set; }
    }
    
}