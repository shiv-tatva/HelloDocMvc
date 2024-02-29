using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;

namespace BLL_Business_Logic_Layer_.Services
{
    public class ConciergeReq : IConcierge
    {
        private readonly ApplicationDbContext _db;

        public ConciergeReq(ApplicationDbContext context)
        {
            _db = context;
        }

        public void ConciergeDetail(ConciergeCustom obj)
        {
            Concierge _concierge = new Concierge();
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();

            if(obj.concierge_firstname != null)
            {
                _concierge.Conciergename = obj.concierge_firstname;
            }
            
            if(obj.concierge_street != null)
            {
                _concierge.Street = obj.concierge_street;
            }
            
            if(obj.concierge_city != null)
            {
                _concierge.City = obj.concierge_city;
            }
            
            if(obj.concierge_state != null)
            {
                _concierge.State = obj.concierge_state;
            }
             if(obj.concierge_zipcode != null)
            {
                _concierge.Zipcode = obj.concierge_zipcode;
            }

            _concierge.Createddate = DateTime.Now;

            _db.Concierges.Add( _concierge );
            _db.SaveChanges();

            var userMain = _db.Users.FirstOrDefault(x => x.Email == obj.email);


            if( userMain != null )
            {
                _request.Userid = userMain.Userid;
            }

            _request.Requesttypeid = 3;

            if (obj.concierge_firstname != null)
            {
                _request.Firstname = obj.concierge_firstname;
            }

            if (obj.concierge_lastname != null)
            {
                _request.Lastname = obj.concierge_lastname;
            }
            
            if (obj.concierge_phone != null)
            {
                _request.Phonenumber = obj.concierge_phone;
            }
            
            if (obj.concierge_email != null)
            {
                _request.Email = obj.concierge_email;
            }
            
            if (obj.concierge_firstname != null)
            {
                _request.Confirmationnumber = obj.concierge_firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
            }
            
            _request.Status = 2;
            _request.Createddate = DateTime.Now;

            _db.Requests.Add( _request );
            _db.SaveChanges();



            if (_request.Requestid != null)
            {
                _requestclient.Requestid = _request.Requestid;
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
            
            if (obj.email != null)
            {
                _requestclient.Email = obj.email;
            }


            //_requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            //_requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            //_requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));

            _db.Requestclients.Add(_requestclient);
            _db.SaveChanges();
        }
    }
}
