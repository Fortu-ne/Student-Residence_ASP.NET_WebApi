using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class RoomController : Controller
    {
        private readonly IRooms _roomRep;
        private readonly IMapper _mapper;

        public RoomController(IRooms roomRep, IMapper mapper)
        {
            _roomRep = roomRep;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Room>))]
        [ProducesResponseType(400)]
        public IActionResult GetRooms()
        {
            var model = _mapper.Map<List<RoomDto>>(_roomRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }



        [HttpGet("{cId}")]
        [ProducesResponseType(200, Type = typeof(Room))]
        [ProducesResponseType(400)]
        public IActionResult GetRoom(int cId)
        {
            if (!_roomRep.RoomExist(cId))
                return NotFound();

            var roomModel = _mapper.Map<RoomDto>(_roomRep.GetRoom(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(roomModel);
        }




        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateRoom([FromQuery] int genderId, [FromBody] RoomDto model)
        {
            if (model == null)
                return BadRequest(ModelState);

            var room = _roomRep.GetAll().Where(c => c.RoomNum.Trim().ToUpper() == model.RoomNum.TrimEnd().ToUpper()).FirstOrDefault();

            if (room != null)
            {
                ModelState.AddModelError("", "The room already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roomMap = _mapper.Map<Room>(model);



            if (!_roomRep.Create(genderId, roomMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        [HttpPut("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoom(int roomId, [FromQuery] int genderId, [FromBody] RoomDto updateModel)
        {
            if (roomId == 0)
                return BadRequest(ModelState);

            if (roomId != updateModel.RoomId)
                return BadRequest(ModelState);

            if (!_roomRep.RoomExist(roomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var roomMap = _mapper.Map<Room>(updateModel);

            if (!_roomRep.Update(genderId, roomMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(int roomId)
        {
            if (!_roomRep.RoomExist(roomId))
                return NotFound();

            var model = _roomRep.GetRoom(roomId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_roomRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}
