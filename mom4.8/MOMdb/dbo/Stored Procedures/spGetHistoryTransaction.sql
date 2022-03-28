CREATE PROCEDURE [dbo].[spGetHistoryTransaction]
	@Ref INT,
	@Type INT,
	@owner INT, 
	@loc INT,
    @status VARCHAR(50),
	@transID INT
AS
BEGIN
	Declare @page varchar(100);
	set @page='&page=addcustomer&lid=' + convert(varchar(50),@owner)
	if @loc<>0 
	begin
	set @page='&page=addlocation&lid=' + convert(varchar(50),@loc)
	end
	-- Type: 1 --> Invoice
	IF @Type=1
	BEGIN		

		select  1 as line,fDate as fDate
		,CONVERT(VARCHAR(50), ref) as Ref ,
		case when CONVERT(VARCHAR(200), fDesc)='' then 'Invoice' else CONVERT(VARCHAR(200), fDesc) End as fDesc,
		Total as Amount,
		'Invoice' as Type,
		CONCAT('addinvoice.aspx?uid=',Ref,@page) AS LinkTo 		
		FROM Invoice i
		where Ref=@Ref
		Union			
		Select  2 as line,t.fDate as fDate
		,Case when isnull(rp.CheckNumber,'')='' then  CONVERT(VARCHAR(50), rp.ID) else rp.CheckNumber End as Ref 
			,CONVERT(VARCHAR(200), rp.fDesc) as fDesc
		,t.Amount *(-1) as Amount
		,'Payment' AS Type
		, CONCAT('addreceivepayment.aspx?id=', pd.ReceivedPaymentID,@page) AS LinkTo
		from PaymentDetails pd 
		inner join Trans t on t.ID=pd.TransID 		
		left join ReceivedPayment rp on rp.ID=pd.ReceivedPaymentID
		where pd.InvoiceID=@Ref and IsInvoice=1		
		Union
		select  2 as line,fdate  as fDate, CONVERT(VARCHAR(50), ref) as Ref,CONVERT(VARCHAR(200), fDesc), amount *(1) as Amount, 'Payment' AS Type ,CONCAT('adddeposit.aspx?id=',Ref,@page) AS LinkTo
		FROM Trans
		Where Status in(select Status from Trans where  Ref=@Ref and isnull(status,'')<>'' and isnull(status,'')<>'void' and Type =1)
		and Type=6
		Union
		select  2 as line,fdate  as fDate, CONVERT(VARCHAR(50), ref) as Ref,CONVERT(VARCHAR(200), fDesc), amount *(1) as Amount, 'Invoice' AS Type ,CONCAT('adddeposit.aspx?uid=',Ref,@page) AS LinkTo
		FROM Trans
		Where Status in(select Status from Trans where  Ref=@Ref and isnull(status,'')<>'' and isnull(status,'')<>'void' and Type =1)
		and Type=1 and Ref<>@Ref
    END 
	
	IF @Type=2 -- Payment
	BEGIN
		-- Payment 
		if (select count (1) from OpenAR where Ref=@Ref and Type=2) =0
			BEGIN
			print 'test'
			--payment apply invoice to credit
				select  1 as line,rp.PaymentReceivedDate AS fDate, 
				Case when CheckNumber='' then CONVERT(VARCHAR(50), ID) ELSE CheckNumber END as Ref  ,
				fDesc AS fDesc,Amount*(-1)  AS Amount , 'Payment' AS Type, CONCAT('addreceivepayment.aspx?id=', rp.ID,@page) AS LinkTo,Amount *(-1) AS Original
				from ReceivedPayment rp			
				where ID=@Ref and  rp.Amount<>0
				Union
				select  2 as line,rp.PaymentReceivedDate AS fDate,				
				case p.IsInvoice
				when 0 then (select  Case when CheckNumber='' then CONVERT(VARCHAR(50), ID) ELSE CheckNumber END as Ref  from ReceivedPayment where Id=p.InvoiceID)
				When 1 then CONVERT(VARCHAR(50), p.InvoiceID)
				When 2 then CONVERT(VARCHAR(50), p.InvoiceID)End AS Ref,				
				case p.IsInvoice
				when 0 then 'Payment'
				when 2 then 'Payment'
				When 1 then isnull (convert (varchar(500), 
				(select case when CONVERT(VARCHAR(200), fDesc)='' then 'Invoice' else CONVERT(VARCHAR(200), fDesc) End  
				from Invoice where Ref=p.InvoiceID)),'') End AS fDesc,
				t.Amount AS Amount ,

				case p.IsInvoice
				when 0 then 'Payment'
				When 1 then 'Invoice' 
				When 2 then 'Payment' End AS Type,
				case p.IsInvoice
				when 0 then  CONCAT('addreceivepayment.aspx?id=',p.InvoiceID,@page)
				When 1 then  CONCAT('addinvoice.aspx?uid=',p.InvoiceID,@page) 
				When 2 then  CONCAT('adddeposit.aspx?id=',p.InvoiceID,@page)End  AS LinkTo
				,case p.IsInvoice
							when 0 then isnull((select top 1 Original from OpenAR where Type=2 and Ref=P.InvoiceID) ,0)
							When 1 then  isnull((select top 1 Total  from Invoice where Ref=P.InvoiceID) ,0) 
							when 2 then isnull((select top 1 Original from OpenAR where Type=1 and Ref=P.InvoiceID and TransID=p.RefTranID) ,0) End AS Original
				from PaymentDetails p		
				left join ReceivedPayment rp on rp.ID=p.ReceivedPaymentID
				left join Trans t on t.ID=p.TransID		
				where ReceivedPaymentID=@Ref
				
				
			End
		Else 
		BEGIN
			-- Overload payment
			-- Credit not apply
			if ((select count(1) from OpenAR where Ref=@Ref and Type=2 and Original=Balance) =1
				and (select count(*) from PaymentDetails where ReceivedPaymentID= @Ref )=0
				)
				begin
					SELECT ar.fDate AS fDate,
					Case when CheckNumber='' then CONVERT(VARCHAR(50), rp.ID) ELSE CheckNumber END as Ref,
					rp.fDesc AS fDesc,ar.Original*(-1) AS Amount, 'Payment' AS Type, 
					CONCAT('addreceivepayment.aspx?id=',ar.Ref,@page) AS LinkTo ,0 as Payment
					FROM OpenAR ar
					LEFT JOIN ReceivedPayment rp ON rp.ID=ar.Ref 
					WHERE Ref=@Ref AND type=2
				ENd
			Else
				BEGIN			
		
					--SELECT  1 as line,ar.fDate AS fDate
					--,Case when CheckNumber='' then CONVERT(VARCHAR(50), rp.ID) ELSE CheckNumber END as Ref 
					--,rp.fDesc AS fDesc,ar.Original AS Amount, 'Payment' AS Type, 
					--CONCAT('addreceivepayment.aspx?id=',ar.Ref,@page) AS LinkTo 
					--FROM OpenAR ar
					--LEFT JOIN ReceivedPayment rp ON rp.ID=ar.Ref 
					--WHERE Ref=@Ref AND type=2
					--UNION
                    	select  2 as line,rp.PaymentReceivedDate AS fDate, 
					Case when CheckNumber='' then CONVERT(VARCHAR(50), rp.ID) ELSE CheckNumber END as Ref  ,
					fDesc AS fDesc,Amount *(-1) AS Amount , 'Payment' AS Type, CONCAT('addreceivepayment.aspx?id=', rp.ID,@page) AS LinkTo
					from ReceivedPayment rp			
					where ID =@Ref 
					and rp.Amount<>0
					UNION
                    
					select  3 as line,rp.PaymentReceivedDate AS fDate,CONVERT(VARCHAR(50), p.InvoiceID) AS Ref,
					case p.IsInvoice
					when 0 then 'Payment'
					When 1 then isnull (convert (varchar(500), 
						(select case when CONVERT(VARCHAR(200), fDesc)='' then 'Invoice' else CONVERT(VARCHAR(200), fDesc) End  
				from Invoice where Ref=p.InvoiceID)),'') End AS fDesc,
					t.Amount AS Amount , 
						case p.IsInvoice
						when 0 then 'Payment'
						When 1 then 'Invoice'  End AS Type,
						case p.IsInvoice
						when 0 then  CONCAT('addreceivepayment.aspx?id=',p.InvoiceID,@page)
						When 1 then  CONCAT('addinvoice.aspx?uid=',p.InvoiceID,@page)  End  AS LinkTo							
					from PaymentDetails p		
					left join ReceivedPayment rp on rp.ID=p.ReceivedPaymentID
					left join Trans t on t.ID=p.TransID		
					where ReceivedPaymentID in(select ReceivedPaymentID from PaymentDetails where InvoiceID=@Ref and IsInvoice=0)
					and InvoiceID<>@Ref
					
					UNION
                    
					select  3 as line,rp.PaymentReceivedDate AS fDate,CONVERT(VARCHAR(50), p.InvoiceID) AS Ref,
					case p.IsInvoice
					when 0 then 'Payment'
					When 1 then isnull (convert (varchar(500), 
						(select case when CONVERT(VARCHAR(200), fDesc)='' then 'Invoice' else CONVERT(VARCHAR(200), fDesc) End  
				from Invoice where Ref=p.InvoiceID)),'') End AS fDesc,
					t.Amount AS Amount , 
						case p.IsInvoice
						when 0 then 'Payment'
						When 1 then 'Invoice'  End AS Type,
						case p.IsInvoice
						when 0 then  CONCAT('addreceivepayment.aspx?id=',p.InvoiceID,@page)
						When 1 then  CONCAT('addinvoice.aspx?uid=',p.InvoiceID,@page)  End  AS LinkTo							
					from PaymentDetails p		
					left join ReceivedPayment rp on rp.ID=p.ReceivedPaymentID
					left join Trans t on t.ID=p.TransID		
					where ReceivedPaymentID=@REf
					and InvoiceID<>@Ref
				END
			END

			
		END 


	IF @loc=0
	BEGIN
		IF @Type=3-- Payment
		BEGIN
		if LOWER(@status)='open'
		begin 
			SELECT  1 as line,ar.fDate AS fDate
					,ar.ref as Ref 
					,t.fDesc AS fDesc,ar.Original AS Amount, 'Payment' AS Type, 
					CONCAT('adddeposit.aspx?id==',ar.Ref,@page) AS LinkTo 
					FROM OpenAR ar	
					INNER JOIN tRANS T on t.id=ar.TransID
					WHERE ar.Ref=@Ref AND ar.type=1 AND t.id=@transID
		End
		else
		begin
			select t.fdate, CONVERT(VARCHAR(50),t.ref) AS Ref,Case when t.fDesc='' then 'Invoice' Else t.fDesc End as fdesc, t.Amount, 'Invoice' AS Type , CONCAT('addinvoice.aspx?uid=', t.ref,@page) AS LinkTo 			
			FROM Trans t
			left join Invoice i on i.Ref=t.Ref
			Where t.Status in(select Status from Trans where  Ref=@Ref and Type =6 AND AcctSub IN(SELECT Loc FROM Loc WHERE Owner=@owner))
			and t.Type=1
			Union
			select fdate  as fDate, CONVERT(VARCHAR(50), ref) as Ref,fdesc as fdesc, amount  as Amount, 'Payment' AS Type ,CONCAT('adddeposit.aspx?id=',Ref,@page) AS LinkTo
			FROM Trans
			Where Status in(select Status from Trans where  Ref=@Ref and Type =6 AND AcctSub IN(SELECT Loc FROM Loc WHERE Owner=@owner))
			and Type=6
		end
			
		END
	END
	ELSE
		BEGIN
		

		IF @Type=3-- Payment
		BEGIN
		if LOWER(@status)='open'
		BEGIN    
			SELECT  1 as line,ar.fDate AS fDate
					,ar.ref as Ref 
					,t.fDesc AS fDesc,ar.Original AS Amount, 'Payment' AS Type, 
					CONCAT('adddeposit.aspx?id==',ar.Ref,@page) AS LinkTo 
					FROM OpenAR ar	
					INNER JOIN tRANS T on t.id=ar.TransID
					WHERE ar.Ref=@Ref AND ar.type=1 AND t.id=@transID
		END 
		ELSE 
		BEGIN
		  SELECT t.fdate, CONVERT(VARCHAR(50),t.ref) AS Ref,Case when t.fDesc='' then 'Invoice' Else t.fDesc End as fdesc, t.Amount, 'Invoice' AS Type , CONCAT('addinvoice.aspx?uid=',t.ref,@page) AS LinkTo 			
			FROM Trans t
			left join Invoice i on i.Ref=t.Ref
			Where t.Status in(select Status from Trans where  Ref=@Ref and Type =6 AND AcctSub=@loc)
			and t.Type=1
			Union
			select fdate  as fDate, CONVERT(VARCHAR(50), ref) as Ref,fdesc as fdesc, amount  as Amount, 'Payment' AS Type ,CONCAT('adddeposit.aspx?id=',Ref,@page) AS LinkTo
			FROM Trans
			Where Status in(select Status from Trans where  Ref=@Ref and Type =6 AND  AcctSub=@loc)
			and Type=6
        END

			
		END
	END

	
	
END

