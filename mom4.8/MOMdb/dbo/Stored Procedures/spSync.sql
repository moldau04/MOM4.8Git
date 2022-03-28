CREATE proc [dbo].[spSync]
as
 


/* Select Jobs Where Passed Inspection Date is Not NULL */
CREATE TABLE #elev(ID int, loc int, owner int, job int)

INSERT INTO #elev 

SELECT   0, loc, owner, id  from AHEI.dbo.job
  WHERE
   
   id in (SELECT JobID
              FROM   AHEI.dbo.tblCustomJob
              WHERE  
			  rtrim(ltrim(isnull(Value,''))) <> ''
                     AND
				      
					  tblCustomFieldsID  in ( Select id  from AHEI.dbo.[tblCustomFields] where ( Label='Passed Inspection' or Label ='Unit Activated' ) ))

					  -----26 NOV 2019  ES-2945 Accredited - Projects/Projects - Specific PA/NY Templates - AHEI to AHES - Transfer Request  
					  -----  All the affected templates will need to transfer when unit activated is activated.

UNION 
SELECT 0, loc, owner, id from AHEI.dbo.job where Status in (1,3) 

/*Step1********Start Customer Sync********/

DECLARE @Owner int 

BEGIN
DECLARE db_cursor CURSOR FOR
select ID from AHEI.dbo.owner where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.owner) and ID in (select owner from #elev)
--SELECT Owner from Job where Status = 3 
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @Owner

WHILE @@FETCH_STATUS = 0
BEGIN
--BEGIN TRANSACTION  
    
 ---$$$$$$ INSERT  ROLE   
INSERT INTO AHES.dbo.Rol
(Name,
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Country,
Contact,
Phone,
Website,
EMail,
Cellular,
PrimarySyncID,
LastUpdateDate
)
select
Name,
City,
State,
Zip,
Address,
0,
Remarks,
0,
Country,
Contact,
Phone,
Website,
EMail,
Cellular,
ID,
GETDATE()
from AHEI.dbo.Rol r where ID =(select Rol from AHEI.dbo.owner where ID =@owner )
--declare @Rol int
--SET @Rol=Scope_identity()

-----$$$$$ INSERT OTYPE
IF NOT EXISTS (SELECT 1 FROM AHES.dbo.OType WHERE TYPE = (SELECT TYPE FROM AHEI.DBO.OWNER WHERE ID =@Owner))
BEGIN
Insert into AHES.dbo.OType
(Type, Remarks)
select 
type, remarks from AHEI.dbo.OType where  Type = (select Type from AHEI.dbo.Owner where ID =@Owner)
END

----$$$$$$ INSERT OWNER
INSERT INTO AHES.dbo.Owner
(
Status,
Ledger,
TicketD,
Internet,
Rol,
Billing,
Type,
CPEquipment,
CreatedBy,
GroupbyWO,
openticket ,
PrimarySyncID
)
SELECT
Status,
Ledger,
TicketD,
Internet,
(SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT  top 1 Rol FROM AHEI.dbo.OWNER WHERE ID = @Owner)),
Billing,
Type,
CPEquipment,
CreatedBy,
GroupbyWO,
openticket,
ID
FROM AHEI.dbo.Owner o where ID = @Owner

----$$$$ INSERT PHONE  
INSERT INTO AHES.dbo.PHONE
 (
 Rol,
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email
 )
 SELECT 
(SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT top 1 Rol FROM AHEI.dbo.OWNER WHERE ID = @Owner)),
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email 
 from AHEI.dbo.Phone p where Rol=(select Rol from AHEI.dbo.Owner where ID = @Owner)

IF @@ERROR <> 0
AND @@TRANCOUNT > 0
BEGIN
RAISERROR ('Error Occured',16,1)
--ROLLBACK TRANSACTION
CLOSE db_cursor
DEALLOCATE db_cursor
RETURN
END


--COMMIT TRANSACTION
FETCH NEXT FROM db_cursor INTO @Owner
END

CLOSE db_cursor
DEALLOCATE db_cursor

END

/********End Customer Sync***********/
 

 /*Step2*******Location Sync********/

