/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 25 Dec Nov 2018	
Description: Add Type, Probability, ChartColor
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spUpdateStage] @ID   int,
                                  @Description VARCHAR(MAX),
								  @Type nvarchar(50),
								  @Probability nvarchar(50),
								  @ChartColor nvarchar(50),
								  @Count int,
								  @Label varchar(50),
                                  @mode    SMALLINT
AS Select * from Stage
    IF ( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   Stage
                        WHERE  Description = @Description)
            BEGIN
                INSERT INTO Stage
                            (Description,Count,Label, Type, Probability, [Chart Colors] )
                VALUES      ( @Description,@Count,@Label, @Type, @Probability, @ChartColor)
            END
          ELSE
            BEGIN
                RAISERROR ('Stage already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @mode = 1 )
      BEGIN
          UPDATE Stage
          SET    Description= @Description,Count=@Count,Label=@Label, Type=@Type, Probability = @Probability, [Chart Colors] = @ChartColor
          WHERE  ID = @ID
      END
    ELSE IF ( @mode = 2 )
      BEGIN
          IF EXISTS (SELECT 1
                         FROM   Stage
                         WHERE  ID = @ID)
            BEGIN
                DELETE FROM Stage
                WHERE  ID = @ID
            END
          ELSE
            BEGIN
                RAISERROR ('Stage not exist.',16,1)

                RETURN
            END
      END