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

            _concierge.Conciergename = obj.concierge_firstname;
            _concierge.Street = obj.concierge_street;
            _concierge.City = obj.concierge_city;
            _concierge.State = obj.concierge_state;
            _concierge.Zipcode = obj.concierge_zipcode;
            _concierge.Createddate = DateTime.Now;

            _db.Concierges.Add( _concierge );
            _db.SaveChanges();

            var userMain = _db.Users.FirstOrDefault(x => x.Email == obj.email);


            if( userMain != null )
            {
                _request.Userid = userMain.Userid;
            }

            _request.Requesttypeid = 2;
            _request.Firstname = obj.concierge_firstname;
            _request.Lastname = obj.concierge_lastname;
            _request.Phonenumber = obj.concierge_phone;
            _request.Email = obj.concierge_email;
            _request.Status = 2;
            _request.Createddate = DateTime.Now;
            _request.Confirmationnumber = obj.concierge_firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);



            _db.Requests.Add( _request );
            _db.SaveChanges();


         
            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = obj.firstname;
            _requestclient.Lastname = obj.lastname;
            _requestclient.Phonenumber = obj.phone;
            _requestclient.Email = obj.email;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));

            _db.Requestclients.Add(_requestclient);
            _db.SaveChanges();
        }
    }
}
