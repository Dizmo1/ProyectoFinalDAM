// Importa autorización y controladores HTTP
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Permite acceder a las Claims del usuario autenticado
using System.Security.Claims;

// Espacio de nombres del proyecto
using System;

// Importa el contexto de base de datos de tu aplicación
using BaloncestoAPI.Datos;

// Permite usar operaciones asincrónicas con Entity Framework
using Microsoft.EntityFrameworkCore;


namespace BaloncestoAPI.Controllers
{
    // Define este controlador como API REST
    [ApiController]

    // Ruta base para este controlador: api/partidas
    [Route("api/[controller]")]

    // Exige que el usuario esté autenticado para acceder a los endpoints
    [Authorize]
    public class PartidasController : ControllerBase
    {
        // Referencia al contexto de base de datos (inyección de dependencias)
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto y lo asigna
        public PartidasController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint: POST /api/partidas/nuevaPartida
        [HttpPost("nuevaPartida")]
        public async Task<IActionResult> NuevaPartida()
        {
            // Extrae el ID del jugador desde el token JWT
            var jugadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Crea una nueva partida asociada al jugador autenticado
            var partida = new Partida
            {
                JugadorId = jugadorId,
                Fecha = DateTime.UtcNow, // Hora de creación en UTC
                PuntosTotales = 0
            };

            // Agrega la partida a la base de datos
            _context.Partidas.Add(partida);
            await _context.SaveChangesAsync();

            // Devuelve el ID generado para la partida
            return Ok(new { partidaId = partida.Id });
        }

        // Endpoint: POST /api/partidas/registrarTiro
        [HttpPost("registrarTiro")]
        public async Task<IActionResult> RegistrarTiro([FromBody] Tiro req)
        {
            // Busca la partida correspondiente al tiro
            var partida = await _context.Partidas.FindAsync(req.PartidaId);
            if (partida == null)
                return NotFound("Partida no encontrada");

            // Crea un nuevo objeto Tiro con los datos recibidos
            var tiro = new Tiro
            {
                PartidaId = req.PartidaId,
                Acierto = req.Acierto,
                TiempoSegundos = req.TiempoSegundos,
                Distancia = req.Distancia
            };

            // Guarda el tiro
            _context.Tiros.Add(tiro);

            // Si el tiro fue acierto, suma puntos a la partida
            partida.PuntosTotales += req.Acierto ? 1 : 0;

            // Obtiene el ID del jugador relacionado a la partida
            var jugadorId = partida.JugadorId;

            // Busca las estadísticas existentes del jugador
            var estadisticas = await _context.Estadisticas.FirstOrDefaultAsync(e => e.JugadorId == jugadorId);
            if (estadisticas == null)
            {
                // Si no existen estadísticas, las crea
                estadisticas = new Estadistica
                {
                    JugadorId = jugadorId,
                    TotalPartidas = 1,
                    AciertosTotales = req.Acierto ? 1 : 0,
                    FallosTotales = req.Acierto ? 0 : 1,
                    MejorPuntuacion = req.Acierto ? 1 : 0
                };
                _context.Estadisticas.Add(estadisticas);
            }
            else
            {
                // Si ya existen, las actualiza
                estadisticas.AciertosTotales += req.Acierto ? 1 : 0;
                estadisticas.FallosTotales += req.Acierto ? 0 : 1;
                estadisticas.MejorPuntuacion = Math.Max(estadisticas.MejorPuntuacion, partida.PuntosTotales);
            }

            // Guarda todos los cambios
            await _context.SaveChangesAsync();

            // Devuelve éxito
            return Ok("Tiro registrado");
        }

        // Endpoint: PUT /api/partidas/finalizar/{id}
        [HttpPut("finalizar/{id}")]
        public async Task<IActionResult> FinalizarPartida(int id)
        {
            // Busca la partida por ID
            var partida = await _context.Partidas.FindAsync(id);
            if (partida == null)
                return NotFound("Partida no encontrada");

            var jugadorId = partida.JugadorId;

            // Actualiza estadísticas aunque no haya tiros
            var estadisticas = await _context.Estadisticas.FirstOrDefaultAsync(e => e.JugadorId == jugadorId);
            if (estadisticas == null)
            {
                // Si no existen, las crea
                estadisticas = new Estadistica
                {
                    JugadorId = jugadorId,
                    TotalPartidas = 1,
                    AciertosTotales = 0,
                    FallosTotales = 0,
                    MejorPuntuacion = partida.PuntosTotales
                };
                _context.Estadisticas.Add(estadisticas);
            }
            else
            {
                // Si existen, suma una partida y actualiza puntuación
                estadisticas.TotalPartidas += 1;
                estadisticas.MejorPuntuacion = Math.Max(estadisticas.MejorPuntuacion, partida.PuntosTotales);
            }

            // Actualiza la fecha de finalización si se desea registrar
            partida.Fecha = DateTime.Now;

            // Guarda cambios
            await _context.SaveChangesAsync();

            // Devuelve confirmación
            return Ok("Partida finalizada y estadísticas actualizadas");
        }
    }
}
