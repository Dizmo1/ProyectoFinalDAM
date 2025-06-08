using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using UnityEngine; // Necesario para usar JsonUtility

    // Clase auxiliar estática para ayudar a deserializar arrays JSON
    public static class JsonHelper
    {
        // Método genérico que convierte un JSON en un array de objetos del tipo T
        public static T[] FromJson<T>(string json)
        {
            // Deserializa el JSON usando la clase interna Wrapper<T>
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.usuarios; // Devuelve el array de objetos
        }

        // Clase interna usada como envoltorio para que JsonUtility pueda deserializar arrays
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] usuarios; // Debe coincidir con la clave del JSON ("usuarios")
        }
    }
}
