using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Usuario
    {
        private string _IdCriptUsuario;

        private long _IdUsuario;

        public long IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        public string IdCriptUsuario
        {
            get { return _IdCriptUsuario; }
            set { _IdCriptUsuario = value; }
        }

    }
}
