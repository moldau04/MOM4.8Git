CREATE PROCEDURE [dbo].[spGetEmployeeListbyID]    
@ID int    
AS     
 Begin    
     
 DECLARE @EmpGLAcct VARCHAR(MAX)    
 DECLARE @CompGLAcct VARCHAR(MAX)    
 DECLARE @CompGLEAcct VARCHAR(MAX)    
 DECLARE @VendorName VARCHAR(MAX)    
 DECLARE @PRTaxEName VARCHAR(MAX)    
    
    
 SELECT @EmpGLAcct = Acct+' '+fDesc FROM Chart WHERE ID = (SELECT ISNULL(EmpGL,0) FROM PRDed WHERE ID = @Id)    
 SELECT @CompGLAcct = Acct+' '+fDesc FROM Chart WHERE ID = (SELECT ISNULL(CompGL,0) FROM PRDed WHERE ID = @Id)    
 SELECT @CompGLEAcct = Acct+' '+fDesc FROM Chart WHERE ID = (SELECT ISNULL(CompGLE,0) FROM PRDed WHERE ID = @Id)    
 SELECT @VendorName = Name FROM Rol WHERE ID = (SELECT Rol FROM Vendor WHERE ID = (SELECT ISNULL(Vendor,0) FROM PRDed WHERE ID = @Id))    
 SELECT @PRTaxEName = Acct+' '+fDesc FROM Chart WHERE ID = (SELECT ISNULL(PRTaxE,0) FROM Emp WHERE ID = @Id)    
    
    
 SELECT     
 Emp.[ID] ,    
 Emp.[fFirst] ,    
 Emp.[Last] ,    
 Emp.[Middle] ,    
 Emp.[Name] ,    
 Emp.[Rol] ,    
 Emp.[SSN] ,    
 Emp.[Title] ,    
 Emp.[Sales] ,    
 Emp.[Field] ,    
 Emp.[Status] ,    
 Emp.[Pager] ,    
 Emp.[InUse] ,    
 Emp.[PayPeriod] ,    
 Emp.[DHired] ,    
 Emp.[DFired] ,    
 Emp.[DBirth] ,    
 Emp.[DReview] ,    
 Emp.[DLast] ,    
 Emp.[FStatus] ,    
 Emp.[FAllow] ,    
 Emp.[FAdd] ,    
 Emp.[SStatus] ,    
 Emp.[SAllow] ,    
 Emp.[SAdd] ,    
 Emp.[CallSign] ,    
 Emp.[VRate] ,    
 Emp.[VBase] ,    
 Emp.[VLast] ,    
 Emp.[VThis] ,    
 Emp.[Sick] ,    
 Emp.[PMethod] ,    
 Emp.[PFixed] ,    
 Emp.[PHour] ,    
 Emp.[LName] ,    
 Emp.[LStatus] ,    
 Emp.[LAllow] ,    
 Emp.[PRTaxE] ,    
 Emp.[State] ,    
 Emp.[Salary] ,    
 Emp.[SalaryF] ,    
 Emp.[SalaryGL] ,    
 Emp.[fWork] ,    
 Emp.[NPaid] ,    
 Emp.[Balance] ,    
 Emp.[PBRate] ,    
 Emp.[FITYTD] ,    
 Emp.[FICAYTD] ,    
 Emp.[MEDIYTD] ,    
 Emp.[FUTAYTD] ,    
 Emp.[SITYTD] ,    
 Emp.[LocalYTD] ,    
 Emp.[BonusYTD] ,    
 Emp.[HolH] ,    
 Emp.[HolYTD] ,    
 Emp.[VacH] ,    
 Emp.[VacYTD] ,    
 Emp.[ZoneH] ,    
 Emp.[ZoneYTD] ,    
 Emp.[ReimbYTD] ,    
 Emp.[MileH] ,    
 Emp.[MileYTD] ,    
 Emp.[Race] ,    
 Emp.[Sex] ,    
 Emp.[Ref] ,    
 Emp.[ACH] ,    
 Emp.[ACHType] ,    
 Emp.[ACHRoute] ,    
 Emp.[ACHBank] ,    
 Emp.[Anniversary] ,    
 Emp.[Level] ,    
 Emp.[WageCat] ,    
 Emp.[DSenior] ,    
 Emp.[PRWBR] ,    
 Emp.[PDASerialNumber] ,    
 Emp.[StatusChange] ,    
 Emp.[SCDate] ,    
 Emp.[SCReason] ,    
 Emp.[DemoChange] ,    
 Emp.[Language] ,    
 Emp.[TicketD] ,    
 Emp.[Custom1] ,    
 Emp.[Custom2] ,    
 Emp.[Custom3] ,    
 Emp.[Custom4] ,    
 Emp.[Custom5] ,    
 Emp.[DDType] ,    
 Emp.[DDRate] ,    
 Emp.[ACHType2] ,    
 Emp.[ACHRoute2] ,    
 Emp.[ACHBank2] ,    
 Emp.[BillRate] ,    
 Emp.[BMSales] ,    
 Emp.[BMInvAve] ,    
 Emp.[BMClosing] ,    
 Emp.[BMBillEff] ,    
 Emp.[BMProdEff] ,    
 Emp.[BMAveTask] ,    
 Emp.[BMCustom1] ,    
 Emp.[BMCustom2] ,    
 Emp.[BMCustom3] ,    
 Emp.[BMCustom4] ,    
 Emp.[BMCustom5] ,    
 Emp.[TaxCodeNR] ,    
 Emp.[TaxCodeR] ,    
 Emp.[TechnicianBio] ,    
 Emp.[DeviceID] ,    
 Emp.[MileageRate] ,    
 Emp.[PayPortalPassword] ,    
 Emp.[SickRate] ,    
 Emp.[SickAccrued] ,    
 Emp.[SickUsed] ,    
 Emp.[SickYTD] ,    
 Emp.[VacAccrued] ,    
 Emp.[MSDeviceId] ,    
 Emp.[SCounty] ,    
    
 Rol.City     
 ,Rol.Zip     
 ,Rol.Phone     
 ,Rol.Address     
 ,Rol.Email     
 ,Rol.Cellular     
 ,Rol.Remarks      
 ,Rol.Type     
 ,Rol.Contact     
 ,Rol.Website     
 ,Rol.Fax    
 ,Rol.Country  
 ,@PRTaxEName AS PRTaxEName  
    
 FROM Emp INNER JOIN Rol ON Emp.Rol = Rol.ID LEFT OUTER JOIN tblWork ON Emp.fWork = tblWork.ID      
 WHERE Emp.ID= @ID    
 ORDER BY Emp.Name    
 ---------------------- Wage Category -----------------    
 SELECT [Wage] ,[GL],[Reg],[OT],[DT],[TT],[InUse]    
           ,[YTD],[YTDH],[OYTD],[OYTDH],[DYTD],[DYTDH],[TYTD],[TYTDH],[NT],[NYTD],[NYTDH],[VacR],[CReg],[COT],[CDT],[CNT],[CTT],[Status],  
           CASE WHEN [Status] = 0 THEN 'Active' ELSE 'Inactive' END AS StatusName,  
     ISNULL([FIT],0)  AS [FIT],ISNULL([FICA],0)  AS [FICA],ISNULL([MEDI],0)  AS [MEDI],ISNULL([FUTA],0)  AS [FUTA],ISNULL([SIT],0)  AS [SIT],ISNULL([Vac],0)  AS [Vac],  
     ISNULL([WC],0)  AS [Wc],ISNULL([Uni],0) AS [Uni],ISNULL([Sick],0) AS Sick,  
     (SELECT fDesc FROM Chart WHERE ID = GL) as GLName,    
     (SELECT fDesc FROM PRWage WHERE ID = Wage) as fDesc    
     FROM PRWageItem WHERE Emp= @ID    
     ---------------------- Wage Category -----------------    
     ---------------------- Deduction ----------------    
  SELECT [Ded] ,[BasedOn],[AccruedOn],[ByW],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL],[CompGLE],[InUse],[YTD],[YTDC],    
  (SELECT fDesc FROM Chart WHERE ID = EmpGL) as EmpGLName,    
  (SELECT fDesc FROM Chart WHERE ID = CompGL) as CompGLName,    
  (SELECT fDesc FROM Chart WHERE ID = CompGLE) as CompGLEName,    
  (SELECT fDesc FROM PRDed WHERE ID = Ded) as fDesc    
  FROm PRDedItem WHERE Emp= @ID    
  ---------------------- Deduction ----------------    
  -------------------- Other --------------------    
 SELECT [Cat],[GL],[Rate],ISNULL([FIT],0)  AS [FIT],ISNULL([FICA],0)  AS [FICA],ISNULL([MEDI],0)  AS [MEDI],ISNULL([FUTA],0)  AS [FUTA],ISNULL([SIT],0)  AS [SIT],ISNULL([Vac],0)  AS [Vac],ISNULL([WC],0)  AS [Wc],ISNULL([Uni],0) AS [Uni],ISNULL([Sick],0) 
 AS Sick,    
 (SELECT fDesc FROM Chart WHERE ID = GL) as ExpAcctName,    
 CASE WHEN Cat =0 THEN 'Bonus'     
  WHEN Cat =1 THEN 'Holiday'    
  WHEN Cat = 2 THEN 'Vacation'    
  WHEN Cat = 3 THEN 'Zone'    
  WHEN Cat = 4 THEN 'Reimbursement'    
  WHEN Cat = 5 THEN 'Mileage'    
  WHEN Cat = 6 THEN 'Sick Leave'    
  ELSE '' END AS fDesc    
 FROM PROther WHERE Emp= @ID    
 -------------------- Other --------------------    
 End    
    