using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class setRegistrarOperacion_Request : RequestBase
    {
        [DataMember]
        public Operacion oOperacion;

        [DataMember]
        public Transaccion oTransaccion;

        [DataMember]
        public Arqueo oArqueo;

        [DataMember]
        public Carga oCarga;

        [DataMember]
        public DataTable Data;
    }
}
