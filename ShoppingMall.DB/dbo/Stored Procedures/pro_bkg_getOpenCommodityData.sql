-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getOpenCommodityData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT f_id,f_name,f_price FROM t_commodity WITH(NOLOCK) WHERE f_open = 1;
		
	RETURN
END