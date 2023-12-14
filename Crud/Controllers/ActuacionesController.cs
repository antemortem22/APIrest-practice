﻿using Crud.Domain.Request;
using Crud.Domain.Response;
using Crud.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Controllers
{
    //Definir controlador
    [ApiController]

    [Route("api/[controller]")]
    public class ActuacionesController : ControllerBase
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
            

            if (actor == null) return NotFound(new { Error = $"No se pudo encontrar el actor {nombre}." });

            var result = _context.Actores
                         .Where(w => w.IdActor == actor.IdActor)
                         .SelectMany(a => a.Actuaciones
                             .Select(actuacion => new
                             {
                                 a.IdActor,
                                 NombreActor = a.Nombre,
                                 actuacion.IdActuacion,
                                 actuacion.Papel,
                                 TituloPelicula = actuacion.IdPeliculaNavigation.Titulo
                             })
                         )
                         .ToList();


            if (result.Count == 0) return NotFound(new {Error = $"No se encontraron actuaciones para el actor {nombre}."});
            

            return Ok(result);
        }
    }
}
