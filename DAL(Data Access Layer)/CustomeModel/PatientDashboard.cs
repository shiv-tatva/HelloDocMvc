using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class PatientDashboard
    {
        public string? created_date { get; set; }

        public string? current_status { get; set; }

        public string? document { get; set; }
        
        public string? pysicianId { get; set; }
    }
}