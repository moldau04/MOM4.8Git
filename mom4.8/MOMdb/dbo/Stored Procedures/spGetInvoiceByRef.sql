CREATE PROCEDURE [dbo].[spGetInvoiceByRef]
	@Ref INT
AS
BEGIN
	 SELECT i.*, 
                   JobDecs = (SELECT fDesc FROM Job AS j WHERE j.ID = i.Job),
                   JobRemarks = (SELECT remarks FROM Job AS j WHERE j.ID = i.Job),
                   SPHandle = (SELECT SPHandle FROM Job AS j WHERE j.ID = i.Job),
                   SRemarks = (SELECT SRemarks FROM Job AS j WHERE j.ID = i.Job),
                   InvServ = (SELECT GLRev FROM Job AS j WHERE j.ID = i.Job),          
                   Isnull(i.Stax,0)+Isnull(i.GTax,0) As TotalTax, 
                   (SELECT TOP 1 Name 
                    FROM   rol 
                    WHERE  ID = (SELECT TOP 1 Rol 
                                 FROM   Owner 
                                 WHERE  ID = l.Owner)) AS customerName, 
                   (SELECT TOP 1 Contact 
                    FROM   rol 
                    WHERE  ID = (SELECT TOP 1 Rol 
                                 FROM   Owner 
                                 WHERE  ID = l.Owner)) AS Contact, 
                   (SELECT TOP 1 Phone 
                    FROM   rol 
                    WHERE  ID = (SELECT TOP 1 Rol 
                                 FROM   Owner 
                                 WHERE  ID = l.Owner)) AS Phone, 
                  l.Custom12 AS EMail, 
                  l.Custom13 AS CCEMail, 
                   l.tag                               AS locname, 
                   l.owner, 
                   l.Address, 
                   (CASE i.status 
                     WHEN 0 THEN 'Open' 
                     WHEN 1 THEN 'Paid' 
                     WHEN 2 THEN 'Voided' 
                     WHEN 4 THEN 'Marked as Pending' 
                     WHEN 5 THEN 'Paid by Credit Card' 
                     WHEN 3 THEN 'Partially Paid' 
                   END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, 
                   (SELECT fdesc 
                    FROM   tblWork 
                    WHERE  ID = i.Mech)                AS MechName, 
                   (SELECT Type 
                    FROM   JobType jt 
                    WHERE  jt.ID = i.Type)             AS typeName, 
                   CASE i.Terms 
                     WHEN 0 THEN 'Upon Receipt' 
                     WHEN 1 THEN 'Net 10 Days' 
                     WHEN 2 THEN 'Net 15 Days' 
                     WHEN 3 THEN 'Net 30 Days' 
                     WHEN 4 THEN 'Net 45 Days' 
                     WHEN 5 THEN 'Net 60 Days' 
                     WHEN 6 THEN '2%-10/Net 30 Days' 
                     WHEN 7 THEN 'Net 90 Days' 
                     WHEN 8 THEN 'Net 180 Days' 
                     WHEN 9 THEN 'COD' 
                   END                                 AS termsText, 
                   isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, 
                  convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, 
                  convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid 

           
                     , l.ID as LocID   
                     , l.custom12 as EmailTo   
                     , l.custom13 as EmailCC   
                     ,  isnull(j.Status, 0) as jobStatus   
                     ,  isnull(l.Status, 0) as locStatus   

          
            FROM   Invoice i 
          
                   INNER JOIN Loc l 
                           ON l.Loc = i.Loc 
                   LEFT JOIN Job j 
                           ON j.ID = i.Job 

            WHERE  Ref = @Ref


      
             
            SELECT i.Ref, 
                   i.Line, 
                   i.Acct, 
                   i.Quan, 
                   i.fDesc, 
                   i.Price, 
                   case isnull(i.stax,0) when 1 then Convert(numeric(30,2),( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) + ( ( ( i.Quan * i.Price ) * isnull(inv.GSTRate,0) ) / 100 )) else convert(numeric(30,2),( i.Quan * i.Price )) end AS Amount, 
                   i.STax,        

                   i.Job, 
                   i.JobItem, 
                   i.TransID, 
                   i.Measure, 
                   i.Disc, 
             i.JobOrg, 
                   case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end   AS StaxAmt, 
                   isnull(i.GstAmount,0) As GTaxAmt,  
                    isnull((case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end),0) +        (case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end) As TotalTax, 
                   ( i.Quan * i.Price ) AS pricequant, 
                   (SELECT Name 
                    FROM   Inv 
                    WHERE  ID = i.Acct) AS billcode,
                 isnull(i.jobitem,0) as code , 
              (SELECT Type        FROM   Inv    WHERE  ID = i.Acct)    as INVType  ,
                     i.Warehouse , 
                isnull(i.WHLocID,0) WHLocID, 
         
            (SELECT Status FROM Inv WHERE ID = i.Acct) AS InvStatus, 
            (SELECT top 1 Status FROM Chart WHERE ID in(SELECT SAcct FROM Inv WHERE ID = i.Acct)) AS AStatus, 
            ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref )  

            ,Case i.Stax when  1 then 
                      Case    when i.GstAmount is null and inv.GTax=0 then 0 
                              when i.GstAmount is null and inv.GTax <> 0 then 1 
                              when i.GstAmount is not null and i.GstAmount = 0 then 0 
                              when i.GstAmount is not null and i.GstAmount <> 0 then 1 
                        END 
             When 0 then 
                       Case 
                              when i.GstAmount is not null and i.GstAmount <> 0 then 1 
            					else 0 
                       END 
             End as EnableGSTTax 

            FROM   InvoiceI i 
          
                   INNER JOIN Invoice inv 
          
                           ON inv.Ref = i.Ref 
            WHERE  i.Ref = @Ref
             order by Line
          

		 DECLARE @oldData INT 
		 DECLARE @TaxType INT 
		 SET @TaxType = (SELECT [type] FROM  Stax 
						WHERE Name =( SELECT STax  FROM Loc  WHERE loc =( SELECT loc from Invoice where REf=@Ref)))
		 SET @oldData=0
         IF (SELECT count(1) from Invoice where Ref=@Ref AND ISNULL(GTax,0)<>0 )>0
		 BEGIN
			IF (SELECT count(1) from InvoiceI where Ref=@Ref AND GSTAmount IS NULL )>0
			BEGIN
				SET @oldData=1
            END
            
         END
         --For old data compound Tax
		 SELECT @taxType AS TaxType, @oldData AS IsOldData

END