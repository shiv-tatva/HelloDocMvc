using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(Users user);

        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}