using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoParametro", Namespace = "http://www.dsystem.co/types/")]
    public class DtoParametro
    {
        private long _IdParametro;
        private string _IdModulo;
        private long _IdSede;
        private string _Codigo;
        private string _Valor;
        private string _Descripcion;
        private bool _Estado;
        private bool _Sincronizacion;
        
        [DataMember]
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }
        [DataMember]
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        [DataMember]
        public string Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        [DataMember]
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        [DataMember]
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
        [DataMember]
        public long IdParametro
        {
            get { return _IdParametro; }
            set { _IdParametro = value; }
        }

        [DataMember]
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
    }
}
