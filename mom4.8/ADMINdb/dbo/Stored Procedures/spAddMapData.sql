CREATE PROCEDURE [dbo].[spAddMapData]
@DtMapData As [dbo].[tblTypeMapData] Readonly

as

insert into MapData
(
deviceId,
latitude,
longitude,
date,
fake,
Accuracy
)
Select Distinct
deviceId, latitude, longitude, date,fake,accuracy
From @DtMapData d 


IF NOT EXISTS (SELECT * FROM MSM2_Admin.SYS.TABLES WHERE NAME = 'MapDataOfOneMonth')
BEGIN
insert into MapDataOfOneMonth
(
deviceId,
latitude,
longitude,
date,
fake,
Accuracy
)
Select Distinct
deviceId, latitude, longitude, date,fake,accuracy
From @DtMapData d 
END
 --WHERE 
 --NOT EXISTS(SELECT 1
 --           FROM MapData m
 --          WHERE m.latitude=d.latitude 
 --          and m.longitude=d.longitude 
 --          and m.deviceId=d.deviceId 
 --          and m.date=d.date) 