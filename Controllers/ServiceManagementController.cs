using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Authentication;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.Model.Users;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceManagementController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IServiceManager _servManager;
        private readonly IMapper _mapper;

        public ServiceManagementController(AppDbContext db, IServiceManager servManager, IMapper mapper)
        {
            _db = db;
            _servManager = servManager;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceManagement>))]
        [ProducesResponseType(400)]
        // IAction method to retrieve all students
        public IActionResult GetAllManagers()
        {
            var model = _mapper.Map<List<ManagerDto>>(_servManager.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }

        [HttpGet("{servicMgrId}")]
        [ProducesResponseType(200, Type = typeof(ServiceManagement))]
        [ProducesResponseType(400)]

        // IAction method to retrieve the current course
        public IActionResult GetServiceManager(string servicMgrId)
        {
            if (!_servManager.UserExists(servicMgrId))
                return NotFound();

            var user = _mapper.Map<ServiceManagerDto>(_servManager.GetUser(servicMgrId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }


        [HttpPut("{servicMgrId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateManager(string servicMgrId, ManagerDto updateUser)
        {
            if (updateUser == null)
                return BadRequest(ModelState);

            if (servicMgrId != updateUser.ManagerId)
                return BadRequest(ModelState);

            if (!_servManager.UserExists(servicMgrId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var exists = _servManager.GetUser(servicMgrId);

            if (exists != null)
            {
                exists.Name = updateUser.Name;
                exists.PhoneNumber = updateUser.PhoneNumber;
                exists.UserName = updateUser.UserName;
                exists.LastName = updateUser.LastName;

            }


            var modelMap = _mapper.Map<ServiceManagement>(exists);



            if (!_servManager.Update(modelMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return Ok("The Manager was succesfully updated");
        }


        [HttpDelete("Delete/{servicMgrId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        // IAction method to delete a service manager
        public IActionResult DeleteManager(string servicMgrId)
        {
            if (!_servManager.UserExists(servicMgrId))
                return NotFound();

            var model = _servManager.GetUser(servicMgrId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_servManager.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}
