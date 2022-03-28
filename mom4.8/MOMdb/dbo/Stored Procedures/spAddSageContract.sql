CREATE PROCEDURE [dbo].[spAddSageContract]
@Remarks text,
@BStart datetime,
@Bcycle varchar(100),
@BAmt numeric(30,2),
@SStart datetime,
@Cycle varchar(100),
@Stime datetime,
@Route varchar(100),
@hours numeric(30,2),
@fdesc varchar(75),
@SagelocKey VARCHAR(100),
@SageJobKey VARCHAR(100),
@SageID  VARCHAR(100),
@servicetype varchar(15),
@LastUpdateDate datetime
as

declare @Job int = convert(int,@SageJobKey)

BEGIN TRANSACTION

if not exists(select 1 from Route where Name = @Route)
begin
insert into Route 
(
Name,
Mech
)
values
(
@Route,
(select ID from tblWork where fDesc =(select top 1 fuser from tblUser where DefaultWorker = 1))
)
end

if not exists(select 1 from LType where Type = @servicetype)
begin
insert into LType 
(
Type,
fDesc,
MatCharge,
Remarks,
Free
)
values
(
@servicetype,
@servicetype,
0,
@servicetype,
0
)
end


if(@SageJobKey = '0')
BEGIN
    if not exists(select 1 from Contract where SageID = @SageID)
      begin
	  if exists(select 1 from loc where loc = CONVERT(int, @SagelocKey))
	  begin
		INSERT INTO job
		(
		Loc,
		Owner,
		fDate,
		Status,
		Remarks,
		Rev,Mat,Labor,Cost,Profit,Ratio,Reg,OT,DT,TT,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,BillRate,NT,Amount,
		Custom20,
		Type,
		fDesc,
		CreditCard,
		CType
		)
		values
		(
		CONVERT(int, @SagelocKey),
		(select top 1 owner from loc where Loc = CONVERT(int, @SagelocKey)),
		getdate(),
		case when @BStart = CONVERT(datetime, '01/01/1900') then 1 else 0 end,
		@Remarks,
		0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,
		(select top 1 ID from Route where Name = @Route),
		--@Route ,
		0,
		@fdesc,
		0,
		@servicetype
		)
		set @Job=SCOPE_IDENTITY()
		
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		 BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		 END
		 
		insert into Contract
		(
		SWE,
		Job,
		BStart,
		BCycle,
		BAmt,
		SStart,
		SCycle,
		STime,
		Loc,
		Owner,
		Hours,
		Status,
		SageID,
		Chart	
		)
		values
		(
		0,
		@Job,
		@BStart,
		case @Bcycle 
		when	 'Annual' then 5
		when	 'Bimonthly' then 1
		when	 'Monthly' then 0
		when	 'NA' then 6
		when	 'Quarterly' then 2
		when	 'Semiannual' then 4
		else 6
		end 	
		,
		@BAmt,
		@SStart,
		case @Cycle 
		when	 'Annual' then 4
		when	 'Bimonthly' then 1
		when	 'Monthly' then 0
		when	 'NA' then -1
		when	 'Quarterly' then 2
		when	 'Semiannual' then 3
		else -1
		end 			
		,		
		@Stime,		
		CONVERT(int, @SagelocKey),
		(select top 1 owner from loc where Loc = CONVERT(int, @SagelocKey)),
		@hours,
		case when @BStart =CONVERT(datetime, '01/01/1900') then 1 else 0 end		,
		@SageID,
		(select top 1 (select top 1 sacct from inv where id = l.InvID) from LType l where type = @servicetype)
		)
		
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		 BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		 END
	  end
	end
		
END
 
ELSE

BEGIN

  DECLARE @lastup INT =0
  
		update Job set
		@lastup = 1,
		Loc=CONVERT(int, @SagelocKey),
		Owner=(select top 1 owner from loc where Loc = CONVERT(int, @SagelocKey)),
		Remarks=@Remarks,
		Custom20= (select top 1 ID from Route where Name = @Route),--@Route, 
		fDesc=@fdesc,
		CType=@servicetype
		where ID=@job
		AND ISNULL( LastUpdateDate, '01/01/1900' ) < @LastUpdateDate

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		 BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		 END
		 
	 IF( @lastup = 1 )
	 BEGIN
		update Contract set
		BStart=@BStart,
		BCycle=case @Bcycle 
		when	 'Annual' then 5
		when	 'Bimonthly' then 1
		when	 'Monthly' then 0
		when	 'NA' then 6
		when	 'Quarterly' then 2
		when	 'Semiannual' then 4
		else 6
		end 	,
		BAmt=@BAmt,
		SStart=@SStart,
		SCycle=case @Cycle 
		when	 'Annual' then 4
		when	 'Bimonthly' then 1
		when	 'Monthly' then 0
		when	 'NA' then -1
		when	 'Quarterly' then 2
		when	 'Semiannual' then 3
		else -1
		end 		,
		STime=@Stime,
		Loc=CONVERT(int, @SagelocKey),
		Owner=(select top 1 owner from loc where Loc = CONVERT(int, @SagelocKey)),
		Hours=@hours,
		SageID=@SageID,
		Chart=(select top 1 (select top 1 sacct from inv where id = l.InvID) from LType l where type = @servicetype)
		where Job=@Job
	END	
	
END

COMMIT TRANSACTION

 return @Job




 
