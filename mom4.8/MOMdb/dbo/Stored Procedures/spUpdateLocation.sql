Create PROCEDURE [dbo].[spUpdateLocation]
@Consult INT,
@Account varchar(50),
@LocName varchar(100),
@Address varchar(255),
@status smallint,
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Route int,
@Terr int,--Default salesperson
@Remarks text,
@ContactName varchar(50),
@Phone varchar(50),
@fax varchar(28),
@Cellular varchar(28),
@Email varchar(50),
@Website varchar(50),
@RolAddress varchar(255),
@RolCity varchar(50),
@RolState varchar(2),
@RolZip varchar(10),
@ContactData As [dbo].[tblTypeContactLocation] Readonly,
@Type varchar(50),
@locID int,
@Owner int,
@Stax varchar(25),
@Lat varchar(50),
@Lng varchar(50),
@Custom1 varchar(50),
@Custom2 varchar(50),
@To varchar(250),
@CC varchar(250),
@ToInv varchar(250),
@CCInv varchar(250),
@CreditHold tinyint,
@DispAlert tinyint,
@CreditReason text,
@ContractBill tinyint,
@terms int,
@Docs AS [dbo].[tbltypDocs] Readonly,
@BillRate numeric(30,2),
@OT numeric(30,2),
@NT numeric(30,2),
@DT numeric(30,2),
@Travel numeric(30,2),
@Mileage numeric(30,2),
@Alerts AS [dbo].[tblTypeAlerts] Readonly,
@AlertContacts AS [dbo].[tblTypeAlertContacts] Readonly,
@tblGCandHomeOwner AS tblGCandHomeOwner1 readonly,
@EmailInvoice As bit,
@PrintInvoice As bit,
@CopyToLocAndJob BIT=0,
@Terr2 int=null, ----Second salesperson
@STax2 varchar(25),
@UTax varchar(25),
@Zone int = null,
@UpdatedBy varchar(100),
@Country varchar(50),
@RolCountry varchar(50),
@NoCustomerStatement BIT,
@BusinessTypeID int = null,
@ApplyServiceTypeRule int=0,
@ServiceTypeName varchar(100),
@ProjectPerDepartmentCount int=-1,
@CreditFlag tinyint

AS

DECLARE  @Currentstatus Varchar (50) ,  @CurrentLoctype Varchar (50)  , @CurrentRoute int
SELECT @Currentstatus = Case When Status = 0 Then 'Active' Else 'Inactive' END , @CurrentLoctype=Type ,@CurrentRoute=Route from Loc where Loc = @locID

DECLARE @Rol int
DECLARE @DucplicateAcctID int = 0
DECLARE @DucplicateLocName int = 0

SELECT @DucplicateAcctID = COUNT(1) from Loc where id =@Account and  Loc <> @locID
SELECT @DucplicateLocName = COUNT(1) from Loc where Tag =@LocName and  Loc <> @locID

