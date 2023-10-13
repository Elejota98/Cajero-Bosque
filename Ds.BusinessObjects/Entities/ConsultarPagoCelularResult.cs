using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ConsultarPagoCelularResult
    {
        public object error { get; set; }
        public bool estado { get; set; }
        public string fechaLiquidacion { get; set; }
        public int idPago { get; set; }
        public int idParqueadero { get; set; }
    }
}
