using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddress _addressRep;
        private readonly IMapper _mapper;

        public AddressController(IAddress addressRep, IMapper mapper)
        {
            _addressRep = addressRep;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Address>))]
        [ProducesResponseType(400)]
        public IActionResult GetAddresses()
        {
            var model = _mapper.Map<List<AddressDto>>(_addressRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);

        }

        [HttpGet("{cId}")]
        [ProducesResponseType(200, Type = typeof(Address))]
        [ProducesResponseType(400)]
        public IActionResult GetAddress(int cId)
        {
            if (!_addressRep.AddressExist(cId))
                return NotFound();

            var student = _mapper.Map<AddressDto>(_addressRep.GetAddress(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(student);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateAddress([FromBody] AddressDto addressModel)
        {
            if (addressModel == null)
                return BadRequest(ModelState);

            var address = _addressRep.GetAll().Where(c => c.City.Trim().ToUpper() == addressModel.City.TrimEnd().ToUpper()).FirstOrDefault();

            if (address != null)
            {
                ModelState.AddModelError("", "The address already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addressMap = _mapper.Map<Address>(addressModel);

            //studentMap.Addresses = _addressRep.GetAddress(addressId);

            if (!_addressRep.Create(/*statusId,*/addressMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        [HttpPut("{addressId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAddress(/*[FromQuery]int studId*/ int addressId, [FromBody] AddressDto updateAdress)
        {
            if (updateAdress == null)
                return BadRequest(ModelState);

            if (addressId != updateAdress.AddressId)
                return BadRequest(ModelState);

            if (!_addressRep.AddressExist(addressId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var addressMap = _mapper.Map<Address>(updateAdress);

            if (!_addressRep.Update(/*studId,*/addressMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{addressId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAddress(int addressId)
        {
            if (!_addressRep.AddressExist(addressId))
                return NotFound();

            var model = _addressRep.GetAddress(addressId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_addressRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}

