-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateCommodityData]
	-- Add the parameters for the stored procedure here
	@commodityId INT,
	@name NVARCHAR(50),
	@description NVARCHAR(200),
	@type INT,
	@image NVARCHAR(60),
	@price INT,
	@stock INT,
	@open BIT,
	@adminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur WITH(NOLOCK) INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

	IF @vaildPermissionCount > 0
	BEGIN
		UPDATE dbo.t_commodity WITH(ROWLOCK) SET f_name = @name,f_description = @description,f_typeId = @type,f_price = @price,f_stock = @stock,f_open = @open WHERE f_id = @commodityId;
		
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