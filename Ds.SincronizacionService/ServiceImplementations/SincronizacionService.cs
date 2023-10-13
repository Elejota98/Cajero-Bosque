using Ds.BaseService;
using Ds.BaseService.MessageBase;
using Ds.BusinessService.DataTransferObject;
using Ds.BusinessService.Entities;
using Ds.BusinessService.Enums;
using Ds.DataService;
using Ds.SincronizacionService.Messages;
using Ds.SincronizacionService.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ds.SincronizacionService.ServiceImplementations
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SincronizacionService : ServiceBase, ISincronizacionService
    {
        public static IDataService _DataService = new DataService.DataService();

        public Sincronizacion_Response getDatosSincronizacion(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();
            Transaccion oTransaccion = new Transaccion();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerSincronizacionTransaccion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoTransacciones oDtoTransaccion = (DtoTransacciones)oResultadoOperacion.EntidadDatos;

                oTransaccion.Anulada = oDtoTransaccion.Anulada;
                oTransaccion.CodigoBarras = oDtoTransaccion.CodigoBarras;
                oTransaccion.Comision = oDtoTransaccion.Comision;
                oTransaccion.EstadoTransaccion = oDtoTransaccion.EstadoTransaccion;
                oTransaccion.FechaTransaccion = oDtoTransaccion.FechaTransaccion;
                oTransaccion.IdDocumento = oDtoTransaccion.IdDocumento;
                oTransaccion.IdModulo = oDtoTransaccion.IdModulo;
                oTransaccion.IdSede = oDtoTransaccion.IdSede;
                oTransaccion.IdTipoTransaccion = oDtoTransaccion.IdTipoTransaccion;
                oTransaccion.IdTransaccion = oDtoTransaccion.IdTransaccion;
                oTransaccion.Iva = oDtoTransaccion.Iva;
                oTransaccion.NumeroFactura = oDtoTransaccion.NumeroFactura;
                oTransaccion.Redondeo = oDtoTransaccion.Redondeo;
                oTransaccion.Sincronizacion = true;
                oTransaccion.SincronizacionPago = oDtoTransaccion.SincronizacionPago;
                oTransaccion.TotalPagado = oDtoTransaccion.TotalPagado;
                oTransaccion.ValorRecibido = oDtoTransaccion.ValorRecibido;


                request.oTransaccion = oTransaccion;

                oResultadoOperacion = _DataService.RegistrarSincronizacionTransaccion(request.oTransaccion, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionTransaccion(oTransaccion.IdTransaccion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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
        public Sincronizacion_Response getDatosSincronizacionPago(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();

            Transaccion oTransaccion = new Transaccion();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerSincronizacionPagoTransaccion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoTransacciones oDtoTransaccion = (DtoTransacciones)oResultadoOperacion.EntidadDatos;

                oTransaccion.Anulada = oDtoTransaccion.Anulada;
                oTransaccion.CodigoBarras = oDtoTransaccion.CodigoBarras;
                oTransaccion.Comision = oDtoTransaccion.Comision;
                oTransaccion.EstadoTransaccion = oDtoTransaccion.EstadoTransaccion;
                oTransaccion.FechaTransaccion = oDtoTransaccion.FechaTransaccion;
                oTransaccion.IdDocumento = oDtoTransaccion.IdDocumento;
                oTransaccion.IdModulo = oDtoTransaccion.IdModulo;
                oTransaccion.IdSede = oDtoTransaccion.IdSede;
                oTransaccion.IdTipoTransaccion = oDtoTransaccion.IdTipoTransaccion;
                oTransaccion.IdTransaccion = oDtoTransaccion.IdTransaccion;
                oTransaccion.Iva = oDtoTransaccion.Iva;
                oTransaccion.NumeroFactura = oDtoTransaccion.NumeroFactura;
                oTransaccion.Redondeo = oDtoTransaccion.Redondeo;
                oTransaccion.Sincronizacion = true;
                oTransaccion.SincronizacionPago = true;
                oTransaccion.TotalPagado = oDtoTransaccion.TotalPagado;
                oTransaccion.ValorRecibido = oDtoTransaccion.ValorRecibido;

                request.oTransaccion = oTransaccion;

                oResultadoOperacion = _DataService.RegistrarSincronizacionPagoTransaccion(request.oTransaccion, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionPagoTransaccion(oTransaccion.IdTransaccion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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

        public Sincronizacion_Response getDatosCambio(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();
            Transaccion oTransaccion = new Transaccion();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerCambioSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoTransacciones oDtoTransaccion = (DtoTransacciones)oResultadoOperacion.EntidadDatos;

                oTransaccion.Anulada = oDtoTransaccion.Anulada;
                oTransaccion.CodigoBarras = oDtoTransaccion.CodigoBarras;
                oTransaccion.Comision = oDtoTransaccion.Comision;
                oTransaccion.EstadoTransaccion = oDtoTransaccion.EstadoTransaccion;
                oTransaccion.FechaTransaccion = oDtoTransaccion.FechaTransaccion;
                oTransaccion.IdDocumento = oDtoTransaccion.IdDocumento;
                oTransaccion.IdModulo = oDtoTransaccion.IdModulo;
                oTransaccion.IdSede = oDtoTransaccion.IdSede;
                oTransaccion.IdTipoTransaccion = oDtoTransaccion.IdTipoTransaccion;
                oTransaccion.IdTransaccion = oDtoTransaccion.IdTransaccion;
                oTransaccion.Iva = oDtoTransaccion.Iva;
                oTransaccion.NumeroFactura = oDtoTransaccion.NumeroFactura;
                oTransaccion.Redondeo = oDtoTransaccion.Redondeo;
                oTransaccion.Sincronizacion = true;
                oTransaccion.TotalPagado = oDtoTransaccion.TotalPagado;
                oTransaccion.ValorRecibido = oDtoTransaccion.ValorRecibido;


                request.oTransaccion = oTransaccion;

                oResultadoOperacion = _DataService.RegistrarSincronizacionCambio(request.oTransaccion, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionCambio(oTransaccion.IdTransaccion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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
        public Sincronizacion_Response getDatosRecargas(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();
            Transaccion oTransaccion = new Transaccion();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerRecargasSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoTransacciones oDtoTransaccion = (DtoTransacciones)oResultadoOperacion.EntidadDatos;

                oTransaccion.Anulada = oDtoTransaccion.Anulada;
                oTransaccion.CodigoBarras = oDtoTransaccion.CodigoBarras;
                oTransaccion.Comision = oDtoTransaccion.Comision;
                oTransaccion.EstadoTransaccion = oDtoTransaccion.EstadoTransaccion;
                oTransaccion.FechaTransaccion = oDtoTransaccion.FechaTransaccion;
                oTransaccion.IdDocumento = oDtoTransaccion.IdDocumento;
                oTransaccion.IdModulo = oDtoTransaccion.IdModulo;
                oTransaccion.IdSede = oDtoTransaccion.IdSede;
                oTransaccion.IdTipoTransaccion = oDtoTransaccion.IdTipoTransaccion;
                oTransaccion.IdTransaccion = oDtoTransaccion.IdTransaccion;
                oTransaccion.Iva = oDtoTransaccion.Iva;
                oTransaccion.NumeroFactura = oDtoTransaccion.NumeroFactura;
                oTransaccion.Redondeo = oDtoTransaccion.Redondeo;
                oTransaccion.Sincronizacion = true;
                oTransaccion.SincronizacionPago = oDtoTransaccion.SincronizacionPago;
                oTransaccion.TotalPagado = oDtoTransaccion.TotalPagado;
                oTransaccion.ValorRecibido = oDtoTransaccion.ValorRecibido;
                oTransaccion.Operador = oDtoTransaccion.Operador;
                oTransaccion.Linea = oDtoTransaccion.Linea;
                oTransaccion.Descripcion = oDtoTransaccion.Descripcion;

                request.oTransaccion = oTransaccion;

                oResultadoOperacion = _DataService.RegistrarSincronizacionRecargas(request.oTransaccion, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionRecargas(oTransaccion.IdTransaccion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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
        public Sincronizacion_Response getDatosDonacion(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();
            Transaccion oTransaccion = new Transaccion();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerDonacionSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoTransacciones oDtoTransaccion = (DtoTransacciones)oResultadoOperacion.EntidadDatos;

                oTransaccion.Anulada = oDtoTransaccion.Anulada;
                oTransaccion.CodigoBarras = oDtoTransaccion.CodigoBarras;
                oTransaccion.Comision = oDtoTransaccion.Comision;
                oTransaccion.EstadoTransaccion = oDtoTransaccion.EstadoTransaccion;
                oTransaccion.FechaTransaccion = oDtoTransaccion.FechaTransaccion;
                oTransaccion.IdDocumento = oDtoTransaccion.IdDocumento;
                oTransaccion.IdModulo = oDtoTransaccion.IdModulo;
                oTransaccion.IdSede = oDtoTransaccion.IdSede;
                oTransaccion.IdTipoTransaccion = oDtoTransaccion.IdTipoTransaccion;
                oTransaccion.IdTransaccion = oDtoTransaccion.IdTransaccion;
                oTransaccion.Iva = oDtoTransaccion.Iva;
                oTransaccion.NumeroFactura = oDtoTransaccion.NumeroFactura;
                oTransaccion.Redondeo = oDtoTransaccion.Redondeo;
                oTransaccion.Sincronizacion = true;
                oTransaccion.SincronizacionPago = oDtoTransaccion.SincronizacionPago;
                oTransaccion.TotalPagado = oDtoTransaccion.TotalPagado;
                oTransaccion.ValorRecibido = oDtoTransaccion.ValorRecibido;
                oTransaccion.Fundacion = oDtoTransaccion.Fundacion;

                request.oTransaccion = oTransaccion;

                oResultadoOperacion = _DataService.RegistrarSincronizacionDonacion(request.oTransaccion, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionDonacion(oTransaccion.IdTransaccion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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
        
        public Sincronizacion_Response getDatosArqueos(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerArqueoSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoArqueo oDtoArqueos = (DtoArqueo)oResultadoOperacion.EntidadDatos;

                Arqueo oArqueos = new Arqueo();

                oArqueos.IdArqueo = oDtoArqueos.IdArqueo;
                oArqueos.FechaInicio = oDtoArqueos.FechaInicio;
                oArqueos.FechaFin = oDtoArqueos.FechaFin;
                oArqueos.Valor = oDtoArqueos.Valor;
                oArqueos.IdUsuario = oDtoArqueos.IdUsuario;
                oArqueos.IdModulo = oDtoArqueos.IdModulo;
                oArqueos.IdSede = oDtoArqueos.IdSede;
                oArqueos.CantTransacciones = oDtoArqueos.CantTransacciones;
                oArqueos.Producido = oDtoArqueos.Producido;
                oArqueos.Tipo = oDtoArqueos.Tipo;
                oArqueos.Conteo = oDtoArqueos.Conteo;
                oArqueos.Sincronizacion = true;


                request.oArqueos = oArqueos;

                if (oArqueos.FechaFin != null)
                {

                    oResultadoOperacion = _DataService.RegistrarSincronizacionArqueo(request.oArqueos, request.sConexion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        string IdArqueo = oResultadoOperacion.EntidadDatos.ToString();
                        long Arqueo = Convert.ToInt64(IdArqueo);


                        oResultadoOperacion = _DataService.ActualizaSincronizacionArqueo(Arqueo);
                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                        }
                    }
                    else
                    {
                        response.Acknowledge = AcknowledgeType.Failure;
                        response.Message = oResultadoOperacion.Mensaje;
                    }
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }
        public Sincronizacion_Response getDatosCargas(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();

            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerCargaSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoCarga oDtoCarga = (DtoCarga)oResultadoOperacion.EntidadDatos;

                Carga oCarga = new Carga();

                oCarga.IdCarga = oDtoCarga.IdCarga;
                oCarga.FechaInicio = oDtoCarga.FechaInicio;
                oCarga.FechaFin = oDtoCarga.FechaFin;
                oCarga.Valor = oDtoCarga.Valor;
                oCarga.IdUsuario = oDtoCarga.IdUsuario;
                oCarga.IdModulo = oDtoCarga.IdModulo;
                oCarga.IdSede = oDtoCarga.IdSede;
                oCarga.Sincronizacion = true;


                request.oCarga = oCarga;

                if (oCarga.FechaFin != null)
                {

                    oResultadoOperacion = _DataService.RegistrarSincronizacionCarga(request.oCarga, request.sConexion);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                       
                        oResultadoOperacion = _DataService.ActualizaSincronizacionCarga(oCarga.IdCarga);
                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                        }
                    }
                    else
                    {
                        response.Acknowledge = AcknowledgeType.Failure;
                        response.Message = oResultadoOperacion.Mensaje;
                    }
                }
            }
            else
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = oResultadoOperacion.Mensaje;
            }

            return response;
        }

        public Sincronizacion_Response getDatosMovimientos(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();

            Movimiento oMovimientos = new Movimiento();
            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerMovimientoSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoMovimiento oDtoMovimientos = (DtoMovimiento)oResultadoOperacion.EntidadDatos;

                oMovimientos.Cantidad = oDtoMovimientos.Cantidad;
                oMovimientos.Denominacion = oDtoMovimientos.Denominacion;
                oMovimientos.FechaMovimiento = oDtoMovimientos.FechaMovimiento;
                oMovimientos.ID_Modulo = oDtoMovimientos.IdCajero;
                oMovimientos.ID_Movimiento = oDtoMovimientos.IdMovimiento;
                oMovimientos.ID_Operacion = oDtoMovimientos.Id;
                oMovimientos.IdArqueo = oDtoMovimientos.IdArqueo;
                oMovimientos.IdCarga = oDtoMovimientos.IdCarga;
                oMovimientos.IdSede = oDtoMovimientos.IdSede;
                oMovimientos.IdTransaccion = oDtoMovimientos.IdTransaccion;
                oMovimientos.Parte = oDtoMovimientos.Parte;
                oMovimientos.Sincronizacion = true;
                oMovimientos.Accion = oDtoMovimientos.Accion;
                oMovimientos.Valor = oDtoMovimientos.Valor;
                
                request.oMovimientos = oMovimientos;

                oResultadoOperacion = _DataService.RegistrarSincronizacionMovimiento(request.oMovimientos, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                   
                    oResultadoOperacion = _DataService.ActualizaSincronizacionMovimiento(oMovimientos.ID_Movimiento);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
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

        public Sincronizacion_Response getDatosPartes(Sincronizacion_Request request)
        {
            Sincronizacion_Response response = new Sincronizacion_Response();

            ParteModulo oParteModulo = new ParteModulo();
            response.CorrelationId = request.RequestId;

            if (!ValidRequest(request, response))
                return response;

            ResultadoOperacion oResultadoOperacion = _DataService.ObtenerPartesSincronizacion();
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                DtoParteModulo oDtoParteModulo = (DtoParteModulo)oResultadoOperacion.EntidadDatos;

                oParteModulo.IdModulo = oDtoParteModulo.IdModulo;
                oParteModulo.IdSede = oDtoParteModulo.IdSede;
                oParteModulo.Nombre_Parte = oDtoParteModulo.Nombre;
                oParteModulo.Dinero_Actual = Convert.ToInt32(oDtoParteModulo.DineroActual);
                oParteModulo.Qty_Actual = Convert.ToInt32(oDtoParteModulo.CantidadActual);
                oParteModulo.Denominacion = Convert.ToInt32(oDtoParteModulo.Denominacion);

                oResultadoOperacion = _DataService.ActualizaSincronizacionParte(oParteModulo.IdModulo, oParteModulo.IdSede, oParteModulo.Nombre_Parte,oParteModulo.Denominacion, oParteModulo.Dinero_Actual, oParteModulo.Qty_Actual, request.sConexion);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = _DataService.ActualizaSincronizacionParteLocal(oParteModulo.IdModulo, oParteModulo.IdSede, oParteModulo.Nombre_Parte,oParteModulo.Denominacion, oParteModulo.Dinero_Actual, oParteModulo.Qty_Actual);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        response.sResult = oResultadoOperacion.EntidadDatos.ToString();
                    }
                    
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
        
    }
}
