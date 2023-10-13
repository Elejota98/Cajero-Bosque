using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceConsultarResult", Namespace = "http://www.dsystem.co/types/")]
    public class ConsultarResult
    {
        private object _error;
        [DataMember]
        public object Error
        {
            get { return _error; }
            set { _error = value; }
        }
        private bool _estado;
        [DataMember]
        public bool Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }
        private string _fechaLiquidacion;
        [DataMember]
        public string FechaLiquidacion
        {
            get { return _fechaLiquidacion; }
            set { _fechaLiquidacion = value; }
        }
        private int _idEntrada;
        [DataMember]
        public int IdEntrada
        {
            get { return _idEntrada; }
            set { _idEntrada = value; }
        }
        private string _nombreTarifa;
        [DataMember]
        public string NombreTarifa
        {
            get { return _nombreTarifa; }
            set { _nombreTarifa = value; }
        }
        private string _permanencia;
        [DataMember]
        public string Permanencia
        {
            get { return _permanencia; }
            set { _permanencia = value; }
        }
        private bool _recobro;
        [DataMember]
        public bool Recobro
        {
            get { return _recobro; }
            set { _recobro = value; }
        }
        private double _valorAPagar;
        [DataMember]
        public double ValorAPagar
        {
            get { return _valorAPagar; }
            set { _valorAPagar = value; }
        }
        private int _valorDescuento;
        [DataMember]
        public int ValorDescuento
        {
            get { return _valorDescuento; }
            set { _valorDescuento = value; }
        }
        private int _valorEmpresa;
        [DataMember]
        public int ValorEmpresa
        {
            get { return _valorEmpresa; }
            set { _valorEmpresa = value; }
        }
        private double _valorServicio;
        [DataMember]
        public double ValorServicio
        {
            get { return _valorServicio; }
            set { _valorServicio = value; }
        }
        private string _serialMaquina;
        [DataMember]
        public string SerialMaquina
        {
            get { return _serialMaquina; }
            set { _serialMaquina = value; }
        }

        private string _IdPago;
        [DataMember]
        public string IdPago
        {
            get { return _IdPago; }
            set { _IdPago = value; }
        }
        private string _IdParqueadero;
        [DataMember]
        public string IdParqueadero
        {
            get { return _IdParqueadero; }
            set { _IdParqueadero = value; }
        }

        private string _PlacaSalida;
        [DataMember]
        public string PlacaSalida
        {
            get { return _PlacaSalida; }
            set { _PlacaSalida = value; }
        }

        private string _RespuestaCajas;
        [DataMember]
        public string RespuestaCajas
        {
            get { return _RespuestaCajas; }
            set { _RespuestaCajas = value; }
        }

    }
}
