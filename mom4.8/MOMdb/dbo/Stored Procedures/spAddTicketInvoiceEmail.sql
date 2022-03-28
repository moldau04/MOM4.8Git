CREATE PROCEDURE [dbo].[spAddTicketInvoiceEmail]

@Type	varchar(20),
@Subject	varchar(50),
@Body	ntext,
@BitMap	bit,
@BodyMulitple	ntext,
@Fields	varchar(50)
As

declare @EmailID int

begin

BEGIN TRANSACTION
  
Insert into Email
(
Type,Subject,Body,BitMap,BodyMulitple)
values
(
@Type,@Subject,@Body,@BitMap,@BodyMulitple)
set @EmailID=SCOPE_IDENTITY()
Insert into EmailDetails
(EmailID, Fields)
Values(@EmailID, @Fields)
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
   end  
 return (@EmailID)
GO