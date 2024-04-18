using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BLL_Business_Logic_Layer_.Services
{
    public class PatientRequest : IPatientRequest
    {

        private readonly ApplicationDbContext _context;

        public PatientRequest(ApplicationDbContext context)
        {
            _context = context;
        }

        public int UserExist(string email)
        {
            var exist = _context.Aspnetusers.FirstOrDefault(u => u.Email == email);
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
            Aspnetuserrole _role = new Aspnetuserrole();

            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);
            if (user == null)
            {
                _aspnetuser.Username = obj.firstname + "_" + obj.lastname;
                _aspnetuser.Email = obj.email;
                _aspnetuser.Phonenumber = obj.phone;
                _aspnetuser.Passwordhash = obj.password;
                _aspnetuser.Createddate = DateTime.Now;
                
                _context.Aspnetusers.Add(_aspnetuser);
                _context.SaveChanges();


                var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

                
                _user.Aspnetuserid = userexist.Id;
                _user.Createdby = userexist.Id;
                _user.Firstname = obj.firstname;
                _user.Lastname = obj.lastname;
                _user.Email = obj.email;
                _user.Mobile = obj.phone;
                _user.Street = obj.street;
                _user.City = obj.city;
                _user.State = _context.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
                _user.Zipcode = obj.zipcode;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Regionid = obj.regionId;
                _user.Createddate = DateTime.Now;

                _context.Users.Add(_user);
                _context.SaveChanges();

                _role.Userid = _aspnetuser.Id;
                _role.Roleid = 2;

                _context.Aspnetuserroles.Add(_role);
                _context.SaveChanges();
            }


            var clientUser = _context.Users.FirstOrDefault(x => x.Email == obj.email);

           
            _request.Userid = clientUser.Userid;
            _request.Firstname = obj.firstname;
            _request.Lastname = obj.lastname;
            _request.Email = obj.email;
            _request.Phonenumber = obj.phone;
            _request.Confirmationnumber = obj.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);//here do Logic For unique Confirmation number that will be used by requestclient to fetch particular request from Request table
            _request.Status = 1;
            _request.Createddate = DateTime.Now;
            
            _context.Requests.Add(_request);
            _context.SaveChanges();


            var requestClientId = _context.Requests.FirstOrDefault(x => x.Confirmationnumber == _request.Confirmationnumber);
                   
            _requestclient.Requestid = requestClientId.Requestid;
            _requestclient.Firstname = obj.firstname;
            _requestclient.Lastname = obj.lastname;
            _requestclient.Phonenumber = obj.phone;
            _requestclient.Email = obj.email;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear =  Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.City = obj.city;
            _requestclient.Street = obj.street;
            _requestclient.State = _context.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
            _requestclient.Zipcode = obj.zipcode;
            _requestclient.Notes = obj.symptoms;
            _requestclient.Regionid = obj.regionId;


            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

            if(obj.upload != null)
            {
                string filename = obj.upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj.upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Request? req = _context.Requests.FirstOrDefault(i => i.Email == obj.email);
                int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Requestid = _request.Requestid,
                    Filename = filename,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(data3);
                _context.SaveChanges();
            }

        }

        
    }
} 
