namespace BaloncestoAPI.DTO
{
    public class EditarUsuarioRequest
    {
        public string email { get; set; }
        public string nuevoNombre { get; set; }
        public string nuevaContraseña { get; set; }
    }
}
