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
    [Route("api/[controller]")]
    [ApiController]
    public class CleaningController : Controller
    {
        private readonly ICleaning _dataRep;
        private readonly IMapper _mapper;
        private readonly IStudent _studRep;
        private readonly IConfiguration _config;

        public CleaningController(ICleaning dataRep, IMapper mapper,IStudent studRep, IConfiguration config)
        {
            _dataRep = dataRep;
            _mapper = mapper;
            _studRep = studRep;
            _config = config;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Service Management")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CleaningService>))]
        [ProducesResponseType(400)]
        public IActionResult GetAll()
        {
            var model = _mapper.Map<List<CleaningServiceDto>>(_dataRep.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);


        }


        [HttpGet("CleaningType")]
        [Authorize(Roles = "Admin,Service Management,User")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CleaningType>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllTypes()
        {
            
            var model = _mapper.Map<List<CleaningTypeDto>>(_dataRep.GetAllTypes());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);


        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles="User")]
        public IActionResult Create(/*[FromQuery] string studId, [FromQuery] int typeId, */[FromBody] CleaningServiceDto service)
        {
            if (service == null)
                return BadRequest(ModelState);

           // var model = _dataRep.GetAll().Where(c => c.Description.Trim().ToUpper() == service.Description.TrimEnd().ToUpper()).FirstOrDefault();

            //if (model != null)
            //{
            //    ModelState.AddModelError("", "The request already exists");
            //    return StatusCode(442, ModelState);
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            service.ServiceDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'hh:mm tt");

            var newCleaning = new CleaningService()
            {               
                CleaningTypeId = service.TypeId,
                Description = service.Description,
                StudId = service.StudId,
                CleaningDate = service.ServiceDate,
        };

            var cleaningMap = _mapper.Map<CleaningService>(newCleaning);



            if (!_dataRep.Create(/*typeId, studId,*/ cleaningMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            var email = new MimeMessage();// Mail entity
            using var smtp = new SmtpClient(); // using mailkit

            var user = _studRep.GetStudent(service.StudId);
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Cleaning Order ";


            if (service.TypeId != null)
            {
                if (service.TypeId > 0)
                {

                    email.Body = new TextPart(TextFormat.Html) { Text = "Goody Day " + user.Name + "<br>" + "Your cleaning order has been accepted, contact service management for further enquires" + "<br>" + "<br>" + " – Kind Regards" + "<br>" + "The RAS team" };

                }
                else
                {
                    email.Body = new TextPart(TextFormat.Html) { Text = "Goody Day " + user.Name + "<br>" + "Your cleaning order has been rejected due to lack of information, contact service management for further enquires" + "<br>" + "<br>" + " – Kind Regards" + "<br>" + "The RAS team" };

                }
            }




            smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);

            return Ok("Successfully Created");
        }


        [HttpGet("Get/{cId}")]
        [ProducesResponseType(200, Type = typeof(CleaningService))]
        [ProducesResponseType(400)]
        public IActionResult GetCleaning(int cId)
        {
            if (!_dataRep.CLeaningExist(cId))
                return NotFound();

            var model = _mapper.Map<CleaningServiceDto>(_dataRep.GetCleaningService(cId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(model);
        }

        [HttpPut("Update/{cleaningId}")]
        [Authorize(Roles = "Admin,Service Management")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCleaning(int cleaningId,/*, [FromQuery] int typeId, [FromQuery] string studId,*/ [FromBody] CleaningServiceDto updateModel)
        {
            if (cleaningId < 0)
                return BadRequest(ModelState);

            if (cleaningId != updateModel.CleaningId)
                return BadRequest(ModelState);

            if (!_dataRep.CLeaningExist(cleaningId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var existing = _dataRep.GetCleaningService(cleaningId);

           

            if (existing != null)
            {
                existing.CleaningId = updateModel.CleaningId;
                existing.StudId = updateModel.StudId;
               
            }
            

            var dataRep = _mapper.Map<CleaningService>(existing);

            if (!_dataRep.Update(/*typeId, studId,*/ dataRep))
            {
                ModelState.AddModelError("", "Something went wrong when updating");
                return StatusCode(500, ModelState);
            }


            var email = new MimeMessage();// Mail entity
            using var smtp = new SmtpClient(); // using mailkit

            var user = _studRep.GetStudent(updateModel.StudId);
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Cleaning Order ";


            if(updateModel.TypeId != null)
            {
                if (updateModel.TypeId > 0)
                {

                    email.Body = new TextPart(TextFormat.Html) { Text = "Goody Day " + user.Name + "<br>" + "Your cleaning order has been accepted, contact service management for further enquires" + "<br>" + "<br>" + " – Kind Regards" + "<br>" + "The RAS team" };

                }
                else
                {
                    email.Body = new TextPart(TextFormat.Html) { Text = "Goody Day " + user.Name + "<br>" + "Your cleaning order has been rejected due to lack of information, contact service management for further enquires" + "<br>" + "<br>" + " – Kind Regards" + "<br>" + "The RAS team" };

                }
            }




            smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);

            return Ok("Successfully updated");
        }

        [HttpDelete("Delete/{cleaningId}")]
        [Authorize(Roles = "Admin,Service Management")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCleaning(int cleaningId)
        {
            if (!_dataRep.CLeaningExist(cleaningId))
                return NotFound();

            var model = _dataRep.GetCleaningService(cleaningId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_dataRep.Delete(model))
            {
                ModelState.AddModelError("", "Something went wrong");

            }

            return NoContent();
        }

    }
}
