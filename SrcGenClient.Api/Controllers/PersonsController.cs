using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrcGenClient.Api.Dto;
using SrcGenClient.Api.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SrcGenClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly SampleContext context;

        public PersonsController(SampleContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var personEntities = await this.context.Persons.ToListAsync();
            var personDtos = personEntities.Select(person => person.ToPersonDto());

            return this.Ok(personDtos);
        }
    }
}
