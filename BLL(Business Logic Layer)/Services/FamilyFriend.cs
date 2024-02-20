using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Services
{
    public class FamilyFriend : IFamilyFriend
    {
        private readonly ApplicationDbContext _context;

        public FamilyFriend  (ApplicationDbContext context)
        {
            _context = context;
        }


        public Task EmailSendar(string email, string subject, string message)
        {
            var mail = "shivsantoki@gmail.com";
            var password = "qrmi ubhn iuvq gujh";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));

        }


        public void FamilyFriendInfo(FamilyFriendData data)
        {
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();

            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == data.email);

            _request.Requesttypeid = 3;
            if (user != null)
            {
                _request.Userid = user.Id;
            }

            if(data.ff_firstname != null)
            {
                 _request.Firstname = data.ff_firstname;
            }
            
            if(data.ff_lastname != null)
            {
                _request.Lastname = data.ff_lastname;
            }
            
            
            if(data.ff_phone != null)
            {
                _request.Phonenumber = data.ff_phone;
            }
            
            
            if(data.ff_email != null)
            {
                _request.Email = data.ff_email;
            }
            
            
            if(data.ff_relation != null)
            {
                _request.Relationname = data.ff_relation;
            }


            _request.Createddate = DateTime.Now;
            _request.Status = 1;
           
            _context.Requests.Add(_request);
            _context.SaveChanges();

            var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == data.email);

            if (_request.Requestid != null)
            {
                _requestclient.Requestid = _request.Requestid;
            }
            
            
            if(data.firstname != null)
            {
                _requestclient.Firstname = data.firstname;
            }
            
            
            if(data.firstname != null)
            {
                _requestclient.Lastname = data.lastname;
            }
            
            if(data.phone != null)
            {
                _requestclient.Phonenumber = data.phone;
            } 
            if(data.email != null)
            {
                if(userexist == null)
                {
                    var receiver = data.email;
                    var subject = "Create Your Account";
                    var message = "Tap on link for Create Account: https://localhost:7173/Home/CreateAccount";
                    try
                    {
                        EmailSendar(receiver, subject, message);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    _requestclient.Email = data.email;
                }
                
            }
           
            //_requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            //_requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
            //_requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(8, 2));

            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

        }
    }
}
