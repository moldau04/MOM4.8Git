/*
     $$$$$ Deployment Process for Data migrate from TS to MOM5.0 $$$

 ========> Step 1 Run MOM 5.0 Generic Script For Create New MSM2_Admin Database.sql (If MSM2_Admin Database not exists in server)
                   Script Path -: ~ Database\MOMdb\Scripts\MOM5_Generic_Deployment_Script\MOM 5.0 Generic Script For Create New MSM2_Admin Database.sql

				 Note :---  (before run table script set Ref column datatype bigint in trans and CD table and fdesc chnaged varchar(MAX))
 ========> Step 2 Run MOM 5.0 Generic deployment script for Table.
                  Script  Path -: ~ Database\MOMdb\Scripts\MOM5_Generic_Deployment_Script\MOM 5.0 Generic deployment script for Table.sql

 ========> Step 3 Run MOM 5.0 Generic deployment script for StoredProcedure.
                 Script  Path -: ~ Database\MOMdb\Scripts\MOM5_Generic_Deployment_Script\MOM 5.0 Generic deployment script for StoredProcedure.sql

========> Step 4 Run MOM 5.0 Generic Script for Data migrate from TS to MOM.
                 Script  Path -: ~ Database\MOMdb\Scripts\MOM5_Generic_Deployment_Script\TS_To_MOM\MOM 5.0 Generic Script for Data migrate from TS to MOM.sql 

========> Step 5 Run Primary Index Script
                 Script Path -: ~ Database\MOMdb\Scripts\MOM 5.0 Generic Script for Primary And Index

========> Step 6 Run Trigger script
               Script Path -: ~ Database\MOMdb\Scripts\ MOM 5.0 Generic Script for Triggers

========> Step 7 Please compare TS DB Schema with MOM db Schema and run the generted script.

========> Step 8 Run  AP Bill Script to Migrate Old-line item to New Table APBillItem.sql.
                 Script  Path -: ~ Database\MOMdb\Scripts\MOM 5.0 Generic Script for Data migrate from TS to MOM.sql
 
   
*/