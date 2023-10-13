using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoParametro
    {
        private string _Codigo = string.Empty;

        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        private string _IdCajero = string.Empty;

        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }

        private string _Valor = string.Empty;

        public string Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        private bool _Activo;

        public bool Activo
        {
            get { return _Activo; }
            set { _Activo = value; }
        }

        private string _Descripcion = string.Empty;

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
    }
}
