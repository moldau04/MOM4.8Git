CREATE PROCEDURE [dbo].[spAddMapDataNew] 
@DtMapDataNew As [dbo].[tblTypeMapDataNew] Readonly
AS

insert into [dbo].[MapDataNew]
(
deviceId,
latitude,
longitude,
date,
fake,
Accuracy,
fUser,
userId
)
Select Distinct
deviceId, latitude, longitude,date,fake,accuracy,fUser,userId
From @DtMapDataNew n 
