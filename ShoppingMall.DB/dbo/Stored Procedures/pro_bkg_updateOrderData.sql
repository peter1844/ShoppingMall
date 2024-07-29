﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateOrderData]
	-- Add the parameters for the stored procedure here
	@orderId VARCHAR(40),
	@payTypeId SMALLINT,
	@payStateId TINYINT,
	@deliverTypeId SMALLINT,
	@deliverStateId TINYINT
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			UPDATE dbo.t_orderMain WITH(ROWLOCK) SET f_payType = @payTypeId, f_payState = @payStateId, f_deliverType = @deliverTypeId, f_deliverState = @deliverStateId WHERE f_id = @orderId;

			IF @deliverStateId = 2
			BEGIN
				UPDATE t_commodity SET f_stock = f_stock + od.f_quantity FROM t_commodity AS c INNER JOIN t_orderDetail AS od ON c.f_id = od.f_commodityId WHERE od.f_orderMainId = @orderId;
			END
        COMMIT TRANSACTION;
		SELECT 200;
    END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT 102;
	END CATCH
END