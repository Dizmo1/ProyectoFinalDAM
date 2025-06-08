// Clase que representa las estadísticas acumuladas de un jugador
public class Estadistica
{
    // Identificador único de la estadística (clave primaria)
    public int Id { get; set; }

    // Clave foránea que hace referencia al jugador dueño de esta estadística
    public int JugadorId { get; set; }

    // Número total de partidas jugadas por el jugador
    public int TotalPartidas { get; set; }

    // Total de tiros acertados por el jugador en todas las partidas
    public int AciertosTotales { get; set; }

    // Total de tiros fallados por el jugador en todas las partidas
    public int FallosTotales { get; set; }

    // Mejor puntuación conseguida por el jugador en una sola partida
    public int MejorPuntuacion { get; set; }

    // Propiedad de navegación para acceder al objeto Jugador relacionado (opcional, puede ser null)
    public Jugador? Jugador { get; set; }
}
