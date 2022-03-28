CREATE PROCEDURE [dbo].[MOM_GenericScript_For_Insert_DefaultData]
 AS

 -------------------
 ------------------- MOM Generic Script to insert Default Data
 -------------------

			     ------- UM -Each/Hours

				   IF NOT EXISTS (select 1 from UM where [fDesc]='Each') INSERT [dbo].[UM] ([fDesc]) VALUES (N'Each')

				   IF NOT EXISTS (select 1 from UM where [fDesc]='Hours') INSERT [dbo].[UM] ([fDesc]) VALUES (N'Hours')  

				   ---- $$$$ UNITOFMEASURE $$$$

					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='EA') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('EA','Each')
		  
		
					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='UN') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('UN','Unit')
		  
		 
					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='HR') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('HR','Hours')
		  
		
					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='M') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('M','Meters') 
		
		
					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='FT') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('FT','Feet')
		  
		
					IF NOT EXISTS (select 1 from UnitOfMeasure where UnitOfMeasureCode ='IN') INSERT INTO UnitOfMeasure (UnitOfMeasureCode,UnitOfMeasureDesc)Values('IN','Inches')
		 
		
		
					----- $$$ Setup Costing Types DDL $$$--
		
					 IF NOT EXISTS (SELECT 1 FROM CostTypes WHERE CostTypes ='Weighted average') BEGIN  IF NOT EXISTS (SELECT 1 FROM CostTypes WHERE InUse = 1)	 BEGIN INSERT INTO CostTypes (CostTypes,InUse)VALUES('Weighted average',1) END
					 ELSE BEGIN INSERT INTO CostTypes (CostTypes,InUse)VALUES('Weighted average',0) END	 END 
		
		
					IF NOT EXISTS (SELECT 1 FROM CostTypes WHERE CostTypes ='FIFO') INSERT INTO CostTypes (CostTypes,InUse)VALUES('FIFO',0)
		 

					 ----- Add Progess billing(% of completion) Record into Posting table does not contain value ------
		
					If NOT EXISTS (Select 1 from Posting where Post ='% of completion') Insert into Posting (Post, ID) values ('% of completion',2) -- Here 2 Is fixed.				 
		  
					  --------------------- ApplicationStatus for WIP
		
					If NOT EXISTS (Select 1 from ApplicationStatus where StatusName ='Open')	 begin	Insert into ApplicationStatus (StatusName, Id) values ('Open',1)  end

		
					If NOT EXISTS (Select 1 from ApplicationStatus where StatusName ='Submitted')	 begin	Insert into ApplicationStatus (StatusName, Id) values ('Submitted',2) 	 end

		
					If NOT EXISTS (Select * from ApplicationStatus where StatusName ='Approval')	 begin		Insert into ApplicationStatus (StatusName, Id) values ('Approval',3) 					 end

	
				   If NOT EXISTS (Select 1 from ApplicationURL where AppUrl ='http://localhost:10628/login.aspx')					 begin						Insert into ApplicationURL (AppUrl,PageName,DBName) values ('http://localhost:10628/login.aspx','ApprovePO','AUnNb2XBSGs=') 				 end
					 -------------------Safety Test Status Default VALUES 
	    
				   IF NOT EXISTS (SELECT 1 FROM ListConfig  )

					 BEGIN  
					INSERT [dbo].[ListConfig] ( [ListName], [ItemName], [ItemValue], [ItemCode], [ItemDesc], [DestTable], [DestField], [IsDefault]) VALUES ( N'Test.Status', N'Open', 0, N'RES_Y', N'Test does not have an open ticket.', N'LoadTestItem', N'Status', 1) 
        
					INSERT [dbo].[ListConfig] ([ListName], [ItemName], [ItemValue], [ItemCode], [ItemDesc], [DestTable], [DestField], [IsDefault]) VALUES (N'Test.Status', N'Assigned', 1, N'TKT', N'Test has an assigned ticket.', N'LoadTestItem', N'Status', 0) 
        
					INSERT [dbo].[ListConfig] ([ListName], [ItemName], [ItemValue], [ItemCode], [ItemDesc], [DestTable], [DestField], [IsDefault]) VALUES (N'Test.Status', N'Close', 2, N'RES_C', N'Test has all ticket complete.', N'LoadTestItem', N'Status', 0) 

       				INSERT [dbo].[ListConfig] ([ListName], [ItemName], [ItemValue], [ItemCode], [ItemDesc], [DestTable], [DestField], [IsDefault]) VALUES (N'Test.Status', N'InActive', 3, N'RES_I', N'Test is inactive.', N'LoadTestItem', N'Status', 0) 
					END 


					---------------Default Terms----------

					IF NOT EXISTS (SELECT 1 FROM tblTerms)

					BEGIN 

					SET IDENTITY_INSERT [dbo].[tblTerms] ON 

					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (0, N'Upon Receipt', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (1, N'Net 10 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (2, N'Net 15 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (3, N'Net 30 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (4, N'Net 45 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (5, N'Net 60 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (6, N'2%-10/Net 30 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (7, N'Net 90 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (8, N'Net 180 Days', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (9, N'COD', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (10, N'Net 120', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (11, N'Net 150', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (12, N'Net 210', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (13, N'Net 240', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (14, N'Net 270', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (15, N'Net 300', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (16, N'Net Due On 10th', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (17, N'Net Due', NULL, NULL)
					INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (18, N'Credit Card', NULL, NULL)

				   SET IDENTITY_INSERT [dbo].[tblTerms] OFF
	
				   END

        ------------------ CType 

       

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Asset') INSERT [dbo].[CType] ([Type]) VALUES (N'Asset')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Liability') INSERT [dbo].[CType] ([Type]) VALUES (N'Liability')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Equity') INSERT [dbo].[CType] ([Type]) VALUES (N'Equity')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Revenue') INSERT [dbo].[CType] ([Type]) VALUES (N'Revenue')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Cost') INSERT [dbo].[CType] ([Type]) VALUES (N'Cost')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Expense') INSERT [dbo].[CType] ([Type]) VALUES (N'Expense')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Bank') INSERT [dbo].[CType] ([Type]) VALUES (N'Bank')

					IF NOT EXISTS (SELECT 1 FROM [CType] where [Type]='Non-Posting') INSERT [dbo].[CType] ([Type]) VALUES (N'Non-Posting')
		 

       ------ [Status]

 

					IF NOT EXISTS (SELECT 1 FROM [Status] where [Status]='Active') INSERT [dbo].[Status] ([Status]) VALUES (N'Active')

					IF NOT EXISTS (SELECT 1 FROM [Status] where [Status]='Inactive') INSERT [dbo].[Status] ([Status]) VALUES (N'Inactive')

					IF NOT EXISTS (SELECT 1 FROM [Status] where [Status]='Hold') INSERT [dbo].[Status] ([Status]) VALUES (N'Hold')
 


      ------BOMT-----
 

 

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Materials') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Materials')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Labor') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Labor')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Sub-Contract') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Sub-Contract')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Permit') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Permit')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Consulting') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Consulting')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Equipment Rental') INSERT [dbo].[BOMT] (  [Type]) VALUES (  N'Equipment Rental')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Other') INSERT [dbo].[BOMT] (  [Type]) VALUES ( N'Other')

					IF NOT EXISTS (SELECT 1 FROM [BOMT] where [Type]='Inventory') INSERT [dbo].[BOMT] (  [Type]) VALUES ( N'Inventory') 

 
 
				  --------------------------------------- 
				  IF NOT EXISTS (SELECT 1 FROM State)

			      BEGIN

			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AB', N'Alberta', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AK', N'Alaska', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AL', N'Alabama', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AR', N'Arkansas', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AZ', N'Arizona', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'BC', N'British Columbia', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CA', N'California', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CO', N'Colorado', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CT', N'Connecticut', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'DC', N'Washington DC', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'DE', N'Delaware', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'FL', N'Florida', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'GA', N'Georgia', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'HI', N'Hawaii', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IA', N'Iowa', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ID', N'Idaho', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IL', N'Illinois', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IN', N'Indiana', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'KS', N'Kansas', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'KY', N'Kentucky', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'LA', N'Louisiana', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MA', N'Massachusetts', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MB', N'Manitoba', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MD', N'Maryland', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ME', N'Maine', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MI', N'Michigan', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MN', N'Minnesota', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MO', N'Missouri', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MS', N'Mississippi', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MT', N'Montana', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NB', N'New Brunswick', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NC', N'North Carolina', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ND', N'North Dakota', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NE', N'Nebraska', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NF', N'Newfoundland', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NH', N'New Hampshire', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NJ', N'New Jersey', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NM', N'New Mexico', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NS', N'Nova Scotia', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NV', N'Nevada', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NY', N'New York', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OH', N'Ohio', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OK', N'Oklahoma', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ON', N'Ontario', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OR', N'Oregon', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PA', N'Pennsylvania', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PE', N'Prince Edward Island', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PQ', N'Quebec', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'RI', N'Rhode Island', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SC', N'South Carolina', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SD', N'South Dakota', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SK', N'Saskatchewan', N'Canada')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'TN', N'Tennessee', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'TX', N'Texas', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'UT', N'Utah', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'VA', N'Virginia', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'VT', N'Vermont', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WA', N'Washington', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WI', N'Wisconsin', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WV', N'West Virginia', N'USA')
			INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WY', N'Wyoming', N'USA')


			END
			      ---------------------------------------

				  IF NOT EXISTS (SELECT 1 FROM Inv where name='Recurring')  INSERT INTO Inv    (Name,    fDesc,     Cat,              Balance,        Measure,              tax,    AllowZero,              inuse,   type,      sacct,    Remarks, Status, Price1)  
			      VALUES      ( 'Recurring', 'Recurring',   0,   0,  '',      0,   0,    0,   1,     10,    '', 0,0) 
  
				  IF NOT EXISTS (SELECT 1 FROM Inv where name='Expenses')
			      INSERT INTO Inv             (Name,              fDesc,              Cat,              Balance,              Measure,              tax,              AllowZero,              inuse,              type,              sacct,              Remarks, Status, Price1) 
			      VALUES      ( 'Expenses',             'Expenses',               0,               0,               '',               0,               0,               0,               1,               10,               '', 0,0) 
  
				  IF NOT EXISTS (SELECT 1 FROM Inv where name='Mileage')
			      INSERT INTO Inv             (Name,              fDesc,              Cat,              Balance,              Measure,              tax,              AllowZero,              inuse,              type,              sacct,              Remarks, Status, Price1) 
			      VALUES      ( 'Mileage',               'Mileage',              0,               0,               '',               0,               0,               0,               1,               10,               '', 0,0) 
  
				  IF NOT EXISTS (SELECT 1 FROM Inv where name='Time Spent')  
				  INSERT INTO Inv             (Name,              fDesc,              Cat,              Balance,              Measure,              tax,              AllowZero,              inuse,              type,              sacct,              Remarks, Status, Price1) 
			      VALUES      ( 'Time Spent',               'Time Spent',               0,               0,               '',               0,               0,               0,               1,               10,               '', 0,0) 

			 
			 --------Estimate Status 
 
					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Open') INSERT [dbo].[OEStatus] ([Name]) VALUES ( N'Open')
 
					 UPDATE [dbo].[OEStatus] SET [Name] = N'Cancelled' WHERE Name='Canceled'
					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Cancelled') INSERT [dbo].[OEStatus] ([Name]) VALUES (N'Cancelled')
 
					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Withdrawn') INSERT [dbo].[OEStatus] ([Name]) VALUES (N'Withdrawn')
 
					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Disqualified') INSERT [dbo].[OEStatus] (  [Name]) VALUES (N'Disqualified')

					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Sold') INSERT [dbo].[OEStatus] ( [Name]) VALUES (N'Sold')

					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Competitor') INSERT [dbo].[OEStatus] ([Name]) VALUES (N'Competitor')

					 IF NOT EXISTS (SELECT 1 FROM [OEStatus] WHERE Name='Quoted') INSERT [dbo].[OEStatus] ([Name]) VALUES (N'Quoted')

   
 
			 ----- project tables setup -----------------------------------------------------------
  
			  -------------[JobCode]

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='100') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (N'100', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='200') INSERT [dbo].[JobCode] (  [Code], [IsDefault]) VALUES (  N'200', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='300') INSERT [dbo].[JobCode] (  [Code], [IsDefault]) VALUES (  N'300', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='400') INSERT [dbo].[JobCode] (  [Code], [IsDefault]) VALUES (  N'400', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='500') INSERT [dbo].[JobCode] (  [Code], [IsDefault]) VALUES ( N'500', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='600') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (  N'600', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='700') INSERT [dbo].[JobCode] (  [Code], [IsDefault]) VALUES (  N'700', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='800') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (  N'800', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='900') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (  N'900', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='1000') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (  N'1000', NULL)

				IF NOT EXISTS (SELECT 1 FROM [JobCode] WHERE Code='999') INSERT [dbo].[JobCode] ( [Code], [IsDefault]) VALUES (  N'999', 1)


			---[JStatus]


				IF NOT EXISTS (SELECT 1 FROM [JStatus] WHERE [Status]='Open') INSERT [dbo].[JStatus] ([Status]) VALUES (N'Open')

				IF NOT EXISTS (SELECT 1 FROM [JStatus] WHERE [Status]='Closed') INSERT [dbo].[JStatus] ([Status]) VALUES (N'Closed')

				IF NOT EXISTS (SELECT 1 FROM [JStatus] WHERE [Status]='Hold') INSERT [dbo].[JStatus] ([Status]) VALUES (N'Hold')

				IF NOT EXISTS (SELECT 1 FROM [JStatus] WHERE [Status]='Completed') INSERT [dbo].[JStatus] ([Status]) VALUES (N'Completed')

 
 
	 ----- project tables setup -----------------------------------------------------------
  
   

			SET IDENTITY_INSERT [dbo].[OrgDep] ON 

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Finance') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (1, N'Finance')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Supply Chain') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (2, N'Supply Chain')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Engineering') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (3, N'Engineering')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Sales') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (4, N'Sales')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Legal') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (5, N'Legal')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Design') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (6, N'Design')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='QA & Inspections') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (7, N'QA & Inspections')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Customer') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (8, N'Customer')

			IF NOT EXISTS (SELECT 1 FROM [OrgDep] WHERE [Department]='Vendor') INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (9, N'Vendor')


			SET IDENTITY_INSERT [dbo].[OrgDep] OFF

 

  ------ [Posting]  

 

			IF NOT EXISTS (SELECT 1 FROM [Posting] WHERE [Post]='Real Time')  INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'Real Time', 0)

			IF NOT EXISTS (SELECT 1 FROM [Posting] WHERE [Post]='On Closing')  INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'On Closing', 1)

			IF NOT EXISTS (SELECT 1 FROM [Posting] WHERE [Post]='% of completion')  INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'% of completion', 2)

 
 ---------  [tblTabs]

		    IF NOT EXISTS (SELECT 1 FROM [tblTabs] )

		    BEGIN

		    SET IDENTITY_INSERT [dbo].[tblTabs] ON  

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (1, 28, N'Header')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (2, 28, N'Attributes - General')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (3, 28, N'Attributes - GC Info')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (4, 28, N'Attributes - Equipment')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (5, 28, N'Finance - General')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (6, 28, N'Finance - Billing')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (7, 28, N'Finance - Budgets')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (8, 28, N'Ticketlist')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (9, 28, N'BOM')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (10, 28, N'Milestones')

			INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (11, 28, N'Task')

			SET IDENTITY_INSERT [dbo].[tblTabs] OFF

		    END

