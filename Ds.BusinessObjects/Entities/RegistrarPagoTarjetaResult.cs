using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class RegistrarPagoTarjetaResult
    {
        public object cadenaQR { get; set; }
        public object error { get; set; }
        public bool estado { get; set; }
        public string factura { get; set; }
        public string fechaHoraMaximaSalida { get; set; }
    }
}
