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
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? business_firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? business_lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? business_phone { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? business_email { get; set; }

        [Required(ErrorMessage = "Please Enter Property/Business Name")]
        public string? business_property { get; set; }

        public string? business_casenumber { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        public string? street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }

        [Required(ErrorMessage = "Please Enter zipcode")]
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]

        public string? zipcode { get; set; }


        [RegularExpression(@"^\S.*$", ErrorMessage = "Symptoms cannot be null or whitespace.")]
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Birth")]
        public string? dateofbirth { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? phone { get; set; }

        public string? room { get; set; }

        public List<DAL_Data_Access_Layer_.DataModels.Region> _RegionTable { get; set; }


        [Required(ErrorMessage = "Please Enter State Name")]
        public int? regionId { get; set; }
    }
}
