using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class LogWS
    {
        private int _IdTransaccion;
        private string _Salida;
        private string _Entrada;
        private string _Metodo;
        private string _IdCajero;

        public int IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        public string Salida
        {
            get { return _Salida; }
            set { _Salida = value; }
        }

        public string Entrada
        {
            get { return _Entrada; }
            set { _Entrada = value; }
        }

        public string Metodo
        {
            get { return _Metodo; }
            set { _Metodo = value; }
        }

        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
    }
}
