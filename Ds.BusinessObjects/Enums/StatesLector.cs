using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum StatesLector
    {
        Conexion_Exitosa,
        Error_Conexion,
        Desconexion_Exitosa,
        ObtenerIdTarjeta,
        ErrorObtenerIdTarjeta,
        CheckPassOK,
        CheckPassError,
        LecturaOK,
        LecturaError,
        ExpulsarOK,
        ExpulsarError,
        NoCard,
        CardIn,
        StatusError,
        ErrorEscribir,
        EscribirOK,
    }
}
