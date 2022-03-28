CREATE PROCEDURE [dbo].[spGetInvoicesByListInvoice]
	@Invoice varchar(5000),
	@CheckNumber VARCHAR(200),
	@IsSeparate Bit
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION 
	
Create Table #temp(
	   Owner            INT  ,
	   OwnerName        Varchar(200),
	   LocID             Varchar(max),
	   LocationName      Varchar(max),
	   STax               NUMERIC (30, 2),  --STax
	   Total            NUMERIC (30, 2),	 
	   PrevDueAmount    NUMERIC (30, 2),
	   Amount            NUMERIC (30, 2),	  --Pretax Amount
	   AmountDue         NUMERIC (30, 2),
	   paymentAmt         NUMERIC (30, 2),
	   Invoice           VARCHAR (max),
	   CheckNumber  VARCHAR (200),
	   BatchReceive  VARCHAR (200),
	   OrderNo int identity (1,1),
	   isChecked SMALLINT,
	   RefTranID VARCHAR (max) 
	)
Create Table #tempInvoice(
	   Owner            INT  ,
	   OwnerName        Varchar(200),
	   LocID            Varchar(max),
	   LocationName     Varchar(max),
	   STax             NUMERIC (30, 2),  --STax
	   Total            NUMERIC (30, 2),	  
	   PrevDueAmount    NUMERIC (30, 2),
	   Amount           NUMERIC (30, 2),	  
	   AmountDue        NUMERIC (30, 2),
	   Invoice          VARCHAR (max),
	   RefTranID		VARCHAR (max),
	
	)
	Insert into #tempInvoice (Owner,OwnerName,LocID,LocationName,STax,Total,PrevDueAmount,Amount,AmountDue,Invoice,RefTranID) 
	SELECT DISTINCT 
	l.Owner, 
	(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
	l.ID, 
	l.Tag, 
	 i.STax+ISNULL(i.GTax,0) As STax,
	i.Total,		
	isnull(o.Balance,0) AS PrevDueAmount, 
	i.Amount,
	isnull(o.Balance,0) AS DueAmount,  
	i.Ref
	,i.TransID
								 
FROM   Invoice i 
	INNER JOIN Loc l 
			ON l.Loc = i.Loc 
	LEFT OUTER JOIN tblInvoicePayment ip 
			ON i.ref = ip.ref 
	LEFT OUTER JOIN PaymentDetails pd
			ON pd.InvoiceID = i.Ref 
	LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
			WHERE i.Status NOT IN (1,2) and i.Ref in (select * from dbo.SplitString( @Invoice,','))	
		order by l.Owner

		
	
	DECLARE @c_Owner      int
	DECLARE @t_LocID     Varchar(max)
	DECLARE @t_LocName     Varchar(max)
	DECLARE @t_Invoice     Varchar(max)

	If @IsSeparate= 0
		Begin

		Insert into #temp (Owner,OwnerName,STax,Total,PrevDueAmount,Amount,AmountDue,Invoice,paymentAmt,CheckNumber,BatchReceive,isChecked) 
		select Owner,OwnerName,sum(STax),sum(Total),sum(PrevDueAmount),sum(Amount),sum(AmountDue),'',0,@CheckNumber,'',1  FROM #tempInvoice
		group by  Owner,OwnerName
		
			DECLARE db_cursor CURSOR FOR 
				Select Owner From #temp
			OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO  @c_Owner
			WHILE @@FETCH_STATUS = 0
				BEGIN
				set @t_Invoice =''
				set @t_LocID =''
				set @t_LocName =''
				 -- Get Invoice
				SELECT @t_Invoice = COALESCE(@t_Invoice + ',', '') +  ISNULL(Invoice, '')
				FROM #tempInvoice where Owner=@c_Owner 
				 -- Get LocID
				SELECT @t_LocID = COALESCE(@t_LocID + ';', '') +  ISNULL(LocID, '')
				FROM #tempInvoice where Owner=@c_Owner 
				 -- Get LocationName
				SELECT @t_LocName = COALESCE(@t_LocName + ';', '') +  ISNULL(LocationName, '')
				FROM #tempInvoice where Owner=@c_Owner 

				Update #temp
				set Invoice =SUBSTRING(@t_Invoice, 2, Len(@t_Invoice))
				,LocID =SUBSTRING(@t_LocID, 2, Len(@t_LocID))
				,LocationName =SUBSTRING(@t_LocName, 2, Len(@t_LocName))
				,paymentAmt =AmountDue
				where Owner=@c_Owner 
		
				FETCH NEXT FROM db_cursor INTO  @c_Owner
				END
			CLOSE db_cursor  
			DEALLOCATE db_cursor
		End
	Else
		BEGIN
			Insert into #temp (Owner,OwnerName,STax,Total,PrevDueAmount,Amount,AmountDue,Invoice,paymentAmt,CheckNumber,BatchReceive,isChecked,LocID,LocationName,RefTranID) 
			select Owner,OwnerName,STax,Total,PrevDueAmount,Amount,AmountDue,Invoice,AmountDue,@CheckNumber,Invoice,1,LocID,LocationName,RefTranID from #tempInvoice			
		END		

	 select * from  #temp
	 DROP TABLE IF EXISTS #temp
	 DROP TABLE IF EXISTS #tempInvoice
COMMIT
END TRY
BEGIN CATCH
CLOSE db_cursor  
DEALLOCATE db_cursor 

	  SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
END