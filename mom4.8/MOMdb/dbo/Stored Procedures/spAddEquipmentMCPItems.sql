CREATE proc [dbo].[spAddEquipmentMCPItems]
 @items    AS [dbo].[tblTypeEquipTempItems1] Readonly
as

 --DELETE FROM EquipTItem
 --   WHERE  Elev in (select Elev from @items)
    
 insert into EquipTItem
	 (
		Code,
		EquipT,
		Elev,
		fDesc,
		Frequency,
		Lastdate,
		Line ,
		NextDateDue,
		Section,
		Notes
	 )
	 select	code,
			EquipT,
			Elev,
			fDesc,
			Frequency,
	    	Lastdate,
			Line, 
			NextDateDue,
			Section,
			Notes
	from	@items
GO

