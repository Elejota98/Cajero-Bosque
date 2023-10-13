using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceCarga", Namespace = "http://www.dsystem.co/types/")]
    public class Carga
    {
        private long _IdCarga;
        private DateTime _FechaInicio;
        private DateTime? _FechaFin;
        private double _Valor;
        private long _IdUsuario;
        private string _IdModulo;
        private long _IdSede;
        private bool _Sincronizacion;


        [DataMember]
        public long IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }

        [DataMember]
        public DateTime FechaInicio
        {
            get { return _FechaInicio; }
            set { _FechaInicio = value; }
        }
        [DataMember]
        public DateTime? FechaFin
        {
            get { return _FechaFin; }
            set { _FechaFin = value; }
        }
        [DataMember]
        public double Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        [DataMember]
        public long IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
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
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }
    }
}
