using System; // Importa funcionalidades básicas del sistema .NET.
using System.Collections.Generic; // Importa clases para trabajar con listas y colecciones genéricas.
using System.Linq; // Permite realizar consultas LINQ sobre colecciones.
using System.Text; // Proporciona clases para trabajar con texto y codificación.
using System.Threading.Tasks; // Permite manejar operaciones asincrónicas y tareas.

namespace Assets.Scripts // Define el espacio de nombres donde se encuentra esta clase.
{
    [System.Serializable] // Permite que la clase se pueda serializar (por ejemplo, convertir a/desde JSON).
    public class ErrorResponse // Clase usada para recibir respuestas de error desde la API.
    {
        public string mensaje; // Mensaje de error devuelto por la API. Se espera una clave "mensaje" en el JSON.
    }

}
