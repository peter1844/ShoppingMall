-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getAllMemberData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT f_id,f_name,f_acc,f_level,f_enabled FROM t_member WITH(NOLOCK);
		
	RETURN
END