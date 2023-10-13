using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceModulo", Namespace = "http://www.eglobalt.com/types/")]
    public class Modulo
    {
        private string _ID_Modulo;
        private List<ParteModulo> _Partes = new List<ParteModulo>();

        [DataMember]
        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        [DataMember]
        public List<ParteModulo> Partes
        {
            get { return _Partes; }
            set { _Partes = value; }
        }

    }
}
