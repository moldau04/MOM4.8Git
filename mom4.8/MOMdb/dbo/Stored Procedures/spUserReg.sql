create proc spUserReg
@str             NVARCHAR(400),
@userlicID       INT,
@userid int

as
 BEGIN TRANSACTION
 
IF @userlicID <> 0
      BEGIN
          IF ( (SELECT Count(1)
                FROM   MSM2_Admin.dbo.tblUserAuth
                WHERE  str = (SELECT str
                              FROM   MSM2_Admin.dbo.tblUserAuth
                              WHERE  ID = @userlicID)) = 1 )
            BEGIN
                INSERT INTO MSM2_Admin.dbo.tbljoinauth
                            (userid,
                             lid,
                             date,
                             status,
                             dbname)
                VALUES      ( @userid,
                              @userlicID,
                              Getdate(),
                              0,
                              (SELECT Db_name()) )

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                UPDATE MSM2_Admin.dbo.tblUserAuth
                SET    str = @str,
                       used = 1,
                       dateupdate = Getdate()
                WHERE  ID = @userlicID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    COMMIT TRANSACTION