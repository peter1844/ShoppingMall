/*
    描述: 刪除訂單資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_delOrderData]
	@deleteDays SMALLINT	--刪除天數
AS
BEGIN
	DECLARE @deleteDate DATE;

    SET @deleteDate = DATEADD(DAY, -@deleteDays, GETDATE());	

	BEGIN TRY
		BEGIN TRANSACTION
			DELETE FROM dbo.t_orderDetail WHERE f_orderMainId IN(SELECT f_id FROM t_orderMain WHERE f_date < @deleteDate);

			DELETE FROM dbo.t_orderMain WHERE f_id IN(SELECT f_id FROM t_orderMain WHERE f_date < @deleteDate);
		COMMIT TRANSACTION;
		SELECT 200;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT 102;
	END CATCH
END