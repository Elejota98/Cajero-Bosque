using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class setRegistrarConvenioValidado_Request : RequestBase
    {
        [DataMember]
        public string sConsecutivo;

        [DataMember]
        public string sCodigoCompleto;

        [DataMember]
        public string sIdModulo;
    }
}
