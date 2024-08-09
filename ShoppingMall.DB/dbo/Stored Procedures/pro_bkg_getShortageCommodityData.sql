/*
    描述: 取得庫存量短缺的商品
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getShortageCommodityData]
	
AS
BEGIN
	SELECT COUNT(f_id) AS CNT FROM t_commodity WITH(NOLOCK) WHERE f_stock <= 5;
END