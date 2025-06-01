public class Partida
{
    public int Id { get; set; }
    public int JugadorId { get; set; }
    public DateTime Fecha { get; set; }
    public int PuntosTotales { get; set; }

    // Propiedades de navegación
    public Jugador? Jugador { get; set; }
    public List<Tiro> Tiros { get; set; } = new();
}