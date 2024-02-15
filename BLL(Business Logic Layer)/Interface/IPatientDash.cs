using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Data_Access_Layer_.DataModels;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IPatientDash
    {
        //IEnumerable<Request> patientDashInfo(Request obj);

        public List<PatientDashboardData> patientDashInfo(string email);    }
}
