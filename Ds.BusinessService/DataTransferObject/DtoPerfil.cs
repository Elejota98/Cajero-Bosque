using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoPerfil", Namespace = "http://www.eglobalt.com/types/")]
    public class DtoPerfil
    {
        private string _NombreControl;

        [DataMember]
        public string NombreControl
        {
            get { return _NombreControl; }
            set { _NombreControl = value; }
        }
    }
}
