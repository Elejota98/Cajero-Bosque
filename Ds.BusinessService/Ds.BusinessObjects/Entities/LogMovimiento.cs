using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class LogMovimiento
    {
        private long _Id;
        private int _Parte;

        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }       
    }
}
