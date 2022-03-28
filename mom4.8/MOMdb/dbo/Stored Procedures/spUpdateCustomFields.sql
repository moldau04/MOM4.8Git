CREATE PROCEDURE [dbo].[spUpdateCustomFields]
	@Screen VARCHAR(50),
	@CustomItems AS tblTypeCommonCustomItem readonly,    
	@CustomItemsValue AS tblTypeCommonCustomItemValue readonly,
    @CustomItemsDelete AS tblTypeCommonCustomItem readonly
AS
BEGIN
BEGIN TRANSACTION
    DECLARE @tblCustomFieldsId int    
    DECLARE @Label VARCHAR(255)    
    DECLARE @Line SMALLINT    
    DECLARE @Value VARCHAR(255)    
    DECLARE @FieldFormat smallint    
    DECLARE @OrderNo int    
    DECLARE @IsAlert bit
    DECLARE @TeamMember varchar(max)   
    DECLARE @TeamMemberDisplay varchar(max)

    ---------------------------------- delete custom fields --------------------------------------------
    
    IF(SELECT TOP 1 1 FROM @CustomItemsDelete) = 1
    BEGIN    
        --UPDATE  c
        --SET c.IsDeleted = 1    
        --FROM tblCommonCustomFields c
        --INNER JOIN @CustomItemsDelete del ON del.ID = c.ID

        DELETE tblCommonCustomFields where ID in (SELECT ID FROM @CustomItemsDelete)

        DELETE FROM tblCommonCustomDefaultValues WHERE tblCommonCustomFieldsID in (SELECT ID FROM @CustomItemsDelete)

        DELETE tblCommonCustomFieldsValue WHERE tblCommonCustomFieldsID in (SELECT ID FROM @CustomItemsDelete)
    
        IF @@ERROR <> 0 
        BEGIN      
            RAISERROR ('Delete custom fields error', 16, 1)      
            IF @@TRANCOUNT > 0 
                ROLLBACK TRANSACTION        
            RETURN    
        END    
    END    

	---------------------------------- update/insert custom template -------------------------------    
    
    DECLARE db_cursor_custItem CURSOR FOR     
    
    SELECT [ID], [Label], [Line], [Format], OrderNo, IsAlert, TeamMember, TeamMemberDisplay FROM @CustomItems    
    OPEN db_cursor_custItem      
    FETCH NEXT FROM db_cursor_custItem INTO @tblCustomFieldsId, @Label, @Line, @FieldFormat ,@OrderNo, @IsAlert, @TeamMember, @TeamMemberDisplay
    WHILE @@FETCH_STATUS = 0    
    BEGIN     
    
        IF (@tblCustomFieldsId = '' OR @tblCustomFieldsId = 0)    
        BEGIN    
            ----------------------------------- insert custom template -----------------------------------    
            IF NOT EXISTS (SELECT TOP 1 1 FROM [tblCommonCustomFields] WHERE @Line = Line AND @Label = Label)
            BEGIN
                INSERT INTO [dbo].[tblCommonCustomFields] ([Screen], [Label], [Line], [Format],OrderNo, IsAlert, TeamMember, TeamMemberDisplay)    
                VALUES (@Screen, @Label, @Line, @FieldFormat,@OrderNo, @IsAlert, @TeamMember, @TeamMemberDisplay)    
      
                SET @tblCustomFieldsId=SCOPE_IDENTITY()    
    
                IF @@ERROR <> 0
                BEGIN      
                    RAISERROR ('Insert custom fields error', 16, 1)      
                    IF @@TRANCOUNT > 0 
                        ROLLBACK TRANSACTION        
                    RETURN    
                END    
    
                IF(SELECT TOP 1  1 FROM @CustomItemsValue WHERE Line = @Line) = 1    
                BEGIN    
      
                    INSERT INTO [dbo].[tblCommonCustomDefaultValues] (tblCommonCustomFieldsID, Line, Value)    
                    SELECT @tblCustomFieldsId, Line, Value FROM @CustomItemsValue WHERE Line = @Line    
      
                    IF @@ERROR <> 0
                    BEGIN      
                        RAISERROR ('Insert default value error', 16, 1)      
                        IF @@TRANCOUNT > 0 
                            ROLLBACK TRANSACTION        
                        RETURN    
                    END    
                END
            END
        END    
        ELSE    
        BEGIN    
            ----------------------------------- update custom template -----------------------------------    
      
            UPDATE [dbo].[tblCommonCustomFields]    
            SET [Label] = @Label    
                , [Line] = @Line    
                , [Format] = @FieldFormat
	            , OrderNo=@OrderNo    
	            , IsAlert = @IsAlert
	            , TeamMember = @TeamMember
	            , TeamMemberDisplay = @TeamMemberDisplay
            WHERE ID = @tblCustomFieldsId    
    
            IF @@ERROR <> 0    
            BEGIN      
                RAISERROR ('Update custom fields error', 16, 1)      
                IF @@TRANCOUNT > 0 
                    ROLLBACK TRANSACTION 
                RETURN    
            END    
            
            -- Delete the old default values
            DELETE FROM tblCommonCustomDefaultValues WHERE tblCommonCustomFieldsID = @tblCustomFieldsId    
    
            IF @@ERROR <> 0  
            BEGIN      
                RAISERROR ('Reset default value error', 16, 1)      
                IF @@TRANCOUNT > 0 
                    ROLLBACK TRANSACTION       
                RETURN    
            END    
    
            IF(SELECT TOP 1  1 FROM @CustomItemsValue WHERE Line = @Line) = 1    
            BEGIN    
                INSERT INTO [dbo].[tblCommonCustomDefaultValues](tblCommonCustomFieldsID, Line, Value)    
                SELECT @tblCustomFieldsId, Line, Value FROM @CustomItemsValue WHERE Line = @Line    
      
                IF @@ERROR <> 0    
                BEGIN      
                    RAISERROR ('Reset default value error', 16, 1)      
                    IF @@TRANCOUNT > 0 
                        ROLLBACK TRANSACTION        
                    RETURN    
                END    
            END    
        END     
    
        FETCH NEXT FROM db_cursor_custItem INTO @tblCustomFieldsId, @Label, @Line, @FieldFormat ,@OrderNo, @IsAlert, @TeamMember, @TeamMemberDisplay
    END      
    
    CLOSE db_cursor_custItem      
    DEALLOCATE db_cursor_custItem    
COMMIT
END