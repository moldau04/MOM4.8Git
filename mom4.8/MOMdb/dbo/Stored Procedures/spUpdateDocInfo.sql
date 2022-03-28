CREATE PROC spUpdateDocInfo 
	@Docs AS [dbo].[tbltypDocs] Readonly,
	@UpdatedBy varchar(100)=NULL
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DECLARE @DocId int
			DECLARE @PortalNew smallint
			DECLARE @RemarksNew varchar(max)
			DECLARE @PortalOld smallint
			DECLARE @RemarksOld varchar(max)
			DECLARE @ScreenId int
			DECLARE @Screen varchar(50)
			DECLARE @DocName varchar(50)
			DECLARE @Field varchar(75)

			DECLARE db_cursor CURSOR FOR
				SELECT tblD.ID, tblD.Portal, tblD.Remarks, Doc.Portal, Doc.Remarks, Doc.ScreenID, Doc.Screen, LEFT(Doc.Filename, 50)
				FROM @Docs tblD INNER JOIN Documents Doc ON tblD.ID = Doc.ID 
				--WHERE tblD.Portal != Doc.Portal OR tblD.Remarks != Doc.Remarks
			OPEN db_cursor FETCH NEXT FROM db_cursor INTO @DocId, @PortalNew, @RemarksNew, @PortalOld, @RemarksOld, @ScreenId, @Screen, @DocName
			WHILE @@FETCH_STATUS = 0
			BEGIN
				UPDATE Documents SET Portal = @PortalNew, Remarks = @RemarksNew WHERE ID = @DocId
				
				IF @Screen = 'PO'
				BEGIN
					IF @PortalOld is null or @PortalNew != @PortalOld
					BEGIN
						SET @Field =   'Doc Portal: '+ @DocName 
						EXEC log2_insert @UpdatedBy,@Screen,@ScreenId, @Field,@PortalOld,@PortalNew
					END
					IF @RemarksOld is null or @RemarksNew != @RemarksOld
					BEGIN
						SET @Field =   'Doc Remarks: '+ @DocName 
						EXEC log2_insert @UpdatedBy,@Screen,@ScreenId, @Field,@RemarksOld,@RemarksNew
					END
				END

				SET @PortalNew=Null; SET @RemarksNew=Null; SET @PortalOld=Null; SET @RemarksOld=Null; SET @DocName=Null;

				FETCH NEXT FROM db_cursor INTO @DocId, @PortalNew, @RemarksNew, @PortalOld, @RemarksOld, @ScreenId, @Screen, @DocName
			END

			CLOSE db_cursor  
			DEALLOCATE db_cursor

		COMMIT 
	END TRY
	BEGIN CATCH

		SELECT ERROR_MESSAGE()

		IF @@TRANCOUNT>0
			ROLLBACK	
			RAISERROR ('An error has occurred on this page.',16,1)
			RETURN

	END CATCH

END
GO

--UPDATE d SET 
--	d.Portal = tblD.Portal,
--	d.Remarks = tblD.Remarks
--FROM Documents d 
--INNER JOIN @Docs tblD ON tblD.ID = d.ID 
