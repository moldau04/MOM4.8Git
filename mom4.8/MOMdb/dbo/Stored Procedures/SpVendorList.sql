CREATE PROCEDURE [dbo].[SpVendorList]

	@Name VARCHAR(150)
As 
Begin 

Select  V.ID,
		V.Acct,
		R.Name,
		V.status,
		R.Address,
		R.City,
		R.State,
		R.Zip,
		V.Type,
		R.Contact,
		R.Phone,
		R.Fax,
		Case When V.status = 1 Then 'InActive'   
			 When V.status = 0 Then 'Active'
			 When V.status = 2 Then 'Hold' 
End as Status
From 
		Vendor as V 
Inner Join  
		Rol as R 
		ON V.Rol = R.ID
Where   R.Type = 1 AND R.Name=@Name

End

--exec SpVendorList NULL


select * from Rol where type=1
