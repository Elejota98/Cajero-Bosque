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
    public class getPartesModulo_Response : ResponseBase
    {
        [DataMember]
        public List<DtoParteModulo> oListaPartesModulo;

        [DataMember]
        public List<DtoParteModuloF56> oListaPartesModuloF56;
    }
}
