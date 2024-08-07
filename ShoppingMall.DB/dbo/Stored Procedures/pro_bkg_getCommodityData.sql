-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getCommodityData]
	-- Add the parameters for the stored procedure here
	@name NVARCHAR(50),
	@type INT
AS
BEGIN
	SELECT c.f_id,c.f_name,c.f_description,c.f_typeId,c.f_image,c.f_price,c.f_stock,c.f_open,ct.f_name AS CommodityName FROM dbo.t_commodity AS c WITH(NOLOCK) INNER JOIN dbo.t_commodityType AS ct ON c.f_typeId = ct.f_id WHERE 1 = 1 AND (c.f_name LIKE '%' + @name + '%' OR @name = '') AND (f_typeId = @type OR @type = 0)
END