DECLARE @Loc int
BEGIN
DECLARE db_cursor CURSOR FOR
select loc from AHEI.dbo.loc where Loc not in (select  isnull(PrimarySyncID,0) from AHES.dbo.loc) and loc in (select loc from #elev)
--SELECT Owner from Job where Status = 3 
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @Loc

WHILE @@FETCH_STATUS = 0
BEGIN
--BEGIN TRANSACTION  

---$$$ INSERT ROL  
    
INSERT INTO AHES.dbo.Rol
(City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
PrimarySyncID,
LastUpdateDate
)
select
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
ID,
GETDATE()
from AHEI.dbo.Rol r where ID =(select Rol from AHEI.dbo.loc where Loc =@Loc )
--declare @Rol int
--SET @Rol=Scope_identity()

--$$$ INSERT LocType
if not exists (select 1 from AHES.dbo.LocType where Type = (select Type from AHEI.dbo.Loc where Loc =@Loc))
BEGIN
Insert into AHES.dbo.LocType
(Type, Remarks)
select 
type, remarks from AHEI.dbo.LocType where  Type = (select Type from AHEI.dbo.Loc where Loc =@Loc)
END

----$$$ INSERT STax

if not exists (select 1 from AHES.dbo.STax where Name = (select STax from AHEI.dbo.Loc where Loc =@Loc))
BEGIN
Insert into AHES.dbo.STax
(name,fdesc,rate,state, Type,UType,IsTaxable, Remarks)
select 
name,fdesc,rate,state, Type,UType,IsTaxable, Remarks
from AHEI.dbo.STax where  Name = (select STax from AHEI.dbo.Loc where Loc =@Loc)
END

------INSERT GC  Information
IF((select isnull(GContractorID,0) from AHEI.dbo.Loc where Loc =@Loc) <> 0)
if not exists (select 1 from AHES.dbo.Rol where PrimarySyncID = (select GContractorID from AHEI.dbo.Loc where Loc =@Loc))
BEGIN
INSERT INTO AHES.dbo.Rol
(City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
PrimarySyncID,
LastUpdateDate
)
select
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
ID,
GETDATE()
from AHEI.dbo.Rol r where ID =(select GContractorID from AHEI.dbo.Loc where Loc =@Loc) 
DECLARE @GCRol int;
SET @GCRol=Scope_identity();
IF NOT EXISTS(select 1 from AHES.dbo.tblLocAddlContact where RolID=@GCRol And LocContactTypeID=1 )
BEGIN
INSERT INTO AHES.dbo.tblLocAddlContact(RolID,LocContactTypeID)values(@GCRol,1)
END
END


------INSERT HO  Information
IF((select isnull(HomeOwnerID,0) from AHEI.dbo.Loc where Loc =@Loc) <> 0)
if not exists (select 1 from AHES.dbo.Rol where PrimarySyncID = (select HomeOwnerID from AHEI.dbo.Loc where Loc =@Loc))
BEGIN
INSERT INTO AHES.dbo.Rol
(City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
PrimarySyncID,
LastUpdateDate
)
select
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Contact,
Name,
Phone,
Website,
EMail,
Cellular,
Fax,
Lat,
Lng,
ID,
GETDATE()
from AHEI.dbo.Rol r where ID =(select HomeOwnerID from AHEI.dbo.Loc where Loc =@Loc) 
DECLARE @HORol int;
SET @HORol=Scope_identity();
IF NOT EXISTS(select 1 from AHES.dbo.tblLocAddlContact where RolID=@HORol And LocContactTypeID=2 )
BEGIN
INSERT INTO AHES.dbo.tblLocAddlContact(RolID,LocContactTypeID)values(@HORol,2)
END
END

print(@Loc)
----$$$ INSERT Location 
INSERT INTO AHES.dbo.Loc
(
PrimarySyncID,
ID,
Tag,
Address,
City,
State,
Zip,
Rol,
Status,
Type,
--Route,
--Terr,
Owner,
STax,
Custom1,
Custom2,
Custom14,
Custom15,
Custom12,
Custom13,
Remarks,
DispAlert,
Credit,
CreditReason,
Prospect,
Billing,
DefaultTerms,
CreatedBy,
GContractorID,
HomeOwnerID
)
select
Loc,
ID,
Tag,
Address,
City,
State,
Zip,
(SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT top 1 Rol FROM AHEI.dbo.LOC WHERE LOC = @Loc)),
Status,
Type,
--Route,
--Terr,
(select top 1 ID from AHES.dbo.owner where PrimarySyncID=Owner),
STax,
Custom1,
Custom2,
Custom14,
Custom15,
Custom12,
Custom13,
Remarks,
DispAlert,
Credit,
CreditReason,
Prospect,
Billing,
DefaultTerms,
CreatedBy,
(SELECT CASE (SELECT top 1 ISNULL(GContractorID,0) FROM AHEI.dbo.LOC WHERE LOC = @Loc) WHEN 0
THEN 0
ELSE (SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT top 1 GContractorID FROM AHEI.dbo.LOC WHERE LOC = @Loc)) end),

