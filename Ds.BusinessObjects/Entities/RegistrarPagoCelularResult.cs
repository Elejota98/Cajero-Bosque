using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class RegistrarPagoCelularResult
    {
        public object cadenaQR { get; set; }
        public string error { get; set; }
        public bool estado { get; set; }
        public int estadoPagoCelular { get; set; }
        public object factura { get; set; }
        public string fechaHoraMaximaSalida { get; set; }
    }
}
