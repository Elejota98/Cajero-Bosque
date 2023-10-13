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
    public class setGenerarClave_Request : RequestBase
    {
        [DataMember]
        public Modulo oModulo;

        [DataMember]
        public Usuario oUsuario;
    }
}
