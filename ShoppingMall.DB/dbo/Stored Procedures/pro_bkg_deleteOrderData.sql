-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_deleteOrderData]
	-- Add the parameters for the stored procedure here
	@deleteDays SMALLINT
AS
BEGIN
	DECLARE @deleteDate DATE;
    SET @deleteDate = DATEADD(DAY, -@deleteDays, GETDATE());	

	BEGIN TRY
		BEGIN TRANSACTION
			DELETE FROM dbo.t_orderMain WHERE f_id IN(SELECT f_id FROM t_orderMain WHERE f_date < @deleteDate);
			DELETE FROM dbo.t_orderDetail WHERE f_orderMainId IN(SELECT f_id FROM t_orderMain WHERE f_date < @deleteDate);
		COMMIT TRANSACTION;
		SELECT 200;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT 102;
	END CATCH
END