using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoParteModulo", Namespace = "http://www.dsystem.co/types/")]
    public class DtoParteModulo
    {
        private long _IdParte;
        private string _IdModulo;
        private long _IdSede;
        private string _TipoParte;
        private string _Nombre;
        private string _Denominacion;
        private string _CantidadMin;
        private string _CantidadMax;
        private string _NumParte;
        private string _IpDispositivo;
        private string _CantidadAlarma;
        private string _DineroActual;
        private string _CantidadActual;
        private bool _Prioridad;
        private bool _Estado;
        private bool _Sincronizacion;


        [DataMember]
        public long IdParte
        {
            get { return _IdParte; }
            set { _IdParte = value; }
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
        public string TipoParte
        {
            get { return _TipoParte; }
            set { _TipoParte = value; }
        }
        [DataMember]
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        [DataMember]
        public string Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }
        [DataMember]
        public string CantidadMin
        {
            get { return _CantidadMin; }
            set { _CantidadMin = value; }
        }
        [DataMember]
        public string CantidadMax
        {
            get { return _CantidadMax; }
            set { _CantidadMax = value; }
        }
        [DataMember]
        public string NumParte
        {
            get { return _NumParte; }
            set { _NumParte = value; }
        }
        [DataMember]
        public string IpDispositivo
        {
            get { return _IpDispositivo; }
            set { _IpDispositivo = value; }
        }
        [DataMember]
        public string CantidadAlarma
        {
            get { return _CantidadAlarma; }
            set { _CantidadAlarma = value; }
        }
        
        [DataMember]
        public string DineroActual
        {
            get { return _DineroActual; }
            set { _DineroActual = value; }
        }
        [DataMember]
        public string CantidadActual
        {
            get { return _CantidadActual; }
            set { _CantidadActual = value; }
        }
        [DataMember]
        public bool Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }
        [DataMember]
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        [DataMember]
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }
        
        
    }
}
