
CREATE VIEW vw_ApprovalStatus  
AS  
SELECT * FROM (  
 SELECT  
 ROW_NUMBER() over(Partition by PO Order by ApproveDate desc) AS RowNo  
,ID
,PO
,Status
,Comments
,Signature
,ApproveDate
,UserID
 FROM ApprovalStatus
)  AS T WHERE T.RowNo = 1