using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;
using AdminLteMvc.Models.WEBSales;
using CrystalDecisions.CrystalReports.Engine;

namespace AdminLteMvc.Controllers
{
    public class EIRController : Controller
    {

        private ContextModel db = new ContextModel();
        // GET: EIRPullOut
        public ActionResult IndexOut()
        {
            return View();
        }
        public ActionResult IndexIn()
        {
            return View();
        }
        public ActionResult Trial()
        {
            return View();
        }
        public ActionResult GetItemsOut(GridParams g, string search)
        {


            var list = db.EirPullOut.Where(o => o.EIRONo.Contains(search) || o.EIROStatus.Contains(search) || o.EIRODate.Contains(search)
            || o.EIROTime.Contains(search) || o.EIROServiceType.Contains(search) || o.EIROConvanNo.Contains(search)
            || o.EIROConvanSize.Contains(search) || o.EIROConvanStatus.Contains(search) || o.EIROTransactionNo.Contains(search)
            || o.EIRORelayPort.Contains(search) || o.EIROShipper.Contains(search) || o.EIROConsignee.Contains(search)
            || o.EIROTrucker.Contains(search) || o.EIROPlateNo.Contains(search) || o.EIRODriversName.Contains(search)
            || o.EIROVessel.Contains(search) || o.EIROVoyageNo.Contains(search) || o.EIROPortOfOrigin.Contains(search)
            || o.EIROPortOfDestination.Contains(search) || o.EIROSealNo.Contains(search) || o.EIROSealStatus.Contains(search)
            || o.EIROWeight.Contains(search) || o.EIROVolume.Contains(search) || o.EIRODamagesCode.Contains(search)
            || o.EIROSCR.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.EirPullOut>(list, g)
            {
                KeyProp = o => o.EIROID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.EIROID,
                    o.EIRONo,
                    o.EIROStatus,
                    o.EIRODate,
                    o.EIROTime,
                    o.EIROServiceType,
                    o.EIROConvanNo,
                    o.EIROConvanSize,
                    o.EIROConvanStatus,
                    o.EIROTransactionNo,
                    o.EIRORelayPort,
                    o.EIROShipper,
                    o.EIROConsignee,
                    o.EIROTrucker,
                    o.EIROPlateNo,
                    o.EIRODriversName,
                    o.EIROVessel,
                    o.EIROVoyageNo,
                    o.EIROPortOfOrigin,
                    o.EIROPortOfDestination,
                    o.EIROSealNo,
                    o.EIROSealStatus,
                    o.EIROWeight,
                    o.EIROVolume,
                    o.EIRODamagesCode,
                    o.EIROSCR
                }
            }.Build());
        }

