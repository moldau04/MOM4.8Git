CREATE PROCEDURE spConvertEstimateDocToProjectDoc
	@Estimate INT,
	@JobID INT
AS
BEGIN
	DECLARE @JfDesc VARCHAR(255), 
		@Line INT,
		@fDesc VARCHAR(500),
		@Filename VARCHAR(1000),
		@Path VARCHAR(1000),
		@Type INT,
		@Remarks VARCHAR(8000),
		@TempID VARCHAR(150),
		@Date DATETIME
	DECLARE db_cursor_Doc CURSOR FOR 
		SELECT [Line],[fDesc],[Filename],[Path],[Type],[Remarks],[TempID],[Date]
		FROM [dbo].[Documents] Where Screen='Estimate' AND ScreenID=@Estimate

    OPEN db_cursor_Doc
	FETCH NEXT FROM db_cursor_Doc INTO
					 @Line, @fDesc, @Filename, @Path, 
					 @Type, @Remarks, @TempID, @Date
	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			INSERT INTO [dbo].[Documents]
								([Screen],[ScreenID],[Line],[fDesc],[Filename],[Path],[Type],[Remarks],[TempID],[Date])
						VALUES ('Project',@JobID, @Line, @fDesc, @Filename, @Path, @Type,@Remarks, @TempID, @Date)

			FETCH NEXT FROM db_cursor_Doc INTO
					 @Line, @fDesc, @Filename, @Path, 
					 @Type, @Remarks, @TempID, @Date
		END
	CLOSE db_cursor_Doc  
	DEALLOCATE db_cursor_Doc
END
GO
