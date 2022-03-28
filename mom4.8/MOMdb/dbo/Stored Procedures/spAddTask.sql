CREATE PROC [dbo].[spAddTask] @rol        INT,
                             @DateDue    DATETIME,
                             @TimeDue    DATETIME,
                             @subject    VARCHAR(50),
                             @desc       VARCHAR(max),
                             @Fuser      VARCHAR(50),
                             @fby        VARCHAR(50),
                             @contact    VARCHAR(50),
                             @Mode       SMALLINT,
                             @ID         INT,
                             @Status     SMALLINT,
                             @Resolution VARCHAR(max),
                             @UpdateUser VARCHAR(50),
							 @Duration   numeric(30,2),
							 @Phone VARCHAR(50),
							 @Email VARCHAR(100),
							 @Screen VARCHAR(255),
                             @Ref Int,
                             @Keyword VARCHAR(50),
                             @IsAlert BIT
AS

BEGIN TRANSACTION
	Declare @Currentrol varchar(150)
	Declare @CurrentContact varchar(50)
	Declare @CurrentCPhone varchar(50)
	Declare @CurrentCEmail varchar(100)
	Declare @CurrentDateDue varchar(50)
	Declare @CurrentTimeDue varchar(50)
	Declare @Currentsubject varchar(50)
	Declare @Currentdesc varchar(1000)
	Declare @CurrentFuser varchar(1000)
    Declare @CurrentKeyword varchar(50)
    Declare @CurrentIsAlert BIT
	IF( @Status = 0 )
	BEGIN
		Select @Currentrol = Tag From Loc Where Rol = (Select Top 1 Rol  From ToDo Where ID = @ID)	
		Select @CurrentContact =  Contact,
			@CurrentCPhone = Phone,
			@CurrentCEmail = Email,
			@CurrentDateDue = CONVERT(varchar(50), DateDue, 101),
			@CurrentTimeDue = FORMAT(TimeDue, 'hh:mm tt'),
			@Currentsubject =  Subject,
			@Currentdesc =  Remarks,
			@CurrentFuser =  fUser,
            @CurrentKeyword = Keyword,
            @CurrentIsAlert = IsAlert
		FROM ToDo where ID =@ID
	END
	ELSE
	BEGIN
		Select @Currentrol = Tag From Loc Where Rol = (Select Top 1 Rol  From Done Where ID = @ID)	
		Select @CurrentContact =  Contact ,
			@CurrentCPhone = Phone,
			@CurrentCEmail = Email,
			@CurrentDateDue = CONVERT(varchar(50), Datedone, 101),
			@CurrentTimeDue = FORMAT(Timedone, 'hh:mm tt'),
			@Currentsubject =  Subject,
			@Currentdesc =  Remarks,
			@CurrentFuser =  fUser,
            @CurrentKeyword = Keyword,
            @CurrentIsAlert = IsAlert
		FROM Done where ID =@ID
		
	END
	Declare @CurrentResolution varchar(1000)
	Select @CurrentResolution =  result from done where ID =@ID


    DECLARE @taskID INT

    SELECT @taskID = Max([NewID]) + 1
    FROM   (SELECT Isnull(Max(ToDo.ID), 0) AS [NewID]
            FROM   ToDo
            UNION ALL
            SELECT Isnull(Max(done.ID), 0) AS [NewID]
            FROM   done) A

    IF( @Mode = 0 )
    BEGIN
      
        --if exists (select 1 from ToDo where Subject=@subject and Rol=@rol union select 1 from Done where Subject=@subject and Rol=@rol)
        --begin
        -- raiserror('Task with this subject already exists for the Contact.',16,1)
        -- return
        --end
      
        IF( @Status = 0 )
        BEGIN
            INSERT INTO ToDo
                        (ID,
                            Type,
                            Rol,
                            fDate,
                            fTime,
                            DateDue,
                            TimeDue,
                            Subject,
                            Remarks,
                            Keyword,
                            Level,
                            fUser,
                            fBy,
                            Duration,
                            Contact,
                            Source,
                            CreateDate,
                            CreatedBy,
                            LastUpdateDate,
                            LastUpdatedBy,
							Phone,
							Email,
                            Screen,
                            Ref,
                            IsAlert
                            )
            VALUES      ( @taskID,
                            0,
                            @rol,
                            Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                            Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                            + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                            @DateDue,
                            @TimeDue,
                            @subject,
                            @desc,
                            @Keyword,--'To Do',
                            1,
                            @Fuser,
                            @fby,
                            @Duration,
                            @contact,
                            '',
                            Getdate(),
                            @UpdateUser ,
                            Getdate(),
                            @UpdateUser,
							@Phone,
							@Email,
                            @Screen,
                            @Ref,
                            @IsAlert
                            )

            IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			    RAISERROR ('Error on insert task', 16, 1)  
			    ROLLBACK TRANSACTION    
			    RETURN
			END
        END
        ELSE
        BEGIN
            INSERT INTO Done
                        (ID,
                            Type,
                            Rol,
                            fDate,
                            fTime,
                            Datedone,
                            Timedone,
                            Subject,
                            Remarks,
                            Keyword,
                            fUser,
                            fBy,
                            Duration,
                            Contact,
                            Source,
                            Result,
                            CreateDate,
                            CreatedBy,
                            LastUpdateDate,
                            LastUpdatedBy,
							Phone,
							Email,
                            Screen,
                            Ref,
                            IsAlert
                            )
            VALUES      ( @taskID,
                            0,
                            @rol,
                            Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                            Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                            + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                            @DateDue,
                            @TimeDue,
                            @subject,
                            @desc,
                            @Keyword,--'To Do',
                            @Fuser,
                            @fby,
                            @Duration,
                            @contact,
                            '',
                            @Resolution,
                            Getdate(),
                            @UpdateUser,
                            Getdate(),
                            @UpdateUser,
							@Phone,
							@Email,
                            @Screen,
                            @Ref,
                            @IsAlert
                            )

            IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			    RAISERROR ('Error on insert task', 16, 1)  
			    ROLLBACK TRANSACTION    
			    RETURN
			END
        END
    END
    ELSE IF( @Mode = 1 )
    BEGIN
        SET @taskID = @ID
        IF( @Status = 0 )
        BEGIN
            
			--if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID union select 1 from Done where Subject=@subject and Rol=@rol and ID<>@ID)
			--  begin
			--   raiserror('Task with this subject already exists for the Contact.',16,1)
			--   return
			--  end
            
            UPDATE ToDo
            SET    Rol = @rol,
                    --fDate = Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                    --fTime = Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                    --        + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                    DateDue = @DateDue,
                    TimeDue = @TimeDue,
                    Subject = @subject,
                    Remarks = @desc,
                    fUser = @Fuser,
                    LastUpdateDate = Getdate(),
                    LastUpdatedBy = @UpdateUser,
					Duration = @Duration,
					Contact = @contact,
					Phone = @Phone,
					Email = @Email,
                    Keyword = @Keyword,
                    IsAlert = @IsAlert
            WHERE  ID = @ID

            IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			    RAISERROR ('Error on update task', 16, 1)  
			    ROLLBACK TRANSACTION    
			    RETURN
			END
        END
        ELSE
        BEGIN
            IF EXISTS (SELECT 1
                        FROM   done
                        WHERE  ID = @ID)
            BEGIN
                  
				--if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID 
				--union select 1 from Done where Subject=@subject and Rol=@rol and ID<>@ID)
				--  begin
				--   raiserror('Task with this subject already exists for the Contact.',16,1)
				--   return
				--  end
					  
                UPDATE done
                SET    Rol = @rol,
                        --fDate = Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                        --fTime = Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                        --        + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                        Datedone = @DateDue,
                        Timedone = @TimeDue,
                        Subject = @subject,
                        Remarks = @desc,
                        fUser = @Fuser,
                        result = @Resolution,
                        LastUpdateDate = Getdate(),
                        LastUpdatedBy = @UpdateUser,
						Duration = @Duration,
						Contact = @contact,
						Phone = @Phone,
						Email = @Email,
                        Keyword = @Keyword,
                        IsAlert = @IsAlert
                WHERE  ID = @ID

                IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			    BEGIN  
			        RAISERROR ('Error on update task', 16, 1)  
			        ROLLBACK TRANSACTION    
			        RETURN
			    END
            END
            ELSE
            BEGIN
                -- if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID union
                -- select 1 from Done where Subject=@subject and Rol=@rol)
				--begin
				-- raiserror('Task with this subject already exists for the Contact.',16,1)
				-- ROLLBACK TRANSACTION   
				-- return
				--end
					  
                INSERT INTO done
                            (ID,
                            Type,
                            Rol,
                            fDate,
                            fTime,
                            Datedone,
                            Timedone,
                            Subject,
                            Remarks,
                            Keyword,
                            fUser,
                            fBy,
                            Duration,
                            Contact,
                            Source,
                            Result,
                            CreateDate,
                            CreatedBy,
                            LastUpdateDate,
                            LastUpdatedBy,
							Phone,
							Email,
                            Screen,
                            Ref,
                            IsAlert
                            )
                VALUES      ( @ID,
                            0,
                            @rol,
                            (SELECT fDate
                                FROM   todo
                                WHERE  id = @id),
                            (SELECT fTime
                                FROM   todo
                                WHERE  id = @id),
                            @DateDue,
                            @TimeDue,
                            @subject,
                            @desc,
                            @Keyword,--'To Do',
                            @Fuser,
                            @fby,
                            0.00,
                            @contact,
                            '',
                            @Resolution,
                            (SELECT createdate FROM   todo WHERE  id = @id),
                            (SELECT createdby FROM   todo WHERE  id = @id),
                            Getdate(),
                            @UpdateUser,
                            @Phone,
							@Email,
                            @Screen,
                            @Ref,
                            @IsAlert
                            )
                      
                IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				BEGIN  
				    RAISERROR ('Error Occured', 16, 1)  
				    ROLLBACK TRANSACTION    
				    RETURN
				END
					
				DELETE FROM ToDo WHERE  ID = @ID
                                           
                IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
					RETURN
				END
            END
        END
    END 

    -- Insert the task contact to Phone table
	IF(ISNULL(@contact,'') != '')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact)
		BEGIN 
			INSERT INTO Phone
			(
				Rol,fDesc,Phone,Fax,Cell,Email
			)
			VALUES
			(
				@Rol,@contact,@phone,'','',@Email
			)
		END
        ELSE
        BEGIN
            UPDATE Phone SET Phone = @Phone, Email = @Email WHERE Rol =@Rol and fDesc = @contact
        END

        IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error on insert contact', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END
	END	 
				 

	/********Start Logs************/
    BEGIN TRY
        Declare @Val varchar(1000)
        if(@rol is not null And @rol != 0)
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Name' order by CreatedStamp desc )		
	        Declare @CurrentName varchar(150)
	        Select @CurrentName = Tag From Loc Where Rol = @rol
	        if(@Val<>@CurrentName)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Name',@Val,@CurrentName
	        end
	        Else IF (@Currentrol <> @CurrentName)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Name',@Currentrol,@CurrentName
	        END
	        Else IF (@Val is null And @rol != 0 And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Name','',@CurrentName
	        END
	    end
        set @Val=null
        if(@contact is not null And @contact != '')
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Contact' order by CreatedStamp desc )		
	        if(@Val<>@contact)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Contact',@Val,@contact
	        end
	        Else IF (@CurrentContact <>  @contact)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Contact',@CurrentContact,@contact
	        END
	        Else IF (@Val is null And @contact != '' And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Contact','',@contact
	        END
	    end

        set @Val=null
        if(@DateDue is not null)
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Due Date' order by CreatedStamp desc )		
	        Declare @DateDueCurrent varchar(50)
	        Select @DateDueCurrent = CONVERT(varchar(50), @DateDue, 101) 	
	        if(@Val<>@DateDueCurrent)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Due Date',@Val,@DateDueCurrent
	        end
	        Else IF (@CurrentDateDue <> @DateDueCurrent)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Due Date',@CurrentDateDue,@DateDueCurrent
	        END
	        Else IF (@Val is null And @DateDue != '' And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Due Date','',@DateDueCurrent
	        END
	    end
	    set @Val=null
	    if(@TimeDue is not null)
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Time' order by CreatedStamp desc )		
	        Declare @Onsitetime nvarchar(150)
	        SELECT @Onsitetime = FORMAT(@TimeDue, 'hh:mm tt')
            if(@Val<>@Onsitetime)
            begin
                exec log2_insert @UpdateUser,'Tasks',@ID,'Time',@Val,@Onsitetime
            end
            Else IF (@CurrentTimeDue <> @Onsitetime)
            Begin
                exec log2_insert @UpdateUser,'Tasks',@ID,'Time',@CurrentTimeDue,@Onsitetime
            END
            Else IF (@Val is null And @TimeDue != '' And @Mode = 0)
            Begin
                exec log2_insert @UpdateUser,'Tasks',@taskID,'Time','',@Onsitetime
            END
        end

	    set @Val=null
	    if(@subject is not null And @subject != '')
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Subject' order by CreatedStamp desc )		
	        if(@Val<>@subject)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Subject',@Val,@subject
	        end
	        Else IF (@Currentsubject <>  @subject)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Subject',@Currentsubject,@subject
	        END
	        Else IF (@Val is null And @subject != '' And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Subject','',@subject
	        END
	    end

        set @Val=null
        if(@desc is not null And @desc != '')
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Description' order by CreatedStamp desc )		
	        if(@Val<>@desc)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Description',@Val,@desc
	        end
	        Else IF (@Currentdesc <> @desc)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Description',@Currentdesc,@desc
	        END
	        Else IF (@Val is null And @desc != '' And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Description','',@desc
	        END
	    end

        set @Val=null
        if(@Resolution is not null And @Resolution != '')
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Resolution' order by CreatedStamp desc )		
	        if(@Val<>@Resolution)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Resolution',@Val,@Resolution
	        end
	        Else IF (@CurrentResolution <> @Resolution)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Resolution',@CurrentResolution,@Resolution
	        END
	        Else IF (@Val is null And @Resolution != '' And @status =1)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Resolution','',@Resolution
	        END
	        Else IF (@Val is null And @Resolution != '' And @Mode =0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Resolution','',@Resolution
	        END
	    end

        set @Val=null
        if(@status is not null)
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Status' order by CreatedStamp desc )
	        Declare @StatusVal varchar(50)
	        Select @StatusVal = Case When @status = 0 Then 'Open' Else 'Completed' END
	        if(@Val<>@StatusVal)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Status',@Val,@StatusVal
	        end
	        Else IF (@status = 0 And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Status','',@StatusVal
	        END
	    end

	    set @Val=null
	    if(@Fuser is not null And @Fuser != '')
	    begin 	
      	    Set @Val =(select Top 1 newVal  from log2 where screen='Tasks' and ref= @ID and Field='Assigned To' order by CreatedStamp desc )		
	        if(@Val<>@Fuser)
	        begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Assigned To',@Val,@Fuser
	        end
	        Else IF (@CurrentFuser <> @Fuser)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@ID,'Assigned To',@CurrentFuser,@Fuser
	        END
	        Else IF (@Val is null And @Fuser != '' And @Mode = 0)
	        Begin
	            exec log2_insert @UpdateUser,'Tasks',@taskID,'Assigned To','',@Fuser
	        END
	    end
        set @Val=null

        IF(ISNULL(@Phone,'') != ISNULL(@CurrentCPhone,''))
        BEGIN 	
	        EXEC log2_insert @UpdateUser,'Tasks',@ID,'Contact Phone',@CurrentCPhone,@Phone
        END

        IF(ISNULL(@Email,'') != ISNULL(@CurrentCEmail,''))
        BEGIN 	
	        EXEC log2_insert @UpdateUser,'Tasks',@ID,'Contact Email',@CurrentCEmail,@Email
        END

        IF(ISNULL(@Keyword,'') != ISNULL(@CurrentKeyword,''))
        BEGIN 	
	        EXEC log2_insert @UpdateUser,'Tasks',@ID,'Task Category',@CurrentKeyword,@Keyword
        END


        IF(ISNULL(@IsAlert,0) != ISNULL(@CurrentIsAlert,0))
        BEGIN 	
            DECLARE @strIsAlert varchar(10), @strCurrIsAlert varchar(10)
            IF ISNULL(@IsAlert,0) = 0
            BEGIN
                SET @strIsAlert = 'No'
                SET @strCurrIsAlert = 'Yes'
            END
            ELSE
            BEGIN
                SET @strIsAlert = 'Yes'
                SET @strCurrIsAlert = 'No'
            END
	        EXEC log2_insert @UpdateUser,'Tasks',@ID,'IsAlert',@strCurrIsAlert,@strIsAlert
        END
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE()

        IF @@TRANCOUNT > 0
            ROLLBACK
        RAISERROR ('An error has occurred on this page.', 16, 1)
        RETURN
    END CATCH
    SELECT @taskID
	/********End Logs************/
COMMIT TRANSACTION
GO