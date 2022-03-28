--================================================================================

 
 
GO

CREATE TRIGGER trgTicketD ON TicketD AFTER DELETE  
AS
BEGIN

      INSERT INTO TicketD_Log ( [TicketID]    ,[Date] , [data])
      SELECT  t1.ID  , (GETDATE()) ,  ( SELECT   [ID]
      ,[CDate] ,[DDate] ,[EDate] ,[fWork]     ,[Job]       ,[Loc] ,[Elev] ,[Type]  ,[Charge]        
      ,[ClearCheck]    ,[ClearPR] ,[Total]    ,[Reg]       ,[OT]   ,[DT]  ,[TT]  ,[Zone]  ,[Toll]
      ,[OtherE]    ,[Status]  ,[Invoice]      ,[Level]     ,[Est]  ,[Cat] ,[Who] ,[fBy]   ,[SMile] ,[EMile] ,[fLong]
      ,[Latt] ,[WageC] ,[Phase]   ,[Car]      ,[CallIn]    ,[Mileage] ,[NT]  ,[CauseID] ,[CauseDesc]  ,[fGroup] ,[PriceL]
      ,[WorkOrder] ,[TimeRoute]   ,[TimeSite] ,[TimeComp]  ,[Source]  ,[Internet]  ,[RBy] ,[Custom1]
      ,[Custom2]   ,[Custom3]     ,[Custom4]  ,[Custom5]   ,[CTime]  ,[DTime]     ,[ETime]  ,[BRemarks]  ,[WorkComplete]  ,[BReview]   ,[PRWBR]  ,[pdaticketid]
      ,[AID]     ,[Custom6] ,[Custom7]  ,[Custom8] ,[Custom9] ,[Custom10] ,[CPhone]
      ,[RegTrav] ,[OTTrav]    ,[DTTrav] ,[NTTrav]  ,[Email]   ,[ManualInvoice]  ,[QBInvoiceID]  ,[LastUpdateDate]
      ,[QBTimeTxnID]  ,[TransferTime]  ,[QBServiceItem]  ,[QBPayrollItem]
      ,[CustomTick1]  ,[CustomTick2]   ,[CustomTick3] ,[CustomTick4] ,[TimesheetID]  ,[HourlyRate]
      ,[CustomTick5]  ,[JobCode]       ,[Import1] ,[Import2]   ,[Import3]   ,[Import4] ,[Import5]
      ,[Recurring]    ,[JobItemDesc]   ,[PrimarySyncID]
      ,[FMSEtid]      ,[PrevEquipLoc]  ,[fmsimportdate]  ,[break_time]   ,[Comments]
      ,[PartsUsed]  FROM deleted where id=t1.ID  FOR XML AUTO)   
	   FROM deleted t1

END

GO
 
CREATE TRIGGER trgTicketDPDA ON TicketDPDA
AFTER DELETE  
AS
BEGIN

      INSERT INTO [TicketDPDA_Log] ( [TicketID]    ,[Date] , [data])
      SELECT  t1.ID  , (GETDATE()) ,  ( SELECT   [ID]
      ,[CDate] ,[DDate] ,[EDate] ,[fWork]     ,[Job]       ,[Loc] ,[Elev] ,[Type]  ,[Charge]       
      ,[ClearCheck]    ,[ClearPR] ,[Total]    ,[Reg]       ,[OT]   ,[DT]  ,[TT]  ,[Zone]  ,[Toll]
      ,[OtherE]    ,[Status]  ,[Invoice]      ,[Level]     ,[Est]  ,[Cat] ,[Who] ,[fBy]   ,[SMile] ,[EMile] ,[fLong]
      ,[Latt] ,[WageC] ,[Phase]   ,[Car]      ,[CallIn]    ,[Mileage] ,[NT]  ,[CauseID] ,[CauseDesc]  ,[fGroup] ,[PriceL]
      ,[WorkOrder] ,[TimeRoute]   ,[TimeSite] ,[TimeComp]  ,[Source]  ,[Internet]    ,[Custom1]
      ,[Custom2]   ,[Custom3]     ,[Custom4]  ,[Custom5]       ,[BRemarks]  ,[WorkComplete]    
      ,[AID]     ,[Custom6] ,[Custom7]  ,[Custom8] ,[Custom9] ,[Custom10] ,[CPhone]
      ,[RegTrav] ,[OTTrav]    ,[DTTrav] ,[NTTrav]  ,[Email]     ,[QBInvoiceID]   ,[break_time]   ,[Comments]
      ,[PartsUsed]  FROM deleted where id=t1.ID  FOR XML AUTO)   
	   FROM deleted t1

