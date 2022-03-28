
CREATE PROCEDURE [dbo].[spUpdateDefaultData] @DbName NVARCHAR(50)
AS

-------------------
------------------- MOM Generic Script to Update  Default Data
-------------------

IF( @DbName = 'MSM' )
BEGIN 
 
 
 

--- Set Default Permission Yes of Ticket, Job, Location, Elevator when it's NULL

UPDATE tbluser
SET    PO = Isnull(PO, 'NNNNNN'),		         Invoice = Isnull(Invoice, 'NNNNNN'),		         JOB = Isnull(JOB, 'NNNNNN'),                 OWNER = Isnull(OWNER, 'NNNNNN'),                 LOCATION = Isnull(LOCATION, 'NNNNNN'),
ELEVATOR = Isnull(ELEVATOR, 'NNNNNN'),                 Dispatch = Isnull(Dispatch, 'NNNNNN'), --  use "Dispatch" Field For Ticket Permission by Ref "TS"
Bill = Isnull(Bill, 'NNNNNN'),				   BillPay = Isnull(BillPay, 'NNNNNN'),
PaymentHistoryPermission = Isnull(PaymentHistoryPermission, 'NNNN'),				 Chart = Isnull(Chart, 'NNNNNN'),					  GLAdj = Isnull(GLAdj, 'NNNNNN'),					   Collection = Isnull(Collection, 'NNNNNN'),					  BankRec = Isnull(BankRec, 'NNNNNN')
,Ticket = Isnull(Ticket, 'NNNNNN'),MapR = Isnull(MapR, 'NNNNNN'),RouteBuilder = Isnull(RouteBuilder, 'NNNNNN'),ETimesheet = Isnull(ETimesheet, 'NNNNNN'),MTimesheet = Isnull(MTimesheet, 'NNNNNN')
WHERE  ( JOB IS NULL	 OR  OWNER IS NULL  OR  LOCATION  IS NULL  OR ELEVATOR IS NULL OR Dispatch IS  NULL  OR PO IS  NULL  OR Invoice IS  NULL		           OR Bill IS  NULL OR BillPay IS  NULL OR PaymentHistoryPermission IS  NULL OR Chart IS  NULL OR GLAdj IS  NULL OR Collection IS  NULL OR BankRec IS  NULL 
 OR Ticket IS  NULL OR MapR IS  NULL OR RouteBuilder IS  NULL OR ETimesheet IS  NULL OR MTimesheet IS  NULL)
         
-- This script was created for copy all the permission of users on PO page to RPO 
-- It should be run after tblUser.RPO column was created to make sure if a user had permission on PO, he/she also had permission on RPO
UPDATE tblUser SET RPO = PO WHERE RPO is NULL AND PO is not NULL
		  
		  
--Set Default Permission  of FinancePermission, BOMPermission, MilestonesPermission    when it's NULL
UPDATE tblUser    SET    FinancePermission = Substring(Job, 4, 1)          WHERE  FinancePermission IS NULL

UPDATE tblUser    SET    BOMPermission = Substring(Job, 1, 4)          WHERE  BOMPermission IS NULL

UPDATE tblUser     SET    MilestonesPermission = Substring(Job, 1, 4)          WHERE  MilestonesPermission IS NULL 	 
		   
UPDATE tblUser   SET    DocumentPermission = Isnull(DocumentPermission, 'NNNN')           WHERE  DocumentPermission IS NULL 

UPDATE tblUser    SET    ContactPermission = Isnull(ContactPermission, 'NNNN')   WHERE  ContactPermission IS NULL 


--Assign   Permission  of Saftey Test Permission from TC to RCSafteyTest Column , when RCSafteyTest is NULL
UPDATE tblUser    SET    RCSafteyTest = TC    WHERE  RCSafteyTest IS NULL
--Assign   Permission  of Ticket List Permission from TC to Resovled permission Column , when Resolve is NULL
update tbluser set Resolve=Ticket where Resolve is null

--update tblUser set SchedulemodulePermission='Y' where  Ticket like '%Y%' and SchedulemodulePermission is null

