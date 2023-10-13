using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getFecha_Request : RequestBase
    {
        [DataMember]
        public long oIdTransaccion;
    }
}
