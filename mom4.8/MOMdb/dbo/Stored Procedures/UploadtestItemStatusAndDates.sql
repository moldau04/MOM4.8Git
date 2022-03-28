CREAte PROC [dbo].[UpdateLoadTestItemStatusAndDates] (
  @idTestItem INT
 ,@fStatus INT
 ,@jLast SMALLDATETIME
) AS

 UPDATE LoadTestItem
    SET Status=@fStatus
       ,Last = @jLast
       ,Ticket=NULL
   FROM LoadTestItem i INNER JOIN LoadTest t ON i.ID=t.ID
  WHERE i.LID=@idTestItem
GO

