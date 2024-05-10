using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net;
using System.Net.Mail;

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

            var uid = db.Users.Where(r => r.Email == email).Select(x => x.Userid).First();
            var request = db.Requests.Where(r => r.Userid == uid).AsNoTracking();

            var requestMain = request.Select(r => new PatientDashboardData()
            {
                created_date = r.Createddate,
                current_status = r.Status,
                doc_Count = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(f => f.Filename).Count(),
                //docC = db.Requestwisefiles.Where(x => x.Requestid == r.Requestid).Select(x => x.Filename).Count(),
                reqid = r.Requestid,
                cnf_number = r.Confirmationnumber,
                fname = r.Firstname,
                lname = r.Lastname,
                req_type_id = r.Requesttypeid,
                status = r.Status.ToString(),
                //phy_fname = db.Physicians.Where(x => x.Physicianid == r.Physicianid).Select(x => x.Firstname).ToList()[0]
                documentsname = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(f => f.Filename).ToList(),
                phy_fname = db.Physicians.Single(x => x.Physicianid == r.Physicianid).Firstname,
                user_id_param = r.Requestid,
                physician_id = r.Physicianid ?? 0,
                admin_id = 1,
            }).OrderByDescending(r => r.reqid).ToList(); ;



            return requestMain;


        }

        public List<Admin> adminDataMain()
        {
            var adminData = db.Admins.ToList();

            return adminData;
        }


        public List<PatientDashboardData> patientDashInfoTwo(string email, int param)
        {

            var uid = db.Users.Where(r => r.Email == email).Select(x => x.Userid).First();
            var request = db.Requests.Where(r => r.Userid == uid).AsNoTracking();

            var requestMain = request.Select(r => new PatientDashboardData()
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
                documentsname = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(f => f.Filename).ToList(),
                phy_fname = db.Physicians.Single(x => x.Physicianid == r.Physicianid).Firstname,
                user_id_param = param
            }).ToList(); ;


            return requestMain;


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
                fulldateofbirth = new DateTime(Convert.ToInt16(r.Intyear), Convert.ToInt16(r.Strmonth), Convert.ToInt16(r.Intdate)).ToString("yyyy-MM-dd"),
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

                _user.Street = obj.street;
                _user.City = obj.city;
                _user.State = db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
                _user.Zipcode = obj.zipcode;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Regionid = obj.regionId;

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
            _request.Phonenumber = obj.phone;


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

            _requestclient.Email = obj.email;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.City = obj.city;
            _requestclient.Street = obj.street;
            _requestclient.State = db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
            _requestclient.Zipcode = obj.zipcode;
            _requestclient.Notes = obj.symptoms;
            _requestclient.Regionid = obj.regionId;


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

        public void SendRegistrationEmail(string toEmail, string registrationLink, string ReqEmail)
        {
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Create Account ",
                IsBodyHtml = true,
                Body = $"Click the following link to complete your registration: <a href='{registrationLink}'>{registrationLink}</a>"
            };


            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + toEmail + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = toEmail,
                Roleid = 2,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = ReqEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (db.Requests.Any(r => r.Email == ReqEmail))
            {
                emailLog.Requestid = db.Requests.Where(r => r.Email == ReqEmail).Select(r => r.Requestid).First();
            }


            db.Emaillogs.Add(emailLog);
            db.SaveChanges();

            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }

        public void userSomeOneDetail(FamilyFriendData obj, string email)
        {
            User _user = new User();
            Aspnetuser _aspnetuser = new Aspnetuser();
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();
            Aspnetuserrole _role = new Aspnetuserrole();

            var clientUser = db.Users.FirstOrDefault(x => x.Email == obj.email);

            var user = db.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

            if (user == null)
            {
                _aspnetuser.Username = obj.firstname + "_" + obj.lastname;
                _aspnetuser.Email = obj.email;
                _aspnetuser.Phonenumber = obj.phone;
                _aspnetuser.Createddate = DateTime.Now;

                db.Aspnetusers.Add(_aspnetuser);
                db.SaveChanges();

                _user.Aspnetuserid = _aspnetuser.Id;
                _user.Createdby = _aspnetuser.Id;
                _user.Firstname = obj.firstname;
                _user.Lastname = obj.lastname;
                _user.Email = obj.email;
                _user.Mobile = obj.phone;
                _user.Street = obj.street;
                _user.City = obj.city;
                _user.State = db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
                _user.Zipcode = obj.zipcode;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Createddate = DateTime.Now;
                _user.Regionid = obj.regionId;

                db.Users.Add(_user);
                db.SaveChanges();

                _role.Userid = _aspnetuser.Id;
                _role.Roleid = 2;

                db.Aspnetuserroles.Add(_role);
                db.SaveChanges();
            }

            _request.Requesttypeid = 2;

            if (user == null)
            {
                _request.Userid = _user.Userid;
            }
            else
            {
                var a = db.Users.Where(r => r.Email == obj.email).Select(r => r.Userid).First();
                _request.Userid = a;
            }

            var loginPatient = db.Users.Where(x => x.Email == email).FirstOrDefault();

            _request.Firstname = loginPatient.Firstname;
            _request.Lastname = loginPatient.Lastname;
            _request.Phonenumber = loginPatient.Mobile;
            _request.Email = email;
            _request.Relationname = obj.relation;
            _request.Confirmationnumber = loginPatient.Firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
            _request.Createddate = DateTime.Now;
            _request.Status = 1;

            db.Requests.Add(_request);
            db.SaveChanges();


            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = obj.firstname;
            _requestclient.Lastname = obj.lastname;
            _requestclient.Phonenumber = obj.phone;
            _requestclient.Email = obj.email;
            _requestclient.Notes = obj.symptoms;
            _requestclient.Street = obj.street;
            _requestclient.City = obj.city;
            _requestclient.State = db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
            _requestclient.Zipcode = obj.zipcode;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.Regionid = obj.regionId;

            db.Requestclients.Add(_requestclient);
            db.SaveChanges();

            if (user == null)
            {
                string emailConfirmationToken = Guid.NewGuid().ToString();

                string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _aspnetuser.Id;

                //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                try
                {
                    SendRegistrationEmail(obj.email, registrationLink, _request.Email);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (obj.upload != null)
            {
                string filename = obj.upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj.upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }


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


        public reviewAgreement reviewAgree(int reqId)
        {
            reviewAgreement reviewAgreement = new reviewAgreement();
            reviewAgreement.reqid = reqId;

            return reviewAgreement;
        }

        public bool checkstatus(int reqId)
        {
            var req = db.Requests.Any(x => x.Requestid == reqId && (x.Status == 4 || x.Status == 7));
            if (req)
            {
                return true;
            }

            return false;
        }

        public void reviewAgree(PatientDashboard obj)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();

            requeststatuslog.Requestid = obj._reviewAgreement.reqid;
            requeststatuslog.Status = 7;
            requeststatuslog.Notes = obj._reviewAgreement.notes;
            requeststatuslog.Createddate = DateTime.Now;

            db.Requeststatuslogs.Add(requeststatuslog);
            db.SaveChanges();

            var req = db.Requests.Where(r => r.Requestid == obj._reviewAgreement.reqid).Select(r => r).FirstOrDefault();

            req.Status = 7;
            db.SaveChanges();
        }

        public void agreeMain(int reqId)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();

            requeststatuslog.Requestid = reqId;
            requeststatuslog.Status = 4;
            requeststatuslog.Createddate = DateTime.Now;

            db.Requeststatuslogs.Add(requeststatuslog);
            db.SaveChanges();

            var req = db.Requests.Where(r => r.Requestid == reqId).Select(r => r).FirstOrDefault();

            req.Status = 4;
            db.SaveChanges();
        }
       
    }
}

