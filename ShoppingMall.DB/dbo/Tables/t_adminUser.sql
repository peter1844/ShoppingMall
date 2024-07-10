CREATE TABLE [dbo].[t_adminUser] (
    [f_id]      INT            IDENTITY (1, 1) NOT NULL,
    [f_name]    NVARCHAR (20)  NOT NULL,
    [f_acc]     VARCHAR (16)   NOT NULL,
    [f_pwd]     VARBINARY (16) NOT NULL,
    [f_enabled] BIT            CONSTRAINT [DF_t_admin_user_f_enabled] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__t_admin___2911CBED987BDA13] PRIMARY KEY CLUSTERED ([f_id] ASC),
    CONSTRAINT [IX_t_admin_user_f_account] UNIQUE NONCLUSTERED ([f_acc] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'管理者帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'管理者名字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUser', @level2type = N'COLUMN', @level2name = N'f_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUser', @level2type = N'COLUMN', @level2name = N'f_acc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUser', @level2type = N'COLUMN', @level2name = N'f_pwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_adminUser', @level2type = N'COLUMN', @level2name = N'f_enabled';

