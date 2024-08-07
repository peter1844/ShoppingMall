-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getShortageCommodityData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT COUNT(f_id) AS CNT FROM t_commodity WITH(NOLOCK) WHERE f_stock <= 5
END