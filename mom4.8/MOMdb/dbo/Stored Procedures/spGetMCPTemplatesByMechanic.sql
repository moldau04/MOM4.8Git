CREATE procedure [dbo].[spGetMCPTemplatesByMechanic]
@Worker varchar(250),
@TicketID INT=null
AS
BEGIN
SELECT 
											CASE WHEN TicketO.ID IS NOT NULL THEN TicketO.ID ELSE TicketO.ID END AS TicketId,
                                            Rol.Name AS 'Customer Name'
                                            , Location.ID AS 'Location Id'
                                            , Location.Tag AS 'Location Tag'
                                            , Unit.Unit AS 'Equip'
                                            , MCP.Code
                                            , MCP.fDesc
                                            , MCP.Section
											,CASE WHEN MCP.nextdatedue > GETDATE() THEN 0 ELSE DATEDIFF(DAY, MCP.NextDateDue, GETDATE()) END As Aging
                                            ,CASE MCP.Frequency
                                                WHEN 1 THEN 'Weekly'
                                                WHEN 2 THEN 'Bi-Weekly'
                                                WHEN 3 THEN 'Monthly'
                                                WHEN 4 THEN 'Bi-Monthly'
                                                WHEN 5 THEN 'Quarterly'
                                                WHEN 6 THEN 'Semi-Annually'
                                                WHEN 7 THEN 'Annually'
                                                ELSE 'Daily'
                                            END AS 'freq'
                                            , CONVERT(VARCHAR, MCP.Lastdate, 110) AS 'LastDate'
                                            , CONVERT(VARCHAR, MCP.NextDateDue, 110) AS 'NextDueDate'
                                            , Template.fdesc AS 'Template' 
                                        FROM EquipTItem AS MCP
                                        LEFT JOIN EquipTemp AS Template ON Template.ID = MCP.EquipT
                                        LEFT JOIN Elev AS Unit ON Unit.ID = MCP.Elev
                                        LEFT JOIN Loc AS Location ON Location.Loc = Unit.Loc
                                        LEFT JOIN Owner AS customer ON Customer.ID = Location.Owner
                                        LEFT JOIN Rol AS Rol ON Rol.ID = customer.Rol
										LEFT JOIN TblWork AS worker ON Location.Route = worker.ID
										LEFT JOIN Contract ON Contract.Loc = Location.Loc
										LEFT JOIN TicketO ON TicketO.Job = Contract.Job
										LEFT JOIN TicketD ON TicketD.Job = Contract.Job
                                        WHERE MCP.Elev <> 0 AND MCP.NextDateDue < GETDATE()
										AND worker.fDesc = @Worker
                                        ORDER BY MCP.Lastdate ASC
END