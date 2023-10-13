using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ds.ModuloComercialService.ServiceContracts;
using Ds.BaseService;
using Ds.DataService;
using Ds.ModuloService.Messages;
using Ds.BusinessService.Entities;
using Ds.BusinessService.Enums;
using Ds.BusinessService.DataTransferObject;
using Ds.BaseService.MessageBase;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Ds.ModuloComercialService.ServiceImplementations
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ModuloService : ServiceBase, IModuloService
    {

        public static IDataService _DataService = new DataService.DataService();

        public getInformacionModulo_Response getInformacionModulo(getInformacionModulo_Request request)
        {
            getInformacionModulo_Response response = new getInformacionModulo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerInformacionModulo(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoModulo oDtoModulo = (DtoModulo)oResultadoOperacion.EntidadDatos;

                response.oDatosModulo = oDtoModulo;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getInformacionFactura_Response getInformacionFactura(getInformacionFactura_Request request)
        {
            getInformacionFactura_Response response = new getInformacionFactura_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerInformacionFactura(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoFactura oDtoFactura = (DtoFactura)oResultadoOperacion.EntidadDatos;

                response.oDtoFactura = oDtoFactura;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getPartesModulo_Response getPartesModulo(getPartesModulo_Request request)
        {
            getPartesModulo_Response response = new getPartesModulo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerInformacionPartesModulo(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoParteModulo> oListaPartes = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

                response.oListaPartesModulo = oListaPartes;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getPartesModulo_Response getPartesF56Modulo(getPartesModulo_Request request)
        {
            getPartesModulo_Response response = new getPartesModulo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerInformacionPartesF56(request.oModulo, request.oTipo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoParteModuloF56> oListaPartes = (List<DtoParteModuloF56>)oResultadoOperacion.ListaEntidadDatos;

                response.oListaPartesModuloF56 = oListaPartes;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getParametrosModulo_Response getParametrosModulo(getParametrosModulo_Request request)
        {
            getParametrosModulo_Response response = new getParametrosModulo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerParametrosModulo(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoParametro> oListaParametros = (List<DtoParametro>)oResultadoOperacion.ListaEntidadDatos;

                response.oListaPartesModulo = oListaParametros;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getValidarSaldosMinimos_Response getValidarSaldosMinimos(getValidarSaldosMinimos_Request request)
        {
            getValidarSaldosMinimos_Response response = new getValidarSaldosMinimos_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarSaldosMinimos(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                TipoValidarSaldosMinimos enumValido = (TipoValidarSaldosMinimos)oResultadoOperacion.EntidadDatos;
                if (enumValido == TipoValidarSaldosMinimos.True)
                {
                    response.validarSaldosMinimos = true;
                    
                }
                else
                {
                    response.validarSaldosMinimos = false;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }


            response.Message = oResultadoOperacion.Mensaje;
            return response;
        }

        public setRegistrarOperacion_Response setRegistrarOperacion(setRegistrarOperacion_Request request)
        {
            setRegistrarOperacion_Response response = new setRegistrarOperacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarTransaccion(request.oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos != string.Empty)
                {
                    response.IdTransaccion = (string)oResultadoOperacion.EntidadDatos;
                }
                else 
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = oResultadoOperacion.Mensaje;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarOperacion_Response setRegistrarArqueo(setRegistrarOperacion_Request request)
        {
            setRegistrarOperacion_Response response = new setRegistrarOperacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarArqueo(request.oArqueo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.IdTransaccion = oResultadoOperacion.EntidadDatos.ToString();
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarOperacion_Response setRegistrarCarga(setRegistrarOperacion_Request request)
        {
            setRegistrarOperacion_Response response = new setRegistrarOperacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarCarga(request.oCarga);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.IdTransaccion = oResultadoOperacion.EntidadDatos.ToString();
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarMovimiento_Response setRegistrarMovimiento(setRegistrarMovimiento_Request request)
        {
            setRegistrarMovimiento_Response response = new setRegistrarMovimiento_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarMovimiento(request.oMovimiento);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                long idOperacion = (long)oResultadoOperacion.EntidadDatos;
                response.lIdMovimiento = idOperacion;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getObtenerSaldosPartes_Response getObtenerSaldosPartes(getObtenerSaldosPartes_Request request)
        {
            getObtenerSaldosPartes_Response response = new getObtenerSaldosPartes_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerSaldoPartes(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.oDtoSaldos = (DtoSaldos)oResultadoOperacion.EntidadDatos;

            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setConfirmarOperacion_Response setConfirmarOperacion(setConfirmarOperacion_Request request)
        {
            setConfirmarOperacion_Response response = new setConfirmarOperacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ConfirmarOperacion(request.oOperacion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoOperacion _operacion = (DtoOperacion)oResultadoOperacion.EntidadDatos;
                response.oDtoOperacion = _operacion;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarTransaccion_Response setRegistrarTransaccionEntrada(setRegistrarTransaccion_Request request)
        {
            setRegistrarTransaccion_Response response = new setRegistrarTransaccion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarTransaccionEntrada(request.oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                long idTransaccion = (long)oResultadoOperacion.EntidadDatos;
                response.lIdTransaccion = idTransaccion;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getObtenerUsuario_Response getObtenerUsuario(getObtenerUsuario_Request request)
        {
            getObtenerUsuario_Response response = new getObtenerUsuario_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerUsuario(request.oUsuario);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.oDtoUsuario = (DtoUsuario)oResultadoOperacion.EntidadDatos;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setGenerarClave_Response setGenerarClave(setGenerarClave_Request request)
        {
            setGenerarClave_Response response = new setGenerarClave_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.GenerarClave(request.oModulo, request.oUsuario);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.bClaveGenerada = true;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bClaveGenerada = false;
            }

            return response;
        }

        public getValidarClave_Response getValidarClave(getValidarClave_Request request)
        {
            getValidarClave_Response response = new getValidarClave_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarClave(request.oIdentificacion, request.sClave);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bUsuarioValido = true;
                    response.sEmpresa = oResultadoOperacion.EntidadDatos.ToString();
                }
                else
                {
                    response.bUsuarioValido = false;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bUsuarioValido = false;
            }            
            return response;
        }

        public getValidarClave_Response getToken(getValidarClave_Request request)
        {
            getValidarClave_Response response = new getValidarClave_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.GenerarToken();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.Token = oResultadoOperacion.EntidadDatos.ToString();
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bUsuarioValido = false;
            }
            return response;
        }

        public getValidarSegundaClave_Response getValidarSegundaClave(getValidarSegundaClave_Request request)
        {
            getValidarSegundaClave_Response response = new getValidarSegundaClave_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarSegundaClave(request.oModulo, request.sClave);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bUsuarioValido = true;
                }
                else
                {
                    response.bUsuarioValido = false;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bUsuarioValido = false;
            }
            return response;
        }

        public setCrearAlarma_Response setCrearAlarma(setCrearAlarma_Request request)
        {
            setCrearAlarma_Response response = new setCrearAlarma_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.CrearAlarma(request.oAlarma);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {

            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setCrearLogWS_Response setCrearLogWS(setCrearLogWS_Request request)
        {
            setCrearLogWS_Response response = new setCrearLogWS_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.InsertarLogWS(request.oLogWS);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {

            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setSolucionarAlarma_Response setSolucionarAlarma(setSolucionarAlarma_Request request)
        {
            setSolucionarAlarma_Response response = new setSolucionarAlarma_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            //ResultadoOperacion oResultadoOperacion = _DataService.LimpiarAlarma(request.oAlarma);
            //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            //{

            //}
            //else
            //{
            //    response.Acknowledge = AcknowledgeType.Failure;
            //    response.Message = oResultadoOperacion.Mensaje;
            //}

            return response;
        }

        public setSolucionarTodasAlarmas_Response setSolucionarTodasAlarmas(setSolucionarTodasAlarmas_Request request)
        {
            setSolucionarTodasAlarmas_Response response = new setSolucionarTodasAlarmas_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.SolucionarTodasAlarmas(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {

            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setInsertarConfirmacion_Response setInsertarConfirmacion(setInsertarConfirmacion_Request request)
        {
            setInsertarConfirmacion_Response response = new setInsertarConfirmacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            //ResultadoOperacion oResultadoOperacion = _DataService.InsertarConfirmarOrdenes(request.tablasInsertar.Tables[0]);
            //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            //{

            //}
            //else
            //{
            //    response.Acknowledge = AcknowledgeType.Failure;
            //    response.Message = oResultadoOperacion.Mensaje;
            //}

            return response;
        }

        //public getDatosCargaActual_Response getDatosCargaActual(getDatosCargaActual_Request request)
        //{
        //    getDatosCargaActual_Response response = new getDatosCargaActual_Response();

        //    if (!ValidRequest(request, response))
        //        return response;

        //    response.CorrelationId = request.RequestId;

        //    ResultadoOperacion oResultadoOperacion = _DataService.ObtenerDatosCargaActual(request.oParametrosTraePagina);
        //    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
        //    {
        //        response.Acknowledge = AcknowledgeType.Failure;
        //        response.Message = oResultadoOperacion.Mensaje;
        //        return response;
        //    }
        //    else
        //    {
        //        response.oListaParteModulo = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;
        //    }

        //    return response;
        //}

        public getIdTransacciones_Response getIdTransacciones(getIdTransacciones_Request request)
        {
            getIdTransacciones_Response response = new getIdTransacciones_Response();

            DataSet oDataSetTransacciones = new DataSet();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            //Obtiene idtransacciones 
            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerIdTransacciones(request.oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.ListaEntidadDatos.Count > 0)
                {
                    response.lstDtoIngresos = (List<DtoIngresos>)oResultadoOperacion.ListaEntidadDatos;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                return response;
            }

            return response;
        }

        public getLogMovimiento_Response getLogMovimiento(getLogMovimiento_Request request)
        {
            getLogMovimiento_Response response = new getLogMovimiento_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            //ResultadoOperacion oResultadoOperacion = _DataService.ObtenerLogMovimiento(request.oLogMovimiento);
            //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            //{
            //    List<DtoLogMovimiento> oListaLogMovimiento = (List<DtoLogMovimiento>)oResultadoOperacion.ListaEntidadDatos;

            //    response.oListaLogMovimiento = oListaLogMovimiento;
            //}
            //else
            //{
            //    response.Acknowledge = AcknowledgeType.Failure;
            //    response.Message = oResultadoOperacion.Mensaje;
            //}

            return response;
        }

        
        
        public getTransaccionesOfflineModulo_Response getTransaccionesOfflineModulo(getTransaccionesOfflineModulo_Request request)
        {
            getTransaccionesOfflineModulo_Response response = new getTransaccionesOfflineModulo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerTransaccionesOfflineModulo(request.oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                
                List<DtoTransacciones> oTransacciones = (List<DtoTransacciones>)oResultadoOperacion.ListaEntidadDatos;

                response.oDtoTransaccionesModulo = oTransacciones;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getPagosOffline_Response getPagosOffline(getPagosOffline_Request request)
        {
            getPagosOffline_Response response = new getPagosOffline_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;
                      
            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerPagosOffline(request.oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.ListaEntidadDatos.Count > 0)
                {
                    response.lstDtoPago = (List<DtoPago>)oResultadoOperacion.ListaEntidadDatos;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                return response;
            }

            return response;
        }

        public getMovimientosOffline_Response getLogMovimientoOffline(getMovimientosOffline_Request request)
        {
            getMovimientosOffline_Response response = new getMovimientosOffline_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerMovimientoOffline(request.oPago);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoLogMovimiento> oListaLogMovimiento = (List<DtoLogMovimiento>)oResultadoOperacion.ListaEntidadDatos;

                response.lstDtoMovimientos = oListaLogMovimiento;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setActualizarRegistro_Response setActualizarRegistro(setActualizarRegistro_Request request)
        {
            setActualizarRegistro_Response response = new setActualizarRegistro_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ActualizarRegistro(request.oPago);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bRegistroActualizado = true;
                }
                else
                {
                    response.bRegistroActualizado = false;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bRegistroActualizado = false;
            }
            return response;
        }

        public getDetalleArqueo_Response getDetalleArqueo(getDetalleArqueo_Request request)
        {
            getDetalleArqueo_Response response = new getDetalleArqueo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerDetalleArqueo(request.oArqueo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoArqueo> oListaDtoArqueo = (List<DtoArqueo>)oResultadoOperacion.ListaEntidadDatos;

                response.lstDtoArqueo = oListaDtoArqueo;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setMonitoreo_Response SetMonitoreo(setMonitoreo_Request request)
        {
            setMonitoreo_Response response = new setMonitoreo_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.Monitoreo(request.oModulo,request.oEstado);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bRegistro = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bRegistro = false;
            }
            return response;
        }


        public setCliente_Response setCliente(setCliente_Request request)
        {
            setCliente_Response response = new setCliente_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarCliente(request.oCelular, request.oEmail);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bRegistro = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bRegistro = false;
            }
            return response;
        }
        public getValidarCliente_Response getValidarCliente(getValidarCliente_Request request)
        {
            getValidarCliente_Response response = new getValidarCliente_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarCliente(request.sCelular);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.sCelular = oResultadoOperacion.EntidadDatos.ToString();
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }
            return response;
        }

        //////////////////////////////////////////////////CITY/////////////////////

        public getCityParking_Response getConsultaValor(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;

                //181.48.146.99:70

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceConsultarValor.svc/consultarValor");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                   "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                   "\"CSN\":\"065ADQ3R\"," +
                                   "\"facial\":\"97901\"," +
                                   "\"consecutivoEntrada\":\"null\"," +
                                   "\"tipoServicio\":\"1\"," +
                                   "\"antipassBack\":\"1\"," +
                                   "\"parkId\":\"70\"," +
                                   "\"zona\":\"1\"," +
                                   "\"puerta\":\"8\"," +
                                   "\"tipoVehiculo\":\"1\"," +
                                   "\"placa\":\"VAT896\"," +
                                   //"\"fechaEntrada\":\"191008233230\"," +
                                   "\"fechaEntrada\":\"" + request.oTarjeta.FechaEntrada + "\"," +
                                   "\"tramaTarifa\":\"400000000000000000000000000001007800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\"," +
                                   "\"idConvenio\":\"0\"," +
                                   "\"banderaPrepago\":\"false\"," +
                                   "\"valorConvenio\":\"0\"," +
                                   "\"serialMaquina\":\"cajero\"}" + "}";

                    //string BANDERA = string.Empty;
                    //if (request.oTarjeta.BanderaPrepago)
                    //{
                    //    BANDERA = "true";
                    //}
                    //else 
                    //{
                    //    BANDERA = "false";
                    //}

                    //string Trama = request.oTarjeta.TramaTarifa.Replace(" ", "");

                    //string json = "{\"objSolicitud\":" +
                    //               "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                    //               "\"CSN\":\"" + request.oTarjeta.IdCard + "\"," +
                    //               "\"facial\":\"" + request.oTarjeta.Facial + "\"," +                                   
                    //               "\"consecutivoEntrada\":\"null\"," +
                    //               "\"tipoServicio\":\"" + request.oTarjeta.TipoServicio + "\"," +
                    //                //"\"antipassBack\":\""+ request.oTarjeta.AntipassBack +"\"," +
                    //               "\"antipassBack\":\"1\"," +
                    //               //"\"parkId\":\"" + request.oTarjeta.ParkId + "\"," +
                    //               "\"parkId\":\"70\"," +
                    //               "\"zona\":\"" + request.oTarjeta.Zona + "\"," +
                    //               "\"puerta\":\"" + request.oTarjeta.Puerta + "\"," +                                   
                    //               "\"tipoVehiculo\":\"" + request.oTarjeta.TipoVehiculo + "\"," +
                    //               "\"placa\":\"" + request.oTarjeta.Placa + "\"," +
                    //               "\"fechaEntrada\":\"" + request.oTarjeta.FechaEntrada + "\"," +                                   
                    //               "\"tramaTarifa\":\""+ Trama +"\"," +
                    //               "\"idConvenio\":\"" + request.oTarjeta.IdConvenio + "\"," +
                    //               "\"banderaPrepago\":\"" + BANDERA + "\"," +
                    //               "\"valorConvenio\":\"" + request.oTarjeta.ValorConvenio + "\"," +
                    //               //"\"serialMaquina\":\"" + request.oTarjeta.IdCajero + "\"}" + "}";
                    //               "\"serialMaquina\":\"cajero\"}" + "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }
        public getCityParking_Response setRegistrarPagoEfectivo(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;

                //181.48.146.99:70

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceRegistrarPagoEfectivo.svc/registrarPagoEfectivo");
                
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                    "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                    "\"idEntrada\":\"" + request.oConsultarResult.IdEntrada + "\"," +
                                    "\"aplicaSticker\":\"false\"," +
                                    "\"placaSalida\":\"" + request.oConsultarResult.PlacaSalida + "\"," +
                                    "\"valorPagado\":\"" + request.oConsultarResult.ValorAPagar + "\"," +
                                    "\"valorServicio\":\"" + request.oConsultarResult.ValorServicio + "\"," +
                                    "\"valorDescuento\":\"" + request.oConsultarResult.ValorDescuento + "\"," +
                                    "\"valorEmpresa\":\"" + request.oConsultarResult.ValorEmpresa + "\"," +
                                    "\"fechaLiquidacion\":\"" + request.oConsultarResult.FechaLiquidacion + "\"," +
                                    "\"serialMaquina\":\"cajero\"}" + "}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }
        public getCityParking_Response getConsultaPagoCelular(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;

                //181.48.146.99:70

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceConsultarPagoCelular.svc/consultarPagoCelular");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                   "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                   "\"idEntrada\":\"" + request.oConsultarResult.IdEntrada + "\"," +
                                   "\"valorAPagar\":\"" + request.oConsultarResult.ValorAPagar + "\"," +
                                   "\"fechaLiquidacion\":\""+ request.oConsultarResult.FechaLiquidacion +"\"," +
                        //"\"serialMaquina\":\"" + request.oConsultarResult.IdCajero + "\"}" + "}";
                                   "\"serialMaquina\":\"cajero\"}" + "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }
        public getCityParking_Response setRegistrarPagoCelular(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;

                //181.48.146.99:70

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceRegistrarPagoCelular.svc/registrarPagoCelular");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceRegistrarPagoCelular.svc/registrarPagoPruebaCelular");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                    "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                    "\"idEntrada\":\"" + request.oConsultarResult.IdEntrada + "\"," +
                                    "\"aplicaSticker\":\"false\"," +
                                    "\"placaSalida\":\"" + request.oConsultarResult.PlacaSalida + "\"," +
                                    "\"valorPagado\":\"" + request.oConsultarResult.ValorAPagar + "\"," +
                                    "\"valorServicio\":\"" + request.oConsultarResult.ValorServicio + "\"," +
                                    "\"valorDescuento\":\"" + request.oConsultarResult.ValorDescuento + "\"," +
                                    "\"valorEmpresa\":\"" + request.oConsultarResult.ValorEmpresa + "\"," +
                                    "\"fechaLiquidacion\":\"" + request.oConsultarResult.FechaLiquidacion + "\"," +
                        //"\"serialMaquina\":\"" + request.oConsultarResult.SerialMaquina + "\"," +
                                    "\"serialMaquina\":\"cajero\"," +
                                    "\"idPago\":\"" + request.oConsultarResult.IdPago + "\"," +
                                    "\"idParqueadero\":\"" + request.oConsultarResult.IdParqueadero + "\"}" + "}";

                    //string json = "{\"objSolicitudPrueba\":" +
                    //                "{\"idPago\":\"28418\"," +
                    //                "\"idParqueadero\":\"192\"}" + "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }
        public getCityParking_Response setRegistrarPagoDatafono(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceRegistrarPagoTarjeta.svc/registrarPagoTarjeta");
                
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                    "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                    "\"idEntrada\":\"" + request.oConsultarResult.IdEntrada + "\"," +
                                    "\"aplicaSticker\":\"false\"," +
                                    "\"placaSalida\":\"" + request.oConsultarResult.PlacaSalida + "\"," +
                                    "\"valorPagado\":\"" + request.oConsultarResult.ValorAPagar + "\"," +
                                    "\"valorServicio\":\"" + request.oConsultarResult.ValorServicio + "\"," +
                                    "\"valorDescuento\":\"" + request.oConsultarResult.ValorDescuento + "\"," +
                                    "\"valorEmpresa\":\"" + request.oConsultarResult.ValorEmpresa + "\"," +
                                    "\"serialMaquina\":\"cajero\"," +
                        //"\"serialMaquina\":\"" + request.oConsultarResult.SerialMaquina + "\"," +
                                    "\"fechaLiquidacion\":\"" + request.oConsultarResult.FechaLiquidacion + "\"," +
                                    "\"respuestaCajas\":\"" + request.oConsultarResult.RespuestaCajas + "\"}" + "}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }
        public getCityParking_Response setRegistrarPagoPrepago(getCityParking_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getCityParking_Response response = new getCityParking_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + request.sIp + "/WcfCajero/ServiceRegistrarPagoSmart.svc/registrarPagoSmart");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"objSolicitud\":" +
                                    "{\"token\":\"9545924352652659EF81EE8B22A93417\"," +
                                    "\"idEntrada\":\"" + request.oConsultarResult.IdEntrada + "\"," +
                                    "\"aplicaSticker\":\"false\"," +
                                    "\"placaSalida\":\"" + request.oConsultarResult.PlacaSalida + "\"," +
                                    "\"valorPagado\":\"" + request.oConsultarResult.ValorAPagar + "\"," +
                                    "\"valorServicio\":\"" + request.oConsultarResult.ValorServicio + "\"," +
                                    "\"valorDescuento\":\"" + request.oConsultarResult.ValorDescuento + "\"," +
                                    "\"valorEmpresa\":\"" + request.oConsultarResult.ValorEmpresa + "\"," +
                                     "\"fechaLiquidacion\":\"" + request.oConsultarResult.FechaLiquidacion + "\"," +
                                    "\"serialMaquina\":\"cajero\"," +
                        //"\"serialMaquina\":\"" + request.oConsultarResult.SerialMaquina + "\"," +
                                    "\"idTipoProducto\":\"" + request.oTarjetaSmart.TipoProducto + "\"," +
                                    "\"CSN\":\"" + request.oTarjetaSmart.IdCard + "\"," +
                                    "\"saldoAnterior\":\"" + request.oTarjetaSmart.SaldoAnterior + "\"," +
                                    "\"facial\":\"" + request.oTarjetaSmart.Facial + "\"," +
                                    "\"identificacion\":\"" + request.oTarjetaSmart.Identificacion + "\"," +
                                    "\"nombreCliente\":\"" + request.oTarjetaSmart.NombreCliente + "\"}" + "}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getDatosConvenio_Response getDatosConvenio(getDatosConvenio_Request request)
        {
            getDatosConvenio_Response response = new getDatosConvenio_Response();

            string NombreConvenio = string.Empty;

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerDatosConvenio(request.idConvenio);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                NombreConvenio = oResultadoOperacion.EntidadDatos.ToString();
                response.sNombreConvenio = NombreConvenio;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarConvenioValidado_Response setRegistrarConvenioValidado(setRegistrarConvenioValidado_Request request)
        {
            setRegistrarConvenioValidado_Response response = new setRegistrarConvenioValidado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarConvenioValidado(request.sConsecutivo, request.sCodigoCompleto,request.sIdModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bRegistro = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bRegistro = false;
            }
            return response;
        }

        public getValidarConvenio_Response getValidarConvenio(getValidarConvenio_Request request)
        {
            getValidarConvenio_Response response = new getValidarConvenio_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarConvenio(request.sCodigo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = false;
                }
                else
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = true;
            }
            return response;
        }

        public getInfoAutorizado_Response getInfoAutorizado(getInfoAutorizado_Request request)
        {
            getInfoAutorizado_Response response = new getInfoAutorizado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerInfoAutorizado(request.oAutorizado);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoAutorizado> olstDtoAutorizado = (List<DtoAutorizado>)oResultadoOperacion.ListaEntidadDatos;
                response.olstDtoAutorizado = olstDtoAutorizado;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getValidarTransaccion_Response getValidarTransaccion(getValidarTransaccion_Request request)
        {
            getValidarTransaccion_Response response = new getValidarTransaccion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarTransaccion(request.sIdTarjeta,request.sFechaEntrada);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.sSecuencia = oResultadoOperacion.EntidadDatos.ToString();
                }
                else
                {
                    response.sSecuencia = string.Empty;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.sSecuencia = string.Empty;
            }
            return response;
        }

        public getFecha_Response getFecha(getFecha_Request request)
        {
            getFecha_Response response = new getFecha_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerFecha();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.oFechaServer = (DateTime)oResultadoOperacion.EntidadDatos;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getFecha_Response getFechaConvenio(getFecha_Request request)
        {
            getFecha_Response response = new getFecha_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerFechaConvenio(request.oIdTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.oFechaServer = (DateTime)oResultadoOperacion.EntidadDatos;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarConvenioAplicado_Response setRegistrarConvenioAplicado(setRegistrarConvenioAplicado_Request request)
        {
            setRegistrarConvenioAplicado_Response response = new setRegistrarConvenioAplicado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarConvenioAplicado(request.IdTransaccion, request.Convenio);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = false;
            }
            return response;
        }

        public setRegistrarConvenioAplicado_Response setRegistrarTiempoGracia(setRegistrarConvenioAplicado_Request request)
        {
            setRegistrarConvenioAplicado_Response response = new setRegistrarConvenioAplicado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarTiempoGracia(request.IdTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = false;
            }
            return response;
        }

        public getTarjetas_Response getTarjetas(getTarjetas_Request request)
        {
            getTarjetas_Response response = new getTarjetas_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerTarjetas(request.oIdEstacionamiento);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoTarjetas> olstDtoTarjetas = (List<DtoTarjetas>)oResultadoOperacion.ListaEntidadDatos;
                response.olstDtoTarjetas = olstDtoTarjetas;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public getIdTransacciones_Response getConsultaPlaca(getIdTransacciones_Request request)
        {
            getIdTransacciones_Response response = new getIdTransacciones_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerResultadoPlaca(request.sLetra1, request.sLetra2);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoTransacciones> olstDtoTransacciones = (List<DtoTransacciones>)oResultadoOperacion.ListaEntidadDatos;
                response.lstDtoTransacciones = olstDtoTransacciones;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public setRegistrarConvenioAplicado_Response setRegistrarPIN(setRegistrarConvenioAplicado_Request request)
        {
            setRegistrarConvenioAplicado_Response response = new setRegistrarConvenioAplicado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarPIN(request.PIN, request.TipoVehiculo,request.IdModulo, request.IdEstacionamiento,request.IdTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = false;
            }
            return response;
        }

        public getValidarConvenio_Response getValidarPIN(getValidarConvenio_Request request)
        {
            getValidarConvenio_Response response = new getValidarConvenio_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ValidarPIN(request.sPIN);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                response.oDtoPIN = (DtoPIN)oResultadoOperacion.EntidadDatos;
                response.Acknowledge = AcknowledgeType.Success;
                response.Message = oResultadoOperacion.Mensaje;
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                
            }
            return response;
        }

        public setRegistrarConvenioAplicado_Response setRegistrarPAGOPIN(setRegistrarConvenioAplicado_Request request)
        {
            setRegistrarConvenioAplicado_Response response = new setRegistrarConvenioAplicado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarPAGOPIN(request.IdTarjeta,request.PIN, request.IdTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = false;
            }
            return response;
        }

        public setRegistrarConvenioAplicado_Response setRegistrarTansaccionEvento(setRegistrarConvenioAplicado_Request request)
        {
            setRegistrarConvenioAplicado_Response response = new setRegistrarConvenioAplicado_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.RegistrarTransaccionEvento(request.IdTransaccion.ToString(), request.IdTarjeta, request.IdEvento);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos.ToString() != string.Empty)
                {
                    response.bResult = true;
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
                response.bResult = false;
            }
            return response;
        }
    }
}
