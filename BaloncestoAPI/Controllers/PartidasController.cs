// PartidasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using BaloncestoAPI.Datos;
using Microsoft.EntityFrameworkCore;

namespace BaloncestoAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PartidasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartidasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("nuevaPartida")]
        public async Task<IActionResult> NuevaPartida()
        {
            var jugadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var partida = new Partida
            {
                JugadorId = jugadorId,
                Fecha = DateTime.UtcNow,
                PuntosTotales = 0
            };

            _context.Partidas.Add(partida);
            await _context.SaveChangesAsync();

            return Ok(new { partidaId = partida.Id });
        }

        [HttpPost("registrarTiro")]
        public async Task<IActionResult> RegistrarTiro([FromBody] Tiro req)
        {
            var partida = await _context.Partidas.FindAsync(req.PartidaId);
            if (partida == null)
                return NotFound("Partida no encontrada");

            var tiro = new Tiro
            {
                PartidaId = req.PartidaId,
                Acierto = req.Acierto,
                TiempoSegundos = req.TiempoSegundos,
                Distancia = req.Distancia
            };

            _context.Tiros.Add(tiro);

            // Actualizar estadísticas y puntos de la partida
            partida.PuntosTotales += req.Acierto ? 1 : 0;

            var jugadorId = partida.JugadorId;
            var estadisticas = await _context.Estadisticas.FirstOrDefaultAsync(e => e.JugadorId == jugadorId);
            if (estadisticas == null)
            {
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
                estadisticas.AciertosTotales += req.Acierto ? 1 : 0;
                estadisticas.FallosTotales += req.Acierto ? 0 : 1;
                estadisticas.MejorPuntuacion = Math.Max(estadisticas.MejorPuntuacion, partida.PuntosTotales);
            }

            await _context.SaveChangesAsync();
            return Ok("Tiro registrado");
        }

        [HttpPut("finalizar/{id}")]
        public async Task<IActionResult> FinalizarPartida(int id)
        {
            var partida = await _context.Partidas.FindAsync(id);
            if (partida == null)
                return NotFound("Partida no encontrada");

            var jugadorId = partida.JugadorId;

            // Actualizar estadística aunque no haya tiros
            var estadisticas = await _context.Estadisticas.FirstOrDefaultAsync(e => e.JugadorId == jugadorId);
            if (estadisticas == null)
            {
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
                estadisticas.TotalPartidas += 1;
                estadisticas.MejorPuntuacion = Math.Max(estadisticas.MejorPuntuacion, partida.PuntosTotales);
            }

            // Actualizar la fecha si quieres dejar constancia de finalización
            partida.Fecha = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok("Partida finalizada y estadísticas actualizadas");
        }


    }
}