
CREATE PROCEDURE [dbo].[SpUpdateACHpaymentHistry] 
                                     @InvoiceID     INT,                                     
                                     @CustomerID    INT,
                                     @Responsestatus        nvarchar(100),
                                     @PaymentUID    nvarchar(500),
                                     @Status int
AS
    BEGIN TRANSACTION
                 UPDATE tblPaymentHistory              
                 SET Response=@Responsestatus,                  
                 Approved=@Responsestatus 
                 WHERE PaymentUID=@PaymentUID and InvoiceID=@InvoiceID 

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
	
	DECLARE @balance MONEY=0
	Declare @IsSuccess int =0;
	if(@Responsestatus='Approved')set @IsSuccess=1;
	Declare @Amount MONEY;	 
	declare @TransDate DATETIME;
	-----------------------------------------
	 select @TransDate=TransDate,@Amount=Amount
	 From tblPaymentHistory
	 WHERE PaymentUID=@PaymentUID and InvoiceID=@InvoiceID 
	 ----------------------------------------
    IF( @IsSuccess = 1 )
      BEGIN
          
          DECLARE @paid SMALLINT = 0
          DECLARE @paidAmt MONEY=0

          SET @paidAmt= ( Isnull((SELECT balance FROM tblinvoicepayment WHERE ref=@InvoiceID), 0) + @Amount )
          SET @balance = (SELECT Isnull(total, 0)
                          FROM   Invoice
                          WHERE  Ref = @InvoiceID) - @paidAmt

          IF( @balance <= 0 )
            BEGIN
                SET @paid=1
            END

          IF( @Status != 5 )
            BEGIN
                UPDATE Invoice
                SET    Status = @paid,
                       --paid = 1,
                       Remarks = CONVERT(VARCHAR(max), Remarks)+ Char(13) + Char(10)+
                       CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + '' + '    Amount $'+ CONVERT(VARCHAR(50), @Amount) ,
                       
                       fdesc = CONVERT(VARCHAR(max), fDesc)+ Char(13) + Char(10)+
                       CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + '' + '    Amount $'+ CONVERT(VARCHAR(50), @Amount) 
                      
                WHERE  Ref = @InvoiceID
            END
          ELSE
            BEGIN
                UPDATE Invoice
                SET    --paid = 1,
                Remarks =  CONVERT(VARCHAR(max), Remarks) + Char(13) + Char(10) +
                 CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + '' + '    Amount $'+ CONVERT(VARCHAR(50), @Amount), 
                 
                fdesc = CONVERT(VARCHAR(max), fDesc)+ Char(13) + Char(10)+
                 CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + '' + '    Amount $'+ CONVERT(VARCHAR(50), @Amount) 
                 
                WHERE  Ref = @InvoiceID
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF NOT EXISTS(SELECT 1
                        FROM   tblinvoicepayment
                        WHERE  ref = @InvoiceID)
            BEGIN
                INSERT INTO tblinvoicepayment
                            (ref,
                             paid,
                             balance)
                VALUES      ( @invoiceid,
                              @paid,
                              @paidAmt)
            END
          ELSE
            BEGIN
                UPDATE tblinvoicepayment
                SET    paid = @paid,
                       balance = @paidAmt
                WHERE  ref = @InvoiceID
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