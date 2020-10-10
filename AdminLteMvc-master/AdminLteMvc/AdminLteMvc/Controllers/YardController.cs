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
    public class YardController : Controller
    {
        private ContextModel db = new ContextModel();
        // GET: Yard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(GridParams g, string search)
        {


            var list = db.Yard.Where(o => o.yardATWBkNo.Contains(search) || o.yardStatus.Contains(search) || o.yardConVanNo.Contains(search) || o.yardTrnNo.Contains(search) || o.yardAssignedBy.Contains(search)).AsQueryable();
            return Json(new GridModelBuilder<Models.WEBSales.Yard>(list, g)
            {
                KeyProp = o => o.yardID,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.yardID,
                    o.yardATWBkNo,
                    o.yardStatus,
                    o.yardTrnNo,
                    o.yardAssignedBy,
                    o.yardConVanNo
                }
            }.Build());
        }

        public ActionResult Create()
        {
            var allatw = db.ATW.ToList();
            List<String> trnsyard = new List<String>();
            foreach (var item in allatw)
            {
                var gettrnyard = db.Yard.Where(s => s.yardATWBkNo.Equals(item.atwBkNo)).Select(s => s.yardTrnNo);
                var atwtrn = item.trns.Split(',');
                foreach (var it in gettrnyard)
                {
                    trnsyard.Add(it);
                }
                //string combinedTrns = string.Join(",", trnsyard);                
                if ((atwtrn.Length == trnsyard.Count()) && item.atwStatus.Equals("Active"))
                {
                    var getatwID = item.atwID;
                    var activityInDb = db.ATW.Find(getatwID);
                    db.ATW.Attach(activityInDb);
                    activityInDb.atwStatus = "Closed";
                    db.Entry(activityInDb).Property("atwStatus").IsModified = true;
                    db.SaveChanges();
                }
            }
            

            var atws = db.ATW.Where(s=>s.atwStatus.Equals("Active")).ToList();
            var ass = db.AssignedBy.ToList();
            List<SelectListItem> atwList = new List<SelectListItem>();
            List<SelectListItem> assignedList = new List<SelectListItem>();
            foreach (ATW item in atws)
            {
                atwList.Add(new SelectListItem
                {
                    Text = item.atwBkNo,
                    Value = item.atwBkNo
                });
            }

            foreach (AssignedBy item in ass)
            {
                assignedList.Add(new SelectListItem
                {
                    Text = item.assigned,
                    Value = item.assigned
                });
            }
            ViewBag.ATWNo = atwList;
            ViewBag.AssignedList = assignedList;
            return View();
        }

        [HttpGet]
        public ActionResult GetTrnsFromATWBK(string atwno)
        {
            var getTrnYard = db.Yard.Where(s=>s.yardATWBkNo.Equals(atwno)).Select(s=>s.yardTrnNo);
            var yardtrns = string.Join(",", getTrnYard);
            var forChecking = yardtrns.ToString();

            var getTrns = db.ATW.Where(s => s.atwBkNo.Equals(atwno)).Select(s => s.trns).Single();
            var getbkNo = db.ATW.Where(s => s.atwBkNo.Equals(atwno)).Select(s => s.bkNo).Single();

            var getCompany = db.TransactionDetails.Where(s => s.docNumber.Equals(getbkNo) && getTrns.Contains(s.transactionNo)).ToList();
            var result = new { trndetails = getCompany, yardtrns = forChecking };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetConVanNos(string atwno)
        {
            var getbkNo = db.ATW.Where(s => s.atwBkNo.Equals(atwno)).Select(s => s.bkNo).Single();
            var consize = db.Booking.Where(s => s.docNum.Equals(getbkNo)).Select(s => s.csize).Single();

            var nos = db.ConVanNo.Where(s => s.convan.Equals(consize)).ToList();
            List<SelectListItem> cvnoList = new List<SelectListItem>();
            foreach (ConVanNo item in nos)
            {
                cvnoList.Add(new SelectListItem
                {
                    Text = item.convanNo,
                    Value = item.convanNo
                });
            }

            return Json(cvnoList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Yard data)
        {
            bool status = false;

            var isValidModel = TryUpdateModel(data);
            if (isValidModel)
            {
                db.Yard.Add(data);
                db.SaveChanges();

                status = true;
            }


            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult YardDetail(int ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yard idDtls = db.Yard.Find(ID);

            return View("YardDetail", idDtls);
        }

        public ActionResult GetYardDetails(GridParams g, string search, string atwBkNo, string trns)
        {
            var getbkno = db.ATW.Where(s => s.atwBkNo.Equals(atwBkNo)).Select(s => s.bkNo).Single();
            var list = db.TransactionDetails.Where(o => o.docNumber.Equals(getbkno) && trns.Contains(o.transactionNo));


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

        
    }
}