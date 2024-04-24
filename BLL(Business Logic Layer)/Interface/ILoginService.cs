using DAL_Data_Access_Layer_.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface ILoginService
    {
        Users login(Users obj);

        public Users forgotPassword(Users obj);
    }
}
