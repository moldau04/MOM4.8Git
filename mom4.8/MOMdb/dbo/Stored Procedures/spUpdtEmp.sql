CREATE PROCEDURE spUpdtEmp(
	
	@ID  int  ,
	@fFirst  varchar (15) ,
	@Last  varchar (25)  ,
	@Middle  varchar (15)  ,
	@Name  varchar (55)  ,
	@Rol  int   ,
	@SSN  varchar (11)  ,
	@Title  varchar (50)  ,
	@Sales  smallint    ,
	@Field  smallint    ,
	@Status  smallint   ,
	@Pager  varchar (100)  ,
	@InUse  smallint    ,
	@PayPeriod  smallint   ,
	@DHired  datetime   ,
	@DFired  datetime   ,
	@DBirth  datetime   ,
	@DReview  datetime   ,
	@DLast  datetime   ,
	@FStatus  smallint   ,
	@FAllow  smallint   ,
	@FAdd  numeric (30, 2)  ,
	@SStatus  smallint   ,
	@SAllow  smallint   ,
	@SAdd  numeric (30, 2)  ,
	@CallSign  varchar (50)  ,
	@VRate  numeric (30, 4)  ,
	@VBase  smallint   ,
	@VLast  numeric (30, 2)  ,
	@VThis  numeric (30, 2)  ,
	@Sick  numeric (30, 2)  ,
	@PMethod  smallint   ,
	@PFixed  smallint   ,
	@PHour  numeric (30, 2)  ,
	@LName  smallint   ,
	@LStatus  smallint   ,
	@LAllow  smallint   ,
	@PRTaxE  int   ,
	@State  varchar (2)  ,
	@Salary  numeric (30, 2)  ,
	@SalaryF  smallint   ,
	@SalaryGL  int   ,
	@fWork  int   ,
	@NPaid  smallint   ,
	@Balance  numeric (30, 2)  ,
	@PBRate  numeric (30, 2)  ,
	@FITYTD  numeric (30, 2)  ,
	@FICAYTD  numeric (30, 2)  ,
	@MEDIYTD  numeric (30, 2)  ,
	@FUTAYTD  numeric (30, 2)  ,
	@SITYTD  numeric (30, 2)  ,
	@LocalYTD  numeric (30, 2)  ,
	@BonusYTD  numeric (30, 2)  ,
	@HolH  numeric (30, 2)  ,
	@HolYTD  numeric (30, 2)  ,
	@VacH  numeric (30, 2)  ,
	@VacYTD  numeric (30, 2)  ,
	@ZoneH  numeric (30, 2)  ,
	@ZoneYTD  numeric (30, 2)  ,
	@ReimbYTD  numeric (30, 2)  ,
	@MileH  numeric (30, 2)  ,
	@MileYTD  numeric (30, 2)  ,
	@Race  varchar (40)  ,
	@Sex  varchar (10)  ,
	@Ref  varchar (15)  ,
	@ACH  smallint   ,
	@ACHType  smallint   ,
	@ACHRoute  varchar (20)  ,
	@ACHBank  varchar (20)  ,
	@Anniversary  datetime   ,
	@Level  int   ,
	@WageCat  int   ,
	@DSenior  datetime   ,
	@PRWBR  int   ,
	@PDASerialNumber_1  varchar (100)  ,
	@StatusChange  tinyint   ,
	@SCDate  datetime   ,
	@SCReason  varchar (2)  ,
	@DemoChange  tinyint   ,
	@Language  varchar (2)  ,
	@TicketD  tinyint   ,
	@Custom1  varchar (50)  ,
	@Custom2  varchar (50)  ,
	@Custom3  varchar (50)  ,
	@Custom4  varchar (50)  ,
	@Custom5  varchar (50)  ,
	@DDType  tinyint   ,
	@DDRate  numeric (30, 4)  ,
	@ACHType2  tinyint   ,
	@ACHRoute2  varchar (20)  ,
	@ACHBank2  varchar (20)  ,
	@BillRate  numeric (30, 4)  ,
	@BMSales  numeric (30, 4)  ,
	@BMInvAve  numeric (30, 4)  ,
	@BMClosing  numeric (30, 4)  ,
	@BMBillEff  numeric (30, 4)  ,
	@BMProdEff  numeric (30, 4)  ,
	@BMAveTask  int   ,
	@BMCustom1  int   ,
	@BMCustom2  int   ,
	@BMCustom3  int   ,
	@BMCustom4  int   ,
	@BMCustom5  int   ,
	@TaxCodeNR  varchar (10)  ,
	@TaxCodeR  varchar (10)  ,
	@DeviceID  varchar (100)  ,
	@MileageRate  numeric (30, 2)  ,
	@Import1  varchar (100)  ,
	@MSDeviceId  nchar (100)  ,
	@TechnicianBio  varchar (100)   ,
	@PayPortalPassword  varchar (30)  ,
	@SickRate  numeric (30, 4)  ,
	@SickAccrued  numeric (30, 2)  ,
	@SickUsed  numeric (30, 2)  ,
	@SickYTD  numeric (30, 2)  ,
	@VacAccrued  numeric (30, 2)  ,
	@SCounty  int   ,
	@PDASerialNumber  varchar (50)  ,
	@City varchar (50)  	
	,@Zip varchar (50)  
	,@Tel varchar (50)  
	,@Address varchar (255)  
	,@Email varchar (50)  
	,@Cell varchar (28)  	
	,@Remarks    varchar (250)  
	,@Type int
	,@Contact varchar (50)  
	,@Website varchar (50)
	,@Fax varchar (28)
	,@Country varchar(50),
	@dtWageCategory tblTypePRWageItem readonly,
	@dtWageDeduction tblPRDedItem readonly
	,
	@dtOtherIncome tblPROther readonly

 )
 as BEGIN
 
