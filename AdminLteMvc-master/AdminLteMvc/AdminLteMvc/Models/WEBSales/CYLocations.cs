using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminLteMvc.Models.WEBSales
{
    public class CYLocations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int cyID { get; set; }
        public string cyCode { get; set; }
        public string cyLocation { get; set; }
    }
}