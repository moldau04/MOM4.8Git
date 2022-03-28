CREATE PROCEDURE spAddEmp(    
     
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
 @dtWageDeduction tblPRDedItem readonly,    
 @dtOtherIncome tblPROther readonly    
    
 )    
 as BEGIN    
    
IF NOT EXISTS (SELECT * FROM Emp WHERE Name = @Name)    
BEGIN    
    
    
INSERT INTO Rol (    
 NAME    
 ,City    
 ,STATE    
 ,Zip    
 ,Phone    
 ,Address    
 ,EMail    
 ,Cellular    
 ,GeoLock    
 ,Remarks    
 ,[Type]    
 ,Contact    
 ,Website,    
 Fax    
 ,Country    
 )    
VALUES (    
 @fFirst + ', ' + @Last    
 ,@City    
 ,@State    
 ,@Zip    
 ,@Tel    
 ,@Address    
 ,@Email    
 ,@Cell    
 ,0    
  ,@Remarks        
 ,@Type    
 ,@Contact    
 ,@Website,    
 @Fax    
 ,@Country    
 )    
    
SET @Rol = Scope_identity()    
    
 INSERT INTO [Emp] ([fFirst] ,[Last],[Middle],[Name],[Rol],[SSN],[Title],[Sales],[Field],[Status],[Pager],[InUse],[PayPeriod],[DHired],[DFired],[DBirth],[DReview],[DLast],[FStatus],[FAllow],    
   [FAdd],[SStatus],[SAllow],[SAdd],[CallSign],[VRate],[VBase],[VLast],[VThis],[Sick],[PMethod],[PFixed],[PHour],[LName],[LStatus],[LAllow],[PRTaxE],[State],[Salary],[SalaryF],    
   [SalaryGL],[fWork],[NPaid],[Balance] ,[PBRate] ,[FITYTD] ,[FICAYTD] ,[MEDIYTD],[FUTAYTD] ,[SITYTD],[LocalYTD] ,[BonusYTD],[HolH] ,[HolYTD] ,[VacH] ,[VacYTD] ,[ZoneH] ,[ZoneYTD] ,    
   [ReimbYTD] ,[MileH] ,[MileYTD] ,[Race] ,[Sex] ,[Ref] ,[ACH] ,[ACHType] ,[ACHRoute] ,[ACHBank]    
           ,[Anniversary] ,[Level] ,[WageCat] ,[DSenior] ,[PRWBR] ,[PDASerialNumber_1] ,[StatusChange] ,[SCDate],[SCReason] ,[DemoChange] ,[Language] ,[TicketD] ,[Custom1]    
           ,[Custom2] ,[Custom3] ,[Custom4] ,[Custom5] ,[DDType],[DDRate] ,[ACHType2] ,[ACHRoute2] ,[ACHBank2] ,[BillRate] ,[BMSales] ,[BMInvAve] ,[BMClosing] ,[BMBillEff]    
           ,[BMProdEff],[BMAveTask],[BMCustom1],[BMCustom2],[BMCustom3] ,[BMCustom4] ,[BMCustom5] ,[TaxCodeNR] ,[TaxCodeR] ,[DeviceID] ,[MileageRate] ,[Import1] ,[MSDeviceId]    
           ,[PayPortalPassword] ,[SickRate],[SickAccrued],[SickUsed] ,[SickYTD] ,[VacAccrued] ,[SCounty] ,[PDASerialNumber])    
     VALUES (@fFirst  , @Last  ,@Middle  , @Name  , @Rol  , @SSN  , @Title  , @Sales  , @Field  , @Status  , @Pager  ,    
 @InUse  , @PayPeriod  , @DHired  , @DFired  , @DBirth  , @DReview  , @DLast  , @FStatus  , @FAllow  , @FAdd  , @SStatus  , @SAllow  , @SAdd  , @CallSign  ,    
 @VRate  , @VBase  , @VLast  , @VThis  , @Sick  , @PMethod ,  @PFixed ,  @PHour , @LName  , @LStatus  , @LAllow  , @PRTaxE  , @State  , @Salary  , @SalaryF  ,    
 @SalaryGL , @fWork  , @NPaid  , @Balance , @PBRate  , @FITYTD  , @FICAYTD  , @MEDIYTD  , @FUTAYTD  , @SITYTD  , @LocalYTD  , @BonusYTD  , @HolH  , @HolYTD  ,    
 @VacH  , @VacYTD  , @ZoneH  , @ZoneYTD  , @ReimbYTD , @MileH  , @MileYTD  , @Race  , @Sex  , @Ref  , @ACH  , @ACHType  , @ACHRoute  , @ACHBank  , @Anniversary  ,    
 @Level  , @WageCat  , @DSenior  , @PRWBR  , @PDASerialNumber_1  , @StatusChange  , @SCDate  , @SCReason  , @DemoChange  , @Language  , @TicketD  ,    
 @Custom1  , @Custom2  , @Custom3  , @Custom4  , @Custom5  , @DDType  , @DDRate  , @ACHType2  , @ACHRoute2  , @ACHBank2  , @BillRate  , @BMSales  , @BMInvAve  ,    
 @BMClosing  , @BMBillEff  , @BMProdEff  , @BMAveTask  , @BMCustom1  , @BMCustom2  , @BMCustom3  , @BMCustom4  , @BMCustom5  , @TaxCodeNR  ,    
 @TaxCodeR  , @DeviceID  , @MileageRate  , @Import1  , @MSDeviceId  ,  @PayPortalPassword  , @SickRate  , @SickAccrued  , @SickUsed  ,    
 @SickYTD  , @VacAccrued  , @SCounty  , @PDASerialNumber)    
    
 SET @ID  = Scope_identity()    
 INSERT INTO [PRWageItem]    
           ([Wage],[Emp],[GL],[Reg],[OT],[DT],[TT],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[InUse]    
           ,[YTD],[YTDH],[OYTD],[OYTDH],[DYTD],[DYTDH],[TYTD],[TYTDH],[NT],[NYTD],[NYTDH],[VacR],[CReg],[COT],[CDT],[CNT],[CTT],[Status],[Sick])    
     SELECT [Wage],@ID,[GL],[Reg],[OT],[DT],[TT],[FIT],[FICA],[MEDI],[FUTA],[SIT],[Vac],[WC],[Uni],[InUse]    
           ,[YTD],[YTDH],[OYTD],[OYTDH],[DYTD],[DYTDH],[TYTD],[TYTDH],[NT],[NYTD],[NYTDH],[VacR],[CReg],[COT],[CDT],[CNT],[CTT],[Status],[Sick] FROM @dtWageCategory    
    
 INSERT INTO [PRDedItem]    
           ([Ded],[Emp],[BasedOn],[AccruedOn],[ByW],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL],[CompGLE],[InUse],[YTD],[YTDC])    
 SELECT [Ded],@ID,[BasedOn],[AccruedOn],[ByW],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL],[CompGLE],[InUse],[YTD],[YTDC] FROM @dtWageDeduction    
    
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
-- AND @@TRANCOUNT > 0    
--BEGIN    
-- RAISERROR (    
--   'Error Occured'    
--   ,16    
--   ,1    
--   )    
    
-- ROLLBACK TRANSACTION    
    
-- RETURN    
--END    
    
 END    
    