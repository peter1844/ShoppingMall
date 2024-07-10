CREATE TABLE [dbo].[t_adminUserRole] (
    [f_id]          INT IDENTITY (1, 1) NOT NULL,
    [f_adminUserId] INT NOT NULL,
    [f_roleId]      INT NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUserRole', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'管理者-關聯admin_user的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUserRole', @level2type = N'COLUMN', @level2name = N'f_adminUserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色-關聯role的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUserRole', @level2type = N'COLUMN', @level2name = N'f_roleId';

