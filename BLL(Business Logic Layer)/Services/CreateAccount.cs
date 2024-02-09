using BLL_Business_Logic_Layer_.Interface;
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

        public void createAccount(Aspnetuser obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
