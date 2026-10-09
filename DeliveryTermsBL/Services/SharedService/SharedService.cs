using DeliveryTermsBL.IServices.ISharedService;
using DeliveryTermsBL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.Services.SharedService
{
    public class SharedService : ISharedService
    {
        private readonly ILogger<SharedService> _logger;
        public SharedService(ILogger<SharedService> logger)
        {
            _logger = logger;
        }
        public ResponseModel HandleException(Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);

            if(ex is DbUpdateException && ex.InnerException?.Message.Contains("ORA-00001")== true)
            {
                return new ResponseModel
                {
                    Status = false,
                    MessageAr = "هذا الكود موجود بالفعل.",
                    MessageEn = "This code already exists."
                };
            }
            return new ResponseModel
            {
                Status = false,
                MessageAr = "حدث خطأ غير متوقع.",
                MessageEn = "An unexpected error occurred."
            };
        }
    }
}
