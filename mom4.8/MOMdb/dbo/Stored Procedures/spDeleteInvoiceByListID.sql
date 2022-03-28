CREATE proc spDeleteInvoiceByListID
@QBInvID varchar(100)

as

declare @InvID int
declare @name varchar(150)

select @InvID = Ref,  @name = Custom1  from Invoice   
WHERE isnull( QBInvoiceID,'')<>'' and  QBInvoiceID = @QBInvID

 if exists(select 1 from Invoice where Ref=@InvID ) 
      begin
      
DELETE FROM invoicei
WHERE  Ref = @InvID

DELETE FROM invoice
WHERE  Ref = @InvID

INSERT INTO tblSyncDeleted 
				(Tbl, 
				 NAME, 
				 RefID, 
				 QBID,
				 DateStamp) 
	 VALUES      ('Invoice', 
				 @name, 
				 @InvID, 
				 @QBInvID,
				 GETDATE() ) 
		
end
