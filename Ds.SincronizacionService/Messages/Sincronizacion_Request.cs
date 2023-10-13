using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.SincronizacionService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class Sincronizacion_Request : RequestBase
    {
        [DataMember]
        public string sConexion = string.Empty;

        [DataMember]
        public Transaccion oTransaccion;

        [DataMember]
        public Pago oPago;

        [DataMember]
        public Movimiento oMovimientos;

        [DataMember]
        public Arqueo oArqueos;

        [DataMember]
        public Carga oCarga;

    }
}
