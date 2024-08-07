namespace ShoppingMall.Models.Commodity
{
    /// <summary>
    /// 商品資料
    /// </summary>
    public class CommodityStockDataDtoResponse
    {
        /// <summary>
        /// 庫存不足的商品數量
        /// </summary>
        public int InventoryShortageCount { get; set; }
    }

}