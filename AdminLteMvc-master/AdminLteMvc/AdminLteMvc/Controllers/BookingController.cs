﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;
using AdminLteMvc.Models.WEBSales;
using Omu.AwesomeMvc;

namespace AdminLteMvc.Controllers
{
    public class BookingController : Controller
    { 
        private ContextModel db = new ContextModel();
        // GET: Booking
        public ActionResult Index(Booking bk)
        {
            return View(bk);
        }

        public ActionResult GetItems(GridParams g, string search)
        {
            var list = db.Booking.Where(o => o.trnStatus.Contains(search) || o.docDate.Contains(search) || o.docNum.Contains(search) || o.cnameshpr.Contains(search) || o.csize.Contains(search) || o.cstatus.Contains(search) || o.mnemonic.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.Booking>(list, g)
            {
                KeyProp = o => o.ID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.ID,
                    o.preparedBy,
                    o.trnStatus,
                    o.docDate,
                    o.docNum,
                    o.billTo,
                    o.mnemonic,
                    o.cnameshpr,
                    o.shipperAddress,
                    o.shipperTelNo,
                    o.csize,
                    o.cstatus
                }
            }.Build());
        }

        public ActionResult Create()
        {
            var checkYear = db.Booking.AsEnumerable().Select(r => r.docID).FirstOrDefault();

            var mnemonic = db.Mnemonics.ToList();
            var custshpr = db.CustomerShippers.ToList();
            var convanSizes = db.ConVanSizes.ToList();
            var convanStatus = db.ConVanStatus.ToList();
            List<SelectListItem> mnemonicList = new List<SelectListItem>();
            List<SelectListItem> customerShipperList = new List<SelectListItem>();
            List<SelectListItem> ConVanSizeList = new List<SelectListItem>();
            List<SelectListItem> ConVanStatList = new List<SelectListItem>();
            foreach (Mnemonics item in mnemonic)
            {
                mnemonicList.Add(new SelectListItem
                {
                    Text = item.mnemonic,
                    Value = item.mnemonic
                });
            }

            foreach (CustomerShippers item in custshpr)
            {
                customerShipperList.Add(new SelectListItem
                {
                    Text = item.custShipr,
                    Value = item.custShipr
                });
            }

            foreach (ConVanSizes item in convanSizes)
            {
                ConVanSizeList.Add(new SelectListItem
                {
                    Text = item.sizes,
                    Value = item.sizes
                });
            }

            foreach (ConVanStatus item in convanStatus)
            {
                ConVanStatList.Add(new SelectListItem
                {
                    Text = item.status,
                    Value = item.status
                });
            }


            ViewBag.MnemonicList = mnemonicList;
            ViewBag.CustomerShipperList = customerShipperList;
            ViewBag.ConVanSizesList = ConVanSizeList;
            ViewBag.ConVanStatusList = ConVanStatList;
            return View();
        }

