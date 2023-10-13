using Ds.BaseService.MessageBase;
using Ds.BusinessService.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.ModuloService.Messages
{
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class getTarjetas_Response : ResponseBase
    {
        [DataMember]
        public List<DtoTarjetas> olstDtoTarjetas;
    }
}
