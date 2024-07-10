-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getLoginData]
	-- Add the parameters for the stored procedure here
	@acc varchar(16),
	@pwd varchar(16)
AS
BEGIN
	SELECT 
		f_id,
		f_name
	FROM 
		t_adminUser 
	WHERE 
		f_acc = @acc 
	AND 
		f_pwd = HASHBYTES('md5',@pwd)
	AND 
		f_enabled = 1

	RETURN
END
