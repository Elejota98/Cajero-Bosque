using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ConsultarValorResult
    {
        public object error { get; set; }
        public bool estado { get; set; }
        public string fechaLiquidacion { get; set; }
        public int idEntrada { get; set; }
        public string nombreTarifa { get; set; }
        public string permanencia { get; set; }
        public bool recobro { get; set; }
        public double valorAPagar { get; set; }
        public int valorDescuento { get; set; }
        public int valorEmpresa { get; set; }
        public double valorServicio { get; set; }
        public string serialMaquina { get; set; }
        public string _IdPago { get; set; }
        public string _IdParqueadero { get; set; }
        public string _PlacaSalida { get; set; }
        public string RespuestaCajas { get; set; }
    }
}
