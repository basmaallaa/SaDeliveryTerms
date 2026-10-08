using DeliveryTermsBL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.IServices.ISharedService
{
    public interface ISharedService
    {
        ResponseModel HandleException(Exception ex);
    }
}
