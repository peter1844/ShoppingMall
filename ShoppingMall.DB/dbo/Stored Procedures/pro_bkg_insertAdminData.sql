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
	@roleId [dbo].[adminUserRoleTempType] READONLY
AS
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