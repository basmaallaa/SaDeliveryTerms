using DeliveryTermsBL.Models;
using DeliveryTermsBL.Models.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.IServices.ISalesService
{
    public interface ISaDeliveryTermService
    {
        List<SaDeliveryTermModel> GetAll(string culture);
        ResponseModel GetByCode(int code);
        ResponseModel Add(SaDeliveryTermModel model, int userId);
        ResponseModel Edit(SaDeliveryTermModel model, int userId);
        ResponseModel Delete(int code);


    }
}
