using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    public class RegistroRequest
    {
        public string nombre;
        public string email;
        public string contraseña;
    }

    [Serializable]
    public class LoginRequest
    {
        public string email;
        public string contraseña;
    }
}

