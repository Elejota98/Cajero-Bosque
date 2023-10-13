using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds.BusinessObjects.Entities;

namespace Ds.ServiceProxy
{
    public partial interface IProxyService
    {
        ResultadoOperacion ObtenerInformacionModulo(Modulo oModulo);
        ResultadoOperacion ObtenerPartesModulo(Modulo oModulo);
        ResultadoOperacion ObtenerPartesF56Modulo(Modulo oModulo, int Tipo);
        ResultadoOperacion ObtenerParametrosModulo(Modulo oModulo);
        ResultadoOperacion ValidarSaldosMinimos(Modulo oModulo);
        ResultadoOperacion RegistrarOperacion(Transaccion oTransaccion);
        ResultadoOperacion RegistrarArqueo(Arqueo oArqueo);
        ResultadoOperacion RegistrarCarga(Carga oCarga);
        ResultadoOperacion RegistrarMovimiento(Movimiento oMovimiento);
        ResultadoOperacion ObtenerSaldosPartes(Modulo oModulo);
        ResultadoOperacion ConfirmarOperacion(Operacion oOperacion);
        ResultadoOperacion RegistrarTransaccion(Transaccion oTransaccion);
        ResultadoOperacion CrearAlarma(Alarma oAlarma);
        ResultadoOperacion SolucionarTodasAlarmas(Modulo oModulo);
        ResultadoOperacion CrearLogWS(LogWS oLogWS);
        ResultadoOperacion ObtenerUsuario(Usuario oUsuario);
        ResultadoOperacion InsertarConfirmacionOrdenes(DataTable tTablaInsertar);
        //ResultadoOperacion ObtenerDatosCargaActual(TraePagina oParametrosTraePagina);
        ResultadoOperacion ValidarClave(long Identificacion, string sClave);
        ResultadoOperacion ObtenerIdTransacciones(Transaccion oTransaccion);
        ResultadoOperacion ObtenerLogMovimiento(LogMovimiento oLogMovimiento);
        ResultadoOperacion ValidarSegundaClave(Modulo oModulo, string sClave);
        ResultadoOperacion GenerarClave(Modulo oModulo, Usuario oUsuario);
        ResultadoOperacion ObtenerTransaccionesOfflineModulo();
        ResultadoOperacion ObtenerPagosOffline(int oTransaccion);
        ResultadoOperacion ObtenerMovimientosOffline(int oPago);
        ResultadoOperacion ActualizarRegistro(int oPago);
        ResultadoOperacion ObtenerDetalleArqueo(Arqueo oArqueo);
        ResultadoOperacion ObtenerInformacionFactura(Modulo oModulo);
        ResultadoOperacion GenerarToken();
        ResultadoOperacion Monitoreo(string Modulo, string Estado);

        ResultadoOperacion ConsultaValor(string sSecuencia, int iTipoVehiculo, bool bMensualidad, bool bReposicion, string Convenios, int TipoTarifa);
        ResultadoOperacion RegistrarPagoEfectivo(string Conexion, ConsultarValorResult oConsultarValorResult);
        ResultadoOperacion ObtenerDatosConvenio(long IdConvenio);
        ResultadoOperacion RegistrarConvenioValidado(string Consecutivo, string CodigoCompleto, string IdModulo);
        ResultadoOperacion ValidarConvenio(string sCodigo);
        ResultadoOperacion ObtenerAutorizado(Autorizado oAutorizado);
        ResultadoOperacion ValidarTransaccion(string IdTarjeta, DateTime FechaEntrada);
        ResultadoOperacion RegistrarIngreso(Transaccion oTransaccion);
        ResultadoOperacion ObtenerFechaServer();
        ResultadoOperacion RegistrarConvenio(long IdTransaccion, int Convenio);
        ResultadoOperacion RegistrarTiempoGracia(long IdTransaccion);
        ResultadoOperacion ObtenerFechaConvenio(long IdTransaccion);
        ResultadoOperacion ObtenerTarjetas(long idEstacionamiento);
        ResultadoOperacion ObtenerDatosPlaca(string Letra1, string Letra2);
        ResultadoOperacion RegistrarPIN(string PIN, int TipoVehiculo, string IdModulo, long IdEstacionamiento, long IdTransaccion);
        ResultadoOperacion ValidarPIN(long PIN);
        ResultadoOperacion RegistrarPAGOPIN(string IdTransaccion, string PIN, long Secuencia);
        ResultadoOperacion RegistrarEvento(long IdTransaccion, string IdTarjeta, long IdEvento);
    }
}
