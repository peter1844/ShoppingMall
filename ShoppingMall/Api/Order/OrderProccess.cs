using ShoppingMall.App_Code;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using System.Web.UI.WebControls;

namespace ShoppingMall.Api.Order
{
    public class OrderProccess : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得訂單資料
        /// </summary>
        public List<OrderDataDtoResponse> GetOrderData(ConditionDataDto conditionData)
        {
            List<OrderDataDtoResponse> orderData = new List<OrderDataDtoResponse>();
            List<OrderDataDtoResponse> groupOrderData = new List<OrderDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getOrderData @id,@startDate,@endDate,@deliveryState";

                command.Parameters.AddWithValue("@id", conditionData.Id);
                command.Parameters.AddWithValue("@deliveryState", conditionData.DeliveryState);

                if (conditionData.StartDate.HasValue && conditionData.EndDate.HasValue)
                {
                    command.Parameters.AddWithValue("@startDate", conditionData.StartDate);
                    command.Parameters.AddWithValue("@endDate", conditionData.EndDate);
                }
                else 
                {
                    command.Parameters.AddWithValue("@startDate", DBNull.Value);
                    command.Parameters.AddWithValue("@endDate", DBNull.Value);
                }

                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        orderData.Add(new OrderDataDtoResponse
                        {
                            Id = dt.Rows[i]["f_id"].ToString(),
                            MemberName = dt.Rows[i]["memberName"].ToString(),
                            OrderDate = (DateTime)dt.Rows[i]["f_date"],
                            PayTypeId = Convert.ToInt32(dt.Rows[i]["f_payType"]),
                            PayStateId = Convert.ToInt32(dt.Rows[i]["f_payState"]),
                            DeliverTypeId = Convert.ToInt32(dt.Rows[i]["f_deliverType"]),
                            DeliverStateId = Convert.ToInt32(dt.Rows[i]["f_deliverState"]),
                            DeliverStateName = Enum.GetName(typeof(DeliveryStateCode), Convert.ToInt32(dt.Rows[i]["f_deliverState"])),
                            TotalMoney = Convert.ToInt32(dt.Rows[i]["f_totalMoney"]),
                            DetailDatas = new List<OrderDetailData>{ new OrderDetailData
                                {
                                    CommodityName = dt.Rows[i]["CommodityName"].ToString(),
                                    Quantity = Convert.ToInt32(dt.Rows[i]["f_quantity"]),
                                    Price = Convert.ToInt32(dt.Rows[i]["f_price"]),
                                    Image = string.IsNullOrEmpty(dt.Rows[i]["f_image"].ToString()) ? "" : $"/images/commodity/{dt.Rows[i]["f_image"].ToString()}",
                                }
                            }
                        });
                    }

