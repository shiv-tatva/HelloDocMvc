using DataLayer.CustomeModel;

namespace BusinessLayer.Interface
{
    public interface ISchoolManagement
    {
        public SchoolManagementCM GetDashboard();
        public void AddStudent(SchoolManagementCM schoolManagementCM, List<int> exampleRadios);
        public SchoolManagementCM GetStudentDetails(int studentId);
        public void EditStudentDetails(SchoolManagementCM schoolManagementCM, List<int> exampleRadios);
        public AddStudent DeletePopup(int studentId);
        public void DeleteConfirm(AddStudent addStudent);

    }
}
