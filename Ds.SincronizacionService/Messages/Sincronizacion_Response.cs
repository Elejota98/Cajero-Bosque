using Ds.BaseService.MessageBase;
using Ds.BusinessService.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.SincronizacionService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class Sincronizacion_Response : ResponseBase
    {
        [DataMember]
        public DtoTransacciones oDtoTransaccion;

        [DataMember]
        public DtoMovimiento oDtoMovimiento;


        [DataMember]
        public DtoArqueo oDtoArqueos;


        [DataMember]
        public string sResult;
    }
}
