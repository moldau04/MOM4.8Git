 
CREATE PROCEDURE [dbo].[spReCalCulateLaborexpensebyDate]
	@FromDate date ,	@ToDate date
AS

 
  ----------- $$$ Re-CalCulate Labor expense   $$$ --------------
	   --BEGIN TRAN
----   COMMIT TRAN
----   ROLLBACK TRAN 
DECLARE @Startfdate Date= @FromDate ; 
DECLARE @Endfdate   Date =   @ToDate ; 
CREATE TABLE #temptable ( ROWNO int IDENTITY (1, 1) NOT NULL ,  TicketID int NOT NULL ) 
  INSERT INTO #temptable (TicketID)
  SELECT  t.ID FROM ticketd t WITH (NOLOCK)
 -- INNER join jobI as ji on cast(ji.ref as varchar) = cast(t.id as varchar) 
  WHERE 1=1 
  --ji.type = 1 
  --and ji.Labor = 1
 -- and isNull(ji.Amount,0) = 0.00 
  and ( cast( t.edate as Date ) >= @Startfdate)
  and (cast (t.edate as Date )  <= @Endfdate)
 -- and isnull(t.total,0) > 0.00 
  -- and t.WageC is null --- if We wants to run only for that tickets their wage category is null
  --and ClearCheck = 1  
  --ORDER BY ji.fDate desc
  --and t.fWork=39
  DECLARE @ROW_Count int ;
  DECLARE @ROW_NO int =1;
  SELECT  @ROW_Count=max(ROWNO) from #temptable where TicketID is not null 
WHILE(@ROW_NO <=@ROW_Count)
BEGIN---1
PRINT(@ROW_NO)
DECLARE  @TicketID  INT,
@Ticketwage int,
@Jobwage int,
@Userwage int,
@SchDt  DATETIME,
@job int,
@Phase int,
@Worker   VARCHAR(50),
@Reg          NUMERIC(30, 2),
@OT           NUMERIC(30, 2),
@NT           NUMERIC(30, 2),
@TT           NUMERIC(30, 2),
@DT           NUMERIC(30, 2)   ,
@MileStart    INT,
@MileEnd      INT,
@MiscExp      NUMERIC(30, 2),
@TollExp      NUMERIC(30, 2),
@ZoneExp      NUMERIC(30, 2) 
  
 

 SELECT 
