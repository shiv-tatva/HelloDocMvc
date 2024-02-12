using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Data_Access_Layer_.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IConcierge
    {
        public void ConciergeDetail(ConciergeCustom obj);

    }
}
