using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoAccion
    {
        private string _NombreAccion;

        public string NombreAccion
        {
            get { return _NombreAccion; }
            set { _NombreAccion = value; }
        }
    }
}
