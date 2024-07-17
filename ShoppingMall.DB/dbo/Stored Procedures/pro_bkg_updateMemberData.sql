-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pro_bkg_updateMemberData]
	-- Add the parameters for the stored procedure here
	@memberId INT,
	@level TINYINT,
	@enabled BIT
AS
BEGIN
	UPDATE dbo.t_member WITH(ROWLOCK) SET f_level = @level,f_enabled = @enabled	WHERE f_id = @memberId;
END