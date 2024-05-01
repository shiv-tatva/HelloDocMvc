using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HelloDocMVC.CustomeModel
{
    public class Custom
    {

        [RegularExpression(@"^\S.*$", ErrorMessage = "Symptoms cannot be null or whitespace.")]
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
        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }


        [Required(ErrorMessage = "Please Enter Your State")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }


        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]
        public string? zipcode { get; set; }

        public string? room { get; set; }

        public string? admin_note { get; set; }

        public IFormFile? upload { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      ErrorMessage = "8 characters long (one uppercase, one lowercase letter, one digit, and one special character.)")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        public string? confirmPassword { get; set; }

        public List<DAL_Data_Access_Layer_.DataModels.Region> _RegionTable { get; set; }

        [Required(ErrorMessage = "Please Enter Your State")]
        public int? regionId { get; set; }

    }
}
