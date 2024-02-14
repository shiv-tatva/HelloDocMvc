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
    public class PatientDash : IPatientDash
    {
        private readonly ApplicationDbContext db;

        public PatientDash(ApplicationDbContext _db)
        {
            db = _db;
        }

        //public IEnumerable<Request> patientDashInfo(Request obj)
        //{
        //    IEnumerable<Request> list = db.Requests.ToList();
        //    return list;
        //}
        public List<PatientDashboard> patientDashInfo()
        {
            var user = db.Requests.Where(x => x.Requestid == 19).FirstOrDefault();

            return new List<PatientDashboard>()
            {
                new PatientDashboard {created_date = user.Createddate.ToString(), current_status = user.Status.ToString() , document = "DOC"}
            };
        }
    }
}

