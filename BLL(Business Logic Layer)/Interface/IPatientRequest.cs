using HelloDocMVC.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IPatientRequest
    {
        public void userDetail(Custom obj);
        public int UserExist(string email);


    }
}
