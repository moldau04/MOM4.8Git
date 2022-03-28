 
CREATE PROCEDURE [dbo].[spAddTicketINVInfo]
	@TicketID INT ,
	@job INT,
	@dtTicketINV as  tblTypeTicketINV  readonly,
	@screen varchar(50),
	@mode varchar(50)
AS
 

  ---  ////////// INVENTORTY  $$$$$$$$$$$$$

if EXISTS( select 1 from @dtTicketINV)


BEGIN
begin tran
 
  DECLARE @INV_MAXBatch int

  SELECT @INV_MAXBatch = ISNULL(MAX(Batch),0)+1 FROM Trans

    DECLARE @Tinv table 
    (
	[PK] int identity(1,1) not null,   [Ticket] [int] NULL,	[Line] [smallint] NULL,	[Item] [int] NULL,	[Quan] [numeric](30, 2) NULL,
	[fDesc] [varchar](255) NULL,	[Charge] [smallint] NULL,	[Amount] [numeric](30, 2) NULL,	[Phase] [smallint] NULL, 	[AID] [varchar](50) NULL ,[TypeID] [int] NULL,[WarehouseID] [varchar](50) NULL,	[LocationID] [int] NULL  ,	[PhaseName] [varchar](50) NULL
	)
	INSERT INTO @Tinv     ([Ticket]  ,	[Line]  ,	[Item] ,	[Quan]  ,	[fDesc]  ,	[Charge]  ,	[Amount]  ,	[Phase]  , [AID],TypeID	 ,	[WarehouseID]  ,	[LocationID]   ,	[PhaseName] )
    SELECT @TicketID  ,	[Line]  ,	[Item] ,	[Quan]  ,	[fDesc]  ,	[Charge]  ,	[Amount]  ,	[Phase]  , [AID],TypeID	 ,	[WarehouseID]  ,	[LocationID]   ,	'Materials'	 FROM @dtTicketINV

DECLARE @INV_ROW_Count INT ;DECLARE @INV_ROW_NO INT =1;

SELECT  @INV_ROW_Count=max(PK) from @Tinv  

WHILE(@INV_ROW_NO <=@INV_ROW_Count)

BEGIN 

IF NOT EXISTS(SELECT 1 from TicketI  ti INNER JOIN @Tinv t on   ti.Ticket=@TicketID and ti.Item=t.Item  and t.pk=@INV_ROW_NO)

 BEGIN

 Declare @INV_AMT numeric(30,2) =0; Declare @INV_Item [int] =0; Declare @INV_Quan  numeric(30,2) =0; Declare @INV_WarehouseID  [varchar](50)= NULL;

 Declare @INV_LocationID  [int]= NULL; Declare @INV_fDesc  [varchar](100)= NULL; 

 Declare @INV_Line [int] =0; Declare @INV_EN [int] =0; Declare @INV_fDate Datetime=getdate();  Declare @INV_Quan1 numeric(30,2) =0; Declare @invTypeID int=1;

 

 select @invTypeID =id from bomt where Type='Materials'


 
     -------- Pickup Lcost FROM INV 

 SELECT  @INV_AMT=(t.Quan * isnull(LCost,0)),@INV_Item=t.Item ,@INV_Quan=t.Quan,@INV_WarehouseID=t.WarehouseID,@INV_LocationID=t.LocationID,@INV_fDesc=t.fDesc,
 @INV_Line=t.Line,@INV_EN =isnull(inv.EN,0)
  from inv   INNER JOIN @Tinv t on t.Item=Inv.ID  where t.pk=@INV_ROW_NO 
  DECLARE @Phase smallint

  	               --CHECK IF ITEM ALREADY EXIST IN BOM
					IF EXISTS (SELECT TOP 1 line from jobtitem where job=@job and fDesc=@INV_fDesc and type=1 )
					BEGIN						 
				    SET @Phase=(SELECT TOP 1 line from jobtitem where job=@job and fDesc=@INV_fDesc and type=1)				 
					END
					ELSE
					BEGIN
				    -- INSERT BOM JOB ITEM
				    EXEC @Phase =  [dbo].[spAddBOMItem]      @job =@job,    @type = @invTypeID,   @item =@INV_Item,  @fDesc=@INV_fDesc,      @Phase  =null ,     @OpSq = 100	 
					END




 INSERT INTO TicketI ([Ticket]  ,	[Line]  ,	[Item] ,	[Quan]  ,	[fDesc]  ,	[Charge]  ,	[Amount]  ,	[Phase]  ,  [WarehouseID]  ,	[LocationID]   ,	[PhaseName],   [TypeID] ) 
  SELECT  [Ticket]  ,	[Line]  ,	[Item] ,	[Quan]  ,	[fDesc]  ,	[Charge]  ,@INV_AMT  ,          	@Phase  , 	[WarehouseID]  ,	[LocationID]   ,      'Materials' ,  @invTypeID 
  from @Tinv where pk=@INV_ROW_NO 
  

