create proc [dbo].[spGetChargeableTicketsMapping]
as
SELECT t.ID                                AS ticketid,
       Isnull(Charge, 0)                   AS chargen,
       t.Total,
       othere,
       toll,
       t.zone,
       emile,
       Smile,
       l.tag                               AS locname,
       l.Loc                               AS lid,
       l.Owner,
       t.Type,
       fwork,
       t.fDesc,
       descres,
       r.Address,
       r.city,
       r.state,
       r.zip,
       (SELECT TOP 1 Name
        FROM   rol
        WHERE  ID = (SELECT TOP 1 Rol
                     FROM   Owner
                     WHERE  ID = l.Owner)) AS customerName,
       l.stax,
       (SELECT Rate
        FROM   stax
        WHERE  name = l.stax)              AS rate,
       (SELECT qblocid
        FROM   loc
        WHERE  loc = l.loc)                AS qbcustid,
       (SELECT QBJobTypeID
        FROM   JobType j
        WHERE  j.ID = t.Type)              AS QBJobTypeID,
       (SELECT QBTermsID
        FROM   tblTerms ter
        WHERE  ter.ID = 0)                 AS qbTermsID,
       (SELECT QBStaxID
        FROM   STax st
        WHERE  st.Name = l.STax)           AS QBStaxID,
        isnull(QBServiceItem,'') as QBServiceItem
FROM   TicketD t
       INNER JOIN Loc l
               ON l.Loc = t.Loc
               inner join Rol r on r.ID=l.Rol
WHERE  Isnull(Charge, 0) <> 0 and isnull(clearcheck,0) = 1 and qbinvoiceid is  null and l.QBLocID is not null

