 CREATE  PROCEDURE [dbo].[spReCalCulateLaborexpense] 
  @nkTicketID int                                      
AS		
 
 
DECLARE  @TicketID  INT,@Ticketwage int,@Jobwage int,@Userwage int,@SchDt  DATETIME,@job int,
@Phase int,@Worker   VARCHAR(50),
@Reg          NUMERIC(30, 2),@OT           NUMERIC(30, 2),
@NT           NUMERIC(30, 2),@TT           NUMERIC(30, 2),
@DT           NUMERIC(30, 2)   ,
@MileStart    INT,
@MileEnd      INT,@MiscExp      NUMERIC(30, 2),@TollExp      NUMERIC(30, 2),@ZoneExp      NUMERIC(30, 2) 
  
 

 SELECT 
@TicketID  =D.ID,@Ticketwage = D.WageC ,@SchDt = d.EDate,
@job =d.Job,@Phase  =d.Phase,@Worker  =(SELECT fDesc FROM   tblWork WHERE  ID = d.fWork),
@Reg  =d.Reg,@OT  =d.OT,@NT =d.NT,@TT =d.TT,@DT  =d.DT,@MileStart =d.SMile,@MileEnd   =d.EMile,@MiscExp  =d.OtherE,@TollExp  =d.Toll,@ZoneExp  =d.Zone
FROM TicketD AS D
WHERE D.ID=(@nkTicketID)

if(isnull(@Phase,0)=0) 
  SELECT TOP 1     @Phase = j.Line
                FROM jobtitem j
                INNER JOIN bom b
                    ON b.JobtItemId = j.ID
                INNER JOIN BOMT
                    ON BOMT.ID = b.Type
                WHERE j.job = @job
                AND BOMT.Type = 'labor'
 
 PRINT('Ticket ID:-')

 PRINT(@TicketID)

 SELECT @Jobwage=WageC  from job where id=@job 
 
 if(isnull(@Ticketwage,0)=0)
 BEGIN
  DECLARE @USERWageCount int =0;

  SELECT @USERWageCount=count(Wage) 
  FROM PRWageItem pr 
  WHERE pr.Emp =  (SELECT id FROM emp e where e.fWork =
  ((SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker)))   


 IF ( @USERWageCount = 1)
  BEGIN 

    ( SELECT  @Ticketwage=(Wage) FROM PRWageItem pr 
     WHERE pr.Emp = (SELECT id FROM emp e where e.fWork = 	
	 (SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker)))
  END

  ELSE IF EXISTS ( SELECT 1 FROM PRWageItem pr 
  WHERE  pr.Wage = @Jobwage   and   pr.Emp = (SELECT id FROM emp e where e.fWork = 
  (SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker)))

  BEGIN 
    SELECT @Ticketwage=(Wage) FROM PRWageItem pr 
  WHERE  pr.Wage = @Jobwage   and   pr.Emp = (SELECT id FROM emp e where e.fWork = 
  (SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker))
  END  
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
		    IF ((SELECT TOP 1 Label FROM Custom WHERE Name='MultiTravel') = '1' )
				begin
					select @RegTrav=RegTrav, @OTTrav=OTTrav, @NTTrav=NTTrav, @DTTrav=DTTrav from(			 
						select RegTrav ,OTTrav, NTTrav, DTTrav from TicketDPDA where ID = @TicketID
						union
						select RegTrav ,OTTrav, NTTrav, DTTrav from TicketD where ID = @TicketID
					) as tabtrav
				end
			else 
				begin
					set @RegTrav=0;					set @OTTrav=0;					set @NTTrav=0;					set @DTTrav=0;
				end
				  
			SELECT @CReg = [CReg]			  ,@COT = [COT]
				  ,@CDT = [CDT]				  ,@CNT = [CNT]				  ,@CTT = [CTT]
			FROM PRWageItem pr 
			WHERE pr.Wage = @Ticketwage 
			and pr.Emp = (SELECT id FROM emp e where e.fWork = 
			(SELECT ID   FROM   tblWork  WHERE  fDesc = @Worker))
   
  
    DECLARE @hourlyrate numeric(30,2) 

    SELECT @hourlyrate = isnull( hourlyrate,0) from tblWork where fDesc = @Worker
                           
    DELETE from JobI where Ref = @TicketID and TransID = -@TicketID --and Job =@job 
		 
    ------$$$$$ JOb Cast feature $$$$--------
	            
    INSERT INTO JobI      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)
      values    (@job,
      @Phase,   @SchDt,    @TicketID,
      'Labor/Time Spent on Ticket - '+ @Worker,  
	  case (select isnull(JobCostLabor,0) from Control) 
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
      values     (@job,      @Phase,      @SchDt,      @TicketID,
      'Mileage on Ticket',       (isnull(@MileEnd,0)- isnull(@MileStart,0))* isnull((select MileageRate from Emp where CallSign = @Worker),0),
      -@TicketID,      1,      0,      0
      )
      
    INSERT INTO JobI      ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],Labor)
      values     (@job,      @Phase,      @SchDt,     @TicketID,     'Expenses on Ticket', 
      isnull(@TollExp,0) + isnull(@ZoneExp,0) + isnull(@MiscExp,0),
      -@TicketID,   1,  0,   0   )
	 
    EXEC spUpdateJobLaborExp @job, @Phase
	declare @JobCode varchar(100)
	declare @JobItemDesc varchar(255); 

	SELECT TOP 1   @JobCode =j.Code ,@JobItemDesc=j.fDesc
                FROM jobtitem j
                INNER JOIN bom b
                    ON b.JobtItemId = j.ID
                INNER JOIN BOMT
                    ON BOMT.ID = b.Type
                WHERE j.job = @job   and j.Line=@Phase

	UPDATE   TicketD  set WageC=@Ticketwage , Phase=@Phase , JobCode=@JobCode , JobItemDesc= @JobItemDesc  WHERE  ID=@TicketID and  isnull(WageC,0)=0

	END ----------------------------------------2>  
	
	

	 
 

 
  




 

