-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateOrderData]
	-- Add the parameters for the stored procedure here
	@orderId VARCHAR(40),
	@payTypeId SMALLINT,
	@payStateId TINYINT,
	@deliverTypeId SMALLINT,
	@deliverStateId TINYINT
AS
BEGIN
	UPDATE dbo.t_orderMain WITH(ROWLOCK) SET f_payType = @payTypeId, f_payState = @payStateId, f_deliverType = @deliverTypeId, f_deliverState = @deliverStateId WHERE f_id = @orderId;
END