CREATE PROC [dbo].[spGetEscalationContracts] 
	@EscDate DATETIME,
	@UserId INT,
	@EN INT
AS	
BEGIN ;

     WITH CTE AS	(		
            SELECT  Loc.ID AS LocID,Loc.Tag, Job.CType,     job.fdesc,
	        CASE Contract.bcycle
	            WHEN 0 THEN 'Monthly'	            WHEN 1 THEN 'Bi-Monthly'  WHEN 2 THEN 'Quarterly'  WHEN 3 THEN '3 Times/Year'
	            WHEN 4 THEN 'Semi-Annually'	        WHEN 5 THEN 'Annually'    WHEN 6 THEN 'Never'	   WHEN 7 THEN '3 Years'
	            WHEN 8 THEN '5 Years'	            WHEN 9 THEN '2 Years'
				END AS Freqency,
	        CASE Contract.BEscType
	            WHEN 0 THEN 'Commodity Index'	    WHEN 1 THEN 'Escalation'  WHEN 2 THEN 'Return'    WHEN 3 THEN 'Manual'
	        END AS EscType,
	         'Renew' Action,
	        Contract.BEscCycle,
	        Contract.BEscType,
	        Contract.BEscFact,
	        CONVERT(varchar(30), Isnull(Contract.EscLast, '1900-1-1'),101) AS EscLast,
	        CONVERT(varchar(30), Isnull(Contract.BStart, '1900-1-1'),101)  AS BStart,
	        CONVERT(varchar(30), Isnull(Contract.Bfinish, '1900-1-1'),101) AS Bfinish,
	        CONVERT(varchar(30), Dateadd(month, BEscCycle, Isnull(Contract.EscLast, '1900-1-1')), 101) AS nextdue,
	        Contract.Bamt,
	        (CASE BEscType
	            WHEN 1 THEN CONVERT(NUMERIC(30, 2), (BAmt + ( ( BAmt * BEscFact ) / 100 )))
	            ELSE BAmt
	        END) AS newamt,
	        Contract.BLenght,
	        Contract.Job,
	        CONVERT(varchar(30), Isnull(Contract.ExpirationDate, '1900-1-1'),101) AS ExpirationDate,
	        CASE IsRenewalNotes
	            WHEN 1 THEN RenewalNotes        ELSE ''
	        END AS RenewalNotes,	        
			CASE Contract.Status 
         WHEN 0 THEN 'Active' 
         WHEN 1 THEN 'Closed' 
         WHEN 2 THEN 'Hold' 
         WHEN 3 THEN 'Completed' 
         END   Status, 
			'' As Company,			r1.Name As Customer,
			r1.Address As Address,			r1.City As City,
			r1.State As State,			r1.Zip As Zip,
			Rol.Address + ','+ Rol.City + ',' + Rol.State + ',' + Rol.Zip  As LocationCompanyName,
			t.Name AS Salesperson,
			t2.Name AS Salesperson2,
			Job.fDate ContractDate ,
			isnull(Contract.LastRenew,Job.fDate) LastContract
	    	FROM Contract 
	        INNER  JOIN Job ON Job.ID = Contract.Job
		    LEFT  JOIN Loc ON Loc.Loc = Contract.Loc  
			LEFT  JOIN Owner o ON o.ID = Loc.Owner
			LEFT  JOIN Rol ON Loc.Rol = Rol.ID
			LEFT  JOIN Rol r1 ON o.Rol = r1.ID 	        
			LEFT  JOIN Terr t ON t.ID = Loc.Terr
			LEFT  JOIN Terr t2 ON t2.ID = Loc.Terr2
	   
	   WHERE
	        cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @EscDate as date  )

			and isnull(Expiration,0) = 1	 

			AND  Job.type = 0  


			----------------------------------
			UNION  ALL

		 SELECT  
			Loc.ID AS LocID,	        Loc.Tag,
	        Job.CType,	        job.fdesc,
	        CASE Contract.bcycle
	            WHEN 0 THEN 'Monthly'	            WHEN 1 THEN 'Bi-Monthly'
	            WHEN 2 THEN 'Quarterly'	            WHEN 3 THEN '3 Times/Year'
	            WHEN 4 THEN 'Semi-Annually'         WHEN 5 THEN 'Annually'
	            WHEN 6 THEN 'Never'  	            WHEN 7 THEN '3 Years'
	            WHEN 8 THEN '5 Years'	            WHEN 9 THEN '2 Years'
	        END AS Freqency,
	        CASE Contract.BEscType
	            WHEN 0 THEN 'Commodity Index'	    WHEN 1 THEN 'Escalation'
	            WHEN 2 THEN 'Return'                WHEN 3 THEN 'Manual'
	        END AS EscType,
			  'Escalate' AS Action,
	        Contract.BEscCycle,
	        Contract.BEscType,
	        Contract.BEscFact,
	        CONVERT(varchar(30), Isnull(Contract.EscLast, '1900-1-1'),101) AS EscLast,
	        CONVERT(varchar(30), Isnull(Contract.BStart, '1900-1-1'),101)  AS BStart,
	        CONVERT(varchar(30), Isnull(Contract.Bfinish, '1900-1-1'),101) AS Bfinish,
	        CONVERT(varchar(30), Dateadd(month, BEscCycle, Isnull(Contract.EscLast, '1900-1-1')), 101) AS nextdue,
	        Contract.Bamt,
	        (CASE BEscType
	            WHEN 1 THEN CONVERT(NUMERIC(30, 2), (BAmt + ( ( BAmt * BEscFact ) / 100 )))
	            ELSE BAmt
	        END) AS newamt,
	        Contract.BLenght,
	        Contract.Job,
	        CONVERT(varchar(30), Isnull(Contract.ExpirationDate, '1900-1-1'),101) AS ExpirationDate,
	        CASE IsRenewalNotes
	            WHEN 1 THEN RenewalNotes
	            ELSE ''
	        END AS RenewalNotes,
	         CASE Contract.Status 
         WHEN 0 THEN 'Active' 
         WHEN 1 THEN 'Closed' 
         WHEN 2 THEN 'Hold' 
         WHEN 3 THEN 'Completed' 
       END                    Status, 
			'' As Company,			r1.Name As Customer,
			r1.Address As Address,			r1.City As City,
			r1.State As State,			r1.Zip As Zip,
			Rol.Address + ','+ Rol.City + ',' + Rol.State + ',' + Rol.Zip  As LocationCompanyName,
			t.Name AS Salesperson,
			t2.Name AS Salesperson2,
			Job.fDate ContractDate ,
			isnull(Contract.LastRenew,Job.fDate) LastContract
	   FROM Contract 
	        INNER  JOIN Job ON Job.ID = Contract.Job
		    LEFT  JOIN Loc ON Loc.Loc = Contract.Loc  
			LEFT  JOIN Owner o ON o.ID = Loc.Owner
			LEFT  JOIN Rol ON Loc.Rol = Rol.ID
			LEFT  JOIN Rol r1 ON o.Rol = r1.ID 	        
			LEFT  JOIN Terr t ON t.ID = Loc.Terr
			LEFT  JOIN Terr t2 ON t2.ID = Loc.Terr2
	   
	   WHERE
	  
	  (Dateadd(month, Contract.BEscCycle, isnull(Contract.Esclast,isnull(Contract.BStart,Contract.SStart) ))) <= @EscDate  
	    and isnull(Expiration,0) = 1
		AND  Job.type = 0  
		and Contract.job not in 
		(    
			SELECT Contract.Job
			FROM Contract 
	        INNER JOIN Job ON Job.ID = Contract.Job
		    LEFT JOIN Loc ON Loc.Loc = Contract.Loc  
			LEFT  JOIN Owner o ON o.ID = Loc.Owner
			LEFT  JOIN Rol ON Loc.Rol = Rol.ID
			LEFT  JOIN Rol r1 ON o.Rol = r1.ID 	        
			LEFT  JOIN Terr t ON t.ID = Loc.Terr
			LEFT  JOIN Terr t2 ON t2.ID = Loc.Terr2
	   
	   WHERE
	        cast (isnull(ExpirationDate,isnull(Contract.BStart,Contract.SStart) ) as date ) <= CAST ( @EscDate as date  )
			and isnull(Expiration,0) = 1	 
			AND  Job.type = 0  

			)  

			UNION  ALL

		 SELECT  
			Loc.ID AS LocID,	        Loc.Tag,
	        Job.CType,	        job.fdesc,
	        CASE Contract.bcycle
	            WHEN 0 THEN 'Monthly'	            WHEN 1 THEN 'Bi-Monthly'
	            WHEN 2 THEN 'Quarterly'	            WHEN 3 THEN '3 Times/Year'
	            WHEN 4 THEN 'Semi-Annually'         WHEN 5 THEN 'Annually'
	            WHEN 6 THEN 'Never'  	            WHEN 7 THEN '3 Years'
	            WHEN 8 THEN '5 Years'	            WHEN 9 THEN '2 Years'
	        END AS Freqency,
	        CASE Contract.BEscType
	            WHEN 0 THEN 'Commodity Index'	    WHEN 1 THEN 'Escalation'
	            WHEN 2 THEN 'Return'                WHEN 3 THEN 'Manual'
	        END AS EscType,
			  'Escalate' AS Action,
	        Contract.BEscCycle,
	        Contract.BEscType,
	        Contract.BEscFact,
	        CONVERT(varchar(30), Isnull(Contract.EscLast, '1900-1-1'),101) AS EscLast,
	        CONVERT(varchar(30), Isnull(Contract.BStart, '1900-1-1'),101)  AS BStart,
	        CONVERT(varchar(30), Isnull(Contract.Bfinish, '1900-1-1'),101) AS Bfinish,
	        CONVERT(varchar(30), Dateadd(month, BEscCycle, Isnull(Contract.EscLast, '1900-1-1')), 101) AS nextdue,
	        Contract.Bamt,  (CASE BEscType  WHEN 1 THEN CONVERT(NUMERIC(30, 2), (BAmt + ( ( BAmt * BEscFact ) / 100 )))  ELSE BAmt
	        END) AS newamt,
	        Contract.BLenght,
	        Contract.Job,
	        '1900-1-1' AS ExpirationDate,
	        CASE IsRenewalNotes
	            WHEN 1 THEN RenewalNotes
	            ELSE ''
	        END AS RenewalNotes,
	         CASE Contract.Status 
         WHEN 0 THEN 'Active' 
         WHEN 1 THEN 'Closed' 
         WHEN 2 THEN 'Hold' 
         WHEN 3 THEN 'Completed' 
       END                    Status, 
			'' As Company,			r1.Name As Customer,
			r1.Address As Address,			r1.City As City,
			r1.State As State,			r1.Zip As Zip,
			Rol.Address + ','+ Rol.City + ',' + Rol.State + ',' + Rol.Zip  As LocationCompanyName,
			t.Name AS Salesperson,
			t2.Name AS Salesperson2,
			Job.fDate ContractDate ,
			isnull(Contract.LastRenew,Job.fDate) LastContract
	   FROM Contract 
	        INNER  JOIN Job ON Job.ID = Contract.Job
		    LEFT  JOIN Loc ON Loc.Loc = Contract.Loc  
			LEFT  JOIN Owner o ON o.ID = Loc.Owner
			LEFT  JOIN Rol ON Loc.Rol = Rol.ID
			LEFT  JOIN Rol r1 ON o.Rol = r1.ID 	        
			LEFT  JOIN Terr t ON t.ID = Loc.Terr
			LEFT  JOIN Terr t2 ON t2.ID = Loc.Terr2
	   
	   WHERE
	  
	  (Dateadd(month, Contract.BEscCycle, isnull(Contract.Esclast,isnull(Contract.BStart,Contract.SStart) ))) <= @EscDate  
	    and isnull(Expiration,0) = 0
		AND  Job.type = 0  
		)
 
		SELECT * FROM CTE  
	
	END
GO