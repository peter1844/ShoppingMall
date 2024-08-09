/*
    描述: 登入成功後取得擁有的權限
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getLoginPermissionsData]
	@adminId INT	--管理者編號
AS
BEGIN
	SELECT rp.f_permissionsId 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId 
	GROUP BY rp.f_permissionsId
		
	RETURN
END