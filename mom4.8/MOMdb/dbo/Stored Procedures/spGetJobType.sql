CREATE PROCEDURE [dbo].[spGetJobType]


as
	begin

	create table #tempJobType
	(
		  ID SMALLINT,
		  Type VARCHAR(50) NULL,
		  Count SMALLINT NULL,
		  Color SMALLINT NULL,
		  Remarks VARCHAR(255) NULL,
		  IsDefault SMALLINT NULL
	)

	INSERT INTO #tempJobType
	SELECT -1,'All',null,null,'All',0

	INSERT INTO #tempJobType
	SELECT  ID,Type,Count,Color,Remarks,IsDefault FROM JobType 


	select * from  #tempJobType

	drop table  #tempJobType
	end
go