using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class BusinessCustome
    {

        [Required(ErrorMessage = "Please Enter First Name")]
        public string? business_firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string? business_lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        public string? business_phone { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? business_email { get; set; }

        [Required(ErrorMessage = "Please Enter Property/Business Name")]
        public string? business_property { get; set; }

        public string? business_casenumber { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        public string? street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string? state { get; set; }

        [Required(ErrorMessage = "Please Enter zipcode")]
        public string? zipcode { get; set; }

        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Birth")]
        public string? dateofbirth { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        public string? phone { get; set; }

        public string? room { get; set; }
    }
}
