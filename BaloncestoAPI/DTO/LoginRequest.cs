// Define el espacio de nombres donde se agrupan los DTO (Data Transfer Objects) usados en la API
namespace BaloncestoAPI.DTO
{
    // Clase que representa los datos enviados por el usuario para iniciar sesión
    public class LoginRequest
    {
        // Dirección de correo electrónico del usuario
        public string email { get; set; }

        // Contraseña en texto plano enviada para autenticar al usuario
        public string contraseña { get; set; }
    }
}
