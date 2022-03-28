



CREATE PROCEDURE [dbo].[spDeleteInventoryInBulk]
@Ids xml 

as
	begin
			begin try
				create table #tempitems
(
	Id int identity(1,1),
	Inventoryid int
)

				insert into #tempitems
select
a.b.value('ID[1]','int')
from @Ids.nodes('Inventory')a(b)

				if exists(select 1 from #tempitems)
	begin
		
		declare @index int
		
		set @index=(select count(*) from #tempitems)

		while(@index>0)
			begin
				
				if exists (select 1 from #tempitems inner join Inv on #tempitems.Inventoryid=Inv.ID where #tempitems.Id=@index)
					--begin
						--delete from Inv where Inv.ID=(select #tempitems.Inventoryid from #tempitems where #tempitems.Id=@index)
					--end
				set @index=@index-1
			end
	end

				select ID from Inv where Inv.ID in
		(select #tempitems.Inventoryid from #tempitems)

				drop table #tempitems
			end try

			begin catch
				
			end catch
		

	end
