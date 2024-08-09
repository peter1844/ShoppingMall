/*
    描述: 取得下拉選項資料-商品
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getAllCommodityOptionData]
	
AS
BEGIN
	SELECT f_id,f_name FROM t_commodityType WITH(NOLOCK);
		
	RETURN
END