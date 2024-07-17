CREATE TABLE [dbo].[t_member] (
    [f_id]      INT            IDENTITY (1, 1) NOT NULL,
    [f_name]    NVARCHAR (20)  NOT NULL,
    [f_acc]     VARCHAR (16)   NOT NULL,
    [f_pwd]     VARBINARY (16) NOT NULL,
    [f_level]   TINYINT        CONSTRAINT [DF_t_user_f_level] DEFAULT ((1)) NOT NULL,
    [f_enabled] BIT            CONSTRAINT [DF_t_user_f_enabled] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_t_user] PRIMARY KEY CLUSTERED ([f_id] ASC),
    CONSTRAINT [IX_t_user_f_acc] UNIQUE NONCLUSTERED ([f_acc] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否有效', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_enabled';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'等級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_level';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_pwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_acc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會員名字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會員資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_member';