update tblUser set AccountPayablemodulePermission='Y' where  Vendor like '%Y%' or Vendor like '%Y%' or BillPay like '%Y%' and AccountPayablemodulePermission is null

-- Assign Module Permission according page permission
update tblUser set CustomermodulePermission='Y' where 
 Owner like '%Y%' or Location like '%Y%' or elevator like '%Y%' or Apply like '%Y%' or Deposit like '%Y%' or Collection like '%Y%' 
 and CustomermodulePermission is null

 update tblUser set SchedulemodulePermission='Y' where 
 Ticket like '%Y%' or Dispatch like '%Y%' or Resolve like '%Y%'
 and FinancialmodulePermission is null

  update tblUser set FinancialmodulePermission='Y' where 
 Chart like '%Y%' or GLAdj like '%Y%' or bankrec like '%Y%'
 and FinancialmodulePermission is null

   update tblUser set PurchasingmodulePermission='Y' where 
 PO like '%Y%' or RPO like '%Y%' 
 and PurchasingmodulePermission is null

   update tblUser set RCmodulePermission='Y' where 
 PO like '%Y%' or RPO like '%Y%' 
 and RCmodulePermission is null

  update tblUser set SalesManager='Y' where 
 Sales like '%Y%' or ToDo=1 or ToDoC=1 or FU like '%Y%' or Estimates like '%Y%' or AwardEstimates like '%Y%' or Proposal  like '%Y%'
 and SalesManager is null

-----------------------------Remove Duplicate  Permissions---------------
---TicketList
       
IF EXISTS (SELECT 1   FROM   tblPages   WHERE  url = 'ticketlistview.aspx')
BEGIN
UPDATE u
SET    u.Dispatch = CASE Isnull(Substring(u.Dispatch, 4, 1), 'NNNNNN')           WHEN 'N' THEN Substring(u.Dispatch, 1, 3) + ( CASE pp.access              WHEN 1 THEN 'Y'                 ELSE 'N'                                                                                    END ) + Substring(u.Dispatch, 5, 2)                                      ELSE u.Dispatch                                    END
FROM   tbluser u INNER JOIN tblpagepermissions pp  ON u.id = pp.[User] INNER JOIN tblPages p  ON p.id = pp.page
WHERE  url = 'ticketlistview.aspx'

DELETE FROM tblPages                 WHERE  url in  ('ticketlistview.aspx','addticket.aspx')
END
---Equipments
        
