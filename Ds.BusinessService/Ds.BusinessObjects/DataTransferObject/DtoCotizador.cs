using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoCotizar
    {
        private string _Resultado;       
        private string _Mensaje;

        public string Mensaje
        {
            get { return _Mensaje; }
            set { _Mensaje = value; }
        }
        public string Resultado
        {
            get { return _Resultado; }
            set { _Resultado = value; }
        }
    }
}
