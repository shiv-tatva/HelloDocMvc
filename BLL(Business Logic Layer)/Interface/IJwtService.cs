using DAL_Data_Access_Layer_.CustomeModel;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(Users user);

        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}