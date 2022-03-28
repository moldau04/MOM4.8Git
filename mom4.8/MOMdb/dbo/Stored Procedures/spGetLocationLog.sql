CREATE Procedure [dbo].[spGetLocationLog] 
     @LocID    INT
AS
BEGIN
	select * from (
	select * from Log2 where ref= @LocID and (Screen='Location' )
	union all
	select * from Log2 where ref in (select Owner from Loc where Loc=@LocID) and  Screen ='iCollections Popup'
	)as Logs  order by CreatedStamp desc
END	

