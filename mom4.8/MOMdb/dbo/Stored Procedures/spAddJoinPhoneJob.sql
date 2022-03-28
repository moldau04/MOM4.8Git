CREATE PROCEDURE [dbo].[spAddJoinPhoneJob]
 @JobID int ,
 @PhoneID int,
 @IsHighLighted int
 AS 
     if(@IsHighLighted=1)
	 begin ---1
	   if not exists(select 1 From tblJoinPhoneJob where  JobID=@JobID and PhoneID=@PhoneID)
	   begin --2
	   insert into tblJoinPhoneJob (JobID,PhoneID)
	   values(@JobID,@PhoneID)
	   end---2
	 END---1
	 else
	 begin --3
	  delete From tblJoinPhoneJob  where  JobID=@JobID and PhoneID=@PhoneID
	 END---3
