using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public class StudentRep : IStudent
    {
        private readonly AppDbContext _db;

        public StudentRep(AppDbContext db)
        {
            _db = db;
        }

        public bool Create(Student student)
        {
        
            _db.Students.Add(student);
            return Save();
        }

        public bool Delete(Student student)
        {
            var model = GetStudent(student.Id);
            if (model != null)
            {
                _db.Students.Remove(model);
                return Save();
            }
            return !Save();
        }

        public IEnumerable<Student> GetAll()
        {
            var query = _db.Students.Include(c => c.Course).Include(r => r.Gender).Include(r => r.Address).Include(r => r.Room).ToList();
            return query;
        }

       

        public Student GetStudent(string studId)
        {
            return _db.Students.FirstOrDefault(r => r.Id == studId);
        }

        public Student GetStudentByEmail(string email)
        {
            return _db.Students.FirstOrDefault(r => r.Email == email);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool StudentExist(string studId)
        {
            return _db.Students.Any(r => r.Id == studId);
        }

        public bool StudentExistByEmail(string email)
        {
            return _db.Students.Any(r => r.Email == email);
        }

        public bool Update(Student student)
        {
            //student.CourseId = courseId;
            //student.GenderId = genderId;
            //student.RoomId = roomId;
            _db.Students.Update(student);
           
            return Save();
        }
    }
}
