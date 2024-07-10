CREATE TABLE [dbo].[t_permissions] (
    [f_id]   INT           IDENTITY (1, 1) NOT NULL,
    [f_name] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_t_permissions] PRIMARY KEY CLUSTERED ([f_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_permissions', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_permissions', @level2type = N'COLUMN', @level2name = N'f_name';

