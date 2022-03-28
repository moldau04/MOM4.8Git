CREATE TABLE [dbo].[Bank] (
    [ID]                      INT             NOT NULL,
    [fDesc]                   VARCHAR (75)    NULL,
    [Rol]                     INT             NULL,
    [NBranch]                 VARCHAR (20)    NULL,
    [NAcct]                   VARCHAR (20)    NULL,
    [NRoute]                  VARCHAR (20)    NULL,
    [NextC]                   BIGINT             NULL,
    [NextD]                   INT             NULL,
    [NextE]                   INT             NULL,
    [Rate]                    NUMERIC (30, 2) NULL,
    [CLimit]                  NUMERIC (30, 2) NULL,
    [Warn]                    SMALLINT        NOT NULL,
    [Recon]                   NUMERIC (30, 2) NULL,
    [Balance]                 NUMERIC (30, 2) NULL,
    [Status]                  SMALLINT        NULL,
    [InUse]                   SMALLINT        NOT NULL,
    [ACHFileHeaderStringA]    VARCHAR (255)   NULL,
    [ACHFileHeaderStringB]    VARCHAR (255)   NULL,
    [ACHFileHeaderStringC]    VARCHAR (255)   NULL,
    [ACHCompanyHeaderString1] VARCHAR (255)   NULL,
    [ACHCompanyHeaderString2] VARCHAR (255)   NULL,
    [ACHBatchControlString1]  VARCHAR (255)   NULL,
    [ACHBatchControlString2]  VARCHAR (255)   NULL,
    [ACHBatchControlString3]  VARCHAR (255)   NULL,
    [ACHFileControlString1]   VARCHAR (255)   NULL,
    [Chart]                   INT             NULL,
    [LastReconDate]           DATETIME        NULL,
    [NextCash] BIGINT
      ,[NextWire] BIGINT
      ,[NextACH] BIGINT
      ,[NextCC] BIGINT
      ,[APACHCompanyID] VARCHAR(MAX)
      ,[APImmediateOrigin] VARCHAR(MAX)
      ,[BankType] int
      ,[ChartID] int
);

