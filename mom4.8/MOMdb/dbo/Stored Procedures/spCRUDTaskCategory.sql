CREATE PROCEDURE [dbo].[spCRUDTaskCategory] @mode    SMALLINT,
    @ID INT,
    @Name VARCHAR(50),
    @Remarks VARCHAR(MAX),
    @CreatedBy VARCHAR(50),
    @CreatedDate DateTime
AS
-- Add new
DECLARE @currCateName VARCHAR(50) = ISNULL((SELECT Name FROM tblTaskCategory WHERE ID= @ID),'')
IF ( @mode = 0 )
BEGIN
    IF NOT EXISTS(SELECT 1 FROM tblTaskCategory WHERE Name= @Name)
    BEGIN
        INSERT INTO tblTaskCategory 
        VALUES (@Name,@Remarks,@CreatedBy,@CreatedDate)
    END
    ELSE
    BEGIN
        RAISERROR ('Task Category already exist',16,1)

        RETURN
    END
END
-- Edit
ELSE IF( @mode = 1 )
BEGIN
    
    IF @currCateName != ''
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM ToDo WHERE Keyword = @currCateName UNION SELECT 1 FROM Done WHERE Keyword = @currCateName)
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM tblTaskCategory WHERE ID != @ID AND Name = @Name)
            BEGIN
                UPDATE tblTaskCategory
                SET    Name= @Name,Remarks=@Remarks
                WHERE  ID = @ID
            END
            ELSE
            BEGIN
                RAISERROR ('Task Category name is using for another one',16,1)
                RETURN
            END
        END
        ELSE
        BEGIN
            IF(@currCateName = @Name)
            BEGIN
                UPDATE tblTaskCategory
                SET    Remarks=@Remarks
                WHERE  ID = @ID
            END
            ELSE
            BEGIN
                RAISERROR ('Item already in use',16,1)
                RETURN
            END
        END
    END
    ELSE
    BEGIN
        RAISERROR ('Item does not exist',16,1)
        RETURN
    END
END
ELSE IF ( @mode = 2 )
    BEGIN
        IF EXISTS (SELECT 1
                        FROM   tblTaskCategory
                        WHERE  ID = @ID)
        BEGIN
            IF NOT EXISTS(SELECT 1 FROM ToDo WHERE Keyword = @currCateName UNION SELECT 1 FROM Done WHERE Keyword = @currCateName)
            BEGIN
                DELETE FROM tblTaskCategory WHERE  ID = @ID
            END
            ELSE
            BEGIN
                RAISERROR ('Item already in use',16,1)
                RETURN
            END
        END
        ELSE
        BEGIN
            RAISERROR ('Item does not exist',16,1)

            RETURN
        END
    END
GO