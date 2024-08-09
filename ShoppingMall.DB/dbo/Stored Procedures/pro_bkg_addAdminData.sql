/*
    描述: 新增管理者帳號
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_addAdminData]
	@name NVARCHAR(20),									--名字
	@acc VARCHAR(16),									--帳號
	@pwd VARCHAR(16),									--密碼
	@enabled BIT,										--是否有效
	@roleId [dbo].[type_bkg_adminUserRole] READONLY,	--管理者角色對應表
	@adminId INT,										--操作者編號
	@permission INT										--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				INSERT INTO dbo.t_adminUser(f_name,f_acc,f_pwd,f_enabled) VALUES(@name,@acc,HASHBYTES('MD5', @pwd),@enabled);

				DECLARE @primaryKey INT;
				SET @primaryKey = SCOPE_IDENTITY();

				INSERT INTO t_adminUserRole	SELECT @primaryKey,roleId FROM @roleId;
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