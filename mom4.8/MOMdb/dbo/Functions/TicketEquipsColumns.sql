CREATE FUNCTION [dbo].[TicketEquipsColumns] 
(
		@ticket int,
		@Column varchar(20)
)
RETURNS varchar(max)
AS
BEGIN

declare @table tblTypeStringToCSV 

if (@Column = 'category')
begin
insert into @table
select Type from elev e 
inner join multiple_equipments me on me.elev_id = e.ID and ticket_id = @ticket
union 
select Type from elev 
where id in (select lelev from TicketO where id = @ticket union select elev from TicketD where id = @ticket)
end
else if (@Column = 'type')
begin
insert into @table
select Category from elev e 
inner join multiple_equipments me on me.elev_id = e.ID and ticket_id = @ticket
union 
select Category from elev 
where id in (select lelev from TicketO where id = @ticket union select elev from TicketD where id = @ticket)
end

DECLARE @listStr VARCHAR(MAX)
SELECT @listStr = COALESCE(@listStr+','+CHAR(13) ,'') + Name
FROM @table

delete from @table

return @listStr

END
