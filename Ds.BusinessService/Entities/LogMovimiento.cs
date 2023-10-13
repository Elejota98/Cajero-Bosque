using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceLogMovimiento", Namespace = "http://www.dsystem.co/types/")]
    public class LogMovimiento
    {       
        private long _Id;
        private int _Parte;
       

        [DataMember]
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [DataMember]
        public int Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }       
    }
}