@TicketID  =D.ID,
@Ticketwage = D.WageC ,
@SchDt = d.EDate,
@job =d.Job,
@Phase  =d.Phase,
@Worker  =(SELECT fDesc FROM   tblWork WITH (NOLOCK) WHERE  ID = d.fWork),
@Reg  =d.Reg,
@OT  =d.OT,
@NT =d.NT,
@TT =d.TT,
@DT  =d.DT,
@MileStart =d.SMile,
@MileEnd   =d.EMile,
@MiscExp  =d.OtherE,
@TollExp  =d.Toll,
@ZoneExp  =d.Zone
FROM TicketD AS D WITH (NOLOCK)
WHERE D.ID=( SELECT TicketID FROM #temptable where ROWNO=@ROW_NO)
 
 PRINT('Ticket ID:-')

 PRINT(@TicketID)

 SELECT @Jobwage=WageC  from job WITH (NOLOCK) where id=@job 
 
 if(@Ticketwage IS null)
 BEGIN
  DECLARE @USERWageCount int =0;

  SELECT @USERWageCount=count(Wage) 
  FROM PRWageItem pr  WITH (NOLOCK)
  WHERE pr.Emp =  (SELECT id FROM emp e WITH (NOLOCK) where e.fWork =
  ((SELECT ID   FROM   tblWork  WITH (NOLOCK) WHERE  fDesc = @Worker)))   


 IF ( @USERWageCount = 1)
  BEGIN 

    ( SELECT  @Ticketwage=(Wage) FROM PRWageItem pr WITH (NOLOCK) 
     WHERE pr.Emp = (SELECT id FROM emp e WITH (NOLOCK) where e.fWork = 	
	 (SELECT ID   FROM   tblWork WITH (NOLOCK)  WHERE  fDesc = @Worker)))
  END

  ELSE IF EXISTS ( SELECT 1 FROM PRWageItem pr WITH (NOLOCK) 
  WHERE  pr.Wage = @Jobwage   and   pr.Emp = (SELECT id FROM emp e WITH (NOLOCK) where e.fWork = 
  (SELECT ID   FROM   tblWork WITH (NOLOCK)  WHERE  fDesc = @Worker)))

  BEGIN 
    SELECT @Ticketwage=(Wage) FROM PRWageItem pr  WITH (NOLOCK)
  WHERE  pr.Wage = @Jobwage   and   pr.Emp = (SELECT id FROM emp e WITH (NOLOCK) where e.fWork = 
  (SELECT ID   FROM   tblWork WITH (NOLOCK)  WHERE  fDesc = @Worker))
  END


  --ELSE 
  --BEGIN 
  --  ( SELECT top 1  @Ticketwage=(Wage) FROM PRWageItem pr 
  --   WHERE pr.Emp = (SELECT id FROM emp e where e.fWork = 	
	 --(SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker)))
  --END
END
print('job')
print(@job)
print('Ticketwage')
print(@Ticketwage)
 IF(@job > 0 and @Ticketwage IS not null)

	BEGIN  -----------------------------------------2>

  		   DECLARE  @RegTrav NUMERIC(30, 2), 
		   @OTTrav NUMERIC(30, 2), 
		   @NTTrav NUMERIC(30, 2), 
		   @DTTrav NUMERIC(30, 2)   ,
		   @CReg   NUMERIC(30, 2)   ,
		   @COT  NUMERIC(30, 2)    ,
		   @CDT  NUMERIC(30, 2)   ,
		   @CNT   NUMERIC(30, 2)    ,
		   @CTT  NUMERIC(30, 2)

		  -- if Multi Travel Feature is ON  
		    IF ((SELECT TOP 1 Label FROM Custom WITH (NOLOCK) WHERE Name='MultiTravel') = '1' )
				begin
					select @RegTrav=RegTrav, @OTTrav=OTTrav, @NTTrav=NTTrav, @DTTrav=DTTrav from(			 
						select RegTrav ,OTTrav, NTTrav, DTTrav from TicketDPDA WITH (NOLOCK) where ID = @TicketID
						union
						select RegTrav ,OTTrav, NTTrav, DTTrav from TicketD WITH (NOLOCK) where ID = @TicketID
					) as tabtrav
				end
			else 
				begin
					set @RegTrav=0;
					set @OTTrav=0;
					set @NTTrav=0;
					set @DTTrav=0;
				end
				  
			SELECT @CReg = [CReg]
				  ,@COT = [COT]
				  ,@CDT = [CDT]
				  ,@CNT = [CNT]
				  ,@CTT = [CTT]
			FROM PRWageItem pr  WITH (NOLOCK)
			WHERE pr.Wage = @Ticketwage 
			and pr.Emp = (SELECT id FROM emp e WITH (NOLOCK) where e.fWork = 
			(SELECT ID   FROM   tblWork WITH (NOLOCK)  WHERE  fDesc = @Worker))
   
  
    DECLARE @hourlyrate numeric(30,2) 

    SELECT @hourlyrate = isnull( hourlyrate,0) from tblWork WITH (NOLOCK) where fDesc = @Worker
                           
    DELETE from JobI where Ref = @TicketID and TransID = -@TicketID --and Job =@job 
		 
    ------$$$$$ JOb Cast feature $$$$--------
	            
    INSERT INTO JobI
      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)
      values    (@job,
      @Phase,   @SchDt,    @TicketID,
      'Labor/Time Spent on Ticket - '+ @Worker,  
	  case (select isnull(JobCostLabor,0) from Control WITH (NOLOCK)) 
	  when 0 then
	  (
      (isnull(@Reg,0)+isnull(@RegTrav,0)) * isnull(@hourlyrate,0)
      +
      (isnull(@OT,0)+isnull(@OTTrav,0)) * (1.5* isnull(@hourlyrate,0))
      +
      (isnull(@NT,0)+isnull(@NTTrav,0)) * (1.7* isnull(@hourlyrate,0))
      +
      (isnull(@DT,0)+isnull(@DTTrav,0)) * (2* isnull(@hourlyrate,0))
      +
      isnull(@TT,0)* isnull(@hourlyrate,0)
	  )
	  when 1 then
	  (
	  (isnull(@Reg,0)+isnull(@RegTrav,0)) * isnull(@CReg,0)
      +
      (isnull(@OT,0)+isnull(@OTTrav,0)) * (isnull(@COT,0))
      +
      (isnull(@NT,0)+isnull(@NTTrav,0)) * (isnull(@CNT,0))
      +
      (isnull(@DT,0)+isnull(@DTTrav,0)) * (isnull(@CDT,0))
      +
      isnull(@TT,0)* isnull(@CTT,0)
	  )
	  end
      ,
      -@TicketID,   1,   0,   1    )
      
    INSERT INTO JobI
      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)
      values     (@job,
      @Phase,
      @SchDt,      @TicketID,
      'Mileage on Ticket', 
      (isnull(@MileEnd,0)- isnull(@MileStart,0))* isnull((select MileageRate from Emp WITH (NOLOCK) where CallSign = @Worker),0),
      -@TicketID,      1,      0,      0
      )
      
    INSERT INTO JobI
      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)
      values     (@job,
      @Phase,
      @SchDt,     @TicketID,     'Expenses on Ticket', 
      isnull(@TollExp,0) + isnull(@ZoneExp,0) + isnull(@MiscExp,0),
      -@TicketID,   1,  0,   0   )
	 
    EXEC spUpdateJobLaborExp @job, @Phase

	UPDATE   TicketD  set WageC=@Ticketwage
    WHERE  ID=@TicketID and  WageC is Null

	END ----------------------------------------2>  
	
	ELSE 

	BEGIN --IF( @Ticketwage IS NUll) 
	UPDATE  TicketD   set ClearCheck=0
    WHERE  ID=@TicketID 
    END

SET @ROW_NO+=1; 

END	----1 
DROP TABLE #temptable 




 


