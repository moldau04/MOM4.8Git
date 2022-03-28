CREATE PROCEDURE [dbo].[spGetCollectionNotes]
	@OwnerID INT,
	@LocID int,
	@ShowAll BIT 
AS
BEGIN
	IF @ShowAll =1 
		BEGIN
			IF @LocID =0
			BEGIN
				SELECT cn.*, isnull(l.tag,'') as LocName FROM CollectionNotes cn LEFT JOIN Loc l on isnull(cn.locID,0)=l.Loc
				WHERE OwnerID=@OwnerID ORDER By ID DESC
			END
			ELSE
			BEGIN
		
				SELECT * FROM (
					SELECT cn.*, isnull(l.tag,'') as LocName FROM CollectionNotes cn LEFT JOIN Loc l on isnull(cn.locID,0)=l.Loc
					WHERE OwnerID=@OwnerID AND LocID IS NULL
					UNION
					SELECT cn.*, isnull(l.tag,'') as LocName FROM CollectionNotes cn LEFT JOIN Loc l on isnull(cn.locID,0)=l.Loc
					WHERE OwnerID=@OwnerID AND ISNULL(LocID,0)=@LocID
					) t ORDER BY ID DESC
			END	
		END
	ELSE
		BEGIN
			IF @LocID =0
			BEGIN
				SELECT cn.*, isnull(l.tag,'') as LocName FROM CollectionNotes cn LEFT JOIN Loc l on isnull(cn.locID,0)=l.Loc
				WHERE OwnerID=@OwnerID AND LocID IS NULL ORDER By ID DESC
			END
			ELSE
			BEGIN
		
				SELECT cn.*, isnull(l.tag,'') as LocName FROM CollectionNotes cn LEFT JOIN Loc l on isnull(cn.locID,0)=l.Loc
				WHERE OwnerID=@OwnerID AND ISNULL(LocID,0)=@LocID
				ORDER BY ID DESC
			END	
		END
	
    Select CNotes FROM Owner Where ID=@OwnerID
END