using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoOperacion", Namespace = "http://www.dsystem.co/types/")]
    public class DtoOperacion
    {
        private string _ID_Modulo = string.Empty;
        private string _ID_Usuario = string.Empty;
        private long _ID_Transaccion = 0;
        private TipoOperacion _TipoOperacion = TipoOperacion.NoAplica;
        private long _ID_Operacion = 0;
        private string _ID_Fake_Operacion = string.Empty;
        private DtoPago _DtoPago = new DtoPago();
        private DtoArqueo _DtoArqueo = new DtoArqueo();
        private DtoCarga _DtoCarga = new DtoCarga();

       
        [DataMember]
        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        [DataMember]
        public string ID_Usuario
        {
            get { return _ID_Usuario; }
            set { _ID_Usuario = value; }
        }

        [DataMember]
        public long ID_Transaccion
        {
            get { return _ID_Transaccion; }
            set { _ID_Transaccion = value; }
        }

        [DataMember]
        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

        [DataMember]
        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

        [DataMember]
        public string ID_Fake_Operacion
        {
            get { return _ID_Fake_Operacion; }
            set { _ID_Fake_Operacion = value; }
        }

        [DataMember]
        public DtoPago DtoPago
        {
            get { return _DtoPago; }
            set { _DtoPago = value; }
        }

        [DataMember]
        public DtoArqueo DtoArqueo
        {
            get { return _DtoArqueo; }
            set { _DtoArqueo = value; }
        }
        [DataMember]
        public DtoCarga DtoCarga
        {
            get { return _DtoCarga; }
            set { _DtoCarga = value; }
        }

    }
}
