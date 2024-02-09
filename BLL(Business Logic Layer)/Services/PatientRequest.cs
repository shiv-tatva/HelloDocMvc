using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMvc.CustomeModel.Custome;
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

        public void userDetail(Custome obj)
        {

            User _user = new User();
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();

            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

            if (user == null)
            {
                throw new Exception("plz enter email");
            }
            else
            {
                var patientUser = _context.Users.FirstOrDefault(x => x.Userid == _user.Userid);


                if(patientUser == null)
                {
                    //_user.Aspnetuserid = user.Id;
                    //_user.Intdate = obj.dateofbirth.
                }
                else
                {


                }


                _context.Add(obj);
                _context.SaveChanges();
            }
        }
    }
}
