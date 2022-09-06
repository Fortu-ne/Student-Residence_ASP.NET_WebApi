using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : Controller
    {
        private readonly IGender _genderRep;
        private readonly IMapper _mapper;

        public GenderController(IGender genderRep, IMapper mapper)
        {
            _genderRep = genderRep;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Gender>))]
        [ProducesResponseType(400)]
        // IAction method to retrieve all genders
        public IActionResult GetGender()
        {
            var model = _mapper.Map<List<GenderDto>>(_genderRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }


        [HttpGet("{genderId}")]
        [ProducesResponseType(200, Type = typeof(Gender))]
        [ProducesResponseType(400)]

        // IAction method to retrieve the current gender
        public IActionResult GetCourse(int genderId)
        {
            if (!_genderRep.GenderExist(genderId))
                return NotFound();

            var model = _mapper.Map<GenderDto>(_genderRep.GetGender(genderId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        // IAction method to create a gender
        public IActionResult CreateGender([FromBody] GenderDto genderModel)
        {
            if (genderModel == null)
                return BadRequest(ModelState);

            var model = _genderRep.GetAll().Where(c => c.GenderType.Trim().ToUpper() == genderModel.GenderType.TrimEnd().ToUpper()).FirstOrDefault();

            if (model != null)
            {
                ModelState.AddModelError("", "The course code already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genderMap = _mapper.Map<Gender>(genderModel);



            if (!_genderRep.Create(genderMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }


        [HttpPut("{genderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGender(int genderId,
          GenderDto updateGender)
        {
            if (updateGender == null)
                return BadRequest(ModelState);

            if (genderId != updateGender.GenderId)
                return BadRequest(ModelState);

            if (!_genderRep.GenderExist(genderId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var genderMap = _mapper.Map<Gender>(updateGender);

            if (!_genderRep.Update(genderMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{genderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        // IAction method to delete a gender
        public IActionResult DeleteGender(int genderId)
        {
            if (!_genderRep.GenderExist(genderId))
                return NotFound();

            var model = _genderRep.GetGender(genderId); ;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_genderRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }
    }

}
