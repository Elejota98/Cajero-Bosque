using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum StatesAzkDevice
    {
        ConexionExitosa,
        ErrorConexion,
        Nothing,
        Reset,
        Test,
        TestHopper1,
        TestHopper2,
        TestHopper3,
        TestHopper4,
        Habilitar,
        HabilitarMonedas,
        HabilitarRecepcion,
        DesHabilitarRecepcion,
        Pagar,
        Hopper1Ready,
        Hopper2Ready,
        Hopper3Ready,
        Hopper4Ready,
        Hopper1Habilitado,
        Hopper2Habilitado,
        Hopper3Habilitado,
        Hopper4Habilitado,
        ErrorConexionHopper,
        Validador,
        EstadoValidador,
        ValidadorReady,

    }
    public enum TipoInsertEvento
    {
        Ninguno,
        Envio,
        Recepcion,
    }
}
