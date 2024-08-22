using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Member
{
    public class MemberProccess : IMember
    {
        private IContextHelper _contextHelper;
        private IDbHelper _dbHelper;
        private ITools _tools;

        public MemberProccess(IContextHelper contextHelper = null, IDbHelper dbHelper = null, ITools tools = null)
        {
            _contextHelper = contextHelper ?? new ContextHelper();
            _dbHelper = dbHelper ?? new DbHelper();
            _tools = tools ?? new Tools();
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        public List<MemberDataDtoResponse> GetAllMemberData()
        {
            List<MemberDataDtoResponse> memberData = new List<MemberDataDtoResponse>();

            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            SqlCommand command = _dbHelper.MsSqlConnection();

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
            SqlCommand command = _dbHelper.MsSqlConnection();

            try
            {
                command.CommandText = "EXEC pro_bkg_editMemberData @memberId,@level,@enabled,@adminId,@permission";

                command.Parameters.AddWithValue("@memberId", updateData.MemberId);
                command.Parameters.AddWithValue("@level", updateData.Level);
                command.Parameters.AddWithValue("@enabled", updateData.Enabled);
                command.Parameters.AddWithValue("@adminId", Convert.ToInt32(_contextHelper.GetContext().Session["id"]));
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
