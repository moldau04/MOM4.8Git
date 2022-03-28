CREATE PROCEDURE [dbo].[spAddConsultant]
	@RolName VARCHAR(75),
	@RolID INT,
	@Count INT,
	@API SMALLINT,
	@Username VARCHAR(75),
	@Password VARCHAR(75),
	@IP VARCHAR(50)
as
INSERT INTO tblConsult
(
Name,
Rol,
Count,
API,
Username,
Password,
IP
)
values
(
@RolName,
@RolID,
@Count,
@API,
@Username,
@Password,
@IP
)