--------- PType

	          IF NOT EXISTS ( SELECT 1 FROM PType WHERE Type='Mobile Service')  INSERT INTO PType  (Type) VALUES ('Mobile Service') 


	          IF NOT EXISTS ( SELECT 1 FROM OType WHERE Type='General Contractor')  INSERT INTO OType      (Type) VALUES      ('General Contractor') 

			  --- ES- 1592 New db - MUST HAVE JOB TYPE RECURRING, JOB TEMPLATE RECURRING 


	          --------  Default Recurring   Category

			  DECLARE @IsDefaultCategory int =0;

			  IF NOT EXISTS ( SELECT 1 FROM Category WHERE ISDefault=1)    SET  @IsDefaultCategory=1;

			  IF NOT EXISTS ( SELECT 1 FROM Category WHERE Type='Recurring')    INSERT INTO Category   ( Type,  Remarks ,ISDefault) VALUES      ( 'Recurring',  'Recurring' , @IsDefaultCategory) 

			 ----- Default Recurring Job type

			  DECLARE @IsDefaultJobType int =0;

			  IF NOT EXISTS ( SELECT 1 FROM JobType WHERE ISDefault=1)    SET  @IsDefaultJobType=1;
			 
       
			  SET IDENTITY_INSERT JobType ON 

			  IF NOT EXISTS ( SELECT 1 FROM JobType WHERE ID = 0) 
			  BEGIN  INSERT INTO JobType  (ID, Type,  Remarks ,Count,Color  ,IsDefault)  	VALUES ( 0, 'Recurring',  'Recurring',0 , 0 ,@IsDefaultJobType )  END 	

			  SET IDENTITY_INSERT JobType OFF

			  --------Default JOB TEMPLATE RECURRING 

			 IF NOT EXISTS ( SELECT 1 FROM JobT WHERE Type = 0) 

             BEGIN

             SET IDENTITY_INSERT [dbo].[JobT] OFF 
 
             INSERT [dbo].[JobT] (  [fDesc],    [Type],  [Remarks] ,Status ,CType   ) 

             VALUES     ( N'Recurring',   0,    N'Recurring' ,0 , ( SELECT top 1 t.Type  FROM JobType t  WHERE t.ID = 0) )
  
             SET IDENTITY_INSERT [dbo].[JobT] on

             DECLARE @JobT int;

