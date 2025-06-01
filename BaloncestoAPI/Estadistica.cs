namespace BaloncestoAPI
{
    public class Estadistica
    {
        public int Id { get; set; }
        public int JugadorId { get; set; }
        public int TotalPartidas { get; set; }
        public int TotalTiros { get; set; }
        public int TirosAcertados { get; set; }
        public double MejorPuntuacion { get; set; } 
    }

}
