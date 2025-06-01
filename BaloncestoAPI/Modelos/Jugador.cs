public class Jugador
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string ContraseñaHash { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Rol { get; set; } = "jugador";

    // Propiedades de navegación
    public List<Partida> Partidas { get; set; } = new();
    public Estadistica? Estadistica { get; set; }
}