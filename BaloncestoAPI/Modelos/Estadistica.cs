public class Estadistica
{
    public int Id { get; set; }
    public int JugadorId { get; set; }
    public int TotalPartidas { get; set; }
    public int AciertosTotales { get; set; }
    public int FallosTotales { get; set; }
    public int MejorPuntuacion { get; set; }

    // Propiedad de navegación
    public Jugador? Jugador { get; set; }
}