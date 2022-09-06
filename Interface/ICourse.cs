
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface ICourse
    {
        IEnumerable<Course> GetAll();
        bool CourseExist(int id);
        bool Create(Course course);
        bool Update(Course course);
        bool Delete(Course course);
        bool Save();

        Course GetCourse(int id);
    }
}