DECLARE @INVValidation varchar(100)='You do not have enough on hand for item';

    -------if Warehouse and Location  Selected
  IF(   (isnull(@INV_WarehouseID,'') <>'') and (isnull(@INV_LocationID,0) <> 0) )
  BEGIN
       
   
    ------Hand
   IF NOT EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@INV_Item   and  i.WarehouseID = @INV_WarehouseID   and  i.LocationID =@INV_LocationID 
   )
    BEGIN     SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@INV_Item
   RAISERROR (@INVValidation,16,1)      RETURN   END

   ------Hand
   IF EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@INV_Item   and  i.WarehouseID = @INV_WarehouseID   and  i.LocationID =@INV_LocationID     AND   (i.Hand < @INV_Quan)  
   )
    BEGIN  
	SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@INV_Item
   RAISERROR (@INVValidation,16,1)      RETURN   END

 
 

     
  END 
   -------if Warehouse Select and Location Not Selected
  ELSE 
  BEGIN
  
       ------Hand
   IF NOT EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@INV_Item   and  i.WarehouseID = @INV_WarehouseID    and  isnull(i.LocationID,0) =0  
   )
    BEGIN     SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@INV_Item
   RAISERROR (@INVValidation,16,1)       RETURN   END

   ------Hand
   IF EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@INV_Item   and  i.WarehouseID = @INV_WarehouseID    and  isnull(i.LocationID,0) =0       AND   (i.Hand < @INV_Quan)  
   )
    BEGIN   SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@INV_Item
   RAISERROR (@INVValidation,16,1)      RETURN   END   


    --UPDATE i SET i.Hand=i.Hand - @INV_Quan   , i.Balance=i.Balance-@INV_AMT
    --FROM IWarehouseLocAdj i  where 
    --i.InvID  =@INV_Item  and   i.WarehouseID  =@INV_WarehouseID
    --and isnull(i.LocationID,0)=0 
  END

  ------- Make Translation in Trans Table
  Declare @CheckInv_Tracking  [varchar](100)= NULL; 
  select distinct @CheckInv_Tracking = Label from custom where Name='InvGL'
  if @CheckInv_Tracking ='True' -- As per ES-5086 Asheville - inventory tracking off (Anita Mam suggested)
  BEGIN

   Declare @INV_fDesc1  [varchar](100)= NULL; 
   SET @INV_fDesc1    ='Cost of Goods Sold - Job ' + cast(@job as varchar (50) );
   Declare @INV_AMT1 numeric(30,2) =0;
   SET @INV_AMT1 = (@INV_AMT * -1);
   SET @INV_Quan1=( @INV_Quan * -1 );
   Declare @INV_Acct71 int=0;
   SELECT @INV_Acct71=GL  from job where  ID=@job   --(MAterial Expence Acct)

   --------- Type 71  Post for job cost  (Amount and Quantity is +ve)
  EXEC      [dbo].[AddTrans]  
            @ID        = NULL  
           ,@Batch     = @INV_MAXBatch
           ,@fDate     =@INV_fDate
           ,@Type     = 71
           ,@Line     = @INV_Line
           ,@Ref       = @TicketID
           ,@fDesc    =@INV_fdesc1
           ,@Amount   = @INV_AMT
           ,@Acct      = @INV_Acct71
           ,@AcctSub   = @INV_Item
           ,@Status    = @INV_Quan
           ,@Sel       = NULL
           ,@VInt      = @job
           ,@VDoub     =0
           ,@EN        =@INV_EN
           ,@strRef    = NULL
   END
    INSERT INTO JobI    
      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)  
        
	  select  @job , @Phase , @INV_fDate , @TicketID , @INV_fDesc , @INV_AMT , -@TicketID, 1,0,0
   --------- Type 70 Pull out From Inventory (Amount and Quantity is -ve)
   if @CheckInv_Tracking ='True'  -- As per ES-5086 Asheville - inventory tracking off (Anita Mam suggested)
  BEGIN
    Declare @INV_Acct70 int=0;
	SELECT Top 1 @INV_Acct70 =Label  from custom  where name ='DefaultInvGLAcct'
    Declare @INV_TransID int =null;

  EXEC      [dbo].[AddTrans]  
            @ID        = @INV_TransID out  
           ,@Batch     = @INV_MAXBatch
           ,@fDate     =@INV_fDate
           ,@Type     = 70
           ,@Line     = @INV_Line
           ,@Ref       = @TicketID
           ,@fDesc    =@INV_fdesc1
           ,@Amount   =@INV_AMT1
           ,@Acct      = @INV_Acct70
           ,@AcctSub   = @INV_Item
           ,@Status    = @INV_Quan1
           ,@Sel       = NULL
           ,@VInt      = @job
           ,@VDoub     =0
           ,@EN        =@INV_EN
           ,@strRef    = NULL 
 END
 -----------Inventory  Adjustment-------------->


               --------------------$$$$  Commmited $$$$$$
     if (@INV_Quan <> 0)

	 BEGIN
     Declare @Committed numeric(32,2)=0;

	 select  @Committed = SUM(isnull(Committed,0)) from tblInventoryWHTrans where Screen='Project' and InvID=@INV_Item and ScreenID=@job and WarehouseID=@INV_WarehouseID
	  
	 if( @INV_Quan <= @Committed) set @Committed=@INV_Quan1;
	 
	 else if( @Committed <= 0) set @Committed=0;	

	 else   set @Committed= @Committed * -1  ;

	 SET @screen = 'PostToProject/InventoryUsed';

	 if(@Committed <> 0  )
	    
		BEGIN

	 	    INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
	     
		     VALUES (@INV_Item,@INV_WarehouseID ,0,0,0,0,@Committed,0,'Project',@job,'Edit',GETDATE(),'out',@INV_MAXBatch,GETDATE())

		 END

		 END

		      --------------------$$$$  Commmited $$$$$$

         INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
	
	     VALUES (@INV_Item,@INV_WarehouseID,@INV_LocationID,@INV_Quan1,@INV_AMT1,0,0,0,@screen,@TicketID,@mode,GETDATE(),'Out',@INV_MAXBatch,GETDATE())
 END

SET @INV_ROW_NO+=1; 

END
 
	 
		  -----------------------Delete MS Inventory Items------------------------>
		 
	    DELETE FROM TicketIPDA   WHERE  Ticket = @TicketID	  
		
		COMMIT TRAN

		EXEC CalculateInventory
END
         

-----/////// END INVENTORTY $$$$$$$$$$$$$$






------------select * from tblInventoryWHTrans where   InvID=InvID and ScreenID=209 and InvID=4591 order by ID desc 
------------Declare @Committed numeric(32,2)=0;

------------Declare @Q numeric(32,2)=11;

------------select  @Committed = isnull(SUM(isnull(Committed,0)),0) from tblInventoryWHTrans where Screen='Project' and ScreenID=209 and InvID=4591 and WarehouseID='ofc'
	 
------------select @Committed as CurrentCommited

------------if( @Q <= @Committed) set @Committed=-@Q;
	 
------------else if( @Committed <= 0) set @Committed=0;		 

------------else   set @Committed= @Committed * -1  ;

------------select @Committed

	 

 
