USE [WEB_Logistics_Panel]
GO
/****** Object:  StoredProcedure [dbo].[SP_BillOfLadingReportByID]    Script Date: 10/15/2020 11:25:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_BillOfLadingReportByID] 
	@ID int
AS
BEGIN
	SELECT TOP  1 [proformaBillID]
      ,[proformaBillDate]
      ,[proformaBillVesselName]
      ,[proformaBillVoyageNo]
      ,[proformaBillDestination]
      ,[proformaBillShipper]
      ,[proformaBillShippersAddress]
      ,[proformaBillShippersTelNo]
      ,[proformaBillServiceType]
      ,[proformaBillConsignee]
      ,[proformaBillConsigneesAddress]
      ,[proformaBillConsigneesTelNo]
      ,[proformaBillNo]
      ,[proformaBillMarks]
      ,[proformaBillQty]
      ,[proformaBillUnit]
      ,[proformaBillDescriptionOfCargo]
      ,[proformaBillValue]
      ,[proformaBillWeight]
      ,[proformaBillMeasurement]
      ,[proformaBillRemarks]
      ,[proformaBillMeasuredBy]
      ,[proformaBillTruckersName]
      ,[proformaBillShippersName]
      ,[proformaBillRefNo]
	FROM [dbo].[ProformaBills] WHERE [proformaBillID] = @ID;
END
