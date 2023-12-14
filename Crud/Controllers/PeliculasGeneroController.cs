using Crud.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class PeliculasGeneroController : ControllerBase
    {
        private readonly IMDBContext _context; //Inyeccion de dependencia 
        public PeliculasGeneroController(IMDBContext context)
        {
            _context = context;
        }
        [HttpGet("{genero}")]
        public IActionResult GetPeliculasByGenero([FromRoute] string genero) 
        {
            var gen = _context.Generos.FirstOrDefault(g => g.Nombre == genero);
            if (gen == null) return NotFound(new { Error = $"No se pudo econtrar el genero {gen}" });

            var result = _context.Generos
                        .Where(w => w.IdGenero == gen.IdGenero)
                        .SelectMany(a => a.PeliculasGeneros
                            .Select(peli => new
                            {
                                a.IdGenero,
                                NombreGenero = a.Nombre,
                                TituloPelicula = peli.IdPeliculaNavigation.Titulo
                            })
                        )
                        .ToList();

            return Ok(result);

            
        }
    }
}
