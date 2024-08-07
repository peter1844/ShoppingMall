-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_insertOrderData]
	-- Add the parameters for the stored procedure here
	@orderId VARCHAR(40),
	@memberId INT,
	@nowDate DATE,
	@payType SMALLINT,
	@payState TINYINT,
	@deliverType SMALLINT,
	@deliverState TINYINT,
	@totalMoney INT,
	@commoditys [dbo].[orderCommodityTempType] READONLY,
	@adminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur WITH(NOLOCK) INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

	IF @vaildPermissionCount > 0
	BEGIN
		DECLARE @insertCommoditysCount INT;
		SELECT @insertCommoditysCount = COUNT(commodityId) FROM @commoditys;

		DECLARE @vaildCommoditysStockCount INT;
		SELECT @vaildCommoditysStockCount = COUNT(c.f_id) FROM t_commodity AS c WITH(NOLOCK) INNER JOIN @commoditys AS c_temp ON c.f_id = c_temp.commodityId WHERE c.f_stock >= c_temp.quantity

		IF @insertCommoditysCount = @vaildCommoditysStockCount
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
					INSERT INTO dbo.t_orderMain(f_id,f_memberId,f_date,f_payType,f_payState,f_deliverType,f_deliverState,f_totalMoney) VALUES(@orderId,@memberId,@nowDate,@payType,@payState,@deliverType,@deliverState,@totalMoney);
					INSERT INTO dbo.t_orderDetail SELECT @orderId,commodityId,quantity,price FROM @commoditys;
					UPDATE t_commodity WITH(ROWLOCK) SET f_stock = f_stock - c_temp.quantity FROM t_commodity AS c INNER JOIN @commoditys AS c_temp ON c.f_id = c_temp.commodityId
				COMMIT TRANSACTION;
				SELECT 200;
			END TRY
			BEGIN CATCH
				ROLLBACK TRANSACTION;
				SELECT 102;
			END CATCH
		END
		ELSE BEGIN
			SELECT 700;
		END
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END