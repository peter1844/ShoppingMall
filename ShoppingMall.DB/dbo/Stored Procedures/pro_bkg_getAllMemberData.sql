/*
    描述: 取得會員資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getAllMemberData]
	
AS
BEGIN
	SELECT f_id,f_name,f_acc,f_level,f_enabled FROM t_member WITH(NOLOCK);
		
	RETURN
END