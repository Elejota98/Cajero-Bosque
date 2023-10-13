using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum StatesDatafono
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
        Inicializarok,
        ErrorIniciar,
        TarjetaLeida,
        ErrorLeerTarjeta,
        ProcesoCredito,
        ProcesoAhorros,
        ErrorRespuestaCredito,
        ErrorRespuestaAhorros,
        IngresoDigitos,
        ErrorIngresoDigitos,
        IngresoCuotas,
        ErrorIngresoCuotas,
        RespuestaAhorros,
        ErrorRespuestaFinal,
        CancelacionTipoCuenta,
        ErrorCancelacionTipoCuenta,
        CancelacionCuotas,
        ErrorCancelacionCuotas,
    }
}
