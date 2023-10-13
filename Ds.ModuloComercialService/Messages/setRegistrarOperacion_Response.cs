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
    public class setRegistrarOperacion_Response : ResponseBase
    {
        [DataMember]
        public DtoOperacion oDtoOperacion;

        [DataMember]
        public DtoArqueo oDtoArqueo;

        [DataMember]
        public DtoCarga oDtoCarga;

        [DataMember]
        public string IdTransaccion;

        [DataMember]
        public bool bInsercionExitosa;
    }
}
