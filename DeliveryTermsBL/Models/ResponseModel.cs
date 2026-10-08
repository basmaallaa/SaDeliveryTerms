using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.Models
{
    public class ResponseModel
    {
        public bool Status { get; set; }

        public object? Data { get; set; }

        public string? MessageAr { get; set; }

        public string? MessageEn { get; set; }
    }
}
