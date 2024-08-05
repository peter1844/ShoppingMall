CREATE TABLE [dbo].[t_orderMain] (
    [f_id]           CHAR (21) NOT NULL,
    [f_memberId]     INT       NOT NULL,
    [f_date]         DATE      NOT NULL,
    [f_payType]      SMALLINT  NOT NULL,
    [f_payState]     TINYINT   NOT NULL,
    [f_deliverType]  SMALLINT  NOT NULL,
    [f_deliverState] TINYINT   NOT NULL,
    [f_totalMoney]   INT       NOT NULL,
    CONSTRAINT [PK_t_orderMain] PRIMARY KEY CLUSTERED ([f_id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_totalMoney';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'配送狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_deliverState';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'配送方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_deliverType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會員-關聯member的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_memberId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_payType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderMain', @level2type = N'COLUMN', @level2name = N'f_payState';

