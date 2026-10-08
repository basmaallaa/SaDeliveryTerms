using DeliveryTermsBL.IServices.ISalesService;
using DeliveryTermsBL.IServices.ISharedService;
using DeliveryTermsBL.Models;
using DeliveryTermsBL.Models.Sales;
using DeliveryTermsDL.Models.SupplyChain;
using DeliveryTermsDL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.Services.SalesService
{
    public class SaDeliveryTermService : ISaDeliveryTermService
    {
        private readonly IUnitOfWork _uowSC;
        private readonly ISharedService _sharedService;

        public SaDeliveryTermService(IUnitOfWork uowSC, ISharedService sharedService)
        {
            _uowSC = uowSC;
            _sharedService = sharedService;
        }

        public List<SaDeliveryTermModel> GetAll(string culture)
        {
            try
            {
                var isEnglish = culture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
                var terms = _uowSC.saDeliveryTerm.GetAll()
                    .OrderBy(x => x.Code)
                    .Select(x => new SaDeliveryTermModel
                    {
                        Code = x.Code,
                        SName = x.SName,
                        BName = x.BName,
                        Days = x.Days,
                        ActiveFlag = x.ActiveFlag ?? false,
                        Name = isEnglish ? x.SName : x.BName
                    })
                    .ToList();
                return terms;
            }
            catch(Exception ex)
            {
                _sharedService.HandleException(ex);
                return new List<SaDeliveryTermModel>();
            }
        }

        public ResponseModel GetByCode(int code)
        {
            try
            {
                var entity = _uowSC.saDeliveryTerm.GetByCode(code);
                if (entity == null)
                {
                    return NotFound();
                }

                return new ResponseModel
                {
                    Status = true,
                    Data = new SaDeliveryTermModel
                    {
                        Code = entity.Code,
                        SName = entity.SName,
                        BName = entity.BName,
                        Days = entity.Days,
                        ActiveFlag = entity.ActiveFlag ?? false
                    }
                };

            }
            catch (Exception ex)
            {
                return _sharedService.HandleException(ex);
            }
        }

        public ResponseModel Add (SaDeliveryTermModel model, int userId)
        {
            try
            {
                var existing = _uowSC.saDeliveryTerm.GetByCode(model.Code);
                if (existing != null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        MessageAr = "هذا الكود موجود بالفعل",
                        MessageEn = "This code already exists."
                    };

                }
                var entity = new SaDeliveryTerm
                {
                    Code = model.Code,
                    SName = model.SName.Trim(),
                    BName = model.BName.Trim(),
                    Days = model.Days,
                    ActiveFlag = model.ActiveFlag,

                    EntryUser = userId,
                    EntryDate = DateTime.Now
                };
                _uowSC.saDeliveryTerm.Add(entity);
                _uowSC.Commit();

                return new ResponseModel
                {
                    Status = true,
                    Data = entity.Code, 
                    MessageAr = "تمت الإضافة بنجاح.",
                    MessageEn = "Added successfully."
                };
            }
            catch(Exception ex)
            {
                return _sharedService.HandleException(ex);
            }
        }

        public ResponseModel Edit(SaDeliveryTermModel model, int userId)
        {
            try
            {
                var entity = _uowSC.saDeliveryTerm.GetByCode(model.Code);
                if (entity == null)
                    return NotFound();

                entity.SName = model.SName.Trim();
                entity.BName = model.BName.Trim();
                entity.Days = model.Days;
                entity.ActiveFlag = model.ActiveFlag;

                entity.ChangeUser = userId;
                entity.ChangeDate = DateTime.Now;

                _uowSC.saDeliveryTerm.Edit(entity);
                _uowSC.Commit();

                return new ResponseModel
                {
                    Status = true,
                    Data = entity.Code,
                    MessageAr = "تم التعديل بنجاح.",
                    MessageEn = "Updated successfully."
                };
            }
            catch(Exception ex)
            {
                return _sharedService.HandleException(ex);
            }
        }

        public ResponseModel Delete (int code)
        {
            try
            {
                var entity = _uowSC.saDeliveryTerm.GetByCode(code);
                
                if (entity == null)
                    return NotFound();

                _uowSC.saDeliveryTerm.Delete(entity);
                _uowSC.Commit();

                return new ResponseModel
                {
                    Status = true,
                    MessageAr = "تم الحذف بنجاح.",
                    MessageEn = "Deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return _sharedService.HandleException(ex);
            }
        }

        private static ResponseModel NotFound() => new()
        {
            Status = false,
            MessageAr = "السجل غير موجود.",
            MessageEn = "Record not found."
        };
    }
}
