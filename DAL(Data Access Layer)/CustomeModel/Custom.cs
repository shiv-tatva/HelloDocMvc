using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDocMVC.CustomeModel
{
    public class Custom
    { 
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Date Of Birth")]
        public string? dateofbirth { get; set; }


        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }


        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? phone { get; set; }


        [Required(ErrorMessage = "Please Enter Your Street")]
        public string? street { get; set; }


        [Required(ErrorMessage = "Please Enter Your City")]
        public string? city { get; set; }


        [Required(ErrorMessage = "Please Enter Your State")]
        public string? state { get; set; }


        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]
        public string? zipcode { get; set; }

        public string? room { get; set; }

        public IFormFile? upload { get; set; }


        [StringLength(15, MinimumLength = 4, ErrorMessage = "Password Have 4 to 15 Char")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        public string? confirmPassword { get; set; }

        public List<DAL_Data_Access_Layer_.DataModels.Region> _RegionTable { get; set; }

        public int? regionId {  get; set; }

    }
}
