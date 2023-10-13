using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceTarjeta", Namespace = "http://www.dsystem.co/types/")]
    public class Tarjeta
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

       

        [DataMember]
        public int TipoTarjeta
        {
            get { return _TipoTarjeta; }
            set { _TipoTarjeta = value; }
        }
        [DataMember]
        public long Facial
        {
            get { return _Facial; }
            set { _Facial = value; }
        }
        [DataMember]
        public string IdCard
        {
            get { return _IdCard; }
            set { _IdCard = value; }
        }
        [DataMember]
        public int AntipassBack
        {
            get { return _AntipassBack; }
            set { _AntipassBack = value; }
        }
        [DataMember]
        public long ParkId
        {
            get { return _ParkId; }
            set { _ParkId = value; }
        }
        [DataMember]
        public int Zona
        {
            get { return _Zona; }
            set { _Zona = value; }
        }
        [DataMember]
        public int Puerta
        {
            get { return _Puerta; }
            set { _Puerta = value; }
        }
        [DataMember]
        public int TipoVehiculo
        {
            get { return _TipoVehiculo; }
            set { _TipoVehiculo = value; }
        }
        [DataMember]
        public string Placa
        {
            get { return _Placa; }
            set { _Placa = value; }
        }
        [DataMember]
        public string FechaEntrada
        {
            get { return _FechaEntrada; }
            set { _FechaEntrada = value; }
        }
        [DataMember]
        public string TramaTarifa
        {
            get { return _TramaTarifa; }
            set { _TramaTarifa = value; }
        }
        [DataMember]
        public int IdConvenio
        {
            get { return _IdConvenio; }
            set { _IdConvenio = value; }
        }
        [DataMember]
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
        [DataMember]
        public int TipoServicio
        {
            get { return _TipoServicio; }
            set { _TipoServicio = value; }
        }
        [DataMember]
        public int ValorConvenio
        {
            get { return _ValorConvenio; }
            set { _ValorConvenio = value; }
        }
        [DataMember]
        public bool BanderaPrepago
        {
            get { return _BanderaPrepago; }
            set { _BanderaPrepago = value; }
        }
        [DataMember]
        public string FechaLimiteSalida
        {
            get { return _FechaLimiteSalida; }
            set { _FechaLimiteSalida = value; }
        }
        [DataMember]
        public string FechaPago
        {
            get { return _FechaPago; }
            set { _FechaPago = value; }
        }
       

    }
}
