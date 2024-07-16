using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingMall.Api.Member
{
    public class MemberProccess : ShoppingMall.Base.Base
    {
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
                throw new Exception("A102");
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }
        }
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
                throw new Exception("A102");
            }
            finally
            {
                command.Connection.Close(); //關閉連線
            }

        }
        public bool CheckUpdateInputData(UpdateMemberDataDto updateData)
        {
            if (updateData.Enabled < 0 || updateData.Enabled > 1) return false;
            if (updateData.Level < 1 || updateData.Enabled > 5) return false;

            return true;
        }
    }
}
