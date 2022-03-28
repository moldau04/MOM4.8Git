
CREATE PROCEDURE [dbo].[spAddUpdateQBLocation] 
	@Account        VARCHAR(50),
    @LocName        VARCHAR(100),
    @Address        VARCHAR(255),
    @status         SMALLINT,
    @City           VARCHAR(50),
    @State          VARCHAR(2),
    @Zip            VARCHAR(10),
    @Remarks        TEXT,
    @ContactName    VARCHAR(50),
    @Phone          VARCHAR(28),
    @fax            VARCHAR(28),
    @Cellular       VARCHAR(28),
    @Email          VARCHAR(50),
    @Owner          INT,
    @RolAddress     VARCHAR(255),
    @RolCity        VARCHAR(50),
    @RolState       VARCHAR(2),
    @RolZip         VARCHAR(10),
    @QBLocationID   VARCHAR(100),
    @QBCustID       VARCHAR(100),
    @LastUpdateDate DATETIME,
    @Type           VARCHAR(50),
    @QBstax         VARCHAR(100),
    @Balance		numeric(30,2)
AS
    DECLARE @Rol INT
	DECLARE @Loc INT

    BEGIN TRANSACTION

    IF NOT EXISTS(SELECT 1
                  FROM   Loc l INNER JOIN Owner o ON l.Owner = o.ID
                  WHERE  l.QBlocid = @QBLocationID OR l.Tag like @LocName + '%')
      BEGIN
          DECLARE @custid VARCHAR(100)

          SELECT TOP 1 @custid = ID
          FROM   Owner
          WHERE  QBCustomerID = @QBCustID

          --IF NOT EXISTS(SELECT 1
          --              FROM   Loc
          --              WHERE  Owner = @custid
          --                     AND @QBCustID = @QBLocationID)
          --  BEGIN
                INSERT INTO Rol
                            (City,
                             State,
                             Zip,
                             Address,
                             GeoLock,
                             Remarks,
                             Type,
                             Contact,
                             Name,
                             Phone,
                             EMail,
                             Cellular,
                             Fax
                --,                       LastUpdateDate
                )
                VALUES      ( @RolCity,
                              @RolState,
                              @RolZip,
                              @RolAddress,
                              0,
                              @Remarks,
                              4,
                              @ContactName,
                              @LocName,
                              @phone,
                              @email,
                              @Cellular,
                              @fax
                --,                        @LastUpdateDate 
                )

                SET @Rol=Scope_identity()

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                --IF NOT EXISTS (SELECT 1
                --               FROM   Loc
                --               WHERE  id = @Account)
                --  BEGIN
                INSERT INTO Loc
                            (ID,
                             Tag,
                             Address,
                             City,
                             State,
                             Zip,
                             Rol,
                             Status,
                             Owner,
                             QBlocid,
                             Type,
                             Remarks,
                             STax,
                             Route,
                             Balance,
							 Credit)
                VALUES      ( @Account,
                              @LocName,
                              @Address,
                              @City,
                              @State,
                              @Zip,
                              @Rol,
                              @status,
                              (SELECT TOP 1 ID
                               FROM   Owner
                               WHERE  QBCustomerID = @QBCustID),
                              @QBLocationID,
                              @Type,
                              @Remarks,
                              (SELECT TOP 1 Name
                               FROM   STax
                               WHERE  QBStaxID = @QBstax) ,
                               (select top 1 ID from Route where Name = (select top 1 fUser from tblUser where DefaultWorker=1) ),
                               @Balance,
							   0
                               )

                --END
                --ELSE
                --  BEGIN
                --      RAISERROR ('Account # already exists, please use different Account # !',16,1)
                --      ROLLBACK TRANSACTION
                --      RETURN
                --  END
                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            --END
      END
    ELSE
      BEGIN
          --SELECT @Rol = Rol
          --FROM   Loc
          --WHERE  QBlocid = @QBLocationID

		  SELECT @Rol = l.Rol, @Loc=l.Loc
          FROM   Loc l INNER JOIN Owner o ON l.Owner = o.ID
          WHERE  l.QBlocid = @QBLocationID OR l.Tag like @LocName + '%'

          DECLARE @lastup INT =0

          UPDATE Rol
          SET    @lastup = 1,
                 Name = @LocName,
                 City = @RolCity,
                 State = @RolState,
                 Zip = @RolZip,
                 Address = @RolAddress,
                 Remarks = @Remarks,
                 Contact = @ContactName,
                 Phone = @phone,
                 EMail = @email,
                 Cellular = @Cellular,
                 Fax = @fax
          --,                 LastUpdateDate = @LastUpdateDate
          WHERE  id = @Rol
                 AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate
                 --AND @QBLocationID <> @QBCustID

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF( @lastup = 1 )
            BEGIN
                UPDATE Loc
                SET    
				       --ID = @Account,
                       Tag = @LocName,
                       Address = @Address,
                       City = @City,
                       State = @State,
                       Zip = @Zip,
                       Status = @status,
                       Owner = (SELECT TOP 1 ID
                                FROM   Owner
                                WHERE  QBCustomerID = @QBCustID),
                       Type = @Type,
                       STax = (SELECT TOP 1 Name
                               FROM   STax
                               WHERE  QBStaxID = @QBstax)	,
                               Balance=@Balance	,
					   QBlocid = @QBLocationID
				WHERE Loc = @Loc
                --WHERE  Loc = (SELECT TOP 1 Loc
                --              FROM   Loc
                --              WHERE  QBlocid = @QBLocationID)

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END

    --    update Control set QBLastSync=GETDATE()
    --IF @@ERROR <> 0
    -- AND @@TRANCOUNT > 0
    --BEGIN
    --	RAISERROR ('Error Occured',16,1)
    --	ROLLBACK TRANSACTION
    --             RETURN
    --         END
    COMMIT TRANSACTION
