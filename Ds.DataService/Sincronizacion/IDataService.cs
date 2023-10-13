using Ds.BusinessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataService
{
    public partial interface IDataService
    {
        ResultadoOperacion ObtenerSincronizacionTransaccion();
        ResultadoOperacion ObtenerSincronizacionPagoTransaccion();

        ResultadoOperacion RegistrarSincronizacionTransaccion(Transaccion oTransaccion, string Conexion);
        ResultadoOperacion RegistrarSincronizacionPagoTransaccion(Transaccion oTransaccion, string Conexion);

        ResultadoOperacion ActualizaSincronizacionTransaccion(long Transaccion);
        ResultadoOperacion ActualizaSincronizacionPagoTransaccion(long Transaccion);

        ResultadoOperacion ObtenerCambioSincronizacion();
        ResultadoOperacion ObtenerRecargasSincronizacion();
        ResultadoOperacion ObtenerDonacionSincronizacion();

        ResultadoOperacion RegistrarSincronizacionCambio(Transaccion oTransaccion, string Conexion);
        ResultadoOperacion RegistrarSincronizacionRecargas(Transaccion oTransaccion, string Conexion);
        ResultadoOperacion RegistrarSincronizacionDonacion(Transaccion oTransaccion, string Conexion);

        ResultadoOperacion ActualizaSincronizacionCambio(long Transaccion);
        ResultadoOperacion ActualizaSincronizacionRecargas(long Transaccion);
        ResultadoOperacion ActualizaSincronizacionDonacion(long Transaccion);

        ResultadoOperacion ObtenerArqueoSincronizacion();
        ResultadoOperacion RegistrarSincronizacionArqueo(Arqueo oArqueos, string Conexion);
        ResultadoOperacion ActualizaSincronizacionArqueo(long Arqueo);

        ResultadoOperacion ObtenerCargaSincronizacion();
        ResultadoOperacion RegistrarSincronizacionCarga(Carga oCargas, string Conexion);
        ResultadoOperacion ActualizaSincronizacionCarga(long Carga);

        ResultadoOperacion ObtenerMovimientoSincronizacion();
        ResultadoOperacion RegistrarSincronizacionMovimiento(Movimiento oMovimiento, string Conexion);
        ResultadoOperacion ActualizaSincronizacionMovimiento(long Movimiento);

        ResultadoOperacion ObtenerPartesSincronizacion();
        ResultadoOperacion ActualizaSincronizacionParte(string IdModulo, long IdSede, string NombreParte, int Denominacion, double DineroActual, int CantidadActual, string Conexion);
        ResultadoOperacion ActualizaSincronizacionParteLocal(string IdModulo, long IdSede, string NombreParte, int Denominacion, double DineroActual, int CantidadActual);
    }
}
