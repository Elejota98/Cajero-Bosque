using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class PagoEfectivo
    {
        private Int64 _ValorPago = 0;
        private Int64 _ValorRecibido = 0;
        private int _ValorCambio = 0;
        private Int64 _ValorEntregado = 0;
        private Int64 _ValorProcesoCambio = 0;
        private Int64 _ValorRecibidoBilletes = 0;
        private Int64 _ValorRecibidoMonedas = 0;
        private Int64 _ValorIva = 0;
        private int _CantidadBilletesRecibidos = 0;
        private int _CantidadMonedasRecibidas = 0;
        private bool _BilleteRecibido = false;
        private bool _MonedaRecibida = false;
        private int _ValorPagoMonedas = 0;
        private Int64 _Subtotal = 0;

        public Int64 Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; }
        }
        private Int64 _Iva = 0;

        public Int64 Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }


        public int ValorPagoMonedas
        {
            get { return _ValorPagoMonedas; }
            set { _ValorPagoMonedas = value; }
        }

        public int CantidadMonedasRecibidas
        {
            get { return _CantidadMonedasRecibidas; }
            set { _CantidadMonedasRecibidas = value; }
        }

        public bool MonedaRecibida
        {
            get { return _MonedaRecibida; }
            set { _MonedaRecibida = value; }
        }

        public bool BilleteRecibido
        {
            get { return _BilleteRecibido; }
            set { _BilleteRecibido = value; }
        }

        public Int64 ValorPago
        {
            get { return _ValorPago; }
            set { _ValorPago = value; }
        }

        public Int64 ValorRecibido
        {
            get { return _ValorRecibido; }
            set { _ValorRecibido = value; }
        }

        public int  ValorCambio
        {
            get { return _ValorCambio; }
            set { _ValorCambio = value; }
        }

        public Int64 ValorEntregado
        {
            get { return _ValorEntregado; }
            set { _ValorEntregado = value; }
        }

        public Int64 ValorProcesoCambio
        {
            get { return _ValorProcesoCambio; }
            set { _ValorProcesoCambio = value; }
        }

        public Int64 ValorRecibidoBilletes
        {
            get { return _ValorRecibidoBilletes; }
            set { _ValorRecibidoBilletes = value; }
        }

        public Int64 ValorRecibidoMonedas
        {
            get { return _ValorRecibidoMonedas; }
            set { _ValorRecibidoMonedas = value; }
        }

        public int CantidadBilletesRecibidos
        {
            get { return _CantidadBilletesRecibidos; }
            set { _CantidadBilletesRecibidos = value; }
        }

        public Int64 ValorIva
        {
            get { return _ValorIva; }
            set { _ValorIva = value; }
        }
    }
}
