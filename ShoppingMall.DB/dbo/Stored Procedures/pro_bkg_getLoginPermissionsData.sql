-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_getLoginPermissionsData]
	-- Add the parameters for the stored procedure here
	@adminId INT
AS
BEGIN
	SELECT rp.f_permissionsId FROM t_adminUserRole AS aur WITH(NOLOCK) INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId GROUP BY rp.f_permissionsId
		
	RETURN
END