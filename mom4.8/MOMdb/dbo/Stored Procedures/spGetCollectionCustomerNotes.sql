CREATE PROCEDURE [dbo].[spGetCollectionCustomerNotes]
	@fDate Datetime
AS
BEGIN	
    DECLARE @acct INT
	SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	
    
	--select * from CollectionNotes where LocID is null and OwnerID in
	--(select Owner from Loc where loc in( ( select AcctSub FROM Trans tsub 
	--where Acct=@acct and tsub.AcctSub is not null  and tsub.fdate<=@fDate 
	--group by tsub.AcctSub 
	--having sum(tsub.Amount)<>0
	--))) order by CreatedDate DESC
    

	select cn.* from CollectionNotes cn  
	left join (
	select Owner from Loc where loc in( ( select AcctSub FROM Trans tsub 
	where Acct=@acct and tsub.AcctSub is not null  and tsub.fdate<=@fDate 
	group by tsub.AcctSub 
	having sum(tsub.Amount)<>0
	)))t on t.Owner=cn.OwnerID
	where LocID is null
END