-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/****** Object:  StoredProcedure [dbo].[SpVendorName]    Script Date: 5/29/2016 2:44:17 PM ******/

CREATE PROCEDURE [dbo].[SpVendorName]
As 
Begin 
Select  V.ID,R.Name
		
from 
		Vendor as V 
Inner join  
		Rol as R ON V.Rol = R.ID
where   R.Type = 1

end
