-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getAllAdminData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	SELECT au.f_id,au.f_name,au.f_acc,au.f_enabled,aur.f_roleId	FROM t_adminUser AS au WITH(NOLOCK)
	INNER JOIN t_adminUserRole AS aur ON au.f_id = aur.f_adminUserId
	WHERE aur.f_roleId != 1;
		
	RETURN
END