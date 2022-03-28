CREATE PROCEDURE [dbo].[spUpdateStageProject] @ID   int,
                                  @Description VARCHAR(MAX),
								  @ChartColor nvarchar(50),
								  @Label varchar(50),
                                  @mode    SMALLINT
AS  
IF ( @mode = 0 )
BEGIN
    IF NOT EXISTS(SELECT 1
            FROM   tblProjectStage
            WHERE  Description = @Description)
    BEGIN
        INSERT INTO tblProjectStage (Description,Label, ChartColors)
        VALUES      ( @Description,@Label, @ChartColor)
    END
    ELSE
    BEGIN
        RAISERROR ('Stage already exists.',16,1)
        RETURN
    END
END
ELSE IF( @mode = 1 )
BEGIN
    UPDATE tblProjectStage
    SET    Description= @Description, ChartColors = @ChartColor
    WHERE  ID = @ID
END
ELSE IF ( @mode = 2 )
BEGIN
    IF EXISTS (SELECT 1
                    FROM   tblProjectStage
                    WHERE  ID = @ID)
    BEGIN
        IF NOT EXISTS (SELECT TOP 1 1 FROM Job WHERE Job.Stage is not null AND Job.Stage = @ID)
        BEGIN
            DELETE FROM tblProjectStage WHERE  ID = @ID
        END
        ELSE
        BEGIN
            RAISERROR ('The selected stage is in use. Cannot be deleted.',16,1)
            RETURN
        END
    END
    ELSE
    BEGIN
        RAISERROR ('Stage not exist.',16,1)
        RETURN
    END
END