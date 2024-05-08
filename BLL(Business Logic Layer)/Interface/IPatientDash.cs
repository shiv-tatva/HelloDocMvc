using DAL_Data_Access_Layer_.CustomeModel;
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

        public void userSomeOneDetail(FamilyFriendData obj, string email);

        public void viewDocumentUpload(PatientDashboard obj);

        public List<PatientDashboardData> patientDashInfoTwo(string email, int param);

        public reviewAgreement reviewAgree(int reqId);
        public void reviewAgree(PatientDashboard obj);
        public void agreeMain(int reqId);

        public bool checkstatus(int reqId);

        
    }
}
