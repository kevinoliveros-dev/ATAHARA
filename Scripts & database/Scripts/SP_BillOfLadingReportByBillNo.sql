USE [WEB_Logistics_Panel]
GO
/****** Object:  StoredProcedure [dbo].[SP_BillOfLadingReportByRefNo]    Script Date: 10/16/2020 3:28:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[SP_BillOfLadingReportByBillNo] 
	@billNo nvarchar(50)
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
	FROM [dbo].[ProformaBills] WHERE [proformaBillNo] = @billNo;
END
