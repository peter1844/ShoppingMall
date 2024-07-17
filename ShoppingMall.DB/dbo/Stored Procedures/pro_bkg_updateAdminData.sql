-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateAdminData]
	-- Add the parameters for the stored procedure here
	@adminId INT,
	@name NVARCHAR(20),
	@pwd VARCHAR(16),
	@enabled BIT,
	@roleId [dbo].[adminUserRoleTempType] READONLY
AS
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