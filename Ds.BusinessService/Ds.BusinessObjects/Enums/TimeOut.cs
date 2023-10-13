using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum TimeOut
    {
        /// <summary>
        /// TimeOut en Segundos
        /// </summary>
        TimeOut_CondicionesRestricciones = 150,
        TimeOut_TransaccionCancelada = 5,
        TimeOut_RetiroRecibo = 8,
        TimeOut_ContraseñaIncorrecta = 5,
        TimeOut_ConteoMonedas = 40,
        TimeOut_Video = 64,
        TimeOut_IngresoPass = 10,
        TimeOut_Sincronizacion = 10,
        TimeOut_Publicidad0 = 7,
        TimeOut_Publicidad1 = 20,
        TimeOut_Publicidad2 = 30,
        TimeOut_Publicidad3 = 40,
        TimeOut_Flecha1 = 1,
        TimeOut_Flecha2 = 6,
        TimeOut_Flujo = 30,
        TimeOut_Smart = 60,
    }
}