IF EXISTS (SELECT 1 FROM   tblPages  WHERE  url = 'equipments.aspx')
BEGIN
UPDATE u
SET    u.Elevator = CASE Isnull(Substring(u.Elevator, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN Substring(u.Elevator, 1, 3) + ( CASE pp.access                                                                                      WHEN 1 THEN 'Y'                                                                                      ELSE 'N'                                                                                    END ) + Substring(u.Elevator, 5, 2)                                      ELSE u.Elevator                                    END
FROM   tbluser u INNER JOIN tblpagepermissions pp  ON u.id = pp.[User] INNER JOIN tblPages p ON p.id = pp.page
WHERE  url = 'equipments.aspx'

DELETE FROM tblPages
WHERE  url  in ('equipments.aspx','addequipment.aspx')
END
---Project
         
IF EXISTS (SELECT 1   FROM   tblPages WHERE  url = 'project.aspx')
BEGIN
UPDATE u
SET    u.Job = CASE Isnull(Substring(u.Job, 4, 1), 'NNNNNN')                                 WHEN 'N' THEN Substring(u.Job, 1, 3) + ( CASE pp.access                                                                            WHEN 1 THEN 'Y'                                                                            ELSE 'N'                                                                          END ) + Substring(u.Job, 5, 2)                                 ELSE u.Job                               END
FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                               ON p.id = pp.page
WHERE  url = 'project.aspx'

DELETE FROM tblPages           WHERE  url in  ('project.aspx','addproject.aspx')
END


---managepo.aspx
IF EXISTS (SELECT 1       FROM   tblPages     WHERE  url = 'managepo.aspx')
BEGIN
UPDATE u   SET    u.PO = CASE Substring(Isnull(u.PO, 'NNNNNN'), 4, 1)                                 WHEN 'N' THEN Substring(u.PO, 1, 3) + ( CASE pp.access   WHEN 1 THEN 'Y'   ELSE 'N'  END ) + Substring(u.PO, 5, 2)                                 ELSE u.PO                               END
FROM   tbluser u    INNER JOIN tblpagepermissions pp       ON u.id = pp.[User]     INNER JOIN tblPages p                  ON p.id = pp.page
WHERE  url = 'managepo.aspx'

DELETE FROM tblPages       WHERE  url in  ('managepo.aspx')
END

---addpo.aspx
IF EXISTS (SELECT 1                     FROM   tblPages                     WHERE  url = 'addpo.aspx')
BEGIN
UPDATE u                SET    u.PO = CASE Substring(Isnull(u.PO, 'NNNNNN'), 1, 1)                                 WHEN 'N' THEN ( CASE pp.access   WHEN 1 THEN 'Y'   ELSE 'N'  END ) + Substring(u.PO, 2, 5)                                 ELSE u.PO                               END
FROM   tbluser u     
INNER JOIN tblpagepermissions pp       ON u.id = pp.[User]
INNER JOIN tblPages p      ON p.id = pp.page                WHERE  url = 'addpo.aspx'

DELETE FROM tblPages     WHERE  url in  ('addpo.aspx')
END
		
---managebills.aspx
IF EXISTS (SELECT 1                     FROM   tblPages                     WHERE  url = 'managebills.aspx')
BEGIN
UPDATE u
SET    u.Bill = CASE Isnull(Substring(u.Bill, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.Bill, 5, 2)                                      ELSE u.Bill                                    END

FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                               ON p.id = pp.page
WHERE  url = 'managebills.aspx'

DELETE FROM tblPages                WHERE  url in  ('managebills.aspx','addbills.aspx')
END

---writechecks.aspx
IF EXISTS (SELECT 1                     FROM   tblPages                     WHERE  url = 'writechecks.aspx')
BEGIN
UPDATE u
SET    u.BillPay = CASE Isnull(Substring(u.BillPay, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.BillPay, 5, 2)                                      ELSE u.BillPay                                    END
FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                               ON p.id = pp.page
WHERE  url = 'writechecks.aspx'

DELETE FROM tblPages                WHERE  url in  ('writechecks.aspx')
END
					 
---paymenthistory.aspx
IF EXISTS (SELECT 1                     FROM   tblPages                     WHERE  url = 'paymenthistory.aspx')
BEGIN
UPDATE u
SET    u.PaymentHistoryPermission = CASE WHEN  u.PaymentHistoryPermission IS NOT  NULL AND  PP.Access=1 THEN 'YYYY' 
			WHEN  u.PaymentHistoryPermission IS NULL AND  PP.Access=1 THEN 'YYYY' ELSE  ISNULL (u.PaymentHistoryPermission,'NNNN')

END 									  
                                      
FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                               ON p.id = pp.page
WHERE  url = 'paymenthistory.aspx'

DELETE FROM tblPages                WHERE  url in  ('paymenthistory.aspx')
END

---receivepayment.aspx
IF EXISTS (SELECT 1         FROM   tblPages                     WHERE  url = 'receivepayment.aspx')
BEGIN
UPDATE u
SET    u.apply = CASE Isnull(Substring(u.apply, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.apply, 5, 2)                                      ELSE u.apply                                    END

FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                               ON p.id = pp.page
WHERE  url = 'receivepayment.aspx'

DELETE FROM tblPages                WHERE  url in  ('receivepayment.aspx','addreceivepayment.aspx')
END

---managedeposit.aspx
IF EXISTS (SELECT 1         FROM   tblPages                     WHERE  url = 'managedeposit.aspx')
BEGIN
UPDATE u
SET    u.Deposit = CASE Isnull(Substring(u.Deposit, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.Deposit, 5, 2)                                      ELSE u.Deposit                                    END

FROM   tbluser u
INNER JOIN tblpagepermissions pp                               ON u.id = pp.[User]
INNER JOIN tblPages p                    ON p.id = pp.page
WHERE  url = 'managedeposit.aspx'

DELETE FROM tblPages                WHERE  url in  ('managedeposit.aspx','adddeposit.aspx')
END

---chartofaccount.aspx
IF EXISTS (SELECT 1                     FROM   tblPages                     WHERE  url = 'chartofaccount.aspx')
BEGIN
UPDATE u              SET    u.Chart = CASE Isnull(Substring(u.Chart, 4, 1), 'NNNNNN')       WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'  END ) + Substring(u.Chart, 5, 2)           ELSE u.Chart                             END

FROM   tbluser u
INNER JOIN tblpagepermissions pp     ON u.id = pp.[User]
INNER JOIN tblPages p             ON p.id = pp.page
WHERE  url = 'chartofaccount.aspx'

DELETE FROM tblPages     WHERE  url in  ('chartofaccount.aspx','addcoa.aspx')
END

---journalentry.aspx
IF EXISTS (SELECT 1         FROM   tblPages                     WHERE  url = 'journalentry.aspx')
BEGIN
UPDATE u
SET    u.GLAdj = CASE Isnull(Substring(u.GLAdj, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.GLAdj, 5, 2)                                      ELSE u.GLAdj                                    END

FROM   tbluser u
INNER JOIN tblpagepermissions pp      ON u.id = pp.[User]
INNER JOIN tblPages p     ON p.id = pp.page
WHERE  url = 'journalentry.aspx'

DELETE FROM tblPages     WHERE  url in  ('journalentry.aspx','addjournalentry.aspx')
END

---Collections.aspx
IF EXISTS (SELECT 1     FROM   tblPages      WHERE  url = 'Collections.aspx')
BEGIN
UPDATE u   SET    u.Collection = CASE Isnull(Substring(u.Collection, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.Collection, 5, 2)                                      ELSE u.Collection                                    END

FROM   tbluser u   
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p   ON p.id = pp.page
WHERE  url = 'Collections.aspx'

DELETE FROM tblPages     WHERE  url in  ('Collections.aspx')
END

---bankrecon.aspx
IF EXISTS (SELECT 1      FROM   tblPages    WHERE  url = 'bankrecon.aspx')
BEGIN
UPDATE u       SET    u.BankRec = CASE Isnull(Substring(u.BankRec, 4, 1), 'NNNNNN')                                      WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'                                                                                      ELSE 'NNNN'                                                                                    END ) + Substring(u.BankRec, 5, 2)                                      ELSE u.BankRec                                    END

FROM   tbluser u
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p        ON p.id = pp.page
WHERE  url = 'bankrecon.aspx'

DELETE FROM tblPages   WHERE  url in  ('bankrecon.aspx')
END
			 
---Scheduler.aspx
IF EXISTS (SELECT 1      FROM   tblPages    WHERE  url = 'Scheduler.aspx')
BEGIN
UPDATE u       SET    
  u.Dispatch = CASE Isnull(Substring(u.Dispatch, 4, 1), 'NNNNNN')       
  WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'      ELSE 'NNNN' END ) + Substring(u.Dispatch, 5, 2) ELSE u.Dispatch   END
 
FROM   tbluser u
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p        ON p.id = pp.page
WHERE  url = 'Scheduler.aspx'

DELETE FROM tblPages   WHERE  url in  ('Scheduler.aspx')
END		

---map.aspx
IF EXISTS (SELECT 1      FROM   tblPages    WHERE  url = 'map.aspx')
BEGIN
UPDATE u       SET    u.MapR = CASE Isnull(Substring(u.MapR, 4, 1), 'NNNNNN')       
  WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'      ELSE 'NNNN' END ) + Substring(u.MapR, 5, 2) ELSE u.MapR 
  END
FROM   tbluser u
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p        ON p.id = pp.page
WHERE  url = 'map.aspx'

DELETE FROM tblPages   WHERE  url in  ('map.aspx')
END		

---routebuilder.aspx
IF EXISTS (SELECT 1      FROM   tblPages    WHERE  url = 'routebuilder.aspx')
BEGIN
UPDATE u       SET    u.RouteBuilder = CASE Isnull(Substring(u.RouteBuilder, 4, 1), 'NNNNNN')       
  WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'      ELSE 'NNNN' END ) + Substring(u.RouteBuilder, 5, 2) ELSE u.RouteBuilder 
  END
FROM   tbluser u
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p        ON p.id = pp.page
WHERE  url = 'routebuilder.aspx'

DELETE FROM tblPages   WHERE  url in  ('routebuilder.aspx')
END
---etimesheet.aspx
IF EXISTS (SELECT 1      FROM   tblPages    WHERE  url = 'etimesheet.aspx')
BEGIN
UPDATE u       SET    u.ETimesheet = CASE Isnull(Substring(u.ETimesheet, 4, 1), 'NNNNNN')       
  WHEN 'N' THEN ( CASE pp.access     WHEN 1 THEN 'YYYY'      ELSE 'NNNN' END ) + Substring(u.ETimesheet, 5, 2) ELSE u.ETimesheet 
  END
FROM   tbluser u
INNER JOIN tblpagepermissions pp    ON u.id = pp.[User]
INNER JOIN tblPages p        ON p.id = pp.page
WHERE  url = 'etimesheet.aspx'

DELETE FROM tblPages   WHERE  url in  ('etimesheet.aspx')
END
------------- Users.aspx  --------------
IF EXISTS (SELECT 1   FROM   tblPages   WHERE  url = 'Users.aspx')
BEGIN
UPDATE u  SET   u.UserS =  'YYYYYY'    FROM   tbluser u
INNER JOIN tblpagepermissions pp   ON u.id = pp.[User]
INNER JOIN tblPages p   ON p.id = pp.page
WHERE  url = 'Users.aspx'  and pp.Access=1
DELETE FROM tblPages  WHERE  url in  ('Users.aspx')
END 

------------- adduser.aspx  --------------
IF EXISTS (SELECT 1   FROM   tblPages   WHERE  url = 'adduser.aspx')
BEGIN
UPDATE u  SET   u.UserS =  'YYYYYY'   FROM   tbluser u
INNER JOIN tblpagepermissions pp   ON u.id = pp.[User]
INNER JOIN tblPages p   ON p.id = pp.page
WHERE  url = 'adduser.aspx'  and pp.Access=1
DELETE FROM tblPages  WHERE  url in  ('adduser.aspx')
END 

------  JobClose Permission

update tblUser set JobClose='YYYYYY' where  Job like '%Y%'  and JobClose is null

update tblUser set JobClose='NNNNNN' where  JobClose is null

------ ProjectModulePermission

update tblUser set ProjectModulePermission='Y' where  ProjectModulePermission is null


------ InventoryModulePermission

update tblUser set InventoryModulePermission='Y' where InventoryModulePermission is null
 

UPDATE JobTItem set OrderNo=Line where OrderNo is null

------ JobCompletedPermission

update tblUser set JobCompletedPermission='Y' where  JobClose like '%Y%'  and JobCompletedPermission is null

update tblUser set JobCompletedPermission='N' where   JobCompletedPermission is null


------ JobReopenPermission

update tblUser set JobReopenPermission='Y' where  JobClose like '%Y%'  and JobReopenPermission is null

update tblUser set JobReopenPermission='N' where   JobReopenPermission is null

---------- When Projcreation is null , getting JobI table min(fdate ) and update -------
 UPDATE j  SET  j.ProjCreationDate = (select  MIN(ji.fdate) from JobI ji Where j.ID = ji.Job )
 FROM JOB j  , JobI ji  WHERE    j.ProjCreationDate IS NULL  AND j.ID = ji.job
 
 ---------- When CloseDate is null , getting JobI table max(fdate ) and update -------
  UPDATE j  SET  j.CloseDate = (select  Max(ji.fdate) from JobI ji Where j.ID = ji.Job )

 FROM JOB j  , JobI ji  WHERE    j.CloseDate IS NULL  AND j.ID = ji.job and j.Status=1


 ----------------#### Currect type , phase ,  wagec , gl , glrev , post , fint for Job Costing ###------

 
 ----- 1 Materials
--INSERT INTO [dbo].[JobTItem] ([JobT]  ,[Job]  ,[Type]  ,[fDesc]    ,[Code]  ,[Actual]   ,[Budget]    ,[Line]    ,[Percent]    ,[Comm]      ,[Stored]    ,[Modifier]   ,[ETC]    ,[ETCMod]           ,[THours]           ,[FC]           ,[Labor]           ,[BHours]           ,[GL]           ,[OrderNo]           ,[GroupID])
      
--select 
--j.Template [JobT]           , j.ID [Job]       ,1 [Type]           ,'Materials' [fDesc],'100'[Code]           ,0 [Actual]           ,0 [Budget]           ,1 [Line]
--,0[Percent]           ,0 [Comm]           ,0 [Stored]           ,0 [Modifier]           ,0 [ETC]           ,0 [ETCMod]           ,0 [THours]           ,0 [FC]           ,0 [Labor]      
--,0 [BHours]           ,0 [GL]           ,1 [OrderNo]           ,0 [GroupID]
		    
--from job j

--where j.id not in (select job from JobTItem)

--INSERT INTO [dbo].[JobTItem] ([JobT]  ,[Job]  ,[Type]     ,[fDesc]    ,[Code]    ,[Actual]    ,[Budget]     ,[Line]    ,[Percent]    ,[Comm]     ,[Stored]   ,[Modifier]    ,[ETC]   ,[ETCMod]    ,[THours]   ,[FC]     ,[Labor]    ,[BHours]   ,[GL]    ,[OrderNo]   ,[GroupID])
      
--select 
--j.Template [JobT]           , j.ID [Job]       ,1 [Type]           ,'Materials' [fDesc],'100'[Code]           ,0 [Actual]           ,0 [Budget]          
-- , ((select isNULL(max(line),0) + 1  from JobTItem where job=j.ID)) [Line]
--,0[Percent]           ,0 [Comm]           ,0 [Stored]           ,0 [Modifier]           ,0 [ETC]           ,0 [ETCMod]           ,0 [THours]           ,0 [FC]           ,0 [Labor]      
--,0 [BHours]           ,0 [GL]           ,((select isNULL(max(line),0) + 1  from JobTItem where job=j.ID)) [OrderNo]           ,0 [GroupID]
		    
--from job j

--where j.id  in (select  job from JobTItem where fDesc <> 'Materials' and type=1)

--and j.id  not in (select  job from JobTItem where fDesc = 'Materials' and type=1)


--INSERT INTO BOM(JobTItemID)

--select   ID from JobTItem where Job is not null and type=1 and isnull(id,0) not in (select isnull(JobTItemID,0) from BOM where JobTItemID is not null )

--update  JobTItem set type = 1 where type not in (0,1)

--update  bom set type=(select top 1 ID from bomt where type='Materials')  where type is null


----2 Labor
--INSERT INTO [dbo].[JobTItem] ([JobT]  ,[Job]  ,[Type]     ,[fDesc]    ,[Code]    ,[Actual]    ,[Budget]     ,[Line]    ,[Percent]    ,[Comm]     ,[Stored]   ,[Modifier]    ,[ETC]   ,[ETCMod]    ,[THours]   ,[FC]     ,[Labor]    ,[BHours]   ,[GL]    ,[OrderNo]   ,[GroupID])
      
--select 
--j.Template [JobT]           , j.ID [Job]       ,1 [Type]           ,'Labor' [fDesc],'100'[Code]           ,0 [Actual]           ,0 [Budget]          
-- , ((select isNULL(max(line),0) + 1  from JobTItem where job=j.ID)) [Line]
--,0[Percent]           ,0 [Comm]           ,0 [Stored]           ,0 [Modifier]           ,0 [ETC]           ,0 [ETCMod]           ,0 [THours]           ,0 [FC]           ,0 [Labor]      
--,0 [BHours]           ,0 [GL]           ,((select isNULL(max(line),0) + 1  from JobTItem where job=j.ID)) [OrderNo]           ,0 [GroupID]
		    
--from job j

--where j.id  in (select  job from JobTItem where fDesc <> 'Labor' and type=1)

--and j.id  not in (select  job from JobTItem where fDesc = 'Labor' and type=1)


--INSERT INTO BOM(JobTItemID,type)

--select   ID , ((select top 1 ID from bomt where type='Labor')) from JobTItem where Job is not null and  fDesc = 'Labor' and type=1 and isnull(id,0) not in (select isnull(JobTItemID,0) from BOM where JobTItemID is not null )


------ type = 0 rev , 1= cost 


update  jobi set type=1 where type not in (0,1)

--- if  phase is null

update  jobi set phase =(select top 1 line from JobTItem where fDesc='Labor' and JobTItem.Job=jobi.Job  and  type=1) where  phase is null and isnull(Labor,0)=1

update  jobi set phase =(select top 1 line from JobTItem where fDesc<>'Labor' and JobTItem.Job=jobi.Job  and  type=1) where  phase is null

update  jobi set phase =(select top 1 line from JobTItem where JobTItem.Job=jobi.Job  and  type=1) where  phase is null


--- if type labor in  AP BILL screen
update jobi set jobi.Phase=(select top 1 line from JobTItem 
inner join bom on bom.JobTItemID=JobTItem.ID
inner join BOMT on bomt.ID =bom.Type  AND ( bomt.Type <> 'Labor'   ) 
where   JobTItem.Job=jobi.Job and JobTItem.type=1)
FROM Jobi 
inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.type=1 and JobTItem.Job=jobi.Job  
inner join bom on bom.JobTItemID=JobTItem.ID
inner join BOMT on bomt.ID =bom.Type  AND ( bomt.Type = 'Labor'  )
WHERE jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
AND isnull(jobi.Type,0) <> 0   
AND (isnull(jobi.Labor,0) = 0)
AND (isnull(jobi.Labor,0) = 0)

-- if phase not exists in JobTItem

update jobi set phase =isnull((select top 1 line from JobTItem where JobTItem.job=jobi.job and type=1 and fDesc<>'Labor' ),jobi.Phase)
from jobi where type= 1 and isnull(job,0) <> 0    
and  phase not in (select line from JobTItem where JobTItem.job=jobi.job and type=1 ) 

-- if WageC not set up in project screen

	update j set  
			j.WageC = case isnull(j.WageC,0) when 0 then   jt.Wage else j.WageC end,
	        j.GL = case isnull(j.GL,0) when 0 then   jt.InvExp else j.GL end,
	        j.GLRev = case isnull(j.GLRev,0) when 0 then   jt.InvServ else j.GLRev end,
	        j.post = case isnull(j.post,0) when 0 then   jt.Post else j.post end,
			j.fint = case isnull(j.fint,0) when 0 then   jt.fInt else j.fint end
	        FROM    job j  
	        INNER JOIN jobt jt ON jt.ID = j.Template	
	        WHERE	 (  
			    isnull(j.WageC,0) = 0 
	        or  isnull(j.gl,0) = 0 
			or  isnull(j.GLRev,0) = 0 
		    or  isnull(j.Post,0) = 0 
			or  isnull(j.fInt,0) = 0 
				 )


--- if  phase is null 

update  d set d.Phase=(select top 1 line from JobTItem where fDesc='Labor' and JobTItem.Job=d.Job  and  type=1) 
from ticketd d where d.phase is null and isnull(d.job,0) <>0
and d.id in ( select  top 100000 id from ticketd where Phase is null and isnull(job,0) <>0)

--- if  JobCode is null  
update   d set d.JobCode=(select top 1 Code from JobTItem where Line =d.Phase and JobTItem.Job=d.Job  and  type=1) 
from ticketd d where d.JobCode is null and d.phase is not null and isnull(d.job,0) <>0
and d.id in ( select  top 100000 id from ticketd where JobCode is null and isnull(job,0) <>0)

--- if  JobItemDesc is null  
update   d set d.JobItemDesc=(select top 1 fDesc from JobTItem where Line =d.Phase and JobTItem.Job=d.Job  and  type=1) 
from ticketd d where d.JobCode is not null and d.phase is not null and JobItemDesc is null and isnull(d.job,0) <>0
and d.id in ( select  top 100000 id from ticketd where JobItemDesc is null and isnull(job,0) <>0)




update Control set TargetHPermission=0  where   TargetHPermission is null

update JobT set TargetHPermission=0  where   TargetHPermission is null

update Job set TargetHPermission=0  where   TargetHPermission is null

update jobtitem set TargetHours=0  where   TargetHours is null

update jobtitem set GroupId=0  where   GroupId is null

update JobT  set Wage=(SELECT  top 1 ID  FROM   PRWage  where fDesc='Mobile Service Manager'  ) WHERE   isnull(Wage,0)=0 

 

update t set t.VDoub=ji.Phase from JobI ji 
   inner join trans t on t.ID=ji.TransID and t.VInt=ji.Job  
where  t.Type=41  and  t.VDoub<>ji.Phase and t.VInt is not null


update c set c.BLenght=999
 from Contract c inner join Job j on j.ID=c.Job
 where  isnull(c.Expiration,0) = 0 and isnull(BLenght,0) =0

 update c set c.BLenght=
                CASE isnull(c.BCycle,0)  
			    WHEN 0 THEN   1	          WHEN 1 THEN 1
	            WHEN 2 THEN   3	          WHEN 3 THEN 4
	            WHEN 4 THEN   6           WHEN 5 THEN 12
	            WHEN 6 THEN   999	      WHEN 7 THEN 36
	            WHEN 8 THEN   60	      WHEN 9 THEN 24
				END
from Contract c inner join Job j on j.ID=c.Job
where  isnull(c.Expiration,0) <> 0 and isnull(BLenght,0) = 0

update c set c.EscLast=   isnull(c.BStart,isnull(c.SStart,j.fDate)) 
from Contract c inner join Job j on j.ID=c.Job
where  isnull(c.Expiration,0) <> 0 and c.EscLast is null and isnull(c.BCycle,0) <> 6

update c set c.ExpirationDate =  isnull(c.BStart,isnull(c.SStart,j.fDate)) 
from Contract c inner join Job j on j.ID=c.Job
where  isnull(c.Expiration,0) <> 0 and c.ExpirationDate is null and isnull(c.BCycle,0) <> 6

update c set c.BStart =  isnull(c.BStart,isnull(c.SStart,j.fDate)) 
from Contract c inner join Job j on j.ID=c.Job
where  isnull(c.Expiration,0) <> 0 and c.BStart is null and isnull(c.BCycle,0) <> 6

update Contract set EscLast=BStart  where EscLast is null


---#########################

END

ELSE IF( @DbName = 'TS' )

BEGIN

------------Company Default Data for TS database----------

DELETE from tblUserCo 

INSERT INTO tblUserCo (UserID,CompanyID,OfficeID,IsSel)

SELECT U.ID AS UserID,b.ID as CompanyID, 0 as OfficeID,1 as IsSel 

FROM Branch b inner join tblUser u on u.EN=b.ID and u.en <> 0

where u.ID not in (SELECT UserID FROM tblUserCo)


INSERT INTO tblUserCo (UserID,CompanyID,OfficeID,IsSel)

SELECT U.ID AS UserID,b.ID as CompanyID, 0 as OfficeID,1 as IsSel 

FROM Branch b left join tblUser u on   u.EN = 0


if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Inside Car')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Inside Car')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Outside Hoistway')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Outside Hoistway')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Top of Car')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Top of Car')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Machine Room')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Machine Room')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Pit')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Pit')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'Escalator/Moving Walk')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('Escalator/Moving Walk')
end
if NOT EXISTS (SELECT * FROM ViolationSection WHERE [Name] = 'All types')
begin
	INSERT INTO [dbo].[ViolationSection]([Name])VALUES('All types')
end

if NOT EXISTS (SELECT * FROM ViolationCategory WHERE [Name] = 'Elevator Part')
begin
	INSERT INTO [dbo].ViolationCategory([Name])VALUES('Elevator Part')
end
if NOT EXISTS (SELECT * FROM ViolationCategory WHERE [Name] = 'Violating Condition')
begin
	INSERT INTO [dbo].ViolationCategory([Name])VALUES('Violating Condition')
end
if NOT EXISTS (SELECT * FROM ViolationCategory WHERE [Name] = 'Suggested Remedy')
begin
	INSERT INTO [dbo].ViolationCategory([Name])VALUES('Suggested Remedy')
END

END