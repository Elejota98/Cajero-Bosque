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
    public class getDetalleArqueo_Request : RequestBase
    {
        [DataMember]
        public Arqueo oArqueo;
    }
}
