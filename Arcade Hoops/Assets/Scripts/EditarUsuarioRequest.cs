using System; // Importa funcionalidades básicas del sistema como tipos primitivos y fechas.
using System.Collections.Generic; // Importa funcionalidades para trabajar con listas genéricas (no se usa directamente aquí).
using System.Linq; // Importa funcionalidades para manipulación de colecciones (no se usa directamente aquí).
using System.Text; // Importa clases para manipulación de texto y codificación (no se usa directamente aquí).
using System.Threading.Tasks; // Importa funcionalidades para programación asíncrona (no se usa directamente aquí).

namespace Assets.Scripts // Define el espacio de nombres donde se encuentra esta clase, útil para organizar el código.
{
    [System.Serializable] // Atributo que permite que la clase pueda ser serializada, por ejemplo, a JSON, en Unity.
    public class EditarUsuarioRequest // Clase que representa los datos enviados al editar un usuario.
    {
        public string email; // Email actual del usuario, usado para identificar qué cuenta modificar.
        public string nuevoNombre; // Nuevo nombre que el usuario quiere usar (opcional).
        public string nuevaContraseña; // Nueva contraseña que el usuario quiere usar (opcional).
    }
}
