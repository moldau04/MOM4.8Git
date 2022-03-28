CREATE PROCEDURE spGetWageDedcuionByID  
@Id int  
AS  
BEGIN  
DECLARE @EmpGLAcct VARCHAR(MAX)  
DECLARE @CompGLAcct VARCHAR(MAX)  
DECLARE @CompGLEAcct VARCHAR(MAX)  
DECLARE @VendorName VARCHAR(MAX)  
  
  
SELECT @EmpGLAcct = fDesc FROM Chart WHERE ID = (SELECT ISNULL(EmpGL,0) FROM PRDed WHERE ID = @Id)  
SELECT @CompGLAcct = fDesc FROM Chart WHERE ID = (SELECT ISNULL(CompGL,0) FROM PRDed WHERE ID = @Id)  
SELECT @CompGLEAcct = fDesc FROM Chart WHERE ID = (SELECT ISNULL(CompGLE,0) FROM PRDed WHERE ID = @Id)  
SELECT @VendorName = Name FROM Rol WHERE ID = (SELECT Rol FROM Vendor WHERE ID = (SELECT ISNULL(Vendor,0) FROM PRDed WHERE ID = @Id))  
  
SELECT [ID] ,[fDesc] ,[Type],[ByW] ,[BasedOn] ,[AccruedOn] ,[Count] ,CONVERT(NUMERIC(19,2),[EmpRate]) AS EmpRate ,CONVERT(NUMERIC(19,2),EmpTop) AS EmpTop ,[EmpGL] ,  
CONVERT(NUMERIC(19,2),CompRate) AS CompRate ,CONVERT(NUMERIC(19,2),CompTop) AS CompTop,[CompGL] ,[CompGLE],[Paid] ,[Vendor] ,[Balance] ,[InUse]  
           ,[Remarks] ,[DedType] ,[Reimb] ,[Job] ,[Box] ,[Frequency] ,[Process],@EmpGLAcct as EmpGLAcct,@CompGLAcct AS CompGLAcct,@CompGLEAcct AS CompGLEAcct,  
       @VendorName AS VendorName,  
    CASE WHEN ByW = 0 THEN 'Company' WHEN ByW=1 THEN 'Employee' WHEN ByW=2 THEN 'Both' ELSE '' END AS ByWDesc,  
    CASE WHEN BasedOn = 0 THEN 'FIT Wages' WHEN BasedOn=1 THEN 'FICA Wages' WHEN BasedOn=2 THEN 'MEDI Wages'   
    WHEN BasedOn = 3 THEN 'FUTA Wages' WHEN BasedOn=4 THEN 'SIT Wages' WHEN BasedOn=5 THEN 'Vacation Wages'   
    WHEN BasedOn = 6 THEN 'Worker''s Comp Wages' WHEN BasedOn=7 THEN 'Union Wages' WHEN BasedOn=8 THEN 'Flat Amount'   
    ELSE '' END AS BasedOnDesc,  
    CASE WHEN AccruedOn = 0 THEN 'Number of Hours' WHEN AccruedOn=1 THEN 'Dollar Amount' WHEN AccruedOn=2 THEN 'Flat Amount' ELSE '' END AS AccruedOnDesc  
     FROM PRDed WHERE ID = @Id  
END