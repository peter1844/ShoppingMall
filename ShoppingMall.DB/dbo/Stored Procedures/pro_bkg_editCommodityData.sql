/*
    描述: 編輯商品資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_editCommodityData]
	@commodityId INT,				--商品編號
	@name NVARCHAR(50),				--名稱
	@description NVARCHAR(200),		--描述
	@type INT,						--類型
	@image NVARCHAR(60),			--圖片
	@price INT,						--價格
	@stock INT,						--庫存量
	@open BIT,						--是否開啟
	@adminId INT,					--操作者編號
	@permission INT					--操作權限
AS
BEGIN
	DECLARE @vaildPermissionCount INT;

	SELECT @vaildPermissionCount = COUNT(rp.f_id) 
	FROM t_adminUserRole AS aur WITH(NOLOCK) 
	INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		UPDATE dbo.t_commodity WITH(ROWLOCK) 
		SET f_name = @name,f_description = @description,f_typeId = @type,f_price = @price,f_stock = @stock,f_open = @open 
		WHERE f_id = @commodityId;
		
		IF @image <> ''
		BEGIN
			IF @image = 'delete'
			BEGIN
				UPDATE dbo.t_commodity WITH(ROWLOCK) SET f_image = '' WHERE f_id = @commodityId;
			END
			ELSE
			BEGIN
				UPDATE dbo.t_commodity WITH(ROWLOCK) SET f_image = @image WHERE f_id = @commodityId;
			END
		END
		SELECT 200;
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END