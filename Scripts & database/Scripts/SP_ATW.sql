USE [WEB_Logistics_Panel]
GO
/****** Object:  StoredProcedure [dbo].[SP_ATW]    Script Date: 10/20/2020 8:51:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [PROC_ATW] '2020-10008','003'
create PROC [dbo].[SP_ATW] @bookingNo nvarchar(50), @transNo nvarchar(50) as 
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
,(SELECT a.cyLocation FROM CYLocations a where a.cyCode =  T1.cyEPO)as  [CYPullOut]
,(SELECT a.cyLocation FROM CYLocations a where a.cyCode =  T1.cySA)as  [CYStuffing]
,(SELECT a.cyLocation FROM CYLocations a where a.cyCode =  T1.cyLD)as [CYDelivery]
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
