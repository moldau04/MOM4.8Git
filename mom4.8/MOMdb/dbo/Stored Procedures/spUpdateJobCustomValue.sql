/*--------------------------------------------------------------------  
Modified By: Thomas  
Modified On: 08 Oct 2019   
Description: Adding Logs  
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spUpdateJobCustomValue] 
	@custom1	VARCHAR(75)	
	,@custom2	VARCHAR(75)	
	,@custom3	VARCHAR(75)	
	,@custom4	VARCHAR(75)	
	,@custom5	VARCHAR(75)	
	,@custom6	VARCHAR(75)	
	,@custom7	VARCHAR(75)	
	,@custom8	VARCHAR(75)	
	,@custom9	VARCHAR(75)	
	,@custom10	VARCHAR(75)	
	,@custom11	VARCHAR(75)	
	,@custom12	VARCHAR(75)	
	,@custom13	VARCHAR(75)	
	,@custom14	VARCHAR(75)	
	,@custom15	VARCHAR(75)	
	,@custom16	VARCHAR(75)	
	,@custom17	VARCHAR(75)	
	,@custom18	VARCHAR(75)	
	,@custom19	VARCHAR(75)	
	,@custom20	VARCHAR(75)	
	,@jobID int
	--,@UpdatedBy varchar(100)
	,@UpdatedByUserId int

AS
BEGIN TRY
    --BEGIN TRANSACTION
	IF EXISTS (SELECT 1 FROM Job WHERE ID = @jobID)
	BEGIN
		/* Start Logs */
		Declare @UpdatedBy varchar(100);-- = 'thomas';
		Select @UpdatedBy = fUser FROM tblUser WHERE ID = @UpdatedByUserId
		DECLARE @Screen varchar(100) = 'Job';
		DECLARE @RefId int = @jobID;

		DECLARE @currCustomValue1 VARCHAR(200)
		DECLARE @currCustomValue2 VARCHAR(200)
		DECLARE @currCustomValue3 VARCHAR(200)
		DECLARE @currCustomValue4 VARCHAR(200)
		DECLARE @currCustomValue5 VARCHAR(200)
		DECLARE @currCustomValue6 VARCHAR(200)
		DECLARE @currCustomValue7 VARCHAR(200)
		DECLARE @currCustomValue8 VARCHAR(200)
		DECLARE @currCustomValue9 VARCHAR(200)
		DECLARE @currCustomValue10 VARCHAR(200)
		DECLARE @currCustomValue11 VARCHAR(200)
		DECLARE @currCustomValue12 VARCHAR(200)
		DECLARE @currCustomValue13 VARCHAR(200)
		DECLARE @currCustomValue14 VARCHAR(200)
		DECLARE @currCustomValue15 VARCHAR(200)
		DECLARE @currCustomValue16 VARCHAR(200)
		DECLARE @currCustomValue17 VARCHAR(200)
		DECLARE @currCustomValue18 VARCHAR(200)
		DECLARE @currCustomValue19 VARCHAR(200)
		DECLARE @currCustomValue20 VARCHAR(200)

		DECLARE @currCustomLabel1 VARCHAR(200)
		DECLARE @currCustomLabel2 VARCHAR(200)
		DECLARE @currCustomLabel3 VARCHAR(200)
		DECLARE @currCustomLabel4 VARCHAR(200)
		DECLARE @currCustomLabel5 VARCHAR(200)
		DECLARE @currCustomLabel6 VARCHAR(200)
		DECLARE @currCustomLabel7 VARCHAR(200)
		DECLARE @currCustomLabel8 VARCHAR(200)
		DECLARE @currCustomLabel9 VARCHAR(200)
		DECLARE @currCustomLabel10 VARCHAR(200)
		DECLARE @currCustomLabel11 VARCHAR(200)
		DECLARE @currCustomLabel12 VARCHAR(200)
		DECLARE @currCustomLabel13 VARCHAR(200)
		DECLARE @currCustomLabel14 VARCHAR(200)
		DECLARE @currCustomLabel15 VARCHAR(200)
		DECLARE @currCustomLabel16 VARCHAR(200)
		DECLARE @currCustomLabel17 VARCHAR(200)
		DECLARE @currCustomLabel18 VARCHAR(200)
		DECLARE @currCustomLabel19 VARCHAR(200)
		DECLARE @currCustomLabel20 VARCHAR(200)

		SELECT
			@currCustomValue1  = ISNULL(Custom1 ,''),
			@currCustomValue2  = ISNULL(Custom2 ,''),
			@currCustomValue3  = ISNULL(Custom3 ,''),
			@currCustomValue4  = ISNULL(Custom4 ,''),
			@currCustomValue5  = ISNULL(Custom5 ,''),
			@currCustomValue6  = ISNULL(Custom6 ,''),
			@currCustomValue7  = ISNULL(Custom7 ,''),
			@currCustomValue8  = ISNULL(Custom8 ,''),
			@currCustomValue9  = ISNULL(Custom9 ,''),
			@currCustomValue10 = ISNULL(Custom10,''),
			@currCustomValue11 = ISNULL(Custom11,''),
			@currCustomValue12 = ISNULL(Custom12,''),
			@currCustomValue13 = ISNULL(Custom13,''),
			@currCustomValue14 = ISNULL(Custom14,''),
			@currCustomValue15 = ISNULL(Custom15,''),
			@currCustomValue16 = ISNULL(Custom16,''),
			@currCustomValue17 = ISNULL(Custom17,''),
			@currCustomValue18 = ISNULL(Custom18,''),
			@currCustomValue19 = ISNULL(Custom19,''),
			@currCustomValue20 = ISNULL(Custom20,'')
		FROM Job WHERE ID = @jobID

		SELECT @currCustomLabel1 = CASE Label WHEN '' THEN 'Custom 1' 	ELSE Label END from Custom where Name='Job1'
		SELECT @currCustomLabel2 = CASE Label WHEN '' THEN 'Custom 2' 	ELSE Label END from Custom where Name='Job2'
		SELECT @currCustomLabel3 = CASE Label WHEN '' THEN 'Custom 3' 	ELSE Label END from Custom where Name='Job3'
		SELECT @currCustomLabel4 = CASE Label WHEN '' THEN 'Custom 4' 	ELSE Label END from Custom where Name='Job4'
		SELECT @currCustomLabel5 = CASE Label WHEN '' THEN 'Custom 5' 	ELSE Label END from Custom where Name='Job5'
		SELECT @currCustomLabel6 = CASE Label WHEN '' THEN 'Custom 6' 	ELSE Label END from Custom where Name='Job6'
		SELECT @currCustomLabel7 = CASE Label WHEN '' THEN 'Custom 7' 	ELSE Label END from Custom where Name='Job7'
		SELECT @currCustomLabel8 = CASE Label WHEN '' THEN 'Custom 8' 	ELSE Label END from Custom where Name='Job8'
		SELECT @currCustomLabel9 = CASE Label WHEN '' THEN 'Custom 9' 	ELSE Label END from Custom where Name='Job9'
		SELECT @currCustomLabel10 = CASE Label WHEN '' THEN 'Custom 10' 	ELSE Label END from Custom where Name='Job10'
		SELECT @currCustomLabel11 = CASE Label WHEN '' THEN 'Custom 11' 	ELSE Label END from Custom where Name='Job11'
		SELECT @currCustomLabel12 = CASE Label WHEN '' THEN 'Custom 12' 	ELSE Label END from Custom where Name='Job12'
		SELECT @currCustomLabel13 = CASE Label WHEN '' THEN 'Custom 13' 	ELSE Label END from Custom where Name='Job13'
		SELECT @currCustomLabel14 = CASE Label WHEN '' THEN 'Custom 14' 	ELSE Label END from Custom where Name='Job14'
		SELECT @currCustomLabel15 = CASE Label WHEN '' THEN 'Custom 15' 	ELSE Label END from Custom where Name='Job15'
		SELECT @currCustomLabel16 = CASE Label WHEN '' THEN 'Custom 16' 	ELSE Label END from Custom where Name='Job16'
		SELECT @currCustomLabel17 = CASE Label WHEN '' THEN 'Custom 17' 	ELSE Label END from Custom where Name='Job17'
		SELECT @currCustomLabel18 = CASE Label WHEN '' THEN 'Custom 18' 	ELSE Label END from Custom where Name='Job18'
		SELECT @currCustomLabel19 = CASE Label WHEN '' THEN 'Custom 19' 	ELSE Label END from Custom where Name='Job19'
		SELECT @currCustomLabel20 = CASE Label WHEN '' THEN 'Custom 20' 	ELSE Label END from Custom where Name='Job20'

		IF @currCustomValue1  != @Custom1  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel1 ,@currCustomValue1 ,@Custom1  END
		IF @currCustomValue2  != @Custom2  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel2 ,@currCustomValue2 ,@Custom2  END
		IF @currCustomValue3  != @Custom3  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel3 ,@currCustomValue3 ,@Custom3  END
		IF @currCustomValue4  != @Custom4  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel4 ,@currCustomValue4 ,@Custom4  END
		IF @currCustomValue5  != @Custom5  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel5 ,@currCustomValue5 ,@Custom5  END
		IF @currCustomValue6  != @Custom6  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel6 ,@currCustomValue6 ,@Custom6  END
		IF @currCustomValue7  != @Custom7  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel7 ,@currCustomValue7 ,@Custom7  END
		IF @currCustomValue8  != @Custom8  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel8 ,@currCustomValue8 ,@Custom8  END
		IF @currCustomValue9  != @Custom9  BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel9 ,@currCustomValue9 ,@Custom9  END
		IF @currCustomValue10 != @Custom10 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel10,@currCustomValue10,@Custom10 END
		IF @currCustomValue11 != @Custom11 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel11,@currCustomValue11,@Custom11 END
		IF @currCustomValue12 != @Custom12 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel12,@currCustomValue12,@Custom12 END
		IF @currCustomValue13 != @Custom13 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel13,@currCustomValue13,@Custom13 END
		IF @currCustomValue14 != @Custom14 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel14,@currCustomValue14,@Custom14 END
		IF @currCustomValue15 != @Custom15 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel15,@currCustomValue15,@Custom15 END
		IF @currCustomValue16 != @Custom16 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel16,@currCustomValue16,@Custom16 END
		IF @currCustomValue17 != @Custom17 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel17,@currCustomValue17,@Custom17 END
		IF @currCustomValue18 != @Custom18 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel18,@currCustomValue18,@Custom18 END
		IF @currCustomValue19 != @Custom19 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel19,@currCustomValue19,@Custom19 END
		IF @currCustomValue20 != @Custom20 BEGIN EXEC log2_insert @UpdatedBy,@Screen,@RefId,@currCustomLabel20,@currCustomValue20,@Custom20 END
		/* End Logs */

		UPDATE Job
		SET Custom1=@custom1
			,Custom2=@custom2
			,Custom3=@custom3
			,Custom4=@custom4
			,Custom5=@custom5
			,Custom6=@custom6
			,Custom7=@custom7
			,Custom8=@custom8
			,Custom9=@custom9
			,Custom10=@custom10
			,Custom11=@custom11
			,Custom12=@custom12
			,Custom13=@custom13
			,Custom14=@custom14
			,Custom15=@custom15
			,Custom16=@custom16
			,Custom17=@custom17
			,Custom18=@custom18
			,Custom19=@custom19
			,Custom20=@custom20
		WHERE ID=@jobID
	END
	--COMMIT
END TRY
BEGIN CATCH
    SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT > 0
        --ROLLBACK
    RAISERROR ('An error has occurred on spUpdateJobCustomValue.', 16, 1)
    RETURN
END CATCH