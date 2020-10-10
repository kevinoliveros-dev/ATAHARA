using System;
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
    public class ATWController : Controller
    {
        private ContextModel db = new ContextModel();
        // GET: ATW
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(GridParams g, string search)
        {


            var list = db.ATW.Where(o => o.aDriver.Contains(search) || o.aTrucker.Contains(search) || o.atwBkNo.Contains(search) || o.bkNo.Contains(search) || o.conPerson.Contains(search) || o.cShipper.Contains(search) || o.eDate.Contains(search) || o.iDate.Contains(search) || o.remarks.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.ATW>(list, g)
            {
                KeyProp = o => o.atwID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.atwID,
                    o.atwBkNo,
                    o.bkNo,
                    o.iDate,
                    o.eDate,
                    o.issuedBy,
                    o.plateNo,
                    o.aTrucker,
                    o.aDriver,
                    o.cShipper,
                    o.conPerson,
                    o.remarks
                }
            }.Build());
        }

        public ActionResult Create()
        {
            var documents = db.Booking.Where(s=> s.trnStatus.Contains("Active")).ToList();
            List<SelectListItem> documentList = new List<SelectListItem>();
            foreach (Booking item in documents)
            {
                var bkNum = db.TransactionDetails.Where(s => s.docNumber.Equals(item.docNum)).Select(s => s.dtlStatus).ToList();
                if (((IList<string>)bkNum).Contains("Active")) { 
                documentList.Add(new SelectListItem
                {
                    Text = item.docNum,
                    Value = item.docNum
                });
                }
                else
                {
                    var getbkID = item.ID;
                    var activityInDb = db.Booking.Find(getbkID);
                    db.Booking.Attach(activityInDb);
                    activityInDb.trnStatus = "Closed";
                    db.Entry(activityInDb).Property("trnStatus").IsModified = true;
                    db.SaveChanges();
                }
            }
            ViewBag.DocumentNumbers = documentList;
            return View();
        }

        [HttpGet]
        public ActionResult GetIDSFromYear(string yearInput)
        {
            List<string> ids = new List<string>();
            var checkYear = db.ATW.AsEnumerable().Select(r => r.atwYear).ToList();
            var idValue = "";
            if (checkYear.Contains(yearInput))
            {
                ids = db.ATW.Where(s => s.atwYear.Equals(yearInput)).Select(r => r.atwBkID).ToList();
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
        public ActionResult GetDetailsFromBooking(string bknumber)
        {
            var getCompany = db.Booking.Where(s=>s.docNum.Equals(bknumber)).Select(r => r.cnameshpr).SingleOrDefault();
            var company = getCompany.ToString();
            return Json(company, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetATWTrnDetails(string bknumber)
        {
            var getCompany = db.TransactionDetails.Where(s => s.docNumber.Equals(bknumber) && s.dtlStatus.Equals("Active")).ToList();
            return Json(getCompany, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BindBookingToTransaction(string bknumber)
        {
            var getTrns = db.TransactionDetails.Where(s => bknumber.Contains(s.docNumber)).Select(r => r.transactionNo).ToList();
           // string trnLIST = string.Join(",", getTrns);
            return Json(getTrns, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveHdr(ATW data)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(data);
            if (isValidModel)
            {
                var trns = data.trns.Split(',');
                for (int i=0;i<trns.Length;i++)
                {
                    var trnloop = trns[i];
                    var getTrnID = db.TransactionDetails.Where(s => s.docNumber.Equals(data.bkNo) && s.transactionNo.Equals(trnloop)).Select(s=>s.tranID).Single();
                    var detail = new TransactionDetails {tranID = getTrnID };
                    db.TransactionDetails.Attach(detail);
                    detail.dtlStatus = "Closed";                    
                    db.Entry(detail).Property("dtlStatus").IsModified = true;
                    db.SaveChanges();
                }
                
                db.ATW.Add(data);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status, atwid = data.atwID} };
        }

        public ActionResult ATWTrnDetails(int ID)
        {  
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ATW idDtls = db.ATW.Find(ID);
            var bkingNum = idDtls.bkNo.ToString();
            var trndb = db.TransactionDetails.Where(s=>s.docNumber.Equals(bkingNum)).Select(s=> s.transactionNo).ToList();
            List<SelectListItem> trnList = new List<SelectListItem>();
            foreach (var item in trndb)
            {
                trnList.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }
            ViewBag.ListOfTRN = trnList;
            string trnID = idDtls.atwBkNo.ToString();
            var atwdetails = db.ATW.Where(t => t.atwBkNo.Contains(trnID)).ToList();
            if (atwdetails == null)
            {
                return HttpNotFound();
            }
            return View("ATWTrnDetails", idDtls);
        }

        public ActionResult GetATWDetails(GridParams g, string search, string atwBkNo, string bknum)
        {
            //var trns = atwBkNo.Split(',');
            //var transactions = new List<TransactionDetails>();

            //for (int i=0; i<trns.Length;i++)
            //{
            //    var tempTrn = trns[i];
            //    var list = db.TransactionDetails.Where(o => o.transactionNo.Contains(tempTrn) && o.docNumber.Equals(bknum)).Where(x => (x.cargoType.Contains(search) || x.conClass.Contains(search) || x.consignee.Contains(search) || x.consigneeAdd.Contains(search) || x.cyEPO.Contains(search) || x.cyLD.Contains(search) || x.cySA.Contains(search) || x.destination.Contains(search) || x.origin.Contains(search) || x.payMode.Contains(search) || x.priceList.Contains(search)));
            //    transactions.Add((TransactionDetails)list);
            //}

            //var list2 = transactions.AsQueryable();

            var list = db.TransactionDetails.Where(o => atwBkNo.Contains(o.transactionNo) && o.docNumber.Equals(bknum)).Where(x => (x.cargoType.Contains(search) || x.conClass.Contains(search) || x.consignee.Contains(search) || x.consigneeAdd.Contains(search) || x.cyEPO.Contains(search) || x.cyLD.Contains(search) || x.cySA.Contains(search) || x.destination.Contains(search) || x.origin.Contains(search) || x.payMode.Contains(search) || x.priceList.Contains(search)));


            return Json(new GridModelBuilder<Models.WEBSales.TransactionDetails>(list, g)
            {
                KeyProp = o => o.tranID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.tranID,
                    o.transactionDate,
                    o.transactionNo,
                    o.priceList,
                    o.origin,
                    o.destination,
                    o.consignee,
                    o.consigneeAdd,
                    o.cargoType,
                    o.conClass,
                    o.conRqrmnts,
                    o.payMode,
                    o.cyEPO,
                    o.cySA,
                    o.cyLD,
                    o.remarks
                }
            }.Build());
        }


        public ActionResult AddATWDetail(int atwID)
        {
            ATW newDetail = db.ATW.Find(atwID);
            return View("AddATWDetail", newDetail);
        }

        public ActionResult ForPrint(int ID, string atwbkno)
        {
            var trndetail = db.TransactionDetails.Find(ID);
            var atwdetail = db.ATW.Where(s=>s.atwBkNo.Equals(atwbkno)).Single();
            var bkdetail = db.Booking.Where(s=>s.docNum.Equals(atwdetail.bkNo)).Single();

            ViewBag.Trucker = atwdetail.aTrucker;
            ViewBag.ATWBookingNo = atwdetail.atwBkNo;
            ViewBag.Driver = atwdetail.aDriver;
            ViewBag.PlateNo = atwdetail.plateNo;
            ViewBag.Shipper = atwdetail.cShipper;
            ViewBag.BookingNo = atwdetail.bkNo;
            ViewBag.ContactPerson = atwdetail.conPerson;
            ViewBag.IssueDate = atwdetail.iDate;
            ViewBag.ExpiryDate = atwdetail.eDate;
            ViewBag.ConvanSize = bkdetail.csize;
            ViewBag.ConvanStatus = bkdetail.cstatus;

            return View("ForPrint", trndetail);
        }
    }
}