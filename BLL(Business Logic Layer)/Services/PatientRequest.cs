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

            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);
            if (user == null)
            {
                if(obj.email != null)
                {
                    _aspnetuser.Username = obj.email.Substring(0, obj.email.IndexOf('@'));
                }
                if(obj.email != null)
                {
                    _aspnetuser.Email = obj.email;
                }
                if(obj.phone != null)
                {
                    _aspnetuser.Phonenumber = obj.phone;
                }
                
                if(obj.password != null)
                {
                    _aspnetuser.Passwordhash = obj.password;
                }
              
                _aspnetuser.Createddate = DateTime.Now;
                
                _context.Aspnetusers.Add(_aspnetuser);
                _context.SaveChanges();


                var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

                if(userexist != null)
                {
                    _user.Aspnetuserid = userexist.Id;
                    _user.Createdby = userexist.Id;
                }

                if(obj.firstname != null)
                {
                    _user.Firstname = obj.firstname;
                }
              
                if(obj.lastname != null)
                {
                    _user.Lastname = obj.lastname;
                }
                
                if(obj.email != null)
                {
                    _user.Email = obj.email;
                }
                
                
                if(obj.phone != null)
                {
                    _user.Mobile = obj.phone;
                }
                
                _user.Createddate = DateTime.Now;

                _context.Users.Add(_user);
                _context.SaveChanges();
            }


            var clientUser = _context.Users.FirstOrDefault(x => x.Email == obj.email);

            if(clientUser != null)
            {
                _request.Userid = clientUser.Userid;
            }
            
            if(obj.firstname != null)
            {
                _request.Firstname = obj.firstname;
            }
            
            if(obj.lastname != null)
            {
                _request.Lastname = obj.lastname;
            }
            
            if(obj.email != null)
            {
                _request.Email = obj.email;
            }
                
            if(obj.firstname != null)
            {
                _request.Confirmationnumber = obj.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);//here do Logic For unique Confirmation number that will be used by requestclient to fetch particular request from Request table

            }

            _request.Status = 1;
            _request.Createddate = DateTime.Now;
            
            _context.Requests.Add(_request);
            _context.SaveChanges();


            var requestClientId = _context.Requests.FirstOrDefault(x => x.Confirmationnumber == _request.Confirmationnumber);

            if(requestClientId != null)
            {
                _requestclient.Requestid = requestClientId.Requestid;
            }
            if(obj.firstname != null)
            {
                _requestclient.Firstname = obj.firstname;
            }
            if(obj.lastname != null)
            {
                _requestclient.Lastname = obj.lastname;
            }
            
            if(obj.phone != null)
            {
                _requestclient.Phonenumber = obj.phone;
            }


            //_requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            //_requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
            //_requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(8, 2));


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
