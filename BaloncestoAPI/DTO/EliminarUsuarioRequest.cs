// Define el espacio de nombres donde se agrupan los DTO (Data Transfer Objects) de la API
namespace BaloncestoAPI.DTO
{
    // Clase que representa la solicitud para eliminar un usuario
    public class EliminarUsuarioRequest
    {
        // Email del usuario que se desea eliminar
        public string email { get; set; }

    }
}
