-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getOrderData]
	-- Add the parameters for the stored procedure here
	@id VARCHAR(40),
	@startDate DATE = NULL,
	@endDate DATE = NULL,
	@deliveryState INT
AS
BEGIN
	SELECT om.f_id,m.f_name AS memberName,om.f_date,om.f_payType,om.f_payState,om.f_deliverType,om.f_deliverState,om.f_totalMoney,c.f_name AS CommodityName,od.f_quantity,od.f_price,c.f_image FROM t_orderMain AS om WITH(NOLOCK)
	INNER JOIN t_member AS m ON om.f_memberId = m.f_id
	INNER JOIN t_orderDetail AS od ON om.f_id = od.f_orderMainId
	INNER JOIN t_commodity AS c ON od.f_commodityId = c.f_id
	WHERE 1 = 1 AND (om.f_id = @id OR @id = '') AND (om.f_deliverState = @deliveryState OR @deliveryState = -1) AND (om.f_date BETWEEN @startDate AND @endDate OR (@startDate IS NULL AND @endDate IS NULL))
		
	RETURN
END