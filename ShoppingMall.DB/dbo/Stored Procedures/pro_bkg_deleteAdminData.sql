-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_deleteAdminData]
	-- Add the parameters for the stored procedure here
	@adminId INT,
	@backAdminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur WITH(NOLOCK) INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @backAdminId AND rp.f_permissionsId = @permission

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