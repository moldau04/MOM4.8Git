/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 07 Dec Nov 2018	
Description: Remove and re-create table defined type
--------------------------------------------------------------------*/

--Drop dependency sp of tblTypeBomItem1
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProject]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProject]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProjectTemplate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

--END

--Drop  tblTypeBomItem1 table type
--BEGIN
IF type_id('[dbo].[tblTypeBomItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeBomItem]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeEstimateBomItem1
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateEstimate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddEstimate]
END

--END

--Drop  tblTypeEstimateBomItem1 table type
--BEGIN
IF type_id('[dbo].[tblTypeEstimateBomItem1]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeEstimateBomItem1]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeEstimateMilestoneItem1
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddEstimate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateEstimate]
END

--END

--Drop  tblTypeEstimateMilestoneItem1 table type
--BEGIN
IF type_id('[dbo].[tblTypeEstimateMilestoneItem1]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeEstimateMilestoneItem1]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeMilestoneItem1
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProject]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProject]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProjectTemplate]
END

--END

--Drop  tblTypeMilestoneItem1 table type
--BEGIN
IF type_id('[dbo].[tblTypeMilestoneItem1]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeMilestoneItem1]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeProjectItem1
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

--END

--Drop  tblTypeProjectItem1 table type
--BEGIN
IF type_id('[dbo].[tblTypeProjectItem1]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeProjectItem1]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------


--Drop dependency sp of tblTypeBomItem
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProject]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProject]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProjectTemplate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddContract]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddContract]
END

--END

--Drop  tblTypeBomItem table type
--BEGIN
IF type_id('[dbo].[tblTypeBomItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeBomItem]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeEstimateBomItem
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateEstimate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddEstimate]
END

--END

--Drop  tblTypeEstimateBomItem table type
--BEGIN
IF type_id('[dbo].[tblTypeEstimateBomItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeEstimateBomItem]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeEstimateMilestoneItem
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddEstimate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateEstimate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateEstimate]
END

--END

--Drop  tblTypeEstimateMilestoneItem table type
--BEGIN
IF type_id('[dbo].[tblTypeEstimateMilestoneItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeEstimateMilestoneItem]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeMilestoneItem
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProject]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProject]
END

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spAddProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spAddProjectTemplate]
END


--END

--Drop  tblTypeMilestoneItem table type
--BEGIN
IF type_id('[dbo].[tblTypeMilestoneItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeMilestoneItem]
END
--END

------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

--Drop dependency sp of tblTypeProjectItem
--BEGIN
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[spUpdateProjectTemplate]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[spUpdateProjectTemplate]
END

--END

--Drop  tblTypeProjectItem table type
--BEGIN
IF type_id('[dbo].[tblTypeProjectItem]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblTypeProjectItem]
END
--END

--Drop  tblBomItemType1 table type
--BEGIN
IF type_id('[dbo].[tblBomItemType1]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblBomItemType1]
END
--END

--Drop  tblBomItemType table type
--BEGIN
IF type_id('[dbo].[tblBomItemType]') IS NOT NULL
BEGIN
	DROP TYPE [dbo].[tblBomItemType]
END
--END


------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------

