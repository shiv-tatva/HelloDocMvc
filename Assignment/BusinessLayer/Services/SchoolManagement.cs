using BusinessLayer.Interface;
using DataLayer.CustomeModel;
using DataLayer.DataContext;
using DataLayer.DataModels;

namespace BusinessLayer.Services
{
    public class SchoolManagement : ISchoolManagement
    {
        private readonly ApplicationDbContext _context;

        public SchoolManagement(ApplicationDbContext context)
        {
            _context = context;
        }

        #region GetDashboard
        public SchoolManagementCM GetDashboard()
        {
            SchoolManagementCM schoolManagement = new SchoolManagementCM();

            schoolManagement.DashboardData = (from r in _context.Courses
                         join rc in _context.Students on r.Id equals rc.Courseid
                         select new DashboardCM
                         {
                             StudentID = rc.Id,
                             FirstName = rc.Firstname,
                             LastName = rc.Lastname,
                             Email = rc.Email,
                             Age = rc.Age,
                             Gender = rc.Gender,
                             Course = r.Name,
                             Grade = rc.Grade,
                         }).ToList();            

            return schoolManagement;
        }
        #endregion

        #region AddStudent
        public void AddStudent(SchoolManagementCM schoolManagementCM, List<int> exampleRadios)
        {

            if(_context.Courses.Any(r => r.Name.ToLower() == schoolManagementCM.AddStudent.Course.Trim().ToLower())){

            }
            else
            {
                Course course = new Course()
                {
                    Name = schoolManagementCM.AddStudent.Course,
                };
                _context.Courses.Add(course);
                _context.SaveChanges();
            }

            Student student = new Student()
            {
                Firstname = schoolManagementCM.AddStudent.FirstName,
                Lastname = schoolManagementCM.AddStudent.LastName,
                Courseid = _context.Courses.Where(r => r.Name.ToLower() == schoolManagementCM.AddStudent.Course.Trim().ToLower()).Select(r => r.Id).First(),
                Age = DateTime.Now.Year - Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(0, 4)),
                Email = schoolManagementCM.AddStudent.Email,
                Gender = (exampleRadios[0] == 1) ? "Male" : (exampleRadios[0] == 2) ? "Female" : "Other",
                Coursename = schoolManagementCM.AddStudent.Course,
                Grade = (schoolManagementCM.AddStudent.Grade == 1) ? "1st-3rd" : (schoolManagementCM.AddStudent.Grade == 2) ? "4th-6th" : (schoolManagementCM.AddStudent.Grade == 3) ? "7th-9th" : "10th-12th",
                Strmonth = schoolManagementCM.AddStudent.DateOfBirth.Substring(5, 2),
                Intdate = Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(8, 2)),
                Intyear = Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(0, 4)),
            };

            _context.Students.Add(student);
            _context.SaveChanges();
        }
        #endregion

        #region GetEditPop With User Details
        public SchoolManagementCM GetStudentDetails(int studentId)
        {
            SchoolManagementCM schoolManagement = new SchoolManagementCM();

            schoolManagement.DashboardData = (from r in _context.Courses
                                              join rc in _context.Students on r.Id equals rc.Courseid
                                              select new DashboardCM
                                              {
                                                  StudentID = rc.Id,
                                                  FirstName = rc.Firstname,
                                                  LastName = rc.Lastname,
                                                  Email = rc.Email,
                                                  Age = rc.Age,
                                                  Gender = rc.Gender,
                                                  Course = r.Name,
                                                  Grade = rc.Grade,
                                                  DateOfBirth = new DateTime(Convert.ToInt16(rc.Intyear), Convert.ToInt16(rc.Strmonth), Convert.ToInt16(rc.Intdate)).ToString("yyyy-MM-dd"),
                                              }).ToList();

            schoolManagement.DashboardData = schoolManagement.DashboardData.Where(r => r.StudentID ==  studentId).ToList();

            return schoolManagement;
        }
        #endregion

        #region EditStudentDetails
        public void EditStudentDetails(SchoolManagementCM schoolManagementCM, List<int> exampleRadios)
        {
            if (_context.Courses.Any(r => r.Name.ToLower() == schoolManagementCM.AddStudent.Course.Trim().ToLower()))
            {

            }
            else
            {
                Course course = new Course()
                {
                    Name = schoolManagementCM.AddStudent.Course,
                };
                _context.Courses.Add(course);
                _context.SaveChanges();
            }

            var editStudent = _context.Students.Where(r => r.Id == schoolManagementCM.AddStudent.StudentID).Select(r => r).First();

            editStudent.Firstname = schoolManagementCM.AddStudent.FirstName;
            editStudent.Lastname = schoolManagementCM.AddStudent.LastName;
            editStudent.Courseid = _context.Courses.Where(r => r.Name.ToLower() == schoolManagementCM.AddStudent.Course.Trim().ToLower()).Select(r => r.Id).First();
            editStudent.Age = DateTime.Now.Year - Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(0, 4));
            editStudent.Email = schoolManagementCM.AddStudent.Email;
            editStudent.Gender = (exampleRadios[0] == 1) ? "Male" : (exampleRadios[0] == 2) ? "Female" : "Other";
            editStudent.Coursename = schoolManagementCM.AddStudent.Course;
            editStudent.Grade = (schoolManagementCM.AddStudent.Grade == 1) ? "1st-3rd" : (schoolManagementCM.AddStudent.Grade == 2) ? "4th-6th" : (schoolManagementCM.AddStudent.Grade == 3) ? "7th-9th" : "10th-12th";
            editStudent.Strmonth = schoolManagementCM.AddStudent.DateOfBirth.Substring(5, 2);
            editStudent.Intdate = Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(8, 2));
            editStudent.Intyear = Convert.ToInt16(schoolManagementCM.AddStudent.DateOfBirth.Substring(0, 4));

            _context.SaveChanges();
        }
        #endregion

        #region DeletePopup
        public AddStudent DeletePopup(int studentId)
        {
            AddStudent addStudent = new AddStudent()
            {
                StudentID = studentId,
            };

            return addStudent;
        }
        #endregion

        #region Delete Student Details
        public void DeleteConfirm(AddStudent addStudent)
        {
            var deleteStudentDetails = _context.Students.Where(x => x.Id == addStudent.StudentID).Select(x => x).First();

            _context.Students.Remove(deleteStudentDetails);
            _context.SaveChanges();
        }
        #endregion       
    }
}
