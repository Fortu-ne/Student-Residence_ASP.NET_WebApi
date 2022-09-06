using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStatus _statusRep;

        public StatusController(IMapper mapper, IStatus statusRep)
        {
            _mapper = mapper;
            _statusRep = statusRep;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StatusType>))]
        [ProducesResponseType(400)]
        public IActionResult GetStatuses()
        {
            var model = _mapper.Map<List<StatusTypeDto>>(_statusRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }



        [HttpGet("{cId}")]
        [ProducesResponseType(200, Type = typeof(StatusType))]
        [ProducesResponseType(400)]
        public IActionResult GetStauts(int cId)
        {
            if (!_statusRep.StatusExist(cId))
                return NotFound();

            var statusModel = _mapper.Map<StatusTypeDto>(_statusRep.GetStatus(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(statusModel);
        }




        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateStatus([FromBody] StatusTypeDto model)
        {
            if (model == null)
                return BadRequest(ModelState);

            var statusModel = _statusRep.GetAll().Where(c => c.Name.Trim().ToUpper() == model.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (statusModel != null)
            {
                ModelState.AddModelError("", "The room already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var statusMap = _mapper.Map<StatusType>(model);



            if (!_statusRep.Create(statusMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        [HttpPut("{statusId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStatus(int statusId, [FromBody] StatusTypeDto updateModel)
        {
            if (statusId > 0)
                return BadRequest(ModelState);

            if (statusId != updateModel.StatusId)
                return BadRequest(ModelState);

            if (!_statusRep.StatusExist(statusId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var statusMap = _mapper.Map<StatusType>(updateModel);

            if (!_statusRep.Update(statusMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{statusId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStatus(int statusId)
        {
            if (!_statusRep.StatusExist(statusId))
                return NotFound();

            var model = _statusRep.GetStatus(statusId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_statusRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}

