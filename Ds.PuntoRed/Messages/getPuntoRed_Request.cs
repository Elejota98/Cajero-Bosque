using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.PuntoRed.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getPuntoRed_Request : RequestBase
    {
        [DataMember]
        public string sValor;
        [DataMember]
        public string sLinea;
        [DataMember]
        public string sTransaccion;
        [DataMember]
        public string sOperador;
        [DataMember]
        public string sCategoria;
        [DataMember]
        public string sPaquete;
        [DataMember]
        public string sSku;
    }
}