END;

 
GO 
 
CREATE TRIGGER trgTicketO ON TicketO
AFTER DELETE  
AS
BEGIN

      INSERT INTO [TicketO_Log] ( [TicketID]    ,[Date] , [data])
      SELECT  t1.ID  , (GETDATE()) ,  ( SELECT   [ID]
      ,[CDate] ,[DDate] ,[EDate] ,[fWork]     ,[Job]  ,LID  
      , [Level]     ,[Est]  ,[Cat] ,[Who] ,[fBy]   ,[SMile] ,[EMile] ,[fLong]
      ,[Latt]   ,[CallIn]    ,   [fGroup] ,[PriceL]
      ,[WorkOrder] ,[TimeRoute]   ,[TimeSite] ,[TimeComp]  ,[Source]   ,[Custom1]
      ,[Custom2]   ,[Custom3]     ,[Custom4]  ,[Custom5]       ,[BRemarks]  ,  
       [AID]     ,[Custom6] ,[Custom7]  ,[Custom8] ,[Custom9] ,[Custom10] ,[CPhone] 
       FROM deleted where id=t1.ID  FOR XML AUTO)   
	   FROM deleted t1

END;

 GO
 
-- ===============================================================================
--Modified By: Thomas
--Modified On: 05 Nov 2019	
--Description:  ES-2860: Updated for log on status change
-- ===============================================================================

 
GO
CREATE TRIGGER [dbo].[TriggerAfteElev] ON [dbo].[Elev] 
 FOR UPDATE
