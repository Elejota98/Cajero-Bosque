using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoAccion", Namespace = "http://www.dsystem.co/types/")]
    public class DtoAccion
    {
        private string _NombreAccion;

        [DataMember]
        public string NombreAccion
        {
            get { return _NombreAccion; }
            set { _NombreAccion = value; }
        }
    }
}
