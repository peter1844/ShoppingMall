-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_deleteOrderData]
	-- Add the parameters for the stored procedure here
	@orderId VARCHAR(40)
AS
BEGIN
	DECLARE @orderDeliverState INT;
	SELECT @orderDeliverState = f_deliverState FROM t_orderMain WHERE f_id = @orderId;

	BEGIN TRY
		BEGIN TRANSACTION

			IF @orderDeliverState = 0
			BEGIN 
				UPDATE t_commodity SET f_stock = f_stock + od.f_quantity FROM t_commodity AS c INNER JOIN t_orderDetail AS od ON c.f_id = od.f_commodityId WHERE od.f_orderMainId = @orderId;
			END

			DELETE FROM dbo.t_orderMain WHERE f_id = @orderId;
			DELETE FROM dbo.t_orderDetail WHERE f_orderMainId = @orderId;
		COMMIT TRANSACTION;
		SELECT 200;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT 102;
	END CATCH
END