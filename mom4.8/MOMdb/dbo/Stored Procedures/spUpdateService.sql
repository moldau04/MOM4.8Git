CREATE PROCEDURE [dbo].[spUpdateService] @ID    int,
                                  @Description VARCHAR(MAX),
								  @Count int,
                                  @mode    SMALLINT,
								  @Label varchar(50)
AS
    IF ( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   Service
                        WHERE  Description= @Description)
            BEGIN
                INSERT INTO Service
                            (Description,Count,Label)
                VALUES      ( @Description,@Count,@Label )
            END
          ELSE
            BEGIN
                RAISERROR ('Already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @mode = 1 )
      BEGIN
          UPDATE Service
          SET    Description= @Description,Count=@Count,Label=@Label
          WHERE  ID = @ID
      END
    ELSE IF ( @mode = 2 )
      BEGIN
          IF EXISTS (SELECT 1
                         FROM   Service
                         WHERE  ID = @ID)
            BEGIN
                DELETE FROM Service
                WHERE  ID = @ID
            END
          ELSE
            BEGIN
                RAISERROR ('Does not exist.',16,1)

                RETURN
            END
      END
GO