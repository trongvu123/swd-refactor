IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Account] (
    [id] int NOT NULL IDENTITY,
    [username] nvarchar(45) NOT NULL,
    [password] nvarchar(100) NOT NULL,
    [register_date] date NOT NULL,
    [status] nvarchar(45) NOT NULL,
    [google_account] bit NOT NULL,
    [by_admin] bit NOT NULL,
    CONSTRAINT [PK__Account__3213E83F1227836C] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Brand] (
    [id] int NOT NULL IDENTITY,
    [brand_name] varchar(100) NOT NULL,
    [brand_image] nvarchar(100) NOT NULL,
    CONSTRAINT [PK__Brand__3213E83F07E0250E] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Category] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(45) NOT NULL,
    CONSTRAINT [PK__Category__3213E83F4E6EE1EC] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Payment] (
    [id] int NOT NULL IDENTITY,
    [total_price] float NULL,
    [payment_method] nvarchar(45) NULL,
    [transaction_date] datetime NULL,
    CONSTRAINT [PK__Payment__3213E83F31074084] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Role] (
    [id] int NOT NULL IDENTITY,
    [role_name] nvarchar(30) NOT NULL,
    CONSTRAINT [PK__Role__3213E83F5E85A4EC] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Product] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(100) NOT NULL,
    [detail] nvarchar(max) NOT NULL,
    [image] nvarchar(100) NOT NULL,
    [update_date] datetime NULL,
    [status] bit NOT NULL,
    [category_id] int NOT NULL,
    [brand_id] int NOT NULL,
    CONSTRAINT [PK__Product__3213E83FD41103F3] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Product_Category_category_id] FOREIGN KEY ([category_id]) REFERENCES [Category] ([id]),
    CONSTRAINT [FK__Product__brand_i__4924D839] FOREIGN KEY ([brand_id]) REFERENCES [Brand] ([id])
);
GO

CREATE TABLE [status_payment] (
    [id] int NOT NULL IDENTITY,
    [type] nvarchar(300) NULL,
    [update_at] datetime NULL,
    [update_by] int NULL,
    [create_at] datetime NULL,
    [create_by] int NULL,
    [payment_id] int NOT NULL,
    CONSTRAINT [PK__status_payment__3213E83F] PRIMARY KEY ([id]),
    CONSTRAINT [FK_status_payment_Payment_payment_id] FOREIGN KEY ([payment_id]) REFERENCES [Payment] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [User] (
    [id] int NOT NULL IDENTITY,
    [full_name] nvarchar(50) NOT NULL,
    [dob] datetime NOT NULL,
    [email] nvarchar(45) NOT NULL,
    [phone] nvarchar(10) NOT NULL,
    [gender] nvarchar(45) NOT NULL,
    [update_date] datetime NOT NULL,
    [update_by] int NOT NULL,
    [account_id] int NOT NULL,
    [role_id] int NOT NULL,
    CONSTRAINT [PK__User__3213E83FD23B5FEA] PRIMARY KEY ([id]),
    CONSTRAINT [FK_User_Role_role_id] FOREIGN KEY ([role_id]) REFERENCES [Role] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK__User__account_id__3DB3258D] FOREIGN KEY ([account_id]) REFERENCES [Account] ([id])
);
GO

CREATE TABLE [Product_Image] (
    [id] int NOT NULL IDENTITY,
    [product_id] int NOT NULL,
    [image] nvarchar(1000) NOT NULL,
    CONSTRAINT [PK__Product___3213E83F88F164DB] PRIMARY KEY ([id]),
    CONSTRAINT [FK__Product_I__produ__4CF5691D] FOREIGN KEY ([product_id]) REFERENCES [Product] ([id])
);
GO

