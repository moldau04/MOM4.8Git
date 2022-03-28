
CREATE PROCEDURE [dbo].[spQBAddLocationMapping] @Account        VARCHAR(50),
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
                                        @Balance		numeric(30,2),
                                        @QBacctID varchar(100)
AS
    DECLARE @Rol INT
	DECLARE @lastup INT =0
    BEGIN TRANSACTION
if(@QBacctID not in ('0', 'MOM'))
      BEGIN
          SELECT @Rol = Rol
          FROM   Loc
          WHERE  Loc = @QBacctID


		  if(isnull(@Rol,0)<> 0)
			begin

          --DECLARE @lastup INT =0

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
                               QBLocID=@QBLocationID				                           
                WHERE  Loc = @QBacctID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END

	END
	
	--ELSE

if(@QBacctID = '0')
	BEGIN
	SELECT @Rol = Rol
          FROM   Loc
          WHERE  QBLocID = @QBLocationID
	

	if(isnull(@Rol,0)<> 0)
	begin

          --DECLARE @lastup1 INT =0

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
                SET    ID = @Account,
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
                               QBLocID=@QBLocationID				                           
                WHERE  QBLocID = @QBLocationID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END


	END

	

if(@QBacctID='MOM')
begin
 IF NOT EXISTS(SELECT 1
                  FROM   Loc
                  WHERE  QBlocid = @QBLocationID)
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
                             Fax,
                             LastUpdateDate
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
                              @fax,
                              getdate() 
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
                             Balance)
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
                               isnull((select top 1 ID from Route where Name = (select top 1 fUser from tblUser where DefaultWorker=1) ),0),
                               @Balance
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
          SELECT @Rol = Rol
          FROM   Loc
          WHERE  QBlocid = @QBLocationID

          set @lastup  =0

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
                 Fax = @fax,
                 LastUpdateDate = getdate()
          WHERE  id = @Rol
                -- AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate
                 ----AND @QBLocationID <> @QBCustID

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
                SET    ID = @Account,
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
                               Balance=@Balance					                           
                WHERE  QBlocid = @QBLocationID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END
END
   
   
    COMMIT TRANSACTION
