using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EGlobalT_ATM.BusinessService.Entities
{
    [DataContract(Name = "ServiceDenominacion", Namespace = "http://www.eglobalt.com/types/")]
    public class Denominacion
    {
        private int _Den50;

        
        private int _100;
        private int _200;
        private int _500;
        private int _1000;

        [DataMember]
        public int Den50
        {
            get { return _Den50; }
            set { _Den50 = value; }
        }
       
    }
}
