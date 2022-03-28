   BEGIN TRAN 

-- ROLLBACK TRAN

-- COMMIT TRAN

 ------------------------>              <------------------------->
 ------------------------> Safety Test  <------------------------->
 ------------------------>              <------------------------->
 
DECLARE @ROW_NO     int = 0;

DECLARE @ROW_Max  int = 1687;
 
DECLARE     @typeID int=2
           ,@Loc    int
           ,@Elev   int
           ,@Last   datetime =null
           ,@Next   datetime =null
           ,@Status int =0
           ,@Ticket int =null
           ,@Remarks varchar(50) =null
           ,@LastDue datetime=null
		   ,@JobId   int=NULL
           ,@Custom1 varchar(50) =NULL
		   ,@Custom2 varchar(50) =NULL
		   ,@Custom3 varchar(50) =NULL
		   ,@Custom4 varchar(50) =NULL		  
           ,@Amount  numeric(32,2) =0
		   ,@OverrideAmount  numeric(32,2) = null
		   ,@ThirdPartyName  varchar(50) = null
		   ,@ThirdPartyPhone varchar(50)  = null  
		   ,@TestDueBy  INT =1
		   ,@Charge     INT = null
		   ,@ThirdParty INT = null


WHILE(@ROW_NO < = 5)

BEGIN  ---- 1
	 
        
BEGIN  ---- 2

IF NOT EXISTS (SELECT 1  FROM   dbo.ColleySafetyTestDataImport rc 
inner join LoadTestItem lti on rc.MOM_Loc_ID = lti.Loc  
and lti.Elev =rc.MOM_Equip_ID and lti.ID=2
where  rc.PK=@ROW_NO
)

SELECT @Loc=isnull(rc.MOM_LOC_ID,0) ,  @Elev=isnull(rc.MOM_Equip_ID,0)  ,
       @JobId=isnull(rc.Project,0),
	   @Last=rc.[Last Tested On],
	   @Next=rc.[Next Due Date] ,
	   @LastDue = rc.[Last Due Date]
FROM   dbo.ColleySafetyTestDataImport rc  
where  rc.PK=@ROW_NO

   if ( @Loc <> 0 and @Elev <> 0 and @JobId <> 0 ) 

           
BEGIN  ---- 3

            PRINT ( 'Not Exists Contract' )
    	  
            Execute [dbo].[spCreateTest]
			 		
			@typeID  = @typeID
           ,@Loc     = @Loc
           ,@Elev    = @Elev
           ,@Last    = @Last
           ,@Next    = @Next
           ,@Status  = @Status
           ,@Ticket  = @Ticket
           ,@Remarks = @Remarks
           ,@LastDue = @LastDue
		   ,@JobId   = @JobId
           ,@Custom1 = @Custom1
		   ,@Custom2 = @Custom2
		   ,@Custom3 = @Custom3
		   ,@Custom4 = @Custom4 
           ,@Amount  = @Amount
		   ,@OverrideAmount  = @OverrideAmount
		   ,@ThirdPartyName  = @ThirdPartyName
		   ,@ThirdPartyPhone = @ThirdPartyPhone
		   ,@TestDueBy       = @TestDueBy 
		   ,@Charge          = @Charge
		   ,@ThirdParty      = @ThirdParty


		   UPDATE LoadTest SET Count=Count+1 WHERE ID=2

		   	   DECLARE  @testid int=0;
		     update rc set rc.MOM_Safety_Test_ID =lti.LID  
		   FROM   dbo.ColleySafetyTestDataImport rc 
inner join LoadTestItem lti on rc.MOM_Loc_ID = lti.Loc  
and lti.Elev =rc.MOM_Equip_ID and lti.ID=2
where  rc.PK=@ROW_NO 

select @testid = rc.MOM_Safety_Test_ID
FROM   dbo.ColleySafetyTestDataImport rc  
where  rc.PK=@ROW_NO
	
		 Execute  [dbo].[spCreateTestHistory]			
         
			@testid =@testid,
			@Username ='Data Import',
			@TestStatus ='Open',
			@LastDate =@Last,
			@TestStatusid =0,
			@TestTicketID =null ,
			@TestTicketStatus  ='UnAssigned'

		 
		   
 
 END ----- 3

 END  ---- 2

 PRINT(@ROW_NO)

 SET @ROW_NO +=1; 

 END  ---- 1





 
		   