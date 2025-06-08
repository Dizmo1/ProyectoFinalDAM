// Clase que representa a un jugador del sistema
public class Jugador
{
    // Identificador único del jugador (clave primaria)
    public int Id { get; set; }

    // Nombre del jugador (debe ser único en el sistema)
    public string Nombre { get; set; }

    // Dirección de correo electrónico del jugador (también usada como identificador de login)
    public string Email { get; set; }

    // Hash de la contraseña del jugador (encriptada con SHA-256)
    public string ContraseñaHash { get; set; }

    // Fecha en la que se registró el jugador en el sistema
    public DateTime FechaRegistro { get; set; }

    // Rol del usuario en el sistema: puede ser "jugador" o "admin" (por defecto, "jugador")
    public string Rol { get; set; } = "jugador";

    // Propiedad de navegación: lista de partidas jugadas por este jugador
    public List<Partida> Partidas { get; set; } = new();

    // Propiedad de navegación: estadísticas asociadas a este jugador (puede ser null)
    public Estadistica? Estadistica { get; set; }
}
