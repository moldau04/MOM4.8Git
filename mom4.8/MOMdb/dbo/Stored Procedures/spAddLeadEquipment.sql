/*--------------------------------------------------------------------
Modified by: Thomas
Modified On: 1 Mar 2019	
Desc: Removed error code and re-format code

Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spAddLeadEquipment 
--------------------------------------------------------------------*/

CREATE PROCEDURE [dbo].[spAddLeadEquipment]
	@Lead INT = NULL
	, @Unit VARCHAR(20) = NULL
	, @fDesc VARCHAR(50) = NULL
	, @Type VARCHAR(20) = NULL
	, @Cat VARCHAR(20) = NULL
	, @Manuf VARCHAR(20) = NULL
	, @Serial VARCHAR(50) = NULL
	, @State VARCHAR(25) = NULL
	, @Since DATETIME = NULL
	, @Last DATETIME = NULL
	, @Price NUMERIC(30, 2) = NULL
	, @Status SMALLINT = NULL
	, @Remarks TEXT = NULL
	, @Install DATETIME = NULL
	, @Category VARCHAR(20) = NULL
	, @template INT = NULL
	, @UpdatedBy AS VARCHAR(75) = NULL
	, @items AS [dbo].[tblTypeEquipTempItems_EquipmentScreen] Readonly
	, @CustomItems AS [dbo].[tblTypeCustomTemplateForLeadEquip] Readonly
	, @EquipIDOut INT = 0 OUTPUT
	, @Building VARCHAR(20) = NULL
	, @Classification VARCHAR(20)
	, @Shutdown BIT
	, @ShutdownReason VARCHAR(MAX)
	, @UserID INT
	, @ShutdownLongDesc VARCHAR(MAX) AS

BEGIN TRANSACTION
IF(SELECT COUNT(*) FROM LeadEquip WHERE Unit=@Unit AND Lead=@Lead)>0
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

	INSERT INTO LeadEquip (
		Lead
		, OWNER
		, Unit
		, fDesc
		, Type
		, Cat
		, Manuf
		, Serial
		, STATE
		, Since
		, Last
		, Price
		, STATUS
		, Building
		, Remarks
		, fGroup
		, Template
		, InstallBy
		, Install
		, category
		, LastUpdateDate
		, Classification
		, shut_down
		, ShutdownReason
		)
	VALUES (
		@Lead
		, null
		, @Unit
		, @fDesc
		, @Type
		, @Cat
		, @Manuf
		, @Serial
		, @State
		, @Since
		, @Last
		, @Price
		, @Status
		, @Building
		, @Remarks
		, ''
		, @template
		, ''
		, @Install
		, @Category
		, @LastUpdateDate
		, @Classification
		, @Shutdown
		, @ShutdownReason
		)

	--UPDATE Prospect SET LeadEquip = (LeadEquip + 1)	WHERE ID = @Lead

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

	DECLARE @leadEquip INT
	SET @leadEquip = Scope_identity()

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
		,LeadEquip
		)
	SELECT code
		,EquipT
		,0
		,fDesc
		,Frequency
		,Lastdate
		,Line
		,NextDateDue
		,Section
		,Notes
		,@leadEquip
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
	SELECT ID , fDesc, Line, value, Format, LastUpdated, LastUpdateUser, OrderNo FROM @CustomItems
	OPEN db_cursor
	FETCH NEXT FROM db_cursor INTO @itemid
		, @ifdesc
		, @line
		, @value
		, @format
		, @LastUpdated
		, @LastUpdateUser
		, @OrderNo
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
			,LeadEquip
			)
		VALUES (
			(
				SELECT ISNULL(Max(ID), 0) + 1
				FROM ElevTItem
			)
			,@template
			,0
			,@itemid
			,@ifdesc
			,@line
			,@value
			,@format
			,CASE @LastUpdated
				WHEN '' THEN NULL
				ELSE convert(DATETIME, @LastUpdated)
				END
			,@LastUpdateUser
			,@OrderNo
			,@leadEquip
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
SELECT @EquipIDOut= @leadEquip
---- Update ElevShutdown table after insert equipment
---- In case the equiment was shut down when adding
--IF(@Shutdown = 1)
--BEGIN
--	--INSERT INTO ElevShutdown (ElevID, Shut_Down, [Description], EquipName, fDate, fUser)  VALUES (@EquipIDOut, @Shutdown, @ShutdownReason, @Unit, @LastUpdateDate, @UpdatedBy)
--	INSERT INTO ElevShutDownLog (elev_id, [status], reason, created_on, created_by, longdesc)  VALUES (@EquipIDOut, @Shutdown, @ShutdownReason, @LastUpdateDate, @UserID, @ShutdownLongDesc)
--END
--if(@Lead is not null)
--begin 
--	Declare @leadName nvarchar(100)
--	SELECT Top 1 @leadName = Tag From Lead Where ID = @Lead
--	exec log2_insert @UpdatedBy,'LeadEquip',@EquipIDOut,'Location','',@locName
--end
--if(@Unit  is not null AND @Unit != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Unit','',@Unit
--end
--if(@fDesc  is not null AND @fDesc !='')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Description','',@fDesc
--end
--if(@Type is not null AND @Type !='')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Type','',@Type
--end
--if(@Cat  is not null And @Cat != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Service Type','',@Cat
--end
--if(@Manuf  is not null And @Manuf != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Manufacturer','',@Manuf
--end
--if(@Serial  is not null And @Serial != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Serial#','',@Serial
--end
--if(@State  is not null And @State != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Unique#','',@State
--end
--if(@Since is not null And @Since != '')
--begin
--	Declare @Sincedate varchar(50)
--	SET @Sincedate = CONVERT(Varchar(50),@Since,101)
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Serviced Since','',@Sincedate
--end
--if(@Last  is not null And @Last != '')
--begin 
--	Declare @Lastdate varchar(50)
--	SET @Lastdate = CONVERT(Varchar(50),@Last,101)
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Last Serviced','',@Lastdate
--end
--if(@Price  is not null)
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Price','',@Price
--end
--if(@Status  is not null)
--begin
--	Declare @StatusVal varchar(50)
--	Select @StatusVal = Case When @Status = 0 Then 'Active' Else 'Inactive' END 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Status','',@StatusVal
--end
--if(@Building  is not null And @Building != '')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Building','',@Building
--end
--if(@Remarks is not null)
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Remarks','',@Remarks
--end
--if(@template  is not null AND @template !='')
--begin 
--	Declare @CustomTemplate nvarchar(100)
--	SELECT Top 1 @CustomTemplate =   fDesc from elevt Where ID = @template
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Template','',@CustomTemplate
--end
--if(@Install is not null AND @Install !='')
--begin 
--	Declare @InstallDate varchar(50)
--	SET @InstallDate = CONVERT(Varchar(50),@Install,101)
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Installed Date','',@InstallDate
--end
--if(@Category  is not null AND @Category !='')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Category','',@Category
--end
--if(@Classification  is not null AND @Classification !='')
--begin 
--	exec log2_insert @UpdatedBy,'Elev',@EquipIDOut,'Classification','',@Classification
--end
COMMIT TRANSACTION
END

	
GO
