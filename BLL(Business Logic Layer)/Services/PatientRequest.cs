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
    public class PatientRequest : IPatientRequest
    {

        private readonly ApplicationDbContext _context;

        public PatientRequest(ApplicationDbContext context)
        {
            _context = context;
        }

        public void userDetail(User obj)
        {
            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.Email);

            if (user == null)
            {
                throw new Exception("plz enter email");


            }
            else
            {
                obj.Aspnetuserid = user.Id;
                _context.Add(obj);
                _context.SaveChanges();
            }
        }
    }
}
