
CREATE PROCEDURE [dbo].[spQBCreateInvoiceMapping] @Invoice        AS [dbo].[TBLTYPEQBINVOICES] Readonly,
                                          @fdate          DATETIME,
                                          @Fdesc          TEXT,
                                          @Amount         NUMERIC(30, 2),
                                          @stax           NUMERIC(30, 2),
                                          @total          NUMERIC(30, 2),
                                          @taxRegion      VARCHAR(25),
                                          @taxrate        NUMERIC(30, 4),
                                          @Taxfactor      NUMERIC(30, 2),
                                          @taxable        NUMERIC(30, 2),

                                          --@type smallint,
                                          @job            INT,

                                          --@loc int,
                                          --@terms smallint,
                                          @PO             VARCHAR(25),
                                          @Status         SMALLINT,
                                          @Batch          INT,
                                          @Remarks        TEXT,
                                          @gtax           NUMERIC(30, 2),
                                          @mech           INT,
                                          @TaxRegion2     VARCHAR(25),
                                          @Taxrate2       NUMERIC(30, 4),
                                          @BillTo         VARCHAR(1000),
                                          @Idate          DATETIME,
                                          @Fuser          VARCHAR(50),
                                          @staxI          INT,
                                          @invoiceID      VARCHAR(50),
                                          @QBLOCID        VARCHAR(100),
                                          @QBTERMSID      VARCHAR(100),
                                          @QBjobtypeID    VARCHAR(100),
                                          @QBInvoiceID    VARCHAR(100),
                                          @TicketID       INT,
                                          @LastUpdateDate DATETIME
AS
    DECLARE @Ref INT
    DECLARE @batchid INT
    DECLARE @StaxAmount NUMERIC(30, 2)=0.00

    --if(@staxI=1)
    --begin
    --set @StaxAmount = (@taxrate*@Amount)/100
    SET @StaxAmount = @stax

    --end
    SELECT @batchid = Isnull(Max(Batch), 0) + 1
    FROM   Invoice

    BEGIN TRANSACTION
IF EXISTS(SELECT 1
                  FROM   Loc
                  WHERE  QBLocID = @QBLOCID)
BEGIN

    IF NOT EXISTS(SELECT 1
                  FROM   Invoice
                  WHERE  QBinvoiceID = @QBInvoiceID)
      BEGIN
          SET IDENTITY_INSERT Invoice ON
		  SET @Ref=(SELECT MAX(ISNULL(Ref,0))+ 1 FROM Invoice )
		  INSERT INTO Invoice
                      (Ref,fDate,
                       fDesc,
                       Amount,
                       STax,
                       Total,
                       TaxRegion,
                       TaxRate,
                       TaxFactor,
                       Taxable,
                       Type,
                       Job,
                       Loc,
                       Terms,
                       PO,
                       Status,
                       Batch,
                       Remarks,
                       TransID,
                       GTax,
                       Mech,
                       TaxRegion2,
                       TaxRate2,
                       BillTo,
                       IDate,
                       fUser,
                       Custom1,
                       Qbinvoiceid)
          SELECT @Ref,@fDate,
                 @fDesc,
                 @Amount,
                 @StaxAmount,
                 @Amount + @StaxAmount,
                 @TaxRegion,
                 @TaxRate,
                 @TaxFactor,
                 @Taxable,
                 Isnull((SELECT top 1 ID
                         FROM   JobType
                         WHERE  QBJobTypeID = @QBjobtypeID), 0),
                 @Job,
                 (SELECT top 1 Loc
                  FROM   Loc
                  WHERE  QBLocID = @QBLOCID),
                 (SELECT top 1 ID
                  FROM   tblTerms
                  WHERE  QBTermsID = @QBTERMSID),
                 @PO,
                 @Status,
                 @batchid,
                 @Remarks,
                 @batchid,
                 @GTax,
                 @Mech,
                 @TaxRegion2,
                 @TaxRate2,
                 @BillTo,
                 @IDate,
                 @fUser,
                 @invoiceID,
                 @QBInvoiceID

         -- SET @Ref = Scope_identity()
		   SET IDENTITY_INSERT Invoice OFF
          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO InvoiceI
                      (Ref,
                       Line,
                       Acct,
                       Quan,
                       fDesc,
                       Price,
                       Amount,
                       STax,
                       Job,
                       jobitem,
                       TransID,
                       Measure,
                       Disc,
                       StaxAmt)
          SELECT @Ref,
                 Line,
                 (SELECT top 1 id
                  FROM   inv i
                  WHERE  i.QBInvID = il.QBInvID),
                 Quan,
                 fDesc,
                 Price,
                 Amount,
                 STax,
                 Job,
                 JobItem,
                 @batchid,
                 Measure,
                 Disc,
                 StaxAmt
          FROM   @Invoice il

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF( @TicketID <> 0
              AND @TicketID IS NOT NULL )
            BEGIN
                UPDATE TicketD
                SET    Invoice = @Ref,
                       Charge = 0
                WHERE  ID = @TicketID
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END
    ELSE
      BEGIN
          DECLARE @InvID INT
          DECLARE @updated INT = 0

          SELECT @InvID = Ref,
                 @batchid = Batch
          FROM   Invoice
          WHERE  QBInvoiceID = @QBInvoiceID

          UPDATE Invoice
          SET    @updated = 1,
                 fDate = @fDate,
                 fDesc = @fDesc,
                 Amount = @Amount,
                 STax = @StaxAmount,
                 Total = @Amount + @StaxAmount,
                 TaxRegion = @TaxRegion,
                 TaxRate = @TaxRate,
                 TaxFactor = @TaxFactor,
                 Taxable = @Taxable,
                 Type = Isnull((SELECT top 1 ID
                                FROM   JobType
                                WHERE  QBJobTypeID = @QBjobtypeID), 0),
                 Job = @Job,
                 Loc = (SELECT top 1 Loc
                        FROM   Loc
                        WHERE  QBLocID = @QBLOCID),
                 Terms = (SELECT top 1 ID
                          FROM   tblTerms
                          WHERE  QBTermsID = @QBTERMSID),
                 PO = @PO,
                 Status = @Status,
                 Remarks = @Remarks,
                 Mech = @Mech,
                 BillTo = @BillTo,
                 IDate = @Idate,
                 fUser = @fUser,
                 Custom1 = @invoiceID
          WHERE  QBInvoiceID = @QBInvoiceID
                 AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF( @updated = 1 )
            BEGIN
                DELETE FROM InvoiceI
                WHERE  Ref = @InvID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                INSERT INTO InvoiceI
                            (Ref,
                             Line,
                             Acct,
                             Quan,
                             fDesc,
                             Price,
                             Amount,
                             STax,
                             Job,
                             jobitem,
                             TransID,
                             Measure,
                             Disc,
                             StaxAmt)
                SELECT @InvID,
                       Line,
                       (SELECT top 1 id
                        FROM   inv i
                        WHERE  i.QBInvID = il.QBInvID),
                       Quan,
                       fDesc,
                       Price,
                       Amount,
                       STax,
                       Job,
                       JobItem,
                       @batchid,
                       Measure,
                       Disc,
                       StaxAmt
                FROM   @Invoice il

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

    SELECT @Ref