(SELECT CASE (SELECT top 1 ISNULL(HomeOwnerID,0) FROM AHEI.dbo.LOC WHERE LOC = @Loc) WHEN 0
THEN 0
ELSE (SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT top 1 HomeOwnerID FROM AHEI.dbo.LOC WHERE LOC = @Loc)) end)
 from AHEI.dbo.Loc o where Loc= @Loc


  ----------------------Insert Documents-----------------------------
INSERT INTO AHES.dbo.Documents(Screen,ScreenID,Filename,Path,Date,fDesc,Type)
select Screen,(select LOC from AHES.dbo.LOC  where PrimarySyncID =@Loc),Filename,Path,Date,fDesc,Type 
from AHEI.dbo.Documents where ScreenID=@Loc and Screen='Location'
-------------------------Insert Documents-----------------------------
  
INSERT INTO AHES.dbo.Phone
 (
 Rol,
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email
 )
 SELECT 
 (SELECT top 1 ID FROM AHES.dbo.ROL WHERE PrimarySyncID=(SELECT top 1 Rol FROM AHEI.dbo.LOC WHERE LOC = @Loc)),
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email 
 from AHEI.dbo.Phone p where Rol=(select Rol from AHEI.dbo.Loc where Loc = @Loc)

IF @@ERROR <> 0
AND @@TRANCOUNT > 0
BEGIN
RAISERROR ('Error Occured',16,1)
--ROLLBACK TRANSACTION
CLOSE db_cursor
DEALLOCATE db_cursor
RETURN
END


--COMMIT TRANSACTION
FETCH NEXT FROM db_cursor INTO @Loc
END

CLOSE db_cursor
DEALLOCATE db_cursor

END
/********End Location Sync***********/


DECLARE @equiptemp as table (ID int)
INSERT into @equiptemp select ID from AHEI.dbo.EquipTemp where ID 
not in 
(select isnull(PrimarySyncID,0) from AHES.dbo.EquipTemp)

INSERT INTO AHES.dbo.EquipTemp
(
fdesc,
Remarks,
primarysyncID)
select fdesc,remarks,ID from AHEI.dbo.EquipTemp where ID in (select id from @equiptemp)
--not in 
--(select isnull(PrimarySyncID,0) from AHES.dbo.EquipTemp)

INSERT INTO AHES.dbo.EquipTItem
(Code,
[EquipT],
[Elev],
[fDesc],
[Line],
[Frequency],
Section,
PrimarySyncID
)

SELECT 
code,
(select ID from AHES.dbo.EquipTemp et where et.PrimarySyncID = eti.EquipT),
0,
fdesc,
line,
Frequency,
Section,
ID
FROM   AHEI.dbo.EquipTItem eti where Elev = 0 and EquipT in (select id from @equiptemp)
--(select ID from EquipTemp where ID not in 
--(select isnull(PrimarySyncID,0) from AHES.dbo.EquipTemp))



DECLARE @Equip int
BEGIN/********Equipment Sync********/
DECLARE db_cursor CURSOR FOR
select ID from AHEI.dbo.elev where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.elev) and ID in 
(
--select ID from #elev
--union 
select elev from AHEI.dbo.ticketD 
	where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.ticketD) 
	and job in (select job from #elev) 
)
--SELECT Owner from Job where Status = 3 
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @Equip

