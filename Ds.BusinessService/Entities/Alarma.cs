using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceAlarma", Namespace = "http://www.dsystem.co/types/")]
    public class Alarma
    {
        private int? _IdTransaccion;
        private int? _IdCarga;
        private int? _IdArqueo;
        private int? _IdLogWS;
        private string _TipoError;
        private string _NombreParte;
        private string _Descripcion;
        private string _IdCajero;
        private long _IdSede;
        private int _NivelError;


        [DataMember]
        public int NivelError
        {
            get { return _NivelError; }
            set { _NivelError = value; }
        }

        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }

        [DataMember]
        public int? IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        [DataMember]
        public int? IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }

        [DataMember]
        public int? IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }

        [DataMember]
        public int? IdLogWS
        {
            get { return _IdLogWS; }
            set { _IdLogWS = value; }
        }

        [DataMember]
        public string TipoError
        {
            get { return _TipoError; }
            set { _TipoError = value; }
        }

        [DataMember]
        public string NombreParte
        {
            get { return _NombreParte; }
            set { _NombreParte = value; }
        }

        [DataMember]
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        [DataMember]
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
    }
}
