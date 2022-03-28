CREATE PROCEDURE [dbo].[spGetEquipDetails]
	
AS

Begin
select (e.unit+' - '+e.fDesc) As [Equipment Name],e.state As State, e.cat As [Service Type],
e.category As Category,e.manuf As Manuf,e.price As Price,e.last As [Last Service],e.since As Installed,
e.Building AS [Building Type],e.id As ID,e.unit As Name,e.type As Type,e.fdesc As Description,
CASE WHEN e.status = 0 THEN 'Active' ELSE 'Inactive' END AS Status,r.name As Customer,
l.id as [Location ID],l.tag As Location,(l.address+', '+l.city+', '+l.state+', '+l.zip) as Address ,e.shut_down AS [Shutdown],e.Classification, e.ShutdownReason
--l.Loc,e.ID as [Unit Id]

FROM elev e INNER JOIN loc l ON l.Loc = e.Loc 
INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r 
ON o.rol = r.id WHERE e.id IS NOT NULL

select

  (select loc.tag+ '+'+ loc.id from loc where loc = e.loc) as Location,

  (Select Owner.ID from Owner where Owner.ID = e.Owner) as OwnerID,

  (Select Rol.Name from Rol where ID = e.Owner and Rol.Type = 0) as OwnerName,

   unit+'-'+fDesc as equipment,

   State as Unique#,

 (select value from ElevTItem where Elev = e.ID and fdesc = 'annual Insp Date') as [Annual Insp Date],

  (select value from ElevTItem where Elev = e.ID and fdesc = 'Annual Inspector') as [Inspector Name],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Balastrades') as Balastrades,

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Building ELBI') as [Building ELBI],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Building Power') as [Building Power],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Capacity') as Capacity,

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Car Station') as [Car Station],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Car Station Bulb') as [Car Station Bulb],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Car Station Mfg') as [Car Station Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Car Weight') as [Car Weight],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Code Year') as [Code Year],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Coil Part Number') as [Coil Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Counterweight Roller Part Number') as [Counterweight Roller Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Door Opening Width') as [Door Opening Width],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Door Operator') as [Door Operator],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Door Operator Belt') as [Door Operator Belt],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Door Roller Part Number') as [Door Roller Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Door Type') as [Door Type],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Due Date') as [Due Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Emergency battery charger') as [Emergency Battery Charger],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Emergency light battery') as [Emergency light battery],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Emergency Light Bulb') as [Emergency Light Bulb],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Comb Teeth') as [Escalator Comb Teeth],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Handrail') as [Escalator Handrail],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Handrail Color') as [Escalator Handrail Color],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Handrail Length') as [Escalator Handrail Length],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Handrail Replacement Date') as [Escalator Handrail Replacement Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Roller Part Number') as [Escalator Roller Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Skirt Switch') as [Escalator Skirt Switch],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Speed') as [Escalator Speed],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Escalator Step Width') as [Escalator Step Width],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Fire Service Key') as [Fire Service Key],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Fire Service Phase II') as [Fire Service Phase II],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Floors') as Floors,

   (select value from ElevTItem where Elev = e.ID and fdesc = 'FPM') as FPM,

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Five Year Insp Date') as [Five Year Insp Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Gate Switch Part Number') as [Gate Switch Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Generator Brush Part Number') as [Generator Brush Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Generator Mfg') as [Generator Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Gib Part Number') as [Gib Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Governor Mfg') as [Governor Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Governor Rope Length') as [Governor Rope Length],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Governor Rope Size') as [Governor Rope Size],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Guide Roller Part Number') as [Guide Roller Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Hall Lantern Lens Cap') as [Hall Lantern Lens Cap],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Hall Lantern Mfg') as [Hall Lantern Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Hall Station bulb') as [Hall Station bulb],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Hall Station Manufacturer') as [Hall Station Manufacturer],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Hoistway Access Switch') as [Hoistway Access Switch],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Input Board') as [Input Board],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Interlock Part Number') as [Interlock Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Mfg Job Number') as [Mfg Job Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Model Type') as [Model Type],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Motor RPM') as [Motor RPM],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Motor Type') as [Motor Type],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Oil Line Size') as [Oil Line Size],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Openings') as [Openings],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Output Board') as [Output Board],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Packing Number') as [Packing Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Piston Diameter') as [Piston Diameter],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Position Indicator Bulb') as [Position Indicator Bulb],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Pump Motor Mfg') as [Pump Motor Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Pump Unit Mfg') as [Pump Unit Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Pump Motor Belt') as [Pump Motor Belt],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'purchase Date') as [purchase Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Altered Date') as [Altered Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Rope Length') as [Rope Length],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Rope Size') as [Rope Size],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'refNo') as [Ref No],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Safety Edges') as [Safety Edges],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Serial No.') as [Serial No.],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Service Interval') as [Service Interval],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Service Interval Unit') as [Service Interval Unit],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Spirator Part Number') as [Spirator Part Number],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Start Type') as [Start Type],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Starter Contact Mfg and Part No') as [Starter Contact Mfg and Part No],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Time Clock') as [Time Clock],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Time Clock Model') as [Time Clock Model],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'TXE') as [TXE],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Valve Mfg') as [Valve Mfg],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'warrantyExpirationDate') as [Warranty Expiration Date],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'wherePurchased') as [Where Purchased],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Annual Inspection Violations') as [Annual Inspection Violations],

   (select value from ElevTItem where Elev = e.ID and fdesc = 'Annual Inspector-Customer Preference') as [Annual Inspector-Customer Preference]

   from Elev  e  

--   where    

--   (select tag from loc where loc = e.loc) is not null and e.Status = 0

--ORDER  BY   OwnerID,location,equipment
End

