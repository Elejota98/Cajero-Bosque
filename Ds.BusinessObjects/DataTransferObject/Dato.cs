using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class Dato
    {
        public string codigoProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string codigoCategoria { get; set; }
        public string descripcion { get; set; }
        public string codigoPaquete { get; set; }
        public string valor { get; set; }
        public string sku { get; set; }
    }
}
