using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Enums
{
    public enum TipoOperacion
    {
        Carga = 4,
        ArqueoParcial = 10,
        ArqueoTotal = 11,
        Pago = 1,
        Reconteo = 19,
        CerrarAplicacion = 18,
        NoAplica = 14,
        ArqueoPersonalizado = 17,
        Mantenimiento = 9,
        Recarga = 12,
        Donacion = 13,
        CambioDona = 12,
        Mensualidad = 2,
        Reposicion = 3,
        CobroTarjetaMensual = 8,
        Casco = 6,
        Evento = 5,
        Datafono = 7,
    }
}
