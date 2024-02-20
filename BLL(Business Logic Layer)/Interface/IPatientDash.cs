using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Data_Access_Layer_.DataModels;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using HelloDocMVC.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IPatientDash
    {
        //IEnumerable<Request> patientDashInfo(Request obj);

        public List<PatientDashboardData> patientDashInfo(string email);

        PatientDashboardData UserProfile(string email);

        Custom userMeDetail(string email);

        FamilyFriendData userSomeDetail(string email);

        public void userDetail(Custom obj);

        public void userSomeOneDetail(FamilyFriendData obj);

        public void viewDocumentUpload(PatientDashboard obj);

    }
}
