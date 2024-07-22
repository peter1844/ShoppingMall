using ShoppingMall.App_Code;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityProccess : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得所有商品資料
        /// </summary>
        public List<CommodityDataDtoResponse> GetCommodityData(ConditionDataDto conditionData)
        {
            List<CommodityDataDtoResponse> commodityData = new List<CommodityDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getCommodityData @name,@type";

                command.Parameters.AddWithValue("@name", conditionData.Name);
                command.Parameters.AddWithValue("@type", conditionData.Type);

                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        commodityData.Add(new CommodityDataDtoResponse
                        {
                            Id = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            Name = dt.Rows[i]["f_name"].ToString(),
                            Description = dt.Rows[i]["f_description"].ToString(),
                            Type = Convert.ToInt32(dt.Rows[i]["f_typeId"]),
                            Image = dt.Rows[i]["f_image"].ToString(),
                            Price = Convert.ToInt32(dt.Rows[i]["f_price"]),
                            Stock = Convert.ToInt32(dt.Rows[i]["f_stock"]),
                            Open = Convert.ToInt32(dt.Rows[i]["f_open"]),
                            CommodityName = dt.Rows[i]["CommodityName"].ToString()
                        });
                    }
                }

                return commodityData;
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

        /// <summary>
        /// 新增商品資料
        /// </summary>
        public bool InsertCommodityData(InsertCommodityDataDto insertData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_insertCommodityData @name,@description,@type,@image,@price,@stock,@open";

                command.Parameters.AddWithValue("@name", insertData.Name);
                command.Parameters.AddWithValue("@description", insertData.Description);
                command.Parameters.AddWithValue("@type", insertData.Type);
                command.Parameters.AddWithValue("@image", insertData.ImagePath);
                command.Parameters.AddWithValue("@price", insertData.Price);
                command.Parameters.AddWithValue("@stock", insertData.Stock);
                command.Parameters.AddWithValue("@open", insertData.Open);

                command.Connection.Open();
                command.ExecuteNonQuery();

                return true;
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

        /// <summary>
        /// 編輯商品資料
        /// </summary>
        public bool UpdateCommodityData(UpdateCommodityDataDto updateData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_updateCommodityData @commodityId,@name,@description,@type,@image,@price,@stock,@open";

                command.Parameters.AddWithValue("@commodityId", updateData.CommodityId);
                command.Parameters.AddWithValue("@name", updateData.Name);
                command.Parameters.AddWithValue("@description", updateData.Description);
                command.Parameters.AddWithValue("@type", updateData.Type);
                command.Parameters.AddWithValue("@image", updateData.ImagePath);
                command.Parameters.AddWithValue("@price", updateData.Price);
                command.Parameters.AddWithValue("@stock", updateData.Stock);
                command.Parameters.AddWithValue("@open", updateData.Open);

                command.Connection.Open();
                command.ExecuteNonQuery();

                return true;
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

        /// <summary>
        /// 檢查新增商品資料的傳入參數
        /// </summary>
        public bool CheckInsertInputData(HttpRequest insertData)
        {
            string rule = @"^[1-9]\d*$";
            string[] allowedExtensions = { "image/jpeg", "image/png", "image/gif" };

            // 檢查商品名稱、類型、價格、庫存量、開放狀態是否為空
            if (string.IsNullOrEmpty(insertData.Form["Name"]) || string.IsNullOrEmpty(insertData.Form["Type"]) || string.IsNullOrEmpty(insertData.Form["Price"]) || string.IsNullOrEmpty(insertData.Form["Stock"]) || string.IsNullOrEmpty(insertData.Form["Open"])) return false;
            // 檢查開放狀態的參數是否合法
            if (Convert.ToInt32(insertData.Form["Open"]) < 0 || Convert.ToInt32(insertData.Form["Open"]) > 1) return false;
            // 檢查商品名稱、描述的長度是否正確
            if (insertData.Form["Name"].Length > 50 || insertData.Form["Description"].Length > 200) return false;
            // 檢查價格、庫存量格式是否合法
            if (!Regex.IsMatch(insertData.Form["Price"], rule) || !Regex.IsMatch(insertData.Form["Stock"], rule)) return false;
            // 檢查檔案上傳類型是否合法
            if (insertData.Files.Count > 0 && !allowedExtensions.Contains(insertData.Files[0].ContentType)) return false;

            return true;
        }

        /// <summary>
        /// 檢查編輯商品資料的傳入參數
        /// </summary>
        public bool CheckUpdateInputData(HttpRequest updateData)
        {
            string rule = @"^[1-9]\d*$";
            string[] allowedExtensions = { "image/jpeg", "image/png", "image/gif" };

            // 檢查商品名稱、類型、價格、庫存量、開放狀態是否為空
            if (string.IsNullOrEmpty(updateData.Form["Name"]) || string.IsNullOrEmpty(updateData.Form["Type"]) || string.IsNullOrEmpty(updateData.Form["Price"]) || string.IsNullOrEmpty(updateData.Form["Stock"]) || string.IsNullOrEmpty(updateData.Form["Open"])) return false;
            // 檢查開放狀態的參數是否合法
            if (Convert.ToInt32(updateData.Form["Open"]) < 0 || Convert.ToInt32(updateData.Form["Open"]) > 1) return false;
            // 檢查商品名稱、描述的長度是否正確
            if (updateData.Form["Name"].Length > 50 || updateData.Form["Description"].Length > 200) return false;
            // 檢查價格、庫存量格式是否合法
            if (!Regex.IsMatch(updateData.Form["Price"], rule) || !Regex.IsMatch(updateData.Form["Stock"], rule)) return false;
            // 檢查檔案上傳類型是否合法
            if (updateData.Files.Count > 0 && !allowedExtensions.Contains(updateData.Files[0].ContentType)) return false;

            return true;
        }
    }
}
