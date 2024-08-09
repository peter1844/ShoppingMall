/*
    描述: 取得可以購買的商品
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getOpenCommodityData]
	
AS
BEGIN
	SELECT f_id,f_name,f_price FROM t_commodity WITH(NOLOCK) WHERE f_open = 1 AND f_stock > 0;
		
	RETURN
END