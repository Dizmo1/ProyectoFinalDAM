namespace BaloncestoAPI
{
    public class Partida
    {
        public int Id { get; set; }
        public int JugadorId { get; set; } // FK a Jugadores
        public DateTime Fecha { get; set; }
        public int Puntuacion { get; set; }
        public int DuracionSegundos { get; set; }
    }
}

