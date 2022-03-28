CREATE TABLE [dbo].[Job] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [fDesc]                 VARCHAR (75)    NULL,
    [Type]                  SMALLINT        NULL,
    [Loc]                   INT             NULL,
    [Owner]                 INT             NULL,
    [Elev]                  INT             NULL,
    [Status]                SMALLINT        NULL,
    [PO]                    VARCHAR (25)    NULL,
    [Remarks]               TEXT            NULL,
    [Rev]                   NUMERIC (30, 2) NOT NULL,
    [Mat]                   NUMERIC (30, 2) NOT NULL,
    [Labor]                 NUMERIC (30, 2) NOT NULL,
    [OtherExp]              NUMERIC (30, 2) NULL,
    [Cost]                  NUMERIC (30, 2) NOT NULL,
    [Profit]                NUMERIC (30, 2) NOT NULL,
    [Ratio]                 NUMERIC (30, 2) NOT NULL,
    [Reg]                   NUMERIC (30, 2) NOT NULL,
    [OT]                    NUMERIC (30, 2) NOT NULL,
    [DT]                    NUMERIC (30, 2) NOT NULL,
    [TT]                    NUMERIC (30, 2) NOT NULL,
    [Hour]                  NUMERIC (30, 2) NOT NULL,
    [BRev]                  NUMERIC (30, 2) NOT NULL,
    [BMat]                  NUMERIC (30, 2) NOT NULL,
    [BLabor]                NUMERIC (30, 2) NOT NULL,
    [BOther]                NUMERIC (30, 2) NULL,
    [BCost]                 NUMERIC (30, 2) NOT NULL,
    [BProfit]               NUMERIC (30, 2) NOT NULL,
    [BRatio]                NUMERIC (30, 2) NOT NULL,
    [BHour]                 NUMERIC (30, 2) NOT NULL,
    [Template]              INT             NULL,
    [fDate]                 DATETIME        NULL,
    [Comm]                  NUMERIC (30, 2) NOT NULL,
    [WageC]                 INT             NULL,
    [NT]                    NUMERIC (30, 2) NOT NULL,
    [Post]                  SMALLINT        NULL,
    [EN]                    INT             NULL,
    [Certified]             SMALLINT        NULL,
    [Apprentice]            SMALLINT        NULL,
    [UseCat]                SMALLINT        NULL,
    [UseDed]                SMALLINT        NULL,
    [BillRate]              NUMERIC (30, 2) NOT NULL,
    [Markup]                NUMERIC (30, 2) NULL,
    [PType]                 SMALLINT        NULL,
    [Charge]                SMALLINT        NULL,
    [Amount]                NUMERIC (30, 2) NOT NULL,
    [GL]                    INT             NULL,
    [GLRev]                 INT             NULL,
    [GandA]                 NUMERIC (30, 2) NULL,
    [OHLabor]               NUMERIC (30, 2) NULL,
    [LastOH]                NUMERIC (30, 2) NULL,
    [etc]                   NUMERIC (30, 2) NULL,
    [ETCModifier]           NUMERIC (30, 2) NULL,
    [FP]                    VARCHAR (15)    NULL,
    [fGroup]                VARCHAR (25)    NULL,
    [CType]                 VARCHAR (15)    NULL,
    [Elevs]                 INT             NULL,
    [RateTravel]            NUMERIC (30, 2) NULL,
    [RateOT]                NUMERIC (30, 2) NULL,
    [RateNT]                NUMERIC (30, 2) NULL,
    [RateDT]                NUMERIC (30, 2) NULL,
    [RateMileage]           NUMERIC (30, 2) NULL,
    [Custom1]               VARCHAR (75)    NULL,
    [Custom2]               VARCHAR (75)    NULL,
    [Custom3]               VARCHAR (75)    NULL,
    [Custom4]               VARCHAR (75)    NULL,
    [Custom5]               VARCHAR (75)    NULL,
    [Custom6]               VARCHAR (75)    NULL,
    [Custom7]               VARCHAR (75)    NULL,
    [Custom8]               VARCHAR (75)    NULL,
    [Custom9]               VARCHAR (75)    NULL,
    [Custom10]              VARCHAR (75)    NULL,
    [Custom11]              VARCHAR (75)    NULL,
    [Custom12]              VARCHAR (75)    NULL,
    [Custom13]              VARCHAR (75)    NULL,
    [Custom14]              VARCHAR (75)    NULL,
    [Custom15]              VARCHAR (75)    NULL,
    [CloseDate]             DATETIME        NULL,
    [SPHandle]              SMALLINT        NULL,
    [SRemarks]              TEXT            NULL,
    [LCode]                 INT             NULL,
    [CreditCard]            TINYINT         NULL,
    [Custom16]              VARCHAR (75)    NULL,
    [Custom17]              VARCHAR (75)    NULL,
    [Custom18]              VARCHAR (75)    NULL,
    [Custom19]              VARCHAR (75)    NULL,
    [Custom20]              VARCHAR (75)    NULL,
    [NCSLock]               TINYINT         NULL,
    [Source]                VARCHAR (20)    NULL,
    [Audit]                 TINYINT         NULL,
    [AuditBy]               VARCHAR (50)    NULL,
    [AuditDate]             DATETIME        NULL,
    [fInt]                  TINYINT         NULL,
    [Reopen]                TINYINT         NULL,
    [NCSClose]              DATETIME        NULL,
    [Custom21]              VARCHAR (75)    NULL,
    [Custom22]              VARCHAR (75)    NULL,
    [Custom23]              VARCHAR (75)    NULL,
    [Custom24]              VARCHAR (75)    NULL,
    [Custom25]              DATETIME        NULL,
    [SO]                    VARCHAR (25)    NULL,
    [ProjCreationDate]      DATETIME        NULL,
    [Rol]                   INT             NULL,
    [LastUpdateDate]        DATETIME        NULL,
    [PrimarySyncID]         INT             NULL,
    [FMSEjob]               INT             NULL,
    [UpdatedDate]           DATETIME        CONSTRAINT [DF_Job_UpdatedDate] DEFAULT (getdate()) NULL,
    [TaskCategory]          VARCHAR (15)    NULL,
    [IsRenewalNotes]        SMALLINT        NULL,
    [RenewalNotes]          VARCHAR (MAX)   NULL,
    [LastProcessed]         VARCHAR (50)    NULL,
    [fmsimportdate]         DATETIME        NULL,
    [PWIP]                  BIT             NULL,
    [UnrecognizedRevenue]   INT             NULL,
    [UnrecognizedExpense]   INT             NULL,
    [RetainageReceivable]   INT             NULL,
    [ArchitectName]         NVARCHAR (50)   NULL,
    [ArchitectAdress]       NVARCHAR (200)  NULL,
    [ReceivePO]             NUMERIC (30, 2) NULL,
    [ProjectManagerUserID]  INT             NULL,
    [AssignedProjectUserID] INT             NULL,
    [TargetHPermission]     NUMERIC (30, 2) NULL,
    [InterestGL]            INT             NULL,
	[FirstLinkedEst]			INT             NULL,
    [SupervisorUserID]  INT             NULL,
    [Stage] INT NULL, 
    CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
CREATE TRIGGER Job_UpdateLastModifiedDate
ON Job
AFTER UPDATE AS
  UPDATE Job
  SET UpdatedDate = GETDATE()
  WHERE ID IN (SELECT DISTINCT ID FROM Inserted)
GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Job_Type]
    ON [dbo].[Job]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Job_Template]
    ON [dbo].[Job]([Template] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Job_owner]
    ON [dbo].[Job]([Owner] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Job_loc]
    ON [dbo].[Job]([Loc] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Job_Ctype]
    ON [dbo].[Job]([CType] DESC);

