CREATE  PROCEDURE [dbo].[spGetCollectionLocationNotes]
	@fDate Datetime
AS
BEGIN
	
	DECLARE @acct INT
	SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

	--select * from CollectionNotes where LocID 
	
	--in( select AcctSub FROM Trans tsub where Acct=@acct and tsub.AcctSub is not null  and tsub.fdate<=@fDate group by tsub.AcctSub having sum(tsub.Amount)<>0)
	-- order by CreatedDate DESC

	 select cn.* from CollectionNotes cn
	 left join (
	 select AcctSub FROM Trans tsub where Acct=@acct and tsub.AcctSub is not null  and tsub.fdate<=@fDate group by tsub.AcctSub having sum(tsub.Amount)<>0
	 )t on t.AcctSub=cn.LocID
     
END