        public ActionResult GetItemsIn(GridParams g, string search)
        {


            var list = db.EIRIn.Where(o => o.EIRINo.Contains(search) || o.EIRIReferenceNo.Contains(search) || o.EIRIStatus.Contains(search) || o.EIRIDate.Contains(search)
            || o.EIRITime.Contains(search) || o.EIRIServiceType.Contains(search) || o.EIRIConvanNo.Contains(search)
            || o.EIRIConvanSize.Contains(search) || o.EIRIConvanStatus.Contains(search) || o.EIRITransactionNo.Contains(search)
            || o.EIRIRelayPort.Contains(search) || o.EIRIShipper.Contains(search) || o.EIRIConsignee.Contains(search)
            || o.EIRITrucker.Contains(search) || o.EIRIPlateNo.Contains(search) || o.EIRIDriversName.Contains(search)
            || o.EIRIVessel.Contains(search) || o.EIRIVoyageNo.Contains(search) || o.EIRIPortOfOrigin.Contains(search)
            || o.EIRIPortOfDestination.Contains(search) || o.EIRISealNo.Contains(search) || o.EIRISealStatus.Contains(search)
            || o.EIRIWeight.Contains(search) || o.EIRIVolume.Contains(search) || o.EIRIDamagesCode.Contains(search)
            || o.EIRISCR.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.EIRIn>(list, g)
            {
                KeyProp = o => o.EIRIID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.EIRIID,
                    o.EIRINo,
                    o.EIRIReferenceNo,
                    o.EIRIStatus,
                    o.EIRIDate,
                    o.EIRITime,
                    o.EIRIServiceType,
                    o.EIRIConvanNo,
                    o.EIRIConvanSize,
                    o.EIRIConvanStatus,
                    o.EIRITransactionNo,
                    o.EIRIRelayPort,
                    o.EIRIShipper,
                    o.EIRIConsignee,
                    o.EIRITrucker,
                    o.EIRIPlateNo,
                    o.EIRIDriversName,
                    o.EIRIVessel,
                    o.EIRIVoyageNo,
                    o.EIRIPortOfOrigin,
                    o.EIRIPortOfDestination,
                    o.EIRISealNo,
                    o.EIRISealStatus,
                    o.EIRIWeight,
                    o.EIRIVolume,
                    o.EIRIDamagesCode,
                    o.EIRISCR
                }
            }.Build());
        }

        public ActionResult EirPullOut(int ID)
        {
            //var yardUpdate = db.Yard.Find(ID);
            //db.Yard.Attach(yardUpdate);
            //yardUpdate.yardStatus = "Pull-Out";
            //db.Entry(yardUpdate).Property("yardStatus").IsModified = true;
            //db.SaveChanges();

            ViewBag.IDPassed = ID;

            var dateTime = DateTime.Now;
            ViewBag.DateToday = DateTime.Now.ToString("dd/MM/yyyy"); ;
            ViewBag.TimeToday = DateTime.Now.ToString("h:mm:ss tt");

            List<string> ids = new List<string>();
            var getNoOfEIR = db.EirPullOut.AsEnumerable().Select(r => r.EIRONo).ToList();
            var idValue = "";
            if (getNoOfEIR.Count() >= 1)
            {
                List<int> idList = new List<int>();
                foreach (var a in getNoOfEIR)
                {
                    string eirno = a.Substring(9);
                    idList.Add(Int32.Parse(eirno));
                }
                int[] IDS = idList.ToArray();
                var biggestID = IDS.Max() + 1;
                idValue = biggestID.ToString();
            }
            else
            {
                idValue = "10001";
            }

            ViewBag.EIRNO = "OUT-" + dateTime.Year + "-" + idValue;

            var activityInDb = db.Yard.Find(ID);
            ViewBag.ATWBkNo = activityInDb.yardATWBkNo;
            ViewBag.ListofTrns = activityInDb.yardTrnNo;
            ViewBag.ConVanNo = activityInDb.yardConVanNo;
            var getATW = db.ATW.Where(s => s.atwBkNo.Equals(activityInDb.yardATWBkNo)).Single();
            var getBooking = db.Booking.Where(s => s.docNum.Equals(getATW.bkNo)).Single();

            ViewBag.ConVanStatus = getBooking.cstatus;
            ViewBag.ConvanSize = getBooking.csize;
            ViewBag.Shipper = getBooking.cnameshpr;
            ViewBag.Trucker = getATW.aTrucker;
            ViewBag.DriversName = getATW.aDriver;
            ViewBag.PlateNo = getATW.plateNo;

            var trnfromyrd = activityInDb.yardTrnNo.Split(',');
            List<string> stype = new List<string>();
            List<string> cyEPO = new List<string>();
            List<string> consign = new List<string>();
            for (int i = 0; i < trnfromyrd.Length; i++)
            {
                var a = trnfromyrd[i];
                var st = db.TransactionDetails.Where(s => s.docNumber.Equals(getATW.bkNo) && s.transactionNo.Equals(a)).Select(s => s.serviceType).Single();
                var cyepo = db.TransactionDetails.Where(s => s.docNumber.Equals(getATW.bkNo) && s.transactionNo.Equals(a)).Select(s => s.cyEPO).Single();
                var cnsgnee = db.TransactionDetails.Where(s => s.docNumber.Equals(getATW.bkNo) && s.transactionNo.Equals(a)).Select(s => s.consignee).Single();

                stype.Add(st);
                cyEPO.Add(cyepo);
                consign.Add(cnsgnee);
            }

            ViewBag.ServiceType = string.Join(",", stype);
            ViewBag.RelayPort = string.Join(",", cyEPO);
            ViewBag.Consignee = string.Join(",", consign);

            List<Vessel> VesselList = db.Vessel.ToList();
            ViewBag.VesselList = new SelectList(VesselList, "vesselID", "vesselName");

            List<PortOfOrigin> portorgList = db.PortOfOrigin.ToList();
            ViewBag.OriginList = new SelectList(portorgList, "poiID", "originport");

            List<PortOfDestination> portdestList = db.PortOfDestination.ToList();
            ViewBag.DestinationList = new SelectList(portdestList, "podID", "destinationport");

            List<SealStatus> sealstatList = db.SealStatus.ToList();
            ViewBag.SealStatusList = new SelectList(sealstatList, "sealID", "sealstatus");

            var damages = db.DamagesCode.ToList();
            List<SelectListItem> damageList = new List<SelectListItem>();
            foreach (DamagesCode item in damages)
            {
                damageList.Add(new SelectListItem
                {
                    Text = item.damage,
                    Value = item.damage
                });
            }
            ViewBag.DamageList = damageList;

            return View();
        }

        public JsonResult GetVesselNo(int vesselID)
        {
            List<VoyageNo> VoyageNoList = db.VoyageNo.Where(s => s.vesselid.Equals(vesselID.ToString())).ToList();
            return Json(VoyageNoList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(EirPullOut data, int ID)
        {
            bool status = false;

            var yardUpdate = db.Yard.Find(ID);
            db.Yard.Attach(yardUpdate);
            yardUpdate.yardStatus = "Pull-Out";
            db.Entry(yardUpdate).Property("yardStatus").IsModified = true;
            db.SaveChanges();

            var isValidModel = TryUpdateModel(data);
            if (isValidModel)
            {
                db.EirPullOut.Add(data);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult EIRReturn(int ID)
        {
            ViewBag.IDFromOut = ID;


            EirPullOut eirDetails = db.EirPullOut.Find(ID);
            var dateTime = DateTime.Now;
            //ViewBag.DateToday = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.TimeToday = DateTime.Now.ToString("h:mm:ss tt");

            List<string> ids = new List<string>();
            var getNoOfEIR = db.EIRIn.AsEnumerable().Select(r => r.EIRINo).ToList();
            var idValue = "";
            if (getNoOfEIR.Count() >= 1)
            {
                List<int> idList = new List<int>();
                foreach (var a in getNoOfEIR)
                {
                    string eirno = a.Substring(8);
                    idList.Add(Int32.Parse(eirno));
                }
                int[] IDS = idList.ToArray();
                var biggestID = IDS.Max() + 1;
                idValue = biggestID.ToString();
            }
            else
            {
                idValue = "10001";
            }
            var damages = db.DamagesCode.ToList();
            List<SelectListItem> damageList = new List<SelectListItem>();
            foreach (DamagesCode item in damages)
            {
                damageList.Add(new SelectListItem
                {
                    Text = item.damage,
                    Value = item.damage
                });
            }
            ViewBag.DamageList = damageList;
            ViewBag.EIRINo = "IN-" + dateTime.Year + "-" + idValue;
            return View("EIRReturn", eirDetails);
        }

        [HttpPost]
        public JsonResult Return(EIRIn data, int ID)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(data);
            if (isValidModel)
            {
                db.EIRIn.Add(data);
                db.SaveChanges();

                status = true;
            }
            var eirout = db.EirPullOut.Find(ID);
            if (eirout.EIROStatus.Equals("Pull-Out"))
            {
                db.EirPullOut.Attach(eirout);
                eirout.EIROStatus = "Returned";
                db.Entry(eirout).Property("EIROStatus").IsModified = true;
                db.SaveChanges();
            }

            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult ReturnPrint(int ID)
        {
            var eirndetails = db.EIRIn.Find(ID);
            return View("ReturnPrint", eirndetails);
        }

        public ActionResult ViewDetails(int ID)
        {
            var eiroutdtls = db.EirPullOut.Find(ID);
            return View("ViewDetails", eiroutdtls);
        }
        public ActionResult EIROViewDetails(int ID)
        {
            ViewBag.EIROID = ID;
            return View("EIROViewDetails");
        }

        public ActionResult EIRIViewDetails(string eirNo)
        {
            ViewBag.EIRINO = eirNo;
            return View("EIRIViewDetails");
        }

        public FileResult DisplayEIROReport(int EIROID)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath(@"~/Report_Documents/EIROReport.rpt")));
            string query = String.Format("exec pr_eirosp {0}", EIROID);
            var list = db.Database.SqlQuery<Reports_VM.EIROVm>(query).ToList();

            if (list.Count > 0)
            {
                rd.SetDataSource(list);
                rd.SetParameterValue(0, EIROID);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
            }

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf");
        }

        public FileResult DisplayEIRIReturnReport(string EIRINo)
        {
            var path = Server.MapPath(@"~/Report_Documents/EIRIReturnInReport.rpt");
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath(@"~/Report_Documents/EIRIReturnInReport.rpt")));
            string query = String.Format("exec SP_ForReturnPrintReport '{0}'", EIRINo);
            var list = db.Database.SqlQuery<Reports_VM.EIRIVm>(query).ToList();

        //    if (list.Count > 0)
        //    {
        //        rd.SetDataSource(list);
        //        rd.SetParameterValue(0, EIRINo);

        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //    }

        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);

        //    return File(stream, "application/pdf");
        //}
    }
}