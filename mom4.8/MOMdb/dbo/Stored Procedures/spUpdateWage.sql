CREATE proc [dbo].[spUpdateWage]
@ID int,
@Name varchar(50),
@Field int,
@Reg numeric(30,4),
@OT1 numeric(30,4),
@OT2 numeric(30,4),
@TT numeric(30,4),
@FIT smallint,
@FICA smallint, 
@MEDI smallint,
@FUTA smallint,
@SIT smallint,
@Vac smallint,
@WC smallint,
@Uni smallint,
@GL int,
@NT numeric(30,4),
@MileageGL int,
@ReimGL int,
@ZoneGL int,
@Globe smallint,
@Status smallint,
@CReg numeric(30,4),
@COT numeric(30,4),
@CDT numeric(30,4),
@CNT numeric(30,4),
@CTT numeric(30,4),
@Remarks varchar(8000),
@RegGL int,
@OTGL int,
@NTGL int,
@DTGL int,
@TTGL int,
@Sick smallint
as
DECLARE @count int

select @count=Count(*) from PRWage where fDesc=@Name and ID<>@ID
if (@count=0)
begin

  UPDATE [dbo].[PRWage]
   SET [fDesc] = @Name
      ,[Field] = @Field
      ,[Reg] = @Reg
      ,[OT1] = @OT1
      ,[OT2] = @OT2
      ,[TT] = @TT
      ,[FIT] = @FIT
      ,[FICA] = @FICA
      ,[MEDI] = @MEDI
      ,[FUTA] = @FUTA
      ,[SIT] = @SIT
      ,[Vac] = @Vac
      ,[WC] = @WC
      ,[Uni] = @Uni
      --,[Count] = <Count, int,>
      --,[LCount] = <LCount, int,>
      ,[Remarks] = @Remarks
      ,[GL] = @GL
      ,[NT] = @NT
      ,[MileageGL] = @MileageGL
      ,[ReimburseGL] = @ReimGL
      ,[ZoneGL] = @ZoneGL
      ,[Globe] = @Globe
      ,[Status] = @Status
      ,[CReg] = @CReg
      ,[COT] = @COT
      ,[CDT] = @CDT
      ,[CNT] = @CNT
      ,[CTT] = @CTT
      --,[QBWageID] = <QBWageID, varchar(100),>
      --,[LastUpdateDate] = <LastUpdateDate, datetime,>
      --,[QBAccountID] = <QBAccountID, varchar(100),>
      ,[RegGL] = @RegGL
      ,[OTGL] = @OTGL
      ,[NTGL] = @NTGL
      ,[DTGL] = @DTGL
      ,[TTGL] = @TTGL
	  ,[Sick] = @Sick
WHERE  ID =  @ID


end
else
BEGIN

 RAISERROR ('Wage Category already exists, please use different name !',16,1)
  RETURN
 

END

