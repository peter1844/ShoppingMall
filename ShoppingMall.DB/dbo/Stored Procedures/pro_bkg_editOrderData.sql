/*
    描述: 編輯訂單資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_editOrderData]
	@orderId VARCHAR(40),		--訂單編號
	@payTypeId SMALLINT,		--付款方式
	@payStateId TINYINT,		--付款狀態
	@deliverTypeId SMALLINT,	--配送方式
	@deliverStateId TINYINT,	--配送狀態
	@adminId INT,				--操作者編號
	@permission INT				--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				UPDATE dbo.t_orderMain WITH(ROWLOCK) SET 
				f_payType = CASE WHEN f_payState = 1 AND @payStateId = 1 THEN f_payType ELSE @payTypeId END, 
				f_deliverType = CASE WHEN f_deliverState = 1 AND @deliverStateId = 1 THEN f_deliverType ELSE @deliverTypeId END,
				f_payState = @payStateId, f_deliverState = @deliverStateId 
				WHERE f_id = @orderId;

				IF @deliverStateId = 2
				BEGIN
					UPDATE t_commodity WITH(ROWLOCK) SET f_stock = f_stock + od.f_quantity 
					FROM t_commodity AS c 
					INNER JOIN t_orderDetail AS od ON c.f_id = od.f_commodityId 
					WHERE od.f_orderMainId = @orderId;
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