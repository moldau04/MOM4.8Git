CREATE PROCEDURE [dbo].[spAddBOMItem] @job int,
@type smallint,
@item int,
@fDesc varchar(255),
@Phase int = 0,
@OpSq varchar(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @jobt int = 0
    DECLARE @line smallint = 1
    DECLARE @JobTItemID int
    DECLARE @MatItem int
    DECLARE @LabItem int

    IF (@Type = 2)
    BEGIN
        SET @LabItem = @Item
        IF (@fDesc IS NULL
            OR @fdesc = '')
        BEGIN
            SET @fDesc = (SELECT
                fdesc
            FROM PRWage
            WHERE id = @Item)
        END
    END
    ELSE
    BEGIN
        SET @MatItem = @Item
        IF (@fDesc IS NULL
            OR @fdesc = '')
        BEGIN
            SET @fDesc = (SELECT
                Name
            FROM Inv
            WHERE id = @Item)
        END
    END
    DECLARE @code varchar(10)
    IF (@OpSq IS NULL)
    BEGIN
        SET @code = (SELECT TOP 1
            code
        FROM JobCode
        WHERE ISNULL(IsDefault, 0) = 1)
    END
    ELSE
    BEGIN
        SET @code = @OpSq
    END

    SET @jobt = ISNULL((SELECT
        Template
    FROM Job
    WHERE ID = @job)
    , 0)

    SET @line = ISNULL((SELECT
        MAX(ISNULL(Line, 0)) + 1
    FROM JobTItem
    WHERE Job = @job)
    , 0)


    IF EXISTS (SELECT
            *
        FROM JobTItem
        WHERE JOB = @job
        AND Line = @Phase)
    BEGIN
        SET @line = @Phase
    END
    ELSE
    BEGIN
    --IF (@type =1 or @type =2)  
    BEGIN
        INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget, Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored)
            VALUES (@jobt, @job, 1, @fdesc, @code, 0, 0, @line, 0, 0, 0, 0, 0, 0, 0)
        SET @jobTItemId = SCOPE_IDENTITY()
    END
        --ELSE  
        --BEGIN  
        --	 --SET @line = isnull((SELECT max(isnull(Line,0)) + 1 from JobTItem WHERE Job = @job and type = 2),0)  
        --	 INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget, Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored)  
        --	 VALUES (@jobt, @job, 2, @fdesc, @code, 0, 0, @line,0, 0, 0, 0, 0, 0, 0)  
        --	 SET @jobTItemId = SCOPE_IDENTITY()  
        --END  



        IF (@type = 1
            OR @type = 2)
        BEGIN
            IF ((SELECT
                    COUNT(*)
                FROM BOM
                WHERE JobTItemID IN (SELECT
                    ID
                FROM JobTItem
                WHERE Job = @job)
                AND Type = @Type
                AND MatItem = @MatItem)
                = 0)
            BEGIN
                INSERT INTO BOM (JobTItemID, Type, MatItem, LabItem)
                    VALUES (@JobTItemID, @type, @MatItem, @LabItem)
            END
            ELSE
            BEGIN
                INSERT INTO BOM (JobTItemID, Type, MatItem, LabItem)
                    VALUES (@JobTItemID, @type, @MatItem, @LabItem)
            END
        END
        ELSE
        BEGIN
            INSERT INTO BOM (JobTItemID, Type, MatItem, LabItem)
                VALUES (@JobTItemID, @type, @MatItem, @LabItem)
        END
    END
    RETURN @line;
END