SELECT top 1 @JobT=ID FROM JobT WHERE Type = 0
 
INSERT [dbo].[JobTItem] (  [JobT], [Job], [Type], [fDesc], [Code], [Actual], [Budget], [Line], [Percent], [Comm], 
[Stored], [Modifier], [ETC], [ETCMod], [THours], [FC], [Labor], [BHours], [GL], [OrderNo] )
VALUES (  @JobT, 0, 1, N'Materials', N'100', CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 1, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, NULL, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, 1 )

INSERT [dbo].[JobTItem] (  [JobT], [Job], [Type], [fDesc], [Code], [Actual], [Budget], [Line], [Percent], [Comm], 
[Stored], [Modifier], [ETC], [ETCMod], [THours], [FC], [Labor], [BHours], [GL], [OrderNo]  ) 
VALUES (  @JobT, 0, 1, N'Labor', N'200', CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 2, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, NULL, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, 2 )
 

INSERT [dbo].[JobTItem] (  [JobT], [Job], [Type], [fDesc], [Code], [Actual], [Budget], [Line], [Percent], [Comm], 
[Stored], [Modifier], [ETC], [ETCMod], [THours], [FC], [Labor], [BHours], [GL], [OrderNo]  ) 
VALUES (  @JobT, 0, 0, N'Revenue', N'200', CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 2, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, NULL, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, 2 )
 


