CREATE PROCEDURE [dbo].[spUpdateTicketInvoiceEmail]
@EmailID int,
@Type	varchar(20),
@Subject	varchar(50),
@Body	ntext,
@BitMap	bit,
@BodyMulitple	ntext,
@Fields	varchar(50)
As


BEGIN TRANSACTION
  
Update Email
SET
	Type	=	@Type,
	Subject	=	@Subject,
	Body	=	@Body,
	BitMap	=	@BitMap,
	BodyMulitple	=	@BodyMulitple

	Where ID = @EmailID

Update EmailDetails
SET
 Fields	= @Fields
 Where EmailID = @EmailID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
 return (@EmailID)
GO