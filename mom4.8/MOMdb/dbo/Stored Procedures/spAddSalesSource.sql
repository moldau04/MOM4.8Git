
CREATE PROCEDURE [dbo].[spAddSalesSource] 
							     @Type    VARCHAR(25),
                                  @SourceDescription VARCHAR(100),
                                  @mode    SMALLINT,
								  @OldSourceDescription VARCHAR(100)
AS
    IF ( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   SalesSource
                        WHERE  fDesc = @SourceDescription)
            BEGIN
                INSERT INTO SalesSource
                            (type,
                             fDesc)
                VALUES      ( @Type,
                              @SourceDescription )
            END
          ELSE
            BEGIN
                RAISERROR ('SaleSource type already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @mode = 1 )
      BEGIN
          UPDATE SalesSource
          SET    Type = @Type,fDesc = @SourceDescription
          WHERE  fDesc = @OldSourceDescription
      END
    ELSE IF ( @mode = 2 )
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   Prospect
                         WHERE  Source = @SourceDescription)
            BEGIN
                DELETE FROM SalesSource
                WHERE  fDesc = @SourceDescription
            END
          ELSE
            BEGIN
                RAISERROR ('SaleSource exists for the selected Prospect type.',16,1)

                RETURN
            END
      END
