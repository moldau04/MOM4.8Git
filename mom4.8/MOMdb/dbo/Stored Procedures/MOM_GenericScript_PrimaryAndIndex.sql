CREATE PROCEDURE [dbo].[MOM_GenericScript_PrimaryAndIndex] 
AS 
  
 

 -------------------
 ------------------- MOM Generic Script to CREATE PRIMARY And NON CLUSTERED Index
 -------------------


          DECLARE @SQLTrans NVARCHAR(MAX) = N''
		SELECT  @SQLTrans = @SQLTrans + 'DROP INDEX [' + so.name + '].[' + id.name + '];' + CHAR(13) + CHAR(10)
				FROM sys.indexes 
				id JOIN sys.index_columns ic ON id.object_id = ic.object_id 
				AND id.index_id = ic.index_id 
				JOIN sys.columns col ON col.column_id = ic.column_id 
				AND col.object_id = id.object_id
				INNER JOIN sys.objects so ON so.object_id = id.object_id 
				WHERE id.object_id in ( 
				object_id('PDATicketSignature'),
				object_id('TicketDPDA'),
				object_id('Ticketo'),
				object_id('TicketD'),
				object_id('log2'),
				object_id('trans'),
				object_id('APBillItem'),
				object_id('OpenAP'),
				object_id('pj'),
				object_id('PJItem'), 
				object_id('CD'), 
				object_id('Paid') , 
				object_id('Vendor') ,
                object_id('GLA') ,
                object_id('Chart') ,
                object_id('PO'),
                object_id('ReceivePO'),
                object_id('RPOItem'),
                object_id('POItem'),
				object_id('Contract'),
				object_id('job'),
				object_id('jobi'),
				object_id('JobTItem'),
				object_id('Milestone'),
				object_id('BOM'),
				object_id('Owner'),
				object_id('loc'),
				object_id('Elev') ,
				object_id('ElevTItem') ,
				object_id('EquipTItem') ,
				object_id('Estimate'), 
				object_id('EstimateI') ,
				object_id('Invoice') ,
				object_id('InvoiceI') 
				)
			    and id.type_desc='NONCLUSTERED' 
				and id.is_primary_key=0
				and id.is_unique=0
				and id.name NOT LIKE '%MOM_INDEX_%'
		PRINT @SQLTrans
		EXEC (@SQLTrans) 

		


