// Define el espacio de nombres donde se encuentra la clase DTO (Data Transfer Object)
namespace BaloncestoAPI.DTO
{
    // Clase que representa la estructura de datos enviada al editar un usuario
    public class EditarUsuarioRequest
    {
        // Correo electrónico del usuario actual (obligatorio para identificarlo)
        public string email { get; set; }

        // Nuevo nombre que desea establecer (puede ser null si no se quiere cambiar)
        public string nuevoNombre { get; set; }

        // Nueva contraseña que desea establecer (puede ser null si no se quiere cambiar)
        public string nuevaContraseña { get; set; }
    }
}
