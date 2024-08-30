using ShoppingMall.Interface;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingMall.Api.Commodity
{
    public class CommodityProccess : ICommodity
    {
        private IContextHelper _contextHelper;
        private IDbHelper _dbHelper;
        private ITools _tools;

        public CommodityProccess(IContextHelper contextHelper, IDbHelper dbHelper, ITools tools)
        {
            _contextHelper = contextHelper;
            _dbHelper = dbHelper;
            _tools = tools;
        }

        /// <summary>
        /// 取得所有商品資料
        /// </summary>
        public List<CommodityDataDtoResponse> GetCommodityData(ConditionDataDto conditionData)
        {
            List<CommodityDataDtoResponse> commodityData = new List<CommodityDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

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
                            Image = string.IsNullOrEmpty(dt.Rows[i]["f_image"].ToString()) ? "" : $"/images/commodity/{dt.Rows[i]["f_image"].ToString()}",
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
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }

        /// <summary>
        /// 取得有開放的商品資料
        /// </summary>
        public List<OpenCommodityData> GetOpenCommodityData()
        {
            List<OpenCommodityData> commodityData = new List<OpenCommodityData>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getOpenCommodityData";

                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        commodityData.Add(new OpenCommodityData
                        {
                            CommodityId = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            CommodityName = dt.Rows[i]["f_name"].ToString(),
                            CommodityPrice = Convert.ToInt32(dt.Rows[i]["f_price"])
                        });
                    }
                }

                return commodityData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_addCommodityData @name,@description,@type,@image,@price,@stock,@open,@adminId,@permission";

                command.Parameters.AddWithValue("@name", insertData.Name);
                command.Parameters.AddWithValue("@description", insertData.Description);
                command.Parameters.AddWithValue("@type", insertData.Type);
                command.Parameters.AddWithValue("@image", insertData.ImagePath);
                command.Parameters.AddWithValue("@price", insertData.Price);
                command.Parameters.AddWithValue("@stock", insertData.Stock);
                command.Parameters.AddWithValue("@open", insertData.Open);
                command.Parameters.AddWithValue("@adminId", Convert.ToInt32(_contextHelper.GetContext().Session["id"]));
                command.Parameters.AddWithValue("@permission", Permissions.CommodityInsert);

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                // 權限不足
                if (statusMessage == (int)StateCode.NoPermission) throw new Exception(StateCode.NoPermission.ToString());
                // DB執行錯誤
                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_editCommodityData @commodityId,@name,@description,@type,@image,@price,@stock,@open,@adminId,@permission";

                command.Parameters.AddWithValue("@commodityId", updateData.CommodityId);
                command.Parameters.AddWithValue("@name", updateData.Name);
                command.Parameters.AddWithValue("@description", updateData.Description);
                command.Parameters.AddWithValue("@type", updateData.Type);
                command.Parameters.AddWithValue("@image", updateData.ImagePath);
                command.Parameters.AddWithValue("@price", updateData.Price);
                command.Parameters.AddWithValue("@stock", updateData.Stock);
                command.Parameters.AddWithValue("@open", updateData.Open);
                command.Parameters.AddWithValue("@adminId", Convert.ToInt32(_contextHelper.GetContext().Session["id"]));
                command.Parameters.AddWithValue("@permission", Permissions.CommodityUpdate);

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                // 權限不足
                if (statusMessage == (int)StateCode.NoPermission) throw new Exception(StateCode.NoPermission.ToString());
                // DB執行錯誤
                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }

        }

        /// <summary>
        /// 上傳商品圖片
        /// </summary>
        public string UploadCommodityFile(HttpPostedFileBase files)
        {
            string filePath = "";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string[] filesInfo = files.FileName.Split('.');

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images/commodity/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images/commodity"));
            }

            files.SaveAs(HttpContext.Current.Server.MapPath($"~/images/commodity/{timestamp}_{_contextHelper.GetContext().Session["id"]}.{filesInfo[filesInfo.Length - 1]}"));
            filePath = $"{timestamp}_{_contextHelper.GetContext().Session["id"]}.{filesInfo[filesInfo.Length - 1]}";

            return filePath;
        }

        /// <summary>
        /// 刪除舊的商品圖片檔
        /// </summary>
        public void DeleteCommodityFile(string filePath)
        {
            string realPath = _contextHelper.GetContext().Server.MapPath($"~{filePath}");

            if (File.Exists(realPath))
            {
                File.Delete(realPath);
            }
        }

        /// <summary>
        /// 檢查新增商品資料的傳入參數
        /// </summary>
        public bool CheckInsertInputData(HttpRequestBase insertData)
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
            // 檢查檔案大小是否超過300KB
            if (insertData.Files.Count > 0 && insertData.Files[0].ContentLength > 1024 * 300) return false;

            return true;
        }

        /// <summary>
        /// 檢查編輯商品資料的傳入參數
        /// </summary>
        public bool CheckUpdateInputData(HttpRequestBase updateData)
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
            // 檢查檔案大小是否超過300KB
            if (updateData.Files.Count > 0 && updateData.Files[0].ContentLength > 1024 * 300) return false;

            return true;
        }

        /// <summary>
        /// 取得商品管理頁面所需的選項
        /// </summary>
        public List<CommodityOptionDataDtoResponse> GetAllCommodityOptionData()
        {
            List<CommodityOptionDataDtoResponse> commodityOptionData = new List<CommodityOptionDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

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
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }

        /// <summary>
        /// 取得庫存不足的商品資料
        /// </summary>
        public List<CommodityStockDataDtoResponse> GetShortageCommodityData()
        {
            List<CommodityStockDataDtoResponse> commodityShortageData = new List<CommodityStockDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

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
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
    }
}
