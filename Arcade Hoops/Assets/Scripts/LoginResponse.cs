// Marca la clase como serializable para poder convertirla desde/hacia JSON
[System.Serializable]
public class LoginResponse
{
    // Token JWT que se recibe tras un login exitoso
    public string token;

    // Nombre del usuario autenticado
    public string nombre;

    // Rol del usuario (por ejemplo: "jugador" o "admin")
    public string rol;

    // Email del usuario
    public string email;
}
