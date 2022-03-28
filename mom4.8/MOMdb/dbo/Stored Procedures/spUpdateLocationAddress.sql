CREATE PROC [dbo].[spUpdateLocationAddress] 
@LocAdd VARCHAR(255),
@City   VARCHAR(50),
@State  VARCHAR(2),
@zip    VARCHAR(10),
@lat    VARCHAR(50),
@Lng    VARCHAR(50),
@locID  INT,
@RolID  INT
AS
    BEGIN TRANSACTION

    UPDATE Loc
    SET    Address = @LocAdd,
           City = @City,
           State = @State,
           Zip = @Zip
    WHERE  Loc = @locID

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    UPDATE Rol
    SET    Lat = @lat,
           Lng = @Lng
    WHERE  ID = @RolID

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    COMMIT TRANSACTION