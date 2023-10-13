using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class TarjetaSmart
    {
        private int _TipoTarjeta;
        private long _Facial;
        private string _IdCard;
        private int _AntipassBack;
        private long _ParkId;
        private int _Zona;
        private int _Puerta;
        private int _TipoVehiculo;
        private string _Placa;
        private string _FechaEntrada;
        private string _TramaTarifa;
        private int _IdConvenio;
        private string _IdCajero;
        private int _TipoServicio;
        private bool _BanderaPrepago;
        private int _ValorConvenio;
        private string _FechaPago;
        private string _FechaLimiteSalida;
        private string _Identificacion;
        private string _NombreCliente;
        private int _TipoProducto;
        private long _SaldoAnterior;
        
        
        
        public string FechaLimiteSalida
        {
            get { return _FechaLimiteSalida; }
            set { _FechaLimiteSalida = value; }
        }

        public string FechaPago
        {
            get { return _FechaPago; }
            set { _FechaPago = value; }
        }

        public int ValorConvenio
        {
            get { return _ValorConvenio; }
            set { _ValorConvenio = value; }
        }


        public bool BanderaPrepago
        {
            get { return _BanderaPrepago; }
            set { _BanderaPrepago = value; }
        } 

        public int TipoServicio
        {
            get { return _TipoServicio; }
            set { _TipoServicio = value; }
        }

        public int TipoTarjeta
        {
            get { return _TipoTarjeta; }
            set { _TipoTarjeta = value; }
        }
        public long Facial
        {
            get { return _Facial; }
            set { _Facial = value; }
        }
        public string IdCard
        {
            get { return _IdCard; }
            set { _IdCard = value; }
        }
        public int AntipassBack
        {
            get { return _AntipassBack; }
            set { _AntipassBack = value; }
        }
        public long ParkId
        {
            get { return _ParkId; }
            set { _ParkId = value; }
        }
        public int Zona
        {
            get { return _Zona; }
            set { _Zona = value; }
        }
        public int Puerta
        {
            get { return _Puerta; }
            set { _Puerta = value; }
        }
        public int TipoVehiculo
        {
            get { return _TipoVehiculo; }
            set { _TipoVehiculo = value; }
        }
        public string Placa
        {
            get { return _Placa; }
            set { _Placa = value; }
        }
        public string FechaEntrada
        {
            get { return _FechaEntrada; }
            set { _FechaEntrada = value; }
        }
        public string TramaTarifa
        {
            get { return _TramaTarifa; }
            set { _TramaTarifa = value; }
        }
        public int IdConvenio
        {
            get { return _IdConvenio; }
            set { _IdConvenio = value; }
        }
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }

        public string NombreCliente
        {
            get { return _NombreCliente; }
            set { _NombreCliente = value; }
        }


        public string Identificacion
        {
            get { return _Identificacion; }
            set { _Identificacion = value; }
        }


        public long SaldoAnterior
        {
            get { return _SaldoAnterior; }
            set { _SaldoAnterior = value; }
        }


        public int TipoProducto
        {
            get { return _TipoProducto; }
            set { _TipoProducto = value; }
        }
    }
}
