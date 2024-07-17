-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getAllAdminOptionData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT f_id,f_name FROM t_role WITH(NOLOCK)	WHERE f_id != 1;
		
	RETURN
END