using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityOption : ShoppingMall.Base.Base
    {
        public List<CommodityOptionDataDtoResponse> GetAllCommodityOptionData()
        {
            List<CommodityOptionDataDtoResponse> commodityOptionData = new List<CommodityOptionDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getAllCommodityOptionData";
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt); 

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        commodityOptionData.Add(new CommodityOptionDataDtoResponse
                        {
                            CommodityId = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            CommodityName = dt.Rows[i]["f_name"].ToString(),
                        });
                    }
                }

                return commodityOptionData;
            }
            catch (Exception ex)
            {
                throw new Exception(StateCode.DbError.ToString());
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
    }
}
