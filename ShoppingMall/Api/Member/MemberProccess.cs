using ShoppingMall.Helper;
using ShoppingMall.Models.Member;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace ShoppingMall.Api.Member
{
    public class MemberProccess
    {
        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        public List<MemberDataDtoResponse> GetAllMemberData()
        {
            List<MemberDataDtoResponse> memberData = new List<MemberDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = DbHelper.MsSqlConnection();

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
                throw new Exception(ex.Message);
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
            HttpContext context = HttpContext.Current;
            SqlCommand command = DbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_updateMemberData @memberId,@level,@enabled,@adminId,@permission";

                command.Parameters.AddWithValue("@memberId", updateData.MemberId);
                command.Parameters.AddWithValue("@level", updateData.Level);
                command.Parameters.AddWithValue("@enabled", updateData.Enabled);
                command.Parameters.AddWithValue("@adminId", Convert.ToInt32(context.Session["id"]));
                command.Parameters.AddWithValue("@permission", Permissions.MemberUpdate);

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
                throw new Exception(ex.Message, ex);
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
