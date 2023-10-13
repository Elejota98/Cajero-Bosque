using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class EstadoModulo
    {
        private string _ID_Modulo;
        private string _Nombre_Pantalla;
        private string _Version;

        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }
        public string Nombre_Pantalla
        {
            get { return _Nombre_Pantalla; }
            set { _Nombre_Pantalla = value; }
        }
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
    }
}
