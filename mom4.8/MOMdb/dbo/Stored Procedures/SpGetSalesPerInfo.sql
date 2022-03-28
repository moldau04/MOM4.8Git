CREATE PROC [dbo].[SpGetSalesPerInfo](@TicketID int , @LocID int)
AS
Declare @SalesPerMailID nvarchar(50)='';
if Exists(SELECT 1  AS IsSendMailToSalesPer FROM lead WHERE TicketID=@TicketID and isnull(IsSendMailToSalesPer,0)=1 )
BEGIN  
	Declare @SalesPerName nvarchar(50);
	SELECT @SalesPerName=Name FROM  Terr  where id=(SELECT terr FROM loc WHERE loc=@LocID) 
	if exists(Select 1 from tblUser where fUser=@SalesPerName and isnull(NotificationOnAddOpportunity,0)=1)
	Begin 
		Select  @SalesPerMailID= EMail from  tblUser u 
		left outer join  Emp e  on u.fUser=e.CallSign
		left outer join  Rol r on e.Rol=r.ID 
		where isnull(NotificationOnAddOpportunity,0)=1
			and fUser=@SalesPerName
	END 
	--if exists(Select 1 from tblUser u INNER JOIN loc l on l.Terr = u.ID where l.Loc = @LocID and isnull(u.NotificationOnAddOpportunity,0)=1)
	--Begin 
	--	Select  @SalesPerMailID= r.EMail from  tblUser u 
	--	INNER JOIN loc l on l.Terr = u.ID
	--	left outer join  Emp e  on u.fUser=e.CallSign
	--	left outer join  Rol r on e.Rol=r.ID 
	--	where 
	--		l.Loc = @LocID and isnull(u.NotificationOnAddOpportunity,0)=1
	--		--isnull(NotificationOnAddOpportunity,0)=1 
	--		--and fUser=@SalesPerName
	--END 
END

Select  @SalesPerMailID as SalesPerMailID

