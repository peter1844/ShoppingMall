-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_insertAdminData]
	-- Add the parameters for the stored procedure here
	@name NVARCHAR(20),
	@acc VARCHAR(16),
	@pwd VARCHAR(16),
	@enabled BIT,
	@roleId [dbo].[adminUserRoleTempType] READONLY,
	@adminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

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