CREATE PROCEDURE [dbo].[SpGetInventoryByTypeName]
	
AS
BEGIN

IF NOT EXISTS(SELECT * FROM BOMT where Type='Inventory') 
BEGIN  
INSERT [dbo].[BOMT] ([Type]) VALUES ('Inventory') 
END

SELECT * FROM BOMT WHERE TYPE='inventory'

END
GO