AS
BEGIN
	Declare @Loc      INT;
	Declare @Unit     VARCHAR(20);
	Declare	@fDesc    VARCHAR(50);
	Declare	@Type     VARCHAR(20);
	Declare	@Cat      VARCHAR(20);
	Declare	@Manuf    VARCHAR(20);
	Declare	@Serial   VARCHAR(50);
	Declare	@State    VARCHAR(25);
	Declare	@Since    VARCHAR(50);
	Declare	@Last     VARCHAR(50);
	Declare	@Price    NUMERIC(30, 2);
	Declare	@Status    VARCHAR(50);
	Declare	@ID       INT;
	Declare	@Remarks  varchar(max);
	Declare	@Install  VARCHAR(50);
	Declare	@Category VARCHAR(20);
	Declare	@template int;
	Declare	@Building varchar(20) = null;
	Declare	@UpdatedBy varchar(100) = null;
	
	Select @ID = i.ID from inserted i;
	select @Loc=i.Loc from inserted i;	
	select @Unit=i.Unit from inserted i;	
	select @fDesc=i.fDesc from inserted i;
	select @Type=i.Type from inserted i;
	select @Cat=i.Cat from inserted i;
	select @Manuf=i.Manuf from inserted i;
	select @Serial=i.Serial from inserted i;
	select @State=i.State from inserted i;
	select @Since=convert(varchar(150), i.Since, 101)  from inserted i;
	select @Last=convert(varchar(150), i.Last, 101) from inserted i;
	select @Price=i.Price from inserted i;
	select @Status=Case When i.Status = 0 Then 'Active' Else 'Inactive' END from inserted i;
	select @Building=i.Building from inserted i;

	select @Remarks =  Remarks from Elev Where ID =@ID -- i.Remarks from inserted i; 
	Select @template = i.Template from inserted i;
	Select @Install = convert(varchar(150), i.Install, 101) from inserted i;
	Select @Category = i.Category from inserted i;
	Select @UpdatedBy =  CONTEXT_INFO()

	
	Declare @TemplateName varchar(150)
	SELECT Top 1 @TemplateName =  fDesc from elevt where ID = @template
	declare @currentloc int 
	declare @currentlocname varchar(50)
	declare @newlocname varchar(50)
	select @currentloc =  i.Loc from deleted i;
	Select top 1 @currentlocname= tag from loc where loc = @currentloc
	select top 1 @newlocname = tag from loc where loc =@Loc
	Declare @CurrentUnit Varchar(50)
	Select @CurrentUnit =  i.Unit from deleted i;
	Declare @CurrentfDesc Varchar(50)
	Select @CurrentfDesc =  i.fDesc from deleted i;
	Declare @CurrentType Varchar(50)
	Select @CurrentType =  i.Type from deleted i;
	Declare @CurrentCat Varchar(50)
	Select @CurrentCat =  i.Cat from deleted i;
	Declare @CurrentManuf Varchar(50)
	Select @CurrentManuf =  i.Manuf from deleted i;
	Declare @CurrentSerial Varchar(50)
	Select @CurrentSerial =  i.Serial from deleted i;
	Declare @CurrentState Varchar(50)
	Select @CurrentState =  i.State from deleted i;
	Declare @CurrentSince Varchar(50)
	Select @CurrentSince = convert(varchar(50), i.Since, 101)  from deleted i; 
	Declare @CurrentLast Varchar(50)
	Select @CurrentLast = convert(varchar(50), i.Last, 101) from deleted i;	 
	Declare @CurrentPrice NUMERIC(30, 2)
	Select @CurrentPrice = i.Price  from deleted i;
	Declare @CurrentStatus Varchar(50)
	Select @CurrentStatus = Case When i.Status = 0 Then 'Active' Else 'Inactive' END from deleted i;
	Declare @CurrentBuilding Varchar(50)
	Select @CurrentBuilding = i.Building From deleted i;
	--Declare @CurrentRemarks Varchar(1000)
	--Select @CurrentRemarks = i.Remarks From deleted i;
	Declare @CurrentTempID int
	SElect @CurrentTempID = i.Template From deleted i;
	
	Declare @CurrentTemplate varchar(150)
	SELECT Top 1 @CurrentTemplate =  fDesc from elevt where ID = @CurrentTempID
	
	Declare @CurrentInstall Varchar(50)
	Select @CurrentInstall = convert(varchar, i.Install, 101) from deleted i; 
	Declare @CurrentCategory Varchar(50)
	Select @CurrentCategory = i.Category from deleted i;
	
	Declare @Val varchar(1000)
	if update(Loc)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Location' order by CreatedStamp desc )
	if(@Val<>@newlocname)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Location',@Val,@newlocname
	end	
	Else IF (@currentlocname <> @newlocname)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Location',@currentlocname,@newlocname
	END
	end
		
	if update(Unit)
		begin 
        Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Unit' order by CreatedStamp desc )
	if(@Val <>@Unit)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Unit',@Val,@Unit
	end	
	Else IF (@CurrentUnit <> @Unit)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Unit',@CurrentUnit,@Unit
	END
	end
	if update(fDesc)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Description' order by CreatedStamp desc )
	if(@Val <>@fDesc) 
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Description',@Val,@fDesc
	end	
	Else IF (@CurrentfDesc <> @fDesc)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Description',@CurrentfDesc,@fDesc
	END
	end
	if update (Type)
	begin 
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Type' order by CreatedStamp desc )
	if(@Val <>@Type)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Type',@Val,@Type
	end	
	Else IF (@CurrentType <> @Type)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Type',@CurrentType,@Type
	END
	end
	if update(Cat)
	begin 
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Service Type' order by CreatedStamp desc )
	if(@Val <> @Cat)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Service Type',@Val,@Cat
	end
	Else if(@CurrentCat <> @Cat)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Service Type',@CurrentCat,@Cat
	end
	end
	if update(Manuf)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Manufacturer' order by CreatedStamp desc )
	if(@Val <> @Manuf)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Manufacturer',@Val,@Manuf
	end	
	Else if(@CurrentManuf <> @Manuf)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Manufacturer',@CurrentManuf,@Manuf
	end	
	end
	if update(Serial)
	begin
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Serial#' order by CreatedStamp desc )	
	if(@Val <> @Serial) 
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Serial#',@Val,@Serial
	end	
	Else IF (@CurrentSerial <> @Serial)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Serial#',@CurrentSerial,@Serial
	END
	end
	if update(State)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Unique#' order by CreatedStamp desc )
	if(@Val <>@State)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Unique#',@Val,@State
	end	
	Else if(@CurrentState <> @State)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Unique#',@CurrentState,@State
	end	
	end
	set @Val=null
	if Update (Since)
	begin 
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Serviced Since' order by CreatedStamp desc )	
	if(@Val <> @Since) 
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Serviced Since',@Val,@Since
	end
 	Else if(@CurrentSince <> @Since) 
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Serviced Since',@CurrentSince,@Since
	end
	end
	if Update (Last)
	begin 
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Last Serviced' order by CreatedStamp desc )
	if(@Val <> @Last)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Last Serviced',@Val,@Last
	end
	Else if(@CurrentLast <> @Last)
	begin
	exec log2_insert @UpdatedBy,'Elev',@ID,'Last Serviced',@CurrentLast,@Last
	end
	end
	if update(Price)
	begin
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Price' order by CreatedStamp desc )	
	if(@Val <> cast(@Price as varchar(255)))
	begin 
	 declare @PriceVar varchar(255)  = cast ( @Price as varchar(255)) 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Price',@Val,@Price
	end
	Else IF (@CurrentPrice <> @Price)
	BEGIN
	exec log2_insert @UpdatedBy,'Elev',@ID,'Price',@CurrentPrice,@Price
	END
	end
	if Update(Status) AND ISNULL(@UpdatedBy,'') != ''-- Thomas: condition ISNULL(@UpdatedBy,'') != '' to ignore case updated status on changed Location status
	begin 
	Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Status' order by CreatedStamp desc )
	if(@Val <> @Status)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Status',@Val,@Status
	end
	Else if(@CurrentStatus <> @Status)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Status',@CurrentStatus,@Status
	end
	end
	if update (Building)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Building' order by CreatedStamp desc )
	if(@Val <> @Building)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Building',@Val,@Building
	end
	 Else if(@CurrentBuilding <> @Building)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Building',@CurrentBuilding,@Building
	end
	end
	if update(Remarks)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Remarks' order by CreatedStamp desc )
	if(@Val <> @Remarks)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Remarks',@Val,@Remarks
	end
	Else if(@Val IS Null AND @Remarks != '')
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Remarks','',@Remarks
	end
	end
	if update(template)
	begin 
	Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Template' order by CreatedStamp desc )
	if(@Val <> @TemplateName)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Template',@Val,@TemplateName
	end
	Else if(@CurrentTemplate <> @TemplateName)
	begin 
	exec log2_insert @UpdatedBy,'Elev',@ID,'Template',@CurrentTemplate,@TemplateName
	end
	end
	if update(Install)	
	BEGIN 
		Set @Val =(select Top 1 newVal from log2 where screen='Elev' and ref= @ID and Field='Installed Date' order by CreatedStamp desc )	 
		if(@Val <> @Install)
		begin
		exec log2_insert @UpdatedBy,'Elev',@ID,'Installed Date',@Val,@Install
		END
		ELSE if(@CurrentInstall <> @Install)
		BEGIN	
		exec log2_insert @UpdatedBy,'Elev',@ID,'Installed Date',@CurrentInstall,@Install
		end	
	END
	if Update(Category)
		begin 
		Set @Val =(select Top 1 newVal  from log2 where screen='Elev' and ref= @ID and Field='Category' order by CreatedStamp desc )
		if(@Val <> @Category)
		begin 
		exec log2_insert @UpdatedBy,'Elev',@ID,'Category',@Val,@Category
		end	
		Else if(@CurrentCategory <> @Category)
		begin 
		exec log2_insert @UpdatedBy,'Elev',@ID,'Category',@CurrentCategory,@Category
		end	
		end
