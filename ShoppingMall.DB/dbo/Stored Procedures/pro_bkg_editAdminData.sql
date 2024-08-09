/*
    描述: 編輯管理者帳號
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_editAdminData]
	@adminId INT,										--管理者編號
	@name NVARCHAR(20),									--名字
	@pwd VARCHAR(16),									--密碼
	@enabled BIT,										--是否有效
	@roleId [dbo].[type_bkg_adminUserRole] READONLY,	--管理者角色對應表
	@backAdminId INT,									--操作者編號
	@permission INT										--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @backAdminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				UPDATE dbo.t_adminUser WITH(ROWLOCK) SET f_name = @name,f_enabled = @enabled WHERE f_id = @adminId;
		
				IF @pwd <> ''
				BEGIN
					UPDATE dbo.t_adminUser WITH(ROWLOCK) SET f_pwd = HASHBYTES('MD5', @pwd) WHERE f_id = @adminId;
				END
			
				DELETE FROM	dbo.t_adminUserRole	WHERE f_adminUserId = @adminId;

				INSERT INTO t_adminUserRole	SELECT @adminId,roleId FROM @roleId;
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