  
--exec spGetPOApproveDetails 1  
CREATE PROCEDURE [dbo].[spGetPOApproveDetails]      
@UserID INT      
AS      
BEGIN  
	IF EXISTS (SELECT 1 FROM tblRole r
				INNER JOIN tblUserRole ur ON ur.RoleId = r.Id
				WHERE ur.UserId = @UserID)
	BEGIN
		DECLARE @POApproveUser smallint;
		DECLARE @POLimit NUMERIC (30, 2);
	
		SELECT @POApproveUser = ISNULL(POApprove,0), @POLimit = ISNULL(POLimit,0) FROM tblUser WHERE ID = @UserID
		IF (ISNULL(@POLimit,0) = 0)
		BEGIN
			SET @POLimit = ISNULL((SELECT r.POLimit
										FROM tblRole r
										INNER JOIN tblUserRole ur ON ur.RoleId = r.Id
										WHERE ur.UserId = @UserID),0)
		END

		IF(@POApproveUser = 1)
		BEGIN
			SELECT 
			     ISNULL(@POLimit,0) As POLimit
				,ISNULL(POApprove,0) As POApprove
				,Isnull(POApproveAmt,0) As POApproveAmt
				,ISNULL(MinAmount,0) AS MinAmount
				,ISNULL(MaxAmount,0) AS MaxAmount  
			FROM tblUser WHERE ID = @UserID      
		END
		ELSE
		BEGIN
			SELECT 
				 ISNULL(@POLimit,0) As POLimit
				,ISNULL(r.POApprove,0) As POApprove
				,ISNULL(r.POApproveAmt,0) As POApproveAmt
				,ISNULL(r.MinAmount,0) AS MinAmount
				,ISNULL(r.MaxAmount,0) AS MaxAmount  
			FROM tblRole r
			INNER JOIN tblUserRole ur ON ur.RoleId = r.Id
			WHERE ur.UserId = @UserID      
		END
	END
	ELSE
	BEGIN
		SELECT 
		     ISNULL(POLimit,0) As POLimit
			,ISNULL(POApprove,0) As POApprove
			,Isnull(POApproveAmt,0) As POApproveAmt
			,ISNULL(MinAmount,0) AS MinAmount
			,ISNULL(MaxAmount,0) AS MaxAmount  
		FROM tblUser WHERE ID = @UserID      
	END
END