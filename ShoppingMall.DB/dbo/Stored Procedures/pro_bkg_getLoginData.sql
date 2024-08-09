/*
    描述: 登入驗證
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getLoginData]
	@acc VARCHAR(16),	--帳號
	@pwd VARCHAR(16)	--密碼
AS
BEGIN
	SELECT f_id,f_name FROM t_adminUser WITH(NOLOCK) WHERE f_acc = @acc AND f_pwd = HASHBYTES('md5',@pwd) AND f_enabled = 1;

	RETURN
END
