using DAL_Data_Access_Layer_.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IProviderDash
    {
        public void ProivderAccept(int reqId);

        public void physicianDataViewNote(adminDashData obj);
    }
}
