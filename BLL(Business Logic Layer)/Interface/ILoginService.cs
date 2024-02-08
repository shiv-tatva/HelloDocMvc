using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface ILoginService
    {
        Aspnetuser login(Aspnetuser obj);
    }
}
