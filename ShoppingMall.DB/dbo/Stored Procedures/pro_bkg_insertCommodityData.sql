-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_insertCommodityData]
	-- Add the parameters for the stored procedure here
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
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur INNER JOIN t_rolePermissions AS rp WITH(NOLOCK) ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

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