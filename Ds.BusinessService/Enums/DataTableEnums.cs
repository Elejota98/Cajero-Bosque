using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Enums
{
    public enum InfoModulo_DB : int
    {
        ID_Modulo = 0,
        ID_Sede = 1,
        Clave = 2,
        Libre = 3,
        Activo = 4,
        Almacen = 6,
        Maquina = 7,
        Direccion = 8,
        Ciudad = 2,
        NumeroResolucion = 9,
        Prefijo = 10,
        FacturaIni = 11,
        FacturaFin = 12,
        FechaResolucion = 13,
    }

    public enum InfoParte_DB : int
    {
        TipoParte = 0,
        Nombre = 1,
        Denominacion = 2,
        CantidadMin = 3,
        CantidadAlarma = 4,
        Prioridad = 5,
        DineroActual = 6,
        CantidadActual = 7,
        NumParte = 8,
        CantidadArqPer = 9,
    }

    public enum InfoUsuario_DB : int
    {
        Password = 0,
        IdUsuario = 1,
    }

    public enum Operacion_DB : int
    {
        Status = 0,
        Code = 1,
        Factura = 2,
    }

    public enum InfoArqueo_DB : int
    {
        Status = 0,
        IdArqueo = 1,
        Producido = 2,
        Valor = 3,
    }

    public enum InfoCarga_DB : int
    {
        Status = 0,
        
    }

    public enum SaldoParte_DB : int
    {
        Tipo_Parte = 0,
        Cantidad = 1,
    }

    public enum Operacion_Result : int
    {
        OK = 1,
        ERROR = 2,
    }
}
