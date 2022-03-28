CREATE PROCEDURE[dbo].[spUpdateBT] @ID int,
								  @Description VARCHAR(MAX),
								  @Count int,
                                  @mode    SMALLINT,
								  @Label varchar(50)
AS
    IF ( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   BusinessType
                        WHERE  Description = @Description)
            BEGIN
                INSERT INTO BusinessType
                            (Description,Count,Label)
                VALUES      (@Description,@Count,@Label)
            END
          ELSE
            BEGIN
                RAISERROR ('This type already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @mode = 1 )
      BEGIN
	   IF NOT EXISTS(SELECT 1  FROM   BusinessType  WHERE  Description = @Description AND ID!=@ID)
	    AND (select count(1) from Prospect where BusinessType in(select Description from BusinessType where ID=@ID))=0
		BEGIN
			 UPDATE BusinessType
			  SET    Description= @Description,Count=@Count,Label=@Label
			  WHERE  ID = @ID
		END
		 ELSE
            BEGIN
                RAISERROR ('This type is in use and cannot be edited.',16,1)

                RETURN
            END

         
      END
    ELSE IF ( @mode = 2 )
      BEGIN
          IF (select count(1) from Prospect
						 where BusinessType in(select Description from BusinessType where ID=@ID))=0
						 AND (select count(1) from Loc
						 where BusinessType =@ID)=0
            BEGIN
                DELETE FROM BusinessType
                WHERE  ID = @ID
            END
         
      END
