﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models.WEBSales
{
    public class ProformaBills
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int proformaBillID { get; set; }
        public string proformaBillRefNo { get; set; }
        public string proformaBillDate { get; set; }
        public string proformaBillVesselName { get; set; }
        public string proformaBillVoyageNo { get; set; }
        public string proformaBillDestination { get; set; }
        public string proformaBillShipper { get; set; }
        public string proformaBillShippersAddress { get; set; }
        public string proformaBillShippersTelNo { get; set; }
        public string proformaBillServiceType { get; set; }
        public string proformaBillConsignee { get; set; }
        public string proformaBillConsigneesAddress { get; set; }
        public string proformaBillConsigneesTelNo { get; set; }
        public string proformaBillNo { get; set; }
        public string proformaBillMarks { get; set; }
        public string proformaBillQty { get; set; }
        public string proformaBillUnit { get; set; }
        public string proformaBillDescriptionOfCargo { get; set; }
        public string proformaBillValue { get; set; }
        public string proformaBillWeight { get; set; }
        public string proformaBillMeasurement { get; set; }
        public string proformaBillRemarks { get; set; }
        public string proformaBillMeasuredBy { get; set; }
        public string proformaBillTruckersName { get; set; }
        public string proformaBillShippersName { get; set; }
    }
}