WHILE @@FETCH_STATUS = 0
BEGIN
--BEGIN TRANSACTION  

if not exists (select 1 from AHES.dbo.ElevatorSpec where ECat = 1 and EDesc = (select Type from AHEI.dbo.Elev where ID=@Equip))
BEGIN
Insert into AHES.dbo.ElevatorSpec
(ECat,EDesc)
select 
ECat,EDesc
from AHEI.dbo.ElevatorSpec where  EDesc = (select Type from AHEI.dbo.Elev where ID =@Equip)
END

if not exists (select 1 from AHES.dbo.ElevatorSpec where ECat = 0 and EDesc = (select Category from AHEI.dbo.Elev where ID=@Equip))
BEGIN
Insert into AHES.dbo.ElevatorSpec
(ECat,EDesc)
select 
ECat,EDesc
from AHEI.dbo.ElevatorSpec where  EDesc = (select Category from AHEI.dbo.Elev where ID =@Equip)
END

if not exists (select 1 from AHES.dbo.LType where Type = (select Cat from AHEI.dbo.Elev where ID=@Equip))
BEGIN
Insert into AHES.dbo.LType
(Type,fDesc,MatCharge,Remarks,Free)
select 
Type,fDesc,MatCharge,Remarks,Free
from AHEI.dbo.LType where  Type = (select Cat from AHEI.dbo.Elev where ID =@Equip)
END

if not exists 
--(select 1 from AHES.dbo.ElevT where fDesc = (select fdesc from elevt where ID = ( select  Template from Elev where ID=@Equip)))
(select 1 from AHES.dbo.ElevT where PrimarySyncID = ( select  Template from AHEI.dbo.Elev where ID=@Equip))
BEGIN
Declare @j int;
Select @j = isnull(max(ID),0) from AHES.dbo.ElevT;
Insert into AHES.dbo.ElevT
(ID,fDesc,Remarks,PrimarySyncID)
select
row_number() over(order by ID) + @j 
,fDesc,Remarks,ID
from AHEI.dbo.ElevT where  ID = (select Template from AHEI.dbo.Elev where ID =@Equip)



Declare @k int;
Select @k =ISNULL( max(ID),0)  from AHES.dbo.ElevTItem;
  
DELETE FROM AHES.dbo.ElevTItem WHERE Elev = 0 and ElevT = (select Template from AHES.dbo.Elev where PrimarySyncID = @Equip)
INSERT INTO AHES.dbo.ElevTItem
(ID,
ElevT,
Elev,
CustomID,
fDesc,
Line,
Value,
Format,
PrimarySyncID
)
select 
row_number() over(order by ID) + @k ,
(select ID from AHES.dbo.ElevT where PrimarySyncID = eti.ElevT),
0,
row_number() over(order by ID) + @k,
fDesc,
Line,
Value,
Format,
ID
from AHEI.dbo.ElevTItem eti where ElevT =  (select Template from AHEI.dbo.Elev where ID =@Equip) and Elev = 0

DELETE FROM AHES.dbo.tblCustomValues WHERE ElevT = (select top 1 Template from AHES.dbo.Elev where PrimarySyncID = @Equip)
insert into AHES.dbo.tblCustomValues		
(
ElevT,
ItemID,
Line,
Value
)	
select 
(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = cv.ElevT),
(select top 1 ID from AHES.dbo.ElevTItem where PrimarySyncID = cv.ItemID),
Line,
Value
from AHEI.dbo.tblCustomValues cv where ElevT =  (select top 1 Template from AHEI.dbo.Elev where ID =@Equip)

END


