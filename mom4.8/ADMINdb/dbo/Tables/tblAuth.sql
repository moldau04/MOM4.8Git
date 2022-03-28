CREATE TABLE [dbo].[tblAuth] (
    [day]         INT            CONSTRAINT [DF_tblAuth_day] DEFAULT ((30)) NULL,
    [date]        DATETIME       NULL,
    [first]       BIT            CONSTRAINT [DF_tblAuth_first] DEFAULT ((0)) NULL,
    [lic]         BIT            CONSTRAINT [DF_tblAuth_lic] DEFAULT ((0)) NULL,
    [str]         NVARCHAR (MAX) NULL,
    [GPSInterval] INT            NULL
);

