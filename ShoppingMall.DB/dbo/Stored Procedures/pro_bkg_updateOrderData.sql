-- =============================================
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
	@deliverStateId TINYINT,
	@adminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

	IF @vaildPermissionCount > 0
	BEGIN
		DECLARE @originPayType INT;
		DECLARE @originPayState INT;
		DECLARE @originDeliverType INT;
		DECLARE @originDeliverState INT;
			
		SELECT @originPayType = f_payType, @originPayState = f_payState, @originDeliverType = f_deliverType, @originDeliverState = f_deliverState FROM t_orderMain WHERE f_id = @orderId;

		IF @originPayState = 1
		BEGIN
			SET @payTypeId = @originPayType
		END

		IF @originDeliverState = 1
		BEGIN
			SET @deliverTypeId = @originDeliverType
		END

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
	ELSE
	BEGIN
		SELECT 4013;
	END
END