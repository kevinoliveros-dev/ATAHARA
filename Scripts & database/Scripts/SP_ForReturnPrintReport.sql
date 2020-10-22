USE [WEB_Logistics_Panel]
GO
/****** Object:  StoredProcedure [dbo].[SP_ForReturnPrintReport]    Script Date: 10/10/2020 5:29:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ForReturnPrintReport] 
	@EIRNo nvarchar(50)
AS
BEGIN
	SELECT Top 1
	  [EIRIID]
      ,[EIRINo]
      ,[EIRIReferenceNo]
      ,[EIRIDate]
      ,[EIRITime]
      ,[EIRIServiceType]
      ,[EIRIConvanStatus]
      ,[EIRITransactionNo]
      ,[EIRIVessel]
      ,[EIRIVoyageNo]
      ,[EIRIPortOfOrigin]
      ,[EIRIRelayPort]
      ,[EIRIPortOfDestination]
      ,[EIRIConvanNo]
      ,[EIRISealNo]
      ,[EIRISealStatus]
      ,[EIRIConvanSize]
      ,[EIRIWeight]
      ,[EIRIVolume]
      ,[EIRIShipper]
      ,[EIRIConsignee]
      ,[EIRITrucker]
      ,[EIRIPlateNo]
      ,[EIRIDriversName]
      ,REPLACE(REPLACE([EIRIDamagesCode], CHAR(13), ''), CHAR(10), '')  as [EIRIDamagesCode]
      ,[EIRISCR]
      ,[EIRIStatus]
      ,[EIRIRemarks]
  FROM [EIRIns] Where [EIRINo] = @EIRNo;
END
