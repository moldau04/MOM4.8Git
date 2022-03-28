Create  PROCEDURE [dbo].[spGetCustomerUnAppliedCredit]
	@UserID int		= 0,
	@filterby int  =1
AS
Begin
    IF (Select count(*) from  tblUserCo) >0
	BEGIN
			--Filter =1 : All
		IF @filterby =1
			BEGIN 
				SELECT distinct o.ID  AS ID, r.Name 
				FROM Owner o
					 LEFT JOIN Loc l on o.ID=l.Owner
					 LEFT JOIN Rol r on r.ID=o.Rol
					 LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN 
				 WHERE UC.IsSel = 1 and Uc.UserID=@UserID
				   group by o.ID, r.Name
				  order by Name
			END
		ELSE		
			BEGIN
				IF @filterby =2
					BEGIN				
						SELECT distinct o.ID  AS ID, r.Name 
						 FROM Owner o
						 left join Loc l on o.ID=l.Owner
						 left join Rol r on r.ID=o.Rol
						 left join tblUserCo UC on UC.CompanyID = r.EN 
						 WHERE o.Balance <0
						  and UC.IsSel = 1 and UC.UserID=@UserID
						  group by o.ID, r.Name
						   order by Name
					END
				ELSE
					BEGIN
						SELECT DISTINCT o.ID AS ID, r.Name 
						FROM Owner o
						left  join Loc l  on o.ID=l.Owner
						left  join Rol r on r.ID=o.Rol
						left  join tblUserCo UC on UC.CompanyID = r.EN  
						WHERE l.Loc in (
	 
						SELECT DISTINCT i.Loc						 
						FROM   Invoice i 
							left JOIN Loc l ON l.Loc = i.Loc 	
							LEFT  JOIN tblInvoicePayment ip ON i.ref = ip.ref 
							LEFT  JOIN PaymentDetails pd ON pd.InvoiceID = i.Ref 
							LEFT  JOIN OpenAR ar on ar.Ref = i.Ref AND ar.Type = 0 and l.Loc=ar.Loc
						WHERE i.Status NOT IN (1,2) and Amount <0
						UNION all
						SELECT l.Loc
						FROM OpenAR o 
							INNER JOIN Loc l  ON l.Loc = o.Loc
						WHERE o.Original<0 and o.Selected <> o.Original	and o.Type IN (1,2,3)
						)
						and UC.IsSel = 1 and Uc.UserID=@UserID AND o.Balance>0
						  group by o.ID, r.Name
						order by Name
					END
			END
	END
	ELSE
		BEGIN
						--Filter =1 : All
			IF @filterby =1
				BEGIN 
					SELECT distinct o.ID  AS ID, r.Name 
					FROM Owner o
						 LEFT JOIN Loc l on o.ID=l.Owner
						 LEFT JOIN Rol r on r.ID=o.Rol	
						   group by o.ID, r.Name
						 order by Name
				END
			ELSE		
			BEGIN
				IF @filterby =2
					BEGIN				
						 SELECT distinct o.ID  AS ID, r.Name 
						 FROM Owner o
						 left join Loc l on o.ID=l.Owner
						 left join Rol r on r.ID=o.Rol						
						 WHERE o.Balance <0			
						  group by o.ID, r.Name
						   order by Name
					END
				ELSE
					BEGIN
						SELECT DISTINCT o.ID AS ID, r.Name 
						FROM Owner o
						left  join Loc l  on o.ID=l.Owner
						left  join Rol r on r.ID=o.Rol						
						WHERE l.Loc in (	 
						SELECT DISTINCT i.Loc						 
						FROM   Invoice i 
							left JOIN Loc l ON l.Loc = i.Loc 	
							LEFT  JOIN tblInvoicePayment ip ON i.ref = ip.ref 
							LEFT  JOIN PaymentDetails pd ON pd.InvoiceID = i.Ref 
							LEFT  JOIN OpenAR ar on ar.Ref = i.Ref AND ar.Type = 0 and l.Loc=ar.Loc
						WHERE i.Status NOT IN (1,2) and Amount <0
						UNION all
						SELECT l.Loc
						FROM OpenAR o 
							INNER JOIN Loc l  ON l.Loc = o.Loc
						WHERE  o.Original <0 and o.Selected <> o.Original	and o.Type IN (1,2,3)) 
						AND o.Balance>0
						  group by o.ID, r.Name
						order by Name
					END
			END
		END	
 End





