using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Domain
{
    public class Location
    {
        public int ModelId { get; set; }
        public string ModelNo { get; set; }
        public string Mpn { get; set; }
        public int BrandId { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Store> Stores { get; set; }
        public string Session { get; set; }
    }

    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Locale { get; set; }
    }

    public class Store
    {
        public int StoreCode { get; set; }
        public string Name { get; set; }
        public Description Description { get; set; }
        public int LocationId { get; set; }
        public string Address { get; set; }
        public string AreaCode { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string CurrencySymbol { get; set; }
        public string Currency { get; set; }
        public string Stock { get; set; }
        public double? PriceInc { get; set; }
        public double? PriceEx { get; set; }
        public int Type { get; set; }
        public Gps Gps { get; set; }
        public List<Storeoption> StoreOptions { get; set; }
        public List<Productoption> ProductOptions { get; set; }
        public List<Storelocationoption> StoreLocationOptions { get; set; }
        public List<Storecategory> StoreCategories { get; set; }
        public string Session { get; set; }
        public string TrackingUrl { get; set; }
    }

    public class Description
    {
    }

    public class Gps
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? Distance { get; set; }
    }

    public class Storeoption
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Locale { get; set; }
    }

    public class Productoption
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Locale { get; set; }
    }

    public class Storelocationoption
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Locale { get; set; }
    }

    public class Storecategory
    {
        public int BrandCategoryId { get; set; }
        public int OrderNumber { get; set; }
        public string CategoryName { get; set; }
    }

}
