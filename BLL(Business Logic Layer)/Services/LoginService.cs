using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataModels;
using DAL_Data_Access_Layer_.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.EntityFrameworkCore;

namespace BLL_Business_Logic_Layer_.Services
{ 
    public class LoginService : ILoginService
    {

        private readonly ApplicationDbContext db;

        public LoginService(ApplicationDbContext _db)
        {
               db = _db;
        }

        public Users login(Users obj)
        {
            Users users = new Users();

            var roleIdMain = db.Admins.Where(r => r.Email == obj.Email).Select(x => x.Roleid).First();

            var check = db.Aspnetusers.Where(x => x.Email == obj.Email && x.Passwordhash == obj.Passwordhash).FirstOrDefault();
            if(check != null)
            {
                var data = db.Aspnetusers.Where(x => x.Email == obj.Email && x.Passwordhash == obj.Passwordhash).Select(r => new Users()
                {
                    Id = r.Id,
                    passwordcheck = "true",
                    Email = obj.Email,
                    Passwordhash = obj.Passwordhash,
                    RoleMain = db.Aspnetroles.Where(y => y.Id == db.Aspnetuserroles.Where(x => x.Userid == r.Id).Select(x => x.Roleid).First()).Select(y => y.Name).First(),
                    roleId = (int)roleIdMain,
                }).ToList().FirstOrDefault();
                return data;
            }
            else
            {
               

                 if(db.Aspnetusers.Where(r => r.Email == obj.Email).FirstOrDefault() == null)
                {
                    users.emailcheck = "emailFalse";
                }
                else
                {
                    users.passwordcheck = "passwordFalse";
                }
                return users;
            }

        }
    }
}
