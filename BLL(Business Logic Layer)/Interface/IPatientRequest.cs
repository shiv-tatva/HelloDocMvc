using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloDocMVC.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IPatientRequest
    {
        public void userDetail(Custom obj);
        public int UserExist(string email);


    }
}
