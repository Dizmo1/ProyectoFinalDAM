using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DTO
{
    [System.Serializable]
    public class UsuarioDTO
    {
        public int id;
        public string nombre;
        public string email;
        public string rol;
        public string fechaRegistro;
    }

}
