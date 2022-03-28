Create proc [dbo].[spSyncUpdate]
as

select 1
--BEGIN/********Customer Sync********/
    
--Insert into AHES.dbo.OType
--(Type, Remarks)
--select 
--type, remarks from AHEI.dbo.OType where  
--Type in (select o.Type from AHES.dbo.Owner owner_sec inner join AHEI.dbo.Owner o on o.ID = owner_sec.PrimarySyncID
--and (select LastUpdateDate from AHEI.dbo.rol where ID = o.Rol) >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
--and type not in (select type from AHES.dbo.OType)

--update rol_sec set
--rol_sec.Name=r.Name,
--rol_sec.City=r.City,
--rol_sec.State=r.State,
--rol_sec.Zip=r.Zip,
--rol_sec.Address=r.Address,
--rol_sec.Remarks=r.Remarks,
--rol_sec.Country=r.Country,
--rol_sec.Contact=r.Contact,
--rol_sec.Phone=r.Phone,
--rol_sec.Website=r.Website,
--rol_sec.EMail=r.EMail,
--rol_sec.Cellular=r.Cellular,
--rol_sec.LastUpdateDate=getdate()
--from AHES.dbo.Rol rol_sec 
--inner join AHEI.dbo.Rol r on r.ID = rol_sec.PrimarySyncID 
--and r.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) 

--update  owner_sec set
--owner_sec.Status=o.Status,
--owner_sec.Ledger=o.Ledger,
--owner_sec.TicketD=o.TicketD,
--owner_sec.Internet=o.Internet,
--owner_sec.Billing=o.Billing,
--owner_sec.Type=o.Type,
--owner_sec.CPEquipment=o.CPEquipment,
--owner_sec.CreatedBy=o.CreatedBy,
--owner_sec.GroupbyWO=o.GroupbyWO,
--owner_sec.openticket=o.openticket
-- from AHES.dbo.Owner owner_sec inner join AHEI.dbo.Owner o on o.ID = owner_sec.PrimarySyncID
-- and (select LastUpdateDate from AHEI.dbo.rol where ID = o.Rol) >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) 
  
