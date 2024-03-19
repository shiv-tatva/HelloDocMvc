using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Services
{
    public class CreateAccount : ICreateAccount
    {
        private readonly ApplicationDbContext _context;

        public CreateAccount(ApplicationDbContext context)
        {
            _context = context;
        }

        public createAcc createMain(int aspuserId)
        {
            createAcc user = new createAcc();

            user.aspnetUserId = aspuserId;
            user.email = _context.Aspnetusers.Where(r => r.Id == aspuserId).Select(r => r.Email).First();

            return user;
        }

        public void createAccount(createAcc obj)
        {
            var emailUser = _context.Aspnetusers.Where(r => r.Id == obj.aspnetUserId).Select(r => r).First();

            emailUser.Passwordhash = obj.password;

            _context.SaveChanges();
        }
    }
}
