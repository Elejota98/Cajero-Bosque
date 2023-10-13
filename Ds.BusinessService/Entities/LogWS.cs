using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceLogWS", Namespace = "http://www.eglobalt.com/types/")]
    public class LogWS
    {
        private int _IdTransaccion;
        private string _Salida;
        private string _Entrada;
        private string _Metodo;
        private string _IdCajero;

        [DataMember]
        public int IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        [DataMember]
        public string Salida
        {
            get { return _Salida; }
            set { _Salida = value; }
        }

        [DataMember]
        public string Entrada
        {
            get { return _Entrada; }
            set { _Entrada = value; }
        }

        [DataMember]
        public string Metodo
        {
            get { return _Metodo; }
            set { _Metodo = value; }
        }

        [DataMember]
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
    }
}
