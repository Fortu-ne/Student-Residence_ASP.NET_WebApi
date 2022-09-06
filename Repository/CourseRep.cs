

using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace JulyProject.Repositroy
{
    public class CourseRep : ICourse
    {
        private readonly AppDbContext _db;

        public CourseRep(AppDbContext db)
        {
           _db = db;
        }

        public bool CourseExist(int id)
        {
            return _db.Courses.Any(r => r.CourseTypeId == id);
        }

        public bool Create(Course course)
        {
            _db.Courses.Add(course);
            return Save();
        }

        public bool Delete(Course course)
        {
            var model = GetCourse(course.CourseTypeId);

            if (model != null)
            {
                _db.Remove(model);
            }

            return Save();
        }

        public IEnumerable<Course> GetAll()
        {
            var query = _db.Courses.ToList();

            return query;
        }

        public Course GetCourse(int id)
        {
            return _db.Courses.FirstOrDefault(r => r.CourseTypeId == id);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0? true: false;  
        }

        public bool Update(Course course)
        {
            _db.Update(course);
            return Save();
        }
    }
}
