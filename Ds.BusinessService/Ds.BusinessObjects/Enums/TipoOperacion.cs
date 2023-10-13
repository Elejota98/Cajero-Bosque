using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum TipoOperacion
    {
        Carga = 1,
        ArqueoParcial = 2,
        ArqueoTotal = 3,
        Pago = 4,
        Reconteo = 5,
        CerrarAplicacion = 6,
        NoAplica = 7,
        ArqueoPersonalizado = 8,
        Mantenimiento = 9,
        Recarga = 10,
        Donacion = 11,
        CambioDona = 12,
        Mensualidad = 13,
        Reposicion = 14,
        CobroTarjetaMensual = 15,
    }
}
