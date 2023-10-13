using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BaseService.MessageBase
{
    /// <summary>
    /// Enumerador encargado de indicar el estado de respuesta de un servicio
    /// </summary>
    [DataContract(Namespace = "http://www.dsystem.co/types/")]
    public enum AcknowledgeType
    {
        /// <summary>
        /// Representa respuesta fallida.
        /// </summary>
        [EnumMember]
        Failure = 0,

        /// <summary>
        /// Representa respuesta satisfactoria
        /// </summary>
        [EnumMember]
        Success = 1
    }
}
