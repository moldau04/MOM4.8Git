/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spAddEquipment table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spAddEquipment] 
	@Loc INT = NULL
	,@Unit VARCHAR(20) = NULL
	,@fDesc VARCHAR(50) = NULL
	,@Type VARCHAR(20) = NULL
	,@Cat VARCHAR(20) = NULL
	,@Manuf VARCHAR(20) = NULL
	,@Serial VARCHAR(50) = NULL
	,@State VARCHAR(25) = NULL
	,@Since DATETIME = NULL
	,@Last DATETIME = NULL
	,@Price NUMERIC(30, 2) = NULL
	,@Status SMALLINT = NULL
	,@Remarks TEXT = NULL
	,@Install DATETIME = NULL
	,@Category VARCHAR(20) = NULL
	,@template INT = NULL
	,@UpdatedBy AS VARCHAR(75) = NULL
	,@items AS [dbo].[tblTypeEquipTempItems_EquipmentScreen] Readonly
	,@CustomItems AS [dbo].[tblTypeCustomTemplate] Readonly
	,@EquipIDOut INT = 0 OUTPUT
	,@Building VARCHAR(20) = NULL
	,@Classification VARCHAR(20)
	,@Shutdown BIT
	,@ShutdownReason VARCHAR(MAX)
	,@UserID INT
	,@ShutdownLongDesc VARCHAR(MAX) AS

BEGIN TRANSACTION

IF(SELECT COUNT(*) FROM ELev WHERE Unit=@Unit AND ELev.Loc=@Loc)>0
BEGIN	
		RAISERROR (
			'Equipment ID already exist.'
			,16
			,1
			)

	ROLLBACK TRANSACTION
END	
ELSE
BEGIN
	DECLARE @LastUpdateDate DATETIME = GETDATE()

UPDATE ElevT
SET [Count] = ([Count] + 1)
WHERE ID = @template

INSERT INTO Elev (
	Loc
	,OWNER
	,Unit
	,fDesc
	,Type
	,Cat
	,Manuf
	,Serial
	,STATE
	,Since
	,Last
	,Price
	,STATUS
	,Building
	,Remarks
	,fGroup
	,Template
	,InstallBy
	,Install
	,category
	,LastUpdateDate
	,Classification
	,shut_down
	,ShutdownReason
	)
VALUES (
	@Loc
	,(
		SELECT OWNER
		FROM loc
		WHERE loc = @loc
		)
	,@Unit
	,@fDesc
	,@Type
	,@Cat
	,@Manuf
	,@Serial
	,@State
	,@Since
	,@Last
	,@Price
	,@Status
	,@Building
	,@Remarks
	,''
	,@template
	,''
	,@Install
	,@Category
	,@LastUpdateDate
	,@Classification
	,@Shutdown
	,@ShutdownReason
	)

UPDATE Loc
SET [Elevs] = ([Elevs] + 1)
WHERE Loc = @Loc

UPDATE [Owner]
SET [Elevs] = ([Elevs] + 1)
WHERE ID = (
		SELECT [Owner]
		FROM Loc
		WHERE Loc = @Loc
		)

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

DECLARE @elev INT

SET @elev = Scope_identity()

INSERT INTO EquipTItem (
	Code
	,EquipT
	,Elev
	,fDesc
	,Frequency
	,Lastdate
	,Line
	,NextDateDue
	,Section
	,Notes
	)
SELECT code
	,EquipT
	,@elev
	,fDesc
	,Frequency
	,Lastdate
	,Line
	,NextDateDue
	,Section
	,Notes
FROM @items

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

DECLARE @itemid INT
DECLARE @ifdesc VARCHAR(50)
DECLARE @line INT
DECLARE @value VARCHAR(50)
DECLARE @format VARCHAR(50)
DECLARE @LastUpdated DATETIME
DECLARE @LastUpdateUser VARCHAR(20)
DECLARE @OrderNo INT

DECLARE db_cursor CURSOR
FOR
SELECT ID
	,fDesc
	,Line
	,value
	,Format
	,LastUpdated
	,LastUpdateUser
	,OrderNo
FROM @CustomItems

OPEN db_cursor

