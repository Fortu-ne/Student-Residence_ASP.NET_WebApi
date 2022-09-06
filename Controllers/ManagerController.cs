using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Authentication;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IManager _manager;
        private readonly IMapper _mapper;

        public ManagerController(AppDbContext db, IManager manager, IMapper mapper)
        {
            _db = db;
            _manager = manager;
            _mapper = mapper;
        }


        [HttpGet("{managerId}")]
        [ProducesResponseType(200, Type = typeof(Manager))]
        [ProducesResponseType(400)]

        // IAction method to retrieve the current course
        public IActionResult GetManager(string managerId)
        {
            if (!_manager.UserExists(managerId))
                return NotFound();


            var exist = _manager.GetUser(managerId);

            if(exist != null)
            {
                exist.Id = managerId; 
                    
            }

            var user = _mapper.Map<ManagerDto>(exist);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }


        [HttpPut("{managerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateManager(string managerId, ManagerDto updateUser)
        {
            if (updateUser == null)
                return BadRequest(ModelState);

            if (managerId != updateUser.ManagerId)
                return BadRequest(ModelState);

            if (!_manager.UserExists(managerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var exists = _manager.GetUser(managerId);

            if (exists != null)
            {
                exists.Name = updateUser.Name;
                exists.PhoneNumber = updateUser.PhoneNumber;
                exists.UserName = updateUser.UserName;
                exists.LastName = updateUser.LastName;
             
            }


            var studMap = _mapper.Map<Manager>(exists);



            if (!_manager.Update(studMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return Ok("The Manager was succesfully updated");
        }


    }
}
