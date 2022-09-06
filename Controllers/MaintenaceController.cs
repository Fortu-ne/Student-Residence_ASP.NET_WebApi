using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using WepApiWithToken.Authentication;
using WepApiWithToken.Interface;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : Controller
    {
        private readonly IMaintenance _maintenaceRep;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IStudent _studRep;

        public MaintenanceController(IMaintenance maintenaceRep, IMapper mapper, IConfiguration config, IStudent studRep )
        {
            _maintenaceRep = maintenaceRep;
            _mapper = mapper;
            _config = config;
            _studRep = studRep;
        }


        [Authorize(Roles = "Admin,Service Management")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Maintenance>))]
        [ProducesResponseType(400)]
        public IActionResult GetAll()
        {
            var list = _maintenaceRep.GetAll();

            var model = _mapper.Map<List<MaintenanceDto>>(list);

            if (list == null)
            { return BadRequest("Can't return all data"); }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);


        }

        [HttpPost("Create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles="User")]

        public IActionResult Create(/*[FromQuery] string studId, [FromQuery] int typeId,*/ [FromBody] MaintenanceDto model)
        {
           
            if (model == null)
                return BadRequest(ModelState);

            // var maintenance = _maintenaceRep.GetAll().Where(c => c.Description.Trim().ToUpper() == model.Description.TrimEnd().ToUpper()).FirstOrDefault();
           // var maintenance = _maintenaceRep.GetAll().Where(c => c.MaintenanceId == model.MaintenanceId && c.MaintenanceTypeId == model.MaintenanceTypeId && c.Description == model.Description);

            //if (maintenance != null)
            //{
            //    ModelState.AddModelError("", "The room already exists");
            //    return StatusCode(442, ModelState);
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.MaintenanceDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'hh:mm tt");
            var newMaintenance = new Maintenance()
            {
                StudId = model.StudId,
                Description = model.Description,
                MaintenanceTypeId = model.MaintenanceTypeId,
                MaintenanceDate = model.MaintenanceDate,
                StatusId = 1,
            };
            var maintenanceMap = _mapper.Map<Maintenance>(newMaintenance);

            if (!_maintenaceRep.Create(/*typeId, studId,*/ maintenanceMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        
        [HttpGet("{cId}")]
        [ProducesResponseType(200, Type = typeof(Maintenance))]
        [ProducesResponseType(400)]
        public IActionResult GetMaintenace(int cId)
        {
            if (!_maintenaceRep.MaintenanceExist(cId))
                return NotFound();

            var roomModel = _mapper.Map<MaintenanceDto>(_maintenaceRep.GetMaintenance(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(roomModel);
        }

        [Authorize(Roles ="Admin,Service Management")]
        [HttpPut("{maintenanceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMaintenace(int maintenanceId/*, [FromQuery] int typeId, [FromQuery] string studId, [FromQuery] int statusId,*/, [FromBody] MaintenanceDto updateModel)
        {
            if (maintenanceId < 0)
                return BadRequest(ModelState);

            if (maintenanceId != updateModel.MaintenanceId)
                return BadRequest(ModelState);

            if (!_maintenaceRep.MaintenanceExist(maintenanceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var existing = _maintenaceRep.GetMaintenance(maintenanceId);

            if(existing != null) {
                existing.StatusId = updateModel.StatusId;
                existing.MaintenanceTypeId = updateModel.MaintenanceTypeId;
                existing.StudId = updateModel.StudId;
               // existing.MaintenanceDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'hh:mm tt");
            }

            var maintenanceRep = _mapper.Map<Maintenance>(existing);

            if (!_maintenaceRep.Update(/*typeId, studId, statusId,*/ maintenanceRep))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }


            var email = new MimeMessage();// Mail entity
            using var smtp = new SmtpClient(); // using mailkit
            var emailBody = "";

            var user = _studRep.GetStudent(updateModel.StudId);
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Maintenance feedback ";


            if (updateModel.StatusId != null)
            {
                if(updateModel.StatusId == 2)
                {
                    emailBody = "Good day " +  "<br>" + ",Your maintenance request has been approved and it will be handled " + "<br>" + "The date requested :" + maintenanceRep.MaintenanceDate + "<br>" + " –Kind Regards"+"<br>"+ "The RAS team" ;
                }
                else if(updateModel.StatusId == 3)
                {
                    emailBody  = "Goody Day " + "<br>" + "Your maintenance order has been declined, contact service management for further enquires" + "<br>" + "The time declined :"+ maintenanceRep.MaintenanceDate + "<br>" + " –Kind Regards" + "<br>" + "The RAS team" ;
                }
            }



            email.Body = new TextPart(TextFormat.Html) { Text = emailBody };
            smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok("The order has been successful");
        }

        [Authorize(Roles = "Admin,Service Management")]
        [HttpDelete("{maintenanceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMaintenance(int maintenanceId)
        {
            if (!_maintenaceRep.MaintenanceExist(maintenanceId))
                return NotFound();

            var model = _maintenaceRep.GetMaintenance(maintenanceId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_maintenaceRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}
