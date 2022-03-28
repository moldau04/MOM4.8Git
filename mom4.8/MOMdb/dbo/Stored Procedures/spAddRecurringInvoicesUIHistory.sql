CREATE PROCEDURE [dbo].[spAddRecurringInvoicesUIHistory]
	@TaxType varchar(255),
	@IsCanadaCompany bit NULL,
	@Taxable bit	NULL,
	@PaymentTerms int,
	@Remarks varchar(Max),
	@UpdatedBy varchar(50)
as
BEGIN
	DECLARE @old_TaxType varchar(255)
	DECLARE @old_IsCanadaCompany bit 
	DECLARE @old_Taxable bit	
	DECLARE @old_PaymentTerms varchar(100)
	DECLARE @current_PaymentTerms varchar(100)
	DECLARE @old_Remarks varchar(Max)
	Declare @Val varchar(1000)

	Set @current_PaymentTerms = (select (case @PaymentTerms when 0 then 'Upon Receipt'
		 when 1 then 'Net 10 Days'
		 when 2 then 'Net 15 Days'
		 when 3 then 'Net 30 Days'
		 when 4 then 'Net 45 Days'
		 when 5 then 'Net 60 Days'
		 when 6 then '2%-10/Net 30 Days'
		 when 7 then 'Net 90 Days'
		 when 8 then 'Net 180 Days'
		 when 9 then 'COD'
		 end ))

	SELECT @old_TaxType=TaxType, @old_IsCanadaCompany=IsCanadaCompany,@old_Taxable=Taxable,
		 @old_PaymentTerms=(case PaymentTerms when 0 then 'Upon Receipt'
		 when 1 then 'Net 10 Days'
		 when 2 then 'Net 15 Days'
		 when 3 then 'Net 30 Days'
		 when 4 then 'Net 45 Days'
		 when 5 then 'Net 60 Days'
		 when 6 then '2%-10/Net 30 Days'
		 when 7 then 'Net 90 Days'
		 when 8 then 'Net 180 Days'
		 when 9 then 'COD'
		 end ),
		 @old_Remarks=Remarks FROM RecurringInvoicesUIHistory	

		if (select count(*) from RecurringInvoicesUIHistory)=0
			BEGIN
				Insert into RecurringInvoicesUIHistory (TaxType ,IsCanadaCompany,Taxable,PaymentTerms,Remarks)
				values(@TaxType ,@IsCanadaCompany,@Taxable,@PaymentTerms,@Remarks)

		
			exec log2_insert @UpdatedBy,'RecurringInvoices',1,'TaxType',@old_TaxType,@TaxType	
		
			exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Taxable',@old_Taxable,@Taxable	

		
			exec log2_insert @UpdatedBy,'RecurringInvoices',1,'PaymentTerms',@old_PaymentTerms,@current_PaymentTerms			
		
			exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Remarks',@old_Remarks,@Remarks
			
		END
	ELSE
	BEGIN
		Update  RecurringInvoicesUIHistory
		SET  TaxType=@TaxType ,
		 IsCanadaCompany=@IsCanadaCompany,
		 Taxable=@Taxable,
		 PaymentTerms=@PaymentTerms,
		 Remarks=@Remarks
		

		
		set @Val=null
		IF(@Taxable is not null)
		BEGIN
			Set @Val =(select Top 1 newVal  from log2 where screen='RecurringInvoices' and ref= 1 and Field='Taxable' order by CreatedStamp desc )
			if(@Val<>@Taxable)
			begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Taxable',@Val,@Taxable
			end
			Else IF (@old_Taxable <> @Taxable)
			Begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Taxable',@old_Taxable,@Taxable
			END
		END

		set @Val=null
		IF(@current_PaymentTerms is not null)
		BEGIN
			Set @Val =(select Top 1 newVal  from log2 where screen='RecurringInvoices' and ref= 1 and Field='PaymentTerms' order by CreatedStamp desc )
			if(@Val<>@current_PaymentTerms)
			begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'PaymentTerms',@Val,@current_PaymentTerms
			end
			Else IF (@old_PaymentTerms <> @current_PaymentTerms)
			Begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'PaymentTerms',@old_PaymentTerms,@current_PaymentTerms
			END
		END

		set @Val=null
		IF(@Remarks is not null)
		BEGIN
			Set @Val =(select Top 1 newVal  from log2 where screen='RecurringInvoices' and ref= 1 and Field='Remarks' order by CreatedStamp desc )
			if(@Val<>@Remarks)
			begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Remarks',@Val,@Remarks
			end
			Else IF (@old_Remarks <> @Remarks)
			Begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'Remarks',@old_PaymentTerms,@Remarks
			END
		END
	
		set @Val=null
		IF(@TaxType is not null)
		BEGIN
			Set @Val =(select Top 1 newVal  from log2 where screen='RecurringInvoices' and ref= 1 and Field='TaxType' order by CreatedStamp desc )
			if(@Val<>@TaxType)
			begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'TaxType',@Val,@TaxType
			end
			Else IF (@old_TaxType <> @TaxType)
			Begin
				exec log2_insert @UpdatedBy,'RecurringInvoices',1,'TaxType',@old_TaxType,@TaxType
			END
		END

	END


	/********Start Logs************/


END
