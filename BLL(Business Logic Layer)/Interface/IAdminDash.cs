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

        public CloseCase closeCaseNote(int reqId);
        public void closeCaseNote(adminDashData obj);

        public List<casetageNote> casetag();

        public AssignCase adminDataAssignCase(int req);
        public void adminDataAssignCase(adminDashData assignObj);
        
        
        public AssignCase adminDataAssignCaseDocList(int regionId);

    }
}
