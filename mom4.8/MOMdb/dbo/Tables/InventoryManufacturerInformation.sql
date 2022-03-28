CREATE TABLE [dbo].[InventoryManufacturerInformation] (
    [ID]                                     INT          IDENTITY (1, 1) NOT NULL,
    [InventoryManufacturerInformation_InvID] INT          NOT NULL,
    [MPN]                                    VARCHAR (75) NULL,
    [ApprovedManufacturer]                   VARCHAR (75) NULL,
    [ApprovedVendor]                         VARCHAR (75) NULL, 
    CONSTRAINT [PK_InventoryManufacturerInformation] PRIMARY KEY ([ID])
);