INSERT INTO AHES.dbo.Elev
(
Loc,
Owner,
Unit,
fDesc,
Type,
Cat,
Manuf,
Serial,
State,
Since,
Last,
Price,
Status,
Building,
Remarks,
fGroup,
Template,
InstallBy,
Install,
category,
PrimarySyncID,
LastUpdateDate
)
select
(select top 1 loc from AHES.dbo.loc where PrimarySyncID = o.Loc),
(select top 1 id from AHES.dbo.owner where PrimarySyncID = o.owner),
Unit,
fDesc,
Type,
Cat,
Manuf,
Serial,
State,
Since,
Last,
Price,
Status,
Building,
Remarks,
fGroup,
(select top 1 ID from AHES.dbo.ElevT where PrimarySyncID = o.Template),
--(select id from AHES.dbo.ElevT where fdesc =(select fdesc from elevt where ID = ( select  Template from Elev where ID=@Equip)) ),
InstallBy,
Install,
category,
ID,
GETDATE()
 from AHEI.dbo.Elev o where ID = @Equip
  
Declare @i int;
Select @i =ISNULL( max(ID),0)  from AHES.dbo.ElevTItem;
DELETE FROM AHES.dbo.ElevTItem WHERE  Elev = (select top 1 ID from AHES.dbo.Elev where PrimarySyncID = @Equip)
INSERT INTO AHES.dbo.ElevTItem
(ID,
ElevT,
Elev,
CustomID,
fDesc,
Line,
Value,
Format,
PrimarySyncID
)
select 
row_number() over(order by ID) + @i ,
--(select top 1 ( ROW_NUMBER() OVER (ORDER BY ID))+1 from AHES.dbo.ElevTItem order by ID desc),
(select ID from AHES.dbo.ElevT where PrimarySyncID = eti.ElevT),
(select ID from AHES.dbo.Elev where PrimarySyncID = eti.Elev),
(select ID from AHES.dbo.ElevTItem where PrimarySyncID = eti.CustomID),
fDesc,
Line,
Value,
Format,
ID
from AHEI.dbo.ElevTItem eti where Elev = @Equip

DELETE FROM AHES.dbo.EquipTItem WHERE  Elev = (select top 1 ID from AHES.dbo.Elev where PrimarySyncID = @Equip)
insert into AHES.dbo.EquipTItem
(
Code,
EquipT,
Elev,
fDesc,
Frequency,
Lastdate,
Line ,
NextDateDue,
Section
)
select	
code,
(select top 1 ID from AHES.dbo.EquipTemp et where et.PrimarySyncID = eti.EquipT),
(select top 1 ID from AHES.dbo.Elev e where e.PrimarySyncID = eti.Elev),
fDesc,
Frequency,
Lastdate,
Line, 
NextDateDue,
Section
from	
AHEI.dbo.EquipTItem eti where Elev = @Equip


IF @@ERROR <> 0
AND @@TRANCOUNT > 0
BEGIN
RAISERROR ('Error Occured',16,1)
--ROLLBACK TRANSACTION
CLOSE db_cursor
DEALLOCATE db_cursor
RETURN
END

--COMMIT TRANSACTION
FETCH NEXT FROM db_cursor INTO @Equip
END

CLOSE db_cursor
DEALLOCATE db_cursor

END
/********End Equipment Sync***********/


DECLARE @job int
BEGIN/*****#####***Job Sync****####****/
DECLARE db_cursor CURSOR FOR
select ID from AHEI.dbo.job where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.job) 
and ID in 
(
	--select job from AHEI.dbo.ticketD 
	--where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.ticketD) 
	--and elev in (select ID from #elev)
	--union
	select job from #elev
)

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @job

WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRANSACTION  

