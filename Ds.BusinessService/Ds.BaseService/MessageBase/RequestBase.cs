using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BaseService.MessageBase
{
    /// <summary>
    ///  Clase base de una petición a un servicio
    /// </summary>
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public class RequestBase
    {
        /// <summary>
        /// Id de la Solicitud
        /// </summary>
        [DataMember]
        public Guid RequestId = new Guid("00000000-0000-0000-0000-000000000000");

        /// <summary>
        /// Usuario que realiza la operación
        /// </summary>
        [DataMember]
        public string Usuario;

    }
}
