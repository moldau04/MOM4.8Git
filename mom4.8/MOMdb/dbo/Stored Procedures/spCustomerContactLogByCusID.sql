CREATE  Procedure [dbo].[spCustomerContactLogByCusID] 
   @CusID      INT
AS 
BEGIN
	DECLARE @Rol INT
    SET @Rol= (SELECT Rol FROM Owner WHERE ID=@CusID)	
	select fDesc, [Ref],fUser
           ,[Field]
           ,[OldVal]
           ,[NewVal],CreatedStamp,fDate,fTime from Log2
left join Phone p on p.ID= log2.Ref
where p.Rol=@Rol AND (Screen='Phone' ) 
ORDER BY Ref
END