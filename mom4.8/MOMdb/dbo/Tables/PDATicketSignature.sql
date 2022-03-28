CREATE TABLE [dbo].[PDATicketSignature] (
    [PDATicketID]   INT              NOT NULL,
    [SignatureType] CHAR (1)         NULL,
    [Signature]     IMAGE            NULL,
    [AID]           UNIQUEIDENTIFIER NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_PDATicketID]
    ON [dbo].[PDATicketSignature]([PDATicketID] DESC);

