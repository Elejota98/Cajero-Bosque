using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoPerfil
    {
        private string _NombreControl;

        public string NombreControl
        {
            get { return _NombreControl; }
            set { _NombreControl = value; }
        }
    }
}
