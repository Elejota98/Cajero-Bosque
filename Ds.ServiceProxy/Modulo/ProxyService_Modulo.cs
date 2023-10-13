using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds.BusinessObjects.DataTransferObject;
using Ds.WCFProxy;
using Ds.BusinessObjects.Entities;
using Ds.ServiceProxy.Ds_ModuloComercialService;
using Ds.BusinessObjects.Enums;
using Ds.ServiceProxy.DataTransferObjectMapper;
using Ds.ServiceProxy.MC_LiquidacionService;


namespace Ds.ServiceProxy
{
    public partial class ProxyService : IProxyService
    {
        public ResultadoOperacion ObtenerInformacionModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoModulo oDtoModulo = new DtoModulo();

            getInformacionModulo_Request request = new getInformacionModulo_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getInformacionModulo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getInformacionModulo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoModulo = Mapper.FromDataTransferObject(response.oDatosModulo);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getPartesModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getPartesModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoModulo;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerInformacionFactura(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoFactura oDtoFactura = new DtoFactura();

            getInformacionFactura_Request request = new getInformacionFactura_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getInformacionFactura_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getInformacionFactura(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoFactura = Mapper.FromDataTransferObject(response.oDtoFactura);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getPartesModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getPartesModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoFactura;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerPartesModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParteModulo> lstDtoParteModulo = new List<DtoParteModulo>();

            getPartesModulo_Request request = new getPartesModulo_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getPartesModulo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getPartesModulo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoParteModulo = Mapper.FromDataTransferObjects(response.oListaPartesModulo);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getPartesModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getPartesModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoParteModulo;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerPartesF56Modulo(Modulo oModulo, int Tipo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParteModuloF56> lstDtoParteModulo = new List<DtoParteModuloF56>();

            getPartesModulo_Request request = new getPartesModulo_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;
            request.oTipo = Tipo;
            getPartesModulo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getPartesF56Modulo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerPartesModulo() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoParteModulo = Mapper.FromDataTransferObjects(response.oListaPartesModuloF56);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "ObtenerPartesModulo() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerPartesModulo() error response == null";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoParteModulo;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerParametrosModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoParametro> lstDtoParametro = new List<DtoParametro>();

            getParametrosModulo_Request request = new getParametrosModulo_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getParametrosModulo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getParametrosModulo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoParametro = Mapper.FromDataTransferObjects(response.oListaPartesModulo);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getParametrosModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getParametrosModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoParametro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarSaldosMinimos(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            TipoValidarSaldosMinimos enumValidaSaldosMinimos;

            getValidarSaldosMinimos_Request request = new getValidarSaldosMinimos_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getValidarSaldosMinimos_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarSaldosMinimos(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        if (response.validarSaldosMinimos)
                        {
                            enumValidaSaldosMinimos = TipoValidarSaldosMinimos.True;
                        }
                        else
                        {
                            enumValidaSaldosMinimos = TipoValidarSaldosMinimos.False;
                        }
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarSaldosMinimos";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarSaldosMinimos";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = enumValidaSaldosMinimos;
            oResultadoOperacion.Mensaje = response.Message;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarOperacion(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string IdTransaccion = string.Empty;

            setRegistrarOperacion_Request request = new setRegistrarOperacion_Request();
            request.RequestId = NuevoRequestId;

            ServiceTransaccion oServiceOperacion = new ServiceTransaccion();
            oServiceOperacion.IdModulo = oTransaccion.IdModulo;
            oServiceOperacion.IdTransaccion = oTransaccion.IdTransaccion;
            oServiceOperacion.IdSede = oTransaccion.IdSede;
           

            request.oTransaccion = oServiceOperacion;
            setRegistrarOperacion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarOperacion(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        IdTransaccion = response.IdTransaccion;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarOperacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarOperacion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = IdTransaccion;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string IdArqueo = string.Empty;

            setRegistrarOperacion_Request request = new setRegistrarOperacion_Request();
            request.RequestId = NuevoRequestId;

            ServiceArqueo oServiceArqueo = new ServiceArqueo();
            oServiceArqueo.IdModulo = oArqueo.IdModulo;
            oServiceArqueo.IdSede = oArqueo.IdSede;
            oServiceArqueo.IdUsuario = oArqueo.IdUsuario;
            oServiceArqueo.Tipo = oArqueo.Tipo;


            request.oArqueo = oServiceArqueo;
            setRegistrarOperacion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarArqueo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        IdArqueo = response.IdTransaccion;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarOperacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarOperacion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = IdArqueo;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarCarga(Carga oCarga)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string IdCarga = string.Empty;

            setRegistrarOperacion_Request request = new setRegistrarOperacion_Request();
            request.RequestId = NuevoRequestId;

            ServiceCarga oServiceCarga = new ServiceCarga();
            oServiceCarga.IdModulo = oCarga.IdModulo;
            oServiceCarga.IdSede = oCarga.IdSede;
            oServiceCarga.IdUsuario = oCarga.IdUsuario;


            request.oCarga = oServiceCarga;
            setRegistrarOperacion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarCarga(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        IdCarga = response.IdTransaccion;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarOperacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarOperacion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = IdCarga;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarMovimiento(Movimiento oMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            long idMovimiento = 0;

            setRegistrarMovimiento_Request request = new setRegistrarMovimiento_Request();
            request.RequestId = NuevoRequestId;

            ServiceMovimiento oServiceMovimiento = new ServiceMovimiento();

            oServiceMovimiento.IdTransaccion = oMovimiento.IdTransaccion;
            oServiceMovimiento.IdSede = oMovimiento.IdSede;
            oServiceMovimiento.IdCarga = oMovimiento.IdCarga;
            oServiceMovimiento.IdArqueo = oMovimiento.IdArqueo;
            oServiceMovimiento.ID_Modulo = oMovimiento.ID_Modulo;
            oServiceMovimiento.Cantidad = oMovimiento.Cantidad;
            oServiceMovimiento.Denominacion = oMovimiento.Denominacion;
            oServiceMovimiento.ID_Movimiento = oMovimiento.ID_Movimiento;
            oServiceMovimiento.ID_Operacion = oMovimiento.ID_Operacion;
            oServiceMovimiento.Parte = oMovimiento.Parte;

            if (oMovimiento.TipoAccionMovimiento == BusinessObjects.Enums.TipoMovimiento.Entrada)
            {
                oServiceMovimiento.TipoAccionMovimiento = Ds.ServiceProxy.Ds_ModuloComercialService.TipoMovimiento.Entrada;
            }
            else if (oMovimiento.TipoAccionMovimiento == BusinessObjects.Enums.TipoMovimiento.Salida)
            {
                oServiceMovimiento.TipoAccionMovimiento = Ds.ServiceProxy.Ds_ModuloComercialService.TipoMovimiento.Salida;
            }
            else if (oMovimiento.TipoAccionMovimiento == BusinessObjects.Enums.TipoMovimiento.Cambio)
            {
                oServiceMovimiento.TipoAccionMovimiento = Ds.ServiceProxy.Ds_ModuloComercialService.TipoMovimiento.Cambio;
            }
            else if (oMovimiento.TipoAccionMovimiento == BusinessObjects.Enums.TipoMovimiento.Donacion)
            {
                oServiceMovimiento.TipoAccionMovimiento = Ds.ServiceProxy.Ds_ModuloComercialService.TipoMovimiento.Donacion;
            }
            else if (oMovimiento.TipoAccionMovimiento == BusinessObjects.Enums.TipoMovimiento.Recarga)
            {
                oServiceMovimiento.TipoAccionMovimiento = Ds.ServiceProxy.Ds_ModuloComercialService.TipoMovimiento.Recarga;
            }
            
            if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.ArqueoParcial)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.ArqueoParcial;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.ArqueoTotal)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.ArqueoTotal;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Carga)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Carga;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.CerrarAplicacion)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.CerrarAplicacion;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Mantenimiento)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Mantenimiento;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Pago)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Pago;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Recarga)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Recarga;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Donacion)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Donacion;
            }
            else if (oMovimiento.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Reconteo)
            {
                oServiceMovimiento.TipoOperacion = Ds.ServiceProxy.Ds_ModuloComercialService.TipoOperacion.Reconteo;
            }

