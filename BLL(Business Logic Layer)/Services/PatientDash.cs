using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        //string emailpatient = HttpContext.Session.GetString("UserSession").ToString();

        public List<PatientDashboardData> patientDashInfo(string email)
        {

            var request = db.Requests.Where(r => r.Email == email).AsNoTracking().Select(r => new PatientDashboardData()
            {
                created_date = r.Createddate,
                current_status = r.Status,
                doc_Count = r.Requestwisefiles.Select(f => f.Filename).Count(),
                //docC = db.Requestwisefiles.Where(x => x.Requestid == r.Requestid).Select(x => x.Filename).Count(),
                reqid = r.Requestid,
                cnf_number = r.Confirmationnumber,
                fname = r.Firstname,
                lname = r.Lastname,
                req_type_id = r.Requesttypeid,
                //phy_fname = db.Physicians.Where(x => x.Physicianid == r.Physicianid).Select(x => x.Firstname).ToList()[0]
                documentsname = r.Requestwisefiles.Select(f => f.Filename).ToList(),
                phy_fname = db.Physicians.Single(x => x.Physicianid == r.Physicianid).Firstname,
                user_id_param=r.Requestid
            }).ToList(); ;
           

            return request;


        }

       public PatientDashboardData UserProfile(string email)
        {
            var request = db.Users.Where(r => r.Email == email).AsNoTracking().Select(r => new PatientDashboardData()
            {
                fname = r.Firstname,
                lname = r.Lastname,
                phone_no = r.Mobile,
                street = r.Street,
                state = r.State,
                city = r.City,
                zipcode = r.Zipcode,
                email = email,

            }).ToList()[0];

            return request;
        }
    }
}

