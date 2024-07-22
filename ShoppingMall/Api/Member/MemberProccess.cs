using ShoppingMall.App_Code;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Member
{
    public class MemberProccess : ShoppingMall.Base.Base
    {
        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        public List<MemberDataDtoResponse> GetAllMemberData()
        {
            List<MemberDataDtoResponse> memberData = new List<MemberDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_getAllMemberData";
                command.Connection.Open();

                da.SelectCommand = command;
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        memberData.Add(new MemberDataDtoResponse
                        {
                            Id = Convert.ToInt32(dt.Rows[i]["f_id"]),
                            Acc = dt.Rows[i]["f_acc"].ToString(),
                            Name = dt.Rows[i]["f_name"].ToString(),
                            Level = Convert.ToInt32(dt.Rows[i]["f_level"]),
                            Enabled = Convert.ToInt32(dt.Rows[i]["f_enabled"]),
                        });
                    }
                }

                return memberData;
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
        /// 編輯會員資料
        /// </summary>
        public bool UpdateMemberData(UpdateMemberDataDto updateData)
        {
            SqlCommand command = MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_updateMemberData @memberId,@level,@enabled";

                command.Parameters.AddWithValue("@memberId", updateData.MemberId);
                command.Parameters.AddWithValue("@level", updateData.Level);
                command.Parameters.AddWithValue("@enabled", updateData.Enabled);

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
        /// 檢查編輯會員的傳入參數
        /// </summary>
        public bool CheckUpdateInputData(UpdateMemberDataDto updateData)
        {
            // 檢查是否有效的參數是否合法
            if (updateData.Enabled < 0 || updateData.Enabled > 1) return false;
            // 檢查會員等級的參數是否合法
            if (updateData.Level < 1 || updateData.Level > 5) return false;

            return true;
        }
    }
}
