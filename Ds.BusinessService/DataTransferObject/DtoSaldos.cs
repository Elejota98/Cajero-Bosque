using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoSaldos", Namespace = "http://www.dsystem.co/types/")]
    public class DtoSaldos
    {
        private string _NombreParte;


        private int _CantMin;


        private int _CantActual;

        private List<DtoParteModulo> _DtoPartes = new List<DtoParteModulo>();

        [DataMember]
        public List<DtoParteModulo> DtoPartes
        {
            get { return _DtoPartes; }
            set { _DtoPartes = value; }
        }
     

        [DataMember]
        public string NombreParte
        {
            get { return _NombreParte; }
            set { _NombreParte = value; }
        }

        [DataMember]
        public int CantMin
        {
            get { return _CantMin; }
            set { _CantMin = value; }
        }

        [DataMember]
        public int CantActual
        {
            get { return _CantActual; }
            set { _CantActual = value; }
        }
    }
}
