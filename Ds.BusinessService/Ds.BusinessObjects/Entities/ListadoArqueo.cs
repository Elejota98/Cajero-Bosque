using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ListadoArqueo
    {
        private List<Items> _LstItems = new List<Items>();

        public List<Items> LstItems
        {
            get { return _LstItems; }
            set { _LstItems  = value; }
        }
    }
}
