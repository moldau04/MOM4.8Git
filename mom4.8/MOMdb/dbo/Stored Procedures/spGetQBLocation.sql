CREATE PROCEDURE [dbo].[spGetQBLocation]
AS
SELECT  o.QBCustomerID AS qbcustomerid, 
        l.Loc                   AS ID, 
        l.QBLocID, 
        r.Address, 
        l.Tag, 
        r.Cellular, 
        r.City, 
        r.Contact, 
        r.Country, 
        r.EMail, 
        r.Fax, 
        r.Name, 
        r.Phone, 
        r.Remarks, 
        r.State, 
        r.Zip, 
        l.Status, 
        isnull(l.balance,0) as balance , 
        r.LastUpdateDate, 
        l.Address               AS shipaddress, 
        l.City                  AS shipcity, 
        l.State                 AS shipstate, 
        l.Zip                   AS shipzip, 
        --(SELECT QBStaxID FROM   stax WHERE  name = l.stax) AS QBstaxID, 
		st.QBStaxID  AS QBstaxID,
        --(SELECT QBlocTypeID FROM   LocType t WHERE  t.Type = l.Type)AS QBlocTypeID ,
		lt.QBlocTypeID AS QBlocTypeID,
		--(SELECT QBCustomertypeID FROM   OType t WHERE  t.Type = o.Type) AS QBCustomertypeID  
		ot.QBCustomerTypeID  AS QBCustomertypeID
FROM   Loc l 
INNER JOIN Owner o ON o.ID = l.Owner
LEFT OUTER JOIN Rol r ON l.Rol = r.ID 
LEFT JOIN stax st ON st.Name = l.STax
LEFT JOIN LocType lt ON lt.Type = l.Type
LEFT JOIN OType ot ON ot.Type = o.Type
WHERE  QBLocID IS NOT NULL and r.LastUpdateDate >= (select QBLastSync from Control) 
		/*For excluding the locations which doesnt exist in QB, which are same as parent.*/
        AND QBLocID <> o.QBCustomerID
		AND QBLocID like '________-__________'
