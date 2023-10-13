using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum EstadoImpresora
    {
        PuertoCerrado = 1,
        ModoDispositivoInvalido = 2,
        ErrorLecturaEstadoDispositivo = 3,
        ErrorDesconocido = 4,

        AtascamientoPapel = 10,
        ImpresoraSinPapel = 11,
        ImpresoraSinRolloPapel = 12,
        NivelBajoPapel = 13,
        EstadoDesconocido = 14,
        ImpresoraDesconectada = 122,
        OK = 15,
    }
}
