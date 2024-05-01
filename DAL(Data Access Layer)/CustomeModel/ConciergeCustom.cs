using System.ComponentModel.DataAnnotations;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class ConciergeCustom
    {
        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? concierge_firstname { get; set; }


        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? concierge_lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? concierge_phone { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? concierge_email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Hotel Name")]
        public string? concierge_hotelname { get; set; }


        [Required(ErrorMessage = "Please Enter Street Name:")]
        public string? concierge_street { get; set; }

        [Required(ErrorMessage = "Please Enter City Name:")]
        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? concierge_city { get; set; }

        [Required(ErrorMessage = "Please Enter State Name:")]
        public string? concierge_state { get; set; }

        [Required(ErrorMessage = "Please Enter Zipcode Name:")]
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]
        public string? concierge_zipcode { get; set; }

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

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? phone { get; set; }

        public string? room { get; set; }

        public List<DAL_Data_Access_Layer_.DataModels.Region> _RegionTable { get; set; }


        [Required(ErrorMessage = "Please Enter State Name")]
        public int? regionId { get; set; }
    }
}