END;

ALTER TABLE [dbo].[Elev] ENABLE TRIGGER [TriggerAfteElev]
GO


-- ===============================================================================
--Created By: NK
--Modified On: 28 Nov 2019	
--Description: Created TRIGGER to track BOM items
-- ===============================================================================

 
GO 
CREATE TRIGGER [dbo].[trgJobTItem] ON [dbo].[JobTItem] 

AFTER   DELETE   

AS

BEGIN

      INSERT INTO JobTItem_Log ( 
	        [Date]
	       ,[ID]
	       ,[JobT]
           ,[Job]
           ,[Type]
           ,[fDesc]
           ,[Code]
           ,[Actual]
           ,[Budget]
           ,[Line]
           ,[Percent]
           ,[Comm]
           ,[Stored]
           ,[Modifier]
           ,[ETC]
           ,[ETCMod]
           ,[THours]
           ,[FC]
           ,[Labor]
           ,[BHours]
           ,[GL]
           ,[OrderNo]
           ,[GroupID]
           ,[TargetHours] )
            SELECT 
			GETDATE()
		   ,[ID]
	       ,[JobT]
           ,[Job]
           ,[Type]
           ,[fDesc]
           ,[Code]
           ,[Actual]
           ,[Budget]
           ,[Line]
           ,[Percent]
           ,[Comm]
           ,[Stored]
           ,[Modifier]
           ,[ETC]
           ,[ETCMod]
           ,[THours]
           ,[FC]
           ,[Labor]
           ,[BHours]
           ,[GL]
           ,[OrderNo]
           ,[GroupID]
           ,[TargetHours]    
	   FROM deleted t1

