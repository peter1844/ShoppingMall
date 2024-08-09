/*
    描述: 取得商品資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getCommodityData]
	@name NVARCHAR(50),		--商品名稱
	@type INT				--商品類型
AS
BEGIN
	SELECT c.f_id,c.f_name,c.f_description,c.f_typeId,c.f_image,c.f_price,c.f_stock,c.f_open,ct.f_name AS CommodityName 
	FROM dbo.t_commodity AS c WITH(NOLOCK) 
	INNER JOIN dbo.t_commodityType AS ct ON c.f_typeId = ct.f_id 
	WHERE 1 = 1 AND (c.f_name LIKE '%' + @name + '%' OR @name = '') AND (f_typeId = @type OR @type = 0);
END