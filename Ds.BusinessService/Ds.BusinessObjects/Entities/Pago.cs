using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Pago
    {
        private long _ID_Pago;
        private string _Factura;
        private string _CodigoBarras;

        private string _Referencia;
        private TipoPago _TipoPago;
        private TipoEstadoPago _EstadoPago;
        private string _NoAutorizacion = string.Empty;

        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }
        public string NoAutorizacion
        {
            get { return _NoAutorizacion; }
            set { _NoAutorizacion = value; }
        }

        private string _NoTarjeta = string.Empty;

        public string NoTarjeta
        {
            get { return _NoTarjeta; }
            set { _NoTarjeta = value; }
        }

        private string _Franquicia = string.Empty;

        public string Franquicia
        {
            get { return _Franquicia; }
            set { _Franquicia = value; }
        }

        public long ID_Pago
        {
            get { return _ID_Pago; }
            set { _ID_Pago = value; }
        }

        public string Factura
        {
            get { return _Factura; }
            set { _Factura = value; }
        }

        public string Referencia
        {
            get { return _Referencia; }
            set { _Referencia = value; }
        }

        public TipoPago TipoPago
        {
            get { return _TipoPago; }
            set { _TipoPago = value; }
        }

        public TipoEstadoPago EstadoPago
        {
            get { return _EstadoPago; }
            set { _EstadoPago = value; }
        }
    }
}