INSERT INTO BOM(JobTItemID,Type,UM,QtyRequired )

SELECT   ID , 1 ,'Each' ,1 from JobTItem where Job =0 and JobT=@JobT and fDesc='Materials'  and type=1 and isnull(id,0) 
not in (select isnull(JobTItemID,0) from BOM where JobTItemID is not null )

INSERT INTO BOM(JobTItemID,Type)

SELECT   ID , 2 from JobTItem where Job =0 and JobT=@JobT and fDesc='Labor'  and type=1 and isnull(id,0) 
not in (select isnull(JobTItemID,0) from BOM where JobTItemID is not null )

INSERT INTO Milestone(JobTItemID,Type,MilestoneName)

SELECT   ID , 0 ,'Revenue' from JobTItem where Job =0 and JobT=@JobT and fDesc='Revenue'  and type=0 and isnull(id,0) 
not in (select isnull(JobTItemID,0) from Milestone where JobTItemID is not null )
 

END

              --------/////
			  
			         -----///// Insert Default Value OFC where WarehouseID is null for Inventory tracking 

			  -------/////
  
			 IF NOT EXISTS (select 1 from Warehouse where ID ='OFC')  INSERT INTO Warehouse (ID,Name,Remarks,Type,Count,Location)Values('OFC','Office Warehouse','Office Warehouse',0,0,0)

            update IWarehouseLocAdj set WarehouseID='OFC'   where isnull(WarehouseID,'') = ''

            update POItem set WarehouseID='OFC'  where ISNULL(Job,0)=0 and TypeID=(Select TOP 1 ID FROM BOMT WHERE Type='Inventory') and isnull(WarehouseID,'') = ''

            update IAdj set WarehouseID='OFC' where   isnull(WarehouseID,'') = ''


        -- ===================================================
        -- Author:	<Harsh>
        -- Date: <23-FEB-2019>
        -- Description:	<Set true if Status column is null> 
       -- ===================================================

       IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'route'  AND COLUMN_NAME = 'Status') 
       BEGIN  
	   UPDATE Route SET Status=1 WHERE status IS NULL
       END


	     ---------  [Custom]

		    IF NOT EXISTS (SELECT 1 FROM [Custom] )

		    BEGIN
 
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'AP1099', N'0', NULL, 1)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APCLimit', N'$0.00', NULL, 2)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDays', N'10', NULL, 3)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDetail', N'0', NULL, 4)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDisc', N'0.00', NULL, 5)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APItemQuan', N'1', NULL, 7)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APStatus', N'0', NULL, 8)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APTerms', N'3', NULL, 9)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APType', N'Cost of Sales', NULL, 10)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APVoid', N'1', NULL, 11)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BankBalance', N'0', NULL, 13)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BonusGL', N'12', NULL, 14)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Branch', N'0', NULL, 15)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BranchNo', N'00', NULL, 16)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV1', N'Budget1', NULL, 17)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV2', N'Budget2', NULL, 18)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV3', N'Budget3', NULL, 19)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'CollAging', N'30', NULL, 23)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Country', N'0', NULL, 24)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Customiz2', N'0', NULL, 26)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'CustType', N'General', NULL, 27)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DArea', N'720', NULL, 28)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc1', N'Discount 1', NULL, 29)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc2', N'Discount 2', NULL, 30)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc3', N'Discount 3', NULL, 31)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc4', N'Discount 4', NULL, 32)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc5', N'Discount 5', NULL, 33)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc6', N'Discount 6', NULL, 34)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DispNature', N'0', NULL, 35)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DispSource', N'None', NULL, 36)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DState', N'CO', NULL, 37)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ECurrent', N'31', NULL, 39)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev1', N'0', NULL, 40)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev10', N'Custom10', NULL, 41)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev11', N'Custom11', NULL, 42)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev12', N'Custom12', NULL, 43)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev13', N'Custom13', NULL, 44)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev14', N'Custom14', NULL, 45)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev15', N'Custom15', NULL, 46)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev2', N'Custom2', NULL, 47)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev3', N'Custom3', NULL, 48)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev4', N'Custom4', NULL, 49)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev5', N'Custom5', NULL, 50)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev6', N'Custom6', NULL, 51)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev7', N'Custom7', NULL, 52)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev8', N'Custom8', NULL, 53)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev9', N'Custom9', NULL, 54)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'EMail', N'info@automatedintegration.com', NULL, 55)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'EndTime', N'05:00 PM', NULL, 56)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'FederalID', N'11-111111', NULL, 57)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ForceAddress', N'False', NULL, 58)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ForcePhoneNumber', N'False', NULL, 59)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLCOST', N'40', NULL, 64)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLOU', N'9', NULL, 67)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLPR', N'7', NULL, 68)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLPRE', N'98', NULL, 70)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLSALES', N'10', NULL, 71)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GSTGL', N'9', NULL, 74)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GSTRate', N'0.00', NULL, 75)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'HolidayGL', N'12', NULL, 76)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvGL', N'0', NULL, 81)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvGL', N'0', NULL, 82)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvItemDes', N'0', NULL, 83)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvoiceVerify', N'', NULL, 84)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvTicket', N'2', NULL, 85)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job1', N'Custom1', NULL, 86)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job10', N'Custom10', NULL, 87)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job11', N'Custom11', NULL, 88)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job12', N'Custom12', NULL, 89)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job13', N'Custom13', NULL, 90)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job14', N'Custom14', NULL, 91)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job15', N'Custom15', NULL, 92)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job2', N'Custom2', NULL, 93)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job3', N'Custom3', NULL, 94)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job4', N'Custom4', NULL, 95)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job5', N'Custom5', NULL, 96)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job6', N'Custom6', NULL, 97)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job7', N'Custom7', NULL, 98)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job8', N'Custom8', NULL, 99)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job9', N'Custom9', NULL, 100)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastCalc', N'01/01/2003', NULL, 102)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastInt', N'0', NULL, 103)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastRun', N'4/01/2011', NULL, 104)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastRunT', N'5/1/2011', NULL, 105)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc1', N'Custom 1', NULL, 106)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc10', N'Custom 10', NULL, 107)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc2', N'Custom 2', NULL, 108)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc3', N'Custom 3', NULL, 109)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc4', N'Custom 4', NULL, 110)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc5', N'Custom 5', NULL, 111)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc6', N'Custom 6', NULL, 112)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc7', N'Custom 7', NULL, 113)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc8', N'Custom 8', NULL, 114)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc9', N'Custom 9', NULL, 115)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDRoute', N'2', NULL, 116)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDSTax', N'CO', NULL, 117)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDSType', N'STANDARD', NULL, 118)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDTerr', N'2', NULL, 119)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDZone', N'1', NULL, 121)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocType', N'Non-Contract', NULL, 122)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoginN', N'1', NULL, 123)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoginY', N'1', NULL, 124)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallAssigned', N'', 0, 127)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallClosed', N'', 0, 128)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallUnassigned', N'', 0, 129)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolHotspots', N'', 0, 130)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolInternal', N'', 0, 131)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolWorker', N'', 0, 132)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MaxCheck', N'15', NULL, 133)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MileRate', N'0.2800', NULL, 136)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ModuleN', N'0', NULL, 137)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ModuleY', N'0', NULL, 138)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextApply', N'34', NULL, 139)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextBatch', N'80', NULL, 140)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextDep', N'10', NULL, 141)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextInv', N'48', NULL, 142)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextJob', N'37', NULL, 143)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextQuote', N'4', NULL, 145)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextTicket', N'265', NULL, 146)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpenCallOptionElapsed', N'False', NULL, 148)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpenCallOptionPastDue', N'False', NULL, 149)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpeningOption', N'OpeningThisWeek', NULL, 150)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionCallVoid', N'False', NULL, 151)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionEnableUser', N'', NULL, 152)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionIncludeHotspot', N'False', NULL, 153)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionMinimize', N'False', NULL, 154)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionTimeStampClose', N'False', NULL, 155)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Owner1', N'Custom1', NULL, 156)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Owner2', N'Custom2', NULL, 157)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PageA', N'0', NULL, 158)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PageT', N'0', NULL, 159)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PDAON', N'1', NULL, 160)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PMarkup', N'0', NULL, 161)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PMarkup', N'0', NULL, 162)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PO1', N'Custom1', NULL, 163)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PO2', N'Custom2', NULL, 164)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'POApprove', N'0', NULL, 165)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRAmount', N'2535.45', NULL, 167)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFICA', N'-0.000000000000212', NULL, 168)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFIT', N'0', NULL, 169)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFUTA', N'0', NULL, 170)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBLocal', N'0', NULL, 171)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBMEDI', N'0', NULL, 172)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBSIT', N'0', NULL, 173)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCFactor', N'10.80', NULL, 174)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCLapsed', N'1', NULL, 175)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCRange', N'2', NULL, 176)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCSTax', N'1', NULL, 177)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCycle', N'0', NULL, 178)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCZero', N'0', NULL, 179)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price1', N'50', NULL, 180)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price2', N'100', NULL, 181)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price3', N'300', NULL, 182)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price4', N'1000', NULL, 183)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD1', N'0', NULL, 184)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD2', N'0', NULL, 185)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD3', N'0', NULL, 186)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD4', N'0', NULL, 187)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD5', N'0', NULL, 188)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRLast', N'2/20/2011', NULL, 189)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRLast2', N'2/26/2011', NULL, 190)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ProgramCustom', N'0', NULL, 191)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PromptForTime', N'False', NULL, 192)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRQuest', N'0', NULL, 193)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFICA', N'2', NULL, 194)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFIT', N'2', NULL, 195)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFUTA', N'2', NULL, 196)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRLOCAL', N'0', NULL, 197)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRMEDI', N'2', NULL, 198)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRSIT', N'2', NULL, 199)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote1', N'Custom1', NULL, 200)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote2', N'Custom2', NULL, 201)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ReconInt', N'21', NULL, 202)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ReconSC', N'21', NULL, 203)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRound', N'0', NULL, 204)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResroundA2', N'1', NULL, 205)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundAM', N'0', NULL, 206)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundOT', N'1', NULL, 207)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResroundP2', N'05:00 PM', NULL, 208)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundPM', N'05:00 PM', NULL, 209)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundTo', N'0', NULL, 210)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Return', N'0.00', NULL, 211)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleAutoSizeColumns', N'True', NULL, 212)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleAutoSizeRows', N'False', NULL, 213)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleConfirmBar', N'False', NULL, 214)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleFindSlotWeekend', N'False', NULL, 215)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleOpenView', N'OptionViewTag', NULL, 216)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleShowIcons', N'False', NULL, 217)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleTimeIncrement', N'30 Minutes', NULL, 218)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleTimeRange', N'Whole Day', NULL, 219)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleWarnConflict', N'False', NULL, 220)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowCType', N'0', NULL, 221)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowWorkerType', N'', NULL, 222)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowWorkerZone', N'', NULL, 223)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'StartTime', N'08:00 AM', NULL, 224)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'StateID', N'11-1111111', NULL, 225)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Terms', N'3', NULL, 226)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket1', N'PO#', NULL, 227)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket2', N'Custom2', NULL, 228)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket3', N'Custom3', NULL, 229)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket4', N'Custom4', NULL, 230)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket5', N'Custom5', NULL, 231)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vacation', N'0', NULL, 232)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'VacationGL', N'12', NULL, 233)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle1', N'Custom1', NULL, 234)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle2', N'Custom2', NULL, 235)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle3', N'Custom3', NULL, 236)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle4', N'Custom4', NULL, 237)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle5', N'Custom5', NULL, 238)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor1', N'Custom1', NULL, 239)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor2', N'Custom2', NULL, 240)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor3', N'Custom3', NULL, 241)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor4', N'Custom4', NULL, 242)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor5', N'Custom5', NULL, 243)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'VerifyAddress', N'', NULL, 244)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ViewRolodex', N'0', NULL, 245)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job16', N'Custom16', NULL, 246)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job17', N'Custom17', NULL, 247)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job18', N'Custom18', NULL, 248)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job19', N'Custom19', NULL, 249)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job20', N'Custom20', NULL, 250)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc11', N'Custom11', NULL, 251)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc12', N'Custom12', NULL, 252)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc13', N'Custom13', NULL, 253)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc14', N'Custom14', NULL, 254)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc15', N'Custom15', NULL, 255)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastCost', N'0', NULL, 256)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'POGrid', N'0', NULL, 257)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OHPercent', N'3.5000', NULL, 258)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor6', N'Custom6', NULL, 260)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor7', N'Custom7', NULL, 261)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor8', N'Custom8', NULL, 262)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor9', N'Custom9', NULL, 263)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor10', N'Custom10', NULL, 264)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect1', N'Custom1', NULL, 265)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect2', N'Custom2', NULL, 266)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect3', N'Custom3', NULL, 267)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect4', N'Custom4', NULL, 268)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect5', N'Custom5', NULL, 269)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Estimate1', N'Custom1', NULL, 270)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Estimate2', N'Custom2', NULL, 271)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TKStatus', N'0', NULL, 272)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OHCharge', N'10/31/2008', NULL, 273)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote3', N'Custom3', NULL, 274)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp1', N'Custom1', NULL, 275)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp2', N'Custom2', NULL, 276)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp3', N'Custom3', NULL, 277)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp4', N'Custom4', NULL, 278)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp5', N'Custom5', NULL, 279)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DBColor', N'14', NULL, 280)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DBColor2', N'12', NULL, 281)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'JobEsc', N'3', NULL, 282)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket6', N'Diag Only', NULL, 283)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket7', N'BIO', NULL, 284)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket8', N'USA', NULL, 285)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket9', N'Custom9', NULL, 286)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket10', N'Custom10', NULL, 287)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MultiTravel', N'0', NULL, 288)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntRate', N'0', NULL, 289)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntPer', N'0', NULL, 290)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntComp', N'0', NULL, 291)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst1', N'TicketCustom1', NULL, 292)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst2', N'TicketCustom2', NULL, 293)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst3', N'TicketCheckbox1', NULL, 294)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst4', N'TicketCheckbox2', NULL, 295)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoadTest1', N'Custom1', NULL, 296)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoadTest2', N'Custom2', NULL, 297)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoadTest3', N'Custom3', NULL, 298)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoadTest4', N'Custom4', NULL, 299)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PJ1', N'Custom1', NULL, 300)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PJ2', N'Custom2', NULL, 301)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DefaultInvGLAcct', NULL, NULL, 302)
END
	   

