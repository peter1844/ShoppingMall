/*
    描述: 刪除管理者帳號
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_delAdminData]
	@adminId INT,		--管理者編號
	@backAdminId INT,	--操作者編號
	@permission INT		--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @backAdminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				DELETE FROM dbo.t_adminUser WHERE f_id = @adminId;

				DELETE FROM dbo.t_adminUserRole WHERE f_adminUserId = @adminId;
			COMMIT TRANSACTION;
			SELECT 200;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			SELECT 102;
		END CATCH
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END