        [HttpGet]
        public ActionResult GetIDSFromYear(string yearInput)
        {
            List<string> ids = new List<string>();
            var checkYear = db.Booking.AsEnumerable().Select(r => r.docYear).ToList();
            var idValue = "";
            if (checkYear.Contains(yearInput))
            {
                ids = db.Booking.Where(s => s.docYear.Equals(yearInput)).Select(r => r.docID).ToList();
                List<int> idList = new List<int>();
                for (int runs = 0; runs < ids.Count(); runs++)
                {
                    idList.Add(Int32.Parse(ids[runs]));
                }
                int[] IDS = idList.ToArray();
                var biggestID = IDS.Max() + 1;
                idValue = biggestID.ToString();
            }
            else
            {
                idValue = "10001";
            }
            ViewBag.yearInputted = yearInput;
            ViewBag.idGenerated = idValue;
            return Json(idValue, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BindMnemonic(string mnemonicValue)
        {
            var getMnemonicID = db.Mnemonics.Where(s => s.mnemonic.Equals(mnemonicValue)).Select(r => r.ID).SingleOrDefault();
            var Custshipper = db.CustomerShippers.Where(s => s.mnemID.Equals(getMnemonicID)).Select(r => r.custShipr).SingleOrDefault();
            //return Json(new Modle.JsonResponseData { Status = flag, Message = msg, Html = html }, JsonRequestBehavior.AllowGet);
            //return Json(custshipper);
            return Json(Custshipper, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult CreateDetail()
        {
            ViewBag.Branch = new SelectList(db.Projects, "ProjectCode", "ProjectName");
            return View("CreateDetail");
        }

        public ActionResult TrnHeader( int id)
        {
            Booking bk = db.Booking.Find(id);
            return View("TrnHeader", bk);
        }

        public ActionResult EditHeader(int id)
        {
            Booking bk = db.Booking.Find(id);
            var mnemonic = db.Mnemonics.ToList();
            var custshpr = db.CustomerShippers.ToList();
            var convanSizes = db.ConVanSizes.ToList();
            var convanStatus = db.ConVanStatus.ToList();
            List<SelectListItem> mnemonicList = new List<SelectListItem>();
            List<SelectListItem> customerShipperList = new List<SelectListItem>();
            List<SelectListItem> ConVanSizeList = new List<SelectListItem>();
            List<SelectListItem> ConVanStatList = new List<SelectListItem>();
            foreach (Mnemonics item in mnemonic)
            {
                mnemonicList.Add(new SelectListItem
                {
                    Text = item.mnemonic,
                    Value = item.mnemonic
                });
            }

            foreach (CustomerShippers item in custshpr)
            {
                customerShipperList.Add(new SelectListItem
                {
                    Text = item.custShipr,
                    Value = item.custShipr
                });
            }

            foreach (ConVanSizes item in convanSizes)
            {
                ConVanSizeList.Add(new SelectListItem
                {
                    Text = item.sizes,
                    Value = item.sizes
                });
            }

            foreach (ConVanStatus item in convanStatus)
            {
                ConVanStatList.Add(new SelectListItem
                {
                    Text = item.status,
                    Value = item.status
                });
            }


            ViewBag.MnemonicList = mnemonicList;
            ViewBag.CustomerShipperList = customerShipperList;
            ViewBag.ConVanSizesList = ConVanSizeList;
            ViewBag.ConVanStatusList = ConVanStatList;
            return View("EditHeader", bk);
        }

        [HttpPost]
        public JsonResult UpdateHdr(Booking bk)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(bk);
            if (isValidModel)
            {
                using (var db = new ContextModel())
                {
                    db.Entry(bk).State = EntityState.Modified;
                    db.SaveChanges();
                }
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult Cancel(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking bkdtls = db.Booking.Find(id);
            if (bkdtls == null)
            {
                return HttpNotFound();
            }
            return View(bkdtls);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var bk = new Booking() { ID = id};
            using (var db = new ContextModel())
            {
                db.Booking.Attach(bk);
                bk.trnStatus = "Cancelled";
                db.Entry(bk).Property(x => x.trnStatus).IsModified = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking bk)
        {
            if (ModelState.IsValid)
            {
                db.Booking.Add(bk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bk);
        }

        [HttpPost]
        public JsonResult Save(Booking data)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(data);
            if (isValidModel)
            {
                db.Booking.Add(data);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status, transacID = data.ID } };
        }

        public ActionResult TrnDetails(int ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking idDtls = db.Booking.Find(ID);
            string docuID = idDtls.docID.ToString();
            var bkdetails = db.Booking.Where(t => t.transactionNo.Contains(docuID)).ToList();
            if (bkdetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.Branch = new SelectList(db.Projects, "ProjectCode", "ProjectName");
            return View("TrnDetails", idDtls);
        }

        public ActionResult GetTrnDetails(GridParams g, string search, string trnID)
        {


            var list = db.TransactionDetails.Where(o => o.documentID.Contains(trnID)).Where(x => x.dtlStatus == "Active" && (x.cargoType.Contains(search) || x.conClass.Contains(search) || x.consignee.Contains(search) || x.consigneeAdd.Contains(search) || x.cyEPO.Contains(search) || x.cyLD.Contains(search) || x.cySA.Contains(search) || x.destination.Contains(search) || x.origin.Contains(search) || x.payMode.Contains(search) || x.priceList.Contains(search))).OrderBy(s => s.transactionNo);
            return Json(new GridModelBuilder<Models.WEBSales.TransactionDetails>(list, g)
            {
                KeyProp = o => o.tranID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.tranID,
                    o.transactionDate,
                    o.transactionNo,
                    o.dtlStatus,
                    o.priceList,
                    o.origin,
                    o.destination,
                    o.consignee,
                    o.consigneeAdd,
                    o.cargoType,
                    o.conClass,
                    o.conRqrmnts,
                    o.quantity,
                    o.unitofmeasurement,
                    o.payMode,
                    o.cyEPO,
                    o.cySA,
                    o.cyLD,
                    o.remarks
                }
            }.Build());
        }

        public ActionResult AddDetail(int ID)
        {
            Booking newDetail = db.Booking.Find(ID);
            var keyID = newDetail.ID;
            ViewBag.ID = keyID;
            var documID = newDetail.docID;
            ViewBag.docuID = documID;
            ViewBag.docNumber = newDetail.docNum;


            var plist = db.PriceGroup.ToList();
            var org = db.Origin.ToList();
            var dest = db.Destination.ToList();
            var dbCon = db.Consignee.ToList();
            var dbCharge = db.ChargeTo.ToList();
            var dbServ = db.ServiceType.ToList();
            var dbCargo = db.CargoType.ToList();
            var dbContainer = db.ContainerClass.ToList();
            var dbConRequirements = db.ConVanRequirements.ToList();
            var dbEPO = db.CYEPO.ToList();
            var dbSA = db.CYSA.ToList();
            var dbLD = db.CYLD.ToList();
            List<SelectListItem> prList = new List<SelectListItem>();
            List<SelectListItem> orgList = new List<SelectListItem>();
            List<SelectListItem> destList = new List<SelectListItem>();
            List<SelectListItem> dbConList = new List<SelectListItem>();
            List<SelectListItem> dbChargeList = new List<SelectListItem>();
            List<SelectListItem> dbServList = new List<SelectListItem>();
            List<SelectListItem> dbCargoList = new List<SelectListItem>();
            List<SelectListItem> dbContainerList = new List<SelectListItem>();
            List<SelectListItem> dbConRequirementsList = new List<SelectListItem>();
            List<SelectListItem> dbEPOList = new List<SelectListItem>();
            List<SelectListItem> dbSAList = new List<SelectListItem>();
            List<SelectListItem> dbLDList = new List<SelectListItem>();
            foreach (PriceGroup item in plist)
            {
                prList.Add(new SelectListItem
                {
                    Text = item.priceGroup,
                    Value = item.priceGroup
                });
            }
            foreach (Origin item in org)
            {
                orgList.Add(new SelectListItem
                {
                    Text = item.origin,
                    Value = item.origin
                });
            }
            foreach (Destination item in dest)
            {
                destList.Add(new SelectListItem
                {
                    Text = item.destination,
                    Value = item.destination
                });
            }
            foreach (Consignee item in dbCon)
            {
                dbConList.Add(new SelectListItem
                {
                    Text = item.consignee,
                    Value = item.consignee
                });
            }
            foreach (ChargeTo item in dbCharge)
            {
                dbChargeList.Add(new SelectListItem
                {
                    Text = item.chargeTo,
                    Value = item.chargeTo
                });
            }
            foreach (ServiceType item in dbServ)
            {
                dbServList.Add(new SelectListItem
                {
                    Text = item.sType,
                    Value = item.sType
                });
            }
            foreach (CargoType item in dbCargo)
            {
                dbCargoList.Add(new SelectListItem
                {
                    Text = item.cargotype,
                    Value = item.cargotype
                });
            }
            foreach (ContainerClass item in dbContainer)
            {
                dbContainerList.Add(new SelectListItem
                {
                    Text = item.conclass,
                    Value = item.conclass
                });
            }
            foreach (ConVanRequirements item in dbConRequirements)
            {
                dbConRequirementsList.Add(new SelectListItem
                {
                    Text = item.requirements,
                    Value = item.requirements
                });
            }
            foreach (CYEPO item in dbEPO)
            {
                dbEPOList.Add(new SelectListItem
                {
                    Text = item.cyEpo,
                    Value = item.cyEpo
                });
            }
            foreach (CYSA item in dbSA)
            {
                dbSAList.Add(new SelectListItem
                {
                    Text = item.cySa,
                    Value = item.cySa
                });
            }
            foreach (CYLD item in dbLD)
            {
                dbLDList.Add(new SelectListItem
                {
                    Text = item.cyLd,
                    Value = item.cyLd
                });
            }

            // var getdocuID = db.Booking.Where(a => a.ID.Equals(keyID)).Select(r => r.docID).SingleOrDefault();
            var getAllTrnNoFromDocu = db.TransactionDetails.Where(s => s.documentID.Equals(documID)).Select(r => r.transactionNo).ToList();
            var trnValue = "";

            if (getAllTrnNoFromDocu.Count() == 0)
            {
                trnValue = "001";
            }
            else
            {
                List<int> trnList = new List<int>();
                for (int runs = 0; runs < getAllTrnNoFromDocu.Count(); runs++)
                {
                    trnList.Add(Int32.Parse(getAllTrnNoFromDocu[runs]));
                }
                int[] IDS = trnList.ToArray();
                var biggestID = IDS.Max() + 1;
                var tempVal = biggestID.ToString();
                trnValue = "00" + tempVal;
            }

            ViewBag.TrnValue = trnValue;
            ViewBag.PriceList = prList;
            ViewBag.OriginList = orgList;
            ViewBag.DestinationList = destList;
            ViewBag.ConsigneeList = dbConList;
            ViewBag.ChargeToList = dbChargeList;
            ViewBag.ServiceTypeList = dbServList;
            ViewBag.CargoTypeList = dbCargoList;
            ViewBag.ContainerClassList = dbContainerList;
            ViewBag.ConVanRequirementList = dbConRequirementsList;
            ViewBag.EPOList = dbEPOList;
            ViewBag.SAList = dbSAList;
            ViewBag.LDList = dbLDList;
            return View();
        }

        [HttpPost]
        public JsonResult SaveNewDetail(TransactionDetails trnD)
        {
            bool status = false;
            var isValidModel = TryUpdateModel(trnD);
            if (isValidModel)
            {
                db.TransactionDetails.Add(trnD);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult CancelDetail(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionDetails trndtls = db.TransactionDetails.Find(id);
            if (trndtls == null)
            {
                return HttpNotFound();
            }
            return View(trndtls);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("CancelDetail")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelDtlConfirmed(int id)
        {

            TransactionDetails detail = db.TransactionDetails.Find(id);
            var IDtran = detail.transactionID;
            var dtl = new TransactionDetails() { tranID = id };
            using (var db = new ContextModel())
            {
                db.TransactionDetails.Attach(dtl);
                dtl.dtlStatus = "Cancelled";
                db.Entry(dtl).Property(x => x.dtlStatus).IsModified = true;
                db.SaveChanges();
            }
            return RedirectToAction("TrnDetails", new { ID = IDtran });
        }

        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionDetails edtDtl = db.TransactionDetails.Find(id);
            if (edtDtl == null)
            {
                return HttpNotFound();
            }

            var plist = db.PriceGroup.ToList();
            var org = db.Origin.ToList();
            var dest = db.Destination.ToList();
            var dbCon = db.Consignee.ToList();
            var dbCharge = db.ChargeTo.ToList();
            var dbServ = db.ServiceType.ToList();
            var dbCargo = db.CargoType.ToList();
            var dbContainer = db.ContainerClass.ToList();
            var dbConRequirements = db.ConVanRequirements.ToList();
            var dbEPO = db.CYEPO.ToList();
            var dbSA = db.CYSA.ToList();
            var dbLD = db.CYLD.ToList();
            List<SelectListItem> prList = new List<SelectListItem>();
            List<SelectListItem> orgList = new List<SelectListItem>();
            List<SelectListItem> destList = new List<SelectListItem>();
            List<SelectListItem> dbConList = new List<SelectListItem>();
            List<SelectListItem> dbChargeList = new List<SelectListItem>();
            List<SelectListItem> dbServList = new List<SelectListItem>();
            List<SelectListItem> dbCargoList = new List<SelectListItem>();
            List<SelectListItem> dbContainerList = new List<SelectListItem>();
            List<SelectListItem> dbConRequirementsList = new List<SelectListItem>();
            List<SelectListItem> dbEPOList = new List<SelectListItem>();
            List<SelectListItem> dbSAList = new List<SelectListItem>();
            List<SelectListItem> dbLDList = new List<SelectListItem>();
            foreach (PriceGroup item in plist)
            {
                prList.Add(new SelectListItem
                {
                    Text = item.priceGroup,
                    Value = item.priceGroup
                });
            }
            foreach (Origin item in org)
            {
                orgList.Add(new SelectListItem
                {
                    Text = item.origin,
                    Value = item.origin
                });
            }
            foreach (Destination item in dest)
            {
                destList.Add(new SelectListItem
                {
                    Text = item.destination,
                    Value = item.destination
                });
            }
            foreach (Consignee item in dbCon)
            {
                dbConList.Add(new SelectListItem
                {
                    Text = item.consignee,
                    Value = item.consignee
                });
            }
            foreach (ChargeTo item in dbCharge)
            {
                dbChargeList.Add(new SelectListItem
                {
                    Text = item.chargeTo,
                    Value = item.chargeTo
                });
            }
            foreach (ServiceType item in dbServ)
            {
                dbServList.Add(new SelectListItem
                {
                    Text = item.sType,
                    Value = item.sType
                });
            }
            foreach (CargoType item in dbCargo)
            {
                dbCargoList.Add(new SelectListItem
                {
                    Text = item.cargotype,
                    Value = item.cargotype
                });
            }
            foreach (ContainerClass item in dbContainer)
            {
                dbContainerList.Add(new SelectListItem
                {
                    Text = item.conclass,
                    Value = item.conclass
                });
            }
            foreach (ConVanRequirements item in dbConRequirements)
            {
                dbConRequirementsList.Add(new SelectListItem
                {
                    Text = item.requirements,
                    Value = item.requirements
                });
            }
            foreach (CYEPO item in dbEPO)
            {
                dbEPOList.Add(new SelectListItem
                {
                    Text = item.cyEpo,
                    Value = item.cyEpo
                });
            }
            foreach (CYSA item in dbSA)
            {
                dbSAList.Add(new SelectListItem
                {
                    Text = item.cySa,
                    Value = item.cySa
                });
            }
            foreach (CYLD item in dbLD)
            {
                dbLDList.Add(new SelectListItem
                {
                    Text = item.cyLd,
                    Value = item.cyLd
                });
            }
            ViewBag.PriceList = prList;
            ViewBag.OriginList = orgList;
            ViewBag.DestinationList = destList;
            ViewBag.ConsigneeList = dbConList;
            ViewBag.ChargeToList = dbChargeList;
            ViewBag.ServiceTypeList = dbServList;
            ViewBag.CargoTypeList = dbCargoList;
            ViewBag.ContainerClassList = dbContainerList;
            ViewBag.ConVanRequirementList = dbConRequirementsList;
            ViewBag.EPOList = dbEPOList;
            ViewBag.SAList = dbSAList;
            ViewBag.LDList = dbLDList;
            return View(edtDtl);
        }

        [HttpPost]
        public JsonResult Update(TransactionDetails trndtl)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(trndtl);
            if (isValidModel)
            {
                using (var db = new ContextModel())
                {
                    db.Entry(trndtl).State = EntityState.Modified;
                    db.SaveChanges();
                }
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

    }
}