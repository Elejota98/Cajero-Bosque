using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServicePago", Namespace = "http://www.dsystem.co/types/")]
    public class Pago
    {
        private long _ID_Pago;
        private string _Factura;
        private string _CodigoBarras;

       
        private string _Referencia;
        private TipoPago _TipoPago = TipoPago.Efectivo;
        private TipoEstadoPago _EstadoPago = TipoEstadoPago.NoAplica;
        private string _NoAutorizacion = string.Empty;

        [DataMember]
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }
        [DataMember]
        public string NoAutorizacion
        {
            get { return _NoAutorizacion; }
            set { _NoAutorizacion = value; }
        }

        private string _NoTarjeta = string.Empty;
        
        [DataMember]
        public string NoTarjeta
        {
            get { return _NoTarjeta; }
            set { _NoTarjeta = value; }
        }

        private string _Franquicia = string.Empty;

        [DataMember]
        public string Franquicia
        {
            get { return _Franquicia; }
            set { _Franquicia = value; }
        }

        [DataMember]
        public long ID_Pago
        {
            get { return _ID_Pago; }
            set { _ID_Pago = value; }
        }

        [DataMember]
        public string Factura
        {
            get { return _Factura; }
            set { _Factura = value; }
        }

        [DataMember]
        public string Referencia
        {
            get { return _Referencia; }
            set { _Referencia = value; }
        }

        [DataMember]
        public TipoPago TipoPago
        {
            get { return _TipoPago; }
            set { _TipoPago = value; }
        }

        [DataMember]
        public TipoEstadoPago EstadoPago
        {
            get { return _EstadoPago; }
            set { _EstadoPago = value; }
        }
    }
}
