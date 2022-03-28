Create Procedure GetServiceTypeDDLData

@SearchBy varchar(100)='',
@Case varchar(100)='INV'

AS

BEGIN
SET @SearchBy = ISNULL(@SearchBy,'')

if(@Case ='INV')

SELECT    ID    value , NAME FROM INV WHERE  Status=0 and  TYPE = 1 AND Name LIKE '%'+@SearchBy+'%' ORDER BY Name   -- Billing Code

if(@Case ='LTYPE')

SELECT    Type  value, Type NAME FROM LocType WHERE    Type LIKE '%'+@SearchBy+'%' ORDER BY Name   -- Location Type

if(@Case ='PRWage')

SELECT    ID  value, fDesc NAME FROM PRWage WHERE  Status=0 and fDesc LIKE '%'+@SearchBy+'%' ORDER BY fDesc   -- Wage Category

if(@Case ='Route')

SELECT   ID    value, Name NAME FROM Route WHERE Status=1 and  Name LIKE '%'+@SearchBy+'%' ORDER BY Name    -- Route 

if(@Case ='Chart')

SELECT  C.ID AS value , C.Acct  +' : '+ C.fDesc as Name 
					FROM Chart C
					left join Bank B on C.ID=b.Chart 
					where C.Status = 0 AND C.Type <> 7 AND (C.fDesc LIKE '%'+@SearchBy+'%' OR C.Acct LIKE '%'+@SearchBy+'%')  ORDER BY C.Acct,C.fDesc

					-- Interest GL or Expense GL 

if(@Case ='Department')
select id Value , Type Name from JobType  WHERE    Type LIKE '%'+@SearchBy+'%' ORDER BY Type--Department

 
END