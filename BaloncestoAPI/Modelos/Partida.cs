// Clase que representa una partida individual jugada por un jugador
public class Partida
{
    // Identificador único de la partida (clave primaria)
    public int Id { get; set; }

    // Clave foránea que referencia al jugador que jugó esta partida
    public int JugadorId { get; set; }

    // Fecha y hora en la que se jugó la partida
    public DateTime Fecha { get; set; }

    // Puntos totales obtenidos por el jugador en esta partida
    public int PuntosTotales { get; set; }

    // Propiedad de navegación: objeto Jugador al que pertenece esta partida (puede ser null si no se ha cargado)
    public Jugador? Jugador { get; set; }

    // Propiedad de navegación: lista de tiros realizados durante esta partida
    public List<Tiro> Tiros { get; set; } = new();
}
