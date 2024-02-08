using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataModels;
using DAL_Data_Access_Layer_.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Services
{
    public class LoginService : ILoginService
    {

        private readonly ApplicationDbContext db;

        public LoginService(ApplicationDbContext _db)
        {
               db = _db;
        }

        public Aspnetuser login(Aspnetuser obj)
        {

            var data = db.Aspnetusers.FirstOrDefault(x => x.Email == obj.Email && x.Passwordhash == obj.Passwordhash);

            return data;

        }
    }
}