FETCH NEXT
FROM db_cursor
INTO @itemid
	,@ifdesc
	,@line
	,@value
	,@format
	,@LastUpdated
	,@LastUpdateUser
	,@OrderNo

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO ElevTItem (
		ID
		,ElevT
		,Elev
		,CustomID
		,fDesc
		,Line
		,Value
		,Format
		,LastUpdated
		,LastUpdateUser
		,OrderNo
		)
	VALUES (
		(
			SELECT ISNULL(Max(ID), 0) + 1
			FROM ElevTItem
			)
		,@template
		,@elev
		,@itemid
		,@ifdesc
		,@line
		,@value
		,@format
		,CASE @LastUpdated
			WHEN ''
				THEN NULL
			ELSE convert(DATETIME, @LastUpdated)
			END
		,@LastUpdateUser
		,@OrderNo
		)

	IF @@ERROR <> 0
		AND @@TRANCOUNT > 0
	BEGIN
		RAISERROR (
				'Error Occured'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		CLOSE db_cursor

		DEALLOCATE db_cursor

		RETURN
	END

	FETCH NEXT
	FROM db_cursor
	INTO @itemid
		,@ifdesc
		,@line
		,@value
		,@format
		,@LastUpdated
		,@LastUpdateUser
		,@OrderNo
END

CLOSE db_cursor

DEALLOCATE db_cursor

SELECT @EquipIDOut = Isnull(Max(ID), 0)
FROM Elev

-- Update ElevShutdown table after insert equipment
-- In case the equiment was shut down when adding
IF (@Shutdown = 1)
BEGIN
	--INSERT INTO ElevShutdown (ElevID, Shut_Down, [Description], EquipName, fDate, fUser)  VALUES (@EquipIDOut, @Shutdown, @ShutdownReason, @Unit, @LastUpdateDate, @UpdatedBy)
	INSERT INTO ElevShutDownLog (
		elev_id
		,[status]
		,reason
		,created_on
		,created_by
		,longdesc
		)
	VALUES (
		@EquipIDOut
		,@Shutdown
		,@ShutdownReason
		,@LastUpdateDate
		,@UserID
		,@ShutdownLongDesc
		)
END

IF (@loc IS NOT NULL)
BEGIN
	DECLARE @locName NVARCHAR(100)

	SELECT TOP 1 @locName = Tag
	FROM Loc
	WHERE Loc = @loc

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Location'
		,''
		,@locName
END

IF (
		@Unit IS NOT NULL
		AND @Unit != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Unit'
		,''
		,@Unit
END

IF (
		@fDesc IS NOT NULL
		AND @fDesc != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Description'
		,''
		,@fDesc
END

IF (
		@Type IS NOT NULL
		AND @Type != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Type'
		,''
		,@Type
END

IF (
		@Cat IS NOT NULL
		AND @Cat != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Service Type'
		,''
		,@Cat
END

IF (
		@Manuf IS NOT NULL
		AND @Manuf != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Manufacturer'
		,''
		,@Manuf
END

IF (
		@Serial IS NOT NULL
		AND @Serial != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Serial#'
		,''
		,@Serial
END

IF (
		@State IS NOT NULL
		AND @State != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Unique#'
		,''
		,@State
END

IF (
		@Since IS NOT NULL
		AND @Since != ''
		)
BEGIN
	DECLARE @Sincedate VARCHAR(50)

	SET @Sincedate = CONVERT(VARCHAR(50), @Since, 101)

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Serviced Since'
		,''
		,@Sincedate
END

IF (
		@Last IS NOT NULL
		AND @Last != ''
		)
BEGIN
	DECLARE @Lastdate VARCHAR(50)

	SET @Lastdate = CONVERT(VARCHAR(50), @Last, 101)

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Last Serviced'
		,''
		,@Lastdate
END

IF (@Price IS NOT NULL)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Price'
		,''
		,@Price
END

IF (@Status IS NOT NULL)
BEGIN
	DECLARE @StatusVal VARCHAR(50)

	SELECT @StatusVal = CASE 
			WHEN @Status = 0
				THEN 'Active'
			ELSE 'Inactive'
			END

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Status'
		,''
		,@StatusVal
END

IF (
		@Building IS NOT NULL
		AND @Building != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Building'
		,''
		,@Building
END

IF (@Remarks IS NOT NULL)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Remarks'
		,''
		,@Remarks
END

IF (
		@template IS NOT NULL
		AND @template != ''
		)
BEGIN
	DECLARE @CustomTemplate NVARCHAR(100)

	SELECT TOP 1 @CustomTemplate = fDesc
	FROM elevt
	WHERE ID = @template

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Template'
		,''
		,@CustomTemplate
END

IF (
		@Install IS NOT NULL
		AND @Install != ''
		)
BEGIN
	DECLARE @InstallDate VARCHAR(50)

	SET @InstallDate = CONVERT(VARCHAR(50), @Install, 101)

	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Installed Date'
		,''
		,@InstallDate
END

IF (
		@Category IS NOT NULL
		AND @Category != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Category'
		,''
		,@Category
END

IF (
		@Classification IS NOT NULL
		AND @Classification != ''
		)
BEGIN
	EXEC log2_insert @UpdatedBy
		,'Elev'
		,@EquipIDOut
		,'Classification'
		,''
		,@Classification
END

COMMIT TRANSACTION
END

