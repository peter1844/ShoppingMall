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
	@open BIT
AS
BEGIN
	INSERT INTO dbo.t_commodity(f_name,f_description,f_typeId,f_image,f_price,f_stock,f_open) VALUES(@name,@description,@type,@image,@price,@stock,@open);
END