using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public interface IStudent
    {
        IEnumerable<Student> GetAll();
        bool StudentExist(string studId);
        bool StudentExistByEmail(string email);
        bool Create(Student student);
        bool Update(Student student);
        bool Delete(Student student);
        bool Save();

        Student GetStudent(string studId);
        Student GetStudentByEmail(string email);

    }


}
