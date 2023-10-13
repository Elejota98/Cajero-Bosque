using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoIngresos
    {
        private string _Status;
        private string _Codigo;



        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
    }
}
