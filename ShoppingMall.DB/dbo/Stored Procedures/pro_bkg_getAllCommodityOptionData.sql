-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getAllCommodityOptionData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT 
		f_id,
		f_name
	FROM 
		t_commodityType WITH(NOLOCK);
		
	RETURN
END