using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Commodity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityStockCheck
    {
        /// <summary>
        /// 取得庫存不足的商品資料
        /// </summary>
        public List<CommodityStockDataDtoResponse> GetShortageCommodityData()
        {
            List<CommodityStockDataDtoResponse> commodityShortageData = new List<CommodityStockDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = DbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getShortageCommodityData";

                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        commodityShortageData.Add(new CommodityStockDataDtoResponse
                        {
                            InventoryShortageCount = Convert.ToInt32(dt.Rows[i]["CNT"])
                        });
                    }
                }

                return commodityShortageData;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString(), ex);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
    }
}
