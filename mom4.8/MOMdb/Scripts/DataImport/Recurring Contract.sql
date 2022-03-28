-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert Recurring Contract
-- =============================================


--- Import Contract 
 

 BEGIN TRAN

-----------COMMIT TRAN

-----------ROLLBACK TRAN

DECLARE @loc   INT,
        @owner            INT,
        @date             DATETIME,
        @Status           INT,
        @Creditcard       INT,
        @Remarks          nvarchar(max),
        @BStart           DATETIME,
        @Bcycle           INT,
        @BAmt             NUMERIC(30, 2),
        @SStart           DATETIME,
        @Cycle            INT,
        @SWE              INT,
        @Stime            DATETIME,
        @Sday             INT,
        @SDate            INT,
        @Route            VARCHAR(75),
        @hours            NUMERIC(30, 2),
        @fdesc            VARCHAR(75),
        @CType            VARCHAR(15),
        @ExpirationDate   DATETIME,
        @ExpirationFreq   SMALLINT,
        @Expiration       SMALLINT,
        @ContractBill     SMALLINT,
        @CustomerBill     SMALLINT,
        @Central          INT,
        @Chart            INT,
        @JobT             INT,
        @EscalationType   SMALLINT,
        @EscalationCycle  SMALLINT,
        @EscalationFactor NUMERIC(30, 2),
        @EscalationLast   DATETIME,
        @BillRate         NUMERIC(30, 2),
        @RateOT           NUMERIC(30, 2),
        @RateNT           NUMERIC(30, 2),
        @RateDT           NUMERIC(30, 2),
        @RateTravel       NUMERIC(30, 2),
        @Mileage          NUMERIC(30, 2),
        @PO               VARCHAR(25),
        @SPHandle         SMALLINT=0,
        @SPRemarks        nvarchar(max)='',
        @IsRenewalNotes   SMALLINT=0,
        @RenewalNotes     nvarchar(max)='',
        @Detail           SMALLINT,
        @taskcategory     VARCHAR(15) = ''; 	
			 
 Declare @ElevJobData   AS  [dbo].[TBLTYPEJOINELEVJOB] ;
DECLARE @Row INT =1;

DECLARE @RowCount INT =0;

SELECT @RowCount = Max(PK) FROM  dbo.WestCoastRecurring$

WHILE( @Row <= @RowCount )
  BEGIN 

      
	 IF  EXISTS(SELECT 1  FROM   dbo.WestCoastRecurring$ where PK=@Row  and  MOM_ContractID is   null and MOM_LocID is not null)
        
		BEGIN

            PRINT ( 'Not Exists Contract' )
		    PRINT ('Row No.-') PRINT (@Row) 
			 Declare @STDate datetime,@notes nvarchar(50);

             select @Loc=c.MOM_LocID    from dbo.WestCoastRecurring$ c   where c.PK=@Row 
		 
		    select @owner=loc, @Route=Route from loc where loc=@Loc
 

		  IF((isnull(@Loc,'') !='') and ( isnull(@owner,'') !=''))
	      BEGIN---------

		   
		   SELECT  
               
                @Date=null,
                @Status= 0,
                @Creditcard=0,
                @Remarks=t.[Customer Name] + t.[Location Name],
                @BStart=t.[Billing Start Date],					 
			    @Bcycle=  case t.[Ticketing Frequency] when 'Quarterly' then 2 else 6 end,
                @BAmt=t.[Billing Amount],
                @SStart=t.[Ticketing Start Date],
                @Cycle=0,
                @SWE=0,
                @Stime='09:00 AM',
                @Sday=0,
                @SDate=0,
                @Route=@Route,
                @hours=1,
                @fdesc=t.[Contract Description],
                @CType='Maintenance',
                @ExpirationDate=t.Expiration,
                @ExpirationFreq=null,
                @Expiration=null,
                @ContractBill=0,
                @CustomerBill=0,
                @Central=0,
                @Chart=119,
                @JobT=0,
                @EscalationType=3,
                @EscalationCycle=12,
                @EscalationFactor=t.[Escalation Factor],
                @EscalationLast=t.[Escalated Last Date],
                @BillRate=0,
                @RateOT=0,
                @RateNT=0,
                @RateDT=0,
                @RateTravel=0,
                @Mileage=0,
                @PO=t.PO#,
                @SPHandle=null,
                @SPRemarks=t.[Customer Name] + t.[Location Name],
                @IsRenewalNotes=null,
                @RenewalNotes=t.[Customer Name] + t.[Location Name],
                @Detail=0,
                @taskcategory=null
			FROM   dbo.WestCoastRecurring$ t
			where t.pk= @Row 


			insert into @ElevJobData 

			select e.MOM_EquipID,0,0 from  [WestCoastEquipment$] e   where e.MOM_LocID=@loc

           PRINT ('LOC -')PRINT ( @Loc)

	       PRINT ('Owner -')PRINT (@owner)

	       PRINT( 'Begin EXECUTE [[spAddContract]]' )
		  
		   EXECUTE [dbo].[spAddContract]
		        @Loc=@loc,
                @owner=@owner,
                @Date=@date,
                @Status=@Status,
                @Creditcard=@Creditcard,
                @Remarks=@Remarks,
                @BStart=@BStart,
                @Bcycle=@Bcycle,
                @BAmt=@BAmt,
                @SStart=@SStart,
                @Cycle=@Cycle,
                @SWE=@SWE,
                @Stime=@Stime,
                @Sday=@Sday,
                @SDate=@SDate,				
				@ElevJobData=@ElevJobData,
                @Route=@Route,
                @hours=@hours,
                @fdesc=@fdesc,
                @CType=@CType,
                @ExpirationDate=@ExpirationDate,
                @ExpirationFreq=@ExpirationFreq,
                @Expiration=@Expiration,
                @ContractBill=@ContractBill,
                @CustomerBill=@CustomerBill,
                @Central=@Central,
                @Chart=@Chart,
                @JobT=@JobT,
                @EscalationType=@EscalationType,
                @EscalationCycle=@EscalationCycle,
                @EscalationFactor=@EscalationFactor,
                @EscalationLast=@EscalationLast,
                @BillRate=@BillRate,
                @RateOT=@RateOT,
                @RateNT=@RateNT,
                @RateDT=@RateDT,
                @RateTravel=@RateTravel,
                @Mileage=@Mileage,
                @PO=@PO,
                @SPHandle=@SPHandle,
                @SPRemarks=@SPRemarks,
                @IsRenewalNotes=@IsRenewalNotes,
                @RenewalNotes=@RenewalNotes,
                @Detail=@Detail,
                @taskcategory=@taskcategory,
				@UpdatedBy=null
				 
		   PRINT( 'End EXECUTE [[spAddContract]]' )

           END
		   
	      update e set  e.MOM_ContractID= (SELECT max(job)  FROM   Contract c where owner=@owner and loc=@loc) FROM  dbo.WestCoastRecurring$ e  where pk= @Row 
		  
		  
		  delete from @ElevJobData
		
		END


     SET @Row=@Row + 1 
  END