if not exists (select 1 from AHES.dbo.JobType where PrimarySyncID = (select top 1 Type from AHEI.dbo.Job where ID=@job))
BEGIN
Insert into AHES.dbo.JobType
(Type,Remarks,ISDefault,PrimarySyncID)
select 
Type,Remarks,ISDefault,ID
from AHEI.dbo.JobType where  ID = (select top 1 Type from AHEI.dbo.Job where ID=@job)
END
    
    
INSERT INTO AHES.dbo.Job
(Loc,
Owner,
fDate,
Status,
Remarks,
fDesc,
Type,
CType,
PO,
SO,
Certified,
Rev,Mat,Labor,Cost,Profit,Ratio,Reg,OT,DT,TT,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,BillRate,NT,Amount,
Template,
Custom21,
Custom22,
Custom23,
Custom24,
Custom25,
ProjCreationDate, 
Rol		,
PrimarySyncID	,
LastUpdateDate	,
TaskCategory					
)
select 
(select top 1 loc from AHES.dbo.loc where PrimarySyncID = j.Loc),
(select top 1 ID from AHES.dbo.Owner where PrimarySyncID = j.Owner),
fDate,
Status,
convert(varchar(max),Remarks) + ' Synced Job:' + CONVERT(varchar(15), ID),
fDesc,
(select top 1 ID from AHES.dbo.JobType where PrimarySyncID = j.Type),
CType,
PO,
SO,
Certified,
0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,
(select case when exists (SELECT Top 1 ID as ID  FROM AHES.dbo.JobT 
where fDesc=(SELECT TOP 1 fDesc  FROM AHEI.dbo.JobT where id=Template)) 
then (SELECT Top 1 ID as ID  FROM AHES.dbo.JobT 
where fDesc=(SELECT TOP 1 fDesc  FROM AHEI.dbo.JobT where id=Template)) else 0 end),
Custom21,
Custom22,
Custom23,
Custom24,
Custom25,
ProjCreationDate, 
0,ID,
GETDATE(),
(select Top 1 Category from AHES.dbo.Diagnostic where Category=j.TaskCategory)
from AHEI.dbo.Job j where ID = @job


----------------------Insert Documents-----------------------------
INSERT INTO AHES.dbo.Documents(Screen,ScreenID,Filename,Path,Date,fDesc,Type)
select Screen,(select j.ID from AHES.dbo.Job j where j.PrimarySyncID =@job),Filename,Path,Date,fDesc,Type 
from AHEI.dbo.Documents where ScreenID=@job and Screen='project'
----------------------Insert Documents-----------------------------


INSERT INTO AHES.dbo.JobTItem
(
JobT,
Job,
Type,
fDesc,
Code,
Actual,
Budget,
Line,
[Percent],
Comm,
Modifier,
ETC,
ETCMod,
Labor, 
Stored
)
Select 
0,
(select top 1 ID from AHES.dbo.Job where PrimarySyncID = jt.Job),
Type,
fDesc,
Code,
Actual,
Budget,
Line,
[Percent],
Comm,
Modifier,
ETC,
ETCMod,
Labor, 
Stored
from AHEI.dbo.JobTItem jt


--------Custom fields transferring over from AHEI to AHES. 

if not Exists(select 1 from AHES.[dbo].[tblCustomJob] where JobID=(select ID Type from AHES.dbo.Job where PrimarySyncID=@job))
Begin

if((select Template Type from AHES.dbo.Job where PrimarySyncID=@job) > 0)
  Begin
  ------- 
  Print('Job ID:-') Print(@job)
  INSERT INTO AHES.[dbo].[tblCustomJob]
			   ([JobID]
			   ,[tblCustomFieldsID]
			    ) 
  SELECT (select ID Type from AHES.dbo.Job where PrimarySyncID=@job),t.tblCustomFieldsID 
  FROM  AHES.[dbo].[tblCustomJobT] t  
  where JobTID=(select Template Type from AHES.dbo.Job where PrimarySyncID=@job)
  ---- Custom fields transferring over from AHEI to AHES. 
  Print('Custom fields transferring over from AHEI to AHES.')

  Update AHES_tblCustomJob  set AHES_tblCustomJob.Value=AHEI_tblCustomJob.Value 
 FROM AHEI.dbo.job AHEI_Job
 INNER JOIN  AHES.dbo.job AHES_Job 
 ON AHEI_Job.ID=AHES_Job.PrimarySyncID
 INNER JOIN AHEI.dbo.tblCustomJob AHEI_tblCustomJob 
 ON AHEI_tblCustomJob.JobID=AHEI_Job.ID 
 INNER JOIN AHES.dbo.tblCustomJob AHES_tblCustomJob 
 ON AHES_tblCustomJob.JobID=AHES_Job.ID
 INNER JOIN AHEI.dbo.[tblCustomFields] AHEI_tblCustomFields 
 ON AHEI_tblCustomFields.ID=AHEI_tblCustomJob.tblCustomFieldsID
 INNER JOIN AHES.dbo.[tblCustomFields] AHES_tblCustomFields 
 ON AHES_tblCustomFields.ID=AHES_tblCustomJob.tblCustomFieldsID
 where AHEI_tblCustomFields.Label=AHES_tblCustomFields.Label 
 --and AHEI_tblCustomFields.tblTabID  in (1,4,10)  and AHES_tblCustomFields.tblTabID  in (1,4,10)
 and AHEI_Job.ID=@job
 and len(isnull(AHEI_tblCustomJob.Value,'')) >  0
 and len(isnull(AHES_tblCustomJob.Value,''))  = 0

  -----------------------
  END
