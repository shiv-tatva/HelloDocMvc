using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDocMVC.CustomeModel
{
    public class Custom
    { 
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? firstname { get; set; }

        public string? lastname { get; set; }

        public string? dateofbirth { get; set; }


        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        public string? phone { get; set; }

        public string? street { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }

        public string? zipcode { get; set; }

        public string? room { get; set; }


        [StringLength(15,MinimumLength = 4 , ErrorMessage = "Password Have 4 to 15 Char")]
        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }

    }
}
