CREATE TABLE [dbo].[t_orderDetail] (
    [f_id]          INT          IDENTITY (1, 1) NOT NULL,
    [f_orderMainId] VARCHAR (40) NOT NULL,
    [f_commodityId] INT          NOT NULL,
    [f_quantity]    INT          NOT NULL,
    [f_price]       INT          NOT NULL,
    CONSTRAINT [PK_t_orderDetail] PRIMARY KEY CLUSTERED ([f_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderDetail', @level2type = N'COLUMN', @level2name = N'f_price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderDetail', @level2type = N'COLUMN', @level2name = N'f_quantity';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號-關聯orderMain的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderDetail', @level2type = N'COLUMN', @level2name = N'f_orderMainId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderDetail', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品-關聯commodity的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_orderDetail', @level2type = N'COLUMN', @level2name = N'f_commodityId';

