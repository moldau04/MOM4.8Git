CREATE PROCEDURE spAddJobWage 
@Job int ,
@JobWageC tblTypeJobWageC readonly,
@JobDed tblTypeJobDed readonly,
@AddEdit int

as
BEGIN

	DECLARE @WageC [int] 	
	DECLARE @Reg [numeric](30, 2) 
	DECLARE @OT [numeric](30, 2) 
	DECLARE @DT [numeric](30, 2) 
	DECLARE @TT [numeric](30, 2) 
	DECLARE @NT [numeric](30, 2) 
	DECLARE @GL [int] 
	DECLARE @Fringe1 [numeric](30, 2) 
	DECLARE @Fringe2 [numeric](30, 2) 
	DECLARE @Fringe3 [numeric](30, 2) 
	DECLARE @Fringe4 [numeric](30, 2) 
	DECLARE @PF1 [smallint] 
	DECLARE @PF2 [smallint] 
	DECLARE @PF3 [smallint] 
	DECLARE @PF4 [smallint] 
	DECLARE @FringeGL [int] 
	DECLARE @CReg [numeric](30, 2) 
	DECLARE @COT [numeric](30, 2) 
	DECLARE @CDT [numeric](30, 2) 
	DECLARE @CTT [numeric](30, 2) 
	DECLARE @CNT [numeric](30, 2) 
	DECLARE @Ded [int] 

if @AddEdit = 1 -- 0 for NEW ,1 for Edit
BEGIN
	DELETE FROM JobWageC WHERE Job = @Job
	DELETE FROM JobDed WHERE Job = @Job
END

	DECLARE db_cursor2 CURSOR FOR 

	SELECT [WageC],[Reg],[OT],[DT],[TT],[NT],[GL],[Fringe1],[Fringe2],[Fringe3],[Fringe4],[PF1],[PF2],[PF3],[PF4],[FringeGL],[CReg],[COT],[CDT],[CTT],[CNT] FROM @JobWageC 

	OPEN db_cursor2  
	FETCH NEXT FROM db_cursor2 INTO @WageC,@Reg,@OT,@DT,@TT,@NT,@GL,@Fringe1,@Fringe2,@Fringe3,@Fringe4,@PF1,@PF2,@PF3,@PF4,@FringeGL,@CReg,@COT,@CDT,@CTT,@CNT

	WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO [dbo].[JobWageC] ([ID],[WageC],[Job],[Reg],[OT],[DT],[TT],[NT],[GL],[Fringe1],[Fringe2],[Fringe3],[Fringe4],[PF1],[PF2],[PF3],[PF4],[FringeGL],[CReg],[COT],[CDT],[CTT],[CNT])
		VALUES ((SELECT ISNULL(MAX(ID),0)+1 FROM JobWageC),@WageC,@Job,@Reg,@OT,@DT,@TT,@NT,@GL,@Fringe1,@Fringe2,@Fringe3,@Fringe4,@PF1,@PF2,@PF3,@PF4,@FringeGL,@CReg,@COT,@CDT,@CTT,@CNT)  
		

	---------------->
	  SET  @WageC= NULL ;  SET  @Reg= NULL ;  SET  @OT= NULL ;  SET  @DT= NULL ;  SET  @TT= NULL ;   SET  @NT= NULL ;  SET  @GL= NULL ;  SET  @Fringe1= NULL ;  SET  @Fringe2= NULL ;  
	  SET  @Fringe3= NULL ;SET @Fringe4= NULL ; SET @PF1= NULL ; SET @PF2=NULL; SET @PF3 = NULL;SET @PF4=NULL; SET @FringeGL= NULL; SET @CReg= NULL; SET @CReg= NULL ; SET @COT = NULL;
	  SET @CDT= NULL ; SET @CTT= NULL; SET @CNT=NULL; 
    ---------------->

	FETCH NEXT FROM db_cursor2 INTO @WageC,@Reg,@OT,@DT,@TT,@NT,@GL,@Fringe1,@Fringe2,@Fringe3,@Fringe4,@PF1,@PF2,@PF3,@PF4,@FringeGL,@CReg,@COT,@CDT,@CTT,@CNT
	END

	CLOSE db_cursor2  
	DEALLOCATE db_cursor2

	------------------------------------------------------------------------------------------------------------
	DECLARE db_cursor3 CURSOR FOR 

	SELECT [Ded] FROM @JobDed 

	OPEN db_cursor3  
	FETCH NEXT FROM db_cursor3 INTO @Ded

	WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO [dbo].[JobDed] ([ID],[Ded],[Job])
		VALUES ((SELECT ISNULL(MAX(ID),0)+1 FROM JobDed),@Ded,@Job)  
		

	---------------->
	  SET  @Ded= NULL ;   
    ---------------->

	FETCH NEXT FROM db_cursor3 INTO @Ded
	END

	CLOSE db_cursor3  
	DEALLOCATE db_cursor3



END
