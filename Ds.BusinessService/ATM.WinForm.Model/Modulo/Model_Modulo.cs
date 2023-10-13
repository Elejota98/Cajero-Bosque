using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial class Model : IModel
    {
        public ResultadoOperacion ObtenerInfoModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerInformacionModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerInfoFactura(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerInformacionFactura(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerPartesModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerPartesModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerPartesF56Modulo(Modulo oModulo, int Tipo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerPartesF56Modulo(oModulo, Tipo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerParametrosModulo(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerParametrosModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarMovimiento(Movimiento oMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarMovimiento(oMovimiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarMovimientoCentral(Movimiento oMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarMovimientoCentral(oMovimiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ConfirmarOperacion(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ConfirmarOperacion(oOperacion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ConfirmarOperacionCentral(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.ConfirmarOperacionCentral(oOperacion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarTransaccion(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarTransaccion(oTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion CrearAlarma(Alarma oAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.CrearAlarma(oAlarma);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion CrearAlarmaCentral(Alarma oAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.CrearAlarmaCentral(oAlarma);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion SolucionarTodasAlarmas(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.SolucionarTodasAlarmas(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarSaldosMinimos(Modulo oModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ValidarSaldosMinimos(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarOperacion(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarOperacion(oTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarArqueo(oArqueo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarCarga(Carga oCarga)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarCarga(oCarga);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarOperacionCentral(Operacion oOperacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarOperacionCentral(oOperacion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerUsuario(Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerUsuario(oUsuario);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarClave(long Identificacion, string sClave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ValidarClave(Identificacion, sClave);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion GenerarToken()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.GenerarToken();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion GenerarClave(Modulo oModulo, Usuario oUsuario)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.GenerarClave(oModulo, oUsuario);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarSegundaClave(Modulo oModulo, string sClave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ////oResultadoOperacion = _ProxyServicios.ValidarSegundaClaveCentral(oModulo, sClave);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        //public List<DtoParteModulo> ObtenerDatosCargaActual(TraePagina oParametrosTraePagina)
        //{
        //    List<DtoParteModulo> lstParteModulo = new List<DtoParteModulo>();

        //    ResultadoOperacion oResultadoOperacion = _ProxyServicios.ObtenerDatosCargaActual(oParametrosTraePagina);

        //    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
        //    {
        //        lstParteModulo = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

        //        //if (lstGeneros.Count > 0)
        //        //{
        //        //    if (oTipoLista == TipoLista.Seleccion)
        //        //    {
        //        //        DtoGenero oDtoGenero = new DtoGenero();
        //        //        oDtoGenero.CodigoGenero = 0;
        //        //        string sSeleccion = AdministradorMensaje.Instance.GetMensajePorCodigo(Mensajes.CodigoMensaje.General_TextoListaSeleccion);
        //        //        oDtoGenero.DescripcionGenero = sSeleccion;
        //        //        lstGeneros.Insert(0, oDtoGenero);
        //        //    }
        //        //    else if (oTipoLista == TipoLista.Filtro)
        //        //    {
        //        //        DtoGenero oDtoGenero = new DtoGenero();
        //        //        oDtoGenero.CodigoGenero = 0;
        //        //        string sFiltro = AdministradorMensaje.Instance.GetMensajePorCodigo(Mensajes.CodigoMensaje.General_TextoListaFiltro);
        //        //        oDtoGenero.DescripcionGenero = sFiltro;
        //        //        lstGeneros.Insert(0, oDtoGenero);
        //        //    }
        //    }


        //    return lstParteModulo;
        //}

        public ResultadoOperacion ObtenerIdTransacciones(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerIdTransacciones(oTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerIdTransaccionesCentral(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.ObtenerIdTransaccionesCentral(oTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerLogMovimiento(LogMovimiento oLogMovimiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerLogMovimiento(oLogMovimiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerTransaccionOffline()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerTransaccionesOfflineModulo();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerPagosOffline(int Id_Transaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerPagosOffline(Id_Transaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerMovimientosOffline(int Id_Pago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerMovimientosOffline(Id_Pago);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ActualizarRegistro(int Id_Pago)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ActualizarRegistro(Id_Pago);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarEstadoModulo(EstadoModulo oModulo, string sUrl)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarEstado(oModulo, sUrl);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarAccion(Modulo oModulo, string sAccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarAccionCentral(oModulo, sAccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ObtenerDetalleArqueo(Arqueo oArqueo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerDetalleArqueo(oArqueo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerUsuarioCentral(long Identificacion, string sClave)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.ObtenerUsuarioCentral(Identificacion, sClave);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion Monitoreo(string Modulo, string Estado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.Monitoreo(Modulo, Estado);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ConsultaValor(string sSecuencia, int iTipoVehiculo, bool bMensualidad, bool bReposicion, string Convenios)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ConsultaValor(sSecuencia, iTipoVehiculo,bMensualidad,bReposicion,Convenios);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarPagoEfectivo(string Conexion, ConsultarValorResult oConsultarValorResult)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarPagoEfectivo(Conexion, oConsultarValorResult);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion ConsultaPagoCelular(string Conexion, ConsultarValorResult oConsultarValorResult)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.ConsultaPagoCelular(Conexion, oConsultarValorResult);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarPagoCelular(string Conexion, ConsultarValorResult oConsultarValorResult)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarPagoCelular(Conexion, oConsultarValorResult);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarPagoDatafono(string Conexion, ConsultarValorResult oConsultarValorResult)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarPagoDatafono(Conexion, oConsultarValorResult);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }
        public ResultadoOperacion RegistrarPagoPrepago(string Conexion, ConsultarValorResult oConsultarValorResult, TarjetaSmart oTarjetaSmart)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //oResultadoOperacion = _ProxyServicios.RegistrarPagoPrepago(Conexion, oConsultarValorResult,oTarjetaSmart);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion DatosConvenio(long IdConvenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerDatosConvenio(IdConvenio);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarConvenioValidado(string Consecutivo, string CodigoCompleto, string IdModulo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarConvenioValidado(Consecutivo,CodigoCompleto,IdModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarConvenio(string Codigo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ValidarConvenio(Codigo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerAutorizado(Autorizado oAutorizado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerAutorizado(oAutorizado);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ValidarTransaccion(string IdTarjeta, DateTime FechaEntrada)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ValidarTransaccion(IdTarjeta, FechaEntrada);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarIngreso(Transaccion oTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarIngreso(oTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerFechaServer()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerFechaServer();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion RegistrarConvenio(long IdTransaccion, int Convenio)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.RegistrarConvenio(IdTransaccion,Convenio);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerFechaConvenio(long IdTransaccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerFechaConvenio(IdTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerTarjetas(long idEstacionamiento)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = _ProxyServicios.ObtenerTarjetas(idEstacionamiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                // Generar Alarma para Base de Datos
            }

            return oResultadoOperacion;
        }


    }
}
