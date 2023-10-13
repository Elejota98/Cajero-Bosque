using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoIngresos", Namespace = "http://www.eglobalt.com/types/")]
    public class DtoIngresos
    {
        private string _Status;
        private string _Codigo;


        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [DataMember]
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

    }
}
