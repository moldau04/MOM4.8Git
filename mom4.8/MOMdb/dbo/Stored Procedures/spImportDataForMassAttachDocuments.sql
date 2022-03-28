CREATE PROCEDURE [dbo].[spImportDataForMassAttachDocuments]
	@MassAttachDocs tblTypeMassAttachDocs readonly 
AS

BEGIN TRY
BEGIN TRANSACTION

	IF NOT EXISTS (SELECT TOP 1 1 FROM tblMassAttachDocuments)
	BEGIN
		INSERT INTO tblMassAttachDocuments (AccountID, [Path], [Filename], FileExt) 
			SELECT AccountID, [Path], [Filename], FileExt FROM @MassAttachDocs md inner join loc l on l.id = md.AccountID

		INSERT INTO [tblMassAttachFailed] (AccountID, [Path], [Filename], FileExt) 
			SELECT AccountID, [Path], [Filename], FileExt FROM @MassAttachDocs md left outer join loc l on l.id = md.AccountID where l.Loc is null

		INSERT INTO Documents (Screen, ScreenID, Line, fDesc, Filename, Path, Type, Remarks)
			SELECT
				'Location',
				l.Loc,
				1,
				md.FileExt,
				md.Filename,
				md.Path,
				(SELECT TOP 1
					ID
				FROM DocType dt
				WHERE dt.fDesc =
				(CASE
					WHEN md.FileExt = 'xlsx' OR
						md.FileExt = 'xls' THEN 'xls'
					WHEN md.FileExt = 'png' OR
						md.FileExt = 'jpg' OR
						md.FileExt = 'bmp' OR
						md.FileExt = 'gif' THEN 'Picture'
					WHEN md.FileExt = 'docx' OR
						md.FileExt = 'doc' THEN 'doc'
					ELSE 'Other'
				END
				)),
				'Mass attach'
			FROM @MassAttachDocs md
			INNER JOIN loc l
				ON l.id = md.AccountID
	
	END
	ELSE
	BEGIN
		RAISERROR ('Mass attach function is already run before. Please check again!',16,1)
	END
	COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()
	DECLARE @error varchar(1000)=(SELECT ERROR_MESSAGE())
    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ( @error,16,1)
        RETURN 
END CATCH 