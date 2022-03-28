CREATE PROCEDURE [dbo].[spGetWageByID]
@id int
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @GLAcct int
	DECLARE @MileageGL int
	DECLARE @ReimGL int
	DECLARE @ZoneGL int
	DECLARE @RegGL int
	DECLARE @OTGL int
	DECLARE @NTGL int
	DECLARE @DTGL int
	DECLARE @TTGL int

	DECLARE @GLAcctName varchar(75)
	DECLARE @MileageGLName varchar(75)
	DECLARE @ReimGLName varchar(75)
	DECLARE @ZoneGLName varchar(75)
	DECLARE @RegGLName varchar(75)
	DECLARE @OTGLName varchar(75)
	DECLARE @NTGLName varchar(75)
	DECLARE @DTGLName varchar(75)
	DECLARE @TTGLName varchar(75)

	select @GLAcct=GL,@MileageGL=MileageGL,@ReimGL=ReimburseGL,@ZoneGL=ZoneGL,@RegGL=RegGL,@OTGL=OTGL,@NTGL=NTGL,@DTGL=DTGL,@TTGL=TTGL from PRWage where ID=@id
    
	select @GLAcctName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.GL = c.ID and p.ID=@id
	select @MileageGLName =(c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.MileageGL = c.ID and p.ID=@id
	select @ReimGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.ReimburseGL = c.ID and p.ID=@id
	select @ZoneGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.ZoneGL = c.ID and p.ID=@id
	select @RegGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.RegGL = c.ID and p.ID=@id
	select @OTGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.OTGL = c.ID and p.ID=@id
	select @NTGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.NTGL = c.ID and p.ID=@id
	select @DTGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.DTGL = c.ID and p.ID=@id
	select @TTGLName = (c.acct+' : '+c.fdesc) from PRWage p, Chart c where p.TTGL = c.ID and p.ID=@id

	select *,'GLName'=@GLAcctName,'MileageGLName'=@MileageGLName,'ReimGLName'=@ReimGLName,'ZoneGLName'=@ZoneGLName,
	'RegGLName'=@RegGLName,'OTGLName'=@OTGLName,'NTGLName'=@NTGLName,'DTGLName'=@DTGLName,'TTGLName'=@TTGLName  from PRWage where ID=@id
END

