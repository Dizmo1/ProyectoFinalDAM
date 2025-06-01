namespace BaloncestoAPI
{
    public class Tiro
    {
        public int Id { get; set; }
        public int PartidaId { get; set; } // FK a Partidas
        public int TiempoDesdeInicio { get; set; } // Segundos desde que empezó la partida
        public bool Acertado { get; set; } // true = canasta, false = fallo
    }

}
