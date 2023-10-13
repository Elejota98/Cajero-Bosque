using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Ds.BusinessService.Entities;
using Ds.BaseService.MessageBase;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getInformacionFactura_Request : RequestBase
    {
        [DataMember]
        public Modulo oModulo;
    }
}