END			  

IF @@ERROR <> 0
AND @@TRANCOUNT > 0
BEGIN
RAISERROR ('Error Occured',16,1)
ROLLBACK TRANSACTION
CLOSE db_cursor
DEALLOCATE db_cursor
RETURN
END


COMMIT TRANSACTION
FETCH NEXT FROM db_cursor INTO @job
END

CLOSE db_cursor
DEALLOCATE db_cursor

END
/********End Job Sync***********/



DECLARE @Ticket int
BEGIN/********Ticket Sync********/
declare @countticket int  = 0
select @countticket = count( ID ) from AHEI.dbo.ticketD where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.ticketD) 
AND Cast (edate AS DATE) > Cast ('9/30/2016' AS DATE)
--year(edate)>2016 and month(edate)>9 
and 
( 
job in (select job from #elev)
)

if (@countticket > 0)
begin

DECLARE db_cursor CURSOR FOR
select ID from AHEI.dbo.ticketD where ID not in (select  isnull(PrimarySyncID,0) from AHES.dbo.ticketD)
AND Cast (edate AS DATE) > Cast ('9/30/2016' AS DATE)
--year(edate)>2016 and month(edate)>9 
and
(
--elev in (select ID from #elev)
--or 
job in (select job from #elev)
)
--SELECT Owner from Job where Status = 3 
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @Ticket

WHILE @@FETCH_STATUS = 0
BEGIN
--BEGIN TRANSACTION  
    
if not exists (select 1 from AHES.dbo.Category where Type = (select Cat from AHEI.dbo.TicketD where ID=@Ticket))
BEGIN
Insert into AHES.dbo.Category
(Type,Remarks,Icon,chargeable,ISDefault)
select 
Type,Remarks,Icon,chargeable,ISDefault
from AHEI.dbo.Category where  Type = (select Cat from AHEI.dbo.TicketD where ID =@Ticket)
END
    
        
if not exists (select 1 from AHES.dbo.JobType where PrimarySyncID = (select Type from AHEI.dbo.TicketD where ID=@Ticket))
BEGIN
Insert into AHES.dbo.JobType
(Type,Remarks,ISDefault,PrimarySyncID)
select 
Type,Remarks,ISDefault,ID
from AHEI.dbo.JobType where  ID = (select Type from AHEI.dbo.TicketD where ID=@Ticket)
END
    
declare @Secondaryticket int 
set @Secondaryticket = (select Max([NewID]) + 1
    FROM   (SELECT Isnull(Max(ID), 0) AS [NewID]
            FROM   AHES.dbo.TicketO
            UNION ALL
            SELECT Isnull(Max(ID), 0) AS [NewID]
            FROM   AHES.dbo.TicketD) A)
Print('Ticket Transferring over from AHEI to AHES.')

Print('Ticket ID:-') Print(@Ticket)
INSERT INTO AHES.dbo.TicketD
(ID,
CDate,
EDate,
TimeRoute,
TimeSite,
TimeComp,
Cat,
fDesc,
Est,
fWork,
Loc,
DescRes,
Reg,
OT,
NT,
TT,
DT,
Total,
Charge,
ClearCheck,
Who,
Type,
Status,
Elev,
BRemarks,
Level,
Custom1,
Custom2,
Custom3,
Custom4,
Custom5,
Custom6,
Custom7,
WorkOrder,
WorkComplete,
OtherE,
Toll,
Zone,
SMile,
EMile,
Internet,
--Invoice
ManualInvoice,
lastupdatedate,
TransferTime,
QBServiceItem,
QBPayrollItem,
CPhone,
CustomTick1 ,
CustomTick2 ,
CustomTick3 ,
CustomTick4 ,
CustomTick5,
Job,
JobCode,
Phase,
WageC,
fBy	,
JobItemDesc	,
PrimarySyncID												
)
select 
@Secondaryticket,
CDate,
EDate,
TimeRoute,
TimeSite,
TimeComp,
Cat,
convert(varchar(max),fdesc) + ' Synced Ticket:' +CONVERT(varchar(15), ID),
Est,
--(case when fWork not in (select ID from AHES.dbo.tblWork ) then (select min(ID) from AHES.dbo.tblWork) else fwork end ),
isnull((select top 1 ID from AHES.dbo.tblWork where fDesc=(select top 1 fDesc from AHEI.dbo.tblWork where ID = fWork)),fWork),
(select top 1 loc from AHES.dbo.loc where PrimarySyncID = d.Loc),
convert(varchar(max),DescRes) + ' Assigned:' + (select fDesc from AHEI.dbo.tblWork where ID = fWork),
0,--Reg,
0,--OT,
0,--NT,
0,--TT,
0,--DT,
0,--Total,
Charge,
ClearCheck,
Who,
(select top 1 ID from AHES.dbo.JobType where PrimarySyncID = d.Type),
Status,
(select top 1  ID from AHES.dbo.Elev where PrimarySyncID = d.Elev),
BRemarks,
Level,
Custom1,
Custom2,
Custom3,
Custom4,
Custom5,
Custom6,
Custom7,
WorkOrder,
WorkComplete,
OtherE,
Toll,
Zone,
SMile,
EMile,
Internet,
--Invoice
ManualInvoice,
GETDATE(),
TransferTime,
QBServiceItem,/**/
QBPayrollItem,/**/
CPhone,
CustomTick1 ,
CustomTick2 ,
CustomTick3 ,
CustomTick4 ,
CustomTick5,
(select top 1 ID from AHES.dbo.Job where PrimarySyncID = d.Job),
JobCode,
Phase,
WageC,
fBy	,
JobItemDesc,
ID
from AHEI.dbo.TicketD D where ID =@Ticket 

----------------------Insert Documents-----------------------------
INSERT INTO AHES.dbo.Documents(Screen,ScreenID,Filename,Path,Date,fDesc,Type)
select Screen,(select ID from AHES.dbo.TicketD D where PrimarySyncID =@Ticket),Filename,Path,Date,fDesc,Type 
from AHEI.dbo.Documents where ScreenID=@Ticket and Screen='Ticket'
----------------------Insert Documents-----------------------------


insert into AHES.dbo.RepDetail
(
[EquipTItem]
,[fwork]
,[Elev]
,[ticketID]
,[Code]
,[Lastdate]
,[NextDateDue]
,[OrigLastdate]
,[OrigNextDateDue]
,[comment]
,[status]
)
select 
[EquipTItem]
,(case when fwork not in (select ID from AHES.dbo.tblWork ) then (select min(ID) from AHES.dbo.tblWork) else fwork end )
,(select top 1 id from AHES.dbo.Elev where PrimarySyncID = r.Elev)
,@Secondaryticket
,[Code]
,[Lastdate]
,[NextDateDue]
,[OrigLastdate]
,[OrigNextDateDue]
,[comment]
,[status]
from AHEI.dbo.RepDetail r where ticketID = @Ticket

IF @@ERROR <> 0
AND @@TRANCOUNT > 0
BEGIN
RAISERROR ('Error Occured',16,1)
--ROLLBACK TRANSACTION
CLOSE db_cursor
DEALLOCATE db_cursor
RETURN
END


--COMMIT TRANSACTION
FETCH NEXT FROM db_cursor INTO @Ticket
END

CLOSE db_cursor
DEALLOCATE db_cursor

END

END
/********End Ticket Sync***********/


DROP TABLE #elev