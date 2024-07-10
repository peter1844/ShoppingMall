CREATE TABLE [dbo].[t_rolePermissions] (
    [f_id]            INT IDENTITY (1, 1) NOT NULL,
    [f_roleId]        INT NOT NULL,
    [f_permissionsId] INT NOT NULL,
    CONSTRAINT [PK_t_role_permissions] PRIMARY KEY CLUSTERED ([f_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_rolePermissions', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色-關聯role的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_rolePermissions', @level2type = N'COLUMN', @level2name = N'f_roleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限-關聯permissions的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_rolePermissions', @level2type = N'COLUMN', @level2name = N'f_permissionsId';

