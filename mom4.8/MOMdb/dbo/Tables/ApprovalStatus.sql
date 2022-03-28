CREATE TABLE [dbo].[ApprovalStatus] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [PO]          INT           NOT NULL,
    [Status]      INT           NULL,
    [Comments]    VARCHAR (200) NULL,
    [ApproveDate] DATETIME      NULL,
    [UserID]      INT           NULL,
    [Signature]   IMAGE         NULL, 
    CONSTRAINT [PK_ApprovalStatus] PRIMARY KEY ([ID])
);

