using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Authentication;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudent _studentRep;
        private readonly IMapper _mapper;
        private readonly IAddress _addressRep;
        private readonly AppDbContext _db;

        public StudentController(IStudent studentRep, IMapper mapper, IAddress addressRep, AppDbContext db)
        {
            _studentRep = studentRep;
            _mapper = mapper;
            _addressRep = addressRep;
            _db = db;
        }


        [HttpGet("List")/*,Authorize(Roles ="Admin")*/]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        [ProducesResponseType(400)]
        // IAction method to retrieve all students
        public IActionResult GetStudents()
        {
            var list = _studentRep.GetAll();

           
            var model = _mapper.Map<List<StudentDto>>(list);

            if(list == null)
            { return BadRequest("Can't return all data"); }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }

        [HttpGet("{studId}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(400)]

        // IAction method to retrieve the current student
        public IActionResult GetStudent(string studId)
        {
           
            if (!_studentRep.StudentExist(studId))
                return NotFound();

            var student = _mapper.Map<StudentDto>(_studentRep.GetStudent(studId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(student);
        }

        [HttpPost("SelectRoom/{studId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles ="User")]
        // IAction method to create a student
        public IActionResult CreateStudent(string studId,[FromBody] StudentDto studentModel)
        {

         

            //var student = _studentRep.GetAll().Where(c => c.Name.Trim().ToUpper() == studentModel.Name.TrimEnd().ToUpper()).FirstOrDefault();

            //if (student != null)
            //{
            //    ModelState.AddModelError("", "The student already exists");
            //    return StatusCode(442, ModelState);
            //}

          
            if (studId != studentModel.Id)
                return BadRequest(ModelState);

            if (!_studentRep.StudentExist(studId))
                return NotFound();

            //if (studentModel == null)
            //    return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var img = _photoService.AddPhotoAsync(studentModel.ImageUrl);

            var exist = _db.Students.FirstOrDefault(r => r.Id == studId);

            if (exist!= null)
            {
                exist.CourseId = studentModel.CourseId;
                exist.RoomId =   studentModel.RoomId;
                exist.Age = studentModel.Age;
                exist.BirthDate = studentModel.BirthDate;
                exist.GenderId = studentModel.GenderId;
                exist.Address = studentModel.Address;
            }

            var studMap = _mapper.Map<Student>(exist);

            //studentMap.ImageUrl = img.ToString();

            if (!_studentRep.Update(studMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully Created");
        }


        [HttpPut("{studId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudent(string studId, StudentDto updateStudent)
        {
            if (updateStudent == null)
                return BadRequest(ModelState);

            if (studId != updateStudent.Id)
                return BadRequest(ModelState);

            if (!_studentRep.StudentExist(studId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var exists = _db.Students.FirstOrDefault(r => r.Id == studId);

            if(exists != null)
            {
                exists.Name = updateStudent.Name;
                exists.PhoneNumber = updateStudent.PhoneNumber;
                exists.UserName = updateStudent.UserName;
                exists.LastName = updateStudent.LastName;
                exists.Address = updateStudent.Address;
            }

           
            var studMap = _mapper.Map<Student>(exists);
            
         

            if (!_studentRep.Update(studMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return Ok("The Student was succesfully updated");
        }


        [HttpDelete("{studId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        // IAction method to delete a student
        public IActionResult DeleteStudent(string studId)
        {
            if (!_studentRep.StudentExist(studId))
                return NotFound();

            var model = _studentRep.GetStudent(studId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_studentRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }
    }

}