            request.oMovimiento = oServiceMovimiento;

            setRegistrarMovimiento_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarMovimiento(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        idMovimiento = response.lIdMovimiento;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarMovimiento";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarMovimiento";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = idMovimiento;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerSaldosPartes(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoSaldos oDtoSaldos = new DtoSaldos();

            getObtenerSaldosPartes_Request request = new getObtenerSaldosPartes_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;

            getObtenerSaldosPartes_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getObtenerSaldosPartes(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoSaldos = Mapper.FromDataTransferObject(response.oDtoSaldos);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getObtenerSaldosPartes";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getObtenerSaldosPartes";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoSaldos;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ConfirmarOperacion(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoOperacion oDtoOperacion = new DtoOperacion();

            setConfirmarOperacion_Request request = new setConfirmarOperacion_Request();
            request.RequestId = NuevoRequestId;

            ServiceOperacion oServiceOperacion = new ServiceOperacion();
            oServiceOperacion.Pago = new ServicePago();
            oServiceOperacion.ID_Modulo = oOperacion.ID_Modulo;
            oServiceOperacion.ID_Operacion = oOperacion.ID_Operacion;
            oServiceOperacion.ID_Transaccion = oOperacion.ID_Transaccion;
            oServiceOperacion.IdSede = oOperacion.IdSede;
            oServiceOperacion.Total = oOperacion.Total;
            oServiceOperacion.Comision = oOperacion.Comision;
            oServiceOperacion.Redondeo = oOperacion.Redondeo;
            oServiceOperacion.Iva = oOperacion.Iva;
            oServiceOperacion.TotalPagado = oOperacion.TotalPagado;
            oServiceOperacion.Donacion = oOperacion.Donacion;
            oServiceOperacion.Linea = oOperacion.Linea;
            oServiceOperacion.Operador = oOperacion.Operador;
            oServiceOperacion.Descripcion = oOperacion.Descripcion;
            oServiceOperacion.Programa = oOperacion.Fundacion;
            oServiceOperacion.ValidacionCobro = oOperacion.ValidacionCobro;
            oServiceOperacion.Pago.CodigoBarras = oOperacion.Pago.CodigoBarras;
            oServiceOperacion.Pago.Referencia = oOperacion.Pago.Referencia;

            if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.ReconteoExitoso)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Pago;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Pago)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Pago;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Mensualidad)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Mensualidad;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Casco)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Casco;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Evento)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Evento;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.CobroTarjetaMensual)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.CobroTarjetaMensual;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Reposicion)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Reposicion;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Recarga)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Recarga;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Donacion)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Donacion;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.ArqueoParcial)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.ArqueoParcial;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.ArqueoTotal)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.ArqueoTotal;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Carga)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Carga;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.CerrarAplicacion)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.CerrarAplicacion;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Mantenimiento)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Mantenimiento;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Pago)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Pago;
            }
            else if (oOperacion.TipoOperacion == BusinessObjects.Enums.TipoOperacion.Datafono)
            {
                oServiceOperacion.TipoOperacion = Ds_ModuloComercialService.TipoOperacion.Datafono;
            }

            if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.Aprobado)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.Aprobado;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.Cancelado)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.Cancelado;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.Error_Dispositivo)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.Error_Dispositivo;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.Error_WebService)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.Error_WebService;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.NoAplica)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.NoAplica;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.Error_Dispositivo_NoConfirmaPago)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.Error_Dispositivo_NoConfirmaPago;
            }
            else if (oOperacion.Pago.EstadoPago == BusinessObjects.Enums.TipoEstadoPago.ReconteoExitoso)
            {
                oServiceOperacion.Pago.EstadoPago = Ds_ModuloComercialService.TipoEstadoPago.ReconteoExitoso;
            }

            oServiceOperacion.Pago.Factura = oOperacion.Pago.Factura;
            oServiceOperacion.Pago.Referencia = oOperacion.Pago.Referencia;

            if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.Credito)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.Credito;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.Ahorros)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.Ahorros;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.Efectivo)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.Efectivo;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.BolsilloCredito)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.BolsilloCredito;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.BolsilloDebito)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.BolsilloDebito;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.BonoDescuento)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.BonoDescuento;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.BonoEfectivo)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.BonoEfectivo;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.BonoRegalo)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.BonoRegalo;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.Corriente)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.Corriente;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.CreditoRotativo)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.CreditoRotativo;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.CuotaMonetaria)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.CuotaMonetaria;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.CupoCredito)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.CupoCredito;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.Lealtad)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.Lealtad;
            }
            else if (oOperacion.Pago.TipoPago == BusinessObjects.Enums.TipoPago.SuperCupo)
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.SuperCupo;
            }
            else
            {
                oServiceOperacion.Pago.TipoPago = Ds_ModuloComercialService.TipoPago.NoAplica;
            }

            oServiceOperacion.Pago.NoAutorizacion = oOperacion.Pago.NoAutorizacion;
            oServiceOperacion.Pago.NoTarjeta = oOperacion.Pago.NoTarjeta;
            oServiceOperacion.Pago.Franquicia = oOperacion.Pago.Franquicia;


            request.oOperacion = oServiceOperacion;

            setConfirmarOperacion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setConfirmarOperacion(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoOperacion = Mapper.FromDataTransferObject(response.oDtoOperacion);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setConfirmarOperacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setConfirmarOperacion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoOperacion;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarTransaccion(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            long idTransaccion = 0;

            setRegistrarTransaccion_Request request = new setRegistrarTransaccion_Request();
            request.RequestId = NuevoRequestId;

            ServiceTransaccion oServiceTransaccion = new ServiceTransaccion();
            oServiceTransaccion.IdModulo = oTransaccion.IdModulo;
            //oServiceTransaccion.NumeroOrden = oTransaccion.NumeroOrden;
            //oServiceTransaccion.ClienteCedula = oTransaccion.ClienteCedula;
            //oServiceTransaccion.ClienteNombre = oTransaccion.ClienteNombre;
            //oServiceTransaccion.Tipo_Transaccion = oTransaccion.Tipo_Transaccion;
            //oServiceTransaccion.Valor_Transaccion = oTransaccion.Valor_Transaccion;

            request.oTransaccion = oServiceTransaccion;

            setRegistrarTransaccion_Response response = null;

            try
            {
                //SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                //{ response = client.setRegistrarTransaccion(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        idTransaccion = response.lIdTransaccion;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarTransaccion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarTransaccion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = idTransaccion;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerUsuario(Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoUsuario oDtoUsuario = new DtoUsuario();

            getObtenerUsuario_Request request = new getObtenerUsuario_Request();
            request.RequestId = NuevoRequestId;

            ServiceUsuario oServiceUsuario = new ServiceUsuario();
            oServiceUsuario.IdCriptUsuario = oUsuario.IdCriptUsuario;

            request.oUsuario = oServiceUsuario;

            getObtenerUsuario_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getObtenerUsuario(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoUsuario = Mapper.FromDataTransferObject(response.oDtoUsuario);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getObtenerUsuario";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getObtenerUsuario";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoUsuario;

            return oResultadoOperacion;
        }

        public ResultadoOperacion GenerarClave(Modulo oModulo, Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bClaveGenerada = false;

            setGenerarClave_Request request = new setGenerarClave_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            ServiceUsuario oServiceUsuario = new ServiceUsuario();
            oServiceUsuario.IdUsuario = oUsuario.IdUsuario;

            request.oModulo = oServiceModulo;
            request.oUsuario = oServiceUsuario;

            setGenerarClave_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setGenerarClave(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bClaveGenerada = response.bClaveGenerada;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bClaveGenerada;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarClave(long Identificacion, string sClave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string bClaveValida = string.Empty;

            getValidarClave_Request request = new getValidarClave_Request();
            request.RequestId = NuevoRequestId;


            request.oIdentificacion = Identificacion;
            request.sClave = sClave;

            getValidarClave_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarClave(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bClaveValida = response.sEmpresa;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bClaveValida;

            return oResultadoOperacion;
        }

        public ResultadoOperacion GenerarToken()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string token = string.Empty;

            getValidarClave_Request request = new getValidarClave_Request();
            request.RequestId = NuevoRequestId;


           
            getValidarClave_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getToken(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        token = response.Token;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = token;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarSegundaClave(Modulo oModulo, string sClave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bClaveValida = false;

            getValidarSegundaClave_Request request = new getValidarSegundaClave_Request();
            request.RequestId = NuevoRequestId;


            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;
            request.sClave = sClave;

            getValidarSegundaClave_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarSegundaClave(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bClaveValida = response.bUsuarioValido;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarSegundaClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarSegundaClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bClaveValida;

            return oResultadoOperacion;
        }

        public ResultadoOperacion CrearAlarma(Alarma oAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            setCrearAlarma_Request request = new setCrearAlarma_Request();
            request.RequestId = NuevoRequestId;

            ServiceAlarma oServiceAlarma = new ServiceAlarma();
            oServiceAlarma.Descripcion = oAlarma.Descripcion;
            oServiceAlarma.IdArqueo = null;
            oServiceAlarma.IdCajero = oAlarma.IdCajero;
            oServiceAlarma.IdCarga = null;
            oServiceAlarma.IdLogWS = null;
            oServiceAlarma.IdTransaccion = null;
            oServiceAlarma.NombreParte = oAlarma.NombreParte;
            oServiceAlarma.TipoError = oAlarma.TipoError;
            oServiceAlarma.IdSede = oAlarma.IdSede;
            oServiceAlarma.NivelError = oAlarma.NivelError;
            request.oAlarma = oServiceAlarma;


            setCrearAlarma_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setCrearAlarma(request) ; });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {

                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setCrearAlarma";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setCrearAlarma";
                return oResultadoOperacion;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion SolucionarAlarma(Alarma oAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            setSolucionarAlarma_Request request = new setSolucionarAlarma_Request();
            request.RequestId = NuevoRequestId;

            ServiceAlarma oServiceAlarma = new ServiceAlarma();
            oServiceAlarma.Descripcion = oAlarma.Descripcion;
            oServiceAlarma.IdArqueo = oAlarma.IdArqueo;
            oServiceAlarma.IdCajero = oAlarma.IdCajero;
            oServiceAlarma.IdCarga = oAlarma.IdCarga;
            oServiceAlarma.IdLogWS = oAlarma.IdLogWS;
            oServiceAlarma.IdTransaccion = oAlarma.IdTransaccion;
            oServiceAlarma.NombreParte = oAlarma.NombreParte;
            oServiceAlarma.TipoError = oAlarma.TipoError;

            request.oAlarma = oServiceAlarma;


            setSolucionarAlarma_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setSolucionarAlarma(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {

                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setSolucionarAlarma";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setSolucionarAlarma";
                return oResultadoOperacion;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion SolucionarTodasAlarmas(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            setSolucionarTodasAlarmas_Request request = new setSolucionarTodasAlarmas_Request();
            request.RequestId = NuevoRequestId;

            ServiceModulo oServiceModulo = new ServiceModulo();
            oServiceModulo.ID_Modulo = oModulo.ID_Modulo;

            request.oModulo = oServiceModulo;


            setSolucionarTodasAlarmas_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setSolucionarTodasAlarmas(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {

                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setSolucionarTodasAlarmas";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setSolucionarTodasAlarmas";
                return oResultadoOperacion;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion CrearLogWS(LogWS oLogWS)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            setCrearLogWS_Request request = new setCrearLogWS_Request();
            request.RequestId = NuevoRequestId;

            ServiceLogWS oServiceLogWS = new ServiceLogWS();
            oServiceLogWS.Entrada = oLogWS.Entrada;
            oServiceLogWS.IdCajero = oLogWS.IdCajero;
            oServiceLogWS.IdTransaccion = oLogWS.IdTransaccion;
            oServiceLogWS.Metodo = oLogWS.Metodo;
            oServiceLogWS.Salida = oLogWS.Salida;

            request.oLogWS = oServiceLogWS;

            setCrearLogWS_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setCrearLogWS(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {

                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setCrearLogWS";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setCrearLogWS";
                return oResultadoOperacion;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion InsertarConfirmacionOrdenes(DataTable tTablaInsertar)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            setInsertarConfirmacion_Request request = new setInsertarConfirmacion_Request();
            request.RequestId = NuevoRequestId;

            DataSet oDataSet = new DataSet();
            oDataSet.Tables.Add(tTablaInsertar);

            request.tablasInsertar = oDataSet;

            setInsertarConfirmacion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setInsertarConfirmacion(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {

                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setInsertarConfirmacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setInsertarConfirmacion";
                return oResultadoOperacion;
            }

            return oResultadoOperacion;
        }

        //public ResultadoOperacion ObtenerDatosCargaActual(TraePagina oParametrosTraePagina)
        //{
        //    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

        //    List<DtoParteModulo> olstParteModulo = new List<DtoParteModulo>();

        //    getDatosCargaActual_Request request = new getDatosCargaActual_Request();
        //    request.RequestId = NuevoRequestId;


        //    ServiceTraeDatosPagina oServiceTraepagina = new ServiceTraeDatosPagina();
        //    oServiceTraepagina.CampoOrden = oParametrosTraePagina.CampoOrden;
        //    oServiceTraepagina.Filtro = oParametrosTraePagina.Filtro;
        //    oServiceTraepagina.Orden = oParametrosTraePagina.Orden;
        //    oServiceTraepagina.PaginaActual = oParametrosTraePagina.PaginaActual;
        //    oServiceTraepagina.RegistrosPagina = oParametrosTraePagina.RegistrosPagina;
        //    oServiceTraepagina.Tipo = oParametrosTraePagina.Tipo;

        //    request.oParametrosTraePagina = oServiceTraepagina;


        //    getDatosCargaActual_Response response = null;

        //    try
        //    {
        //        SafeProxy.DoAction<ModuloServiceClient>(_EGlobalT_ATM_ModuloServices, client =>
        //        { response = client.getDatosCargaActual(request); });
        //    }
        //    catch (System.Exception)
        //    {
        //        oResultadoOperacion.oEstado = TipoRespuesta.Error;
        //        oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
        //        return oResultadoOperacion;
        //    }

        //    if (response != null)
        //    {
        //        if (request.RequestId == response.CorrelationId)
        //        {
        //            if (response.Acknowledge == Ds.ServiceProxy.EGlobalT_ATM_ModuloService.AcknowledgeType.Success)
        //            {
        //                olstParteModulo = Mapper.FromDataTransferObjects(response.oListaParteModulo);
        //            }
        //            else
        //            {
        //                oResultadoOperacion.oEstado = TipoRespuesta.Error;
        //                oResultadoOperacion.Mensaje = response.Message;
        //                return oResultadoOperacion;
        //            }
        //        }
        //        else
        //        {
        //            oResultadoOperacion.oEstado = TipoRespuesta.Error;
        //            oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getObtenerUsuario";
        //            return oResultadoOperacion;
        //        }
        //    }
        //    else
        //    {
        //        oResultadoOperacion.oEstado = TipoRespuesta.Error;
        //        oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getObtenerUsuario";
        //        return oResultadoOperacion;
        //    }

        //    oResultadoOperacion.ListaEntidadDatos = olstParteModulo;

        //    return oResultadoOperacion;
        //}

        public ResultadoOperacion ObtenerIdTransacciones(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //DataSet setPendientes = new DataSet();
            //DtoIngresos oDtoIngresos = new DtoIngresos();
            List<DtoIngresos> lstDtoIngresos = new List<DtoIngresos>();

            getIdTransacciones_Request request = new getIdTransacciones_Request();
            request.RequestId = NuevoRequestId;

            ServiceTransaccion oServiceTransaccion = new ServiceTransaccion();
            oServiceTransaccion.IdModulo = oTransaccion.IdModulo;           
            //oServiceTransaccion.NumeroFactura = oTransaccion.NumeroDocumentoOrigen;
           
            request.oTransaccion = oServiceTransaccion;

            getIdTransacciones_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getIdTransacciones(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = response.Message;
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoIngresos = Mapper.FromDataTransferObjects(response.lstDtoIngresos);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = response.Message;
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = response.Message; ;
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoIngresos;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerLogMovimiento(LogMovimiento oLogMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoLogMovimiento> lstDtoLogMovimiento = new List<DtoLogMovimiento>();

            getLogMovimiento_Request request = new getLogMovimiento_Request();
            request.RequestId = NuevoRequestId;

            ServiceLogMovimiento oServiceLogMovimiento = new ServiceLogMovimiento();
            oServiceLogMovimiento.Id = oLogMovimiento.Id;
            oServiceLogMovimiento.Parte = oLogMovimiento.Parte;

            request.oLogMovimiento = oServiceLogMovimiento;

            getLogMovimiento_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getLogMovimiento(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoLogMovimiento = Mapper.FromDataTransferObjects(response.oListaLogMovimiento);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getParametrosModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getParametrosModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoLogMovimiento;

            return oResultadoOperacion;
        }



        public ResultadoOperacion ObtenerTransaccionesOfflineModulo()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTransacciones> oDtoTransacciones = new List<DtoTransacciones>();

            getTransaccionesOfflineModulo_Request request = new getTransaccionesOfflineModulo_Request();
            request.RequestId = NuevoRequestId;

            getTransaccionesOfflineModulo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getTransaccionesOfflineModulo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = response.Message;
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoTransacciones = Mapper.FromDataTransferObjects(response.oDtoTransaccionesModulo);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = response.Message;
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = response.Message; ;
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoTransacciones;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerPagosOffline(int oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoPago> oDtoPago = new List<DtoPago>();

            getPagosOffline_Request request = new getPagosOffline_Request();
            request.RequestId = NuevoRequestId;

            ServiceTransaccion oServiceTransaccion = new ServiceTransaccion();
            oServiceTransaccion.IdTransaccion = oTransaccion;

            request.oTransaccion = oServiceTransaccion;

            getPagosOffline_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getPagosOffline(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoPago = Mapper.FromDataTransferObjects(response.lstDtoPago);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarTransaccion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarTransaccion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoPago;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerMovimientosOffline(int oPago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoLogMovimiento> LstDtoMovimiento = new List<DtoLogMovimiento>();

            getMovimientosOffline_Request request = new getMovimientosOffline_Request();
            request.RequestId = NuevoRequestId;

            ServicePago oServicePago = new ServicePago();
            oServicePago.ID_Pago = oPago;
            oServicePago.TipoPago = Ds_ModuloComercialService.TipoPago.NoAplica;


            request.oPago = oServicePago;

            getMovimientosOffline_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getLogMovimientoOffline(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        LstDtoMovimiento = Mapper.FromDataTransferObjects(response.lstDtoMovimientos);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarTransaccion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarTransaccion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = LstDtoMovimiento;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ActualizarRegistro(int oPago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistroActualizado = false;

            setActualizarRegistro_Request request = new setActualizarRegistro_Request();
            request.RequestId = NuevoRequestId;

            ServicePago oServicePago = new ServicePago();
            oServicePago.ID_Pago = oPago;
            oServicePago.TipoPago = Ds_ModuloComercialService.TipoPago.NoAplica;


            request.oPago = oServicePago;

            setActualizarRegistro_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setActualizarRegistro(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistroActualizado = response.bRegistroActualizado;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarTransaccion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarTransaccion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistroActualizado;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerDetalleArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoArqueo> LstDtoArqueo = new List<DtoArqueo>();

            getDetalleArqueo_Request request = new getDetalleArqueo_Request();
            request.RequestId = NuevoRequestId;

            ServiceArqueo oServiceArqueo = new ServiceArqueo();
            oServiceArqueo.IdArqueo = oArqueo.IdArqueo;


            request.oArqueo = oServiceArqueo;

            getDetalleArqueo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getDetalleArqueo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        LstDtoArqueo = Mapper.FromDataTransferObjects(response.lstDtoArqueo);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarTransaccion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarTransaccion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = LstDtoArqueo;

            return oResultadoOperacion;
        }

        public ResultadoOperacion Monitoreo(string Modulo, string Estado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bResult = false;

            setMonitoreo_Request request = new setMonitoreo_Request();
            request.RequestId = NuevoRequestId;

            request.oEstado = Estado;
            request.oModulo = Modulo;

            setMonitoreo_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.SetMonitoreo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bResult = response.bRegistro;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bResult;

            return oResultadoOperacion;
        }


        /// <summary>
        /// CITY SERVICES
        /// </summary>
        /// <returns></returns>

        public ResultadoOperacion ConsultaValor(string sSecuencia, int iTipoVehiculo, bool bMensualidad, bool bReposicion, string Convenios, int TipoTarifa)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            List<DatosLiquidacion> lstDatosLiquidacion = new List<DatosLiquidacion>();
            LiquidacionServiceClient oClient = new LiquidacionServiceClient();
            Liquidacion_Request request = new Liquidacion_Request();
            Liquidacion_Response response = new Liquidacion_Response();

            request.RequestId = NuevoRequestId;

            request.bMensualidad = bMensualidad;
            request.bReposicion = bReposicion;
            request.iTipoVehiculo = iTipoVehiculo;
            request.sSecuencia = sSecuencia;
            request.sConvenios = Convenios;
            request.iTipoTarifa = TipoTarifa;

            response = oClient.getDatosLiquidacion(request);

            if (response.Acknowledge == MC_LiquidacionService.AcknowledgeType.Success)
            {
                oResultadoOperacion.Mensaje = response.Message;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                if (response.olstDtoLiquidacion.Length > 0)
                {
                    if (response.olstDtoLiquidacion.Length == 2)
                    {
                        //oResultadoOperacion.EntidadDatos = response.olstDtoLiquidacion[0].Total + ";" + response.olstDtoLiquidacion[0].SubTotal + ";" + response.olstDtoLiquidacion[0].Iva + ";" + response.olstDtoLiquidacion[1].Total + ";" + response.olstDtoLiquidacion[1].SubTotal + ";" + response.olstDtoLiquidacion[1].Iva;
                        oResultadoOperacion.EntidadDatos = response.olstDtoLiquidacion[0].Tipo + ";" + response.olstDtoLiquidacion[1].Tipo;
                        foreach (ServiceDtoDatosLiquidacion item in response.olstDtoLiquidacion)
                        {

                            DatosLiquidacion oDatosLiquidacionService = new DatosLiquidacion();
                            oDatosLiquidacionService.Tipo = item.Tipo;
                            oDatosLiquidacionService.SubTotal = item.SubTotal;
                            oDatosLiquidacionService.Iva = item.Iva;
                            oDatosLiquidacionService.Total = item.Total;

                            lstDatosLiquidacion.Add(oDatosLiquidacionService);
                        }

                        oResultadoOperacion.ListaEntidadDatos = lstDatosLiquidacion;
                    }
                    else
                    {
                        oResultadoOperacion.EntidadDatos = response.olstDtoLiquidacion[0].Total + ";" + response.olstDtoLiquidacion[0].SubTotal + ";" + response.olstDtoLiquidacion[0].Iva + ";" + response.olstDtoLiquidacion[0].Tipo;
                        foreach (ServiceDtoDatosLiquidacion item in response.olstDtoLiquidacion)
                        {

                            DatosLiquidacion oDatosLiquidacionService = new DatosLiquidacion();
                            oDatosLiquidacionService.Tipo = item.Tipo;
                            oDatosLiquidacionService.SubTotal = item.SubTotal;
                            oDatosLiquidacionService.Iva = item.Iva;
                            oDatosLiquidacionService.Total = item.Total;

                            lstDatosLiquidacion.Add(oDatosLiquidacionService);
                        }

                        oResultadoOperacion.ListaEntidadDatos = lstDatosLiquidacion;
                    }
                }
                else 
                {
                    oResultadoOperacion.Mensaje = response.Message;
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                }
            }
            else 
            {
                oResultadoOperacion.Mensaje = response.Message;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarPagoEfectivo(string Conexion, ConsultarValorResult oConsultarValorResult)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string bResult = string.Empty;

            getCityParking_Request request = new getCityParking_Request();
            request.RequestId = NuevoRequestId;

            ServiceConsultarResult oServiceConsultarResult = new ServiceConsultarResult();
            oServiceConsultarResult.IdEntrada = oConsultarValorResult.idEntrada;
            oServiceConsultarResult.PlacaSalida = oConsultarValorResult._PlacaSalida;
            oServiceConsultarResult.ValorAPagar = oConsultarValorResult.valorAPagar;
            oServiceConsultarResult.ValorServicio = oConsultarValorResult.valorServicio;
            oServiceConsultarResult.ValorDescuento = oConsultarValorResult.valorDescuento;
            oServiceConsultarResult.ValorEmpresa = oConsultarValorResult.valorEmpresa;
            oServiceConsultarResult.FechaLiquidacion = oConsultarValorResult.fechaLiquidacion;
            oServiceConsultarResult.SerialMaquina = oConsultarValorResult.serialMaquina;

            request.oConsultarResult = oServiceConsultarResult;
            request.sIp = Conexion;

            getCityParking_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarPagoEfectivo(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bResult = response.oResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bResult;

            return oResultadoOperacion;
        }


        public ResultadoOperacion ObtenerDatosConvenio(long IdConvenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string NombreConvenio = string.Empty;

            getDatosConvenio_Request request = new getDatosConvenio_Request();
            request.RequestId = NuevoRequestId;

            request.idConvenio = IdConvenio;

            getDatosConvenio_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getDatosConvenio(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        NombreConvenio = response.sNombreConvenio;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getParametrosModulo";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getParametrosModulo";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = NombreConvenio;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarConvenioValidado(string Consecutivo, string CodigoCompleto, string IdModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioValidado_Request request = new setRegistrarConvenioValidado_Request();
            request.RequestId = NuevoRequestId;

            request.sCodigoCompleto = Consecutivo;
            request.sConsecutivo = CodigoCompleto;
            request.sIdModulo = IdModulo;


            setRegistrarConvenioValidado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarConvenioValidado(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bRegistro;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarConvenio(string sCodigo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bResult = false;

            getValidarConvenio_Request request = new getValidarConvenio_Request();
            request.RequestId = NuevoRequestId;


            request.sCodigo = sCodigo;

            getValidarConvenio_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarConvenio(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bResult = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bResult;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerAutorizado(Autorizado oAutorizado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoAutorizado> olstDtoAutorizado = new List<DtoAutorizado>();


            getInfoAutorizado_Request request = new getInfoAutorizado_Request();
            request.RequestId = NuevoRequestId;

            ServiceAutorizado oServiceAutorizado = new ServiceAutorizado();
            oServiceAutorizado.IdTarjeta = oAutorizado.IdTarjeta;
            if (oAutorizado.Placa1 == string.Empty)
            {
                oServiceAutorizado.Placa1 = null;
            }
            else
            {
                oServiceAutorizado.Placa1 = oAutorizado.Placa1;
            }
            request.oAutorizado = oServiceAutorizado;

            getInfoAutorizado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getInfoAutorizado(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "RegistrarTransaccion() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        olstDtoAutorizado = Mapper.FromDataTransferObjects(response.olstDtoAutorizado);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "RegistrarTransaccion() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "RegistrarTransaccion() error response == null";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = olstDtoAutorizado;
            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarTransaccion(string IdTarjeta, DateTime FechaEntrada)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string Secuencia = string.Empty;

            getValidarTransaccion_Request request = new getValidarTransaccion_Request();
            request.RequestId = NuevoRequestId;


            request.sIdTarjeta = IdTarjeta;
            request.sFechaEntrada = FechaEntrada;

            getValidarTransaccion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarTransaccion(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        Secuencia = response.sSecuencia;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = Secuencia;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarIngreso(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string Secuencia = string.Empty;

            setRegistrarTransaccion_Request request = new setRegistrarTransaccion_Request();
            request.RequestId = NuevoRequestId;

            ServiceTransaccion oServiceTransaccion = new ServiceTransaccion();
            oServiceTransaccion.IdTransaccion = oTransaccion.IdTransaccion;
            oServiceTransaccion.CarrilEntrada = oTransaccion.CarrilEntrada;
            oServiceTransaccion.ModuloEntrada = oTransaccion.ModuloEntrada;
            oServiceTransaccion.IdEstacionamiento = oTransaccion.IdEstacionamiento;
            oServiceTransaccion.IdTarjeta = oTransaccion.IdTarjeta;
            oServiceTransaccion.PlacaEntrada = oTransaccion.PlacaEntrada;
            oServiceTransaccion.IdTipoVehiculo = oTransaccion.IdTipoVehiculo;


            request.oTransaccion = oServiceTransaccion;
            setRegistrarTransaccion_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarTransaccionEntrada(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        Secuencia = response.lIdTransaccion.ToString();
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setRegistrarOperacion";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setRegistrarOperacion";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = Secuencia;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerFechaServer()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getFecha_Request request = new getFecha_Request();
            request.RequestId = NuevoRequestId;

            getFecha_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getFecha(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = response.Message;
                        oResultadoOperacion.EntidadDatos = response.oFechaServer;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error response == null";
                return oResultadoOperacion;
            }


            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarConvenio(long IdTransaccion, int Convenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioAplicado_Request request = new setRegistrarConvenioAplicado_Request();
            request.RequestId = NuevoRequestId;

            request.IdTransaccion = IdTransaccion;
            request.Convenio = Convenio;


            setRegistrarConvenioAplicado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarConvenioAplicado(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarTiempoGracia(long IdTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioAplicado_Request request = new setRegistrarConvenioAplicado_Request();
            request.RequestId = NuevoRequestId;

            request.IdTransaccion = IdTransaccion;



            setRegistrarConvenioAplicado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarTiempoGracia(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerFechaConvenio(long IdTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getFecha_Request request = new getFecha_Request();
            request.RequestId = NuevoRequestId;

            request.oIdTransaccion = IdTransaccion;

            getFecha_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getFechaConvenio(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = response.Message;
                        oResultadoOperacion.EntidadDatos = response.oFechaServer;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error response == null";
                return oResultadoOperacion;
            }


            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerTarjetas(long idEstacionamiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTarjetas> lstDtoTarjetas = new List<DtoTarjetas>();

            getTarjetas_Request request = new getTarjetas_Request();
            request.RequestId = NuevoRequestId;

            request.oIdEstacionamiento = idEstacionamiento;

            getTarjetas_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getTarjetas(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoTarjetas = Mapper.FromDataTransferObjects(response.olstDtoTarjetas);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error response == null";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoTarjetas;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerDatosPlaca(string Letra1, string Letra2)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<DtoTransacciones> lstDtoTransacciones = new List<DtoTransacciones>();

            getIdTransacciones_Request request = new getIdTransacciones_Request();
            request.RequestId = NuevoRequestId;

            request.sLetra1 = Letra1;
            request.sLetra2 = Letra2;

            getIdTransacciones_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getConsultaPlaca(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() Exception";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        lstDtoTransacciones = Mapper.FromDataTransferObjects(response.lstDtoTransacciones);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error request.RequestId != response.CorrelationId";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "ObtenerTarjetasModulo() error response == null";
                return oResultadoOperacion;
            }

            oResultadoOperacion.ListaEntidadDatos = lstDtoTransacciones;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarPIN(string PIN, int TipoVehiculo, string IdModulo, long IdEstacionamiento, long IdTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioAplicado_Request request = new setRegistrarConvenioAplicado_Request();
            request.RequestId = NuevoRequestId;

            request.PIN = PIN;
            request.TipoVehiculo = TipoVehiculo;
            request.IdModulo = IdModulo;
            request.IdEstacionamiento = IdEstacionamiento;
            request.IdTransaccion = IdTransaccion;

            setRegistrarConvenioAplicado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarPIN(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarPIN(long PIN)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DtoPIN oDtoPIN = new DtoPIN();

            getValidarConvenio_Request request = new getValidarConvenio_Request();
            request.RequestId = NuevoRequestId;


            request.sPIN = PIN;

            getValidarConvenio_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.getValidarPIN(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        oDtoPIN = Mapper.FromDataTransferObject(response.oDtoPIN);
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: getValidarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: getValidarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = oDtoPIN;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarPAGOPIN(string IdTransaccion,string PIN, long Secuencia)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioAplicado_Request request = new setRegistrarConvenioAplicado_Request();
            request.RequestId = NuevoRequestId;

            request.PIN = PIN;
            request.IdTarjeta = IdTransaccion;
            request.IdTransaccion = Secuencia;

            setRegistrarConvenioAplicado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarPAGOPIN(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarEvento(long IdTransaccion, string IdTarjeta, long IdEvento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool bRegistro = false;

            setRegistrarConvenioAplicado_Request request = new setRegistrarConvenioAplicado_Request();
            request.RequestId = NuevoRequestId;

            request.IdEvento = IdEvento;
            request.IdTarjeta = IdTarjeta;
            request.IdTransaccion = IdTransaccion;

            setRegistrarConvenioAplicado_Response response = null;

            try
            {
                SafeProxy.DoAction<ModuloServiceClient>(_Ds_ModuloServices, client =>
                { response = client.setRegistrarTansaccionEvento(request); });
            }
            catch (System.Exception)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error conexion Modulo Service";
                return oResultadoOperacion;
            }

            if (response != null)
            {
                if (request.RequestId == response.CorrelationId)
                {
                    if (response.Acknowledge == Ds.ServiceProxy.Ds_ModuloComercialService.AcknowledgeType.Success)
                    {
                        bRegistro = response.bResult;
                    }
                    else
                    {
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = response.Message;
                        return oResultadoOperacion;
                    }
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Respuesta Invalida Modulo Service: setGenerarClave";
                    return oResultadoOperacion;
                }
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Respuesta Modulo Service: setGenerarClave";
                return oResultadoOperacion;
            }

            oResultadoOperacion.EntidadDatos = bRegistro;

            return oResultadoOperacion;
        }
    }
}
