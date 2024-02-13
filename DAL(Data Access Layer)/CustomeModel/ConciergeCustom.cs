using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class ConciergeCustom
    {


        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? concierge_firstname { get; set; }

        public string? concierge_lastname { get; set; }

        public string? concierge_phone { get; set; }

        public string? concierge_email { get; set; }

        public string? concierge_hotelname { get; set; }


        [Required(ErrorMessage = "Please Enter Street Name:")]
        public string? concierge_street { get; set; }

        [Required(ErrorMessage = "Please Enter City Name:")]
        public string? concierge_city { get; set; }

        [Required(ErrorMessage = "Please Enter State Name:")]
        public string? concierge_state { get; set; }

        [Required(ErrorMessage = "Please Enter Zipcode Name:")]
        public string? concierge_zipcode { get; set; }

        public string? symptoms { get; set; }

        public string? firstname { get; set; }

        public string? lastname { get; set; }

        public string? dateofbirth { get; set; }

        public string? email { get; set; }

        public string? phone { get; set; }

        public string? room { get; set; }
    }
}
