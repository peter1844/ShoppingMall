-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_deleteAdminData]
	-- Add the parameters for the stored procedure here
	@adminId INT
AS
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