----------------ES - 1224 Multiple Estimate convert to a  Project for an Opportunity with Group Name

If Exists (SELECT  1  FROM  Estimate  where isnull(job,0) <> 0 and isnull(Opportunity,0) <> 0 and id not in (SELECT EstimateID FROM  tblEstimateConvertToProject ) )

BEGIN

INSERT INTO tblEstimateConvertToProject(ProjectID,EstimateID,OpportunityID,IsFinancialDataConverted)
SELECT job,ID,Opportunity,1  FROM  Estimate  
where isnull(job,0) <> 0 and isnull(Opportunity,0) <> 0
and id not in (SELECT EstimateID FROM  tblEstimateConvertToProject  )

END

----- Created new table for store job-code description  on-behalf  job-type ID
If EXISTS (SELECT  1  FROM  JobType  where ID not in (SELECT JobTypeID FROM  tblJobCodeDesc_ByJobType ) )

BEGIN
INSERT INTO tblJobCodeDesc_ByJobType(JobtypeID,JobCodeID,JobCodeDesc)
SELECT Jobtype.ID JobtypeID,JobCode.ID as JobCodeID, JobCode.Code  as CodeDesc FROM  Jobtype 
left join JobCode on Jobtype.ID=Jobtype.ID
where Jobtype.ID not in (SELECT JobTypeID FROM  tblJobCodeDesc_ByJobType)
END



 -- ===============================================================================
