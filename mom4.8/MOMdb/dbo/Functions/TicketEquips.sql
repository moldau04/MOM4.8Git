CREATE FUNCTION [dbo].[TicketEquips] 
(
		@ticket int
)
RETURNS varchar(max)
AS
BEGIN

declare @table tblTypeStringToCSV 
insert into @table
select Unit from elev e 
inner join multiple_equipments me on me.elev_id = e.ID and ticket_id = @ticket
union 
select unit from elev where id in (select lelev from TicketO where id = @ticket union select elev from TicketD where id = @ticket)

DECLARE @listStr VARCHAR(MAX)
SELECT @listStr = COALESCE(@listStr+','+CHAR(13) ,'') + Name
FROM @table

delete from @table

return @listStr

END
