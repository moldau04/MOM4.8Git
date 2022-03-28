CREATE PROCEDURE [dbo].[spGetWageDedcuion]  
AS  
BEGIN  
  
 SELECT [ID] ,[fDesc] ,[Type],[ByW] ,[BasedOn] ,[AccruedOn] ,[Count] ,  
 CASE WHEN PRDed.Type = 0 THEN 'Federal' WHEN PRDed.Type =1 THEN 'State' WHEN PRDed.Type =2 THEN 'City' WHEN PRDed.Type =3 THEN 'Local' WHEN PRDed.Type =4 THEN 'Other' ELSE '' END AS TypeName,  
 CONVERT(NUMERIC(19,2),[EmpRate]) AS EmpRate ,CONVERT(NUMERIC(19,2),EmpTop) AS EmpTop ,[EmpGL] ,  
 CONVERT(NUMERIC(19,2),CompRate) AS CompRate ,CONVERT(NUMERIC(19,2),CompTop) AS CompTop,[CompGL] ,[CompGLE],[Paid] ,[Vendor] ,[Balance] ,[InUse]  
    ,[Remarks] ,[DedType] ,[Reimb] ,[Job] ,[Box] ,[Frequency] ,[Process],  
 (SELECT fDesc FROM Chart WHERE ID = PRDed.EmpGL) as EmpGLAcct,  
 (SELECT fDesc FROM Chart WHERE ID = PRDed.CompGL) as CompGLAcct,  
 (SELECT fDesc FROM Chart WHERE ID = PRDed.CompGLE) as CompGLEAcct,  
 (SELECT Name FROM Rol WHERE ID = (SELECT Rol FROM Vendor WHERE ID = Vendor)) as VendorName,  
 CASE WHEN PRDed.ByW = 0 THEN 'Company' WHEN PRDed.ByW =1 THEN 'Employee' WHEN PRDed.Type =2 THEN 'Both' ELSE '' END AS PaidBy,  
 CASE WHEN BasedOn = 0 THEN 'FIT Wages' WHEN BasedOn=1 THEN 'FICA Wages' WHEN BasedOn=2 THEN 'MEDI Wages'   
 WHEN BasedOn = 3 THEN 'FUTA Wages' WHEN BasedOn=4 THEN 'SIT Wages' WHEN BasedOn=5 THEN 'Vacation Wages'   
 WHEN BasedOn = 6 THEN 'Worker''s Comp Wages' WHEN BasedOn=7 THEN 'Union Wages' WHEN BasedOn=8 THEN 'Flat Amount'   
 ELSE '' END AS BasedOnDesc,  
 CASE WHEN AccruedOn = 0 THEN 'Number of Hours' WHEN AccruedOn=1 THEN 'Dollar Amount' WHEN AccruedOn=2 THEN 'Flat Amount' ELSE '' END AS AccruedOnDesc,  
 PRDed.ID as Ded  
 FROM PRDed ORDER BY PRDed.fDesc  
  
END