---------------------------------------------------------------------------------
------------------------- TicketD --------------------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_Edate'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_Edate ON TicketD (Edate desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_fwork'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_fwork ON TicketD (fwork desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_job'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_job ON TicketD (job desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_loc'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_loc ON TicketD (loc desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_type'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_type ON TicketD (type desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_invoice'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_invoice ON TicketD (invoice desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_ManualInvoice'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_ManualInvoice ON TicketD (manualinvoice desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_cat'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_cat ON TicketD (cat desc);  END

   
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketD_Timestamps'   
   AND object_id = OBJECT_ID('TicketD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketD_Timestamps ON TicketD (timeroute desc) include ( timesite,timecomp);  END

    ---------------------------------------------------------------------------------
------------------------- TicketO --------------------------------------------------


   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_Edate'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_Edate ON TicketO (Edate desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_fwork'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_fwork ON TicketO (fwork desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_job'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_job ON TicketO (job desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_loc'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_loc ON TicketO (lid desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_type'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_type ON TicketO (type desc);  END
     

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_cat'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_cat ON TicketO (cat desc);  END

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketO_Timestamps'   
   AND object_id = OBJECT_ID('TicketO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketO_Timestamps ON TicketO (timeroute desc) include ( timesite,timecomp);  END


   ---------------------------------------------------------------------------------
------------------------- TicketDPDA --------------------------------------------------


 IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_Edate'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_Edate ON TicketDPDA (Edate desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_fwork'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_fwork ON TicketDPDA (fwork desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_job'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_job ON TicketDPDA (job desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_loc'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_loc ON TicketDPDA (loc desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_type'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_type ON TicketDPDA (type desc);  END
    

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_cat'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_cat ON TicketDPDA (cat desc);  END

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_Timestamps'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_Timestamps ON TicketDPDA (timeroute desc) include ( timesite,timecomp);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_invoice'   
   AND object_id = OBJECT_ID('TicketDPDA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_invoice ON TicketDPDA (invoice desc);  END

      IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_TicketDPDA_PDATicketID'   
   AND object_id = OBJECT_ID('PDATicketSignature')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_TicketDPDA_PDATicketID ON PDATicketSignature (PDATicketID desc);  END 
      ---------------------------------------------------------------------------------
 ------------------------- TRANS --------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_Batch'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_Batch ON Trans (Batch desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_fDate'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_fDate ON Trans (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_Type'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_Type ON Trans (Type desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_Ref'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_Ref ON Trans (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_Acct'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_Acct ON Trans (Acct desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_strRef'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_strRef ON Trans (strRef desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Trans_BalanceSheet'   
   AND object_id = OBJECT_ID('Trans')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Trans_BalanceSheet ON Trans (fDate desc) INCLUDE ( [Acct],[AcctSub]);  END
 

---------------------------------------------------------------------------------
------------------------- LOG2 --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_LOG2_Ref'   
   AND object_id = OBJECT_ID('LOG2')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_LOG2_Ref ON LOG2 (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_LOG2_Screen'   
   AND object_id = OBJECT_ID('LOG2')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_LOG2_Screen ON LOG2 (Screen desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_LOG2_fUser'   
   AND object_id = OBJECT_ID('LOG2')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_LOG2_fUser ON LOG2 (fUser desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_LOG2_fDate'   
   AND object_id = OBJECT_ID('LOG2')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_LOG2_fDate ON LOG2 (fDate desc);  END
---------------------------------------------------------------------------------
------------------------- PJ --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PJ_fDate'   
   AND object_id = OBJECT_ID('PJ')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PJ_fDate ON PJ (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PJ_TRID'   
   AND object_id = OBJECT_ID('PJ')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PJ_TRID ON PJ (TRID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PJ_Ref'   
   AND object_id = OBJECT_ID('PJ')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PJ_Ref ON PJ (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PJ_Status'   
   AND object_id = OBJECT_ID('PJ')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PJ_Status ON PJ (Status desc);  END
---------------------------------------------------------------------------------
------------------------- APBillItem --------------------------------------------------  
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_APBillItem_PJID'   
   AND object_id = OBJECT_ID('APBillItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_APBillItem_PJID ON APBillItem (PJID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_APBillItem_Batch'   
   AND object_id = OBJECT_ID('APBillItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_APBillItem_Batch ON APBillItem (Batch desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_APBillItem_TRID'   
   AND object_id = OBJECT_ID('APBillItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_APBillItem_TRID ON APBillItem (TRID desc);  END
---------------------------------------------------------------------------------
------------------------- OpenAP -------------------------------------------------- 
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_OpenAP_PJID'   
   AND object_id = OBJECT_ID('OpenAP')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_OpenAP_PJID ON OpenAP (PJID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_OpenAP_Ref'   
   AND object_id = OBJECT_ID('OpenAP')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_OpenAP_Ref ON OpenAP (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_OpenAP_TRID'   
   AND object_id = OBJECT_ID('OpenAP')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_OpenAP_TRID ON OpenAP (TRID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_OpenAP_Vendor'   
   AND object_id = OBJECT_ID('OpenAP')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_OpenAP_Vendor ON OpenAP (Vendor desc);  END
---------------------------------------------------------------------------------
------------------------- PJItem --------------------------------------------------  
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PJItem_TRID'   
   AND object_id = OBJECT_ID('PJItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PJItem_TRID ON PJItem (TRID desc);  END
---------------------------------------------------------------------------------
------------------------- CD --------------------------------------------------  
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_CD_fDate'   
   AND object_id = OBJECT_ID('CD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_CD_fDate ON CD (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_CD_Ref'   
   AND object_id = OBJECT_ID('CD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_CD_Ref ON CD (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_CD_TransID'   
   AND object_id = OBJECT_ID('CD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_CD_TransID ON CD (TransID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_CD_Vendor'   
   AND object_id = OBJECT_ID('CD')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_CD_Vendor ON CD (Vendor desc);  END
---------------------------------------------------------------------------------
------------------------- Paid -------------------------------------------------- 

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Paid_fDate'   
   AND object_id = OBJECT_ID('Paid')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Paid_fDate ON Paid (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Paid_TRID'   
   AND object_id = OBJECT_ID('Paid')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Paid_TRID ON Paid (TRID desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Paid_Ref'   
   AND object_id = OBJECT_ID('Paid')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Paid_Ref ON Paid (Ref desc);  END
---------------------------------------------------------------------------------
------------------------- Vendor --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Vendor_Rol'   
   AND object_id = OBJECT_ID('Vendor')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Vendor_Rol ON Vendor (Rol desc);  END
---------------------------------------------------------------------------------
------------------------- PO --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PO_fDate'   
   AND object_id = OBJECT_ID('PO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PO_fDate ON PO (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PO_Status'   
   AND object_id = OBJECT_ID('PO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PO_Status ON PO (Status desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_PO_Vendor'   
   AND object_id = OBJECT_ID('PO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_PO_Vendor ON PO (Vendor desc);  END
---------------------------------------------------------------------------------
------------------------- POItem --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_POItem_PO'   
   AND object_id = OBJECT_ID('POItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_POItem_PO ON POItem (PO desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_POItem_Job'   
   AND object_id = OBJECT_ID('POItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_POItem_Job ON POItem (Job desc);  END
---------------------------------------------------------------------------------
------------------------- ReceivePO --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_ReceivePO_PO'   
   AND object_id = OBJECT_ID('ReceivePO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_ReceivePO_PO ON ReceivePO (PO desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_ReceivePO_Ref'   
   AND object_id = OBJECT_ID('ReceivePO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_ReceivePO_Ref ON ReceivePO (Ref desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_ReceivePO_fDate'   
   AND object_id = OBJECT_ID('ReceivePO')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_ReceivePO_fDate ON ReceivePO (fDate desc);  END
---------------------------------------------------------------------------------
------------------------- RPOItem --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_RPOItem_ReceivePO'   
   AND object_id = OBJECT_ID('RPOItem')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_RPOItem_ReceivePO ON RPOItem (ReceivePO desc);  END
---------------------------------------------------------------------------------
------------------------- GLA --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_GLA_fDate'   
   AND object_id = OBJECT_ID('GLA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_GLA_fDate ON GLA (fDate desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_GLA_Internal'   
   AND object_id = OBJECT_ID('GLA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_GLA_Internal ON GLA (Internal desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_GLA_Batch'   
   AND object_id = OBJECT_ID('GLA')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_GLA_Batch ON GLA (Batch desc);  END
---------------------------------------------------------------------------------
------------------------- Chart --------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Chart_Type'   
   AND object_id = OBJECT_ID('Chart')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Chart_Type ON Chart (Type desc);  END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Chart_Status'   
   AND object_id = OBJECT_ID('Chart')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Chart_Status ON Chart (Status desc);  END

   ---------------------------------------------------------------------------------
---------------------------------Contract------------------------------------------------


 IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_job'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_job ON Contract (job desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_loc'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_loc ON Contract (loc desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_owner'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_owner ON Contract (owner desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_chart'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_chart ON Contract (chart desc);  END


     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_BStart'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_BStart ON Contract (BStart desc);  END

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_BFinish'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_BFinish ON Contract (BFinish desc);  END

       IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Contract_SStart'   
   AND object_id = OBJECT_ID('Contract')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Contract_SStart ON Contract (SStart desc);  END


   
   ---------------------------------------------------------------------------------
---------------------------------job------------------------------------------------


   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_loc'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_loc ON Job (loc desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_owner'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_owner ON Job (owner desc);  END 

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_Type'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_Type ON Job (Type desc);  END


    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_Template'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_Template ON Job (Template desc);  END

       IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_CType'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_Ctype ON Job (Ctype desc);  END

      IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_PWIP'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_PWIP ON Job (PWIP desc);  END

      IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_Status'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_Status ON Job (Status desc);  END


   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Job_fdate'   
   AND object_id = OBJECT_ID('Job')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Job_fdate ON Job (fdate desc);  END
    
   ---------------------------------------------------------------------------------
---------------------------------jobi------------------------------------------------
 
      IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_JobI_Job'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_Job ON JobI (Job desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_fdate'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_fdate ON JobI (fdate desc);  END 

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_Ref'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_Ref ON JobI (Ref desc);  END

     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_TransId'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_TransId ON JobI (TransId desc);  END


    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_Phase'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_Phase ON JobI (Phase desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_Type'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_Type ON JobI (Type desc);  END

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Jobi_Labor'   
   AND object_id = OBJECT_ID('JobI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_JobI_Labor ON JobI (Labor desc);  END
   ---------------------------------------------------------------------
   ------------------------------Owner-----------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Owner_Rol'
AND object_id = OBJECT_ID('Owner') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Owner_Rol ON Owner (Rol desc); END
---------------------------------------------------------------------------
-----------------------------Loc-------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_Rol'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_Rol ON Loc (Rol desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_Owner'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_Owner ON Loc (Owner desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_Route'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_Route ON Loc (Route desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_ID'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_ID ON Loc (ID desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_Tag'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_Tag ON Loc (Tag desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Loc_Terr'
AND object_id = OBJECT_ID('Loc') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Loc_Terr ON Loc (Terr desc); END
---------------------------------------------------------------------------
----------------------------------Elev------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Elev_Owner'
AND object_id = OBJECT_ID('Elev') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Elev_Owner ON Elev (Owner desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Elev_type'
AND object_id = OBJECT_ID('Elev') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Elev_type ON Elev (type desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Elev_Unit'
AND object_id = OBJECT_ID('Elev') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Elev_Unit ON Elev (Unit desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Elev_Cat'
AND object_id = OBJECT_ID('Elev') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Elev_Cat ON Elev (Cat desc); END 
  
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_Elev_Building'
AND object_id = OBJECT_ID('Elev') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_Elev_Building ON Elev (Building desc); END
----------------------------------------------------------------------------------
--------------------------------ElevTItem------------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_ElevTItem_Elev'
AND object_id = OBJECT_ID('ElevTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_ElevTItem_Elev ON ElevTItem (Elev desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_ElevTItem_ElevT'
AND object_id = OBJECT_ID('ElevTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_ElevTItem_ElevT ON ElevTItem (ElevT desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_ElevTItem_CustomID'
AND object_id = OBJECT_ID('ElevTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_ElevTItem_CustomID ON ElevTItem (CustomID desc); END
------------------------------------------------------------------------------------
-----------------------------------EquipTItem----------------------------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_EquipTItem_Elev'
AND object_id = OBJECT_ID('EquipTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_EquipTItem_Elev ON EquipTItem (Elev desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_EquipTItem_EquipT'
AND object_id = OBJECT_ID('EquipTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_EquipTItem_EquipT ON EquipTItem (EquipT desc); END

---------------------------------------------------------------------------
----------------------------------JobTItem---------------------------------
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_JobTItem_job'
AND object_id = OBJECT_ID('JobTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_JobTItem_job ON JobTItem (job desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_JobTItem_jobT'
AND object_id = OBJECT_ID('JobTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_JobTItem_jobT ON JobTItem (jobT desc); END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'MOM_INDEX_JobTItem_line'
AND object_id = OBJECT_ID('JobTItem') and is_primary_key=0 )
BEGIN CREATE INDEX MOM_INDEX_JobTItem_line ON JobTItem (line desc); END



---------------------------------------------------------------------------------
------------------------- BOM --------------------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_BOM_JobTitemiD'   
   AND object_id = OBJECT_ID('BOM')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_BOM_JobTitemiD ON BOM (JobTitemiD desc);  END

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_BOM_EstimateIID'   
   AND object_id = OBJECT_ID('BOM')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_BOM_EstimateIID ON BOM (EstimateIID desc);  END


   ---------------------------------------------------------------------------------
------------------------- Milestone --------------------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Milestone_JobTitemiD'   
   AND object_id = OBJECT_ID('Milestone')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Milestone_JobTitemiD ON Milestone (JobTitemiD desc);  END

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Milestone_EstimateIID'   
   AND object_id = OBJECT_ID('Milestone')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Milestone_EstimateIID ON Milestone (EstimateIID desc);  END

   ---------------------------------------------------------------------------------
------------------------- Estimate --------------------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Estimate_RolID'   
   AND object_id = OBJECT_ID('Estimate')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Estimate_RolID ON Estimate (RolID desc);  END

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Estimate_fdate'   
   AND object_id = OBJECT_ID('Estimate')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Estimate_fdate ON Estimate (fdate desc);  END

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Estimate_LociD'   
   AND object_id = OBJECT_ID('Estimate')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Estimate_LociD ON Estimate (LociD desc);  END

     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Estimate_Template'   
   AND object_id = OBJECT_ID('Estimate')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Estimate_Template ON Estimate (Template desc);  END

     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Estimate_Job'   
   AND object_id = OBJECT_ID('Estimate')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Estimate_Job ON Estimate (Job desc);  END

   ---------------------------------------------------------------------------------
------------------------- EstimateI --------------------------------------------------

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_EstimateI_Estimate'   
   AND object_id = OBJECT_ID('EstimateI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_EstimateI_Estimate ON EstimateI (Estimate desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_EstimateI_Line'   
   AND object_id = OBJECT_ID('EstimateI')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_EstimateI_Line ON EstimateI (Line desc);  END


    

  ---------------------------------------------------------------------------------
---------------------------------Invoice------------------------------------------------
 
   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoice_loc'   
   AND object_id = OBJECT_ID('Invoice')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoice_loc ON Invoice (loc desc);  END

   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoice_job'   
   AND object_id = OBJECT_ID('Invoice')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoice_job ON Invoice (job desc);  END 

   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoice_fdate'   
   AND object_id = OBJECT_ID('Invoice')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoice_fdate ON Invoice (fdate desc);  END


      IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoice_Batch'   
   AND object_id = OBJECT_ID('Invoice')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoice_Batch ON Invoice (Batch desc);  END

     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoice_TransID'   
   AND object_id = OBJECT_ID('Invoice')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoice_TransID ON Invoice (TransID desc);  END


     ---------------------------------------------------------------------------------
---------------------------------Invoicei------------------------------------------------
 
   
   IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoicei_ref'   
   AND object_id = OBJECT_ID('Invoicei')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoicei_ref ON Invoicei (ref desc);  END


   
     IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoicei_TransID'   
   AND object_id = OBJECT_ID('Invoicei')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoicei_TransID ON Invoicei (TransID desc);  END


    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE  name = 'MOM_INDEX_Invoicei_job'   
   AND object_id = OBJECT_ID('Invoicei')  and is_primary_key=0  )
   BEGIN  CREATE INDEX MOM_INDEX_Invoicei_job ON Invoicei (job desc);  END 


 
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'MOM_INDEX_PaymentDetails_RefTranID' AND object_id = OBJECT_ID('PaymentDetails'))
BEGIN
  	CREATE INDEX [MOM_INDEX_PaymentDetails_RefTranID] ON [dbo].[PaymentDetails]([RefTranID] DESC);

END





 
