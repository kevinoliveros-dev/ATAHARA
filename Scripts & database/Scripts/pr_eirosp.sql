USE [WEB_Logistics_Panel]
GO
/****** Object:  StoredProcedure [dbo].[pr_eirosp]    Script Date: 10/10/2020 10:06:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[pr_eirosp] @eirono nvarchar(50)  as 
select 
T0.EIRONo,
T0.EIRODate,
T0.EIROVessel,
T0.EIROConvanNo,
T0.EIROShipper,
T0.EIROTime,
T0.EIROVoyageNo,
T0.EIROSealNo,
T0.EIROConsignee,
T0.EIROServiceType,
T0.EIROPortOfOrigin,
T0.EIROSealStatus,
T0.EIROTrucker,
T0.EIROConvanStatus,
T0.EIRORelayPort,
T0.EIROConvanSize,
T0.EIROPlateNo,
T0.EIROTransactionNo,
T0.EIROPortOfDestination,
T0.EIROWeight,
T0.EIROVolume,
T0.EIRODriversName,
T0.EIRODamagesCode,
T0.EIROSCR
From 
EirPullOuts T0 
where T0.EIRONo = @eirono;

