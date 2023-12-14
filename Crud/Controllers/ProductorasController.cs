using Crud.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class ProductorasController : ControllerBase
    {
        private readonly IMDBContext _context; //Inyeccion de dependencia 
        public ProductorasController(IMDBContext context)
        {
            _context = context;
        }

        [HttpGet("{productora}")]
        public IActionResult GetPeliculasByProductoras([FromRoute] string productora)
        {
            var produ = _context.Productoras.FirstOrDefault(p => p.Nombre == productora);
            if (produ == null) return NotFound(new { Error = $"No se pudo encontrar la productora {produ}" });

            var result = _context.Productoras
                                   .Where(w => w.IdProductora == produ.IdProductora)
                                   .Select(productora => new
                                   {
                                       productora.IdProductora,
                                       productora.Nombre,
                                       RecaudacionTotal = productora.Peliculas.Sum(pelicula => pelicula.Recaudacion),
                                       Peliculas = productora.Peliculas.Select(pelicula => new
                                       {
                                           pelicula.IdPelicula,
                                           pelicula.Titulo,
                                           pelicula.Recaudacion
                                       })
                                   })
                                   .ToList();

            return Ok(result);
        }
    }
}
