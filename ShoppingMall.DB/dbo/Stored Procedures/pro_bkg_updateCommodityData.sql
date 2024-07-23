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
	@open BIT
AS
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
END