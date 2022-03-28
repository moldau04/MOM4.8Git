﻿CREATE FUNCTION [dbo].[GetAlreadyCreatedTicketInCSV] 
(
		 @JobID int ,
		 @scheduledt datetime,
		 @Type nvarchar(10)
)
RETURNS varchar(max)
AS
BEGIN

DECLARE @listStr VARCHAR(MAX); 
DECLARE @listStr2 VARCHAR(MAX); 

        Set @listStr='';
		Set @listStr2='';

		if(@Type='Ticket')
		BEGIN
        SELECT @listStr +=  STUFF( (SELECT ',' + cast(t.ID as nvarchar) FROM TicketO t 
		where t.Job = @JobID 
        and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date, @scheduledt))
		FOR XML PATH ('')), 1, 1, '' )  

		SELECT @listStr2 =  STUFF( (SELECT ',' + cast(t.ID as nvarchar) FROM TicketD t 
		where t.Job = @JobID 
        and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date, @scheduledt))
		FOR XML PATH ('')), 1, 1, '' )  
		END
		ELSE
		BEGIN

		SELECT @listStr +=  STUFF( (SELECT ',' + cast(Elev.Unit as nvarchar) FROM TicketO t 
		inner join Elev on Elev.ID=t.LElev
		where t.Job = @JobID 
        and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date, @scheduledt))
		FOR XML PATH ('')), 1, 1, '' )  

		SELECT @listStr2 =  STUFF( (SELECT ',' + cast(Elev.Unit as nvarchar) FROM TicketD t 
		inner join Elev on Elev.ID=t.Elev
		where t.Job = @JobID 
        and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date, @scheduledt))
		FOR XML PATH ('')), 1, 1, '' )  

		END 

	   if(len(@listStr) > 0  and len(@listStr2) > 0 ) 
	   	   
	   set @listStr = @listStr +','+@listStr2

	   else if(len(@listStr2) > 0 )

	   set @listStr = @listStr2

return @listStr

END
GO

