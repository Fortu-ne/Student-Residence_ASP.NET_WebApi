using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceTypeController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IMaintenancetype _maintenanceTypeRep;

        public MaintenanceTypeController(IMapper mapper, IMaintenancetype maintenanceTypeRep)
        {
            _mapper = mapper;
            _maintenanceTypeRep = maintenanceTypeRep;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaintenanceType>))]
        [ProducesResponseType(400)]
        public IActionResult GetMaintenanceTypes()
        {
            var model = _mapper.Map<List<MaintenanceTypeDto>>(_maintenanceTypeRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }



        [HttpGet("{cId}")]
        [ProducesResponseType(200, Type = typeof(MaintenanceType))]
        [ProducesResponseType(400)]
        public IActionResult GetMaintenanceType(int cId)
        {
            if (!_maintenanceTypeRep.TypeExist(cId))
                return NotFound();

            var model = _mapper.Map<MaintenanceTypeDto>(_maintenanceTypeRep.GetType(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);
        }




        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateMaintenanceType([FromBody] MaintenanceTypeDto model)
        {
            if (model == null)
                return BadRequest(ModelState);

            var type = _maintenanceTypeRep.GetAll().Where(c => c.Name.Trim().ToUpper() == model.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (type != null)
            {
                ModelState.AddModelError("", "The room already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mapper = _mapper.Map<MaintenanceType>(model);



            if (!_maintenanceTypeRep.Create(mapper))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        [HttpPut("{typeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMaintenanceType(int typeId, [FromBody] MaintenanceTypeDto updateModel)
        {
            if (typeId > 0)
                return BadRequest(ModelState);

            if (typeId != updateModel.MaintenanceTypeId)
                return BadRequest(ModelState);

            if (!_maintenanceTypeRep.TypeExist(typeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var mapper = _mapper.Map<MaintenanceType>(updateModel);

            if (!_maintenanceTypeRep.Update(mapper))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{typeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMaintenanceType(int typeId)
        {
            if (!_maintenanceTypeRep.TypeExist(typeId))
                return NotFound();

            var model = _maintenanceTypeRep.GetType(typeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_maintenanceTypeRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}