                    groupOrderData = orderData
                        .GroupBy(u => u.Id)
                        .Select(g => new OrderDataDtoResponse
                        {
                            Id = g.First().Id,
                            MemberName = g.First().MemberName,
                            OrderDate = g.First().OrderDate,
                            PayTypeId = g.First().PayTypeId,
                            PayStateId = g.First().PayStateId,
                            DeliverTypeId = g.First().DeliverTypeId,
                            DeliverStateId = g.First().DeliverStateId,
                            DeliverStateName = g.First().DeliverStateName,
                            TotalMoney = g.First().TotalMoney,
                            DetailDatas = g.SelectMany(u => u.DetailDatas).ToList()
                        }).ToList();
                }

                return groupOrderData;
            }
            catch (Exception ex)
            {
                //throw new Exception(StateCode.DbError.ToString(), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        public bool InsertOrderData(InsertOrderDataDto insertData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                DataTable tempTable = new DataTable();
                DateTime now = DateTime.Now;
                string orderId = $"T{now.ToString("yyyyMMddHHmmss")}{GenerateRandomString(6)}";
                
                tempTable.Columns.Add("commodityId", typeof(int));
                tempTable.Columns.Add("quantity", typeof(int));
                tempTable.Columns.Add("price", typeof(int));

                foreach (CommodityInsertData data in insertData.CommodityDatas)
                {
                    tempTable.Rows.Add(data.CommodityId, data.Quantity, data.Price);
                }

                command.CommandText = "EXEC pro_bkg_insertOrderData @orderId,@memberId,@nowDate,@payType,@payState,@deliverType,@deliverState,@totalMoney,@commoditys";

                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@memberId", insertData.MemberId);
                command.Parameters.AddWithValue("@nowDate", now);
                command.Parameters.AddWithValue("@payType", insertData.PayType);
                command.Parameters.AddWithValue("@payState", 0);
                command.Parameters.AddWithValue("@deliverType", insertData.DeliverType);
                command.Parameters.AddWithValue("@deliverState", 0);
                command.Parameters.AddWithValue("@totalMoney", insertData.TotalMoney);

                SqlParameter parameter = command.Parameters.AddWithValue("@commoditys", tempTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo.orderCommodityTempType";
                
                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                // 庫存量不足
                if (statusMessage == (int)StateCode.StockError) throw new Exception(StateCode.StockError.ToString());
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
        /// 編輯訂單資料
        /// </summary>
        public bool UpdateOrderData(UpdateOrderDataDto updateData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_updateOrderData @orderId,@payTypeId,@payStateId,@deliverTypeId,@deliverStateId";

                command.Parameters.AddWithValue("@orderId", updateData.OrderId);
                command.Parameters.AddWithValue("@payTypeId", updateData.PayTypeId);
                command.Parameters.AddWithValue("@payStateId", updateData.PayStateId);
                command.Parameters.AddWithValue("@deliverTypeId", updateData.DeliverTypeId);
                command.Parameters.AddWithValue("@deliverStateId", updateData.DeliverStateId);

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                // DB執行錯誤
                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());

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
        /// 刪除訂單資料
        /// </summary>
        public bool DeleteOrderData()
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_deleteOrderData @deleteDays";

                command.Parameters.AddWithValue("@deleteDays", Convert.ToInt32(ConfigurationManager.AppSettings["OrderDeleteDays"]));

                command.Connection.Open();

                int statusMessage = Convert.ToInt32(command.ExecuteScalar());

                // DB執行錯誤
                if (statusMessage != (int)StateCode.Success) throw new Exception(StateCode.DbError.ToString());

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
        /// 檢查搜尋條件的傳入參數
        /// </summary>
        public bool CheckConditionInputData(ConditionDataDto conditionData)
        {
            // 檢查訂單日期起迄是否只有一邊有值
            if ((conditionData.StartDate.HasValue && !conditionData.EndDate.HasValue) || (conditionData.EndDate.HasValue && !conditionData.StartDate.HasValue)) return false;
            // 檢查結束日是否比開始日小
            if (conditionData.EndDate < conditionData.StartDate) return false;

            return true;
        }

        /// <summary>
        /// 檢查新增訂單資料的傳入參數
        /// </summary>
        public bool CheckInsertInputData(InsertOrderDataDto insertData)
        {
            // 檢查是否有該付款方式
            if (!Enum.IsDefined(typeof(PayTypeCode), insertData.PayType)) return false;
            // 檢查是否有該配送方式
            if (!Enum.IsDefined(typeof(DeliveryTypeCode), insertData.DeliverType)) return false;

            string rule = @"^[1-9]\d*$";
            foreach (CommodityInsertData item in insertData.CommodityDatas)
            {
                if (!Regex.IsMatch(item.Quantity.ToString(), rule)) return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查編輯訂單資料的傳入參數
        /// </summary>
        public bool CheckUpdateInputData(UpdateOrderDataDto updateData)
        {
            // 檢查是否有該付款方式
            if (!Enum.IsDefined(typeof(PayTypeCode), updateData.PayTypeId)) return false;
            // 檢查是否有該付款狀態
            if (!Enum.IsDefined(typeof(PayStateCode), updateData.PayStateId)) return false;
            // 檢查是否有該配送方式
            if (!Enum.IsDefined(typeof(DeliveryTypeCode), updateData.DeliverTypeId)) return false;
            // 檢查是否有該配送狀態
            if (!Enum.IsDefined(typeof(DeliveryStateCode), updateData.DeliverStateId)) return false;

            return true;
        }

        /// <summary>
        /// 檢查刪除訂單資料的傳入參數
        /// </summary>
        public bool CheckDeleteInputData(DeleteOrderDataDto deleteData)
        {
            // 檢查訂單編號是否有T及M
            if (!deleteData.OrderId.Contains('T') && !deleteData.OrderId.Contains('M')) return false;

            return true;
        }
    }
}
