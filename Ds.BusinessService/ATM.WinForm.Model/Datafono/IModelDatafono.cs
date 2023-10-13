using Ds.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial interface IModel
    {
        ResultadoOperacion IniciarDatafono(string sPathArchivoRespuesta, string sNombreArchivoRespuesta, string sArchivoSolicitud, string sArchivoCajero);
        ResultadoOperacion PagarDatafono(int iOperacion, Int64 iMonto, Int64 iIva, string sFactura, Int64 iBaseDev, Int64 iImpConsumidor, string sCodigoCajero);
        ResultadoOperacion AnularPagoDatafono(int Operacion, string Recibo, string Factura, string Clave, string CodigoCajero);
    }
}
