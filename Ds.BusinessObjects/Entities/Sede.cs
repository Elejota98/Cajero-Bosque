using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Sede
    {
        private string _ID_Sede;
        private string _Nombre_Sede;
        private string _Ciudad;
        private string _Departamento;
        private string _Direccion;
        private string _Descripcion;

        public string ID_Sede
        {
            get { return _ID_Sede; }
            set { _ID_Sede = value; }
        }

        public string Nombre_Sede
        {
            get { return _Nombre_Sede; }
            set { _Nombre_Sede = value; }
        }

        public string Ciudad
        {
            get { return _Ciudad; }
            set { _Ciudad = value; }
        }

        public string Departamento
        {
            get { return _Departamento; }
            set { _Departamento = value; }
        }

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
    }
}
