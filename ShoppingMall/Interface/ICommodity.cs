using ShoppingMall.Models.Commodity;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface ICommodity
    {
        /// <summary>
        /// 取得所有商品資料
        /// </summary>
        List<CommodityDataDtoResponse> GetCommodityData(ConditionDataDto conditionData);

        /// <summary>
        /// 取得有開放的商品資料
        /// </summary>
        List<OpenCommodityData> GetOpenCommodityData();

        /// <summary>
        /// 新增商品資料
        /// </summary>
        bool InsertCommodityData(InsertCommodityDataDto insertData);

        /// <summary>
        /// 編輯商品資料
        /// </summary>
        bool UpdateCommodityData(UpdateCommodityDataDto updateData);

        /// <summary>
        /// 上傳商品圖片
        /// </summary>
        string UploadCommodityFile(HttpPostedFileBase files);

        /// <summary>
        /// 刪除舊的商品圖片檔
        /// </summary>
        void DeleteCommodityFile(string filePath);

        /// <summary>
        /// 檢查新增商品資料的傳入參數
        /// </summary>
        bool CheckInsertInputData(HttpRequestBase insertData);

        /// <summary>
        /// 檢查編輯商品資料的傳入參數
        /// </summary>
        bool CheckUpdateInputData(HttpRequestBase updateData);

        /// <summary>
        /// 取得商品管理頁面所需的選項
        /// </summary>
        List<CommodityOptionDataDtoResponse> GetAllCommodityOptionData();

        /// <summary>
        /// 取得庫存不足的商品資料
        /// </summary>
        List<CommodityStockDataDtoResponse> GetShortageCommodityData();
    }
}
