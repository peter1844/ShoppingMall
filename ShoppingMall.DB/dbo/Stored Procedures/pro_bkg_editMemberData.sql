/*
    描述: 編輯會員資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_editMemberData]
	@memberId INT,		--會員編號
	@level TINYINT,		--等級
	@enabled BIT,		--是否有效
	@adminId INT,		--操作者編號
	@permission INT		--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		UPDATE dbo.t_member WITH(ROWLOCK) SET f_level = @level,f_enabled = @enabled	WHERE f_id = @memberId;
		SELECT 200;
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END