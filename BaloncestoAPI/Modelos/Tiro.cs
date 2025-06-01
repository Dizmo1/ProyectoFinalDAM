public class Tiro
{
    public int Id { get; set; }
    public int PartidaId { get; set; }
    public bool Acierto { get; set; }
    public float TiempoSegundos { get; set; }
    public float Distancia { get; set; }

    // Propiedad de navegación
    public Partida? Partida { get; set; }
}