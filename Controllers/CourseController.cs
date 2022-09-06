using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace JulyProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : Controller
{
    private readonly ICourse _courseRep;
    private readonly IMapper _mapper;

    public CourseController(ICourse courseRep, IMapper mapper)
    {
        _courseRep = courseRep;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]
    [ProducesResponseType(400)]
    // IAction method to retrieve all students
    public IActionResult GetCourse()
    {
        var model = _mapper.Map<List<CourseTypeDto>>(_courseRep.GetAll());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(model);

    }

    [HttpGet("{courseId}")]
    [ProducesResponseType(200, Type = typeof(Course))]
    [ProducesResponseType(400)]

    // IAction method to retrieve the current course
    public IActionResult GetCourse(int courseId)
    {
        if (!_courseRep.CourseExist(courseId))
            return NotFound();

        var course = _mapper.Map<CourseTypeDto>(_courseRep.GetCourse(courseId));


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(course);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    // IAction method to create a course
    public IActionResult CreateCourse( [FromBody] CourseTypeDto courseModel)
    {
        if (courseModel == null)
            return BadRequest(ModelState);

        var model = _courseRep.GetAll().Where(c => c.CourseName.Trim().ToUpper() == courseModel.CourseName.TrimEnd().ToUpper()).FirstOrDefault();

        if (model != null)
        {
            ModelState.AddModelError("", "The course code already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courseMap = _mapper.Map<Course>(courseModel);



        if (!_courseRep.Create(courseMap))
        {
            ModelState.AddModelError("", "Something went wrong");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Created");
    }


    [HttpPut("{courseId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCourse(int courseId,
      CourseTypeDto updateCourse)
    {
        if (updateCourse == null)
            return BadRequest(ModelState);

        if (courseId != updateCourse.CourseTypeId)
            return BadRequest(ModelState);

        if (!_courseRep.CourseExist(courseId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var courseMap = _mapper.Map<Course>(updateCourse);

        if (!_courseRep.Update(courseMap))
        {
            ModelState.AddModelError("", "Something went wrong when updating");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }


    [HttpDelete("{courseId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    // IAction method to delete a course
    public IActionResult DeleteCourse(int courseId)
    {
        if (!_courseRep.CourseExist(courseId)) 
            return NotFound();

        var model = _courseRep.GetCourse(courseId); ;

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_courseRep.Delete(model))
        {
            ModelState.AddModelError("", "Something went wrong");

        }

        return NoContent();
    }
}
