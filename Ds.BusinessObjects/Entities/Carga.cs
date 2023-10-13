using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Carga
    {
        private long _IdUsuario;
        private long _IdSede;
        private string _IdModulo;

        
        public long IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }
        
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
       
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
    }
}
