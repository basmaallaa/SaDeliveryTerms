using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsBL.IServices.IAuthService
{
    public interface IJwtTokenService
    {
        string CreateToken(int userId, int companyCode, string username);
    }
}
