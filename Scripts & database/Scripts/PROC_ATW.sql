USE [WEB_Logistics_Panel_New]
GO

/****** Object:  StoredProcedure [dbo].[PROC_ATW]    Script Date: 15/10/2020 8:15:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--exec [PROC_ATW] '2020-10008','003'
CREATE PROC [dbo].[PROC_ATW] @bookingNo nvarchar(50), @transNo nvarchar(50) as 
SELECT distinct
T0.aTrucker [AuthorizedTrucker]
,T0.aDriver [AuthorizedDriver]
,T0.cShipper [CompanyShipper]
,T0.conPerson [ContactPerson]
,T0.atwBkNo [ATWBookingNo]
,T0.plateNo [TruckPlateNo]
,T0.bkNo [Booking]
,T0.iDate [IssueDate]
,T0.eDate [ExpiryDate]
,T1.consignee [Consignee]
,T1.cargoType [Cargo]
,T1.serviceType [ServiceMode]
,T1.cyEPO [CYPullOut]
,T1.cySA [CYStuffing]
,T1.cyLD [CYDelivery]
,T1.conRqrmnts [ConVanRequirements]
,T1.transactionNo [TransactionNo]
,T1.origin [Origin]
,T1.destination [Destination]
,T1.remarks [Remarks]
,T2.csize [ConVanSize]
,T2.cstatus [ConVanStatus]
FROM ATWs T0
INNER JOIN TransactionDetails T1 on T0.bkNo = T1.docNumber
INNER JOIN Bookings T2 on T1.docNumber = T2.docNum
WHERE t0.bkNo = @bookingNo and t1.transactionNo = @transNo
GO

