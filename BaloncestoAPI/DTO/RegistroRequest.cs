// Define el espacio de nombres donde se encuentran los DTO (objetos de transferencia de datos) de la API
namespace BaloncestoAPI.DTO
{
    // Clase que representa los datos necesarios para registrar un nuevo usuario
    public class RegistroRequest
    {
        // Nombre del usuario que se quiere registrar
        public string nombre { get; set; }

        // Dirección de correo electrónico del usuario
        public string email { get; set; }

        // Contraseña que el usuario elige para su cuenta
        public string contraseña { get; set; }
    }
}
