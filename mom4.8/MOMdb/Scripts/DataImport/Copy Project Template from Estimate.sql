--------- # Copy Project Template from Estimate  # -----  
  
	DECLARE @jobT int,    
	@fdesc varchar(50),    
	@Type smallint,    
	@NRev smallint,    
	@NDed smallint,    
	@Count int,    
	@Remarks varchar(max),     
	@InvExp int,    
	@InvServ int,    
	@Wage int,    
	@UnrecognizedRevenue int = null,  
	@UnrecognizedExpense int = null,  
	@RetainageReceivable int = null,  
	@CType varchar(15),    
	@Status smallint = 0,    
	@Charge smallint,    
	@Post smallint,    
	@fInt smallint,    
	@GLInt smallint,    
	@JobClose smallint,    
	@tempRev varchar(150),    
	@RevRemarks varchar(max),    
	@alertType smallint,    
	@alertMgr bit,    
	@MilestoneMgr bit,    
	@BomItem AS tblTypeBomItem  ,    
	@MilestonItem AS tblTypeMilestoneItem  ,    
	@CustomTabItem AS tblTypeCustomTabItem3  ,    
	@CustomItem AS tblTypeCustomItem   ,
	@TargetHPermission smallint
  

	DECLARE @mid int    
	DECLARE @bid int    
	DECLARE @jobTItemId int    
	DECLARE @jtype smallint    
	DECLARE @jfDesc varchar(255)    
	DECLARE @jCode varchar(10)    
	DECLARE @jBudget numeric(30,2)    
	DECLARE @Line smallint    
	DECLARE @Btype smallint    
	DECLARE @Bitem int    
	DECLARE @QtyReq numeric(30,2)    
	DECLARE @UM varchar(50)    
	DECLARE @ScrapFact numeric(30,2)    
	DECLARE @BudgetUnit numeric(30,2)    
	DECLARE @BudgetExt numeric(30,2)    
	DECLARE @MileName varchar(150)    
	DECLARE @RequiredBy datetime    
	DECLARE @LeadTime numeric(30,2)    
	DECLARE @ProjAcquDate datetime    
	DECLARE @OrgDep int    
	DECLARE @Amount numeric(30,2)  
	DECLARE @tblCustomFieldsId int    
	DECLARE @tblTabID int    
	DECLARE @Label VARCHAR(50)    
	DECLARE @TabLine SMALLINT    
	DECLARE @Value VARCHAR(50)    
	DECLARE @Format smallint    
	DECLARE @CustomID int    
	DECLARE @LabItem int    
	DECLARE @MatItem int    
	DECLARE @LabMod numeric(30,2)    
	DECLARE @MatMod numeric(30,2)    
	DECLARE @LabExt numeric(30,2)    
	DECLARE @LabRate numeric(30,2)    
	DECLARE @LabHours numeric(30,2)    
	DECLARE @SDate datetime    
	DECLARE @Vendor int    
	DECLARE @OrderNo int    
	DECLARE @GroupID int  
	DECLARE @IsAlert bit    
	DECLARE @TeamMember varchar(max)
	DECLARE @TeamMemberDisplay varchar(max)    

	--------------------------------------------------SET Values------------------------>

	DECLARE @EstimateID int=51629

	SELECT @Type = TYPE FROM  JobT WHERE id= (SELECT template FROM Estimate WHERE id= @EstimateID)

	SELECT @InvExp=InvExp,@InvServ= InvServ,@Wage=  Wage,@NRev=NRev,@CType=CType,@GLInt=GLInt FROM JobT WHERE id  = 2


	SET @fdesc='LULA'

	INSERT INTO @BomItem ( JobT, Job , JobTItemID , Line , OrderNo , fDesc  , Code  ,BType)

	            SELECT    0 , 0, 0  ,  Line , Line , fDesc , code , type FROM EstimateI WHERE Estimate=@EstimateID and type=1

			   	INSERT INTO @MilestonItem ( JobT, Job ,  Line , OrderNo , fDesc     )

	            SELECT    0 , 0,    Line , Line , fDesc   FROM EstimateI WHERE Estimate=@EstimateID and type=0

	-------------------------------------------------------------------------------------->
	BEGIN TRANSACTION         
	 
	IF (SELECT count(1) FROM JobT j where j.fDesc=@fdesc  ) > 0

	BEGIN      

	RAISERROR ('Template name already exist. Please use a different template name.', 16, 1)
	
	ROLLBACK TRANSACTION 
	
	RETURN 
	
	END    
       
	INSERT INTO [dbo].[JobT]    
	([fDesc] ,[Type] ,[NRev] ,[NDed] ,[Count] ,[Remarks] ,[InvExp] ,[InvServ]    
	,[Wage] ,[UnrecognizedRevenue] ,[UnrecognizedExpense] ,[RetainageReceivable]   
	,[CType] ,[Status]  ,[Charge]  ,[Post]  ,[fInt]  ,[GLInt]  ,[JobClose] ,[AlertType]    
	,[AlertMgr]  ,[TemplateRev]  ,[RevRemarks], TargetHPermission
	)    
	VALUES    
	(@fdesc ,@Type  ,@NRev  ,@NDed  ,@Count 	,@Remarks  ,@InvExp  ,@InvServ    
	,@Wage ,@UnrecognizedRevenue ,@UnrecognizedExpense  
	,@RetainageReceivable ,@CType ,@Status  ,@Charge ,@Post ,@fInt ,@GLInt  ,@JobClose ,@alertType  ,@alertMgr  ,@tempRev  ,@RevRemarks  , @TargetHPermission
	)    
    
	SET @jobT=SCOPE_IDENTITY()    
    
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0    

	BEGIN      

	RAISERROR ('Error Occured', 16, 1) 
	
	ROLLBACK TRANSACTION
	
	RETURN    

	END    
    
	DECLARE db_cursor CURSOR FOR     
    
	SELECT fDesc, Code, Line, Btype, QtyReq, UM, BudgetUnit, BudgetExt, LabItem, MatItem,     
	LabMod, MatMod, LabExt, LabRate, LabHours, SDate, VendorId ,OrderNo,GroupID  FROM  @BomItem     
    
	OPEN db_cursor    
	
	FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,    
	@LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor ,@OrderNo ,@GroupID  
    
	WHILE @@FETCH_STATUS = 0   
	
	BEGIN   
	
	INSERT INTO JobTItem    
	(JobT, Job, Type, fDesc,  Code,Actual,Budget,  Line,[Percent],Comm,  Modifier,ETC,ETCMod, Labor,Stored,BHours   ,OrderNo, GroupID
	)    
	VALUES(@jobT, 0, 1, @jfDesc, @jCode, 0, @BudgetExt, @Line, 0, 0,  @MatMod, @LabExt, @LabMod,  0, 0,@LabHours   ,@OrderNo ,@GroupID 
	)    

	SET @jobTItemId = SCOPE_IDENTITY()    
    
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0  
	
	BEGIN      

	RAISERROR ('Error Occured', 16, 1) 
	
	ROLLBACK TRANSACTION        

	RETURN    
	
	END    
       
         
	INSERT INTO [dbo].[BOM]    
	([JobTItemID],[Type],[QtyRequired], [UM],[BudgetUnit],[BudgetExt], [LabItem],[MatItem],[LabRate], [SDate],[Vendor])    
	VALUES (@jobTItemId, @Btype, @QtyReq, 	@UM, @BudgetUnit, @BudgetExt,   @LabItem, @MatItem, @LabRate, @SDate,@Vendor)    
        
	FETCH NEXT FROM db_cursor INTO     
	@jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,    
	@LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor    ,@OrderNo ,@GroupID 
    
	END      
    
	CLOSE db_cursor     
	
	DEALLOCATE db_cursor    
    
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0  
	
	BEGIN      	RAISERROR ('Error Occured', 16, 1)    
	
	ROLLBACK TRANSACTION    
	
	RETURN  
	
	END    
       
	DECLARE db_cursor1 CURSOR FOR     
    
	select jtype, fdesc, jCode, Line, MilesName,RequiredBy,LeadTime,Type,Amount,OrderNo ,GroupID from @MilestonItem     
    
	OPEN db_cursor1   
	
	FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep,@Amount  ,@OrderNo ,@GroupID  
    
	WHILE @@FETCH_STATUS = 0  
	
	BEGIN 
	
	INSERT INTO JobTItem  
	
	( JobT, 	Job, 	Type, fDesc,    	Code,  Actual, Budget, Line,   	[Percent], Comm, Modifier, ETC,  ETCMod, Labor, Stored    ,OrderNo ,GroupID)  
	
	values(@jobT, 0, @jType, @jfDesc, @jCode, 0, @Amount, @Line,0, 0, 0, 0, 0, 0, 0,@OrderNo ,@GroupID)    

	SET @jobTItemId = SCOPE_IDENTITY()    
    
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0    

	BEGIN      

	RAISERROR ('Error Occured', 16, 1)    
	
	ROLLBACK TRANSACTION       
	
	RETURN    

	END       
         
    
	INSERT INTO [dbo].[Milestone]   	([JobTItemID]  ,[MilestoneName]  ,[RequiredBy] 		,[CreationDate]     ,[ProjAcquistDate]   ,[Type]     ,[Amount] )   
	
	VALUES (@jobTItemId    ,@MileName  ,@RequiredBy    ,GETDATE () ,@ProjAcquDate   ,@OrgDep   ,@Amount)    
        
	FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep,@Amount ,@OrderNo ,@GroupID    
	END      
    
	CLOSE db_cursor1      

	DEALLOCATE db_cursor1    
    
	--------------------------------------- insert custom template ---------------------------------------    
     
	DECLARE db_cursor2 CURSOR FOR     
    
	SELECT [ID], [tblTabID], [Label], [Line], [Format] , OrderNo, IsAlert, TeamMember, TeamMemberDisplay FROM @CustomTabItem    
    
	OPEN db_cursor2      

	FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Format, @OrderNo, @IsAlert, @TeamMember, @TeamMemberDisplay
    
	WHILE @@FETCH_STATUS = 0   
	
	BEGIN        
  
	IF(@tblTabID = 0)    

	BEGIN  SET @tblTabID = NULL END    
    
	INSERT INTO [dbo].[tblCustomFields] ([tblTabID], [Label], [Line], [Format], OrderNo, IsAlert, TeamMember, TeamMemberDisplay)    

	VALUES (@tblTabID, @Label, @TabLine, @Format,@OrderNo, @IsAlert, @TeamMember, @TeamMemberDisplay)    
      
	SET @tblCustomFieldsId=SCOPE_IDENTITY()    
    
	IF(SELECT TOP 1  1 FROM @CustomItem WHERE Line = @TabLine) = 1    
	BEGIN    
      
	INSERT INTO [dbo].[tblCustom](tblCustomFieldsID, Line, Value)    

	SELECT @tblCustomFieldsId, Line, Value FROM @CustomItem WHERE Line = @TabLine    
      
	END    
    
	INSERT INTO [dbo].[tblCustomJobT] ([JobTID],[tblCustomFieldsID])    

	VALUES (@jobT ,@tblCustomFieldsId)    
      
	FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Format ,@orderno, @IsAlert, @TeamMember, @TeamMemberDisplay    
	END      
    
	CLOSE db_cursor2     
	
	DEALLOCATE db_cursor2    
    
	COMMIT TRANSACTION    
    
	SELECT @jobT