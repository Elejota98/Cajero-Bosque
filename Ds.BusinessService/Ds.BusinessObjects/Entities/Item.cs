using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Item
    {
        private string _NumeroOrdenCompra = string.Empty;
        private string _NumeroCliente = string.Empty;
        private string _NombreCliente = string.Empty;
        private string _NumeroVendedor = string.Empty;
        private string _NombreVendedor = string.Empty;
        private string _Responsable = string.Empty;
        private string _ValorTotal = string.Empty;
        private string _Iva = string.Empty;
        private string _Subtotal = string.Empty;
        private int _NumeroFilas = 0;

        public string NumeroOrdenCompra
        {
            get { return _NumeroOrdenCompra; }
            set { _NumeroOrdenCompra = value; }
        }

        public string NumeroCliente
        {
            get { return _NumeroCliente; }
            set { _NumeroCliente = value; }
        }

        public string NombreCliente
        {
            get { return _NombreCliente; }
            set { _NombreCliente = value; }
        }

        public string NumeroVendedor
        {
            get { return _NumeroVendedor; }
            set { _NumeroVendedor = value; }
        }

        public string NombreVendedor
        {
            get { return _NombreVendedor; }
            set { _NombreVendedor = value; }
        }

        public string Responsable
        {
            get { return _Responsable; }
            set { _Responsable = value; }
        }

        public string ValorTotal
        {
            get { return _ValorTotal; }
            set { _ValorTotal = value; }
        }

        public string Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        public string Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; }
        }

        public int NumeroFilas
        {
            get { return _NumeroFilas; }
            set { _NumeroFilas = value; }
        }
    }
}
