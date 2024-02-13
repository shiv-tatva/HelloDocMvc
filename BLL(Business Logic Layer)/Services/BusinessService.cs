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
    public class BusinessService : IBusiness
    {
        private readonly ApplicationDbContext _context;

        public BusinessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void businessInfo(BusinessCustome obj)
        {
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();  
            Business _business = new Business();
            Requestbusiness _requestbusiness = new Requestbusiness();

            _request.Requesttypeid = 4;

            var user = _context.Users.FirstOrDefault(x => x.Email == obj.email);

            if(user != null)
            {
                _request.Userid = user.Userid;
            }

            if (obj.business_firstname != null)
            {
                _request.Firstname = obj.business_firstname;
            }
            
            if (obj.business_lastname != null)
            {
                _request.Lastname = obj.business_lastname;
            }
            
            if (obj.business_phone != null)
            {
                _request.Phonenumber = obj.business_phone;
            }
            
            if (obj.business_email != null)
            {
                _request.Email = obj.business_email;
            }
             
            if (obj.business_casenumber != null)
            {
                _request.Casenumber = obj.business_casenumber;
            }

           
            _context.Requests.Add(_request);
            _context.SaveChanges();

            if(_request.Requestid != null)
            {
                _requestclient.Requestid = _request.Requestid;
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
           
           
            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

            if(obj.business_property != null)
            {
                _business.Name = obj.business_property;
            }
            _business.Createddate = DateTime.Now;
            _context.Businesses.Add(_business);
            _context.SaveChanges();


            if(_request.Requestid != null)
            {
                _requestbusiness.Requestid = _request.Requestid;
            }
            
            if(_business.Businessid != null)
            {
                _requestbusiness.Businessid = _business.Businessid;
            }

            _context.Requestbusinesses.Add(_requestbusiness);
            _context.SaveChanges();


        }
    }
}
