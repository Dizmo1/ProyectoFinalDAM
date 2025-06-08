// Clase que representa un lanzamiento (tiro) realizado durante una partida
public class Tiro
{
    // Identificador único del tiro (clave primaria)
    public int Id { get; set; }

    // Clave foránea que indica a qué partida pertenece este tiro
    public int PartidaId { get; set; }

    // Indica si el tiro fue un acierto (true) o un fallo (false)
    public bool Acierto { get; set; }

    // Tiempo (en segundos) transcurrido desde el inicio de la partida hasta que se hizo este tiro
    public float TiempoSegundos { get; set; }

    // Distancia (en metros o unidades definidas) desde donde se lanzó el tiro
    public float Distancia { get; set; }

    // Propiedad de navegación: referencia a la partida a la que pertenece este tiro (puede ser null si no se ha cargado)
    public Partida? Partida { get; set; }
}