--delete from AHES.dbo.Phone where Rol in (select sr.ID from AHES.dbo.Rol sr inner join AHEI.dbo.Rol r on r.ID = sr.PrimarySyncID 
--and r.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
--INSERT INTO AHES.dbo.Phone
-- (
-- Rol,
-- fDesc,
-- Phone,
-- Fax, 
-- Cell,
-- Email
-- )
-- select 
-- (select top 1 ID from AHES.dbo.Rol where PrimarySyncID = p.Rol),
-- fDesc,
-- Phone,
-- Fax, 
-- Cell,
-- Email 

-- from AHEI.dbo.Phone p where Rol in (select r.ID from AHES.dbo.Rol sr inner join AHEI.dbo.Rol r on r.ID = sr.PrimarySyncID 
-- and r.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--END
--/********End Customer Sync***********/





--BEGIN/********Location Sync********/

--Insert into AHES.dbo.STax
--(name,fdesc,rate,state, Type,UType,IsTaxable, Remarks)
--select 
--name,fdesc,rate,state, Type,UType,IsTaxable, Remarks
--from AHEI.dbo.STax where  Name not in (select Name from AHES.dbo.STax) 
--and Name in (select l.STax from AHES.dbo.Loc sl inner join AHEI.dbo.loc l on l.loc = sl.PrimarysyncID
--and (select LastUpdateDate from AHEI.dbo.rol where ID = l.Rol) >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--Insert into AHES.dbo.LocType
--(Type, Remarks)
--select 
--type, remarks from AHEI.dbo.LocType where  Type in
-- (select l.Type from AHES.dbo.Loc sl inner join AHEI.dbo.loc l on l.loc = sl.PrimarysyncID
-- and (select LastUpdateDate from AHEI.dbo.rol where ID = l.Rol) >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and type not in 
-- (select type from AHES.dbo.LocType)


--update sl set
--sl.ID=l.ID,
--sl.Tag=l.Tag,
--sl.Address=l.Address,
--sl.City=l.City,
--sl.State=l.State,
--sl.Zip=l.Zip,
--sl.Status=l.Status,
--sl.Type=l.Type,
--sl.Owner=(select top 1 ID from AHES.dbo.owner where PrimarySyncID = l.owner) ,
--sl.STax = l.STax,
--sl.Custom1= l.Custom1,
--sl.Custom2=l.Custom2,
--sl.Custom14=l.Custom14,
--sl.Custom15=l.Custom15,
--sl.Custom12=l.Custom12,
--sl.Custom13=l.Custom13,
--sl.Remarks=l.Remarks,
--sl.DispAlert=l.DispAlert,
--sl.Credit=l.Credit,
--sl.CreditReason=l.CreditReason,
--sl.Prospect=l.prospect,
--sl.Billing=l.Billing,
--sl.DefaultTerms=l.DefaultTerms,
--sl.CreatedBy=l.CreatedBy
--from AHES.dbo.Loc sl inner join AHEI.dbo.loc l on l.loc = sl.PrimarysyncID
--and (select LastUpdateDate from AHEI.dbo.rol where ID = l.Rol) >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) 


--END
--/********End Location Sync***********/





--BEGIN/********Equipment Sync********/


--Insert into AHES.dbo.ElevatorSpec
--(ECat,EDesc)
--select 
--ECat,EDesc
--from AHEI.dbo.ElevatorSpec where ecat = 1 and  EDesc 
--in (select e.Type from AHEI.dbo.Elev e inner join AHES.dbo.Elev et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and edesc not in (select edesc from AHES.dbo.ElevatorSpec where ECat = 1)
 

--Insert into AHES.dbo.ElevatorSpec
--(ECat,EDesc)
--select 
--ECat,EDesc
--from AHEI.dbo.ElevatorSpec where ecat =0 and  EDesc 
--in (select e.Category from AHEI.dbo.Elev e inner join AHES.dbo.Elev et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and edesc not in (select edesc from AHES.dbo.ElevatorSpec where ECat = 0)
 


--Insert into AHES.dbo.LType
--(Type,fDesc,MatCharge,Remarks,Free)
--select 
--Type,fDesc,MatCharge,Remarks,Free
--from AHEI.dbo.LType where Type in (select e.Cat from AHEI.dbo.Elev e inner join AHES.dbo.Elev et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and type not in (select type from AHES.dbo.LType)
 

--Declare @j int;
--Select @j = isnull(max(ID),0) from AHES.dbo.ElevT;
--Insert into AHES.dbo.ElevT
--(ID,fDesc,Remarks,PrimarySyncID)
--select
--row_number() over(order by ID) + @j 
--,fDesc,Remarks,ID
--from AHEI.dbo.ElevT where  ID in 
--(select e.Template from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--and ID not in (select PrimarySyncID from AHES.dbo.ElevT)
 
--Declare @k int;
--Select @k =ISNULL( max(ID),0)  from AHES.dbo.ElevTItem;
  
--DELETE FROM AHES.dbo.ElevTItem WHERE Elev = 0 and ElevT in 
--(select te.Template from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )


--INSERT INTO AHES.dbo.ElevTItem
--(ID,
--ElevT,
--Elev,
--CustomID,
--fDesc,
--Line,
--Value,
--Format,
--PrimarySyncID
--)
--select 
--row_number() over(order by ID) + @k ,
--(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = eti.ElevT),
--0,
--row_number() over(order by ID) + @k,
--fDesc,
--Line,
--Value,
--Format,
--ID
--from AHEI.dbo.ElevTItem eti where ElevT in  
--(select e.Template from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and Elev = 0

--DELETE FROM AHES.dbo.tblCustomValues WHERE ElevT in 
--(select te.Template from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )


--insert into AHES.dbo.tblCustomValues		
--(
--ElevT,
--ItemID,
--Line,
--Value
--)	
--select 
--(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = cv.ElevT),
--(select  top 1 ID from AHES.dbo.ElevTItem where PrimarySyncID = cv.ItemID),
--Line,
--Value
--from AHEI.dbo.tblCustomValues cv where ElevT in
--(select e.Template from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control)
--)  


--update elev_sec set
--elev_sec.Loc = (select top 1  loc from AHES.dbo.Loc where PrimarySyncID = e.Loc),
--elev_sec.Owner = (select top 1 ID from AHES.dbo.Owner where PrimarySyncID = e.owner),
--elev_sec.Unit = e.Unit ,
--elev_sec.fDesc = e.fDesc,
--elev_sec.Type = e.Type,
--elev_sec.Cat = e.Cat,
--elev_sec.Manuf = e.Manuf,
--elev_sec.Serial = e.Serial,
--elev_sec.State = e.State,
--elev_sec.Since = e.Since,
--elev_sec.Last = e.Last,
--elev_sec.Price = e.price,
--elev_sec.Status = e.Status,
--elev_sec.Building= e.Building,
--elev_sec.Remarks=e.Remarks,
--elev_sec.fGroup=e.fGroup,
--elev_sec.Template=(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = e.Template),
--elev_sec.InstallBy= e.InstallBy,
--elev_sec.Install=e.Install,
--elev_sec.category=e.Category,
--elev_sec.LastUpdateDate=GETDATE()
--from AHES.dbo.Elev elev_sec inner join AHEI.dbo.Elev e on e.ID= elev_sec.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) 



--Declare @i int;
--Select @i =ISNULL( max(ID),0)  from AHES.dbo.ElevTItem;
--DELETE FROM AHES.dbo.ElevTItem WHERE  Elev in (select te.ID from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )


--INSERT INTO AHES.dbo.ElevTItem
--(ID,
--ElevT,
--Elev,
--CustomID,
--fDesc,
--Line,
--Value,
--Format,
--PrimarySyncID
--)
--select 
--row_number() over(order by ID) + @i ,
--(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = eti.ElevT),
--(select top 1 ID from AHES.dbo.Elev where PrimarySyncID = eti.Elev),
--(select top 1 ID from AHES.dbo.ElevTItem where PrimarySyncID = eti.CustomID),
--fDesc,
--Line,
--Value,
--Format,
--ID
--from AHEI.dbo.ElevTItem eti WHERE  Elev in (select e.ID from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--DELETE FROM AHES.dbo.EquipTItem WHERE  Elev in (select te.ID from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
--insert into AHES.dbo.EquipTItem
--(
--Code,
--EquipT,
--Elev,
--fDesc,
--Frequency,
--Lastdate,
--Line ,
--NextDateDue,
--Section
--)
--select	
--code,
--(select top 1 ID from AHES.dbo.EquipTemp et where et.PrimarySyncID = eti.EquipT),
--(select top 1 ID from AHES.dbo.Elev e where e.PrimarySyncID = eti.Elev),
--fDesc,
--Frequency,
--Lastdate,
--Line, 
--NextDateDue,
--Section
--from	
--AHEI.dbo.EquipTItem eti where Elev in (select e.ID from AHES.dbo.Elev te inner join AHEI.dbo.Elev e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )



--END
--/********End Equipment Sync***********/





--BEGIN/********Job Sync********/

--Insert into AHES.dbo.JobType
--(Type,Remarks,ISDefault,PrimarySyncID)
--select 
--Type,Remarks,ISDefault,ID
--from AHEI.dbo.JobType where  ID 
--in (select e.type from AHEI.dbo.job e inner join AHES.dbo.job et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and ID not in (select PrimarySyncID from AHES.dbo.JobType)
 
    
--update sj set
--sj.Loc=(select top 1 loc from AHES.dbo.loc where PrimarySyncID = j.Loc),
--sj.owner=(select top 1 ID from AHES.dbo.Owner where PrimarySyncID = j.Owner),
--sj.fdate = j.fDate,
--sj.Status=j.Status,
--sj.Remarks= j.Remarks,
--sj.fDesc=j.fdesc,
--sj.Type=(select top 1 ID from AHES.dbo.JobType where PrimarySyncID = j.Type),
--sj.CType=j.CType,
--sj.PO=j.PO,
--sj.so=j.SO,
--sj.Certified=j.Certified,
--sj.Template=j.Template,
--sj.Custom21=j.Custom21,
--sj.Custom22=j.Custom22,
--sj.Custom23=j.Custom23,
--sj.Custom24=j.Custom24,
--sj.Custom25=j.Custom25,
--sj.ProjCreationDate=j.ProjCreationDate, 
--sj.Rol=sj.rol		,
--sj.LastUpdateDate=GETDATE()								
--from 
--AHES.dbo.Job sj inner join AHEI.dbo.job j on j.id= sj.PrimarySyncID
--and j.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) 

--delete from AHES.dbo.JobTItem where Job in
--(select te.ID from AHES.dbo.job te inner join AHEI.dbo.job e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--INSERT INTO AHES.dbo.JobTItem
--(
--JobT,
--Job,
--Type,
--fDesc,
--Code,
--Actual,
--Budget,
--Line,
--[Percent],
--Comm,
--Modifier,
--ETC,
--ETCMod,
--Labor, 
--Stored
--)
--Select 
--0,
--(select top 1 ID from AHES.dbo.Job where PrimarySyncID = jt.Job),
--Type,
--fDesc,
--Code,
--Actual,
--Budget,
--Line,
--[Percent],
--Comm,
--Modifier,
--ETC,
--ETCMod,
--Labor, 
--Stored
--from AHEI.dbo.JobTItem jt
-- WHERE  Job in (select te.ID from AHES.dbo.Job te inner join AHEI.dbo.Job e on e.ID = te.PrimarySyncID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )

--END
--/********End Job Sync***********/




--BEGIN/********Ticket Sync********/
   

--Insert into AHES.dbo.Category
--(Type,Remarks,Icon,chargeable,ISDefault)
--select 
--Type,Remarks,Icon,chargeable,ISDefault
--from AHEI.dbo.Category where  Type 
--in (select e.cat from AHEI.dbo.ticketd e inner join AHES.dbo.ticketd et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and Type not in (select Type from AHES.dbo.Category)


--Insert into AHES.dbo.JobType
--(Type,Remarks,ISDefault,PrimarySyncID)
--select 
--Type,Remarks,ISDefault,ID
--from AHEI.dbo.JobType where  ID  
--in (select e.type from AHEI.dbo.ticketd e inner join AHES.dbo.ticketd et on et.PrimarySyncID = e.ID
--and e.LastUpdateDate >= (select isnull(SyncLast,'01/01/1753') from AHES.dbo.control) )
-- and ID not in (select PrimarySyncID from AHES.dbo.JobType)

--Update sec_ticket set
--sec_ticket.CDate = d.CDate,
--sec_ticket.EDate=d.EDate,
--sec_ticket.TimeRoute=d.TimeRoute,
--sec_ticket.TimeSite=d.TimeSite,
--sec_ticket.TimeComp=d.TimeComp,
--sec_ticket.Cat=d.Cat,
--sec_ticket.fDesc=d.fDesc,
--sec_ticket.Est=d.Est,
--sec_ticket.fWork=(case when d.fWork not in (select ID from AHES.dbo.tblWork ) then (select min(ID) from AHES.dbo.tblWork) else d.fWork end ),
--sec_ticket.Loc=(select top 1 loc from AHES.dbo.loc where PrimarySyncID = d.Loc),
--sec_ticket.DescRes=d.DescRes,
--sec_ticket.Reg=d.Reg,
--sec_ticket.OT=d.OT,
--sec_ticket.NT=d.NT,
--sec_ticket.TT=d.TT,
--sec_ticket.DT=d.DT,
--sec_ticket.Total=d.Total,
--sec_ticket.Charge=d.Charge,
--sec_ticket.ClearCheck=d.ClearCheck,
--sec_ticket.Who=d.Who,
--sec_ticket.Type=(select top 1 ID from AHES.dbo.JobType where PrimarySyncID = d.Type),
--sec_ticket.Status=d.Status,
--sec_ticket.Elev=(select top 1 ID from AHES.dbo.Elev where PrimarySyncID = d.Elev),
--sec_ticket.BRemarks=d.BRemarks,
--sec_ticket.Level=d.Level,
--sec_ticket.Custom1=d.Custom1,
--sec_ticket.Custom2=d.Custom2,
--sec_ticket.Custom3=d.Custom3,
--sec_ticket.Custom4=d.Custom4,
--sec_ticket.Custom5=d.Custom5,
--sec_ticket.Custom6=d.Custom6,
--sec_ticket.Custom7=d.Custom7,
--sec_ticket.WorkOrder=d.WorkOrder,
--sec_ticket.WorkComplete=d.WorkComplete,
--sec_ticket.OtherE=d.OtherE,
--sec_ticket.Toll=d.Toll,
--sec_ticket.Zone=d.Zone,
--sec_ticket.SMile=d.SMile,
--sec_ticket.EMile=d.EMile,
--sec_ticket.Internet=d.Internet,
--sec_ticket.ManualInvoice=d.ManualInvoice,
--sec_ticket.lastupdatedate=getdate(),
--sec_ticket.TransferTime=d.TransferTime,
----sec_ticket.QBServiceItem=d.QBServiceItem,
----sec_ticket.QBPayrollItem=d.QBPayrollItem,
--sec_ticket.CPhone=d.CPhone,
--sec_ticket.CustomTick1 =d.CustomTick1,
--sec_ticket.CustomTick2 =d.CustomTick2,
--sec_ticket.CustomTick3 =d.CustomTick3,
--sec_ticket.CustomTick4 =d.customtick4,
--sec_ticket.CustomTick5= d.CustomTick5,
--sec_ticket.Job=(select top 1 ID from AHES.dbo.Job where PrimarySyncID = d.Job),
--sec_ticket.JobCode=d.JobCode,
--sec_ticket.Phase=d.Phase,
--sec_ticket.WageC=d.WageC,
--sec_ticket.fBy	=d.fBy,
--sec_ticket.JobItemDesc	=d.JobItemDesc
--from AHES.dbo.TicketD sec_ticket inner join AHEI.dbo.ticketd d on d.id= sec_ticket.PrimarySyncID



--END
--/********End Ticket Sync***********/



    
    
    
    
    
