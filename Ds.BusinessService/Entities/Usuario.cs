using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceUsuario", Namespace = "http://www.eglobalt.com/types/")]
    public class Usuario
    {
        private string _IdCriptUsuario;

        private long _IdUsuario;

        [DataMember]
        public string IdCriptUsuario
        {
            get { return _IdCriptUsuario; }
            set { _IdCriptUsuario = value; }
        }

        [DataMember]
        public long IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

    }
}
