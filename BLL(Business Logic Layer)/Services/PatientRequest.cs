using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
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

                _aspnetuser.Username = obj.email.Substring(0, obj.email.IndexOf('@'));
                _aspnetuser.Email = obj.email;
                _aspnetuser.Createddate = DateTime.Now;
                _aspnetuser.Phonenumber = obj.phone;
                _aspnetuser.Passwordhash = obj.password;
               
                

                _context.Aspnetusers.Add(_aspnetuser);
                _context.SaveChanges();


                var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);
                _user.Aspnetuserid = userexist.Id;
                _user.Firstname = obj.firstname;
                _user.Lastname = obj.lastname;
                _user.Email = obj.email;
                _user.Mobile = obj.phone;
                _user.Street = obj.street;
                _user.City = obj.city;
                _user.Createddate = DateTime.Now;
                _user.Createdby = _aspnetuser.Id;
                _user.State = obj.state;
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Zipcode = obj.zipcode;

                _context.Users.Add(_user);
                _context.SaveChanges();
            }



                _request.Userid = _context.Users.FirstOrDefault(x => x.Email == obj.email).Userid;
                _request.Firstname = obj.firstname;
                _request.Lastname = obj.lastname;
                _request.Email = obj.email;
                _request.Phonenumber = obj.phone;
                _request.Status = 1;
                _request.Createddate = DateTime.Now;
            _request.Confirmationnumber = obj.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);//here do Logic For unique Confirmation number that will be used by requestclient to fetch particular request from Request table

            _context.Requests.Add(_request);
                _context.SaveChanges();

            _requestclient.Requestid = _context.Requests.FirstOrDefault(x => x.Confirmationnumber == _request.Confirmationnumber).Requestid; 
            _requestclient.Firstname = obj.firstname;
                _requestclient.Lastname = obj.lastname;
                _requestclient.Phonenumber = obj.phone;
                _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
                _requestclient.City = obj.city;
                _requestclient.Street = obj.street;
                _requestclient.State = obj.state;

                _context.Requestclients.Add(_requestclient);
                _context.SaveChanges();
            
        }
    }
}
