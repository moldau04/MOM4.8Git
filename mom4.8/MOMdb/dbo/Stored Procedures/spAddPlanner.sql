CREATE PROCEDURE [dbo].[spAddPlanner]
	@PID int,
	@Desc varchar(500),
	@Type varchar(50)
AS
	INSERT INTO Planner([PID],[Desc],[Type]) VALUES (@PID,@Desc,@Type)

	Select @@Identity
