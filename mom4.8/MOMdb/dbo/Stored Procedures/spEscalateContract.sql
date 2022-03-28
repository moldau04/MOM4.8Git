CREATE PROC spEscalateContract
@Cdate datetime,
@jobs varchar(max),
@UpdatedBy varchar(100) 
AS 
-----------------------------------------
---------------------------------------->     CONTRACT RENEW
-----------------------------------------

Insert into Log2 ( [fUser]  ,[Screen]   ,[Ref]  ,[Field] ,[OldVal] ,[NewVal] ,[CreatedStamp]    )

select     @UpdatedBy,'Job' , job,  'Escalation - Last Renew Date' ,  LastRenew , @Cdate, GETDATE()  
from CONTRACT WHERE job in (@jobs)  AND isnull(Expiration,0) <> 0  
AND cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @Cdate as date  )
UNION ALL
select     @UpdatedBy,'Job' , job,  'Escalation - Review Date' , Review , dateadd(month,BEscCycle,@Cdate), GETDATE() 
from CONTRACT WHERE job in (@jobs)  AND isnull(Expiration,0) <> 0
AND cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @Cdate as date  )
UNION ALL
select     @UpdatedBy,'Job' , job,  'Escalation - Billing End Date' ,BFinish , dateadd(day,-1, dateadd(month,BEscCycle,@Cdate)), GETDATE()  
from CONTRACT WHERE job in (@jobs)  AND isnull(Expiration,0) <> 0
AND cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @Cdate as date  )
UNION ALL
select     @UpdatedBy,'Job' , job,  'Escalation - Expiration Date' ,ExpirationDate , dateadd(month,BEscCycle,@Cdate), GETDATE()  
from CONTRACT WHERE job in (@jobs) AND isnull(Expiration,0) <> 0
AND cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @Cdate as date  )


UPDATE CONTRACT SET  
LastRenew = @Cdate, 
Review = dateadd(month,BEscCycle,@Cdate),
BFinish =dateadd(day,-1, dateadd(month,BEscCycle,@Cdate)),
ExpirationDate = case Expiration when 1 then dateadd(month,BEscCycle,@Cdate) else NULL end
WHERE job in (@jobs)  
AND isnull(Expiration,0) <> 0
AND cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @Cdate as date  )
  

-----------------------------------------
---------------------------------------->    CONTRACT ESCALATE
-----------------------------------------

Insert into Log2 ( [fUser]  ,[Screen]   ,[Ref]  ,[Field] ,[OldVal] ,[NewVal] ,[CreatedStamp]    )
select     @UpdatedBy,'Job' , job,  'Escalation - Last ESCALATE Date' , cast (  EscLast  as nvarchar), cast ( @Cdate as nvarchar), GETDATE() 
from CONTRACT WHERE job in (@jobs) 
AND (Dateadd(month, Contract.BEscCycle, isnull(Contract.Esclast,isnull(Contract.BStart,Contract.SStart)))) < = @Cdate 
UNION ALL
select     @UpdatedBy,'Job' , job,  'Escalation - Billing Amount' , cast( BAmt as nvarchar) , cast ( case BEscType when 1 then convert(numeric(30,2), (BAmt + ((BAmt * BEscFact)/100))) else BAmt end as nvarchar), GETDATE() 
from CONTRACT WHERE job in (@jobs) 
AND (Dateadd(month, Contract.BEscCycle, isnull(Contract.Esclast,isnull(Contract.BStart,Contract.SStart)))) < = @Cdate 

--------ES-6010-Potomac - Last esc date not updating when renewed the contract but contract date is updating

UPDATE CONTRACT SET
EscLast = @Cdate,
BAmt = case BEscType when 1 then convert(numeric(30,2), (BAmt + ((BAmt * BEscFact)/100))) else BAmt end 
WHERE job in (@jobs) 
AND (Dateadd(month, Contract.BEscCycle, isnull(Contract.Esclast,isnull(Contract.BStart,Contract.SStart)))) < = @Cdate  

EXECUTE SpUpdatingProjectBudgetAmount @job=@jobs,@UpdatedBy=@UpdatedBy