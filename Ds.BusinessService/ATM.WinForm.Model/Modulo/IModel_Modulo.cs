using Ds.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial interface IModel
    {
        ResultadoOperacion ObtenerInfoModulo(Modulo oModulo);
        ResultadoOperacion ObtenerInfoFactura(Modulo oModulo);
        ResultadoOperacion ObtenerPartesModulo(Modulo oModulo);
        ResultadoOperacion ObtenerPartesF56Modulo(Modulo oModulo, int Tipo);
        ResultadoOperacion ObtenerParametrosModulo(Modulo oModulo);
        ResultadoOperacion RegistrarMovimiento(Movimiento oMovimiento);
        ResultadoOperacion RegistrarMovimientoCentral(Movimiento oMovimiento);
        ResultadoOperacion ConfirmarOperacion(Operacion oOperacion);
        ResultadoOperacion RegistrarTransaccion(Transaccion oTransaccion);
        ResultadoOperacion CrearAlarma(Alarma oAlarma);
        ResultadoOperacion CrearAlarmaCentral(Alarma oAlarma);
        ResultadoOperacion SolucionarTodasAlarmas(Modulo oModulo);
        ResultadoOperacion ValidarSaldosMinimos(Modulo oModulo);
        ResultadoOperacion RegistrarOperacion(Transaccion oTransaccion);
        ResultadoOperacion ObtenerUsuario(Usuario oUsuario);
        ResultadoOperacion ValidarClave(long Identificacion, string sClave);
        //List<DtoParteModulo> ObtenerDatosCargaActual(TraePagina oParametrosTraePagina);
        ResultadoOperacion ObtenerIdTransacciones(Transaccion oTransaccion);
        ResultadoOperacion ObtenerIdTransaccionesCentral(Transaccion oTransaccion);
        ResultadoOperacion ObtenerLogMovimiento(LogMovimiento oLogMovimiento);
        ResultadoOperacion ValidarSegundaClave(Modulo oModulo, string sClave);
        ResultadoOperacion GenerarClave(Modulo oModulo, Usuario oUsuario);
        ResultadoOperacion RegistrarOperacionCentral(Operacion oOperacion);
        ResultadoOperacion ConfirmarOperacionCentral(Operacion oOperacion);
        ResultadoOperacion RegistrarArqueo(Arqueo oArqueo);
        ResultadoOperacion ObtenerTransaccionOffline();
        ResultadoOperacion ObtenerPagosOffline(int Id_Transaccion);
        ResultadoOperacion ObtenerMovimientosOffline(int Id_Pago);
        ResultadoOperacion ActualizarRegistro(int Id_Pago);
        ResultadoOperacion RegistrarEstadoModulo(EstadoModulo oModulo, string sUrl);
        ResultadoOperacion RegistrarAccion(Modulo oModulo, string sAccion);
        ResultadoOperacion ObtenerDetalleArqueo(Arqueo oArqueo);
        ResultadoOperacion RegistrarCarga(Carga oCarga);
        ResultadoOperacion ObtenerUsuarioCentral(long Identificacion, string sClave);
        ResultadoOperacion GenerarToken();
        ResultadoOperacion Monitoreo(string Modulo, string Estado);
        ResultadoOperacion ConsultaValor(string sSecuencia, int iTipoVehiculo, bool bMensualidad, bool bReposicion, string Convenios);
        ResultadoOperacion RegistrarPagoEfectivo(string Conexion, ConsultarValorResult oConsultarValorResult);
        ResultadoOperacion ConsultaPagoCelular(string Conexion, ConsultarValorResult oConsultarValorResult);
        ResultadoOperacion RegistrarPagoCelular(string Conexion, ConsultarValorResult oConsultarValorResult);
        ResultadoOperacion RegistrarPagoDatafono(string Conexion, ConsultarValorResult oConsultarValorResult);
        ResultadoOperacion RegistrarPagoPrepago(string Conexion, ConsultarValorResult oConsultarValorResult, TarjetaSmart oTarjetaSmart);
        ResultadoOperacion DatosConvenio(long IdConvenio);
        ResultadoOperacion RegistrarConvenioValidado(string Consecutivo, string CodigoCompleto, string IdModulo);
        ResultadoOperacion ValidarConvenio(string Codigo);
        ResultadoOperacion ObtenerAutorizado(Autorizado oAutorizado);
        ResultadoOperacion ValidarTransaccion(string IdTarjeta, DateTime FechaEntrada);
        ResultadoOperacion RegistrarIngreso(Transaccion oTransaccion);
        ResultadoOperacion ObtenerFechaServer();
        ResultadoOperacion RegistrarConvenio(long IdTransaccion, int Convenio);
        ResultadoOperacion ObtenerFechaConvenio(long IdTransaccion);
        ResultadoOperacion ObtenerTarjetas(long idEstacionamiento);
    }
}
