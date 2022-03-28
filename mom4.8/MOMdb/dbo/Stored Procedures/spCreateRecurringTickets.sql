Create PROCEDURE [dbo].[spCreateRecurringTickets] @RecurringTicket AS [dbo].[TBLTYPERECURRINGTICKET] Readonly,
                                                 @RemarksOpt      VARCHAR(255),
                                                 @JobRemarksOpt   INT,
                                                 @ProcessPeriod   VARCHAR(75),
                                                 @LastProcessedBy VARCHAR(50)
AS
    DECLARE @LocID INT
    DECLARE @LocTag VARCHAR(50)
    DECLARE @LocAdd VARCHAR(100)
    DECLARE @City VARCHAR(50)
    DECLARE @State VARCHAR(2)
    DECLARE @Zip VARCHAR(100)
    DECLARE @Phone VARCHAR(28)
    DECLARE @Cell VARCHAR(50)
    DECLARE @Worker VARCHAR(50)
    DECLARE @CallDt DATETIME
    DECLARE @SchDt DATETIME
    DECLARE @Status SMALLINT
    DECLARE @Category VARCHAR(25)
    DECLARE @Unit INT
    DECLARE @custID INT
    DECLARE @JobRemarks VARCHAR(max)
    DECLARE @Remarks VARCHAR(255)
    DECLARE @job INT
    DECLARE @ESt FLOAT
    DECLARE @cat VARCHAR(25)
	DECLARE @Throw VARCHAR(max)

    IF NOT EXISTS(SELECT TOP 1 Type
                  FROM   Category
                  WHERE  ISDefault = 1)
      BEGIN
          RAISERROR ('Default category not set, tickets can not be processed!',16,1)

          RETURN
      END

    SELECT TOP 1 @cat = Isnull(Type, 'Recurring')
    FROM   Category
    WHERE  ISDefault = 1

    SET @cat = Isnull(@cat, 'Recurring')

    DECLARE db_cursor CURSOR FOR
      SELECT *
      FROM   @RecurringTicket

    OPEN db_cursor

    FETCH NEXT FROM db_cursor INTO @locid,
                                   @LocAdd,
                                   @City,
                                   @State,
                                   @Zip,
                                   @CallDt,
                                   @SchDt,
                                   @Status,
                                   @Worker,
                                   @Category,
                                   @Unit,
                                   @custID,
                                   @JobRemarks,
                                   @Remarks,
                                   @job,
                                   @est

    WHILE @@FETCH_STATUS = 0
      BEGIN
        
          --BEGIN TRANSACTION

          UPDATE Job
          SET    Custom19 = CONVERT(VARCHAR(50), Getdate(), 121),
                 Custom16 = @ProcessPeriod,
                 LastProcessed = @LastProcessedBy
          WHERE  ID = @job

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                --ROLLBACK TRANSACTION

                CLOSE db_cursor

                DEALLOCATE db_cursor

                RETURN
            END

          --end
          IF( @JobRemarksOpt = 0 )
            BEGIN
                SET @JobRemarks=NULL
            END

          DECLARE @recurring DATETIME

          SET @recurring =CONVERT(DATETIME, CONVERT(DATE, isnull(@SchDt,@CallDt)));

          DECLARE @Equipments TBLTYPEMULTIPLEEEQUIPMENTS

		   if (ISNULL(@Unit,0) > 0 )
          begin
          INSERT INTO @Equipments
                      (ticket_id,
                       elev_id,
                       labor_percentage)
          SELECT 0,
                 @Unit,
                 0
          END
		  ELSE
		  BEGIN
		   INSERT INTO @Equipments
                      (ticket_id,
                       elev_id,
                       labor_percentage)
          SELECT 0,
                 Elev.ID,
                 0 FROM Elev 
				 inner join tblJoinElevJob on tblJoinElevJob.Elev=Elev.ID
				 where tblJoinElevJob.Job=@job and Elev.status=0
		  END
          SET @CallDt=Getdate()

          BEGIN Try------------	AddTicket	Try			            
              EXEC Spaddticket
                @locid,
                NULL,
                @locAdd,
                @City,
                @State,
                @Zip,
                NULL,
                NULL,
                @Worker,
                @CallDt,
                @SchDt,
                @Status,
                NULL,
                NULL,
                NULL,
                @cat,
                @Unit,
                @RemarksOpt,
                NULL,
                @custID,
                @ESt,
                NULL,
                0,
                NULL,
                @LastProcessedBy,
                NULL,
                0.00,
                0.00,
                0.00,
                0.00,
                0.00,
                0.00,
                0,
                0,
                @JobRemarks,
                10,
                0,
                @job,
                '',
                '',
                '',
                '',
                '',
                0,
                0,
                '',
                1,
                0,
                0,
                0,
                0,
                0,
                1,
                '',
                0,
                0,
                0,
                '',
                1,
                NULL,
                NULL,
                '',
                '',
                '',
                '',
                '',
                '',
                '',
                '',
                '',
                0,
                NULL,
                NULL,
                NULL,
                NULL,
                '',
                @recurring,
                @Equipments
          END Try---------------AddTicket   Try
          BEGIN Catch------------AddTicket Catch
              set @Throw =ERROR_MESSAGE()
              --ROLLBACK TRANSACTION
              CLOSE db_cursor
              DEALLOCATE db_cursor
               RAISERROR (@Throw,16,1)
              RETURN
          END Catch------------AddTicket   Catch

          DELETE FROM @Equipments

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                --ROLLBACK TRANSACTION

                CLOSE db_cursor

                DEALLOCATE db_cursor

                RETURN
            END

         -- COMMIT TRANSACTION

          FETCH NEXT FROM db_cursor INTO @locid,
                                         @LocAdd,
                                         @City,
                                         @State,
                                         @Zip,
                                         @CallDt,
                                         @SchDt,
                                         @Status,
                                         @Worker,
                                         @Category,
                                         @Unit,
                                         @custID,
                                         @JobRemarks,
                                         @Remarks,
                                         @job,
                                         @est
      END

    CLOSE db_cursor

    DEALLOCATE db_cursor






