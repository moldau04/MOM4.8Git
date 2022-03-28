/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spAddCustomTemplate
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spAddCustomTemplate] 
	@fdesc VARCHAR(255)
	,@remarks VARCHAR(8000)
	,@Items AS [dbo].[tblTypeCustomTempl] Readonly
	,@equipt INT
	,@mode INT
	,@ItemsDeleted AS [dbo].[tblTypeCustomTemplDelet] Readonly
	,@CustomValues AS [dbo].[tblTypeCustomValues] Readonly
	
AS
BEGIN TRANSACTION

IF (@mode = 0) --Insert Mode
BEGIN ---Insert Elevator custom type
	IF NOT EXISTS (
			SELECT 1
			FROM ElevT
			WHERE fDesc = @fdesc
			)
	BEGIN
		SET @equipt = (
				SELECT ISNULL(Max(ID), 0) + 1
				FROM ElevT
				)

		INSERT INTO ElevT (
			ID
			,fDesc
			,Remarks
			,Count
			)
		VALUES (
			@equipt
			,@fdesc
			,@remarks
			,0
			)
	END
	ELSE
	BEGIN
		RAISERROR (
				'Template name already exists, please use different name !'
				,16
				,1
				)

		RETURN
	END

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

	DECLARE @id INT
	DECLARE @ielev INT
	DECLARE @ifdesc VARCHAR(50)
	DECLARE @line INT
	DECLARE @format VARCHAR(50)
	DECLARE @OrderNo INT
	DECLARE @LeadEquip INT

	DECLARE db_cursor CURSOR
	FOR
	SELECT Elev
		,fDesc
		,Line
		,Format
		,OrderNo
		,LeadEquip
	FROM @Items

	OPEN db_cursor

	FETCH NEXT
	FROM db_cursor
	INTO @ielev
		,@ifdesc
		,@line
		,@format
		,@OrderNo
		,@LeadEquip

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @id = (
				SELECT ISNULL(Max(ID), 0) + 1
				FROM ElevTItem
				)

		INSERT INTO ElevTItem (
			ID
			,ElevT
			,Elev
			,CustomID
			,fDesc
			,Line
			,Value
			,Format
			,OrderNo
			,LeadEquip
			)
		SELECT @id
			,@equipt
			,@ielev
			,@id
			,@ifdesc
			,@line
			,NULL
			,@format
			,@OrderNo
			,@LeadEquip

		--Insert Formate DropDown Value if Formate Type is DropDown											 
		INSERT INTO tblCustomValues (
			ElevT
			,ItemID
			,Line
			,Value
			)
		SELECT @equipt
			,@id
			,@line
			,value
		FROM @CustomValues
		WHERE Line = @line

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
		INTO @ielev
			,@ifdesc
			,@line
			,@format
			,@OrderNo
			,@LeadEquip
	END

	CLOSE db_cursor

	DEALLOCATE db_cursor
