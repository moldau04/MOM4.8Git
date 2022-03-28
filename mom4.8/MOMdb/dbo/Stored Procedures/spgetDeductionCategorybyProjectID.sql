CREATE PROCEDURE spgetDeductionCategorybyProjectID (
@Job int 
)
as
BEGIN

	SELECT j.[Ded] as id,
p.BasedOn,
p.AccruedOn,p.ByW,p.EmpRate,p.EmpTop,p.EmpGL,(select c.acct+' : '+c.fdesc from Chart c where c.ID  =p.EmpGL) AS EmpGLName,
	p.CompRate,p.CompTop,p.CompGL,(select c.acct+' : '+c.fdesc from Chart c where c.ID  =p.CompGL) AS CompGLName,p.CompGLE,
	(select c.acct+' : '+c.fdesc from Chart c where c.ID  =p.CompGLE) AS CompGLEName,p.InUse,0.00 AS YTD,0.00 AS YTDC,p.fDesc,
	CASE WHEN p.ByW = 0 THEN 'Company' WHEN p.ByW=1 THEN 'Employee' WHEN p.ByW=2 THEN 'Both' ELSE '' END AS ByWDesc,
				CASE WHEN p.BasedOn = 0 THEN 'FIT Wages' WHEN p.BasedOn=1 THEN 'FICA Wages' WHEN p.BasedOn=2 THEN 'MEDI Wages' 
				WHEN p.BasedOn = 3 THEN 'FUTA Wages' WHEN p.BasedOn=4 THEN 'SIT Wages' WHEN p.BasedOn=5 THEN 'Vacation Wages' 
				WHEN p.BasedOn = 6 THEN 'Worker''s Comp Wages' WHEN p.BasedOn=7 THEN 'Union Wages' WHEN p.BasedOn=8 THEN 'Flat Amount' 
				ELSE '' END AS BasedOnDesc,
				CASE WHEN p.AccruedOn = 0 THEN 'Number of Hours' WHEN p.AccruedOn=1 THEN 'Dollar Amount' WHEN p.AccruedOn=2 THEN 'Flat Amount' ELSE '' END AS AccruedOnDesc
	FROM JobDed as j INNER JOIN PRDed p on p.ID = j.Ded WHERE j.Job =  @Job

END