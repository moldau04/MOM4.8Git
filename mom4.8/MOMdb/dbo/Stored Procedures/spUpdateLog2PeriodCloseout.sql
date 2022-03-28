CREATE PROCEDURE [dbo].[spUpdateLog2PeriodCloseout]
	@CloseOutDate DATETIME,
	@YearEndClose int,
	@RetainedGLAcct varchar(150),
	@CurrentGLAcct varchar(150),
	@StartDate DATETIME,
	@EndDate DATETIME, 
	@UserID int,
	@UpdatedBy varchar(100)
As
	Declare @CurrentCloseOutDate varchar(50)
	Select @CurrentCloseOutDate = CONVERT(varchar(50), CODt, 101) from tblUser Where ID = @UserID

	Declare @CurrentStartDate varchar(50)
	Select @CurrentStartDate = CONVERT(varchar(50), fStart, 101) from tblUser Where ID = @UserID

	Declare @CurrentEndDate varchar(50)
	Select @CurrentEndDate = CONVERT(varchar(50), fEnd, 101) from tblUser Where ID = @UserID

 /********Start Logs************/
 Declare @Val varchar(1000)
  if(@CloseOutDate is not null And @CloseOutDate != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='Close Out Date' order by CreatedStamp desc )		
	 Declare @CloseOutDt nvarchar(150)
	 SELECT @CloseOutDt = convert(varchar, @CloseOutDate, 101)
	if(@Val<>@CloseOutDt)
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Close Out Date',@Val,@CloseOutDt
	end
	Else IF (@CurrentCloseOutDate <>  @CloseOutDt)
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Close Out Date',@CurrentCloseOutDate,@CloseOutDt
	END
	end
 set @Val=null
 if(@StartDate is not null And @StartDate != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='Start Date' order by CreatedStamp desc )		
	 Declare @StartDt nvarchar(150)
	 SELECT @StartDt = convert(varchar, @StartDate, 101)
	if(@Val<>@StartDt)
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Start Date',@Val,@StartDt
	end
	Else IF (@CurrentStartDate <>  @StartDt)
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Start Date',@CurrentStartDate,@StartDt
	END
	end
 set @Val=null
  if(@EndDate is not null And @EndDate != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='End Date' order by CreatedStamp desc )		
	 Declare @EndDt nvarchar(150)
	 SELECT @EndDt = convert(varchar, @EndDate, 101)
	if(@Val<>@EndDt)
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'End Date',@Val,@EndDt
	end
	Else IF (@CurrentEndDate <>  @EndDt)
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'End Date',@CurrentEndDate,@EndDt
	END
	end
 set @Val=null
   
 if(@YearEndClose is not null And @YearEndClose != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='Year End Close' order by CreatedStamp desc )	
	if(@Val<> CONVERT(varchar(10), @YearEndClose))
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Year End Close',@Val,@YearEndClose
	end
	Else IF (@Val is null And  @YearEndClose != 0)
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Year End Close','',@YearEndClose
	END
	end
 set @Val=null
 if(@RetainedGLAcct is not null And @RetainedGLAcct != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='Retained Earnings GL Account' order by CreatedStamp desc )	
	if(@Val<>@RetainedGLAcct)
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Retained Earnings GL Account',@Val,@RetainedGLAcct
	end
	Else IF (@Val is null And  @RetainedGLAcct != '')
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Retained Earnings GL Account','',@RetainedGLAcct
	END
	end
 set @Val=null
 if(@CurrentGLAcct is not null And @CurrentGLAcct != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PeriodCloseout' and Field='Current Earnings GL Account' order by CreatedStamp desc )	
	if(@Val<>@CurrentGLAcct)
	begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Current Earnings GL Account',@Val,@CurrentGLAcct
	end
	Else IF (@Val is null And  @CurrentGLAcct != '')
	Begin
	exec log2_insert @UpdatedBy,'PeriodCloseout',@UserID,'Current Earnings GL Account','',@CurrentGLAcct
	END
	end
	/********End Logs************/
GO