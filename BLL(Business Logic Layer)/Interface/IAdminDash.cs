using DAL_Data_Access_Layer_.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IAdminDash
    {
        public List<adminDash> adminData();
        public List<adminDash> adminDataViewCase(int reqId);
        public viewNotes adminDataViewNote(int reqId);
        public void adminDataViewNote(adminDashData obj);
        
    }
}
