using DAL_Data_Access_Layer_.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface ICreateAccount
    {
        public createAcc createMain(int aspuserId);
        public void createAccount(createAcc obj);

    }
}
