using DAL_Data_Access_Layer_.CustomeModel;
using HelloDocMVC.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IFamilyFriend
    {
        public void FamilyFriendInfo(FamilyFriendData data);

        public Task EmailSendar(string email, string subject, string message);
    }
}
