using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoOperacion
    {
        private string _ID_Modulo = string.Empty;
        private string _ID_Usuario = string.Empty;
        private string _Factura = string.Empty;

        public string Factura
        {
            get { return _Factura; }
            set { _Factura = value; }
        }
        private string _CodigoBarras = string.Empty;

        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }
        private long _ID_Transaccion = 0;
        private TipoOperacion _TipoOperacion = TipoOperacion.NoAplica;
        private long _ID_Operacion = 0;
        private string _ID_Fake_Operacion = string.Empty;
        private DtoPago _DtoPago = new DtoPago();
        private DtoArqueo _DtoArqueo = new DtoArqueo();

        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        public string ID_Usuario
        {
            get { return _ID_Usuario; }
            set { _ID_Usuario = value; }
        }

        public long ID_Transaccion
        {
            get { return _ID_Transaccion; }
            set { _ID_Transaccion = value; }
        }

        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

        public string ID_Fake_Operacion
        {
            get { return _ID_Fake_Operacion; }
            set { _ID_Fake_Operacion = value; }
        }

        public DtoPago DtoPago
        {
            get { return _DtoPago; }
            set { _DtoPago = value; }
        }

        public DtoArqueo DtoArqueo
        {
            get { return _DtoArqueo; }
            set { _DtoArqueo = value; }
        }
    }
}