SELECT @Rol = Rol FROM Emp WHERE ID = @ID
IF NOT EXISTS (SELECT * FROM Emp WHERE Name = @Name AND ID <> @ID)
BEGIN

--UPDATE Rol SET City = @City
--	,STATE = @State
--	,Zip = @Zip
--	,Phone = @Tel
--	,Address = @Address
--	,EMail = @Email
--	,Cellular = @Cell
	
--	,Remarks = @Remarks
--	,[Type] = @Type
--	,Contact = @Contact
--	,Website = @Website
--	,Fax = @Fax
--	,Country = @Country WHERE ID = @Rol
SELECT @SalaryGL = ID FROM CHART WHERE Acct = 'D3100'
SELECT @fWork = ID FROM tblWork WHERE fDesc = @CallSign
--SET @SalaryGL =7;
--SET @fWork  =15;

UPDATE Emp SET [fFirst] = @fFirst,[Last] = @Last,[Middle] = @Middle,[Name] = @Name,[SSN] = @SSN,[Title] = @Title,[Sales] = @Sales,[Field] = @Field,[Status] = @Status,[Pager] =@Pager,
[InUse] = @InUse,[PayPeriod] = @PayPeriod,[DHired] = @DHired,[DFired] = @DFired,[DBirth] = @DBirth,[DReview] = @DReview,[DLast] = @DLast,[FStatus] =@FStatus,[FAllow] =@FAllow,
			[FAdd] =@FAdd,[SStatus] =@SStatus,[SAllow] =@SAllow,[SAdd] =@SAdd,[CallSign]=@CallSign,[VRate]=@VRate,[VBase]=@VBase,[VLast]=@VLast,[VThis]=@VThis,[Sick]=@Sick,[PMethod]=@PMethod,
			[PFixed]=@PFixed,[PHour]=@PHour,[LName]=@LName,[LStatus]=@LStatus,[LAllow]=@LAllow,[PRTaxE]=@PRTaxE,[State]=@State,[Salary]=@Salary,[SalaryF]=@SalaryF,
			[SalaryGL]=@SalaryGL,[fWork]=@fWork,[NPaid]=@NPaid,[Balance]=@Balance ,[PBRate]=@PBRate ,[FITYTD] =@FITYTD,[FICAYTD]=@FICAYTD ,[MEDIYTD]=@MEDIYTD,
			[FUTAYTD] =@FUTAYTD,[SITYTD]=@SITYTD,[LocalYTD] =@LocalYTD,[BonusYTD]=@BonusYTD,[HolH] =@HolH,[HolYTD] =@HolYTD,[VacH] =@VacH,[VacYTD] =@VacYTD,[ZoneH] =@ZoneH,[ZoneYTD] =@ZoneYTD,
			[ReimbYTD] =@ReimbYTD,[MileH] =@MileH,[MileYTD] =@MileYTD,
			
			[Race] =@Race,[Sex] =@Sex,[Ref] =@Ref,[ACH] =@ACH,[ACHType] =@ACHType,[ACHRoute] =@ACHRoute,[ACHBank]=@ACHBank
           ,[Anniversary] =@Anniversary,[Level] =@Level,[WageCat] =@WageCat,[DSenior] =@DSenior,[PRWBR] =@PRWBR,[PDASerialNumber_1] =@PDASerialNumber_1,[StatusChange] =@StatusChange,
		   [SCDate]=@SCDate,[SCReason] =@SCReason,[DemoChange] =@DemoChange,[Language] =@Language,[TicketD] =@TicketD,[Custom1]=@Custom1
           ,[Custom2] =@Custom2,[Custom3] =@Custom3,[Custom4] =@Custom4,[Custom5] =@Custom5,[DDType]=@DDType,[DDRate] =@DDRate,[ACHType2] =@ACHType2,[ACHRoute2] =@ACHRoute2,[ACHBank2] =@ACHBank2,
		   [BillRate] =@BillRate,[BMSales] =@BMSales,[BMInvAve] =@BMInvAve,[BMClosing] =@BMClosing,[BMBillEff]=@BMBillEff
           ,[BMProdEff]=@BMProdEff,[BMAveTask]=@BMAveTask,[BMCustom1]=@BMCustom1,[BMCustom2]=@BMCustom2,[BMCustom3] =@BMCustom3,[BMCustom4] =@BMCustom4,[BMCustom5] =@BMCustom5,
		   [TaxCodeNR] =@TaxCodeNR,[TaxCodeR] =@TaxCodeR,[DeviceID] =@DeviceID ,[MileageRate] =@MileageRate,[Import1] =@Import1,[MSDeviceId]=@MSDeviceId
           ,[PayPortalPassword]=@PayPortalPassword ,[SickRate]=@SickRate,[SickAccrued]=@SickAccrued,[SickUsed] =@SickUsed,[SickYTD] =@SickYTD,[VacAccrued] =@VacAccrued,[SCounty]=@SCounty ,
		   [PDASerialNumber]=@PDASerialNumber WHERE ID = @ID
     
	 DELETE FROM PRWageItem WHERE Emp = @ID
	INSERT INTO [PRWageItem]
           ([Wage],[Emp],[GL],[Reg],[OT],[DT],[TT],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[InUse]
           ,[YTD],[YTDH],[OYTD],[OYTDH],[DYTD],[DYTDH],[TYTD],[TYTDH],[NT],[NYTD],[NYTDH],[VacR],[CReg],[COT],[CDT],[CNT],[CTT],[Status],[Sick])
     SELECT [Wage],@ID,[GL],[Reg],[OT],[DT],[TT],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[InUse]
           ,[YTD],[YTDH],[OYTD],[OYTDH],[DYTD],[DYTDH],[TYTD],[TYTDH],[NT],[NYTD],[NYTDH],[VacR],[CReg],[COT],[CDT],[CNT],[CTT],[Status],[Sick] FROM @dtWageCategory
	DELETE FROM PRDedItem WHERE Emp = @ID
	INSERT INTO [PRDedItem]
           ([Ded],[Emp],[BasedOn],[AccruedOn],[ByW],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL],[CompGLE],[InUse],[YTD],[YTDC])
	SELECT [Ded],@ID,[BasedOn],[AccruedOn],[ByW],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL],[CompGLE],[InUse],[YTD],[YTDC] FROM @dtWageDeduction
	DELETE FROM PROther WHERE Emp = @ID
	INSERT INTO [PROther]
           ([Cat],[Emp],[GL],[Rate],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[Sick]) 
	SELECT [Cat],@ID,[GL],[Rate],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[Sick] FROM @dtOtherIncome
	
END
ELSE 
BEGIN 
RAISERROR ('Employee name already exists, please use different name !',16,1)
--ROLLBACK TRANSACTION
RETURN
END

--IF @@ERROR <> 0
--	AND @@TRANCOUNT > 0
--BEGIN
--	RAISERROR (
--			'Error Occured'
--			,16
--			,1
--			)

--	ROLLBACK TRANSACTION

--	RETURN
--END

 END

 
