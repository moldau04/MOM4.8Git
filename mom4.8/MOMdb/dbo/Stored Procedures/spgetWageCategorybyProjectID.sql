CREATE PROCEDURE spgetWageCategorybyProjectID (
@Job int 
)
as
BEGIN
	SELECT [WageC] as id,j.[Reg],j.[OT],j.[DT],j.[TT],j.[NT],j.[GL],(select c.acct+' : '+c.fdesc from Chart c where c.ID  =j.GL) AS GLName,
	p.FIT,p.FICA,p.MEDI,p.FUTA,p.SIT,p.Vac,p.WC,p.Uni,0 as InUse,p.Sick,p.Status,0.00 as YTD,0.00 AS YTDH,0.00 AS OYTD,0.00 AS OYTDH,0.00 AS DYTD,0.00 AS DYTDH,0.00 AS TYTD,
	0.00 AS TYTDH,0.00 AS NYTD,0.00 AS NYTDH,'' AS VacR,j.CReg,j.COT,j.CDT,j.CNT,j.CTT,p.fDesc FROM JobWageC j INNER JOIN PRWage p ON p.ID= j.WageC WHERE j.Job =  @Job

END