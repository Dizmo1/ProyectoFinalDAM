using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [System.Serializable]
    public class EditarUsuarioRequest
    {
        public string email;
        public string nuevoNombre;
        public string nuevaContraseña;
    }

}
