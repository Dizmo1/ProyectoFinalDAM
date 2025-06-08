using System; // Importa funcionalidades básicas del sistema como tipos de datos y atributos
using System.Collections.Generic; // (No usado en este archivo pero incluido por defecto)
using System.Linq; // (No usado en este archivo pero incluido por defecto)
using System.Text; // (No usado en este archivo pero incluido por defecto)
using System.Threading.Tasks; // (No usado en este archivo pero incluido por defecto)

namespace Assets.Scripts // Espacio de nombres donde se agrupan las clases relacionadas con scripts del proyecto
{
    // Clase que representa la estructura de datos para registrar un usuario
    [Serializable] // Permite que la clase pueda ser convertida a JSON u otros formatos serializables
    public class RegistroRequest
    {
        public string nombre;       // Nombre del usuario
        public string email;        // Correo electrónico del usuario
        public string contraseña;   // Contraseña del usuario (sin cifrar, se cifra en el servidor)
    }

    // Clase que representa la estructura de datos para iniciar sesión
    [Serializable] // También serializable para enviar a través de JSON
    public class LoginRequest
    {
        public string email;        // Correo electrónico del usuario
        public string contraseña;   // Contraseña del usuario
    }
}
