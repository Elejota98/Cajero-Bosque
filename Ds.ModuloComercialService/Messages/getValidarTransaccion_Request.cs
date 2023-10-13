using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Ds.BaseService.MessageBase;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getValidarTransaccion_Request : RequestBase
    {
        [DataMember]
        public string sIdTarjeta;
        [DataMember]
        public DateTime sFechaEntrada;
    }
}