END
ELSE IF (@mode = 1) --Updatre MODE
BEGIN --Update Elevator custom type 
	IF NOT EXISTS (
			SELECT 1
			FROM ElevT
			WHERE fDesc = @fdesc
				AND ID <> @equipt
			)
	BEGIN
		UPDATE ElevT
		SET fDesc = @fdesc
			,Remarks = @remarks
		WHERE ID = @equipt
	END
	ELSE
	BEGIN
		RAISERROR (
				'Template name already exists, please use different name !'
				,16
				,1
				)

		RETURN
	END

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

	DECLARE db_cursor CURSOR
	FOR
	SELECT ID
		,Elev
		,fDesc
		,Line
		,Format
		,OrderNo
		,LeadEquip
	FROM @Items

	OPEN db_cursor

	FETCH NEXT
	FROM db_cursor
	INTO @id
		,@ielev
		,@ifdesc
		,@line
		,@format
		,@OrderNo
		,@LeadEquip

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE
		FROM tblCustomValues
		WHERE ItemID IN (
				SELECT ID
				FROM @ItemsDeleted
				)

		DELETE
		FROM ElevTItem
		WHERE CustomID IN (
				SELECT ID
				FROM @ItemsDeleted
				)

		IF (@id = 0)
		BEGIN
			SET @id = (
					SELECT ISNULL(Max(ID), 0) + 1
					FROM ElevTItem
					)

			DECLARE @Customid INT

			SET @Customid = @id

			--Insert Elevator Custom Value Cursor
			INSERT INTO ElevTItem (
				ID
				,ElevT
				,Elev
				,CustomID
				,fDesc
				,Line
				,Value
				,Format
				,fExists
				,OrderNo
				,LeadEquip
				)
			SELECT @id
				,@equipt
				,@ielev
				,@Customid
				,@ifdesc
				,@line
				,NULL
				,@format
				,2
				,@OrderNo
				,@LeadEquip

			---------Insert Formate DropDown Value if Formate Type is DropDown						 
			INSERT INTO tblCustomValues (
				ElevT
				,ItemID
				,Line
				,Value
				)
			SELECT @equipt
				,@id
				,@line
				,value
			FROM @CustomValues
			WHERE Line = @line

			DECLARE db_cursor1 CURSOR
			FOR
			SELECT Elev
			FROM (
				SELECT DISTINCT Elev
				FROM ElevTItem
				WHERE Elev <> 0
					AND ElevT = @equipt
				) AS t

			OPEN db_cursor1

			FETCH NEXT
			FROM db_cursor1
			INTO @ielev

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @id = (
						SELECT ISNULL(Max(ID), 0) + 1
						FROM ElevTItem
						)

				INSERT INTO ElevTItem (
					ID
					,ElevT
					,Elev
					,CustomID
					,fDesc
					,Line
					,Value
					,Format
					,OrderNo
					,LeadEquip
					)
				SELECT @id
					,@equipt
					,@ielev
					,@Customid
					,@ifdesc
					,@line
					,NULL
					,@format
					,@OrderNo
					,@LeadEquip

				IF @@ERROR <> 0
					AND @@TRANCOUNT > 0
				BEGIN
					RAISERROR (
							'Error Occured'
							,16
							,1
							)

					ROLLBACK TRANSACTION

					CLOSE db_cursor1

					DEALLOCATE db_cursor1

					RETURN
				END

				FETCH NEXT
				FROM db_cursor1
				INTO @ielev
			END

			CLOSE db_cursor1

			DEALLOCATE db_cursor1

			DECLARE db_cursor2 CURSOR
			FOR
			SELECT LeadEquip
			FROM (
				SELECT DISTINCT LeadEquip
				FROM ElevTItem
				WHERE LeadEquip <> 0
					AND ElevT = @equipt
				) AS t

			OPEN db_cursor2

			FETCH NEXT
			FROM db_cursor2
			INTO @LeadEquip

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @id = (
						SELECT ISNULL(Max(ID), 0) + 1
						FROM ElevTItem
						)

				INSERT INTO ElevTItem (
					ID
					,ElevT
					,Elev
					,CustomID
					,fDesc
					,Line
					,Value
					,Format
					,OrderNo
					,LeadEquip
					)
				SELECT @id
					,@equipt
					,@ielev
					,@Customid
					,@ifdesc
					,@line
					,NULL
					,@format
					,@OrderNo
					,@LeadEquip

				IF @@ERROR <> 0
					AND @@TRANCOUNT > 0
				BEGIN
					RAISERROR (
							'Error Occured'
							,16
							,1
							)

					ROLLBACK TRANSACTION

					CLOSE db_cursor2

					DEALLOCATE db_cursor2

					RETURN
				END

				FETCH NEXT
				FROM db_cursor2
				INTO @LeadEquip
			END

			CLOSE db_cursor2

			DEALLOCATE db_cursor2
		END
		ELSE
		BEGIN
			UPDATE ElevTItem
			SET fDesc = @ifdesc
				,Line = @line
				,Format = @format
				,fExists = 1
				,OrderNo = @OrderNo
			WHERE ID = @id

			DELETE
			FROM tblCustomValues
			WHERE ElevT = @equipt
				AND ItemID = @id

			INSERT INTO tblCustomValues (
				ElevT
				,ItemID
				,Line
				,Value
				)
			SELECT @equipt
				,@id
				,@line
				,value
			FROM @CustomValues
			WHERE ItemID = @id

			UPDATE ElevTItem
			SET fDesc = @ifdesc
				,Line = @line
				,Format = @format
				,OrderNo = @OrderNo
			WHERE Elev <> 0
				AND ElevT = @equipt
				AND CustomID = @id

			UPDATE ElevTItem
			SET fDesc = @ifdesc
				,Line = @line
				,Format = @format
				,OrderNo = @OrderNo
			WHERE LeadEquip <> 0
				AND ElevT = @equipt
				AND CustomID = @id
		END

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
		INTO @id
			,@ielev
			,@ifdesc
			,@line
			,@format
			,@OrderNo
			,@LeadEquip
	END

	CLOSE db_cursor

	DEALLOCATE db_cursor
END
ELSE IF (@mode = 2)
BEGIN
	IF EXISTS (
			SELECT 1
			FROM ElevTItem
			WHERE ElevT = @equipt
				AND Elev <> 0
			)
	BEGIN
		RAISERROR (
				'Template is in use!'
				,16
				,1
				)

		RETURN
	END

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

	DELETE
	FROM ElevT
	WHERE ID = @equipt

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

	DELETE
	FROM ElevTItem
	WHERE ElevT = @equipt
		AND Elev = 0

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

	DELETE
	FROM tblCustomValues
	WHERE ElevT = @equipt
END

COMMIT TRANSACTION
