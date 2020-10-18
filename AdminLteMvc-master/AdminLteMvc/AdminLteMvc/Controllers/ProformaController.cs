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
using Omu.AwesomeMvc;

namespace AdminLteMvc.Controllers
{
    public class ProformaController : Controller
    {
        private ContextModel db = new ContextModel();
        // GET: Proforma
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Billing()
        {
            List<EirPullOut> EirOut = db.EirPullOut.Where(s=>s.EIROStatus != "Billed").ToList();
            ViewBag.ReferenceNos = new SelectList(EirOut, "EIRONo", "EIRONo");

            var dateTime = DateTime.Now;

            List<string> ids = new List<string>();
            var getNoOfBills = db.ProformaBills.AsEnumerable().Select(r => r.proformaBillNo).ToList();
            var idValue = "";
            if (getNoOfBills.Count() >= 1)
            {
                List<int> idList = new List<int>();
                foreach (var a in getNoOfBills)
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

            ViewBag.ProformaNo = "PBL-" + dateTime.Year + "-" + idValue;
            return View();
        }

        public ActionResult GetBills(GridParams g, string search)
        {


            var list = db.ProformaBills.Where(o => o.proformaBillNo.Contains(search) || o.proformaBillDate.Contains(search) || o.proformaBillVesselName.Contains(search)
            || o.proformaBillVoyageNo.Contains(search) || o.proformaBillDestination.Contains(search) || o.proformaBillShipper.Contains(search)
            || o.proformaBillShippersAddress.Contains(search) || o.proformaBillShippersTelNo.Contains(search) || o.proformaBillServiceType.Contains(search)
            || o.proformaBillConsignee.Contains(search) || o.proformaBillConsigneesAddress.Contains(search) || o.proformaBillConsigneesTelNo.Contains(search)
            || o.proformaBillMarks.Contains(search) || o.proformaBillQty.Contains(search) || o.proformaBillUnit.Contains(search)
            || o.proformaBillDescriptionOfCargo.Contains(search) || o.proformaBillValue.Contains(search) || o.proformaBillWeight.Contains(search)
            || o.proformaBillMeasurement.Contains(search) || o.proformaBillRemarks.Contains(search) || o.proformaBillMeasuredBy.Contains(search)
            || o.proformaBillTruckersName.Contains(search) || o.proformaBillShippersName.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.ProformaBills>(list, g)
            {
                KeyProp = o => o.proformaBillID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.proformaBillID,
                    o.proformaBillNo,
                    o.proformaBillDate,
                    o.proformaBillVesselName,
                    o.proformaBillVoyageNo,
                    o.proformaBillDestination,
                    o.proformaBillShipper,
                    o.proformaBillShippersAddress,
                    o.proformaBillShippersTelNo,
                    o.proformaBillServiceType,
                    o.proformaBillConsignee,
                    o.proformaBillConsigneesAddress,
                    o.proformaBillConsigneesTelNo,
                    o.proformaBillMarks,
                    o.proformaBillQty,
                    o.proformaBillUnit,
                    o.proformaBillDescriptionOfCargo,
                    o.proformaBillValue,
                    o.proformaBillWeight,
                    o.proformaBillMeasurement,
                    o.proformaBillRemarks,
                    o.proformaBillMeasuredBy,
                    o.proformaBillTruckersName,
                    o.proformaBillShippersName
                }
            }.Build());
        }

        [HttpGet]
        public ActionResult GetDetails(string refno)
        {
            var getEIR = db.EirPullOut.Where(s => s.EIRONo.Equals(refno)).Single();
            var getbkno = db.ATW.Where(s=>s.atwBkNo.Equals(getEIR.EIROAtwBkNo)).Select(s=>s.bkNo).Single();
            var bkdtls = db.Booking.Where(s=>s.docNum.Equals(getbkno)).Single();
            var trndetails = db.TransactionDetails.Where(s=>s.docNumber.Equals(getbkno)).First();
            var ctel = db.TransactionDetails.Where(s => s.docNumber.Equals(getbkno)).Select(s => s.consigneetelno).First();

            var VesselName = getEIR.EIROVessel;
            var VoyageNo = getEIR.EIROVoyageNo;
            var Destination = getEIR.EIROPortOfDestination;
            var Shipper = getEIR.EIROShipper;
            var ShippersAddress = bkdtls.shipperAddress;
            var ShippersTelNo = bkdtls.shipperTelNo;
            var ServiceType = getEIR.EIROServiceType;
            var Consignee = getEIR.EIROConsignee;
            var ConsigneeAddress = trndetails.consigneeAdd;
            var ConsigneeTelNo = trndetails.consigneetelno;
            var Quantity = trndetails.quantity;
            var Unit = trndetails.unitofmeasurement;
            var Price = trndetails.price;
            var Trucker = getEIR.EIROTrucker;

            var result = new { 
                ven = VesselName, 
                von = VoyageNo, 
                des = Destination, 
                shpr = Shipper, 
                shpra = ShippersAddress, 
                shprtel = ShippersTelNo,
                st = ServiceType,
                con = Consignee,
                cona = ConsigneeAddress,
                cont = ConsigneeTelNo,
                trucker = Trucker,
                qty = Quantity,
                unit = Unit,
                price = Price
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBill(ProformaBills bill)
        {
            bool status = false;

            var outeir = db.EirPullOut.Where(s=>s.EIRONo.Equals(bill.proformaBillRefNo)).Single();

            var eirpull = db.EirPullOut.Find(outeir.EIROID);
            db.EirPullOut.Attach(eirpull);
            eirpull.EIROStatus = "Billed";
            db.Entry(eirpull).Property("EIROStatus").IsModified = true;
            db.SaveChanges();

            var isValidModel = TryUpdateModel(bill);
            if (isValidModel)
            {
                db.ProformaBills.Add(bill);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult ViewBill(int ID)
        {
            var billinfo = db.ProformaBills.Find(ID);
            return View("ViewBill", billinfo);
        }
        public ActionResult ViewBillDetails(string billNo)
        {
            ViewBag.BillNo = billNo;
            return View("ViewBillDetails");
        }
        public FileResult DisplayProformaBOLReport(string billNo)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath(@"~/Report_Documents/ProformaBillOfLading.rpt")));
            string query = String.Format("exec SP_BillOfLadingReportByBillNo '{0}'", billNo.Trim());
            var list = db.Database.SqlQuery<Reports_VM.ProformaVm>(query).ToList();

            if (list.Count > 0)
            {
                rd.SetDataSource(list);
                rd.SetParameterValue(0, billNo);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
            }

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf");
        }
    }
}