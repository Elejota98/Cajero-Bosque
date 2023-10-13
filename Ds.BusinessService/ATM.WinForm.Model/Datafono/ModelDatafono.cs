using Ds.BusinessObjects.Entities;
using Ds.Datafono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial class Model : IModel
    {
        public ResultadoOperacion IniciarDatafono(string sPathArchivoRespuesta, string sNombreArchivoRespuesta, string sArchivoSolicitud, string sArchivoCajero)
        {
            return Datafono.Instancia.IniciarDatafono(sPathArchivoRespuesta, sNombreArchivoRespuesta, sArchivoSolicitud, sArchivoCajero);
        }

        public ResultadoOperacion PagarDatafono(int iOperacion, Int64 iMonto, Int64 iIva, string sFactura, Int64 iBaseDev, Int64 iImpConsumidor, string sCodigoCajero)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            return Datafono.Instancia.PagarDatafono(iOperacion, iMonto, iIva, sFactura, iBaseDev, iImpConsumidor, sCodigoCajero);
        }

        //<Operación>,<Recibo>,<factura>,<Clave>,<cod_cajero>
        public ResultadoOperacion AnularPagoDatafono(int Operacion, string Recibo, string Factura, string Clave, string CodigoCajero)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            return Datafono.Instancia.AnularPagoDatafono(Operacion, Recibo, Factura, Clave, CodigoCajero);
        }
    }
}