END

GO 
ALTER TABLE [dbo].[JobTItem] ENABLE TRIGGER [trgJobTItem]
 
 
GO 
CREATE TRIGGER [dbo].[trgBOM] ON [dbo].[BOM] 

AFTER   DELETE   

AS

BEGIN

      INSERT INTO BOM_Log ( 
	    [Date]
	   ,[ID]
      ,[JobTItemID]
      ,[Type]
      ,[Item]
      ,[QtyRequired]
      ,[UM]
      ,[ScrapFactor]
      ,[BudgetUnit]
      ,[BudgetExt]
      ,[Vendor]
      ,[Currency]
      ,[EstimateIId]
      ,[MatItem]
      ,[LabItem]
      ,[SDate]
      ,[LabRate])
            SELECT 
			GETDATE()
	  ,[ID]
      ,[JobTItemID]
      ,[Type]
      ,[Item]
      ,[QtyRequired]
      ,[UM]
      ,[ScrapFactor]
      ,[BudgetUnit]
      ,[BudgetExt]
      ,[Vendor]
      ,[Currency]
      ,[EstimateIId]
      ,[MatItem]
      ,[LabItem]
      ,[SDate]
      ,[LabRate]  
	   FROM deleted t1

END
  
GO 
ALTER TABLE [dbo].[BOM] ENABLE TRIGGER [trgBOM]
GO

 

Create TRIGGER [dbo].[trgJobI] ON [dbo].[JobI] 

AFTER   DELETE   

AS

BEGIN

   INSERT INTO [dbo].[JobI_Log]
           ([Date]
           ,[Job]
           ,[Phase]
           ,[fDate]
           ,[Ref]
           ,[fDesc]
           ,[Amount]
           ,[TransID]
           ,[Type]
           ,[Labor]
           ,[Billed]
           ,[Invoice]
           ,[UseTax]
           ,[APTicket])
		   select 
		   getdate()
		   ,[Job]
           ,[Phase]
           ,[fDate]
           ,[Ref]
           ,[fDesc]
           ,[Amount]
           ,[TransID]
           ,[Type]
           ,[Labor]
           ,[Billed]
           ,[Invoice]
           ,[UseTax]
           ,[APTicket]
		   FROM deleted t1

END

GO

ALTER TABLE [dbo].[JobI] ENABLE TRIGGER [trgJobI]
GO

 