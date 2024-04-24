using System.ComponentModel.DataAnnotations;

namespace DataLayer.CustomeModel
{
    public class SchoolManagementCM
    {
        public List<DashboardCM> DashboardData { get; set; }
        public AddStudent AddStudent { get; set; }
    }

    public class DashboardCM
    {
        public int? StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Course { get; set; }
        public string? Grade { get; set; }
    }

    public class AddStudent
    {
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public string? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public string? Course { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        public int? Grade { get; set; }
    }
}
