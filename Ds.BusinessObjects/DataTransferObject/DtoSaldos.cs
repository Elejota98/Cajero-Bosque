using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoSaldos
    {
        private List<DtoParteModulo> _DtoPartes = new List<DtoParteModulo>();

        public List<DtoParteModulo> DtoPartes
        {
            get { return _DtoPartes; }
            set { _DtoPartes = value; }
        }

    }
}
