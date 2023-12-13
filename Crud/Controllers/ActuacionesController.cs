using Crud.Domain.Request;
using Crud.Domain.Response;
using Crud.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Controllers
{
    //Definir controlador
    [ApiController]

    [Route("api/[controller]")]
    public class ActuacionesController :ControllerBase
    {
        private readonly IMDBContext _context; //Inyeccion de dependencia 
        public ActuacionesController(IMDBContext context)
        {
            _context = context;
        }

        [HttpGet("{nombre}")]

        public IActionResult GetActuacionesByName([FromRoute] string nombre)
        {
            var actor = _context.Actores.FirstOrDefault(a => a.Nombre == nombre);
            var count = _context.Actores.Count();

            if (actor == null) return NotFound(new { Error = $"No se pudo encontrar el actor {nombre}." });




            var papeles = _context.Actuaciones
                                  .Where(actuacion => actuacion.IdActor == actor.IdActor)
                                  .Select(actuacion => actuacion.Papel)
                                  .ToList();

            if (papeles.Count == 0) return NotFound(new {Error = $"No se encontraron actuaciones para el actor {nombre}."});
            

            return Ok(papeles);
        }
    }
}
