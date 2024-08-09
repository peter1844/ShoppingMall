/*
    描述: 新增訂單資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_addOrderData]
	@orderId VARCHAR(40),									--訂單編號
	@memberId INT,											--會員編號
	@nowDate DATE,											--下單日期
	@payType SMALLINT,										--付款方式
	@payState TINYINT,										--付款狀態
	@deliverType SMALLINT,									--配送方式
	@deliverState TINYINT,									--配送狀態
	@totalMoney INT,										--總金額
	@commoditys [dbo].[type_bkg_orderCommodity] READONLY,	--購買商品列表
	@adminId INT,											--操作者編號
	@permission INT											--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		DECLARE @insertCommoditysCount INT;
		DECLARE @vaildCommoditysStockCount INT;

		SELECT @insertCommoditysCount = COUNT(commodityId) FROM @commoditys;

		SELECT @vaildCommoditysStockCount = COUNT(c.f_id) 
		FROM t_commodity AS c WITH(NOLOCK) 
		INNER JOIN @commoditys AS c_temp ON c.f_id = c_temp.commodityId 
		WHERE c.f_stock >= c_temp.quantity;

		IF @insertCommoditysCount = @vaildCommoditysStockCount
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
					INSERT INTO dbo.t_orderMain(f_id,f_memberId,f_date,f_payType,f_payState,f_deliverType,f_deliverState,f_totalMoney)
					VALUES(@orderId,@memberId,@nowDate,@payType,@payState,@deliverType,@deliverState,@totalMoney);

					INSERT INTO dbo.t_orderDetail SELECT @orderId,commodityId,quantity,price FROM @commoditys;

					UPDATE t_commodity WITH(ROWLOCK) SET f_stock = f_stock - c_temp.quantity 
					FROM t_commodity AS c INNER JOIN @commoditys AS c_temp ON c.f_id = c_temp.commodityId;
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