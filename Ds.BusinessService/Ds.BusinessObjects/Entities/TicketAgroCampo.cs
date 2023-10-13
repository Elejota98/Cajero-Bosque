using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class TicketAgroCampo
    {
        private string _RazonSocial = string.Empty;

        public string RazonSocial
        {
            get { return _RazonSocial; }
            set { _RazonSocial = value; }
        }

        private string _Direccion = string.Empty;

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }

        private string _NIT = string.Empty;

        public string NIT
        {
            get { return _NIT; }
            set { _NIT = value; }
        }

        private double _TOTAL = 0;

        public double TOTAL
        {
            get { return _TOTAL; }
            set { _TOTAL = value; }
        }

        private double _TOTAL1 = 0;

        public double TOTAL1
        {
            get { return _TOTAL1; }
            set { _TOTAL1 = value; }
        }

        private double _TOTAL2 = 0;

        public double TOTAL2
        {
            get { return _TOTAL2; }
            set { _TOTAL2 = value; }
        }

        private string _TEXTRE1LBL = string.Empty;

        public string TEXTRE1LBL
        {
            get { return _TEXTRE1LBL; }
            set { _TEXTRE1LBL = value; }
        }

        private double _TOTBA1 = 0;

        public double TOTBA1
        {
            get { return _TOTBA1; }
            set { _TOTBA1 = value; }
        }

        private double _TOTIM1 = 0;

        public double TOTIM1
        {
            get { return _TOTIM1; }
            set { _TOTIM1 = value; }
        }

        private double _TOTIM2 = 0;

        public double TOTIM2
        {
            get { return _TOTIM2; }
            set { _TOTIM2 = value; }
        }

        private string _TEXTRE2LBL = string.Empty;

        public string TEXTRE2LBL
        {
            get { return _TEXTRE2LBL; }
            set { _TEXTRE2LBL = value; }
        }

        private double _TOTBA2 = 0;

        public double TOTBA2
        {
            get { return _TOTBA2; }
            set { _TOTBA2 = value; }
        }

        private double _TOTBA3 = 0;

        public double TOTBA3
        {
            get { return _TOTBA3; }
            set { _TOTBA3 = value; }
        }

        private string _TEXTRE3LBL = string.Empty;

        public string TEXTRE3LBL
        {
            get { return _TEXTRE3LBL; }
            set { _TEXTRE3LBL = value; }
        }

        private double _TOTIM3 = 0;

        public double TOTIM3
        {
            get { return _TOTIM3; }
            set { _TOTIM3 = value; }
        }

        private double _TOTBASFA = 0;

        public double TOTBASFA
        {
            get { return _TOTBASFA; }
            set { _TOTBASFA = value; }
        }

        private double _TOTIVAFA = 0;

        public double TOTIVAFA
        {
            get { return _TOTIVAFA; }
            set { _TOTIVAFA = value; }
        }

        private int _TOTALQTY = 0;

        public int TOTALQTY
        {
            get { return _TOTALQTY; }
            set { _TOTALQTY = value; }
        }

        private string _RESOLDIANLBL1 = string.Empty;

        public string RESOLDIANLBL1
        {
            get { return _RESOLDIANLBL1; }
            set { _RESOLDIANLBL1 = value; }
        }

        private string _RESOLDIANLBL2 = string.Empty;

        public string RESOLDIANLBL2
        {
            get { return _RESOLDIANLBL2; }
            set { _RESOLDIANLBL2 = value; }
        }

        private string _TITLE1 = string.Empty;

        public string TITLE1
        {
            get { return _TITLE1; }
            set { _TITLE1 = value; }
        }

        private string _DOTY = string.Empty;

        public string DOTY
        {
            get { return _DOTY; }
            set { _DOTY = value; }
        }

        private string _INVNO = string.Empty;

        public string INVNO
        {
            get { return _INVNO; }
            set { _INVNO = value; }
        }

        private string _INVDAT = string.Empty;

        public string INVDAT
        {
            get { return _INVDAT; }
            set { _INVDAT = value; }
        }

        private string _ORDDAT = string.Empty;

        public string ORDDAT
        {
            get { return _ORDDAT; }
            set { _ORDDAT = value; }
        }

        private string _CAJERO = string.Empty;

        public string CAJERO
        {
            get { return _CAJERO; }
            set { _CAJERO = value; }
        }

        private string _CUSNO = string.Empty;

        public string CUSNO
        {
            get { return _CUSNO; }
            set { _CUSNO = value; }
        }

        private string _INVADDR = string.Empty;

        public string INVADDR
        {
            get { return _INVADDR; }
            set { _INVADDR = value; }
        }

        private string _INVADDR11 = string.Empty;

        public string INVADDR11
        {
            get { return _INVADDR11; }
            set { _INVADDR11 = value; }
        }

        private string _INVADDR12 = string.Empty;

        public string INVADDR12
        {
            get { return _INVADDR12; }
            set { _INVADDR12 = value; }
        }

        private string _INVADDR13 = string.Empty;

        public string INVADDR13
        {
            get { return _INVADDR13; }
            set { _INVADDR13 = value; }
        }

        private string _VENDEDORLBL = string.Empty;

        public string VENDEDORLBL
        {
            get { return _VENDEDORLBL; }
            set { _VENDEDORLBL = value; }
        }

        private string _VENDEDOR = string.Empty;

        public string VENDEDOR
        {
            get { return _VENDEDOR; }
            set { _VENDEDOR = value; }
        }

        private string _INVNODOTY = string.Empty;

        public string INVNODOTY
        {
            get { return _INVNODOTY; }
            set { _INVNODOTY = value; }
        }

        private List<ProductoTicket> _LstProductos = new List<ProductoTicket>();

        public List<ProductoTicket> LstProductos
        {
            get { return _LstProductos; }
            set { _LstProductos = value; }
        }
    }
}