--Created By: API
--Modified On: 11 Mar 2019	
--Description:  For EditLocation issue
-- ===============================================================================

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc'  AND COLUMN_NAME = 'NoCustomerStatement') 

BEGIN  
UPDATE Loc SET NoCustomerStatement = 0 WHERE NoCustomerStatement IS NULL
END
-- ===============================================================================
--Created By: Rustam
--Modified On: 26 April 2019	
--Description:  For Update Default Level Fields TicketO,TicketD,TicketDPDA 
-- ===============================================================================

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO'  AND COLUMN_NAME = 'Level') 

BEGIN  
UPDATE TicketO SET Level = 1 WHERE Level IS NULL
END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'  AND COLUMN_NAME = 'Level') 

BEGIN  
UPDATE TicketD SET Level = 1 WHERE Level IS NULL
END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA'  AND COLUMN_NAME = 'Level') 
BEGIN  
UPDATE TicketDPDA SET Level = 1 WHERE Level IS NULL
END

----ESS-1698  IEC - Mass Review error Generic sp Script - Labels ( Level ) table

IF NOT EXISTS(SELECT 1 FROM [Labels])
INSERT INTO [dbo].[Labels]   ( [Screen] ,[Name] ,[Label]) VALUES ('Level','1','Level')

-- ===============================================================================
-- Created By: Thayer
-- Modified On: Jnly 2, 2019	
-- Description: Add KPI
-- ===============================================================================

