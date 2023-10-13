using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoPago", Namespace = "http://www.dsystem.co/types/")]
    public class DtoPago
    {
        private long _ID_Pago = 0;
        private string _Factura = string.Empty;
        private string _Referencia = string.Empty;
        private string _CodigoBarras = string.Empty;
        private TipoPago _TipoPago = TipoPago.Efectivo;
        private TipoEstadoPago _EstadoPago = TipoEstadoPago.NoAplica;
        private double _Total;
        private double _Comision;
        private double _Redondeo;
        private double _Iva;

        [DataMember]
        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
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
        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        [DataMember]
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
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
