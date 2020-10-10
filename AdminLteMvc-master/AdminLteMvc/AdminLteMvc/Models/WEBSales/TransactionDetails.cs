using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models.WEBSales
{
    public class TransactionDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tranID { get; set; }
        public int transactionID { get; set; }
        public string documentID { get; set; }
        public string transactionNo { get; set; }
        public string dtlStatus { get; set; }
        public string priceList { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string consignee { get; set; }
        public string consigneeAdd { get; set; }
        public string consigneetelno { get; set; }
        public string cargoType { get; set; }
        public string conClass { get; set; }
        public string conRqrmnts { get; set; }
        public string quantity { get; set; }
        public string unitofmeasurement { get; set; }
        public string payMode { get; set; }
        public string cyEPO { get; set; }
        public string cySA { get; set; }
        public string cyLD { get; set; }
        public string remarks { get; set; }
        public string chargeTo { get; set; }
        public string serviceType { get; set; }
        public string transactionDate { get; set; }
        public string trnInputtedDate { get; set; }
        public string docNumber { get; set; }
        public string price { get; set; }

    }
}