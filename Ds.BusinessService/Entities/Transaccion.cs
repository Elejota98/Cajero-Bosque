using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceTransaccion", Namespace = "http://www.dsystem.co/types/")]
    public class Transaccion
    {

        private long _IdTransaccion;
        private string _IdModulo;
        private long _IdTipoTransaccion;
        private long _IdSede;
        private string _IdDocumento;
        private double _ValorRecibido;
        private double _Iva;
        private double _Comision;
        private double _Redondeo;
        private double _TotalPagado;
        private string _CodigoBarras;
        private string _NumeroFactura;
        private bool _Anulada;
        private bool _Sincronizacion;
        private int _EstadoTransaccion;
        private DateTime _FechaTransaccion;
        private bool _SincronizacionPago;
        private string _Operador;


        private int _CarrilEntrada;
        private string _ModuloEntrada;
        private long _IdEstacionamiento;
        private string _IdTarjeta;
        private string _PlacaEntrada;
        private int _IdTipoVehiculo;

        [DataMember]
        public int CarrilEntrada
        {
            get { return _CarrilEntrada; }
            set { _CarrilEntrada = value; }
        }
        [DataMember]
        public string ModuloEntrada
        {
            get { return _ModuloEntrada; }
            set { _ModuloEntrada = value; }
        }
        [DataMember]
        public long IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }
        [DataMember]
        public string IdTarjeta
        {
            get { return _IdTarjeta; }
            set { _IdTarjeta = value; }
        }
        [DataMember]
        public string PlacaEntrada
        {
            get { return _PlacaEntrada; }
            set { _PlacaEntrada = value; }
        }

        [DataMember]
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }



        [DataMember]
        public string Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }
        private string _Linea;
        [DataMember]
        public string Linea
        {
            get { return _Linea; }
            set { _Linea = value; }
        }
        private string _Descripcion;
        [DataMember]
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        private string _Fundacion;
        [DataMember]
        public string Fundacion
        {
            get { return _Fundacion; }
            set { _Fundacion = value; }
        }

        [DataMember]
        public double ValorRecibido
        {
            get { return _ValorRecibido; }
            set { _ValorRecibido = value; }
        }

        [DataMember]
        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        [DataMember]
        public double Comision
        {
            get { return _Comision; }
            set { _Comision = value; }
        }

        [DataMember]
        public double Redondeo
        {
            get { return _Redondeo; }
            set { _Redondeo = value; }
        }

        [DataMember]
        public double TotalPagado
        {
            get { return _TotalPagado; }
            set { _TotalPagado = value; }
        }

        [DataMember]
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }

        [DataMember]
        public string NumeroFactura
        {
            get { return _NumeroFactura; }
            set { _NumeroFactura = value; }
        }

        [DataMember]
        public bool Anulada
        {
            get { return _Anulada; }
            set { _Anulada = value; }
        }

        [DataMember]
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }

        [DataMember]
        public int EstadoTransaccion
        {
            get { return _EstadoTransaccion; }
            set { _EstadoTransaccion = value; }
        }

        [DataMember]
        public bool SincronizacionPago
        {
            get { return _SincronizacionPago; }
            set { _SincronizacionPago = value; }
        }


        [DataMember]
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        [DataMember]
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
        [DataMember]
        public string IdDocumento
        {
            get { return _IdDocumento; }
            set { _IdDocumento = value; }
        }
        [DataMember]
        public DateTime FechaTransaccion
        {
            get { return _FechaTransaccion; }
            set { _FechaTransaccion = value; }
        }
        [DataMember]
        public long IdTipoTransaccion
        {
            get { return _IdTipoTransaccion; }
            set { _IdTipoTransaccion = value; }
        }
        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
    }
}