IF(@DucplicateLocName=0)
BEGIN
	IF(@DucplicateAcctID=0)
	BEGIN
		BEGIN TRANSACTION

		/** Start Logs */
		Exec spUpdateLocationLogs @Consult,
									@Account,
									@LocName,
									@Address,
									@status ,
									@City   ,
									@State  ,
									@Zip 	,
									@Route	,
									@Terr 	,
									@Remarks,
									@ContactName,
									@Phone 		,
									@fax 		,
									@Cellular	,
									@Email 		,
									@Website 	,
									@RolAddress ,
									@RolCity 	,
									@RolState 	,
									@RolZip 	,
									@Type 		,
									@locID 		,
									@Owner 		,
									@Stax 		,
									@Lat 		,
									@Lng 		,
									@Custom1 	,
									@Custom2	,
									@To 		,
									@CC 		,
									@ToInv 		,
									@CCInv 		,
									@CreditHold ,
									@CreditFlag ,
									@DispAlert 	,
									@CreditReason,
									@ContractBill,
									@terms 		,
									@BillRate    ,
									@OT 		,
									@NT 		,
									@DT 		,
									@Travel 	,
									@Mileage 	,
									@EmailInvoice	,
									@PrintInvoice 	,
									@CopyToLocAndJob,
									@Terr2 			,
									@STax2 			,
									@UTax 			,
									@Zone 			,
									@UpdatedBy 		,
									@Country 		,
									@RolCountry		
		/* End Logs*/
  
		select @Rol=Rol from Loc where Loc=@locID
  
		update Rol set
			Name=@LocName,
			City=@RolCity,
			State=@RolState,
			Zip=@RolZip,
			Country = @RolCountry,
			Address=@RolAddress,
			Remarks=@Remarks,
			Contact=@ContactName,
			Phone=@phone,
			Website=@Website,
			EMail=@email,
			Cellular=@Cellular,
			Fax=@fax,
			Lat=@Lat,
			lng=@Lng,
			LastUpdateDate=GETDATE()
			where id =@Rol

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END

		IF (@ContactName<>'')
		BeGIN

		 IF EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @ContactName)
				BEGIN				
				SELECT  @phone,@fax,@Cellular,@email,@ContactName
					UPDATE  Phone SET  Phone=@phone,Fax=@fax,Cell=@Cellular,Email=@email where Rol =@Rol and fDesc = @ContactName 
				END
		   ELSE
				BEGIN
					INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email)VALUES(@Rol,@ContactName,@phone,@fax,@Cellular,@email)
				END  
		END

		declare @sageid varchar(100)
		declare @locsageid varchar(100)
		select @sageid = SageID, @locsageid=ID from Loc where loc= @locID
		if(@locsageid<>@Account)
		begin
			set @sageid = 'NA'
		end

		-------------------------------------Add/Update GC/Ho info ---------------------------------------------
		DECLARE @GContractorID INT;
		DECLARE @HomeOwnerID INT;
 
		Execute spAddGCandHomeOwner @tblGCandHomeOwner,@GContractorID output,@HomeOwnerID output
		--------------------------------------------------------------------------------------------------------

		update Loc set
			Consult=@Consult,
			ID=@Account,
			Tag=@LocName,
			Address=@Address,
			City=@City,
			State=@State,
			Zip=@Zip,
			Country = @Country,
			Type=@Type,
			Route=@Route,
			Terr=@Terr, 
			Terr2=@Terr2, 
			Owner=@Owner,
			STax=@Stax,
			Custom1=@Custom1,
			Custom2=@Custom2,
			Custom14=@To,
			Custom15=@CC,
			Custom12=@ToInv,
			Custom13=@CCInv,
			Status=@status,
			Remarks=@Remarks,
			DispAlert=@DispAlert,
			Credit=@CreditHold,
			CreditFlag=@CreditFlag,
			CreditReason=@CreditReason,
			SageID=@sageid,
			Billing=@ContractBill,
			DefaultTerms=@terms,
			BillRate=@BillRate,
			RateOT=@OT,
			RateNT=@NT,
			RateDT=@DT,
			RateTravel=@Travel,
			RateMileage=@Mileage,
			HomeOwnerID=isnull(@HomeOwnerID,0),
			GContractorID=isnull(@GContractorID,0),
			EmailInvoice=@EmailInvoice,
			PrintInvoice=@PrintInvoice,
			STax2	=	@STax2,
			UTax	=	@UTax,
			Zone	=	@Zone,
			NoCustomerStatement=@NoCustomerStatement,
			BusinessType=@BusinessTypeID
			--mapaddress=@MAPAddress
		where Loc = @locID

		------when user making a location inactive to make all projects/equipment inactive
		if(@status=1)
		BEGIN 
				-- Check if any Open Tickets
				IF EXISTS(Select 1 from TicketO Where LID = @LocID and Assigned != 4)
				BEGIN
					EXEC spVoidedTicket @LocID  = @LocID , @UpdatedBy=@UpdatedBy , @Tickets=0
				END
		 

			-- Update Logs for Contract
			INSERT INTO log2 (fUser, Screen, Ref, Field, OldVal, NewVal)
			SELECT @UpdatedBy,'Job',Job,'Status'
				,Case Status When 0 Then 'Active'
					when 2 Then 'Hold'
					when 3 Then 'Completed'
					else '' end
				,'Closed'
			FROM Contract 
			WHERE loc = @locID and Status <> 1 ;

			UPDATE Contract set Contract.Status=1
			WHERE  Contract.loc = @locID

			-- Update Logs for Project
			INSERT INTO log2 (fUser, Screen, Ref, Field, OldVal, NewVal)
			SELECT @UpdatedBy,'Project',id,'Status'
				,Case Status When 0 Then 'Open'
					when 2 Then 'Hold'
					when 3 Then 'Completed'
					else '' end
				,'Closed'
			FROM job 
			WHERE  loc = @locID and Status <> 1 ;

			UPDATE Job set Job.Status=1
			WHERE  Job.loc = @locID
	
			-- Update Logs for Equipment
			INSERT INTO log2 (fUser, Screen, Ref, Field, OldVal, NewVal)
			SELECT @UpdatedBy,'Elev',id,'Status'
				,Case Status When 0 Then 'Active'
					else '' end
				,'Inactive'
			FROM Elev
			WHERE  loc = @locID and Status <> 1 ;
			-- We won't need to update log for Elev table.  There is a trigger do that
			-- Just need to pass UpdatedBy for the trigger
			--DECLARE @Ctx varbinary(128)     
			--SELECT @Ctx = convert(varbinary(128), @UpdatedBy)    
			--SET context_info @Ctx 
			UPDATE Elev set Elev.Status=1
			WHERE  Elev.loc = @locID
		END


		---------Copy to Billing Code  Project
        IF( @CopyToLocAndJob = 1 )
        BEGIN
			---Project
			UPDATE job
                SET     BillRate = @BillRate,
                        RateOT = @OT,
                        RateDT = @DT,
                        RateNT = @NT,
                        RateMileage = @Mileage,
                        RateTravel = @Travel
                WHERE  Loc = @locID 
        END
        ------------------------

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END
 
		update Elev set Owner=@Owner where Loc=@locID
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END
 
		update Job set Owner=@Owner, Custom20=@Route where Loc=@locID
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END

		update TicketO set Owner=@Owner where LID=@locID and LType = 0
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END
    
 
		exec spUpdateDocInfo @docs
                  
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END   

		delete from tblAlertContacts where AlertID in (select AlertID from tblAlerts where ScreenName='Loc' and ScreenID = @locID)

		delete from tblAlerts where ScreenName='Loc' and ScreenID = @locID

		insert into tblAlerts (
			[ScreenID] ,
			[ScreenName] ,
			[AlertCode] ,
			[AlertSubject] ,
			[AlertMessage]
		)

		select @locID, 'Loc', AlertCode, AlertSubject, AlertMessage from @Alerts

		insert into tblAlertContacts
		(
			[ScreenID],
			[ScreenName],
			[AlertID],
			[Email],
			[Text]
		)

		select 
			c.[ScreenID],
			c.[ScreenName],
			a.[AlertID],
			[Email],
			[Text]
		from @AlertContacts c inner join tblAlerts a on c.AlertCode = a.AlertCode and a.ScreenName='Loc' and a.ScreenID = @locID
 
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END  

		--if Location Type or route mechanic is edited then update all service type fields
		
		IF(@ApplyServiceTypeRule = 1 and isnull(@ServiceTypeName,'') <> '' and isnull(@ProjectPerDepartmentCount,0) =  1)

		BEGIN

	   	IF(@CurrentLoctype<>@Type or @CurrentRoute <> @Route)	 
         
        BEGIN 

		-- Adding logs for project/contract

		INSERT INTO [Log2] ( [fUser] ,[Screen],[Ref],[Field],[OldVal],[NewVal])

