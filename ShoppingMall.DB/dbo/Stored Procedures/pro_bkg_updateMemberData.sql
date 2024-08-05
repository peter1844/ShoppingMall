-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateMemberData]
	-- Add the parameters for the stored procedure here
	@memberId INT,
	@level TINYINT,
	@enabled BIT,
	@adminId INT,
	@permission INT
AS
BEGIN
	DECLARE @vaildPermissionCount INT;
	SELECT @vaildPermissionCount = COUNT(rp.f_id) FROM t_adminUserRole AS aur INNER JOIN t_rolePermissions AS rp ON aur.f_roleId = rp.f_roleId WHERE aur.f_adminUserId = @adminId AND rp.f_permissionsId = @permission

	IF @vaildPermissionCount > 0
	BEGIN
		UPDATE dbo.t_member WITH(ROWLOCK) SET f_level = @level,f_enabled = @enabled	WHERE f_id = @memberId;
	END
	ELSE
	BEGIN
		SELECT 4013;
	END
END