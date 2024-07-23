CREATE TABLE [dbo].[t_commodity] (
    [f_id]          INT            IDENTITY (1, 1) NOT NULL,
    [f_name]        NVARCHAR (50)  NOT NULL,
    [f_description] NVARCHAR (200) NOT NULL,
    [f_typeId]      INT            NOT NULL,
    [f_image]       VARCHAR (30)   NOT NULL,
    [f_price]       INT            NOT NULL,
    [f_stock]       INT            NOT NULL,
    [f_open]        BIT            CONSTRAINT [DF_t_commodity_f_open] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_t_commodity] PRIMARY KEY CLUSTERED ([f_id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否開啟', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_open';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_stock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品圖示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品類型-關聯commodityType的id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_typeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品細項描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity', @level2type = N'COLUMN', @level2name = N'f_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N't_commodity';

