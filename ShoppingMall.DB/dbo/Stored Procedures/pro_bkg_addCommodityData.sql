/*
    描述: 新增商品資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_addCommodityData]
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
	FROM t_adminUserRole AS aur 
	INNER JOIN t_rolePermissions AS rp WITH(NOLOCK) ON aur.f_roleId = rp.f_roleId 
	WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission;

	IF @vaildPermissionCount > 0
	BEGIN
		INSERT INTO dbo.t_commodity(f_name,f_description,f_typeId,f_image,f_price,f_stock,f_open) VALUES(@name,@description,@type,@image,@price,@stock,@open);
		SELECT 200;
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END