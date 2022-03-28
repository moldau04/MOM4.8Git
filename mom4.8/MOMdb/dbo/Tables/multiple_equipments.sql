CREATE TABLE [dbo].[multiple_equipments] (
    [id]               INT        IDENTITY (1, 1) NOT NULL,
    [ticket_id]        INT        NULL,
    [elev_id]          INT        NULL,
    [labor_percentage] FLOAT (53) NULL, 
    CONSTRAINT [PK_multiple_equipments_ID] PRIMARY KEY ([id])
);


---------------------------####### Please don't drop it ########## --------------------

GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170830-145147]
    ON [dbo].[multiple_equipments]([elev_id] ASC);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170830-145135]
    ON [dbo].[multiple_equipments]([ticket_id] ASC);

 