SELECT     @UpdatedBy , 'job' , j1.ID ,'Finance - Service Type', j1.CType,t1.type  
    FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 
UNION ALL

SELECT     @UpdatedBy , 'Project' , j1.ID ,'Finance - Service Type', j1.CType,t1.type  
    FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 
UNION ALL
SELECT     @UpdatedBy , 'Project' , j1.ID ,'Finance - Labor Wage',  (SELECT  fDesc FROM PRWage WHERE id=j1.WageC),(SELECT  fDesc FROM PRWage WHERE id=t1.LaborWageC)        
     FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 
UNION ALL
SELECT    @UpdatedBy , 'Project' , j1.ID ,'Finance - Billing Code', (SELECT  Name FROM Inv WHERE id=j1.GLRev),(SELECT  Name FROM Inv WHERE id=t1.InvID)
     FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 
UNION ALL
SELECT     @UpdatedBy , 'Project' , j1.ID ,'Finance - Inerest GL',   (SELECT  fDesc FROM Chart WHERE id=j1.InterestGL),(SELECT  fDesc FROM Chart WHERE id=t1.InterestGL)
    FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 
UNION ALL
SELECT     @UpdatedBy , 'Project' , j1.ID ,'Finance - ExpenseGL',    (SELECT  fDesc FROM Chart WHERE id=j1.GL),(SELECT  fDesc FROM Chart WHERE id=t1.ExpenseGL)
    FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
 

					 
	    -- End logs


 	update j1 set 
	j1.WageC =       isnull(t1.LaborWageC,j1.WageC),
	j1.GLRev =       isnull(t1.InvID,j1.GLRev),
	j1.GL =          isnull(t1.ExpenseGL,j1.GL)  , 
	J1.InterestGL =  isnull(t1.InterestGL,j1.InterestGL) ,
	j1.CType  =      isnull(@ServiceTypeName,j1.ctype) 
	FROM  Job j1    
	INNER JOIN loc       on loc.loc=j1.Loc   and loc.Type=@Type and loc.Route=@Route and loc.loc=@locID
	INNER JOIN LType  t1 on t1.LocType=Loc.Type and t1.Type=@ServiceTypeName 
	WHERE j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @ServiceTypeName)
	and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @ServiceTypeName)
	and isnull(t1.Route,'') <> ''  and isnull(t1.Department,'') <> ''
      
 END
 
		
		END
		----------------------
		COMMIT TRANSACTION

	END  
	ELSE
	BEGIN
		RAISERROR ('Account # already exists, please use different Account # !', 16, 1)  
		RETURN
	END
END 
ELSE
BEGIN
	RAISERROR ('Location name already exists, please use different Location name !', 16, 1)  
	RETURN
END