SET IDENTITY_INSERT [dbo].[KPI] ON 

IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 1)
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (1, N'120+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 2)
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (2, N'90+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 3)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (3, N'60+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 4)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (4, N'Avg Estimate Conversion Rate', N'Sales', N'Estimate', NULL, N'~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 5)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (5, N'Equipment by Type', N'Customer', N'Equipment', NULL, N'~/NewKPI/Components/EquipmentTypeChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 6)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (6, N'Equipment By Building', N'Customer', N'Equipment', NULL, N'~/NewKPI/Components/EquipmentBuildingChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 7)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (7, N'Converted Estimates By Salesperson Avg. Days', N'Sales', N'Estimate', NULL, N'~/NewKPI/Components/RecurringHoursChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 8)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (8, N'Monthly Recurring Open vs Completed', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/TicketRecurringChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 9)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (9, N'Actual vs Budgeted Revenue', N'Statements', N'Profit and Loss', NULL, N'~/NewKPI/Components/ActualBudgetedRevenueChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 10)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (10, N'Recurring Hours Remaining for Current Month by Route', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/RecurringHoursRemaining.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 11)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (11, N'Monthly Revenue by Department', N'Statements', N'Profit and Loss', NULL, N'~/NewKPI/Components/MonthlyRevenueByDeptChart.ascx')
	
IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 12)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (12, N'Trouble Calls by Equipment', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/TroubleCallsByEquipment.ascx')

IF NOT EXISTS (SELECT 1 FROM [KPI] WHERE [ID] = 13)	
	INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (13, N'Ticket Count By Category And Date Range', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/TicketCountByCategoryAndDateRange.ascx')

SET IDENTITY_INSERT [dbo].[KPI] OFF

-- ===============================================================================
-- Created By: Juily
-- Modified On: June 12, 2020
-- Description: Add Default Values For API Integration
-- ===============================================================================

IF NOT EXISTS (SELECT 1 FROM [Core_APIIntegration])	

BEGIN
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('DashBoard', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Customers', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Recurring', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Schedule', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Billing', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('AP', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Purchasing', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Sales', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Projects', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Inventory', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Financials', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Statements', 0, GETDATE())
INSERT [dbo].[Core_APIIntegration] ([ModuleName], [Integration], [UpdateOn]) VALUES ('Programs', 0, GETDATE())

END

---#########################
if not exists (select 1 from chart where acct='D3100') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D3100', N'Payroll Withholdings & Contributions (DF)', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 1, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3100')
---#########################

---#########################
if not exists (select 1 from chart where acct='D3101') 
BEGIN
INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D3101', N'Inventory Asset', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 1, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3101')
END
---### SET DEFAULT INVENTORY TRACKING ACCT ######################
IF NOT EXISTS(SELECT 1 FROM Custom WHERE Name = 'DefaultInvGLAcct')  
BEGIN  
DECLARE @TId int
SELECT @TId = MAX(ID)+1 FROM Custom
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DefaultInvGLAcct', NULL, NULL, @TId)
END 

IF EXISTS(SELECT * FROM Custom WHERE Name = 'DefaultInvGLAcct' AND ISNULL(Label,'') ='')
BEGIN
print 'Here'
UPDATE Custom SET Label = (SELECT TOP 1 ID FROM Chart WHERE DefaultNo = 'D3101') WHERE Name = 'DefaultInvGLAcct' 
END
---#########################
---#########################
if not exists (select 1 from chart where acct='D3100') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D3100', N'Payroll Withholdings & Contributions (DF)', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 1, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3100')
---#########################

-- ===============================================================================
-- Created By: Azhar
-- Modified On: Oct 06, 2020
-- Description: Add Default Values For Tax Table
-- ===============================================================================

IF NOT EXISTS (SELECT 1 FROM [TaxTable])	

BEGIN
INSERT INTO [dbo].[TaxTable] ([Tax] ,[ERRate] ,[ERCeiling] ,[EERate] ,[EECeiling] ,[Other]) VALUES ('FICA',6.20,113700.00,6.20,113700.00,0.00)
INSERT INTO [dbo].[TaxTable] ([Tax] ,[ERRate] ,[ERCeiling] ,[EERate] ,[EECeiling] ,[Other]) VALUES ('FUTA',0.06,7000.00,0.00,0.00,0.00)
INSERT INTO [dbo].[TaxTable] ([Tax] ,[ERRate] ,[ERCeiling] ,[EERate] ,[EECeiling] ,[Other]) VALUES ('MEDI',1.45,0.00,1.45,0.00,0.00)
END