CREATE TABLE [Storage] (
    [id] int NOT NULL IDENTITY,
    [storage] int NOT NULL,
    [original_price] float NOT NULL,
    [sale_price] float NOT NULL,
    [quantity] int NOT NULL,
    [product_id] int NOT NULL,
    CONSTRAINT [PK_Storage] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Storage_Product_product_id] FOREIGN KEY ([product_id]) REFERENCES [Product] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Campaigns] (
    [id] int NOT NULL IDENTITY,
    [title] nvarchar(max) NOT NULL,
    [description] nvarchar(max) NOT NULL,
    [status] nvarchar(max) NOT NULL,
    [created_by] int NOT NULL,
    [approved_by] int NULL,
    [start_date] datetime2 NOT NULL,
    [end_date] datetime2 NOT NULL,
    CONSTRAINT [PK_Campaigns] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Campaigns_User_created_by] FOREIGN KEY ([created_by]) REFERENCES [User] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Promotion] (
    [promotion_id] int NOT NULL IDENTITY,
    [promotion_name] nvarchar(max) NOT NULL,
    [start_date] datetime2 NULL,
    [end_date] datetime2 NULL,
    [minimum_purchase] decimal(18,2) NULL,
    [created_at] datetime2 NULL,
    [updated_at] datetime2 NULL,
    [created_by] int NOT NULL,
    [updated_by] int NOT NULL,
    CONSTRAINT [PK_Promotion] PRIMARY KEY ([promotion_id]),
    CONSTRAINT [FK_Promotion_User_created_by] FOREIGN KEY ([created_by]) REFERENCES [User] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Promotion_User_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [User] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [User_Address] (
    [id] int NOT NULL IDENTITY,
    [user_address] nvarchar(255) NOT NULL,
    [status] bit NOT NULL,
    [user_id] int NOT NULL,
    CONSTRAINT [PK__User_Add__3213E83F371BAF55] PRIMARY KEY ([id]),
    CONSTRAINT [FK__User_Addr__user___4183B671] FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
);
GO

CREATE TABLE [Budgets] (
    [id] int NOT NULL IDENTITY,
    [Amount] float NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatedBy] int NOT NULL,
    [CampaignId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Budgets] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Budgets_Campaigns_CampaignId] FOREIGN KEY ([CampaignId]) REFERENCES [Campaigns] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Cart] (
    [id] int NOT NULL IDENTITY,
    [storage_id] int NOT NULL,
    [customer_id] int NOT NULL,
    [user_address_id] int NOT NULL,
    [quantity] int NULL,
    [price] float NULL,
    [status] nvarchar(max) NULL,
    CONSTRAINT [PK_Cart] PRIMARY KEY ([storage_id], [customer_id], [user_address_id], [id]),
    CONSTRAINT [AK_Cart_id] UNIQUE ([id]),
    CONSTRAINT [FK_Cart_Storage_storage_id] FOREIGN KEY ([storage_id]) REFERENCES [Storage] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Cart_User_Address_user_address_id] FOREIGN KEY ([user_address_id]) REFERENCES [User_Address] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Cart_User_customer_id] FOREIGN KEY ([customer_id]) REFERENCES [User] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Order] (
    [id] int NOT NULL IDENTITY,
    [order_date] datetime NULL,
    [sale_id] int NULL,
    [cart_id] int NULL,
    [payment_id] int NOT NULL,
    [index] int NOT NULL,
    CONSTRAINT [PK__Order__3213E83FCBD4A6FA] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Order_Cart_cart_id] FOREIGN KEY ([cart_id]) REFERENCES [Cart] ([id]),
    CONSTRAINT [FK__Order__payment_i__62E4AA3C] FOREIGN KEY ([payment_id]) REFERENCES [Payment] ([id])
);
GO

CREATE TABLE [status_order] (
    [id] int NOT NULL IDENTITY,
    [type] nvarchar(300) NULL,
    [update_at] datetime NULL,
    [update_by] int NULL,
    [create_at] datetime NULL,
    [create_by] int NULL,
    [checkout_id] int NOT NULL,
    CONSTRAINT [PK__Status__3213E83F97D90152] PRIMARY KEY ([id]),
    CONSTRAINT [FK_status_order_Order_checkout_id] FOREIGN KEY ([checkout_id]) REFERENCES [Order] ([id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [UQ__Account__F3DBC572D661B57A] ON [Account] ([username]);
GO

CREATE INDEX [IX_Budgets_CampaignId] ON [Budgets] ([CampaignId]);
GO

CREATE INDEX [IX_Campaigns_created_by] ON [Campaigns] ([created_by]);
GO

CREATE INDEX [IX_Cart_customer_id] ON [Cart] ([customer_id]);
GO

CREATE INDEX [IX_Cart_user_address_id] ON [Cart] ([user_address_id]);
GO

CREATE INDEX [IX_Order_cart_id] ON [Order] ([cart_id]);
GO

CREATE UNIQUE INDEX [IX_Order_payment_id] ON [Order] ([payment_id]);
GO

CREATE INDEX [IX_Product_brand_id] ON [Product] ([brand_id]);
GO

CREATE INDEX [IX_Product_category_id] ON [Product] ([category_id]);
GO

CREATE INDEX [IX_Product_Image_product_id] ON [Product_Image] ([product_id]);
GO

CREATE INDEX [IX_Promotion_created_by] ON [Promotion] ([created_by]);
GO

CREATE INDEX [IX_Promotion_updated_by] ON [Promotion] ([updated_by]);
GO

CREATE INDEX [IX_status_order_checkout_id] ON [status_order] ([checkout_id]);
GO

CREATE INDEX [IX_status_payment_payment_id] ON [status_payment] ([payment_id]);
GO

CREATE INDEX [IX_Storage_product_id] ON [Storage] ([product_id]);
GO

CREATE INDEX [IX_User_role_id] ON [User] ([role_id]);
GO

CREATE UNIQUE INDEX [UQ__User__46A222CC47970D57] ON [User] ([account_id]);
GO

CREATE UNIQUE INDEX [UQ__User__AB6E6164D8A3A270] ON [User] ([email]);
GO

CREATE UNIQUE INDEX [UQ__User__B43B145F81F86A8C] ON [User] ([phone]);
GO

CREATE INDEX [IX_User_Address_user_id] ON [User_Address] ([user_id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250308152527_v1', N'8.0.6');
GO

COMMIT;
GO

