using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoFactura", Namespace = "http://www.dsystem.co/types/")]
    public class DtoFactura
    {
        private long _IdFacturacion;
        private string _IdModulo;
        private long _IdSede;
        private string _Prefijo;
        private int _FacturaInicial;
        private int _FacturaFinal;
        private int _FacturaActual;
        private long _NumeroResolucion;
        private string _FechaResolucion;
        private string _FechaFinResolucion;
        private bool _Estado;

      

        [DataMember]
        public long IdFacturacion
        {
            get { return _IdFacturacion; }
            set { _IdFacturacion = value; }
        }
        [DataMember]
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        [DataMember]
        public string Prefijo
        {
            get { return _Prefijo; }
            set { _Prefijo = value; }
        }
        [DataMember]
        public int FacturaInicial
        {
            get { return _FacturaInicial; }
            set { _FacturaInicial = value; }
        }
        [DataMember]
        public int FacturaFinal
        {
            get { return _FacturaFinal; }
            set { _FacturaFinal = value; }
        }
        [DataMember]
        public int FacturaActual
        {
            get { return _FacturaActual; }
            set { _FacturaActual = value; }
        }
        [DataMember]
        public long NumeroResolucion
        {
            get { return _NumeroResolucion; }
            set { _NumeroResolucion = value; }
        }
        [DataMember]
        public string FechaResolucion
        {
            get { return _FechaResolucion; }
            set { _FechaResolucion = value; }
        }
        [DataMember]
        public string FechaFinResolucion
        {
            get { return _FechaFinResolucion; }
            set { _FechaFinResolucion = value; }
        }
        [DataMember]
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        
    }
}
