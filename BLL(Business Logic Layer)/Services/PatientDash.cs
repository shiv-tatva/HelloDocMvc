using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
using Microsoft.AspNetCore.Http;
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
                user_id_param = r.Requestid
            }).ToList(); ;


            return request;


        }
        
        
        public List<PatientDashboardData> patientDashInfoTwo(string email,int param)
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
                user_id_param = param
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

        public Custom userMeDetail(string email)
        {
            var requestMe = db.Users.Where(r => r.Email == email).AsNoTracking().Select(r => new Custom()
            {
                email = email,
            }).ToList()[0];

            return requestMe;
        }

        public FamilyFriendData userSomeDetail(string email)
        {
            var requestMe = db.Users.Where(r => r.Email == email).AsNoTracking().Select(r => new FamilyFriendData()
            {
                email = email,
            }).ToList()[0];

            return requestMe;
        }

        public int UserExist(string email)
        {
            var exist = db.Aspnetusers.FirstOrDefault(u => u.Email == email);
            if (exist == null)
            {
                return 402;
            }
            else
            {
                return 401;
            }

        }

        public void userDetail(Custom obj)
        {

            User _user = new User();
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();
            Aspnetuser _aspnetuser = new Aspnetuser();

            var user = db.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);
            if (user == null)
            {
                if (obj.email != null)
                {
                    _aspnetuser.Username = obj.email.Substring(0, obj.email.IndexOf('@'));
                }
                if (obj.email != null)
                {
                    _aspnetuser.Email = obj.email;
                }
                if (obj.phone != null)
                {
                    _aspnetuser.Phonenumber = obj.phone;
                }

                if (obj.password != null)
                {
                    _aspnetuser.Passwordhash = obj.password;
                }

                _aspnetuser.Createddate = DateTime.Now;

                db.Aspnetusers.Add(_aspnetuser);
                db.SaveChanges();


                var userexist = db.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

                if (userexist != null)
                {
                    _user.Aspnetuserid = userexist.Id;
                    _user.Createdby = userexist.Id;
                }

                if (obj.firstname != null)
                {
                    _user.Firstname = obj.firstname;
                }

                if (obj.lastname != null)
                {
                    _user.Lastname = obj.lastname;
                }

                if (obj.email != null)
                {
                    _user.Email = obj.email;
                }


                if (obj.phone != null)
                {
                    _user.Mobile = obj.phone;
                }

                _user.Createddate = DateTime.Now;

                db.Users.Add(_user);
                db.SaveChanges();
            }


            var clientUser = db.Users.FirstOrDefault(x => x.Email == obj.email);

            if (clientUser != null)
            {
                _request.Userid = clientUser.Userid;
            }

            if (obj.firstname != null)
            {
                _request.Firstname = obj.firstname;
            }

            if (obj.lastname != null)
            {
                _request.Lastname = obj.lastname;
            }

            if (obj.email != null)
            {
                _request.Email = obj.email;
            }

            if (obj.firstname != null)
            {
                _request.Confirmationnumber = obj.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);//here do Logic For unique Confirmation number that will be used by requestclient to fetch particular request from Request table

            }

            _request.Status = 1;
            _request.Createddate = DateTime.Now;

            db.Requests.Add(_request);
            db.SaveChanges();


            var requestClientId = db.Requests.FirstOrDefault(x => x.Confirmationnumber == _request.Confirmationnumber);

            if (requestClientId != null)
            {
                _requestclient.Requestid = requestClientId.Requestid;
            }
            if (obj.firstname != null)
            {
                _requestclient.Firstname = obj.firstname;
            }
            if (obj.lastname != null)
            {
                _requestclient.Lastname = obj.lastname;
            }

            if (obj.phone != null)
            {
                _requestclient.Phonenumber = obj.phone;
            }


            //_requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            //_requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
            //_requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(8, 2));


            db.Requestclients.Add(_requestclient);
            db.SaveChanges();

            if (obj.upload != null)
            {
                string filename = obj.upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj.upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Request? req = db.Requests.FirstOrDefault(i => i.Email == obj.email);
                int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Requestid = _request.Requestid,
                    Filename = filename,
                    Createddate = DateTime.Now,
                };
                db.Requestwisefiles.Add(data3);
                db.SaveChanges();
            }

        }


        public void userSomeOneDetail(FamilyFriendData obj)
        {

            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();

            var clientUser = db.Users.FirstOrDefault(x => x.Email == obj.email);

            if (clientUser != null)
            {
                _request.Userid = clientUser.Userid;
            }

            if (clientUser.Firstname != null)
            {
                _request.Firstname = clientUser.Firstname;
            }

            if (clientUser.Lastname != null)
            {
                _request.Lastname = clientUser.Lastname;
            }

            if (clientUser.Email != null)
            {
                _request.Email = clientUser.Email;
            }

            if (clientUser.Firstname != null)
            {
                _request.Confirmationnumber = clientUser.Firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);//here do Logic For unique Confirmation number that will be used by requestclient to fetch particular request from Request table

            }


            if (obj.relation != null)
            {
                _request.Relationname = obj.relation;
            }

            _request.Status = 1;
            _request.Createddate = DateTime.Now;

            db.Requests.Add(_request);
            db.SaveChanges();


            var requestClientId = db.Requests.FirstOrDefault(x => x.Confirmationnumber == _request.Confirmationnumber);

            if (requestClientId != null)
            {
                _requestclient.Requestid = requestClientId.Requestid;
            }
            if (obj.firstname != null)
            {
                _requestclient.Firstname = obj.firstname;
            }
            if (obj.lastname != null)
            {
                _requestclient.Lastname = obj.lastname;
            }

            if (obj.phone != null)
            {
                _requestclient.Phonenumber = obj.phone;
            }


            //_requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            //_requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
            //_requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(8, 2));


            db.Requestclients.Add(_requestclient);
            db.SaveChanges();

            if (obj.upload != null)
            {
                string filename = obj.upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj.upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Request? req = db.Requests.FirstOrDefault(i => i.Email == obj.email);
                int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Requestid = _request.Requestid,
                    Filename = filename,
                    Createddate = DateTime.Now,
                };
                db.Requestwisefiles.Add(data3);
                db.SaveChanges();
            }

        }


        Request _request = new Request();

        public void viewDocumentUpload(PatientDashboard obj)
        {
            if (obj.Upload != null)
            {
                string filename = obj.Upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj.Upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Request? req = db.Requests.FirstOrDefault(i => i.Requestid == obj.data[0].reqid);
                int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Requestid = db.Requests.FirstOrDefault(i => i.Requestid == obj.data[0].reqid).Requestid,
                    Filename = filename,
                    Createddate = DateTime.Now,
                };
                db.Requestwisefiles.Add(data3);
                db.SaveChanges();
            }
        }



    }
}

