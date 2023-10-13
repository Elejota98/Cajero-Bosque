using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Ds.BaseService.MessageBase;
using Ds.BusinessService.DataTransferObject;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getObtenerSaldosPartes_Response : ResponseBase
    {
        [DataMember]
        public DtoSaldos oDtoSaldos;
    }
}
