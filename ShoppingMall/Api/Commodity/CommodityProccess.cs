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
        public List<CommodityDataDtoResponse> GetAllCommodityData()
        {
            List<CommodityDataDtoResponse> commodityData = new List<CommodityDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getAllCommodityData";
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
        public bool CheckInsertInputData(HttpRequest insertData)
        {
            string rule = @"^[1-9]\d*$";
            string[] allowedExtensions = { "image/jpeg", "image/png", "image/gif" };

            if (string.IsNullOrEmpty(insertData.Form["Name"]) || string.IsNullOrEmpty(insertData.Form["Description"]) || string.IsNullOrEmpty(insertData.Form["Type"]) || string.IsNullOrEmpty(insertData.Form["Price"]) || string.IsNullOrEmpty(insertData.Form["Stock"]) || string.IsNullOrEmpty(insertData.Form["Open"])) return false;
            if (Convert.ToInt32(insertData.Form["Open"]) < 0 || Convert.ToInt32(insertData.Form["Open"]) > 1) return false;
            if (insertData.Form["Name"].Length > 50 || insertData.Form["Description"].Length > 200) return false;
            if (!Regex.IsMatch(insertData.Form["Price"], rule) || !Regex.IsMatch(insertData.Form["Stock"], rule)) return false;
            if (insertData.Files.Count > 0 && !allowedExtensions.Contains(insertData.Files[0].ContentType)) return false;

            return true;
        }
    }
}
