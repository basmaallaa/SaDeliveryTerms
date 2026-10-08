using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeliveryTermsBL.Models.Sales
{
    public class SaDeliveryTermModel
    {
        [Range(1, 999)]
        public int Code { get; set; }

        [Required, StringLength(60)]
        public string SName { get; set; } = "";

        [Required, StringLength(60)]
        public string BName { get; set; } = "";

        [Range(0, 9999)]
        public int? Days { get; set; }

        public bool ActiveFlag { get; set; } = true;

        public string? Name { get; set; }
    }
}
