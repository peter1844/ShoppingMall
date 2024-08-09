/*
    描述: 取得訂單資料
    日期: 2024-08-09
*/
CREATE PROCEDURE [dbo].[pro_bkg_getOrderData]
	@id VARCHAR(40),			--訂單編號
	@startDate DATE = NULL,		--訂單日期-起
	@endDate DATE = NULL,		--訂單日期-迄
	@deliveryState INT			--配送狀態
AS
BEGIN
	SELECT om.f_id,m.f_name AS memberName,om.f_date,om.f_payType,om.f_payState,om.f_deliverType,om.f_deliverState,om.f_totalMoney,c.f_name AS CommodityName,od.f_quantity,od.f_price,c.f_image 
	FROM t_orderMain AS om WITH(NOLOCK)
	INNER JOIN t_member AS m ON om.f_memberId = m.f_id
	INNER JOIN t_orderDetail AS od ON om.f_id = od.f_orderMainId
	INNER JOIN t_commodity AS c ON od.f_commodityId = c.f_id
	WHERE 1 = 1 AND (om.f_id = @id OR @id = '') AND (om.f_deliverState = @deliveryState OR @deliveryState = -1) AND (om.f_date BETWEEN @startDate AND @endDate OR (@startDate IS NULL AND @endDate IS NULL))
		
	RETURN
END