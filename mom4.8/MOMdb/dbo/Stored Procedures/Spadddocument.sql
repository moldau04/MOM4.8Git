CREATE PROCEDURE [dbo].[Spadddocument] 
	@screen   VARCHAR(20),
    @TicketID INT,
    @filename VARCHAR(1000),
    @Path     VARCHAR(1000),
    @Type     VARCHAR(10),
    @TempID   VARCHAR(150),
    @subject  VARCHAR(70),
    @body     VARCHAR(250),
    @mode     SMALLINT,
    @id       INT,
	--@portal	  SMALLINT = 0,
	--@remarks  VARCHAR(MAX),
	@UpdatedBy varchar(100)
AS
    IF ( @Type = 'xlsx' )
    BEGIN
        SET @Type='xls'
    END
    ELSE IF( @Type = 'docx' )
    BEGIN
        SET @Type='doc'
    END
    ELSE IF( @Type = 'png'
         OR @Type = 'jpg'
         OR @Type = 'bmp'
         OR @Type = 'gif' )
    BEGIN
        SET @Type='Picture'
    END

    DECLARE @DocType SMALLINT

    SELECT @DocType = ID
    FROM   doctype
    WHERE  fdesc LIKE @Type + '%'

    IF ( @DocType IS NULL )
    BEGIN
        SET @DocType=(SELECT ID
                    FROM   doctype
                    WHERE  fdesc = 'other')
    END

    IF( @mode = 0 )
    BEGIN
        INSERT INTO documents
                    (screen,
                    screenid,
                    line,
                    filename,
                    path,
                    type,
                    tempid,
                    [subject],
                    body--,
					--Portal,
					--Remarks
					)
        VALUES      ( @screen,
                    @TicketID,
                    1,
                    @filename,
                    @Path,
                    @DocType,
                    @TempID,
                    @subject,
                    @body--,
					--@portal,
					--@remarks
					)
		IF @screen = 'PO'
		BEGIN
			EXEC log2_insert @UpdatedBy,@screen,@TicketID,'Add document: Name','',@filename
			--EXEC log2_insert @UpdatedBy,@screen,@TicketID,'Add document: Path','',@Path
			--EXEC log2_insert @UpdatedBy,@screen,@TicketID,'Add document: Portal','',@portal
			--IF @remarks is not null or @remarks != ''
			--BEGIN
			--	EXEC log2_insert @UpdatedBy,@screen,@TicketID,'Add document: Remarks','',@remarks
			--END
		END
    END
    ELSE IF( @mode = 1 )
    BEGIN
		IF(@filename='')
		BEGIN
			SELECT @filename= filename , @Path =Path, @DocType=Type  FROM Documents WHERE ID=@id
		END
      
        UPDATE Documents
        SET    filename = @filename,
                path = @Path,
                Type = @DocType,
                [subject] = @subject,
                body = @body
        WHERE  ID = @id
    END 
