using System.Collections.Generic;

namespace ShoppingMall.Models.Commodity
{
    /// <summary>
    /// 商品資料
    /// </summary>
    public class CommodityDataDtoResponse
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 圖片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 開啟狀態
        /// </summary>
        public int Open { get; set; }
        /// <summary>
        /// 商品類型名字
        /// </summary>
        public string CommodityName { get; set; }
    }

    /// <summary>
    /// 篩選條件
    /// </summary>
    public class ConditionDataDto
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public int Type { set; get; }
    }

    /// <summary>
    /// 新增商品傳入資料
    /// </summary>
    public class InsertCommodityDataDto
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 開啟狀態
        /// </summary>
        public int Open { get; set; }
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }
    }

    /// <summary>
    /// 編輯商品傳入資料
    /// </summary>
    public class UpdateCommodityDataDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int CommodityId { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 開啟狀態
        /// </summary>
        public int Open { get; set; }
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }
    }
}