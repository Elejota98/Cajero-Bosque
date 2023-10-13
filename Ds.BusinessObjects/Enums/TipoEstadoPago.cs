using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum TipoEstadoPago
    {
        NoAplica = 0,
        Aprobado = 1,
        Cancelado = 2,
        Error_Dispositivo = 3,
        Error_WebService = 4,
        Error_Dispositivo_NoConfirmaPago = 5,
        ReconteoExitoso = 10,
    }
}
