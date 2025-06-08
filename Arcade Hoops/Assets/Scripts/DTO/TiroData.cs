// Espacios de nombres estándar de .NET para funcionalidades básicas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Espacio de nombres que agrupa las clases de transferencia de datos (DTO) del proyecto
namespace Assets.Scripts.DTO
{
    // Atributo que indica que esta clase puede ser serializada (necesario para convertirla a JSON)
    [System.Serializable]
    public class TiroData
    {
        // ID de la partida a la que pertenece el tiro
        public int partidaId;

        // Indica si el tiro fue acertado (true) o fallado (false)
        public bool acierto;

        // Tiempo transcurrido en segundos desde el inicio de la partida hasta el momento del tiro
        public float tiempoSegundos;

        // Distancia desde el punto de lanzamiento hasta el punto donde se recogió el impacto
        public float distancia;
    }
}
