
CREATE PROCEDURE [dbo].[spGetAPBillItem]  
 @batch int  
AS  
BEGIN  
 SET NOCOUNT ON;  
  SELECT 
  [Batch] ,
[TRID] AS ID ,
[JobId] ,
[jobName] ,
[Ticket] ,
[TypeID] ,
CAST(CAST ([PhaseID] AS NUMERIC) AS INT) as  [PhaseID],
[phase] ,
[ItemID] ,
--[ItemDesc] ,
CASE WHEN phase = 'Inventory' THEN (SELECT Name FROM Inv WHERE ID= ItemID) ELSE [ItemDesc] END AS ItemDesc,
[Warehouse] ,
[Warehousefdesc] ,
[WHLocID] ,
[Locationfdesc] ,
[AcctID] ,
[AcctName] ,
[Quan] ,
[Amount] ,
[line] ,
[Ref] ,
[Sel] ,
[Type] ,
[strRef] ,
[AcctNo] ,
--[fDesc] ,
CASE WHEN phase = 'Inventory' THEN (SELECT fDesc FROM Inv WHERE ID= ItemID) ELSE [fDesc] END AS fDesc,
[UseTax] ,
[UtaxGL] ,
[UName] ,
[loc] ,
[OpSq] ,
[PrvIn] ,
[PrvInQuan] ,
[OutstandQuan] ,
[OutstandBalance] ,
CAST(ISNULL([STax],0) as [smallint]) AS  [STax],
[STaxName] ,
[STaxRate] ,
[STaxAmt] ,
[STaxGL] ,
[GSTRate] ,
[GTaxAmt] ,
[GSTTaxGL] ,
[STaxType] ,
[UTaxType] ,
ISNULL([IsPO],'1') AS IsPO,
CAST(ISNULL([GTax],0) as [smallint]) AS  [GTax],
Price,
0.00 AS OrderedQuan,
0.00 AS Ordered
  FROM APBillItem 
  
  WHERE batch = @batch

END
