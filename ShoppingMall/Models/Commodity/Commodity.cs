using System.Collections.Generic;

namespace ShoppingMall.Models.Commodity
{
    public class CommodityDataDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Open { get; set; }
    }
    public class InsertCommodityDataDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Open { get; set; }
        public string ImagePath { get; set; }
    }
}