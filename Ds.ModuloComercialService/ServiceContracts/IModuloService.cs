using Ds.ModuloService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ds.ModuloComercialService.ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IModuloService
    {

        [OperationContract]
        getInformacionModulo_Response getInformacionModulo(getInformacionModulo_Request request);

        [OperationContract]
        getInformacionFactura_Response getInformacionFactura(getInformacionFactura_Request request);

        [OperationContract]
        getPartesModulo_Response getPartesModulo(getPartesModulo_Request request);

        [OperationContract]
        getPartesModulo_Response getPartesF56Modulo(getPartesModulo_Request request);

        [OperationContract]
        getParametrosModulo_Response getParametrosModulo(getParametrosModulo_Request request);

        [OperationContract]
        getValidarSaldosMinimos_Response getValidarSaldosMinimos(getValidarSaldosMinimos_Request request);

        [OperationContract]
        setRegistrarOperacion_Response setRegistrarOperacion(setRegistrarOperacion_Request request);

        [OperationContract]
        setRegistrarOperacion_Response setRegistrarArqueo(setRegistrarOperacion_Request request);

        [OperationContract]
        setRegistrarOperacion_Response setRegistrarCarga(setRegistrarOperacion_Request request);

        [OperationContract]
        setRegistrarMovimiento_Response setRegistrarMovimiento(setRegistrarMovimiento_Request request);

        [OperationContract]
        getObtenerSaldosPartes_Response getObtenerSaldosPartes(getObtenerSaldosPartes_Request request);

        [OperationContract]
        setConfirmarOperacion_Response setConfirmarOperacion(setConfirmarOperacion_Request request);

        //[OperationContract]
        //setRegistrarTransaccion_Response setRegistrarTransaccion(setRegistrarTransaccion_Request request);

        [OperationContract]
        getObtenerUsuario_Response getObtenerUsuario(getObtenerUsuario_Request request);

        [OperationContract]
        setGenerarClave_Response setGenerarClave(setGenerarClave_Request request);

        [OperationContract]
        getValidarClave_Response getValidarClave(getValidarClave_Request request);

        [OperationContract]
        getValidarClave_Response getToken(getValidarClave_Request request);

        [OperationContract]
        setCrearAlarma_Response setCrearAlarma(setCrearAlarma_Request request);

        [OperationContract]
        setSolucionarAlarma_Response setSolucionarAlarma(setSolucionarAlarma_Request request);

        [OperationContract]
        setSolucionarTodasAlarmas_Response setSolucionarTodasAlarmas(setSolucionarTodasAlarmas_Request request);

        [OperationContract]
        setCrearLogWS_Response setCrearLogWS(setCrearLogWS_Request request);

        [OperationContract]
        setInsertarConfirmacion_Response setInsertarConfirmacion(setInsertarConfirmacion_Request request);

        //[OperationContract]
        //getDatosCargaActual_Response getDatosCargaActual(getDatosCargaActual_Request request);

        [OperationContract]
        getIdTransacciones_Response getIdTransacciones(getIdTransacciones_Request request);

        [OperationContract]
        getLogMovimiento_Response getLogMovimiento(getLogMovimiento_Request request);

        [OperationContract]
        getValidarSegundaClave_Response getValidarSegundaClave(getValidarSegundaClave_Request request);

        [OperationContract]
        getTransaccionesOfflineModulo_Response getTransaccionesOfflineModulo(getTransaccionesOfflineModulo_Request request);

        [OperationContract]
        getPagosOffline_Response getPagosOffline(getPagosOffline_Request request);

        [OperationContract]
        getMovimientosOffline_Response getLogMovimientoOffline(getMovimientosOffline_Request request);

        [OperationContract]
        setActualizarRegistro_Response setActualizarRegistro(setActualizarRegistro_Request request);

        [OperationContract]
        getDetalleArqueo_Response getDetalleArqueo(getDetalleArqueo_Request request);

        [OperationContract]
        setMonitoreo_Response SetMonitoreo(setMonitoreo_Request request);

        [OperationContract]
        setCliente_Response setCliente(setCliente_Request request);

        [OperationContract]
        getValidarCliente_Response getValidarCliente(getValidarCliente_Request request);

        [OperationContract]
        getCityParking_Response getConsultaValor(getCityParking_Request request);

        [OperationContract]
        getCityParking_Response getConsultaPagoCelular(getCityParking_Request request);

        [OperationContract]
        getCityParking_Response setRegistrarPagoCelular(getCityParking_Request request);

        [OperationContract]
        getCityParking_Response setRegistrarPagoEfectivo(getCityParking_Request request);

        [OperationContract]
        getCityParking_Response setRegistrarPagoDatafono(getCityParking_Request request);

        [OperationContract]
        getCityParking_Response setRegistrarPagoPrepago(getCityParking_Request request);

        [OperationContract]
        getDatosConvenio_Response getDatosConvenio(getDatosConvenio_Request request);

        [OperationContract]
        setRegistrarConvenioValidado_Response setRegistrarConvenioValidado(setRegistrarConvenioValidado_Request request);

        [OperationContract]
        getValidarConvenio_Response getValidarConvenio(getValidarConvenio_Request request);

        [OperationContract]
        getInfoAutorizado_Response getInfoAutorizado(getInfoAutorizado_Request request);

        [OperationContract]
        getValidarTransaccion_Response getValidarTransaccion(getValidarTransaccion_Request request);

        [OperationContract]
        setRegistrarTransaccion_Response setRegistrarTransaccionEntrada(setRegistrarTransaccion_Request request);

        [OperationContract]
        getFecha_Response getFecha(getFecha_Request request);

        [OperationContract]
        setRegistrarConvenioAplicado_Response setRegistrarConvenioAplicado(setRegistrarConvenioAplicado_Request request);

        [OperationContract]
        setRegistrarConvenioAplicado_Response setRegistrarTiempoGracia(setRegistrarConvenioAplicado_Request request);

        [OperationContract]
        getFecha_Response getFechaConvenio(getFecha_Request request);

        [OperationContract]
        getTarjetas_Response getTarjetas(getTarjetas_Request request);

        [OperationContract]
        getIdTransacciones_Response getConsultaPlaca(getIdTransacciones_Request request);

        [OperationContract]
        setRegistrarConvenioAplicado_Response setRegistrarPIN(setRegistrarConvenioAplicado_Request request);

        [OperationContract]
        getValidarConvenio_Response getValidarPIN(getValidarConvenio_Request request);

        [OperationContract]
        setRegistrarConvenioAplicado_Response setRegistrarPAGOPIN(setRegistrarConvenioAplicado_Request request);

        [OperationContract]
        setRegistrarConvenioAplicado_Response setRegistrarTansaccionEvento(setRegistrarConvenioAplicado_Request request);
    }
}
