// Espacios de nombres básicos para funcionalidades comunes de .NET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Espacio de nombres que contiene los objetos de transferencia de datos (DTO) usados en Unity
namespace Assets.Scripts.DTO
{
    // Atributo que indica que esta clase puede ser serializada (esencial para convertirla a JSON)
    [System.Serializable]
    public class UsuarioDTO
    {
        // Identificador único del usuario (viene de la base de datos)
        public int id;

        // Nombre del usuario
        public string nombre;

        // Correo electrónico del usuario
        public string email;

        // Rol del usuario (por ejemplo, "jugador" o "admin")
        public string rol;

        // Fecha en la que se registró el usuario, almacenada como string (puede venir en formato ISO 8601)
        public string fechaRegistro;
    }
}
