

--spGetItemQuantity

CREATE PROCEDURE [dbo].[spGetItemQuantity]


 

as
	begin

			create table #tempquantity
			(
				id int identity (1,1),
				OnHand decimal(17,2) default(0),
				OnOrder decimal(17,2) default(0),
				Comitted decimal(17,2) default(0),
				Avaliable decimal(17,2) default(0),
				IssuesToOpenJobs decimal(17,2) default(0)
			)
			insert into #tempquantity (#tempquantity.OnHand) values 
			(isnull(((select isnull(sum(POItem.Quan),0) as OnHand from Inv inner join POItem on
				Inv.ID=POItem.Inv or POItem.Job is null
				group by Inv.ID)),0))

			update #tempquantity set #tempquantity.OnOrder=
				(select sum(POItem.Quan) as OnOrder from PO
				inner join POItem on PO.PO=POItem.PO
				where PO.Status=0)


				update #tempquantity set #tempquantity.Comitted=
				(select sum(POItem.Quan) as Comitted from PO
				inner join POItem on PO.PO=POItem.PO
				inner join Job on POItem.Job=Job.ID
				where PO.Status=0 and Job.Status=0)


				update #tempquantity set #tempquantity.Avaliable=(#tempquantity.OnHand-#tempquantity.Comitted)


				update #tempquantity set #tempquantity.IssuesToOpenJobs=
				(select sum(POItem.Quan) as Comitted from PO
				inner join POItem on PO.PO=POItem.PO
				inner join Job on POItem.Job=Job.ID
				where Job.Status=0)
				
				select * from #tempquantity
				--select * from Job
				--select * from JobTItem
				--select * from BOM
	end