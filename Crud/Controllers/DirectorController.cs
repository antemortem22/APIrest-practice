using Crud.Domain.Request;
using Crud.Domain.Response;
using Crud.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Controllers
{
    //Definir controlador
    [ApiController]
    //Definir una ruta
        //La siguiente linea de codigo es un nombre por defecto y el "[controller]" es un comodin del framework
    [Route("api/[controller]")]// "api/Directotr" seria en este caso
    public class DirectorController : ControllerBase
    {
        private readonly IMDBContext _context; //Inyeccion de dependencia 
        public DirectorController(IMDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetDirectores([FromQuery] GetDirectoresRequest request) 
        {          
            //var result = _context.Directores.ToList(); // => esta opcion es == a Select All

            //Paginado => nos ayuda a que si tenemos un registro muy grande, no devolvamos todo de una y se optimice la API
            int skip = request.Skip;
            int take = request.Take;    

            var result = _context.Directores.Skip(skip).Take(take).ToList();
            int count = _context.Directores.Count();

            //objeto full declarado
            var response = new GetDirectoresResponse()
            {
                Directores = result,
                Total = count
            };

            return Ok(response);

            //return Ok(new {Datos = result, Count = count}); //=> objeto anonimo, no se suele usar
        }

        [HttpGet("{id}")]

        public IActionResult GetDirectorById([FromRoute] int id)
        {
            var result = _context.Directores.FirstOrDefault(w => w.IdDirector == id);

            if (result == null ) return NotFound(new {Error = $"No se pudo encontrar el id {id}"});

            return Ok(result);     
        }

        [HttpGet("peliculas")]
        public IActionResult GetPeliculasByDirector([FromQuery] int idDirector) 
        {
            var result = _context.Directores
                            .Where(w => w.IdDirector == idDirector)
                            .Include(i => i.Peliculas)
                            .ToList(); 
            

            return Ok(result);
        }


    }
}
