using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{

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

        
        public int CarrilEntrada
        {
            get { return _CarrilEntrada; }
            set { _CarrilEntrada = value; }
        }
       
        public string ModuloEntrada
        {
            get { return _ModuloEntrada; }
            set { _ModuloEntrada = value; }
        }
        
        public long IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }
      
        public string IdTarjeta
        {
            get { return _IdTarjeta; }
            set { _IdTarjeta = value; }
        }
       
        public string PlacaEntrada
        {
            get { return _PlacaEntrada; }
            set { _PlacaEntrada = value; }
        }

       
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }



       
        public string Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }
        private string _Linea;
       
        public string Linea
        {
            get { return _Linea; }
            set { _Linea = value; }
        }
        private string _Descripcion;
        
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        private string _Fundacion;
       
        public string Fundacion
        {
            get { return _Fundacion; }
            set { _Fundacion = value; }
        }

       
        public double ValorRecibido
        {
            get { return _ValorRecibido; }
            set { _ValorRecibido = value; }
        }

       
        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

       
        public double Comision
        {
            get { return _Comision; }
            set { _Comision = value; }
        }

      
        public double Redondeo
        {
            get { return _Redondeo; }
            set { _Redondeo = value; }
        }

        
        public double TotalPagado
        {
            get { return _TotalPagado; }
            set { _TotalPagado = value; }
        }

     
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }

       
        public string NumeroFactura
        {
            get { return _NumeroFactura; }
            set { _NumeroFactura = value; }
        }

       
        public bool Anulada
        {
            get { return _Anulada; }
            set { _Anulada = value; }
        }

        
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }

       
        public int EstadoTransaccion
        {
            get { return _EstadoTransaccion; }
            set { _EstadoTransaccion = value; }
        }

       
        public bool SincronizacionPago
        {
            get { return _SincronizacionPago; }
            set { _SincronizacionPago = value; }
        }


        
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
       
        public string IdDocumento
        {
            get { return _IdDocumento; }
            set { _IdDocumento = value; }
        }
       
        public DateTime FechaTransaccion
        {
            get { return _FechaTransaccion; }
            set { _FechaTransaccion = value; }
        }
        
        public long IdTipoTransaccion
        {
            get { return _IdTipoTransaccion; }
            set { _IdTipoTransaccion = value; }
        }
       
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
    }

}
