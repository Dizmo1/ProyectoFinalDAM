// Espacios de nombres necesarios para tareas generales del sistema, listas, consultas LINQ y compatibilidad de tareas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Declaración del namespace donde está ubicada la clase (DTO = Data Transfer Object)
namespace Assets.Scripts.DTO
{
    // Atributo que indica que esta clase puede ser serializada (importante para JSON)
    [System.Serializable]
    public class PartidaResponse
    {
        // Campo público que representa el identificador de la partida que se recibe como respuesta de la API
        public int partidaId;
    }
}
