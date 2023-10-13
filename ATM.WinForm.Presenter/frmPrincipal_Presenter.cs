using ATM.WinForm.Presenter.Base;
using ATM.WinForm.View;
using Ds.BusinessObjects.DataTransferObject;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.CashPaymentDevice;
using Ds.Datafono;
using Ds.Utilidades;
using Ds.LectorDevice;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using GS.Apdu;
using GS.Util.Hex;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CredibancoDevice;
using Ds.SmartPayOut;
using Ds.AzcoyenDevice;

namespace ATM.WinForm.Presenter
{
    public class frmPrincipal_Presenter : Presenter<IView_Principal>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">The view</param>
        public frmPrincipal_Presenter(IView_Principal view)
            : base(view)
        {
            oPaymentDevice.DeviceMessage += new EventHandler(oPaymentDevice_DeviceMessage);
            _F56Device.DeviceMessageF56State += new EventHandler(oF56EventDevice_DeviceMessage);
            _F56Device.DeviceMessageSerialPort += new EventHandler(oF56SerialDevice_DeviceMessage);
            BilleteroSerial.Instancia.EventoLectura += new BilleteroSerial.Enviar(delegate(string Lugar, int Denominacion) { DineroRecibido(Lugar, Denominacion); });
            BilleteroSerial.Instancia.EventoSecuencia += new BilleteroSerial.Funcion(delegate(string t) { EventosBilletero(t); });
            BilleteroSerial.Instancia.EventoDispensar += new BilleteroSerial.Funcion2(delegate(string estado, int den1, int cant1, int den2, int cant2, int den3, int cant3) { EventosDispensar(estado, den1, cant1, den2, cant2, den3, cant3); });
            Datafono.Instancia.EventoRespuesta += new Datafono.Funcion(delegate(string t) { EventosDatafono(t); });
            DataTable ArqueoCoinBag = new DataTable();
            oCRT288Device.DeviceMessageCRTState += new EventHandler(oCRT288Device_DeviceMessage);
            oDatafonoDevice.DeviceMessageDatafonoState += new EventHandler(oDatafonoDevice_DeviceMessage);
            oSmartPayOutDevice.DeviceMessagePayOutState += new EventHandler(oSmartPayOutDevice_DeviceMessage);
            oSmartPayOutDevice.DeviceMessageSerialPort += new EventHandler(oSmartPayOutSerialDevice_DeviceMessage);
            oAzcoyenDevice.DeviceMessageAzkState += new EventHandler(oAzcoyenDevice_DeviceMessage);
            oAzcoyenDevice.DeviceMessageSerialPort += new EventHandler(oAzcoyenSerialDevice_DeviceMessage);
        }


        private static object syncLock = new object();
        F56Device _F56Device = new F56Device();
        CRT288DeviceClass oCRT288Device = new CRT288DeviceClass();
        SmartPayOutDeviceClass oSmartPayOutDevice = new SmartPayOutDeviceClass();
        AzcoyenDeviceClass oAzcoyenDevice = new AzcoyenDeviceClass();
        PCSCReader reader = new PCSCReader();
        PrintDocument oPrintDocument = new PrintDocument();
        PaymentDevice oPaymentDevice = new PaymentDevice();
        DispositivoPago oDispositivoPago = new DispositivoPago();
        DatafonoDeviceClass oDatafonoDevice = new DatafonoDeviceClass();
        private int DenomCassette1;
        string SecuenciaTransaccion = string.Empty;
        private int DenomCassette2;
        private int DenomCassette3;
        private int DenomMenor;
        Lectora_CRT288 oReader = new Lectora_CRT288();
        ITarjeta oTarjeta;
        SMARTCARD_PARKING_V1 spTarjeta = new SMARTCARD_PARKING_V1();
        Rspsta_Conexion_LECTOR RspConexion = new Rspsta_Conexion_LECTOR();
        Rspsta_Leer_Tarjeta_LECTOR RspLeer = new Rspsta_Leer_Tarjeta_LECTOR();
        Rspsta_Escribir_Tarjeta_LECTOR RspEscribir = new Rspsta_Escribir_Tarjeta_LECTOR();
        /// <summary>
        /// BLOQUE-SECTOR
        /// </summary>
        /// 
        Rspsta_LecturaTarjeta_SectorBloque_LECTOR RspLect = new Rspsta_LecturaTarjeta_SectorBloque_LECTOR();
        Rspsta_CheckPassword_Tarjeta_LECTOR RspCheck = new Rspsta_CheckPassword_Tarjeta_LECTOR();
        Rspsta_EstablecerClave_LECTOR RspClave = new Rspsta_EstablecerClave_LECTOR();
        Rspsta_CodigoTarjeta_LECTOR RspId = new Rspsta_CodigoTarjeta_LECTOR();
        private int DenomHopper1 = 0;
        private int DenomHopper2 = 0;
        private int DenomHopper3 = 0;
        private int DenomHopper4 = 0;
        TicketArqueo oTicketArqueo = new TicketArqueo();
        TicketCarga oTicketCarga = new TicketCarga();
        DataTable ArqueoCoinBag = new DataTable();
        DataTable CargaPreliminarBilletes;
        DataTable CargaPreliminarBilletesTicket;
        DataTable CargaPreliminarMonedas;
        string TipoCuenta = string.Empty;
        int RESULT = 0;
        double VALORCAMBIO = 0;

        #region Modulo
        public DateTime ObtenerFechaServer()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DateTime fecha = new DateTime();

            oResultadoOperacion = Model.ObtenerFechaServer();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                fecha = (DateTime)oResultadoOperacion.EntidadDatos;
                View.General_EventsBill = "(Presenter ObtenerFechaServer) OK";

            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_EventsBill = "(Presenter ObtenerFechaServer) Error";
            }

            return fecha;
        }
        public DateTime ObtenerFechaConvenio()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            DateTime fecha = new DateTime();

            oResultadoOperacion = Model.ObtenerFechaConvenio(Convert.ToInt64(View.Operacion.ID_Transaccion));

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                fecha = (DateTime)oResultadoOperacion.EntidadDatos;
                View.General_Events = "(Presenter ObtenerFechaServer) OK";

            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "(Presenter ObtenerFechaServer) Error";
            }

            return fecha;
        }
        public void RegistrarAlarma(TipoAlarma oTipoAlarma, string sParte, string sDescripcion, int NivelError)
        {
            //bool ok = false;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Alarma oAlarma = new Alarma();
            oAlarma.IdCajero = View.DtoModulo.IdModulo;
            oAlarma.NombreParte = sParte.ToString();
            oAlarma.TipoError = oTipoAlarma.ToString();
            oAlarma.Descripcion = sDescripcion;
            oAlarma.NivelError = NivelError;
            oAlarma.IdSede = View.DtoModulo.IdSede;
            oResultadoOperacion = Model.CrearAlarma(oAlarma);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-RegistrarAlarma -> " + oResultadoOperacion.Mensaje + " Parte: " + oAlarma.NombreParte + " Descripcion: " + oAlarma.Descripcion;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-RegistrarAlarma -> " + oResultadoOperacion.Mensaje;
            }

            oResultadoOperacion = Model.CrearAlarmaCentral(oAlarma);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-RegistrarAlarma -> " + oResultadoOperacion.Mensaje + " Parte: " + oAlarma.NombreParte + " Descripcion: " + oAlarma.Descripcion;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-RegistrarAlarma -> " + oResultadoOperacion.Mensaje;
            }
        }
        public void SolucionarAlarmas()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            oResultadoOperacion = Model.SolucionarTodasAlarmas(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-SolucionarAlarmas OK " + oResultadoOperacion.Mensaje;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-SolucionarAlarmas: " + oResultadoOperacion.Mensaje;
            }
        }
        public void Monitoreo(string Estado)
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            oResultadoOperacion = Model.Monitoreo(oModulo.ID_Modulo, Estado);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-Monitoreo OK " + oResultadoOperacion.Mensaje;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-Monitoreo: " + oResultadoOperacion.Mensaje;
            }
        }
        public bool ObtenerInfoModulo()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerInfoModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModulo = (DtoModulo)oResultadoOperacion.EntidadDatos;
                View.General_Events = "Presenter-ObtenerInfoModulo OK " + oResultadoOperacion.Mensaje;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ObtenerInfoModulo: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ObtenerInfoModulo -> Sistema Suspendido - Info Modulo Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool ObtenerInfoFactura()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerInfoFactura(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModulo.Factura = (DtoFactura)oResultadoOperacion.EntidadDatos;
                View.General_Events = "Presenter-ObtenerInfoFactura OK " + oResultadoOperacion.Mensaje;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ObtenerInfoFactura: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ObtenerInfoFactura -> Sistema Suspendido - Info factura Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool ObtenerInfoParametros()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.iCodigoEstacionamiento;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            View.DtoModulo.Parametros.Clear();
            oResultadoOperacion = Model.ObtenerParametrosModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModulo.Parametros = (List<DtoParametro>)oResultadoOperacion.ListaEntidadDatos;

                View.General_Events = "Presenter-ObtenerInfoParametros OK " + oResultadoOperacion.Mensaje;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ObtenerInfoParametros: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ObtenerInfoParametros -> Sistema Suspendido - Info Parametros Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool ValidarClave(string Identificacion, string Clave)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            long ss = Convert.ToInt64(Identificacion);
            oResultadoOperacion = Model.ValidarClave(ss, Clave);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {

                if (oResultadoOperacion.EntidadDatos != string.Empty && oResultadoOperacion.EntidadDatos != null)
                {
                    View.IdEmpresa = oResultadoOperacion.EntidadDatos.ToString();
                    ok = true;
                }
                else
                {
                    //oResultadoOperacion = Model.ObtenerUsuarioCentral(Identificacion, Clave);
                    //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    //{
                    //    View.DtoUsuario.lstDtoPerfil = (List<DtoPerfil>)oResultadoOperacion.ListaEntidadDatos;
                    //}
                    //else
                    //{
                    //    //View.Presentacion = Pantalla.TransaccionNoResponde;
                    //}
                }
            }
            else
            {
                //View.Presentacion = Pantalla.TransaccionNoResponde;
            }
            return ok;
        }
        public string GenerarToken()
        {
            string token = string.Empty;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.GenerarToken();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                token = oResultadoOperacion.EntidadDatos.ToString();
            }

            return token;
        }
        public bool GenerarClave()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.GenerarClave(oModulo, View.Usuario);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {

                if ((bool)oResultadoOperacion.EntidadDatos == true)
                {
                    ok = true;
                }
            }
            else
            {
                //View.Presentacion = Pantalla.TransaccionNoResponde;
            }
            return ok;
        }
        public bool ValidarSegundaClave(string Clave)
        {

            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ValidarSegundaClave(oModulo, Clave);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if ((bool)oResultadoOperacion.EntidadDatos == true)
                {
                    ok = true;
                }
            }
            else
            {
                //View.Presentacion = Pantalla.TransaccionNoResponde;
            }
            return ok;
        }
        public bool ValidarSaldosMinimos()
        {
            //return true;
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ValidarSaldosMinimos(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if ((TipoValidarSaldosMinimos)oResultadoOperacion.EntidadDatos == TipoValidarSaldosMinimos.True)
                {
                    ok = false;
                    View.General_Events = "Presenter-ValidarSaldosMinimos -> Sistema Suspendido - Saldos Insuficientes";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                }
                else if ((TipoValidarSaldosMinimos)oResultadoOperacion.EntidadDatos == TipoValidarSaldosMinimos.False)
                {
                    ok = true;
                }

                View.General_Events = "Presenter-ValidarSaldosMinimos: " + oResultadoOperacion.Mensaje;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ValidarSaldosMinimos: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter ValidarSaldosMinimos -> Sistema Suspendido - Validación Saldos Mínimos Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool RegistrarOperacion(TipoOperacion oTipoOperacion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Transaccion oTransaccion = new Transaccion();
            SecuenciaTransaccion = string.Empty;
            if (View.TipoPago == "MENSUALIDAD")
            {
                SecuenciaTransaccion = View.NumeroDocumentoOrigen.ToString();
                oTransaccion.IdTransaccion = Convert.ToInt64(SecuenciaTransaccion);
            }
            else
            {

                DateTime Entrada = Convert.ToDateTime(View.Tarjeta.DateTimeEntrance);
                string IdTransaccion = Entrada.ToString("yyyyMMddHHmmss");
                int Carril = 0;

                if (View.Tarjeta.EntranceModule.Trim().Length > 5)
                {
                    string temp = View.Tarjeta.EntranceModule.Trim().Substring(4, 2);
                    Carril = Convert.ToInt32(temp);
                }
                else
                {
                    string Modulo = View.Tarjeta.EntranceModule.Trim();
                    if (Modulo == "ADM01")
                    {
                        Carril = 30;
                    }
                }

                SecuenciaTransaccion = IdTransaccion + Carril + Globales.iCodigoEstacionamiento;
                oTransaccion.IdTransaccion = Convert.ToInt64(SecuenciaTransaccion);

            }

            //oTransaccion.ID_Transaccion = 2020031612045771;


            oTransaccion.IdModulo = View.DtoModulo.IdModulo;
            oTransaccion.IdSede = Convert.ToInt64(Globales.iCodigoEstacionamiento);

            if ((oTipoOperacion == TipoOperacion.Carga) || (oTipoOperacion == TipoOperacion.ArqueoParcial) || (oTipoOperacion == TipoOperacion.ArqueoTotal))
            {

                if (View.Usuario.IdCriptUsuario != null)
                {
                    View.Operacion.ID_Usuario = View.Usuario.IdCriptUsuario.ToString();
                    ok = true;
                }

                #region old
                List<DtoIngresos> lstDtoIngresos = null;

                #endregion
            }
            else if (oTipoOperacion == TipoOperacion.Pago || oTipoOperacion == TipoOperacion.Mensualidad || oTipoOperacion == TipoOperacion.Reposicion || oTipoOperacion == TipoOperacion.CobroTarjetaMensual)
            {
                View.Operacion.ID_Usuario = string.Empty;
                ok = true;
            }
            if (ok)
            {

                oResultadoOperacion = Model.RegistrarOperacion(oTransaccion);
                ok = false;

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {

                    View.IdTransaccion = (string)oResultadoOperacion.EntidadDatos;
                    View.General_Events = "Presenter-RegistrarOperacion IdTransaccion" + View.IdTransaccion;
                    View.Operacion.ID_Transaccion = Convert.ToInt64(View.IdTransaccion);
                    View.General_Events = "Presenter-RegistrarOperacion: " + oResultadoOperacion.Mensaje + " ID_Operacion: " + View.IdTransaccion;
                    View.General_EventsBill = "Presenter-RegistrarOperacion: " + oResultadoOperacion.Mensaje + " ID_Operacion: " + View.IdTransaccion;
                    ok = true;
                    if (oTipoOperacion == TipoOperacion.Carga)
                    {
                        oTicketCarga.CodigoCarga = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion.ToString();
                        oTicketCarga.UsuarioCarga = View.Operacion.ID_Usuario;
                        oTicketCarga.FechaCarga = DateTime.Now;
                        oTicketCarga.ModuloCarga = View.Operacion.ID_Modulo;
                    }
                    else if (oTipoOperacion == TipoOperacion.ArqueoParcial || oTipoOperacion == TipoOperacion.ArqueoTotal)
                    {
                        oTicketArqueo.CodigoArqueo = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion.ToString();
                    }
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_Events = "Error Presenter-RegistrarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter RegistrarOperacion -> ConsultaFallida - Registro Operación Error";
                    ExpulsarTarjeta();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                }
            }

            return ok;
        }
        public bool RegistrarArqueo(string Tipo)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Arqueo oArqueo = new Arqueo();
            oArqueo.IdModulo = Globales.sSerial;
            oArqueo.IdSede = Convert.ToInt64(Globales.iCodigoEstacionamiento);
            oArqueo.IdUsuario = Convert.ToInt64(View.Usuario.IdCriptUsuario);
            oArqueo.Tipo = Tipo;

            oResultadoOperacion = Model.RegistrarArqueo(oArqueo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.IdArqueo = (string)oResultadoOperacion.EntidadDatos;
                View.General_Events = "Presenter-RegistrarArqueo: OK ID_Arqueo: " + View.IdArqueo;
                oTicketArqueo.CodigoArqueo = View.IdArqueo;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-RegistrarArquep: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter RegistrarArqueo -> Sistema Suspendido - Registro Arqueo Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool RegistrarCarga()
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Carga oCarga = new Carga();
            oCarga.IdModulo = Globales.sSerial;
            oCarga.IdSede = Convert.ToInt64(Globales.iCodigoEstacionamiento);
            oCarga.IdUsuario = Convert.ToInt64(View.Usuario.IdCriptUsuario);


            oResultadoOperacion = Model.RegistrarCarga(oCarga);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.IdCarga = (string)oResultadoOperacion.EntidadDatos;
                View.General_Events = "Presenter-RegistrarCarga: OK ID_Carga: " + View.IdCarga;
                oTicketCarga.CodigoCarga = View.IdCarga;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-RegistrarCarga: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter RegistrarCarga -> Sistema Suspendido - Registro Carga Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool RegistrarOperacionCentral(TipoOperacion oTipoOperacion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            DataTable oDataTable = new DataTable();
            DataSet DS = new DataSet("IdTransacciones");

            View.Operacion.ID_Modulo = View.DtoModulo.IdModulo;
            View.Operacion.TipoOperacion = oTipoOperacion;
            View.Operacion.ID_Operacion = 0;

            if ((oTipoOperacion == TipoOperacion.Carga) || (oTipoOperacion == TipoOperacion.ArqueoParcial) || (oTipoOperacion == TipoOperacion.ArqueoTotal))
            {

                if (View.Usuario.IdCriptUsuario != null)
                {
                    View.Operacion.ID_Usuario = View.Usuario.IdCriptUsuario.ToString();
                    ok = true;
                }

                #region old
                List<DtoIngresos> lstDtoIngresos = null;

                #endregion
            }
            else if (oTipoOperacion == TipoOperacion.Pago)
            {
                View.Operacion.ID_Usuario = string.Empty;
                #region idtransacciones
                List<DtoIngresos> lstDtoIngresos = ObtenerIdTransaccionesCentral();

                if (lstDtoIngresos != null)
                {
                    View.Operacion.ID_Transaccion = Convert.ToInt32(lstDtoIngresos[0].Codigo);

                    ok = true;
                }
                else
                {
                    return ok;
                }
                #endregion
            }
            if (ok)
            {

                oResultadoOperacion = Model.RegistrarOperacionCentral(View.Operacion);
                ok = false;

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {

                    //View.Operacion.ID_Operacion = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion;
                    //View.Operacion.ID_Fake_Operacion = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion.ToString();

                    //Thread ohilo = new Thread(unused => CapturaFoto("_" + oTipoOperacion.ToString() + "_"));
                    //ohilo.Start();

                    View.General_Events = "Presenter-RegistrarOperacion: " + oResultadoOperacion.Mensaje + " ID_Operacion: " + View.Operacion.ID_Operacion;
                    ok = true;
                    if (oTipoOperacion == TipoOperacion.Carga)
                    {
                        //oTicketCarga.CodigoCarga = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion.ToString();
                        //oTicketCarga.UsuarioCarga = View.Operacion.ID_Usuario;
                        //oTicketCarga.FechaCarga = DateTime.Now;
                        //oTicketCarga.ModuloCarga = View.Operacion.ID_Modulo;
                    }
                    else if (oTipoOperacion == TipoOperacion.ArqueoParcial || oTipoOperacion == TipoOperacion.ArqueoTotal)
                    {
                        //oTicketArqueo.CodigoArqueo = ((DtoOperacion)oResultadoOperacion.EntidadDatos).ID_Operacion.ToString();
                    }
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_Events = "Error Presenter-RegistrarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter-RegistrarOperacion -> Sistema Suspendido - Registro Operación Error";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                }
            }

            return ok;
        }
        public bool RegistrarMovimiento(TipoOperacion oTipoOperacion, TipoParte oParte, TipoMovimiento oTipoMovimiento, int? IdParte, int? Denominacion, int Cantidad)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Movimiento oMovimiento = new Movimiento();
            oMovimiento.ID_Modulo = View.DtoModulo.IdModulo;
            if (View.ProcesoCarga)
            {
                oMovimiento.IdCarga = Convert.ToInt64(View.IdCarga);
            }
            else
            {

                //oMovimiento.IdTransaccion = Convert.ToInt64(View.IdTransaccion);
                oMovimiento.IdTransaccion = View.Operacion.ID_Transaccion;
            }
            View.General_Events = "idtransaccion movimiento " + oMovimiento.IdTransaccion;
            oMovimiento.IdSede = View.DtoModulo.IdSede;
            oMovimiento.TipoOperacion = oTipoOperacion;
            oMovimiento.TipoAccionMovimiento = oTipoMovimiento;
            oMovimiento.Cantidad = Cantidad;
                        
            if (IdParte == null)
            {
                for (int i = 0; i < View.DtoModulo.Partes.Count; i++)
                {
                    if ((View.DtoModulo.Partes[i].TipoParte == oParte.ToString()) && (Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion) == Denominacion))
                    {
                        oMovimiento.Parte = View.DtoModulo.Partes[i].Nombre;
                        oMovimiento.Denominacion = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < View.DtoModulo.Partes.Count; i++)
                {
                    if ((View.DtoModulo.Partes[i].TipoParte == oParte.ToString()) && (Convert.ToInt32(View.DtoModulo.Partes[i].NumParte) == IdParte))
                    {
                        oMovimiento.Parte = View.DtoModulo.Partes[i].Nombre;
                        oMovimiento.Denominacion = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        break;
                    }
                }
            }
            //}
            //else
            //{

            //    oMovimiento.Parte = "Cass" + IdParte.ToString();
            //    oMovimiento.Denominacion = Convert.ToInt32(Denominacion);
            //}

            oResultadoOperacion = Model.RegistrarMovimiento(oMovimiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-RegistrarMovimiento: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                oResultadoOperacion = Model.RegistrarMovimiento(oMovimiento);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_Events = "Presenter-RegistrarMovimiento: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                    ok = true;
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_Events = "Error Presenter-RegistrarMovimiento: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                    View.General_Events = "Presenter-RegistrarMovimiento -> Sistema Suspendido - Registro Movimiento Error";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    //View.ErrorDiagnostico = "RegistraMovimiento";
                }
            }

            return ok;
        }
        public bool ObtenerInfoPartes()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            View.DtoModulo.Partes.Clear();
            oResultadoOperacion = Model.ObtenerPartesModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModulo.Partes = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

                View.General_Events = "Presenter-ObtenerInfoPartes: " + oResultadoOperacion.Mensaje;
                ok = true;

                if (!View.ConfigModulo)
                {
                    View.ConfigModulo = true;

                    InicializarObjetos();
                    ObtenerCantidadDispositivos(View.DtoModulo);
                    ObtenerDenominaciones(View.DtoModulo);
                    OrdenarDenominacion(Denom_Type.Bill);
                    OrdenarDenominacion(Denom_Type.Coin);
                    ObtenerCantidadesEfectivoBase(View.DtoModulo);
                }
                else
                {
                    ObtenerCantidadesEfectivoBase(View.DtoModulo);
                }

                if (View.ProcesoCarga)
                {
                    if (View.CargaMonedas)
                    {
                        DataTable CargaTotal = ObtenerTablaParte(TipoParte.Hopper);
                        DataTable CargaActual = ObtenerTablaParte(TipoParte.Hopper);

                        if (!View.MonedasStart)
                        {
                            CargaPreliminarMonedas = ObtenerTablaParte(TipoParte.Hopper);
                            View.MonedasStart = true;
                        }

                        for (int i = 0; i < CargaTotal.Rows.Count; i++)
                        {
                            int cantidadcargatotal = Convert.ToInt32(CargaTotal.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargapreliminar = Convert.ToInt32(CargaPreliminarMonedas.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargaactual = cantidadcargatotal - cantidadcargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaCantidad] = cantidadcargaactual.ToString();

                            int dinerocargatotal = Convert.ToInt32(CargaTotal.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargapreliminar = Convert.ToInt32(CargaPreliminarMonedas.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargaactual = dinerocargatotal - dinerocargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaDinero] = dinerocargaactual.ToString();
                        }

                        View.CargaTotalTemporal = CargaTotal;
                        View.CargaActualTemporal = CargaActual;
                    }
                    else if (View.CargaBilletesBB)
                    {
                        DataTable CargaTotal = ObtenerTablaParte(TipoParte.TodosBilletes);
                        DataTable CargaActual = ObtenerTablaParte(TipoParte.TodosBilletes);

                        if (!View.BilletesStart)
                        {
                            CargaPreliminarMonedas = ObtenerTablaParte(TipoParte.TodosBilletes);
                            View.BilletesStart = true;
                        }

                        for (int i = 0; i < CargaTotal.Rows.Count; i++)
                        {
                            int cantidadcargatotal = Convert.ToInt32(CargaTotal.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargapreliminar = Convert.ToInt32(CargaPreliminarMonedas.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargaactual = cantidadcargatotal - cantidadcargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaCantidad] = cantidadcargaactual.ToString();

                            int dinerocargatotal = Convert.ToInt32(CargaTotal.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargapreliminar = Convert.ToInt32(CargaPreliminarMonedas.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargaactual = dinerocargatotal - dinerocargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaDinero] = dinerocargaactual.ToString();
                        }

                        View.CargaTotalTemporal = CargaTotal;
                        View.CargaActualTemporal = CargaActual;
                    }
                }
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ObtenerInfoPartes: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ObtenerInfoPartes -> Sistema Suspendido - Info Partes Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
                ok = false;
            }

            return ok;
        }
        public bool ObtenerInfoPartesF56()
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            View.DtoModulo.Partes.Clear();
            oResultadoOperacion = Model.ObtenerPartesModulo(oModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModulo.Partes = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

                View.General_Events = "Presenter ObtenerInfoPartes: " + oResultadoOperacion.Mensaje;
                ok = true;

                if (!View.ConfigModulo)
                {
                    View.ConfigModulo = true;

                    InicializarObjetos();
                    ObtenerCantidadDispositivos(View.DtoModulo);
                    ObtenerDenominaciones(View.DtoModulo);
                    OrdenarDenominacion(Denom_Type.Bill);
                    OrdenarDenominacion(Denom_Type.Coin);
                    ObtenerCantidadesEfectivoBase(View.DtoModulo);
                }
                else
                {
                    ObtenerCantidadesEfectivoBase(View.DtoModulo);
                }

                if (View.ProcesoCarga)
                {
                    if (View.CargaBilletesF56)
                    {
                        DataTable CargaTotal = ObtenerTablaParte(TipoParte.F56);
                        DataTable CargaActual = ObtenerTablaParte(TipoParte.F56);

                        if (!View.BilletesStart)
                        {
                            CargaPreliminarBilletes = ObtenerTablaParte(TipoParte.F56);
                            CargaPreliminarBilletesTicket = ObtenerTablaParte(TipoParte.TodosBilletes);
                            View.BilletesStart = true;
                        }

                        for (int i = 0; i < CargaTotal.Rows.Count; i++)
                        {
                            int cantidadcargatotal = Convert.ToInt16(CargaTotal.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargapreliminar = Convert.ToInt16(CargaPreliminarBilletes.Rows[i][InfoPresentacion.ColumnaCantidad]);
                            int cantidadcargaactual = cantidadcargatotal - cantidadcargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaCantidad] = cantidadcargaactual.ToString();

                            int dinerocargatotal = Convert.ToInt32(CargaTotal.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargapreliminar = Convert.ToInt32(CargaPreliminarBilletes.Rows[i][InfoPresentacion.ColumnaDinero]);
                            int dinerocargaactual = dinerocargatotal - dinerocargapreliminar;
                            CargaActual.Rows[i][InfoPresentacion.ColumnaDinero] = dinerocargaactual.ToString();
                        }

                        View.CargaTotalTemporal = CargaTotal;
                        View.CargaActualTemporal = CargaActual;
                    }
                }
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ObtenerInfoPartes: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ObtenerInfoPartes -> Sistema Suspendido - Info Partes Error";
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool ObtenerPartesF56Carga()
        {
            bool ok = false;
            Modulo oModulo = new Modulo();
            int Tipo = 0;
            oModulo.ID_Modulo = Globales.sSerial;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerPartesF56Modulo(oModulo, Tipo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModuloF56.PartesF56 = (List<DtoParteModuloF56>)oResultadoOperacion.ListaEntidadDatos;
                View.General_Events = "Presenter-ObtenerInfoPartesF56: " + oResultadoOperacion.Mensaje;
                ok = true;
            }

            return ok;

        }
        public bool ConfirmarOperacion(TipoOperacion oTipoOperacion, TipoEstadoPago oEstadoPago)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            DtoOperacion oDtoOperacion = new DtoOperacion();

            if (oTipoOperacion == TipoOperacion.Pago || oTipoOperacion == TipoOperacion.Casco || oTipoOperacion == TipoOperacion.Evento || oTipoOperacion == TipoOperacion.Mensualidad || oTipoOperacion == TipoOperacion.Reposicion || oTipoOperacion == TipoOperacion.CobroTarjetaMensual)
            {
                if (View.Operacion.Pago.TipoPago == TipoPago.Efectivo)
                {
                    View.Operacion.Pago.TipoPago = TipoPago.Efectivo;
                }
                View.Operacion.Pago.EstadoPago = oEstadoPago;


                if (View.Operacion.Pago.TipoPago != TipoPago.Efectivo)
                {
                    if (View.Operacion.Pago.TipoPago == 0)
                    {
                        if (oTipoOperacion != TipoOperacion.Evento)
                        {
                            View.Operacion.TipoOperacion = TipoOperacion.Pago;
                        }
                        else 
                        {
                            View.Operacion.TipoOperacion = TipoOperacion.Evento;
                        }
                    }
                    else
                    {
                        View.Operacion.TipoOperacion = TipoOperacion.Datafono;
                    }
                }
                else
                {
                    View.Operacion.TipoOperacion = oTipoOperacion;
                }
                View.Operacion.Descripcion = View.Tarjeta.CodeAgreement1.ToString();
                if (View.TipoPago == "REPOSICIÓN")
                {
                    View.Operacion.Operador = "REPO";
                }
                else if (View.TipoPago == "CASCOS")
                {
                    View.Operacion.Operador = "CASCO";
                }
                else if (View.TipoPago == "EVENTO")
                {
                    View.Operacion.Operador = "EVENTO";
                }
                else
                {
                    View.Operacion.Operador = string.Empty;
                }
            }
            else if (oTipoOperacion == TipoOperacion.ArqueoParcial || oTipoOperacion == TipoOperacion.ArqueoTotal)
            {
                View.Operacion.ID_Operacion = Convert.ToInt64(View.IdArqueo);
                View.Operacion.Pago.EstadoPago = oEstadoPago;
                View.Operacion.TipoOperacion = oTipoOperacion;
            }
            else if (oTipoOperacion == TipoOperacion.Carga)
            {
                View.Operacion.ID_Operacion = Convert.ToInt64(View.IdCarga);
                View.Operacion.Pago.EstadoPago = oEstadoPago;
                View.Operacion.TipoOperacion = oTipoOperacion;
            }


            View.Operacion.ValidacionCobro = View.RetornoCobro;

            oResultadoOperacion = Model.ConfirmarOperacion(View.Operacion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                oDtoOperacion = (DtoOperacion)oResultadoOperacion.EntidadDatos;

                ok = true;

                if (oTipoOperacion == TipoOperacion.ArqueoParcial)
                {
                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Valor : " + oDtoOperacion.DtoArqueo.Valor;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Producido : " + oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.ValorArqueo = oDtoOperacion.DtoArqueo.Valor;
                    //oTicketArqueo.ProducidoArqueo = oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.CodigoArqueo = oDtoOperacion.DtoArqueo.ID_Arqueo.ToString();
                    //oTicketArqueo.ModuloArqueo = View.DtoModulo.IdModulo;
                    //oTicketArqueo.UsuarioArqueo = View.Usuario.IdCriptUsuario;
                    //oTicketArqueo.FechaArqueo = DateTime.Now;
                }
                else if (oTipoOperacion == TipoOperacion.ArqueoTotal)
                {

                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Valor : " + oDtoOperacion.DtoArqueo.Valor;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Producido : " + oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.ValorArqueo = oDtoOperacion.DtoArqueo.Valor;
                    //oTicketArqueo.ProducidoArqueo = oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.CodigoArqueo = oDtoOperacion.DtoArqueo.ID_Arqueo;
                    //oTicketArqueo.ModuloArqueo = View.DtoModulo.IdModulo;
                    //oTicketArqueo.UsuarioArqueo = View.Usuario.IdCriptUsuario;
                    //oTicketArqueo.FechaArqueo = DateTime.Now;
                    //View.ProcesoArqueoTotal = false;
                    //if (!SincronizarModulo())
                    //{
                    //    View.General_Events = "Error (Presenter ConfirmarOperacion): No sincroniza";
                    //}
                }
                else if (oTipoOperacion == TipoOperacion.Carga)
                {
                    oTicketCarga.CodigoCarga = View.IdCarga;
                    oTicketCarga.FechaCarga = DateTime.Now;
                    oTicketCarga.ModuloCarga = View.DtoModulo.IdModulo;
                    oTicketCarga.UsuarioCarga = View.Usuario.IdCriptUsuario;
                }
                else if (oTipoOperacion == TipoOperacion.Pago || oTipoOperacion == TipoOperacion.Mensualidad || oTipoOperacion == TipoOperacion.Evento)
                {
                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje + " " + oEstadoPago.ToString();
                    View.DtoOperacion = (DtoOperacion)oResultadoOperacion.EntidadDatos;
                }
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ConfirmarOperacion -> Sistema Suspendido - Confirmación Operación Error " + oTipoOperacion.ToString();
                //View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool ConfirmarOperacionCentral(TipoOperacion oTipoOperacion, TipoEstadoPago oEstadoPago)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            DtoOperacion oDtoOperacion = new DtoOperacion();

            if (oTipoOperacion == TipoOperacion.Pago)
            {
                View.Operacion.Pago.TipoPago = TipoPago.Efectivo;
                View.Operacion.Pago.EstadoPago = oEstadoPago;
                View.Operacion.TipoOperacion = oTipoOperacion;
                View.Operacion.Comision = View.DtoPago[0].Comision;
                View.Operacion.Iva = View.DtoPago[0].Iva;
                View.Operacion.Redondeo = View.DtoPago[0].Redondeo;
                View.Operacion.Total = View.DtoPago[0].Total;
                View.Operacion.CodigoBarras = View.DtoPago[0].CodigoBarras;
                View.Operacion.Pago.Factura = View.DtoPago[0].Factura;

            }

            oResultadoOperacion = Model.ConfirmarOperacionCentral(View.Operacion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                oDtoOperacion = (DtoOperacion)oResultadoOperacion.EntidadDatos;

                ok = true;

                if (oTipoOperacion == TipoOperacion.ArqueoParcial)
                {
                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Valor : " + oDtoOperacion.DtoArqueo.Valor;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Producido : " + oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.ValorArqueo = oDtoOperacion.DtoArqueo.Valor;
                    //oTicketArqueo.ProducidoArqueo = oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.CodigoArqueo = oDtoOperacion.DtoArqueo.ID_Arqueo.ToString();
                    //oTicketArqueo.ModuloArqueo = View.Operacion.ID_Modulo;
                    //oTicketArqueo.UsuarioArqueo = View.Operacion.ID_Usuario;
                    //oTicketArqueo.FechaArqueo = DateTime.Now;
                }
                else if (oTipoOperacion == TipoOperacion.ArqueoTotal)
                {

                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Valor : " + oDtoOperacion.DtoArqueo.Valor;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Producido : " + oDtoOperacion.DtoArqueo.Producido;
                    //oTicketArqueo.ValorArqueo = oDtoOperacion.DtoArqueo.Valor;
                    //oTicketArqueo.ProducidoArqueo = oDtoOperacion.DtoArqueo.Producido;
                    ////oTicketArqueo.CodigoArqueo = oDtoOperacion.DtoArqueo.ID_Arqueo;
                    //oTicketArqueo.ModuloArqueo = View.Operacion.ID_Modulo;
                    //oTicketArqueo.UsuarioArqueo = View.Operacion.ID_Usuario;
                    //oTicketArqueo.FechaArqueo = DateTime.Now;
                    ////View.ProcesoArqueoTotal = false;
                    ////if (!SincronizarModulo())
                    ////{
                    ////    View.General_Events = "Error (Presenter ConfirmarOperacion): No sincroniza";
                    ////}
                }
                else if (oTipoOperacion == TipoOperacion.Pago)
                {
                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje + " " + oEstadoPago.ToString();
                    View.DtoOperacion = (DtoOperacion)oResultadoOperacion.EntidadDatos;
                }
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje;
                View.General_Events = "Presenter-ConfirmarOperacion -> Sistema Suspendido - Confirmación Operación Error " + oTipoOperacion.ToString();
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }

            return ok;
        }
        public bool RegistrarMovimientoCentral(TipoOperacion oTipoOperacion, TipoParte oParte, TipoMovimiento oTipoMovimiento, int? IdParte, int? Denominacion, int Cantidad)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            Movimiento oMovimiento = new Movimiento();
            oMovimiento.ID_Modulo = View.DtoModulo.IdModulo;
            if (View.ProcesoCarga)
            {
                oMovimiento.ID_Operacion = Convert.ToInt64(View.Carga.CodigoCargaCentral);
            }
            else
            {
                oMovimiento.ID_Operacion = View.Operacion.ID_Operacion;
            }
            oMovimiento.TipoOperacion = oTipoOperacion;
            oMovimiento.TipoAccionMovimiento = oTipoMovimiento;
            oMovimiento.Cantidad = Cantidad;

            if (IdParte == null)
            {
                for (int i = 0; i < View.DtoModulo.Partes.Count; i++)
                {
                    if ((View.DtoModulo.Partes[i].TipoParte == oParte.ToString()) && (Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion) == Denominacion))
                    {
                        oMovimiento.Parte = View.DtoModulo.Partes[i].Nombre;
                        oMovimiento.Denominacion = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < View.DtoModulo.Partes.Count; i++)
                {
                    if ((View.DtoModulo.Partes[i].TipoParte == oParte.ToString()) && (Convert.ToInt32(View.DtoModulo.Partes[i].NumParte) == IdParte))
                    {
                        oMovimiento.Parte = View.DtoModulo.Partes[i].Nombre;
                        oMovimiento.Denominacion = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        break;
                    }
                }
            }

            oResultadoOperacion = Model.RegistrarMovimientoCentral(oMovimiento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                ok = true;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                oResultadoOperacion = Model.RegistrarMovimientoCentral(oMovimiento);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_Events = "Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                    ok = true;
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_Events = "Error Presenter-ConfirmarOperacion: " + oResultadoOperacion.Mensaje + " Denominacion: " + oMovimiento.Denominacion + " Cantidad: " + oMovimiento.Cantidad;
                    View.General_Events = "Presenter-ConfirmarOperacion -> Sistema Suspendido - Registro Movimiento Error";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    //View.ErrorDiagnostico = "RegistraMovimiento";
                }
            }

            return ok;
        }
        public bool ActualizarRegistro(int Id_Pago)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ActualizarRegistro(Id_Pago);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if ((bool)oResultadoOperacion.EntidadDatos == true)
                {
                    ok = true;
                }
            }
            else
            {
                //View.Presentacion = Pantalla.TransaccionNoResponde;
            }
            return ok;
        }
        public bool RegistrarAccion(string Accion)
        {

            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.RegistrarAccion(oModulo, Accion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if ((bool)oResultadoOperacion.EntidadDatos == true)
                {
                    ok = true;
                }
            }
            else
            {
                //View.Presentacion = Pantalla.TransaccionNoResponde;
            }
            return ok;
        }
        private DataTable ObtenerTablaParte(TipoParte oTipoParte)
        {
            List<DtoParteModulo> lstParteModulo = View.DtoModulo.Partes;

            DataTable tableParte = new DataTable();
            tableParte.Columns.Add(InfoPresentacion.ColumnaTipoParte);
            tableParte.Columns.Add(InfoPresentacion.ColumnaDenominacion);
            tableParte.Columns.Add(InfoPresentacion.ColumnaCantidad);
            tableParte.Columns.Add(InfoPresentacion.ColumnaDinero);
            if (oTipoParte != TipoParte.Hopper)
            {
                tableParte.Columns.Add(InfoPresentacion.ColumnaCantidadMaxima);
            }

            if (oTipoParte != TipoParte.TodosBilletes)
            {
                foreach (DtoParteModulo Parte in lstParteModulo)
                {
                    if (Parte.TipoParte == oTipoParte.ToString())
                    {
                        DataRow fila = tableParte.NewRow();
                        fila[InfoPresentacion.ColumnaTipoParte] = Parte.TipoParte + Parte.NumParte;
                        fila[InfoPresentacion.ColumnaDenominacion] = Parte.Denominacion;
                        fila[InfoPresentacion.ColumnaCantidad] = Parte.CantidadActual;
                        fila[InfoPresentacion.ColumnaDinero] = Parte.DineroActual;
                        if (oTipoParte != TipoParte.Hopper)
                        {
                            fila[InfoPresentacion.ColumnaCantidadMaxima] = 100;
                        }
                        tableParte.Rows.Add(fila);
                    }
                }
            }
            else
            {
                foreach (DtoParteModulo Parte in lstParteModulo)
                {
                    if (Parte.TipoParte == TipoParte.Box.ToString() || Parte.TipoParte == TipoParte.Cassette.ToString())
                    {
                        DataRow fila = tableParte.NewRow();
                        fila[InfoPresentacion.ColumnaTipoParte] = Parte.TipoParte + Parte.NumParte;
                        fila[InfoPresentacion.ColumnaDenominacion] = Parte.Denominacion;
                        fila[InfoPresentacion.ColumnaCantidad] = Parte.CantidadActual;
                        fila[InfoPresentacion.ColumnaDinero] = Parte.DineroActual;
                        if (oTipoParte != TipoParte.Hopper)
                        {
                            fila[InfoPresentacion.ColumnaCantidadMaxima] = 100;
                        }
                        tableParte.Rows.Add(fila);
                    }
                }
            }

            return tableParte;
        }
        public bool RegistrarConvenioValidado(string Consecutivo,string CodigoCompleto)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            string IdModulo = Globales.sSerial;

            oResultadoOperacion = Model.RegistrarConvenioValidado(Consecutivo,CodigoCompleto,IdModulo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;                    
            }

            return ok;
 
        }
        public bool RegistrarConvenio(long IdTransaccion, int Convenio)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            string IdModulo = Globales.sSerial;

            oResultadoOperacion = Model.RegistrarConvenio(IdTransaccion,Convenio);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;
            }

            return ok;

        }
        public bool RegistrarTiempoGracia(long IdTransaccion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            string IdModulo = Globales.sSerial;

            oResultadoOperacion = Model.RegistrarTiempoGracia(IdTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;
            }

            return ok;

        }
        public bool ValidarConvenio(string Codigo)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
           
            oResultadoOperacion = Model.ValidarConvenio(Codigo);

            if (Convert.ToBoolean(oResultadoOperacion.EntidadDatos) == true)
            {
                View.General_Events = "Codigo ok";
                ok = true;
            }
            else
            {
                View.General_Events = "Codigo no valido";
            } 
            
            return ok;

        }
        public bool ObtenerAutorizado(Autorizado oAutorizado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool ok = false;
            oResultadoOperacion = Model.ObtenerAutorizado(oAutorizado);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "ObtenerAutorizado ok";
                View.lstDtoAutorizado = (List<DtoAutorizado>)oResultadoOperacion.ListaEntidadDatos;
                ok = true;
            }
            else
            {
                View.General_Events = "ObtenerAutorizado Error";
            }

            return ok;
        }
        public bool ObtenerTarjetas()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool ok = false;
            oResultadoOperacion = Model.ObtenerTarjetas(Convert.ToInt64(Globales.iCodigoEstacionamiento));

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.lstDtoTarjetas = (List<DtoTarjetas>)oResultadoOperacion.ListaEntidadDatos;
                ok = true;
            }

            return ok;
        }
        public bool ObtenerTransacciones(string sLetra1, string sLetra2)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            bool ok = false;
            oResultadoOperacion = Model.ObtenerDatosPlaca(sLetra1,sLetra2);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.lstDtoTransacciones = (List<DtoTransacciones>)oResultadoOperacion.ListaEntidadDatos;
                ok = true;
            }

            return ok;
        }
        public bool RegistrarPIN(string PIN, int TipoVehiculo, long IdTransaccion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            string IdModulo = Globales.sSerial;
            long IdEstacionamiento = Convert.ToInt64( Globales.iCodigoEstacionamiento);

            oResultadoOperacion = Model.RegistrarPIN(PIN, TipoVehiculo, IdModulo, IdEstacionamiento, IdTransaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;
            }

            return ok;

        }
        public bool RegistrarPAGOPIN(string PIN)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            
            oResultadoOperacion = Model.RegistrarPAGOPIN(PIN, View.Tarjeta.CodeCard, Convert.ToInt64(View.IdTransaccion));

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;
            }

            return ok;

        }
        public bool RegistraEvento(long IdEvento)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();


            oResultadoOperacion = Model.RegistrarEvento(View.Tarjeta.CodeCard, Convert.ToInt64(View.IdTransaccion),IdEvento);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                ok = true;
            }

            return ok;

        }
        public bool ValidarPIN(long PIN)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ValidarPIN(PIN);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoPIN = (DtoPIN)oResultadoOperacion.EntidadDatos;
                int TipoVehiculo = 0;
                if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                {
                    TipoVehiculo = 1;
                }
                else
                {
                    TipoVehiculo = 2;
                }

                if (View.DtoPIN.TipoVehiculo == TipoVehiculo)
                {
                    DateTime? fechaentrada = View.Tarjeta.DateTimeEntrance;
                    if (View.Tarjeta.DateTimeEntrance > View.DtoPIN.FechaValidacion)
                    {
                        //registar transaccion 
                        if (RegistrarPAGOPIN(View.DtoPIN.PIN))
                        {
                            View.SetearPantalla(Pantalla.GarciasPago);
                        }
                        else 
                        {
                            View.SetearPantalla(Pantalla.TransaccionCancelada);
                        }
                    }
                    else 
                    {
                        View.SetearPantalla(Pantalla.PinInvalido);
                    }
                }
                else
                {
                    View.SetearPantalla(Pantalla.PinInvalido);
                }

                View.General_Events = "Codigo ok";
                ok = true;
            }
            else
            {
                View.General_Events = "Codigo no valido";
                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                Expulsar();
                View.SetearPantalla(Pantalla.PinInvalido);
            }

            return ok;

        }
        #endregion

        #region ModuloCentral
        public bool ListarTransaccionesOffline()
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerTransaccionOffline();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos != null)
                {
                    View.DtoTransacciones = (List<DtoTransacciones>)oResultadoOperacion.EntidadDatos;
                    View.General_Events = "Presenter-ListarTransaccionesOffline -> OK " + oResultadoOperacion.Mensaje;
                    ok = true;
                }
            }
            return ok;
        }
        public bool ListarPagosOffline(int Id_Transaccion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerPagosOffline(Id_Transaccion);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos != null)
                {
                    View.DtoPago = (List<DtoPago>)oResultadoOperacion.EntidadDatos;
                    View.General_Events = "Presenter-ListarPagosOffline -> OK " + oResultadoOperacion.Mensaje;
                    ok = true;
                }
            }
            return ok;
        }
        public bool ListarMovimientosOffline(int Id_Pago)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerMovimientosOffline(Id_Pago);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                if (oResultadoOperacion.EntidadDatos != null)
                {
                    View.DtoLogMovimiento = (List<DtoLogMovimiento>)oResultadoOperacion.EntidadDatos;
                    View.General_Events = "Presenter-ListarMovimientosOffline -> OK " + oResultadoOperacion.Mensaje;
                    ok = true;
                }
            }
            return ok;
        }
        public bool ListarDetalleArqueo(Arqueo oArqueo)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerDetalleArqueo(oArqueo);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoArqueo = (List<DtoArqueo>)oResultadoOperacion.ListaEntidadDatos;
                View.General_Events = "Presenter-ListarDetalleArqueo -> OK " + oResultadoOperacion.Mensaje;

                ArqueoCoinBag = TicketArqu();

                ok = true;

            }
            else
            {
                View.DtoArqueo = new List<DtoArqueo>();
                ArqueoCoinBag = TicketArqu();
            }
            return ok;
        }
        #endregion

        #region General
        private void InicializarObjetos()
        {
            //oDispositivoPago = null;
            //View.Operacion = null;
            //oDispositivoPago = new DispositivoPago();
            //View.Operacion = new Operacion();
        }
        public string ObtenerValorParametro(Parametros idParametro)
        {
            string ValorParametro = string.Empty;
            foreach (DtoParametro item in View.DtoModulo.Parametros)
            {
                if (item.Codigo == idParametro.ToString())
                {
                    ValorParametro = item.Valor;
                    break;
                }
            }
            return ValorParametro;
        }
        public List<DtoIngresos> ObtenerIdTransacciones()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            List<DtoIngresos> lstDtoIngresos = new List<DtoIngresos>();
            Transaccion oTransaccion = new Transaccion();
            oTransaccion.IdModulo = Globales.sSerial;
           // oTransaccion.NumeroDocumentoOrigen = View.NumeroDocumentoOrigen.ToString();

            oResultadoOperacion = Model.ObtenerIdTransacciones(oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-ObtenerIdTransacciones -> ObtenerIdTransacciones exitosa.";
                lstDtoIngresos = (List<DtoIngresos>)oResultadoOperacion.ListaEntidadDatos;
            }
            else
            {
                View.General_Events = "Presenter-ObtenerIdTransacciones -> Error ObtenerIdTransacciones.";
                lstDtoIngresos = null;
            }

            return lstDtoIngresos;
        }
        public List<DtoIngresos> ObtenerIdTransaccionesCentral()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            List<DtoIngresos> lstDtoIngresos = new List<DtoIngresos>();
            Transaccion oTransaccion = new Transaccion();
            oTransaccion.IdModulo = Globales.sSerial;
            //oTransaccion.NumeroDocumentoOrigen = View.NumeroDocumentoOrigen.ToString();

            oResultadoOperacion = Model.ObtenerIdTransaccionesCentral(oTransaccion);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Presenter-ObtenerIdTransacciones -> ObtenerIdTransacciones exitosa.";
                lstDtoIngresos = (List<DtoIngresos>)oResultadoOperacion.ListaEntidadDatos;
            }
            else
            {
                View.General_Events = "Presenter-ObtenerIdTransacciones -> Error ObtenerIdTransacciones.";
                lstDtoIngresos = null;
            }

            return lstDtoIngresos;
        }
        public bool AsignarDenominacionesLocales()
        {
            bool ok = true;
            try
            {
                for (int i = 0; i < View.DtoModulo.Partes.Count; i++)
                {
                    if (View.DtoModulo.Partes[i].TipoParte == TipoParte.Hopper.ToString())
                    {
                        if (View.DtoModulo.Partes[i].Nombre == "Hopper1")
                            DenomHopper1 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        else if (View.DtoModulo.Partes[i].Nombre == "Hopper2")
                            DenomHopper2 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        else if (View.DtoModulo.Partes[i].Nombre == "Hopper3")
                            DenomHopper3 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        else if (View.DtoModulo.Partes[i].Nombre == "Hopper4")
                            DenomHopper4 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                    }
                    else if (View.DtoModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                    {
                        if (View.DtoModulo.Partes[i].Nombre == "Cass1")
                            DenomCassette1 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        else if (View.DtoModulo.Partes[i].Nombre == "Cass2")
                            DenomCassette2 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                        else if (View.DtoModulo.Partes[i].Nombre == "Cass3")
                            DenomCassette3 = Convert.ToInt32(View.DtoModulo.Partes[i].Denominacion);
                    }
                }

                if (DenomCassette1 > DenomCassette2 && DenomCassette1 > DenomCassette3)
                {
                    if (DenomCassette2 > DenomCassette3) DenomMenor = DenomCassette3;
                    else DenomMenor = DenomCassette2;
                }
                else
                {
                    if (DenomCassette2 > DenomCassette3)
                    {
                        if (DenomCassette1 > DenomCassette3) DenomMenor = DenomCassette3;
                        else DenomMenor = DenomCassette1;
                    }
                    else
                    {
                        if (DenomCassette1 > DenomCassette2) DenomMenor = DenomCassette2;
                        else DenomMenor = DenomCassette1;
                    }
                }

               
            }
            catch (Exception ex)
            {
                ok = false;
            }

            return ok; 
        }
        #endregion

        #region Data
        private void ObtenerCantidadDispositivos(DtoModulo oDtoModulo)
        {
            for (int i = 0; i < oDtoModulo.Partes.Count; i++)
            {
                if (oDtoModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                {
                    oDispositivoPago.Num_Cassettes++;
                }
                else if (oDtoModulo.Partes[i].TipoParte == TipoParte.Hopper.ToString())
                {
                    oDispositivoPago.Num_Hoppers++;
                }
            }

            oDispositivoPago.Bill_Denom = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Coin_Denom = new int[oDispositivoPago.Num_Hoppers];
            oDispositivoPago.Cassette_Denom = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Hopper_Denom = new int[oDispositivoPago.Num_Hoppers];
            oDispositivoPago.Bill_Qty = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Coin_Qty = new int[oDispositivoPago.Num_Hoppers];
            oDispositivoPago.Bill_Order = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Coin_Order = new int[oDispositivoPago.Num_Hoppers];
            oDispositivoPago.Bill_Min = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Coin_Min = new int[oDispositivoPago.Num_Hoppers];
            oDispositivoPago.Bill_Alarm = new int[oDispositivoPago.Num_Cassettes];
            oDispositivoPago.Coin_Alarm = new int[oDispositivoPago.Num_Hoppers];
        }
        private string ObtenerDispositivo(int device)
        {
            string unidad = string.Empty;
            switch (device)
            {
                case (int)Pay_Unit.Bill_Validator:
                    unidad = Pay_Unit.Bill_Validator.ToString();
                    break;

                case (int)Pay_Unit.Cashless_System:
                    unidad = Pay_Unit.Cashless_System.ToString();
                    break;

                case (int)Pay_Unit.Coin_Validator:
                    unidad = Pay_Unit.Coin_Validator.ToString();
                    break;

                case (int)Pay_Unit.Hopper1:
                    unidad = Pay_Unit.Hopper1.ToString();
                    break;

                case (int)Pay_Unit.Hopper2:
                    unidad = Pay_Unit.Hopper2.ToString();
                    break;

                //case (int)Pay_Unit.Hopper3:
                //    unidad = Pay_Unit.Hopper3.ToString();
                //    break;

                //case (int)Pay_Unit.Hopper4:
                //    unidad = Pay_Unit.Hopper4.ToString();
                //    break;

                case (int)Pay_Unit.Inputs:
                    unidad = Pay_Unit.Inputs.ToString();
                    break;

                case (int)Pay_Unit.Payment_Manager:
                    unidad = Pay_Unit.Payment_Manager.ToString();
                    break;
            }
            return unidad;
        }
        private void ObtenerDenominaciones(DtoModulo oModulo)
        {
            for (int i = 0, b = 0, h = 0; i < oModulo.Partes.Count; i++)
            {
                if (oModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                {
                    oDispositivoPago.Cassette_Denom[b] = Convert.ToInt32(oModulo.Partes[i].Denominacion);
                    oDispositivoPago.Bill_Denom[b] = Convert.ToInt32(oModulo.Partes[i].Denominacion);
                    oDispositivoPago.Bill_Qty[b] = Convert.ToInt32(oModulo.Partes[i].CantidadActual);
                    oDispositivoPago.Bill_Order[b] = Convert.ToInt32(oModulo.Partes[i].NumParte);
                    oDispositivoPago.Bill_Min[b++] = Convert.ToInt32(oModulo.Partes[i].CantidadMin);
                }
                else if (oModulo.Partes[i].TipoParte == TipoParte.Hopper.ToString())
                {
                    oDispositivoPago.Hopper_Denom[h] = Convert.ToInt32(oModulo.Partes[i].Denominacion);
                    oDispositivoPago.Coin_Denom[h] = Convert.ToInt32(oModulo.Partes[i].Denominacion);
                    oDispositivoPago.Coin_Qty[h] = Convert.ToInt32(oModulo.Partes[i].CantidadActual);
                    oDispositivoPago.Coin_Order[h] = Convert.ToInt32(oModulo.Partes[i].NumParte);
                    oDispositivoPago.Coin_Min[h++] = Convert.ToInt32(oModulo.Partes[i].CantidadMin);
                }
            }
        }
        private void OrdenarDenominacion(Denom_Type oDenomType)
        {
            int veces = 0, elemento = 0, temp = 0;
            int sort_size = 0;

            if (oDenomType == Denom_Type.Bill)
            {
                sort_size = oDispositivoPago.Num_Cassettes;

                for (veces = 1; veces < sort_size; veces++)
                {
                    for (elemento = 0; elemento < sort_size - 1; elemento++)
                    {
                        if (oDispositivoPago.Bill_Denom[elemento] < oDispositivoPago.Bill_Denom[elemento + 1])
                        {
                            temp = oDispositivoPago.Bill_Denom[elemento];
                            oDispositivoPago.Bill_Denom[elemento] = oDispositivoPago.Bill_Denom[elemento + 1];
                            oDispositivoPago.Bill_Denom[elemento + 1] = temp;

                            temp = oDispositivoPago.Bill_Qty[elemento];
                            oDispositivoPago.Bill_Qty[elemento] = oDispositivoPago.Bill_Qty[elemento + 1];
                            oDispositivoPago.Bill_Qty[elemento + 1] = temp;

                            temp = oDispositivoPago.Bill_Order[elemento];
                            oDispositivoPago.Bill_Order[elemento] = oDispositivoPago.Bill_Order[elemento + 1];
                            oDispositivoPago.Bill_Order[elemento + 1] = temp;

                            temp = oDispositivoPago.Bill_Min[elemento];
                            oDispositivoPago.Bill_Min[elemento] = oDispositivoPago.Bill_Min[elemento + 1];
                            oDispositivoPago.Bill_Min[elemento + 1] = temp;
                        }
                    }
                }
            }
            else if (oDenomType == Denom_Type.Coin)
            {
                sort_size = oDispositivoPago.Num_Hoppers;

                for (veces = 1; veces < sort_size; veces++)
                {
                    for (elemento = 0; elemento < sort_size - 1; elemento++)
                    {
                        if (oDispositivoPago.Coin_Denom[elemento] < oDispositivoPago.Coin_Denom[elemento + 1])
                        {
                            temp = oDispositivoPago.Coin_Denom[elemento];
                            oDispositivoPago.Coin_Denom[elemento] = oDispositivoPago.Coin_Denom[elemento + 1];
                            oDispositivoPago.Coin_Denom[elemento + 1] = temp;

                            temp = oDispositivoPago.Coin_Qty[elemento];
                            oDispositivoPago.Coin_Qty[elemento] = oDispositivoPago.Coin_Qty[elemento + 1];
                            oDispositivoPago.Coin_Qty[elemento + 1] = temp;

                            temp = oDispositivoPago.Coin_Order[elemento];
                            oDispositivoPago.Coin_Order[elemento] = oDispositivoPago.Coin_Order[elemento + 1];
                            oDispositivoPago.Coin_Order[elemento + 1] = temp;

                            temp = oDispositivoPago.Coin_Min[elemento];
                            oDispositivoPago.Coin_Min[elemento] = oDispositivoPago.Coin_Min[elemento + 1];
                            oDispositivoPago.Coin_Min[elemento + 1] = temp;
                        }
                    }
                }
            }
        }
        private void ObtenerCantidadesEfectivoBase(DtoModulo oDtoModulo)
        {
            for (int i = 0; i < oDtoModulo.Partes.Count; i++)
            {
                if (oDtoModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                {
                    for (int b = 0; b < oDispositivoPago.Num_Cassettes; b++)
                    {
                        if (oDispositivoPago.Bill_Denom[b] == Convert.ToInt32(oDtoModulo.Partes[i].Denominacion))
                        {
                            oDispositivoPago.Bill_Qty[b] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadActual);
                            oDispositivoPago.Bill_Min[b] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadMin);
                            oDispositivoPago.Bill_Alarm[b] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadAlarma);
                        }
                    }
                }
                else if (oDtoModulo.Partes[i].TipoParte == TipoParte.Hopper.ToString())
                {
                    for (int h = 0; h < oDispositivoPago.Num_Hoppers; h++)
                    {
                        if (oDispositivoPago.Coin_Denom[h] == Convert.ToInt32(oDtoModulo.Partes[i].Denominacion))
                        {
                            oDispositivoPago.Coin_Qty[h] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadActual);
                            oDispositivoPago.Coin_Min[h] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadMin);
                            oDispositivoPago.Coin_Alarm[h] = Convert.ToInt32(oDtoModulo.Partes[i].CantidadAlarma);
                        }
                    }
                }
            }
        }
        public bool RegistrarEstadoPuntoPago(string sPantalla, string sVersion)
        {
            EstadoModulo oModulo = new EstadoModulo();
            oModulo.ID_Modulo = Globales.sSerial;
            oModulo.Nombre_Pantalla = sPantalla;
            oModulo.Version = sVersion;
            string sUrl = Globales.sUrlMonitoreo;

            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.RegistrarEstadoModulo(oModulo, sUrl);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.General_Events = "Exito Presenter-RegistrarEstadoPuntoPago: " + oResultadoOperacion.Mensaje;
                ok = (bool)oResultadoOperacion.EntidadDatos;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_Events = "Error Presenter-RegistrarEstadoPuntoPago: " + oResultadoOperacion.Mensaje;
            }

            return ok;
        }
        
        private ResultadoOperacion GestionarPagoEfectivoMonedas(DispositivoPago oDispositivoPago, PagoEfectivo oPagoEfectivo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int[] coin_cnt_denom = { 0, 0, 0, 0 };
            int coin_index = 0;
            Int64 valor_temp = 0;

            List<TransaccionEfectivo> Transacciones = new List<TransaccionEfectivo>();

            while ((oPagoEfectivo.ValorProcesoCambio >= 0) && (coin_index < oDispositivoPago.Num_Hoppers))
            {
                while (oDispositivoPago.Coin_Qty[coin_index] <= oDispositivoPago.Coin_Min[coin_index])
                {
                    coin_index++;
                    if (coin_index == oDispositivoPago.Num_Hoppers)
                        break;
                }

                if (coin_index <= oDispositivoPago.Num_Hoppers)
                {
                    valor_temp = oPagoEfectivo.ValorProcesoCambio - oDispositivoPago.Coin_Denom[coin_index];

                    if (valor_temp >= 0)
                    {
                        coin_cnt_denom[coin_index]++;
                        oPagoEfectivo.ValorProcesoCambio = valor_temp;
                    }
                    else
                    {
                        if (coin_cnt_denom[coin_index] > 0)
                        {
                            int pago_parcial_hopper = (oDispositivoPago.Coin_Denom[coin_index]) * (coin_cnt_denom[coin_index]);
                            int payout = 0;

                            for (int i = 0; i < oDispositivoPago.Num_Hoppers; i++)
                            {
                                if ((oDispositivoPago.Coin_Denom[coin_index] == oDispositivoPago.Hopper_Denom[i]) && (payout == 0))
                                {
                                    Pay_Unit oPay_Unit = new Pay_Unit();

                                    if (i == 0)
                                        oPay_Unit = Pay_Unit.Hopper1;
                                    else if (i == 1)
                                        oPay_Unit = Pay_Unit.Hopper2;
                                    else if (i == 2)
                                        oPay_Unit = Pay_Unit.Hopper3;
                                    else if (i == 3)
                                        oPay_Unit = Pay_Unit.Hopper4;

                                    ResultadoOperacion oResultParcial = new ResultadoOperacion();
                                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "Set_Coin_Pay: pago_parcial_hopper: " + pago_parcial_hopper + " oPay_Unit " + oPay_Unit, TipoLog.TRAZA);

                                    //NRI                                  

                                    //oResultParcial = oPaymentDevice.Set_Coin_Pay(pago_parcial_hopper, oPay_Unit);

                                    //AZK


                                    Thread.Sleep(500);
                                    oResultParcial = oAzcoyenDevice.PayOut(oDispositivoPago.Hopper_Denom[i], pago_parcial_hopper, oPay_Unit);

                                    if (oResultParcial.oEstado == TipoRespuesta.Exito)
                                    {
                                        payout = (int)oResultParcial.EntidadDatos;
                                        if (payout > 0)
                                        {
                                            TransaccionEfectivo oTransaccionEfectivo = new TransaccionEfectivo();
                                            oTransaccionEfectivo.TipoParte = TipoParte.Hopper;
                                            oTransaccionEfectivo.Denominacion = oDispositivoPago.Hopper_Denom[i];
                                            oTransaccionEfectivo.Cantidad = payout / oDispositivoPago.Hopper_Denom[i];
                                            //oTransaccionEfectivo.IdParte = IdParte;
                                            Transacciones.Add(oTransaccionEfectivo);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        oResultadoOperacion.Mensaje = oResultParcial.Mensaje;
                                    }
                                }
                            }

                            oPagoEfectivo.ValorProcesoCambio += (pago_parcial_hopper - payout);
                        }

                        coin_index++;
                    }
                }
                else
                    break;
            }

            if (Transacciones.Count > 0)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.ListaEntidadDatos = Transacciones;
            }
            else
                oResultadoOperacion.oEstado = TipoRespuesta.Error;

            return oResultadoOperacion;
        }
        #endregion

        #region BilltoBill
        public bool InicializarBilletero()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            //obtener paramettro puerto 
            bool OK = false;

            string COM = Globales.sPuertoBill;

            oResultadoOperacion.EntidadDatos = Model.InicializarBilletero(View.DtoModulo, COM);
            if (Convert.ToBoolean(oResultadoOperacion.EntidadDatos) == true)
            {
                //Trace.WriteLineIf(ts.TraceVerbose, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff") + "-->" + "Método: InicializarBilletero --> Mensaje: Billetero OK");
                View.BanderaEstadoBilletero = true;
                OK = true;
            }
            else
            {
                //Trace.WriteLineIf(ts.TraceVerbose, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff") + "-->" + "Método: InicializarBilletero --> Mensaje ERROR:" + oResultadoOperacion.Mensaje);
                //RegistrarAlarma(TipoAlarma.Billetero, oResultadoOperacion.Mensaje);
                View.BanderaEstadoBilletero = false;
                OK = false;
            }

            return OK;
        }
        private async void DineroRecibido(string Lugar, int Denominacion)
        {
            View.Cnt_Reinicio = 0;
            TipoParte Destino = 0;
            if (Lugar == "Box") { Destino = TipoParte.Box; }
            else if (Lugar == "Cassette") { Destino = TipoParte.Cassette; }
            else if (Lugar == "CoinBag") { Destino = TipoParte.Coinbag; }
            else if (Lugar == "Escrow") { Destino = TipoParte.Escrow; }

            lock (syncLock)
            {
                if (View.BanderaRecaudo)
                {
                    if (Destino != TipoParte.Escrow)
                    {
                        View.General_Events = "Presenter RegistrarMovimiento Entrada Billetes" + " Denominacion " + Denominacion;
                        if (RegistrarMovimiento(TipoOperacion.Pago, Destino, TipoMovimiento.Entrada, null, Denominacion, 1))
                        {
                            View.General_Events = "Presenter RegistrarMovimiento ok";
                            View.PagoEfectivo.ValorRecibido += Denominacion;
                            View.General_Events = "Presenter ValorRecibido " + Denominacion;
                            long aux = View.PagoEfectivo.ValorRecibido - View.PagoEfectivo.ValorPago;

                            if ((View.PagoEfectivo.ValorRecibido - View.PagoEfectivo.ValorPago) < 0)
                            {
                                View.PagoEfectivo.ValorCambio = 0;
                                if (View.TipoPago == "MENSUALIDAD")
                                {
                                    View.SetearPantalla(Pantalla.DetallePagoMensual);
                                }
                                else 
                                {
                                    View.SetearPantalla(Pantalla.DetallePago);
                                }
                            }
                            else
                            {
                                View.PagoEfectivo.ValorCambio = Convert.ToInt32(View.PagoEfectivo.ValorRecibido - View.PagoEfectivo.ValorPago);
                                if (View.TipoPago == "MENSUALIDAD")
                                {
                                    View.SetearPantalla(Pantalla.DetallePagoMensual);
                                }
                                else
                                {
                                    View.SetearPantalla(Pantalla.DetallePago);
                                }
                            }
                        }
                    }
                    else
                    {
                        View.General_Events = "Presenter-DineroRecibido:  Escrow " + Denominacion;
                    }
                }
            }

            if (View.PagoEfectivo.ValorRecibido >= View.PagoEfectivo.ValorPago && View.BanderaRecaudo)
            {
                View.BanderaRecaudo = false;
                View.BanderaPagoFinal = true;
                if (View.TipoPago == "MENSUALIDAD")
                {
                    View.SetearPantalla(Pantalla.DetallePagoMensual);
                }
                else
                {
                    View.SetearPantalla(Pantalla.DetallePago);
                }
            }            
            else if (View.CargaBilletesBB)
            {
                if (RegistrarMovimiento(TipoOperacion.Carga, Destino, TipoMovimiento.Entrada, null, Denominacion, 1))
                {
                    ObtenerInfoPartes();
                    View.SetearPantalla(Pantalla.MenuCargBilletes);
                }
            }
            else
            {
                if (View.BanderaRecaudo)
                {
                    if (View.TipoPago == "MENSUALIDAD")
                    {
                        View.SetearPantalla(Pantalla.DetallePagoMensual);
                    }
                    else
                    {
                        View.SetearPantalla(Pantalla.DetallePago);
                    }
                }
            }
        }
        private void EventosBilletero(string t)
        {
            if (t == "POWER_RECOVERY")
            {
                //RegistrarAlarma(TipoAlarma.Billetero, m);
            }
            else if (t == "FinInicio")
            {
                View.General_Events = "Presenter-EventosBilletero FinInicio OK";
            }
            else if (t == "BoxRemovido")
            {
                View.FinalizarArqueo();
            }
            else if (t == "ArqueoBoxRemovido")
            {
                View.FinalizarArqueo();
            }
            else if (t == "FinArqueo")
            {
                View.SetearPantalla(Pantalla.ArqueoTotal);
            }
            else if (t == "UNLOADED_CASSETTES")
            {
                //View.Interfaz = Presentacion.FormRetirarBox;
            }
            else if (t == "DROP_CASSETTE_REMOVED")
            {
                if (!View.BanderaBox && (View.BanderaArqueoParcial || View.BanderaArqueoTotal))
                {
                    View.BanderaBox = true;
                    //RegistrarAlarma(TipoAlarma.Billetero, "BOX REMOVIDO");
                }
                else if (!View.BanderaBox)
                {
                    View.BanderaBox = true;
                    //RegistrarAlarma(TipoAlarma.Billetero, "BOX REMOVIDO");
                    View.BanderaEstadoBilletero = false;
                    //sMensajeServicio = "Box removido";
                    //View.Interfaz = Presentacion.FormMensaje;
                }
            }
            else if (t == "CHASIS_REMOVED")
            {
                //if (!BanderaChasis)
                //{
                //    View.BanderaChasis = true;
                //    //RegistrarAlarma(TipoAlarma.Billetero, "CHASIS REMOVIDO");
                //    View.BanderaEstadoBilletero = false;
                //    //sMensajeServicio = "Chasis removido";
                //    //View.Interfaz = Presentacion.FormMensaje;
                //}
            }
            else if (t == "DROP_CASSETTE_FULL")
            {

            }
            else if (t == "STACKING" || t == "ACCEPTING" || t == "DISPENSING" || t == "DISPENSING01")
            {
                View.oTimeOut = 0;
            }
            else if (t == "Atascamiento" && !View.BanderaJam)
            {
                ExpulsarTarjeta();
                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                View.BanderaJam = true;
                View.BanderaEstadoBilletero = false;
                //View.Interfaz = Presentacion.Transaccion_En_Proceso;
                View.oTimeOut = 0;

                if (View.BanderaRecaudo || View.BanderaCancelacion || View.BanderaPagoFinal)
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    //if (!View.ComPrint)
                    //{
                    //View.ComPrint = true;
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    //}
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCargaBilletes)
                {
                    //ConfirmarOperacion(TipoOperacion.Carga, TipoEstadoPago.Error_Dispositivo);
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaArqueoTotal)
                {
                    //ConfirmarOperacion(TipoOperacion.ArqueoTotal, TipoEstadoPago.Error_Dispositivo); // no confirmar arqueo para que puedan reintentar
                    //View.BanderaJamArqueoT = true;
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCancelacion)
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    if (!View.ComPrint)
                    {
                        View.ComPrint = true;
                        View.TicketDevolucion = true;
                        View.Imprimir();
                    }
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    //if (!View.ComPrint)
                    //{
                    //View.ComPrint = true;
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    //}
                    View.SetearPantalla(Pantalla.Atasco);
                }
            }
            else if (t == "Atascado" && !View.BanderaJam)
            {
                ExpulsarTarjeta();
                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                View.BanderaJam = true;
                View.BanderaEstadoBilletero = false;
                //View.Interfaz = Presentacion.Transaccion_En_Proceso;
                View.oTimeOut = 0;

                if (View.BanderaRecaudo || View.BanderaCancelacion || View.BanderaPagoFinal)
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    //if (!View.ComPrint)
                    //{
                    //View.ComPrint = true;
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    //}
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCargaBilletes)
                {
                    //ConfirmarOperacion(TipoOperacion.Carga, TipoEstadoPago.Error_Dispositivo);
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaArqueoTotal)
                {
                    //ConfirmarOperacion(TipoOperacion.ArqueoTotal, TipoEstadoPago.Error_Dispositivo); // no confirmar arqueo para que puedan reintentar
                    //View.BanderaJamArqueoT = true;
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCancelacion)
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    //if (!View.ComPrint)
                    //{
                    //View.ComPrint = true;
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    //}
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    View.TicketDevolucion = true;
                    View.Imprimir();

                    View.SetearPantalla(Pantalla.Atasco);
                }
            }
            else if (t == "ErrorDispensar" && !View.BanderaJam)
            {
                ExpulsarTarjeta();
                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                View.BanderaJam = true;
                View.BanderaEstadoBilletero = false;

                View.oTimeOut = 0;

                if (View.BanderaRecaudo || View.BanderaCancelacion || View.BanderaPagoFinal)
                {
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    if (!View.ComPrint)
                    {
                        View.ComPrint = true;
                        View.TicketDevolucion = true;
                        View.Imprimir();
                    }
                    View.SetearPantalla(Pantalla.Atasco);

                }
                else if (View.BanderaCargaBilletes)
                {
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaArqueoTotal)
                {
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCancelacion)
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    if (!View.ComPrint)
                    {
                        View.ComPrint = true;
                        View.TicketDevolucion = true;
                        View.Imprimir();
                    }
                    View.SetearPantalla(Pantalla.Atasco);
                }
                else
                {
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    if (!View.ComPrint)
                    {
                        View.ComPrint = true;
                        View.TicketDevolucion = true;
                        View.Imprimir();
                    }
                    View.SetearPantalla(Pantalla.Atasco);
                }
            }
            else if (t == "DISPENSING_ERROR" && !View.BanderaDispensing) // no entregó dinero
            {
                View.BanderaDispensing = true;
                View.BanderaEstadoBilletero = false;
                //RegistrarAlarma(TipoAlarma.Billetero, "ERROR AL DISPENSAR");

                if (View.BanderaRecaudo || View.BanderaCancelacion || View.BanderaPagoFinal)
                {
                    ExpulsarTarjeta();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.PagoEfectivo.ValorEntregado = 0;
                    View.PagoEfectivo.ValorProcesoCambio = 0;
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);

                    //View.ComPrint = true;
                    View.TicketDevolucion = true;
                    View.Imprimir();

                    View.SetearPantalla(Pantalla.Atasco);
                }
                else if (View.BanderaCancelacion)
                {
                    ExpulsarTarjeta();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.General_Events = "Error en el billetero ¡Billetero Atascado!";
                    RegistrarAlarma(TipoAlarma.Billetero, TipoParte.Cassette.ToString(), "Billetero Atascado", 1);
                    if (!View.ComPrint)
                    {
                        View.ComPrint = true;
                        View.TicketDevolucion = true;
                        View.Imprimir();
                    }
                    View.SetearPantalla(Pantalla.Atasco);
                }
            }
            else if (View.EstadoSecuencia == "DISABLED")
            {
                if (View.BanderaBox)
                {
                    //DI.Mensaje = "1";
                    //View.oDatosInterfaz = DI;
                    //View.BanderaBox = false;
                }
                if (!View.BanderaEstadoBilletero)
                {
                    View.BanderaBox = false;
                    View.BanderaChasis = false;
                    View.BanderaJam = false;
                    View.BanderaEstadoBilletero = true;
                }
            }
            else
            {
                //View.General_Events = "Evento temporal billetero " + t;
                if (View.Presentacion == Pantalla.DetallePago || View.Presentacion == Pantalla.DetallePagoMensual)
                {
                    View.DatosPantallaPago();
                }
            }

            View.EstadoSecuencia = t;
        }
        private void EventosDispensar(string estado, int den1, int cant1, int den2, int cant2, int den3, int cant3) //ok
        {
            View.EstadoSecuenciaDispensar = estado;

            if (estado == "Dispensado")
            {
                View.Cnt_Reinicio = 0;

                if (cant1 > 0)
                    RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Cassette, TipoMovimiento.Salida, null, den1, cant1);
                if (cant2 > 0)
                    RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Cassette, TipoMovimiento.Salida, null, den2, cant2);
                if (cant3 > 0)
                    RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Cassette, TipoMovimiento.Salida, null, den3, cant3);
            }
            else if (estado == "Resto")
            {
                if (cant1 > 0)
                {
                    View.PagoEfectivo.ValorEntregado = den1;
                    View.PagoEfectivo.ValorPagoMonedas = cant1;  //Valor a pagar con monedas
                    View.PagoEfectivo.ValorProcesoCambio = cant1;
                    if (View.PagoEfectivo.ValorProcesoCambio < DenomMenor)
                    {
                        if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                        {
                            if (View.BanderaCancelacion)
                            {
                                if (View.TipoPago == "MENSUALIDAD")
                                {
                                    ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }
                                else if (View.TipoPago == "EVENTO")
                                {
                                    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Cancelado);
                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }
                                else
                                {
                                    if (View.TipoPago == "MENSUALIDAD")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else if (View.TipoPago == "EVENTO")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else
                                    {
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                }

                            }
                            else if (!View.BanderaRecaudo)
                            {
                                if (View.BanderaPagoFinal)
                                {
                                    if (View.TipoPago == "MENSUALIDAD")
                                    {
                                        EscribirTarjeta();
                                        View.Efectivo = true;
                                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                                        if (View.CobroTarjetaMensual)
                                        {
                                            ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                                        }
                                        View.Cnt_Reinicio = 0;
                                        View.SetearPantalla(Pantalla.ImprimirFactura);
                                        View.BanderaRecaudo = false;
                                        View.BanderaPagoFinal = false;
                                    }
                                    else if (View.TipoPago == "EVENTO")
                                    {
                                        EscribirTarjeta();
                                        View.Efectivo = true;
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                        ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                        View.Cnt_Reinicio = 0;
                                        View.SetearPantalla(Pantalla.ImprimirFactura);
                                        View.BanderaRecaudo = false;
                                        View.BanderaPagoFinal = false;
                                    }
                                    else
                                    {
                                        if (View.TipoPago == "MENSUALIDAD")
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                                            View.Cnt_Reinicio = 0;
                                            if (View.CobroTarjetaMensual)
                                            {
                                                ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                                            }
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                        //else if (View.TipoPago == "EVENTO")
                                        //{
                                        //    EscribirTarjeta();
                                        //    View.Efectivo = true;
                                        //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                        //    View.Cnt_Reinicio = 0;
                                        //    View.SetearPantalla(Pantalla.ImprimirFactura);
                                        //    View.BanderaRecaudo = false;
                                        //    View.BanderaPagoFinal = false;
                                        //}
                                        else
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                            if (View.TipoPago == "REPOSICIÓN")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "CASCOS")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "EVENTO")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                            }
                                            View.Cnt_Reinicio = 0;
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (View.BanderaCancelacion)
                            {
                                if (View.TipoPago == "MENSUALIDAD")
                                {
                                    View.TicketDevolucion = true;
                                    View.Imprimir();
                                    ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Error_Dispositivo);

                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }
                                else if (View.TipoPago == "EVENTO")
                                {
                                    View.TicketDevolucion = true;
                                    View.Imprimir();
                                    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Error_Dispositivo);

                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }
                                else
                                {
                                    View.TicketDevolucion = true;
                                    View.Imprimir();
                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }

                            }
                            else if (!View.BanderaRecaudo)
                            {
                                if (View.BanderaPagoFinal)
                                {
                                    if (View.TipoPago == "MENSUALIDAD")
                                    {
                                        EscribirTarjeta();
                                        View.TicketDevolucion = true;
                                        View.Imprimir();
                                        View.Efectivo = true;
                                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Error_Dispositivo);
                                        View.SetearPantalla(Pantalla.ImprimirFactura);
                                        View.BanderaRecaudo = false;
                                        View.BanderaPagoFinal = false;
                                    }
                                    else if (View.TipoPago == "EVENTO")
                                    {
                                        EscribirTarjeta();
                                        View.TicketDevolucion = true;
                                        View.Imprimir();
                                        View.Efectivo = true;
                                        ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Error_Dispositivo);
                                        View.SetearPantalla(Pantalla.ImprimirFactura);
                                        View.BanderaRecaudo = false;
                                        View.BanderaPagoFinal = false;
                                    }
                                    else
                                    {
                                        EscribirTarjeta();
                                        View.TicketDevolucion = true;
                                        View.Imprimir();
                                        View.Efectivo = true;
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                                        View.SetearPantalla(Pantalla.ImprimirFactura);
                                        View.BanderaRecaudo = false;
                                        View.BanderaPagoFinal = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (View.TipoPago == "MENSUALIDAD")
                        {
                            oReader.ExpulsarTarjeta();
                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Error_Dispositivo);
                            View.TicketDevolucion = true;
                            View.Imprimir();
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                        else if (View.TipoPago == "EVENTO")
                        {
                            oReader.ExpulsarTarjeta();
                            ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Error_Dispositivo);
                            View.TicketDevolucion = true;
                            View.Imprimir();
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                        else
                        {
                            oReader.ExpulsarTarjeta();
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                            View.TicketDevolucion = true;
                            View.Imprimir();
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                    }
                }
                else
                {
                    if (!View.BanderaCancelacion)
                    {
                        if (View.TipoPago == "MENSUALIDAD")
                        {
                            EscribirTarjeta();
                            View.Efectivo = true;
                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                            if (View.CobroTarjetaMensual)
                            {
                                ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                            }
                            View.BanderaRecaudo = false;
                            View.Cnt_Reinicio = 0;
                            View.SetearPantalla(Pantalla.ImprimirFactura);
                        }
                        //else if (View.TipoPago == "EVENTO")
                        //{
                        //    EscribirTarjeta();
                        //    View.Efectivo = true;
                        //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                           
                        //    View.BanderaRecaudo = false;
                        //    View.Cnt_Reinicio = 0;
                        //    View.SetearPantalla(Pantalla.ImprimirFactura);
                        //}
                        else
                        {
                            EscribirTarjeta();
                            View.Efectivo = true;
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                            if (View.TipoPago == "REPOSICIÓN")
                            {
                                ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                            }
                            else if (View.TipoPago == "CASCOS")
                            {
                                ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                            }
                            else if (View.TipoPago == "EVENTO")
                            {
                                ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                            }
                            View.BanderaRecaudo = false;
                            View.Cnt_Reinicio = 0;
                            View.SetearPantalla(Pantalla.ImprimirFactura);
                        }
                    }
                    else
                    {
                        if (View.TipoPago == "MENSUALIDAD")
                        {
                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                            if (ValidarSaldosMinimos())
                            {
                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
                            }
                        }
                        else if (View.TipoPago == "EVENTO")
                        {
                            ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Cancelado);
                            if (ValidarSaldosMinimos())
                            {
                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
                            }
                        }
                        else
                        {
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            if (ValidarSaldosMinimos())
                            {
                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
                            }
                        }
                    }
                }
            }
        }
        public bool AdministrarDenominaciones(TipoAdministracionDenominacion oTipoAdministracionDenominacion, TipoDenominacion oDenominacion)
        {
            bool Resultado = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oTipoAdministracionDenominacion == TipoAdministracionDenominacion.Habilitar)
            {
                oResultadoOperacion = Model.AdministrarDenominaciones(oPaymentDevice, TipoAdministracionDenominacion.Habilitar, oDenominacion);

                if (oDenominacion == TipoDenominacion.ALL)
                {
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        Resultado = true;
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        //DatosLog.IdCajero = oEoModulo.ID_EoModulo;
                        //DatosLog.CodigoCajero = oEoModulo.CodigoPS;
                        //DatosLog.IdOperacion = "N/A";
                        //DatosLog.TipoOperacion = "Inicialización";
                        //DatosLog.Parte = TipoParteLog.Hopper.ToString();
                        //DatosLog.Descripcion = "Error al habilitar denominaciones";
                        //DatosLog.Solucion = "1. Restablecer aplicación, 2. Validar conexiones, 3. Revisar sensores,  4. Reconectar Hoppers";
                        //Task.Run(() =>
                        //{
                        //    Model.InsertaErrorLogCajero(DatosLog);
                        //    RegistrarAlarma(TipoAlarma.Hopper, "Error al habilitar denominaciones");
                        //});
                        Resultado = false;
                    }
                }
                else
                {
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        Resultado = true;
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        //DatosLog.IdCajero = oEoModulo.ID_EoModulo;
                        //DatosLog.CodigoCajero = oEoModulo.CodigoPS;
                        //DatosLog.IdOperacion = "N/A";
                        //DatosLog.TipoOperacion = "Inicialización";
                        //DatosLog.Parte = TipoParteLog.Hopper.ToString();
                        //DatosLog.Descripcion = "Error al habilitar denominaciones";
                        //DatosLog.Solucion = "1. Restablecer aplicación, 2. Validar conexiones, 3. Revisar sensores,  4. Reconectar Hoppers";
                        //Task.Run(() =>
                        //{
                        //    Model.InsertaErrorLogCajero(DatosLog);
                        //    RegistrarAlarma(TipoAlarma.Hopper, "Error al habilitar denominaciones");
                        //});
                        Resultado = false;
                    }
                }
            }
            else if (oTipoAdministracionDenominacion == TipoAdministracionDenominacion.Deshabilitar)
            {
                oResultadoOperacion = Model.AdministrarDenominaciones(oPaymentDevice, TipoAdministracionDenominacion.Deshabilitar, oDenominacion);

                if (oDenominacion == TipoDenominacion.ALL)
                {
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        Resultado = true;
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        //DatosLog.IdCajero = oEoModulo.ID_EoModulo;
                        //DatosLog.CodigoCajero = oEoModulo.CodigoPS;
                        //DatosLog.IdOperacion = "N/A";
                        //DatosLog.TipoOperacion = "Inicialización";
                        //DatosLog.Parte = TipoParteLog.Hopper.ToString();
                        //DatosLog.Descripcion = "Error al deshabilitar denominaciones";
                        //DatosLog.Solucion = "1. Restablecer aplicación, 2. Validar conexiones, 3. Revisar sensores,  4. Reconectar Hoppers";
                        //Task.Run(() =>
                        //{
                        //    Model.InsertaErrorLogCajero(DatosLog);
                        //    RegistrarAlarma(TipoAlarma.Hopper, "Error al deshabilitar denominaciones");
                        //});
                        Resultado = false;
                    }
                }
                else
                {
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        Resultado = true;
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        //DatosLog.IdCajero = oEoModulo.ID_EoModulo;
                        //DatosLog.CodigoCajero = oEoModulo.CodigoPS;
                        //DatosLog.IdOperacion = "N/A";
                        //DatosLog.TipoOperacion = "Inicialización";
                        //DatosLog.Parte = TipoParteLog.Hopper.ToString();
                        //DatosLog.Descripcion = "Error al deshabilitar denominaciones";
                        //DatosLog.Solucion = "1. Restablecer aplicación, 2. Validar conexiones, 3. Revisar sensores,  4. Reconectar Hoppers";
                        //Task.Run(() =>
                        //{
                        //    Model.InsertaErrorLogCajero(DatosLog);
                        //    RegistrarAlarma(TipoAlarma.Hopper, "Error al deshabilitar denominaciones");
                        //});
                        Resultado = false;
                    }
                }
            }

            return Resultado;
        }
        public void HabilitarSecuenciaRecibir()
        {
            Model.HabilitarSecuenciaRecibir();
        }
        public void HabilitarSecuenciaMonitor()
        {
            Model.HabilitarSecuenciaMonitor();
        }
        public void HabilitarSecuenciaArqueoTotal()
        {
            Model.HabilitarSecuenciaArqueoTotal(View.DtoModulo);
        }
        public void DeshabilitarRecepcion()
        {
            Model.DeshabilitarRecepcion();
        }
        public void EfectuarPago(long VALOR)
        {

            View.PagoEfectivo.ValorProcesoCambio = Convert.ToInt64(VALOR);
            View.PagoEfectivo.ValorCambio = Convert.ToInt32(View.PagoEfectivo.ValorProcesoCambio);

            if (View.PagoEfectivo.ValorCambio > 0)
            {
                if (View.PagoEfectivo.ValorProcesoCambio >= 2000)
                {
                    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                    {
                        Model.DispensarBilletes(Convert.ToInt32(View.PagoEfectivo.ValorCambio), View.DtoModulo);
                    }
                }
                else
                {
                    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                    {
                        View.General_Events = "Presenter-AdministrarProcesoPago Monedas -> Prepara Proceso Cambio Monedas";
                        if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                        {
                            View.General_Events = "Presenter-Billetero -> Ingresa Proceso Cambio Monedas";
                            if (View.PagoEfectivo.ValorProcesoCambio <= 0)
                            {
                                //escribir TI
                                oReader.ExpulsarTarjeta();
                                View.Efectivo = true;
                                View.BanderaRecaudo = false;
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                View.SetearPantalla(Pantalla.ImprimirFactura);

                            }
                            else
                            {
                                oReader.ExpulsarTarjeta();
                                View.General_Events = "Presenter-ConfirmarOperacion -> Error_Dispositivo";
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                                View.TicketDevolucion = true;
                                View.Imprimir();
                                View.SetearPantalla(Pantalla.SistemaSuspendido);
                            }
                        }
                        else
                        {
                            oReader.ExpulsarTarjeta();
                            View.General_Events = "Presenter-ConfirmarOperacion -> Error_Dispositivo";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                            View.TicketDevolucion = true;
                            View.Imprimir();
                            View.BanderaRecaudo = false;
                            View.SetearPantalla(Pantalla.SistemaSuspendido);

                        }
                    }
                }
            }
            else
            {
                oReader.ExpulsarTarjeta();
                View.Efectivo = true;
                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                View.BanderaRecaudo = false;
                View.SetearPantalla(Pantalla.ImprimirFactura);
            }
        }
        //public void EfectuarPagoCancelado(long VALOR)
        //{
        //    View.General_Events = "Presenter-AdministrarProcesoCambio Preparando para Efectuar Pago";

        //    View.PagoEfectivo.ValorProcesoCambio = Convert.ToInt64(VALOR);
        //    View.PagoEfectivo.ValorCambio = Convert.ToInt32(View.PagoEfectivo.ValorProcesoCambio);

        //    View.Operacion.ID_Transaccion = Convert.ToInt64(View.IdTransaccion);
        //    View.Operacion.Comision = View.PagoEfectivo.ValorCambio;
        //    View.Operacion.ID_Modulo = Globales.sSerial;
        //    //View.SetearPantalla(Pantalla.Procesando);

        //    if (View.PagoEfectivo.ValorCambio > 0)
        //    {
        //        if (View.PagoEfectivo.ValorProcesoCambio >= 2000)
        //        {
        //            if (View.PagoEfectivo.ValorProcesoCambio > 0)
        //            {
        //                //Model.DispensarBilletes(Convert.ToInt32(View.PagoEfectivo.ValorCambio), View.DtoModulo);
        //                AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Billete);
        //            }
        //        }
        //        else
        //        {
        //            if (View.PagoEfectivo.ValorProcesoCambio > 0)
        //            {
        //                View.General_Events = "Presenter-AdministrarProcesoCambio Monedas -> Prepara Proceso Cambio Monedas";
        //                if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
        //                {
        //                    if (View.BanderaCancelacion)
        //                    {
        //                        if (View.TipoPago == "MENSUALIDAD")
        //                        {
        //                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
        //                            if (ValidarSaldosMinimos())
        //                            {
        //                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                            }
        //                        }
        //                        else if (View.TipoPago == "EVENTO")
        //                        {
        //                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
        //                            if (ValidarSaldosMinimos())
        //                            {
        //                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
        //                            if (ValidarSaldosMinimos())
        //                            {
        //                                View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                            }
        //                        }

        //                    }
        //                    else if (!View.BanderaRecaudo)
        //                    {
        //                        if (View.BanderaPagoFinal)
        //                        {
        //                            if (View.TipoPago == "MENSUALIDAD")
        //                            {
        //                                EscribirTarjeta();
        //                                View.Efectivo = true;
        //                                ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
        //                                if (View.CobroTarjetaMensual)
        //                                {
        //                                    ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
        //                                }
        //                                View.Cnt_Reinicio = 0;
        //                                View.SetearPantalla(Pantalla.ImprimirFactura);
        //                                View.BanderaRecaudo = false;
        //                                View.BanderaPagoFinal = false;
        //                            }
        //                            //else if (View.TipoPago == "EVENTO")
        //                            //{
        //                            //    EscribirTarjeta();
        //                            //    View.Efectivo = true;
        //                            //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
        //                            //    View.Cnt_Reinicio = 0;
        //                            //    View.SetearPantalla(Pantalla.ImprimirFactura);
        //                            //    View.BanderaRecaudo = false;
        //                            //    View.BanderaPagoFinal = false;
        //                            //}
        //                            else
        //                            {
        //                                EscribirTarjeta();
        //                                View.Efectivo = true;
        //                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
        //                                if (View.TipoPago == "REPOSICIÓN")
        //                                {
        //                                    ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
        //                                }
        //                                else if (View.TipoPago == "CASCOS")
        //                                {
        //                                    ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
        //                                }
        //                                else if (View.TipoPago == "EVENTO")
        //                                {
        //                                    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
        //                                }
        //                                View.Cnt_Reinicio = 0;
        //                                View.SetearPantalla(Pantalla.ImprimirFactura);
        //                                View.BanderaRecaudo = false;
        //                                View.BanderaPagoFinal = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (View.BanderaCancelacion)
        //                    {

        //                        View.TicketDevolucion = true;
        //                        View.Imprimir();
        //                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

        //                        if (ValidarSaldosMinimos())
        //                        {
        //                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                        }

        //                    }
        //                    else if (!View.BanderaRecaudo)
        //                    {
        //                        if (View.BanderaPagoFinal)
        //                        {
        //                            EscribirTarjeta();
        //                            View.TicketDevolucion = true;
        //                            View.Imprimir();
        //                            View.Efectivo = true;
        //                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
        //                            View.SetearPantalla(Pantalla.ImprimirFactura);
        //                            View.BanderaRecaudo = false;
        //                            View.BanderaPagoFinal = false;
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (View.BanderaCancelacion)
        //        {
        //            if (View.TipoPago == "MENSUALIDAD")
        //            {
        //                ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
        //                if (ValidarSaldosMinimos())
        //                {
        //                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                }
        //            }
        //            else if (View.TipoPago == "EVENTO")
        //            {
        //                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
        //                if (ValidarSaldosMinimos())
        //                {
        //                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                }
        //            }
        //            else
        //            {
        //                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
        //                if (ValidarSaldosMinimos())
        //                {
        //                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
        //                }
        //            }
        //        }
        //        else if (View.BanderaPagoFinal)
        //        {
        //            if (View.TipoPago == "MENSUALIDAD")
        //            {
        //                EscribirTarjeta();
        //                View.Efectivo = true;
        //                ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
        //                View.Cnt_Reinicio = 0;
        //                if (View.CobroTarjetaMensual)
        //                {
        //                    ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
        //                }
        //                View.SetearPantalla(Pantalla.ImprimirFactura);
        //                View.BanderaRecaudo = false;
        //                View.BanderaPagoFinal = false;
        //            }
        //            //if (View.TipoPago == "EVENTO")
        //            //{
        //            //    EscribirTarjeta();
        //            //    View.Efectivo = true;
        //            //    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
        //            //    View.Cnt_Reinicio = 0;
        //            //    View.SetearPantalla(Pantalla.ImprimirFactura);
        //            //    View.BanderaRecaudo = false;
        //            //    View.BanderaPagoFinal = false;
        //            //}
        //            else
        //            {
        //                EscribirTarjeta();
        //                View.Efectivo = true;
        //                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
        //                if (View.TipoPago == "REPOSICIÓN")
        //                {
        //                    ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
        //                }
        //                else if (View.TipoPago == "CASCOS")
        //                {
        //                    ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
        //                }
        //                else if (View.TipoPago == "EVENTO")
        //                {
        //                    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
        //                }
        //                View.Cnt_Reinicio = 0;
        //                View.SetearPantalla(Pantalla.ImprimirFactura);
        //                View.BanderaRecaudo = false;
        //                View.BanderaPagoFinal = false;
        //            }
        //        }
        //    }
        //}

        public void EfectuarPagoCancelado(long VALOR)
        {
            RESULT = 0;
            
            View.PagoEfectivo.ValorProcesoCambio = Convert.ToInt64(VALOR);
            VALORCAMBIO = View.PagoEfectivo.ValorProcesoCambio;

            View.PagoEfectivo.ValorCambio = Convert.ToInt32(View.PagoEfectivo.ValorProcesoCambio);


            if (View.PagoEfectivo.ValorCambio > 0)
            {
                if (View.PagoEfectivo.ValorProcesoCambio >= 2000)
                {
                    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                    {
                        AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Billete);
                    }
                }
                else
                {
                    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                    {
                        View.General_Events = "Presenter-AdministrarProcesoPago Monedas -> Prepara Proceso Cambio Monedas";
                        if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                        {
                            View.General_Events = "Presenter-Billetero -> Ingresa Proceso Cambio Monedas";
                            if (View.PagoEfectivo.ValorProcesoCambio <= 0)
                            {

                                //ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                ////View.Imprimir();
                                ////if (ValidarSaldosMinimos())
                                ////{
                                ////    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                ////}
                                //View.SetearPantalla(Pantalla.ImprimirFactura);
                                ////else
                                ////{
                                ////    View.SetearPantalla(Pantalla.SistemaSuspendido);
                                ////}

                                if (View.BanderaCancelacion)
                                {
                                    if (View.TipoPago == "MENSUALIDAD")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else if (View.TipoPago == "EVENTO")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else
                                    {
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }

                                }
                                else if (!View.BanderaRecaudo)
                                {
                                    if (View.BanderaPagoFinal)
                                    {
                                        if (View.TipoPago == "MENSUALIDAD")
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                                            if (View.CobroTarjetaMensual)
                                            {
                                                ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                                            }
                                            View.Cnt_Reinicio = 0;
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                        //else if (View.TipoPago == "EVENTO")
                                        //{
                                        //    EscribirTarjeta();
                                        //    View.Efectivo = true;
                                        //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                        //    View.Cnt_Reinicio = 0;
                                        //    View.SetearPantalla(Pantalla.ImprimirFactura);
                                        //    View.BanderaRecaudo = false;
                                        //    View.BanderaPagoFinal = false;
                                        //}
                                        else
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                            if (View.TipoPago == "REPOSICIÓN")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "CASCOS")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "EVENTO")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                            }
                                            View.Cnt_Reinicio = 0;
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                    }
                                }

                            }
                            else
                            {

                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

                                View.Imprimir();

                                View.TicketDevolucion = true;
                                View.Imprimir();

                            }
                        }
                        else
                        {

                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

                            View.Imprimir();

                            View.TicketDevolucion = true;
                            View.Imprimir();

                        }
                    }
                }
            }
            else
            {
                //ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                ////View.Imprimir();
                //View.SetearPantalla(Pantalla.ImprimirFactura);
                if (View.BanderaCancelacion)
                {
                    if (View.TipoPago == "MENSUALIDAD")
                    {
                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                        if (ValidarSaldosMinimos())
                        {
                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                        }
                    }
                    else if (View.TipoPago == "EVENTO")
                    {
                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                        if (ValidarSaldosMinimos())
                        {
                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                        }
                    }
                    else
                    {
                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                        if (ValidarSaldosMinimos())
                        {
                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                        }
                    }

                }
                else if (!View.BanderaRecaudo)
                {
                    if (View.BanderaPagoFinal)
                    {
                        if (View.TipoPago == "MENSUALIDAD")
                        {
                            EscribirTarjeta();
                            View.Efectivo = true;
                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                            if (View.CobroTarjetaMensual)
                            {
                                ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                            }
                            View.Cnt_Reinicio = 0;
                            View.SetearPantalla(Pantalla.ImprimirFactura);
                            View.BanderaRecaudo = false;
                            View.BanderaPagoFinal = false;
                        }
                        //else if (View.TipoPago == "EVENTO")
                        //{
                        //    EscribirTarjeta();
                        //    View.Efectivo = true;
                        //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                        //    View.Cnt_Reinicio = 0;
                        //    View.SetearPantalla(Pantalla.ImprimirFactura);
                        //    View.BanderaRecaudo = false;
                        //    View.BanderaPagoFinal = false;
                        //}
                        else
                        {
                            EscribirTarjeta();
                            View.Efectivo = true;
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                            if (View.TipoPago == "REPOSICIÓN")
                            {
                                ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                            }
                            else if (View.TipoPago == "CASCOS")
                            {
                                ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                            }
                            else if (View.TipoPago == "EVENTO")
                            {
                                ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                            }
                            View.Cnt_Reinicio = 0;
                            View.SetearPantalla(Pantalla.ImprimirFactura);
                            View.BanderaRecaudo = false;
                            View.BanderaPagoFinal = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region BillF56
        public bool ConectarF56()
        {
            //return true;
            bool ok = false;
            Modulo oModulo = new Modulo();
            int Tipo = 0;
            oModulo.ID_Modulo = Globales.sSerial;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.ObtenerPartesF56Modulo(oModulo, Tipo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                View.DtoModuloF56.PartesF56 = (List<DtoParteModuloF56>)oResultadoOperacion.ListaEntidadDatos;
                View.General_Events = "Presenter-ObtenerInfoPartesF56: " + oResultadoOperacion.Mensaje;

                List<F56Bill> listBills = new List<F56Bill>();

                for (int i = 0; i < View.DtoModuloF56.PartesF56.Count; i++)
                {

                    listBills.Add(new F56Bill(110, 175, 1.3));

                }

                if (_F56Device.Conectar(listBills, F56ShutterAction.OnlyFront, true))
                {
                    ok = true;
                }
                else
                {
                    ok = false;
                }

            }

            return ok;
        }
        public int CantBilltes(long ValorRetiro)
        {
            Modulo oModulo = new Modulo();
            oModulo.ID_Modulo = Globales.sSerial;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            List<List<F56PayParameter>> olist = new List<List<F56PayParameter>>();
            oResultadoOperacion = Model.ObtenerPartesModulo(oModulo);
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                List<DtoParteModulo> partesPagar = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

                List<F56PayParameter> pagarLista = new List<F56PayParameter>();
                foreach (DtoParteModulo item in partesPagar)
                {
                    if (item.TipoParte == "F56")
                    {
                        pagarLista.Add(new F56PayParameter(Convert.ToInt32(item.NumParte), Convert.ToInt32(item.Denominacion), Convert.ToInt32(item.CantidadActual)));
                    }
                }
                List<F56PayParameter> SortedList = pagarLista.OrderBy(o => o.NumeroCassette).ToList();


                if (Globales.sDispenseOption == "Front" || Globales.sDispenseOption == string.Empty)
                {
                    olist = _F56Device.CantidadPagar(SortedList, ValorRetiro, F56Style.Front);
                }
                else
                {
                    olist = _F56Device.CantidadPagar(SortedList, ValorRetiro, F56Style.Rear);
                }
            }



            int Cantem = 0;
            int final = 0;

            for (int a = 0; a < olist.Count; a++)
            {
                for (int i = 0; i < olist[a].Count; i++)
                {
                    if (olist[a][i].NumeroCassette == 1)
                    {
                        Cantem = 2000 * olist[a][i].Cantidad;

                        final += Cantem;
                    }
                    else if (olist[a][i].NumeroCassette == 2)
                    {
                        Cantem = 5000 * olist[a][i].Cantidad;

                        final += Cantem;
                    }
                    else if (olist[a][i].NumeroCassette == 3)
                    {
                        Cantem = 10000 * olist[a][i].Cantidad;

                        final += Cantem;
                    }
                    //else if (olist[a][i].NumeroCassette == 4)
                    //{
                    //    Cantem = 50000 * olist[a][i].Cantidad;

                    //    final += Cantem;
                    //}
                }
            }
            return final;

        }
        public void EfectuarPagoDevolucionCambio()
        {
            RESULT = 0;
            View.ProcesoPago = true;
            if (View.PagoEfectivo.ValorCambio > 0)
            {
                //double Total = Convert.ToDouble(View.PagoEfectivo.ValorCambio);
                //View.Operacion.Total = Total;
                //View.Operacion.Comision = Total * 0.08;
                //View.Operacion.Iva = View.Operacion.Comision * 0.19;
                //double temp = View.Operacion.Iva + View.Operacion.Comision;
                //temp = Math.Round(temp, 2);
                //double temp2 = Total - temp;
                //temp2 = Math.Round(temp2, 0);
                //double temp3 = temp2;
                //temp3 %= 100;
                //if (temp3 != 0)
                //{
                //    temp3 = 100 - temp3;
                //    temp3 = Math.Round(temp3, 2);
                //    View.Operacion.Redondeo = temp3;
                //    View.Operacion.TotalPagado = temp2 + temp3;
                //}
                //else if (temp2 != 0)
                //{
                //    temp3 = Math.Round(temp3, 2);
                //    View.Operacion.Redondeo = temp3;
                //    View.Operacion.TotalPagado = temp2 + temp3;
                //}
                //else
                //{
                //    View.Operacion.Redondeo = 0;
                //    View.Operacion.TotalPagado = 0;
                //}

                //View.General_Events = "Presenter-AdministrarProcesoPago -> Total Cliente = " + View.Operacion.Total;
                //View.General_Events = "Presenter-AdministrarProcesoPago -> Total Redondeo = " + View.Operacion.Redondeo;
                //View.General_Events = "Presenter-AdministrarProcesoPago -> Total IVA = " + View.Operacion.Iva;
                //View.General_Events = "Presenter-AdministrarProcesoPago -> Total Comision = " + View.Operacion.Comision;
                //View.General_Events = "Presenter-AdministrarProcesoPago -> Total A Pagar = " + View.Operacion.TotalPagado;
                View.Operacion.ID_Transaccion = Convert.ToInt64(View.IdTransaccion);
                //View.PagoEfectivo.ValorProcesoCambio = Convert.ToInt64(View.Operacion.TotalPagado);
                //VALORCAMBIO = View.PagoEfectivo.ValorProcesoCambio;



                
                //if (View.PagoEfectivo.ValorProcesoCambio >= 2000)
                //{
                //    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                //    {
                //        //View.PagoEfectivo.ValorCambio = View.PagoEfectivo.ValorProcesoCambio;
                //        AdministrarProcesoPago(View.PagoEfectivo,TipoMedioPago.Billete);
                //    }
                //}
                //else
                //{
                //    if (View.PagoEfectivo.ValorProcesoCambio > 0)
                //    {
                //        View.General_Events = "Presenter-AdministrarProcesoPago Monedas -> Prepara Proceso Cambio Monedas";
                //        if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                //        {
                //            View.General_Events = "Presenter-Billetero -> Ingresa Proceso Cambio Monedas";
                //            if (View.PagoEfectivo.ValorProcesoCambio <= 0)
                //            {

                //                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                //                View.Imprimir();
                //                if (!_ErrorCleaning)
                //                {
                //                    if (ValidarSaldosMinimos())
                //                    {
                //                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                //                    }
                //                }
                //                else
                //                {
                //                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                //                }
                //            }
                //            else
                //            {
                //                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                //                View.Imprimir();
                //                View.TicketDevolucion = true;
                //                View.Imprimir();
                //                View.SetearPantalla(Pantalla.SistemaSuspendido);
                //            }
                //        }
                //        else
                //        {
                //            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                //            View.Imprimir();
                //            View.TicketDevolucion = true;
                //            View.Imprimir();
                //            View.SetearPantalla(Pantalla.SistemaSuspendido);
                //        }
                //    }
                //}
            }
            else
            {
                
                    if (ValidarSaldosMinimos())
                    {
                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                    }
                
            }
        }
        public bool AdministrarProcesoPago(PagoEfectivo oPagoEfectivo, TipoMedioPago oTipoDenominacion)
        {
            bool ok = false;
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oTipoDenominacion == TipoMedioPago.Billete)
            {
                //View.PagoEfectivo.ValorCambio = (Int32)Math.Ceiling(View.PagoEfectivo.ValorCambio / 50d) * 50;
                //Model.DispensarBilletes(Convert.ToInt32(View.PagoEfectivo.ValorCambio), View.DtoModulo);
                //ok = true;
                RESULT = 0;
                Modulo oModulo = new Modulo();
                oModulo.ID_Modulo = Globales.sSerial;

                //oResultadoOperacion = Model.ObtenerPartesF56Modulo(oModulo,Tipo);
                oResultadoOperacion = Model.ObtenerPartesModulo(oModulo);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    //View.Imprimir();
                    List<DtoParteModulo> partesPagar = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

                    List<F56PayParameter> pagarLista = new List<F56PayParameter>();
                    foreach (DtoParteModulo item in partesPagar)
                    {
                        if (item.TipoParte == "F56")
                        {
                            pagarLista.Add(new F56PayParameter(Convert.ToInt32(item.NumParte), Convert.ToInt32(item.Denominacion), Convert.ToInt32(item.CantidadActual)));
                        }
                    }
                    List<F56PayParameter> SortedList = pagarLista.OrderBy(o => o.NumeroCassette).ToList();



                    _F56Device.Pagar(SortedList, Convert.ToInt32(oPagoEfectivo.ValorProcesoCambio), F56Style.Front);

                }
                else
                {

                }
            }
            else if (oTipoDenominacion == TipoMedioPago.Moneda)
            {

                View.General_Events = "Presenter-AdministrarProcesoPago -> ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
                View.General_EventsBill = "Presenter-AdministrarProcesoPago -> ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;

                #region NRI
                oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    if (oResultadoOperacion.ListaEntidadDatos != null)
                    {
                        ok = true;

                        List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

                        for (int i = 0; i < Transacciones.Count; i++)
                        {
                            TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

                            if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
                            {
                                RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
                            }
                        }

                        ObtenerInfoPartes();
                    }
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    Model.StartCashPayment(oPaymentDevice);
                    Thread.Sleep(30000);

                    View.General_Events = "REINTENTO 1 -> Presenter AdministrarProcesoPago: Monedas // ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorEntregado: " + oPagoEfectivo.ValorEntregado + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
                    oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        if (oResultadoOperacion.ListaEntidadDatos != null)
                        {
                            ok = true;

                            List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

                            for (int i = 0; i < Transacciones.Count; i++)
                            {
                                TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

                                if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
                                {
                                    RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
                                }
                            }

                            ObtenerInfoPartes();
                        }
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        View.General_Events = "REINTENTO 2 -> Presenter AdministrarProcesoPago: Monedas // ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorEntregado: " + oPagoEfectivo.ValorEntregado + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
                        oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            if (oResultadoOperacion.ListaEntidadDatos != null)
                            {
                                ok = true;

                                List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

                                for (int i = 0; i < Transacciones.Count; i++)
                                {
                                    TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

                                    if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
                                    {
                                        RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
                                    }
                                }

                                ObtenerInfoPartes();
                            }
                        }
                        else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                        {
                            ok = false;
                            View.General_Events = "Error Presenter-AdministrarProcesoPago: " + oResultadoOperacion.Mensaje;
                        }
                    }
                }
                #endregion

                #region SH
                //Thread.Sleep(1000);
                //if (View.DisensSH(View.PagoEfectivo.ValorProcesoCambio.ToString()))
                //{
                //    ok = true;
                //    View.PagoEfectivo.ValorProcesoCambio = 0;
                //    List<TransaccionEfectivo> Transacciones = new List<TransaccionEfectivo>();
                //    Transacciones = View.Transacciones;

                //    for (int i = 0; i < Transacciones.Count; i++)
                //    {
                //        TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

                //        if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
                //        {
                //            //RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
                //        }
                //    }

                //    ObtenerInfoPartes();
                //}

                #endregion

                #region AZK

                //oResultadoOperacion = GestionarPagoEfectivoMonedas(oDispositivoPago, oPagoEfectivo);


                //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                //{
                //    if (oResultadoOperacion.ListaEntidadDatos != null)
                //    {
                //        ok = true;

                //        List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

                //        for (int i = 0; i < Transacciones.Count; i++)
                //        {
                //            TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

                //            if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
                //            {
                //                RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
                //            }
                //        }

                //        ObtenerInfoPartes();
                //    }
                //}

                #endregion
            }
            return ok;
        }
        //public bool AdministrarProcesoPago(PagoEfectivo oPagoEfectivo, TipoMedioPago oTipoDenominacion)
        //{
        //    bool ok = false;
        //    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

        //    if (oTipoDenominacion == TipoMedioPago.Billete)
        //    {
        //        RESULT = 0;
        //        Modulo oModulo = new Modulo();
        //        oModulo.ID_Modulo = Globales.sSerial;

        //        //oResultadoOperacion = Model.ObtenerPartesF56Modulo(oModulo,Tipo);
        //        oResultadoOperacion = Model.ObtenerPartesModulo(oModulo);
        //        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
        //        {
        //            //View.Imprimir();
        //            List<DtoParteModulo> partesPagar = (List<DtoParteModulo>)oResultadoOperacion.ListaEntidadDatos;

        //            List<F56PayParameter> pagarLista = new List<F56PayParameter>();
        //            foreach (DtoParteModulo item in partesPagar)
        //            {
        //                if (item.TipoParte == "F56")
        //                {
        //                    pagarLista.Add(new F56PayParameter(Convert.ToInt32(item.NumParte), Convert.ToInt32(item.Denominacion), Convert.ToInt32(item.CantidadActual)));
        //                }
        //            }
        //            List<F56PayParameter> SortedList = pagarLista.OrderBy(o => o.NumeroCassette).ToList();


        //            if (Globales.sDispenseOption == "Front" || Globales.sDispenseOption == string.Empty)
        //            {
        //                _F56Device.Pagar(SortedList, Convert.ToInt32(oPagoEfectivo.ValorProcesoCambio), F56Style.Front);
        //            }
        //            else
        //            {
        //                _F56Device.Pagar(SortedList, Convert.ToInt32(oPagoEfectivo.ValorProcesoCambio), F56Style.Rear);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    else if (oTipoDenominacion == TipoMedioPago.Moneda)
        //    {

        //        View.General_Events = "Presenter-AdministrarProcesoPago -> ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
        //        oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

        //        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
        //        {
        //            if (oResultadoOperacion.ListaEntidadDatos != null)
        //            {
        //                ok = true;

        //                List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

        //                for (int i = 0; i < Transacciones.Count; i++)
        //                {
        //                    TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

        //                    if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
        //                    {
        //                        RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
        //                    }
        //                }

        //                ObtenerInfoPartes();
        //            }
        //        }
        //        else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
        //        {
        //            Model.StartCashPayment(oPaymentDevice);
        //            Thread.Sleep(30000);

        //            View.General_Events = "REINTENTO 1 -> Presenter AdministrarProcesoPago: Monedas // ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorEntregado: " + oPagoEfectivo.ValorEntregado + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
        //            oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

        //            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
        //            {
        //                if (oResultadoOperacion.ListaEntidadDatos != null)
        //                {
        //                    ok = true;

        //                    List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

        //                    for (int i = 0; i < Transacciones.Count; i++)
        //                    {
        //                        TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

        //                        if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
        //                        {
        //                            RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
        //                        }
        //                    }

        //                    ObtenerInfoPartes();
        //                }
        //            }
        //            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
        //            {
        //                View.General_Events = "REINTENTO 2 -> Presenter AdministrarProcesoPago: Monedas // ValorCambio: " + oPagoEfectivo.ValorCambio + " ValorEntregado: " + oPagoEfectivo.ValorEntregado + " ValorPago: " + oPagoEfectivo.ValorPago + " ValorProcesoCambio: " + oPagoEfectivo.ValorProcesoCambio;
        //                oResultadoOperacion = Model.AdministrarProcesoPagoEfectivo(oPaymentDevice, oDispositivoPago, oPagoEfectivo, oTipoDenominacion);

        //                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
        //                {
        //                    if (oResultadoOperacion.ListaEntidadDatos != null)
        //                    {
        //                        ok = true;

        //                        List<TransaccionEfectivo> Transacciones = (List<TransaccionEfectivo>)oResultadoOperacion.ListaEntidadDatos;

        //                        for (int i = 0; i < Transacciones.Count; i++)
        //                        {
        //                            TransaccionEfectivo oTransaccionEfectivo = Transacciones[i];

        //                            if (oTransaccionEfectivo.TipoParte == TipoParte.Hopper)
        //                            {
        //                                RegistrarMovimiento(TipoOperacion.Pago, oTransaccionEfectivo.TipoParte, TipoMovimiento.Salida, null, oTransaccionEfectivo.Denominacion, oTransaccionEfectivo.Cantidad);
        //                            }
        //                        }

        //                        ObtenerInfoPartes();
        //                    }
        //                }
        //                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
        //                {
        //                    ok = false;
        //                    View.General_Events = "Error Presenter-AdministrarProcesoPago: " + oResultadoOperacion.Mensaje;
        //                }
        //            }
        //        }

        //    }
        //    return ok;
        //}
        void oF56SerialDevice_DeviceMessage(object sender, EventArgs e)
        {
            var i = (EventArgsF56SerialCommunicationDevice)e;

        }
        void oF56EventDevice_DeviceMessage(object sender, EventArgs e)
        {
            var i = (EventArgsF56EventsDevice)e;
            View.General_EventsBill = "Presenter-BilleteroF56 -> Solicitud F56 / " + i.StatesF56Device;

            switch (i.StatesF56Device)
            {
                #region EndInicialization
                case StatesF56Device.EndInicialization:
                    //if (!_BanderaTransaccionPendienteInicio)
                    //{
                        View.General_EventsBill = "Presenter-BilleteroF56 -> Inicialization ok ";
                        if (ValidarSaldosMinimos())
                        {
                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                        }
                        else
                        {
                            View.General_EventsBill = "Presenter-BilleteroF56 -> Inicialization ok con saldos minimos insuficientes";
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                    //}
                    //else
                    //{
                    //    _BanderaTransaccionPendienteInicio = false;
                    //    View.General_EventsBill = "Presenter-BilleteroF56 -> Inicialization ok con transaccion pendiente";
                    //    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    //}
                    break;
                #endregion
                case StatesF56Device.AbnormalBillCount:
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Error AbnormalBillCount";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "BilleteroF56", "Error AbnormalBillCount", 1);
                    View.BilleteroReady = false;
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    break;
                case StatesF56Device.AbnormalBillCountAndTransportFront:
                    List<F56PayParameter> listaPurgaAbnormal = i.Rechazado;
                    if (listaPurgaAbnormal.Count > 0)
                    {
                        foreach (F56PayParameter item in listaPurgaAbnormal)
                        {
                            if (item.Cantidad > 0)
                            {
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.F56, TipoMovimiento.Salida, item.NumeroCassette, item.Denominacion, item.Cantidad);
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Purga, TipoMovimiento.Entrada, null, item.Denominacion, item.Cantidad);
                            }
                        }
                    }
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Error AbnormalBillCountAndTransportFront";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "BilleteroF56", "Sistema suspendido error AbnormalBillCountAndTransportFront", 1);
                    View.BilleteroReady = false;
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.Imprimir();
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    break;
                case StatesF56Device.AbnormalBillCountAndTransportRear:
                    List<F56PayParameter> listaPurgaAbnormal2 = i.Rechazado;
                    if (listaPurgaAbnormal2.Count > 0)
                    {
                        foreach (F56PayParameter item in listaPurgaAbnormal2)
                        {
                            if (item.Cantidad > 0)
                            {
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.F56, TipoMovimiento.Salida, item.NumeroCassette, item.Denominacion, item.Cantidad);
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Purga, TipoMovimiento.Entrada, null, item.Denominacion, item.Cantidad);
                            }
                        }
                    }
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Error AbnormalBillCountAndTransportRear";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "BilleteroF56", "Sistema suspendido error AbnormalBillCountAndTransportRear", 1);
                    View.BilleteroReady = false;
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.Imprimir();
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    break;
                case StatesF56Device.AbnormalBillTransportation:
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Error AbnormalBillTransportation";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "BilleteroF56", "Sistema suspendido error AbnormalBillTransportation", 1);
                    View.BilleteroReady = false;
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                    View.TicketDevolucion = true;
                    View.Imprimir();
                    break;
                case StatesF56Device.AbnormalInitialization:
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Error AbnormalInitialization";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "BilleteroF56", "Sistema suspendido error AbnormalInitialization", 1);
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    break;
                case StatesF56Device.ClosingShutter:
                    break;
                case StatesF56Device.Disable:
                    break;
                case StatesF56Device.Dispensing:
                    break;
                case StatesF56Device.EndDispensePack:
                    List<F56PayParameter> listaDispensada = i.Dispensado; //Aca es donde obtiene los valores que dispenso.
                    List<F56PayParameter> listaPurga = i.Rechazado; //Aca es donde obtiene los valores que rechazo.
                    int CALC = 0;

                    if (listaDispensada.Count > 0)
                    {
                        foreach (F56PayParameter item in listaDispensada)
                        {
                            if (item.Cantidad > 0)
                            {
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.F56, TipoMovimiento.Salida, item.NumeroCassette, item.Denominacion, item.Cantidad);
                                View.General_EventsBill = "Presenter-BilleteroF56 -> Dispensa denominacion = " + item.Denominacion + " cantidad = " + item.Cantidad;
                                CALC = item.Denominacion * item.Cantidad;

                                RESULT += CALC;
                                CALC = 0;
                            }
                        }
                    }

                    if (listaPurga.Count > 0)
                    {
                        foreach (F56PayParameter item in listaPurga)
                        {
                            if (item.Cantidad > 0)
                            {
                                View.General_EventsBill = "Presenter-BilleteroF56 -> A Purga denominacion = " + item.Denominacion + " cantidad = " + item.Cantidad;
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.F56, TipoMovimiento.Salida, item.NumeroCassette, item.Denominacion, item.Cantidad);
                                RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Purga, TipoMovimiento.Entrada, null, item.Denominacion, item.Cantidad);
                            }
                        }
                    }
                    break;
                case StatesF56Device.EndDispense:
                    ////Finaliza el proceso de pago, el cleinte retiro el dinero y el shutter se cerro. En este momento se finaliza la transaccion
                    View.General_EventsBill = "Presenter-BilleteroF56 -> EndDispense";
                    if (i.Resto > 0)
                    {
                        View.PagoEfectivo.ValorProcesoCambio = i.Resto;

                        if (View.PagoEfectivo.ValorProcesoCambio > 0)
                        {

                            if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                            {
                                if (View.BanderaCancelacion)
                                {
                                    if (View.TipoPago == "MENSUALIDAD")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else if (View.TipoPago == "EVENTO")
                                    {
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }
                                    else
                                    {
                                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                        if (ValidarSaldosMinimos())
                                        {
                                            View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                        }
                                    }

                                }
                                else if (!View.BanderaRecaudo)
                                {
                                    if (View.BanderaPagoFinal)
                                    {
                                        if (View.TipoPago == "MENSUALIDAD")
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                                            if (View.CobroTarjetaMensual)
                                            {
                                                ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                                            }
                                            View.Cnt_Reinicio = 0;
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                        //else if (View.TipoPago == "EVENTO")
                                        //{
                                        //    EscribirTarjeta();
                                        //    View.Efectivo = true;
                                        //    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                        //    View.Cnt_Reinicio = 0;
                                        //    View.SetearPantalla(Pantalla.ImprimirFactura);
                                        //    View.BanderaRecaudo = false;
                                        //    View.BanderaPagoFinal = false;
                                        //}
                                        else
                                        {
                                            EscribirTarjeta();
                                            View.Efectivo = true;
                                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                            if (View.TipoPago == "REPOSICIÓN")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "CASCOS")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                                            }
                                            else if (View.TipoPago == "EVENTO")
                                            {
                                                ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                            }
                                            View.Cnt_Reinicio = 0;
                                            View.SetearPantalla(Pantalla.ImprimirFactura);
                                            View.BanderaRecaudo = false;
                                            View.BanderaPagoFinal = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                                View.Imprimir();
                                View.TicketDevolucion = true;
                                View.Imprimir();
                                View.SetearPantalla(Pantalla.SistemaSuspendido);
                            }
                        }
                        else
                        {

                            if (View.BanderaCancelacion)
                            {

                                View.TicketDevolucion = true;
                                View.Imprimir();
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);

                                if (ValidarSaldosMinimos())
                                {
                                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                }

                            }
                            else if (!View.BanderaRecaudo)
                            {
                                if (View.BanderaPagoFinal)
                                {
                                    EscribirTarjeta();
                                    View.TicketDevolucion = true;
                                    View.Imprimir();
                                    View.Efectivo = true;
                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                                    View.SetearPantalla(Pantalla.ImprimirFactura);
                                    View.BanderaRecaudo = false;
                                    View.BanderaPagoFinal = false;
                                }

                            }
                        }
                    }
                    else
                    {
                        if (View.BanderaCancelacion)
                        {
                            if (View.TipoPago == "MENSUALIDAD")
                            {
                                ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Cancelado);
                                if (ValidarSaldosMinimos())
                                {
                                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                }
                            }
                            else if (View.TipoPago == "EVENTO")
                            {
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                if (ValidarSaldosMinimos())
                                {
                                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                }
                            }
                            else
                            {
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                if (ValidarSaldosMinimos())
                                {
                                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                }
                            }
                        }
                        else if (View.BanderaPagoFinal)
                        {
                            if (View.TipoPago == "MENSUALIDAD")
                            {
                                EscribirTarjeta();
                                View.Efectivo = true;
                                ConfirmarOperacion(TipoOperacion.Mensualidad, TipoEstadoPago.Aprobado);
                                View.Cnt_Reinicio = 0;
                                if (View.CobroTarjetaMensual)
                                {
                                    ConfirmarOperacion(TipoOperacion.CobroTarjetaMensual, TipoEstadoPago.Aprobado);
                                }
                                View.SetearPantalla(Pantalla.ImprimirFactura);
                                View.BanderaRecaudo = false;
                                View.BanderaPagoFinal = false;
                            }
                            //if (View.TipoPago == "EVENTO")
                            //{
                            //    EscribirTarjeta();
                            //    View.Efectivo = true;
                            //    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                            //    View.Cnt_Reinicio = 0;
                            //    View.SetearPantalla(Pantalla.ImprimirFactura);
                            //    View.BanderaRecaudo = false;
                            //    View.BanderaPagoFinal = false;
                            //}
                            else
                            {
                                EscribirTarjeta();
                                View.Efectivo = true;
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                if (View.TipoPago == "REPOSICIÓN")
                                {
                                    ConfirmarOperacion(TipoOperacion.Reposicion, TipoEstadoPago.Aprobado);
                                }
                                else if (View.TipoPago == "CASCOS")
                                {
                                    ConfirmarOperacion(TipoOperacion.Casco, TipoEstadoPago.Aprobado);
                                }
                                else if (View.TipoPago == "EVENTO")
                                {
                                    ConfirmarOperacion(TipoOperacion.Evento, TipoEstadoPago.Aprobado);
                                }
                                View.Cnt_Reinicio = 0;
                                View.SetearPantalla(Pantalla.ImprimirFactura);
                                View.BanderaRecaudo = false;
                                View.BanderaPagoFinal = false;
                            }
                        }
                       
                    }
                    break;
                case StatesF56Device.EndPartialDispense:
                    View.PagoEfectivo.ValorProcesoCambio = i.Resto;
                    if (View.PagoEfectivo.ValorProcesoCambio < View.DenomMenor)
                    {
                        if (AdministrarProcesoPago(View.PagoEfectivo, TipoMedioPago.Moneda))
                        {
                            if (View.ProcesoPago)
                            {
                                if (View.PagoEfectivo.ValorProcesoCambio <= 0)
                                {
                                    //Entrega todo el cambio
                                    View.General_EventsBill = "Presenter-BilleteroF56 -> EndPartialDispense con Resto = " + i.Resto + " / Entrega cambio completo";
                                    //Model.DetenerSecuencias();
                                    //ObtenerGuiaConfirmacion();

                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                                    View.SetearPantalla(Pantalla.ImprimirFactura);
                                    View.ProcesoPago = true;
                                    //View.Imprimir();
                                    //if (ValidarSaldosMinimos())
                                    //{
                                    //    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    //}
                                }
                                else
                                {
                                    View.General_EventsBill = "Presenter-BilleteroF56 -> EndPartialDispense con Resto = " + i.Resto + " / No entrega cambio de monedas completo";
                                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Monedero", "Sistema Suspendido - No entrega valor complero Monedas en Pago Valor falto por entregar: " + View.PagoEfectivo.ValorProcesoCambio, 1);
                                    //Model.DetenerSecuencias();
                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                                    View.Imprimir();
                                    View.TicketDevolucion = true;
                                    View.Imprimir();
                                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                                }
                            }
                            else //cancelacion
                            {
                                if (View.PagoEfectivo.ValorProcesoCambio <= 0)
                                {
                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                    if (ValidarSaldosMinimos())
                                    {
                                        View.SetearPantalla(Pantalla.PublicidadPrincipal);
                                    }
                                }
                                else
                                {
                                    View.General_EventsBill = "Presenter-BilleteroF56 Resto: Cancela operacion, no regresa vueltas";
                                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Billetero", "Sistema Suspendido - No entrega devolucion al cancelar operacion", 1);
                                    //Model.DetenerSecuencias();
                                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                    View.TicketDevolucion = true;
                                    View.Imprimir();

                                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                                }
                            }
                        }
                        else
                        {

                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                            if (View.ProcesoPago)
                            {
                                View.General_EventsBill = "Presenter-BilleteroF56 Resto: Error al dar vuetas billetero";
                                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Billetero", "Sistema Suspendido - No retorna vueltas completas en Pago Valor falto por entregar: " + View.PagoEfectivo.ValorProcesoCambio, 1);
                                View.Imprimir();
                                View.TicketDevolucion = true;
                                View.Imprimir();

                                View.SetearPantalla(Pantalla.SistemaSuspendido);
                            }
                            else
                            {
                                View.General_EventsBill = "Presenter-BilleteroF56 Resto: Error al dar vuetas falto " + +View.PagoEfectivo.ValorProcesoCambio;
                                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Billetero", "Sistema Suspendido - No entrega devolucion al cancelar operacion falto " + View.PagoEfectivo.ValorProcesoCambio, 1);
                                View.TicketDevolucion = true;
                                View.Imprimir();
                                View.SetearPantalla(Pantalla.SistemaSuspendido);
                            }
                        }
                    }
                    else
                    {

                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Error_Dispositivo);
                        if (View.ProcesoPago)
                        {
                            View.General_EventsBill = "Presenter-BilleteroF56 -> Resto = " + i.Resto + " / Error al dar vuetas billetero";
                            RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Billetero", "No retorna vueltas completas en Pago Valor falto por entregar: " + View.PagoEfectivo.ValorProcesoCambio, 1);
                            View.Imprimir();
                            View.TicketDevolucion = true;
                            View.Imprimir();
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                        else //cancelacion
                        {
                            View.General_EventsBill = "Presenter-BilleteroF56 -> Resto = " + i.Resto + " /  Error al dar vuetas billetero devolucion";
                            RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "Billetero", "Sistema Suspendido - No entrega devolucion al cancelar operacion", 1);
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                        }
                    }
                    break;
                case StatesF56Device.EndTransport:
                    //Infroma que acabo de transaportar, no es necesario hacer nada en este evento
                    break;
                case StatesF56Device.EndWaitingRemoveBills:
                    //Informa que los billetes ya fueron retirados por el cliente, no es necesario hacer nada
                    break;
                case StatesF56Device.ErrorDeviceInformation:
                    //Informa que hay un error al inicializar en billetero, ir a suspendido
                    View.BilleteroReady = false;
                    break;
                case StatesF56Device.Inicializing:
                    //Infroma que el billetero esta inicializando, no es necesario hacer nada en este evento
                    break;
                case StatesF56Device.InicializingDispensing:
                    //Infroma que inicializo a dispensar, no es necesario hacer nada en este eveneto
                    break;
                case StatesF56Device.InicializingTransporting:
                    //Informa que inicializa a transportar, no es necesario hacer nada en este evento 
                    break;
                case StatesF56Device.Nothing:
                    //EVENTO TOTALMENTE DESPRECIABLE 
                    break;
                case StatesF56Device.Transporting:
                    //Informa que se esta transportando el dinero. No es necesario hacer nada en este evento
                    break;
                case StatesF56Device.WaitingRemoveBills:
                    //View.SetearPantalla(Pantalla.RetiroDinero);
                    //Informa que se esta esperando a que el usuario retire los billetes, no es necesario hacer nada
                    break;
                default:
                    View.General_EventsBill = "Presenter-BilleteroF56 -> Unknow";
                    break;
            }

        }
        #endregion

        #region Azkoyen

        #region EVENTOS
        void oAzcoyenSerialDevice_DeviceMessage(object sender, EventArgs e)
        {
            var i = (EventArgsAzkSerialCommunicationDevice)e;

        }
        void oAzcoyenDevice_DeviceMessage(object sender, EventArgs e)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            var i = (EventArgsAzkDevice)e;


            List<TipoMoneda> monedas = i.CoinsCtCoin;

            switch (i.result)
            {
                #region ConexionExitosa
                case StatesAzkDevice.ConexionExitosa:
                    //View.General_EventsBill = i.result.ToString();
                    Reset(Globales.sIdH1, 1);
                    break;
                #endregion

                #region ErrorConexion
                case StatesAzkDevice.ErrorConexion:
                    View.General_EventsBill = i.result.ToString();

                    break;
                #endregion

                #region TestHopper1
                case StatesAzkDevice.TestHopper1:
                    View.General_EventsBill = i.result.ToString();
                    ACK(Globales.sIdH1, 1);
                    break;
                #endregion

                #region TestHopper2
                case StatesAzkDevice.TestHopper2:
                    View.General_EventsBill = i.result.ToString();
                    ACK(Globales.sIdH2, 2);
                    break;
                #endregion

                #region TestHopper3
                case StatesAzkDevice.TestHopper3:
                    View.General_EventsBill = i.result.ToString();
                    ACK(Globales.sIdH3, 3);
                    break;
                #endregion

                #region TestHopper4
                case StatesAzkDevice.TestHopper4:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region ConexionHopper1Ready
                case StatesAzkDevice.Hopper1Ready:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper1Ready = true;
                    Reset(Globales.sIdH2, 2);
                    break;
                #endregion

                #region ConexionHopper2Ready
                case StatesAzkDevice.Hopper2Ready:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper2Ready = true;
                    Reset(Globales.sIdH3, 3);
                    break;
                #endregion

                #region ConexionHopper3Ready
                case StatesAzkDevice.Hopper3Ready:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper3Ready = true;
                    //ACK(Globales.sIdH4, 4);
                    break;
                #endregion

                #region ConexionHopper4Ready
                case StatesAzkDevice.Hopper4Ready:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper4Ready = true;
                    break;
                #endregion

                #region ConexionHopper1Habilitado
                case StatesAzkDevice.Hopper1Habilitado:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper1Habilitado = true;
                    HabilitarHPS(Globales.sIdH2, 2);
                    break;
                #endregion

                #region ConexionHopper2Habilitado
                case StatesAzkDevice.Hopper2Habilitado:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper2Habilitado = true;
                    HabilitarHPS(Globales.sIdH3, 3);
                    break;
                #endregion

                #region ConexionHopper3Habilitado
                case StatesAzkDevice.Hopper3Habilitado:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper3Habilitado = true;
                    //HabilitarHPS(Globales.sIdH4, 4);
                    break;
                #endregion

                #region ConexionHopper4Habilitado
                case StatesAzkDevice.Hopper4Habilitado:
                    View.General_EventsBill = i.result.ToString();
                    View.Hopper4Habilitado = true;
                    break;
                #endregion

                #region ValidadorReady
                case StatesAzkDevice.ValidadorReady:
                    View.General_EventsBill = i.result.ToString();
                    View.ValidadorReady = true;
                    break;
                #endregion

                #region ErrorConexionHoppers
                case StatesAzkDevice.ErrorConexionHopper:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region Habilitar Recepcion
                case StatesAzkDevice.HabilitarRecepcion:
                    View.General_EventsBill = i.result.ToString();
                    if (View.BanderaRecaudo)
                    {
                        if (monedas.Count > 0)
                        {
                            View.PagoEfectivo.ValorRecibido += monedas[0].Denominacion;
                            View.General_EventsBill = "FrontEnd Principal RegistrarMovimiento Moneda" + " Denominacion " + monedas[0].Denominacion;
                            RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Coinbag, TipoMovimiento.Entrada, null, monedas[0].Denominacion, 1);

                        }

                        if (View.TipoPago == "MENSUALIDAD")
                        {
                            View.SetearPantalla(Pantalla.DetallePagoMensual);
                        }
                        else
                        {
                            View.SetearPantalla(Pantalla.DetallePago);
                        }



                        HabilitarRecepcion();

                    }
                    break;
                #endregion

                #region DesHabilitar Recepcion
                case StatesAzkDevice.DesHabilitarRecepcion:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion
            }
        }
        #endregion

        #region FUNCIONES
        public void Reset(string IdH, int H)
        {
            oAzcoyenDevice.Reset(IdH, H);
        }
        public void ACK(string IdH, int H)
        {
            oAzcoyenDevice.ACK(IdH, H);
        }
        public void ConectarHoppers()
        {
            oAzcoyenDevice.Conectar(Globales.sPuertoAzk);
        }
        public void HabilitarHPS(string IdH, int H)
        {
            oAzcoyenDevice.HABILITAR(IdH, H);
        }
        public void HabilitarRecepcion()
        {
            oAzcoyenDevice.HTM(Globales.sIdV);
            Thread.Sleep(500);
            oAzcoyenDevice.HabilitarRecepcion(Globales.sIdV);
        }
        public void DeshabilitarRecepcionMonedas()
        {
            oAzcoyenDevice.DesHabilitarRecepcion(Globales.sIdV);
        }
        public void Pagar()
        {
            oAzcoyenDevice.PayOut(200, 200, Pay_Unit.Hopper2);
        }

        public void ConectarValidador()
        {
            string IdV = Globales.sIdV;

            oAzcoyenDevice.ConectarValidador(IdV);
        }
        #endregion

        #endregion

        #region SmartPayOut

        #region EVENTOS
        void oSmartPayOutSerialDevice_DeviceMessage(object sender, EventArgs e)
        {
            var i = (EventArgsPayOutSerialCommunicationDevice)e;

        }
        void oSmartPayOutDevice_DeviceMessage(object sender, EventArgs e)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            var i = (EventArgsPayOutDevice)e;


            List<TipoBillete> Billetes = i.BilletesBox;


            View.Infovalidator = i.resultString.Mensaje;

            switch (i.result)
            {
                #region ConexionExitosa
                case StatesPayOutDevice.ConexionExitosa:
                    if (!View.LogConexion)
                    {
                        View.General_Events = i.result.ToString();
                        View.LogConexion = true;
                    }
                    View.BilleteroReady = true;
                    //DisableSmartPayOut();
                    break;
                #endregion

                #region ErrorConexion
                case StatesPayOutDevice.ErrorConexion:
                    if (!View.LogConexion)
                    {
                        View.General_Events = i.result.ToString();
                        View.LogConexion = true;
                    }
                    View.BilleteroReady = false;
                    break;
                #endregion

                #region ValidadorReady
                case StatesPayOutDevice.ValidadorReady:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region DisableValidator
                case StatesPayOutDevice.DisableValidator:
                    View.General_EventsBill = i.result.ToString();
                    //DisableSmartPayOut();
                    ACK();
                    break;
                #endregion

                #region DisableValidatorError
                case StatesPayOutDevice.DisableValidatorError:
                    View.General_EventsBill = i.result.ToString();
                    ACK();
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    break;
                #endregion

                #region EnableValidator
                case StatesPayOutDevice.EnableValidator:
                    View.General_EventsBill = i.result.ToString();
                    ACK();
                    break;
                #endregion

                #region Nothing
                case StatesPayOutDevice.Nothing:

                    break;
                #endregion

                #region SSP_POLL_SLAVE_RESET
                case StatesPayOutDevice.SSP_POLL_SLAVE_RESET:
                    View.General_EventsBill = "Front End SSP_POLL_SLAVE_RESET";
                    //if (!bSuspendido)
                    //{
                    View.General_EventsBill = "Front End ERROR  SSP_POLL_SLAVE_RESET";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    //}
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_DISABLED
                case StatesPayOutDevice.SSP_POLL_DISABLED:
                    //if (!bSuspendido)
                    //{
                    //    bSuspendido = true;
                    //View.General_EventsBill = "Front End ERROR  SSP_POLL_DISABLED";
                    //View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    //}
                    //if (Presentacion == Pantalla.PublicidadPrincipal)
                    //{
                    //    Recepcion();
                    //}
                    break;
                #endregion

                #region SSP_POLL_READ_NOTE
                case StatesPayOutDevice.SSP_POLL_READ_NOTE:
                    View.General_EventsBill = "Front End SSP_POLL_READ_NOTE";
                    View.General_EventsBill = i.resultString.Mensaje;
                    ACK();

                    //bIntento = true;
                    break;
                #endregion

                #region SSP_POLL_CREDIT_NOTE
                case StatesPayOutDevice.SSP_POLL_CREDIT_NOTE:
                    View.General_EventsBill = "Front End SSP_POLL_CREDIT_NOTE";
                    if (View.BanderaRecaudo)
                    {
                        if (RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Box, TipoMovimiento.Entrada, null, Billetes[0].Denominacion, 1))
                        {
                            View.PagoEfectivo.ValorRecibido += Billetes[0].Denominacion;
                            View.Cnt_Reinicio = 0;
                            View.General_EventsBill = "FrontEnd Principal RegistrarMovimiento Entrada Billetero" + " Denominacion " + Billetes[0].Denominacion;
                            if (View.TipoPago == "MENSUALIDAD")
                            {
                                View.SetearPantalla(Pantalla.DetallePagoMensual);
                            }
                            else
                            {
                                View.SetearPantalla(Pantalla.DetallePago);
                            }
                        }
                    }
                    else
                    {
                        if (View.CargaBilletesBB)
                        {
                            View.Cnt_Reinicio = 0;
                            View.General_EventsBill = "FrontEnd Principal RegistrarMovimiento Entrada Carga Billetero" + " Denominacion " + Billetes[0].Denominacion;
                        }
                    }
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_REJECTING
                case StatesPayOutDevice.SSP_POLL_NOTE_REJECTING:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_REJECTING";
                    //if (!bSuspendido)
                    //{
                    //    General_EventsBill = "Front End ERROR  SSP_POLL_NOTE_REJECTING";
                    //View.SetearPantalla(Pantalla.SistemaSuspendido);
                    //}
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_REJECTED
                case StatesPayOutDevice.SSP_POLL_NOTE_REJECTED:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_REJECTED";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_STACKING
                case StatesPayOutDevice.SSP_POLL_NOTE_STACKING:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_STACKING";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_FLOATING
                case StatesPayOutDevice.SSP_POLL_FLOATING:
                    View.General_EventsBill = "Front End SSP_POLL_FLOATING";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_STACKED
                case StatesPayOutDevice.SSP_POLL_NOTE_STACKED:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_STACKED";
                    if (View.CargaBilletesBB)
                    {
                        if (Billetes.Count > 0)
                        {

                            if (RegistrarMovimiento(TipoOperacion.Carga, TipoParte.Box, TipoMovimiento.Entrada, null, Billetes[0].Denominacion, 1))
                            {
                                ObtenerInfoPartes();
                                View.SetearPantalla(Pantalla.MenuCargBilletes);
                            }
                        }
                    }
                    else
                    {
                        if (Billetes.Count > 0)
                        {
                            RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Box, TipoMovimiento.Entrada, null, Billetes[0].Denominacion, 1);
                        }
                    }
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_FLOATED
                case StatesPayOutDevice.SSP_POLL_FLOATED:
                    View.General_EventsBill = "Front End SSP_POLL_FLOATED";
                    //if (!bSuspendido)
                    //{
                    //    General_EventsBill = "Front End ERROR  SH SSP_POLL_INCOMPLETE_PAYOUT";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    //}
                    break;
                #endregion

                #region SSP_POLL_NOTE_STORED_IN_PAYOUT
                case StatesPayOutDevice.SSP_POLL_NOTE_STORED_IN_PAYOUT:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_STORED_IN_PAYOUT";
                    if (View.CargaBilletesBB)
                    {
                        if (RegistrarMovimiento(TipoOperacion.Carga, TipoParte.Cassette, TipoMovimiento.Entrada, null, Billetes[0].Denominacion, 1))
                        {
                            ObtenerInfoPartes();
                            View.SetearPantalla(Pantalla.MenuCargBilletes);
                        }
                    }
                    else
                    {
                        RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Cassette, TipoMovimiento.Entrada, null, Billetes[0].Denominacion, 1);
                    }
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_SAFE_NOTE_JAM
                case StatesPayOutDevice.SSP_POLL_SAFE_NOTE_JAM:
                    View.General_EventsBill = "Front End SSP_POLL_SAFE_NOTE_JAM";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_UNSAFE_NOTE_JAM
                case StatesPayOutDevice.SSP_POLL_UNSAFE_NOTE_JAM:
                    View.General_EventsBill = "Front End SSP_POLL_UNSAFE_NOTE_JAM";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_ERROR_DURING_PAYOUT
                case StatesPayOutDevice.SSP_POLL_ERROR_DURING_PAYOUT:
                    View.General_EventsBill = "Front End SSP_POLL_ERROR_DURING_PAYOUT";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_FRAUD_ATTEMPT
                case StatesPayOutDevice.SSP_POLL_FRAUD_ATTEMPT:
                    View.General_EventsBill = "Front End SSP_POLL_FRAUD_ATTEMPT";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_STACKER_FULL
                case StatesPayOutDevice.SSP_POLL_STACKER_FULL:
                    View.General_EventsBill = "Front End SSP_POLL_STACKER_FULL";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_CLEARED_FROM_FRONT
                case StatesPayOutDevice.SSP_POLL_NOTE_CLEARED_FROM_FRONT:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_CLEARED_FROM_FRONT";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_CLEARED_TO_CASHBOX
                case StatesPayOutDevice.SSP_POLL_NOTE_CLEARED_TO_CASHBOX:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_CLEARED_TO_CASHBOX";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP
                case StatesPayOutDevice.SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP
                case StatesPayOutDevice.SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_CASHBOX_REMOVED
                case StatesPayOutDevice.SSP_POLL_CASHBOX_REMOVED:
                    View.General_EventsBill = "Front End SSP_POLL_CASHBOX_REMOVED";
                    break;
                #endregion

                #region SSP_POLL_CASHBOX_REPLACED
                case StatesPayOutDevice.SSP_POLL_CASHBOX_REPLACED:
                    View.General_EventsBill = "Front End SSP_POLL_CASHBOX_REPLACED";
                    //btn_ConfirmarArqueo.Visible = true;
                    //btn_ConfirmarArqueoTotal.Visible = true;
                    break;
                #endregion

                #region SSP_POLL_DISPENSING
                case StatesPayOutDevice.DISPENSING:
                    View.General_EventsBill = "Front End SSP_POLL_DISPENSING";

                    break;
                #endregion

                #region SSP_POLL_DISPENSED
                case StatesPayOutDevice.DISPENSED:
                    //General_EventsBill = "Front End SSP_POLL_DISPENSED";
                    //if (_BanderaCancelacion || _BanderaPagoFinal)
                    //{
                    //    if (!bIntentob)
                    //    {
                    //        for (int i = 0; i < Payout.UnitDataList.Count; i++)
                    //        {
                    //            if (Payout.UnitDataList[i].Value == 100000)
                    //            {
                    //                _1000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 200000)
                    //            {
                    //                _2000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 500000)
                    //            {
                    //                _5000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 1000000)
                    //            {
                    //                _10000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 2000000)
                    //            {
                    //                _20000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 5000000)
                    //            {
                    //                _50000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //            else if (Payout.UnitDataList[i].Value == 10000000)
                    //            {
                    //                _100000bDespues = Payout.UnitDataList[i].Level;
                    //            }
                    //        }

                    //        _Dispensadob1000 = _1000bAntes - _1000bDespues;
                    //        _Dispensadob2000 = _2000bAntes - _2000bDespues;
                    //        _Dispensadob5000 = _5000bAntes - _5000bDespues;
                    //        _Dispensadob10000 = _10000bAntes - _10000bDespues;
                    //        _Dispensadob20000 = _20000bAntes - _20000bDespues;
                    //        _Dispensadob50000 = _50000bAntes - _50000bDespues;
                    //        _Dispensadob100000 = _100000bAntes - _100000bDespues;



                    //        for (int i = 0; i < Payout.UnitDataList.Count; i++)
                    //        {
                    //            if (_Dispensadob1000 > 0 && Payout.UnitDataList[i].Value == 100000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 1000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob1000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob2000 > 0 && Payout.UnitDataList[i].Value == 200000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 2000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob2000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob5000 > 0 && Payout.UnitDataList[i].Value == 500000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 5000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob5000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob10000 > 0 && Payout.UnitDataList[i].Value == 1000000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 10000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob10000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob20000 > 0 && Payout.UnitDataList[i].Value == 2000000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 20000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob20000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob50000 > 0 && Payout.UnitDataList[i].Value == 5000000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 50000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob50000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //            else if (_Dispensadob100000 > 0 && Payout.UnitDataList[i].Value == 10000000)
                    //            {
                    //                _TransaccionEfectivoBilletes = new TransaccionEfectivoBillete();
                    //                _TransaccionEfectivoBilletes.TipoParte = TipoParte.Cassette;
                    //                _TransaccionEfectivoBilletes.Denominacion = 100000;
                    //                _TransaccionEfectivoBilletes.Cantidad = _Dispensadob100000;

                    //                _TransaccionesBilletes.Add(_TransaccionEfectivoBilletes);
                    //            }
                    //        }

                    //        _PagoEfectivo.ValorProcesoCambio = 0;
                    //        General_EventsBill = "FrontEnd SSP_POLL_DISPENSED";
                    //        bIntentob = true;
                    //        _frmPrincipal_Presenter.ProcesoPagoBilletes(_PagoEfectivo, TipoMedioPago.Moneda);
                    //    }
                    //}
                    break;
                #endregion

                #region SSP_POLL_EMPTYING
                case StatesPayOutDevice.SSP_POLL_EMPTYING:
                    View.General_EventsBill = "Front End SSP_POLL_EMPTYING";
                    break;
                #endregion

                #region SSP_POLL_EMPTIED
                case StatesPayOutDevice.SSP_POLL_EMPTIED:
                    View.General_EventsBill = "Front End SSP_POLL_EMPTIED";
                    View.SetearPantalla(Pantalla.ArqueoTotal);
                    break;
                #endregion

                #region SSP_POLL_SMART_EMPTYING
                case StatesPayOutDevice.SSP_POLL_SMART_EMPTYING:
                    //General_EventsBill = "Front End SSP_POLL_SMART_EMPTYING";
                    break;
                #endregion

                #region SSP_POLL_SMART_EMPTIED
                case StatesPayOutDevice.SSP_POLL_SMART_EMPTIED:
                    //General_EventsBill = "Front End SSP_POLL_SMART_EMPTIED";
                    break;
                #endregion

                #region SSP_POLL_JAMMED
                case StatesPayOutDevice.SSP_POLL_JAMMED:
                    View.General_EventsBill = "Front End SSP_POLL_JAMMED";
                    //if (!bSuspendido)
                    //{
                    //    bSuspendido = true;
                    //    General_EventsBill = "Front End ERROR SSP_POLL_JAMMED";
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    ACK();
                    //}
                    break;
                #endregion

                #region SSP_POLL_HALTED
                case StatesPayOutDevice.SSP_POLL_HALTED:
                    View.General_EventsBill = "Front End SSP_POLL_HALTED";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_INCOMPLETE_PAYOUT
                case StatesPayOutDevice.SSP_POLL_INCOMPLETE_PAYOUT:
                    View.General_EventsBill = "Front End SSP_POLL_INCOMPLETE_PAYOUT";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_INCOMPLETE_FLOAT
                case StatesPayOutDevice.SSP_POLL_INCOMPLETE_FLOAT:
                    View.General_EventsBill = "Front End SSP_POLL_INCOMPLETE_FLOAT";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_TRANSFERED_TO_STACKER
                case StatesPayOutDevice.SSP_POLL_NOTE_TRANSFERED_TO_STACKER:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_TRANSFERED_TO_STACKER";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_NOTE_HELD_IN_BEZEL
                case StatesPayOutDevice.SSP_POLL_NOTE_HELD_IN_BEZEL:
                    View.General_EventsBill = "Front End SSP_POLL_NOTE_HELD_IN_BEZEL";
                    ACK();
                    break;
                #endregion

                #region SSP_POLL_TIME_OUT
                case StatesPayOutDevice.SSP_POLL_TIME_OUT:
                    View.General_EventsBill = "Front End SSP_POLL_TIME_OUT";
                    ACK();
                    break;
                #endregion

            }
        }
        #endregion

        #region FUNCIONES
        public void ConectarSmartPayOut()
        {
            oSmartPayOutDevice.ConectarAceptador(Globales.sPuertoBill);
        }
        public void DisableSmartPayOut()
        {
            //TextBox tt = new TextBox();

            //tt.Text = "OFF";

            oSmartPayOutDevice.Disable("OFF");
        }
        public void ACK()
        {
            oSmartPayOutDevice.ACK();
        }
        public void EnableValidator()
        {
            //TextBox tt = new TextBox();

            //tt.Text = "ON";

            oSmartPayOutDevice.EnableValidator("ON");
        }
        public void Halt()
        {
            oSmartPayOutDevice.Halt();
        }
        #endregion

        #endregion

        #region MiniHub

        public async Task ConectarMiniHub(TipoConexionDispositivo oTipoConexionDispositivo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            ConexionDispositivo oConexionDispositivo = new ConexionDispositivo();
            oConexionDispositivo.oTipoConexionDispositivo = oTipoConexionDispositivo;

            var t1 = await Model.AdministrarConexionCashPayment(View.DtoModulo, oPaymentDevice, oConexionDispositivo);
            oResultadoOperacion = t1;

            if (oTipoConexionDispositivo == TipoConexionDispositivo.Abrir)
            {
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "Presenter-AdministrarConexionMiniHub: " + oResultadoOperacion.Mensaje;
                }
                else
                {
                    View.General_EventsBill = "Error Presenter-AdministrarConexionMiniHub: " + oResultadoOperacion.Mensaje;
                    View.General_EventsBill = "Presenter-AdministrarConexionMiniHub -> Sistema Suspendido - Conexión Fallida MiniHub";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, "MiniHub", "Sistema Suspendido - Conexión Fallida MiniHub", 1);
                    //View.SetearPantalla(Pantalla.SistemaSuspendido);
                }
            }
            else if (oTipoConexionDispositivo == TipoConexionDispositivo.Cerrar)
            {
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "Presenter-AdministrarConexionMiniHub: " + oResultadoOperacion.Mensaje;
                }
                else
                {
                    View.General_EventsBill = "Error Presenter-AdministrarConexionMiniHub: " + oResultadoOperacion.Mensaje;
                    View.General_EventsBill = "Presenter-AdministrarConexionMiniHub -> Sistema Suspendido - Desconexión Fallida";
                }
            }
        }
        void oPaymentDevice_DeviceMessage(object sender, EventArgs e)
        {
            var i = (EventArgsPaymentDevice)e;

            PaymentEvent oPaymentEvent = (PaymentEvent)i.result.EntidadDatos;

            int id_event = oPaymentEvent.id_event;
            int device = oPaymentEvent.device;
            int message = oPaymentEvent.message;
            string m = oPaymentEvent.wm_event;
            string view_message = string.Empty;

            if (id_event == (int)Event.Payment_Unit)
            {
                if (device == (int)Pay_Unit.Coin_Validator)
                {
                    view_message += "Coin Validator: ";
                    Status_Messages(device, message, ref view_message);
                }
                else if (device == (int)Pay_Unit.Hopper1)
                {
                    view_message += "Hopper 1: ";
                    Status_Messages(device, message, ref view_message);
                }
                else if (device == (int)Pay_Unit.Hopper2)
                {
                    view_message += "Hopper 2: ";
                    Status_Messages(device, message, ref view_message);
                }
                else if (device == (int)Pay_Unit.Hopper3)
                {
                    view_message += "Hopper 3: ";
                    Status_Messages(device, message, ref view_message);
                }
                else if (device == (int)Pay_Unit.Hopper4)
                {
                    view_message += "Hopper 4: ";
                    Status_Messages(device, message, ref view_message);
                }
                else
                {
                    view_message += "Evento aún no controlado Payment Unit Status: ";
                    view_message += m;
                }
            }
            else if (id_event == (int)Event.Cash_Acceptance)
            {
                if (device == (int)Pay_Unit.Coin_Validator)
                {
                    view_message += "Coin Validator: ";
                    view_message += "Cash Acceptance - Denomination : $ ";
                    view_message += message.ToString().Remove(message.ToString().Length - 2, 2);

                    message /= (int)General.Factor_Denominacion;

                    if (View.BanderaRecaudo)
                    {
                        DineroRecibido("CoinBag", message);
                    }
                }
                else
                {
                    view_message += "Evento aún no controlado Cash Acceptance: ";
                    view_message += m;
                }
            }
            else if (id_event == (int)Event.Cash_Payout)
            {
                if (device == (int)Pay_Unit.Hopper1)
                {
                    view_message += "Hopper 1: ";
                    view_message += "Cash Payout - Denomination : $ ";
                    view_message += message.ToString();
                }
                else if (device == (int)Pay_Unit.Hopper2)
                {
                    view_message += "Hopper 2: ";
                    view_message += "Cash Payout - Denomination : $ ";
                    view_message += message.ToString();
                }
                else if (device == (int)Pay_Unit.Hopper3)
                {
                    view_message += "Hopper 3: ";
                    view_message += "Cash Payout - Denomination : $ ";
                    view_message += message.ToString();
                }
                else if (device == (int)Pay_Unit.Hopper4)
                {
                    view_message += "Hopper 4: ";
                    view_message += "Cash Payout - Denomination : $ ";
                    view_message += message.ToString();
                }
                else
                {
                    view_message += "Evento aún no controlado Cash Payout: ";
                    view_message += m;
                }
            }
            else if (id_event == (int)Event.Error)
            {
                if (device == (int)Pay_Unit.Coin_Validator)
                {
                    view_message += "Coin Validator: ";
                    NRI_Error_Messages(message, ref view_message, device);
                }
                else if (device == (int)Pay_Unit.Hopper1)
                {
                    view_message += "Hopper 1: ";
                    NRI_Error_Messages(message, ref view_message, device);
                }
                else if (device == (int)Pay_Unit.Hopper2)
                {
                    view_message += "Hopper 2: ";
                    NRI_Error_Messages(message, ref view_message, device);
                }
                else if (device == (int)Pay_Unit.Hopper3)
                {
                    view_message += "Hopper 3: ";
                    NRI_Error_Messages(message, ref view_message, device);
                }
                else if (device == (int)Pay_Unit.Hopper4)
                {
                    view_message += "Hopper 4: ";
                    NRI_Error_Messages(message, ref view_message, device);
                }
                else
                {
                    view_message += "Evento aún no controlado Error: ";
                    view_message += m;
                }
            }
            else
            {
                view_message += "Evento aún no controlado Undefined: ";
                view_message += m;
            }

            //text_message = view_message;
        }

        void Status_Messages(int device, int message, ref string text_message)
        {
            if (message == (int)Status_Message.Found_Startup)
            {
                text_message += "Found";
            }
            else if (message == (int)Status_Message.Ready)
            {
                text_message += "Ready";
                EstadoMiniHub(device);
            }
            else if (message == (int)Status_Message.Hardware_Problem)
            {
                text_message += "Hardware Problem";
            }
            else if (message == (int)Status_Message.Not_Found_Running)
            {
                text_message += "Not Found";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (message == (int)Status_Message.Tube_Coin_Empty)
            {
                text_message += "Tube Coin Empty";
            }
            else if (message == (int)Status_Message.Not_Found_Startup)
            {
                text_message += "Not Found";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (message == (int)Status_Message.No_Configuration_Found)
            {
                text_message += "No Configuration Found";
            }
            else
            {
                text_message += "Evento aún no controlado Status: " + message.ToString();
            }
        }

        void NRI_Error_Messages(int error_code, ref string text_message, int device)
        {
            if (error_code == (int)NRI_ErrorCode.Communication_Error)
            {
                text_message += "Communication Error";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Coin_Validator_Reset)
            {
                text_message += "Coin Validator Reset";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Sensor_Problem)
            {
                text_message += "Sensor Problem";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Defective_Motor)
            {
                text_message += "Defective Motor";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Coin_Jam_Tube)
            {
                text_message += "Jam In Tube";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Jam_Validator)
            {
                text_message += "Jam In Validator";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Coin_Validator_ROM_Checksum_Error)
            {
                text_message += "Coin Validator ROM Checksum Error";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Defective_Tube_Sensor)
            {
                text_message += "Defective Tube Sensor";
            }
            else if (error_code == (int)NRI_ErrorCode.Payment_Unit_Disabled)
            {
                text_message += "Payment Unit Disabled";
            }
            else if (error_code == (int)NRI_ErrorCode.Validator_Unplugged)
            {
                text_message += "Validator Unplugged";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Coin_Jam)
            {
                text_message += "Coin Jam";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Coin_Sorting_Error)
            {
                text_message += "Coin Sorting Error";
            }
            else if (error_code == (int)NRI_ErrorCode.String_Recognition)
            {
                text_message += "String Recognition";
            }
            else if (error_code == (int)NRI_ErrorCode.Hopper_Motor_Blocked)
            {
                text_message += "Hopper Motor Blocked";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Hopper_Empty)
            {
                text_message += "Hopper Empty";
            }
            else if (error_code == (int)NRI_ErrorCode.Hopper_Optics_Blocked)
            {
                text_message += "Hopper Optics Blocked";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Hopper_Optics_Error)
            {
                text_message += "Hopper Optics Error";
                RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
            }
            else if (error_code == (int)NRI_ErrorCode.Hopper_Payout_Blocked)
            {
                text_message += "Hopper Payout Blocked";
            }
            else if (error_code == (int)NRI_ErrorCode.Tube_Cassette_Removed)
            {
                text_message += "Tube Cassette Removed";
            }
            else if (error_code == (int)NRI_ErrorCode.Sorting_Opened)
            {
                text_message += "Sorting Opened";
            }
            else if (error_code == (int)NRI_ErrorCode.Low_Voltage_Coin_Charger_Detected)
            {
                text_message += "Low Voltaje Coin";
            }
            else if (error_code == (int)NRI_ErrorCode.Rejected)
            {
                View.Cnt_Reinicio = 0;

                text_message += "Rejected";
            }
            else
            {
                text_message += "Evento aún no controlado NRI_Error: " + error_code.ToString();
            }
        }
        void B2B_Troubleshooting_Error_Messages(int error_code, ref string text_message, int device)
        {
            if (error_code == (int)B2B_Troubleshooting_ErrorCode.Chassis_Removed)
            {
                text_message += "Chassis Removed.";
            }
            else
            {
                if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_81)
                {
                    text_message += "Error_Link2_81 : Error on the link with the external EEPROM – wire SDA.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_82)
                {
                    text_message += "Error_Link2_82 : Error on the link with the external EEPROM – wire SCL.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_83)
                {
                    text_message += "Error_Link2_83 : Error on the link with the Cassette #1.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_84)
                {
                    text_message += "Error_Link2_84 : Error on the link with the Cassette #2.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_85)
                {
                    text_message += "Error_Link2_85 : Error on the link with the Cassette #3.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_86)
                {
                    text_message += "Error_Link2_86 : Error on the link with the Dispenser.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_87)
                {
                    text_message += "Error_Link2_87 : Error on the link with the Cassette #1.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_88)
                {
                    text_message += "Error_Link2_88 : Error on the link with the Cassette #2.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_89)
                {
                    text_message += "Error_Link2_89 : Error on the link with the Cassette #3.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_8A)
                {
                    text_message += "Error_Link2_8A : Error on the link with the Dispenser.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_8D)
                {
                    text_message += "Error_Link2_8D : Incorrect CRC of the main program.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_8E)
                {
                    text_message += "Error_Link2_8E : ACK signal is absent during exchange with RTC.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link2_8F)
                {
                    text_message += "Error_Link2_8F : EEPROM exchange error.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_90)
                {
                    text_message += "Error_Transport_90 : Clock Battery Discharged.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_91)
                {
                    text_message += "Error_Transport_91 : Transport Motor Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_92)
                {
                    text_message += "Error_Transport_92 : It is impossible to support transport motor speed 300mm/Sec.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_93)
                {
                    text_message += "Error_Transport_93 : One or some sensors of chassis upper tract overlapped.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_94)
                {
                    text_message += "Error_Transport_94 : One or some sensor of chassis lower tract overlapped.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_95)
                {
                    text_message += "Error_Transport_95 : All cassettes are absent.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Transport_96)
                {
                    text_message += "Error_Transport_96 : Dispenser is absent.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Switch_97)
                {
                    text_message += "Error_Transport_97 : Switch initialization error – step counter signal is absent.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Switch_98)
                {
                    text_message += "Error_Transport_98 : Switch initialization error – initial marker sensor signal is absent.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Switch_99)
                {
                    text_message += "Error_Transport_99 : Switch setting error – step counter signal is absent.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A1)
                {
                    text_message += "Error_Cassette1_A1 : Entry Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A2)
                {
                    text_message += "Error_Cassette1_A2 : Overflow Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A3)
                {
                    text_message += "Error_Cassette1_A3 : Tape Begin/End Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A4)
                {
                    text_message += "Error_Cassette1_A4 : Tape Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A5)
                {
                    text_message += "Error_Cassette1_A5 : It is impossible to support speed 300mm/Sec.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A6)
                {
                    text_message += "Error_Cassette1_A6 : Bill Jam Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A7)
                {
                    text_message += "Error_Cassette1_A7 : EEProm Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A8)
                {
                    text_message += "Error_Cassette1_A8 : Time out of bill entrance to entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_A9)
                {
                    text_message += "Error_Cassette1_A9 : Time out of bill exit from entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_AA)
                {
                    text_message += "Error_Cassette1_AA : Time out of successful completion bill accepting is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_AB)
                {
                    text_message += "Error_Cassette1_AB : Time out of successful completion bill giving out is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_AC)
                {
                    text_message += "Error_Cassette1_AC : Bill Jam Sensor is Cut In.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette1_AD)
                {
                    text_message += "Error_Cassette1_AD : Requested number of bills is not given out.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_AE)
                {
                    text_message += "Error_Cassette2_AE : Entry Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_AF)
                {
                    text_message += "Error_Cassette2_AF : Overflow Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B0)
                {
                    text_message += "Error_Cassette2_B0 : Tape Begin/End Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B1)
                {
                    text_message += "Error_Cassette2_B1 : Tape Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B2)
                {
                    text_message += "Error_Cassette2_B2 : It is impossible to support speed 300mm/Sec.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B3)
                {
                    text_message += "Error_Cassette2_B3 : Bill Jam Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B4)
                {
                    text_message += "Error_Cassette2_B4 : EEProm Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B5)
                {
                    text_message += "Error_Cassette2_B5 : Time out of bill entrance to entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B6)
                {
                    text_message += "Error_Cassette2_B6 : Time out of bill exit from entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B7)
                {
                    text_message += "Error_Cassette2_B7 : Time out of successful completion bill accepting is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B8)
                {
                    text_message += "Error_Cassette2_B8 : Time out of successful completion bill giving out is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_B9)
                {
                    text_message += "Error_Cassette2_B9 : Bill Jam Sensor is Cut In.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette2_BA)
                {
                    text_message += "Error_Cassette2_BA : Requested number of bills is not given out.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_BB)
                {
                    text_message += "Error_Cassette3_BB : Entry Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_BC)
                {
                    text_message += "Error_Cassette3_BC : Overflow Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_BD)
                {
                    text_message += "Error_Cassette3_BD : Tape Begin/End Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_BE)
                {
                    text_message += "Error_Cassette3_BE : Tape Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_BF)
                {
                    text_message += "Error_Cassette3_BF : It is impossible to support speed 300mm/Sec.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C0)
                {
                    text_message += "Error_Cassette3_C0 : Bill Jam Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C1)
                {
                    text_message += "Error_Cassette3_C1 : EEProm Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C2)
                {
                    text_message += "Error_Cassette3_C2 : Time out of bill entrance to entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C3)
                {
                    text_message += "Error_Cassette3_C3 : Time out of bill exit from entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C4)
                {
                    text_message += "Error_Cassette3_C4 : Time out of successful completion bill accepting is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C5)
                {
                    text_message += "Error_Cassette3_C5 : Time out of successful completion bill giving out is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C6)
                {
                    text_message += "Error_Cassette3_C6 : Bill Jam Sensor is Cut In.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Cassette3_C7)
                {
                    text_message += "Error_Cassette2_C7 : Requested number of bills is not given out.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_C8)
                {
                    text_message += "Error_Dispenser_C8 : Entry Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_C9)
                {
                    text_message += "Error_Dispenser_C9 : Drum Initial Location Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_CB)
                {
                    text_message += "Error_Dispenser_CB : Lower Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_CE)
                {
                    text_message += "Error_Dispenser_CE : Exit Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_CF)
                {
                    text_message += "Error_Dispenser_CF : Drum Motor Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D0)
                {
                    text_message += "Error_Dispenser_D0 : Shutter Sensor Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D1)
                {
                    text_message += "Error_Dispenser_D1 : Shutter Motor Obturator Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D2)
                {
                    text_message += "Error_Dispenser_D2 : Keyboard Failed.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D3)
                {
                    text_message += "Error_Dispenser_D3 : It is impossible to support drum speed 300mm/Sec.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D4)
                {
                    text_message += "Error_Dispenser_D4 : Time out of bill entrance to entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D5)
                {
                    text_message += "Error_Dispenser_D5 : Time out occurred with bills waiting for removal by customers.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D6)
                {
                    text_message += "Error_Dispenser_D6 : Time out of bill entrance to lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D7)
                {
                    text_message += "Error_Dispenser_D7 : Time out of bill entrance to exit sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D8)
                {
                    text_message += "Error_Dispenser_D8 : Time out of bill exit from lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_D9)
                {
                    text_message += "Error_Dispenser_D9 : Time out of successful bill accepting is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Dispenser_DA)
                {
                    text_message += "Error_Dispenser_DA : Time out of bill exit from entry sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_DB)
                {
                    text_message += "Jam_Transport_DB : Time out of bill entrance to upper chassis tract lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_DC)
                {
                    text_message += "Jam_Transport_DC : Time out of bill exit from upper chassis tract lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_DD)
                {
                    text_message += "Jam_Transport_DD : Time out of bill entrance to upper chassis tract middle sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_DE)
                {
                    text_message += "Jam_Transport_DE : Time out of bill exit from upper chassis tract middle sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_DF)
                {
                    text_message += "Jam_Transport_DF : Time out of bill entrance to upper chassis tract upper sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E0)
                {
                    text_message += "Jam_Transport_E0 : Time out of bill exit from upper chassis tract upper sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E1)
                {
                    text_message += "Jam_Transport_E1 : Time out of bill entrance to lower chassis tract lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E2)
                {
                    text_message += "Jam_Transport_E2 : Time out of bill exit from lower chassis tract lower sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E3)
                {
                    text_message += "Jam_Transport_E3 : Time out of bill entrance to lower chassis tract middle sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E4)
                {
                    text_message += "Jam_Transport_E4 : Time out of bill exit from lower chassis tract middle sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E5)
                {
                    text_message += "Jam_Transport_E5 : Time out of bill entrance to lower chassis tract upper sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Jam_Transport_E6)
                {
                    text_message += "Jam_Transport_E6 : Time out of bill exit from lower chassis tract upper sensor is exceeded.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_E7)
                {
                    text_message += "Error_Link1_E7 : I2C-Bus Error – Start Condition.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_E8)
                {
                    text_message += "Error_Link1_E8 : I2C-Bus Error – Writing Initialization.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_E9)
                {
                    text_message += "Error_Link1_E9 : I2C-Bus Error – Reading Initialization.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_EA)
                {
                    text_message += "Error_Link1_EA : I2C-Bus Error – Byte Writing.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_EB)
                {
                    text_message += "Error_Link1_EB : I2C-Bus Error – Byte Reading.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_Link1_EC)
                {
                    text_message += "Error_Link1_EC : CRC Error.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F0)
                {
                    text_message += "Error_HV_F0 : Error of the Bill Stacking.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F1)
                {
                    text_message += "Error_HV_F1 : Optical sensors don't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F2)
                {
                    text_message += "Error_HV_F2 : Magnetic sensors don't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F3)
                {
                    text_message += "Error_HV_F3 : Capacitive sensors don't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F4)
                {
                    text_message += "Error_HV_F4 : Stacker motor doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F5)
                {
                    text_message += "Error_HV_F5 : Interlink with sensor boxes don't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F6)
                {
                    text_message += "Error_HV_F6 : Jam in the Validator Head.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F7)
                {
                    text_message += "Error_HV_F7 : Transport motor doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F8)
                {
                    text_message += "Error_HV_F8 : Alignment motor doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_F9)
                {
                    text_message += "Error_HV_F9 : Unsupported version of the chassis firmware or bootprogram of the chassis doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_FA)
                {
                    text_message += "Error_HV_FA : Boot-program of the dispenser doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_FB)
                {
                    text_message += "Error_HV_FB : Boot-program of the Cassette # 1 doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_FC)
                {
                    text_message += "Error_HV_FB : Boot-program of the Cassette # 2 doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else if (error_code == (int)B2B_Troubleshooting_ErrorCode.Error_HV_FD)
                {
                    text_message += "Error_HV_FB : Boot-program of the Cassette # 3 doesn't work.";
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                else
                {
                    text_message += "Evento aún no controlado B2B_TS_Error: " + error_code.ToString();
                    RegistrarAlarma(TipoAlarma.ErrorPaymentDevice, ObtenerDispositivo(device), text_message, 1);
                }
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }
        }
        public void EstadoMiniHub(int device)
        {
            switch (device)
            {
                case (int)Pay_Unit.Hopper1:
                    View.Hopper1Ready = true;
                    break;

                case (int)Pay_Unit.Hopper2:
                    View.Hopper2Ready = true;
                    break;

                case (int)Pay_Unit.Hopper3:
                    View.Hopper3Ready = true;
                    break;

                case (int)Pay_Unit.Hopper4:
                    View.Hopper4Ready = true;
                    break;
            }
        }
        public void AdministrarDescargaMonedas(Pay_Unit oPay_Unit)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = Model.AdministrarDescargaMonedas(oPaymentDevice, oPay_Unit, 1000000);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                int cantidad = (int)oResultadoOperacion.EntidadDatos;
                View.General_EventsBill = "Presenter-AdministrarDescargaMonedas: " + oResultadoOperacion.Mensaje;
                View.General_EventsBill = "Presenter-AdministrarDescargaMonedas -> Administrar Descarga Monedas OK - " + oPay_Unit.ToString() + " - " + "Qty: " + cantidad;
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_EventsBill = oResultadoOperacion.Mensaje;
                View.General_EventsBill = "Presenter-AdministrarDescargaMonedas -> Sistema Suspendido - Administrar Descarga Monedas Error - " + oPay_Unit.ToString();
                View.SetearPantalla(Pantalla.SistemaSuspendido);
            }
        }
        
        #endregion

        #region Reader

        string CODE = string.Empty;
        string DATA = string.Empty;
        public bool LeerTarjeta()
        {
            bool OK = false;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string SKey = ObtenerValorParametro(Parametros.KeyCard);

            try
            {
                RspLeer = oReader.LeerTarjeta(EGlobalT.Device.SmartCard.TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, SKey, true);
                if (RspLeer.TarjetaLeida)
                {
                    oResultadoOperacion.EntidadDatos = RspLeer.Tarjeta;
                    spTarjeta = (SMARTCARD_PARKING_V1)oResultadoOperacion.EntidadDatos;

                    View.Tarjeta.CodeCard = spTarjeta.CodeCard;
                    View.Tarjeta.ActiveCycle = spTarjeta.ActiveCycle;
                    View.Tarjeta.DateTimeEntrance = spTarjeta.DateTimeEntrance;
                    View.Tarjeta.TypeVehicle = spTarjeta.TypeVehicle.Value;
                    View.Tarjeta.EntranceModule = spTarjeta.EntranceModule;
                    bool? repo = spTarjeta.Replacement;

                    View.Tarjeta.Replacement = repo;

                    //View.Tarjeta.CodeAgreement1 = spTarjeta.CodeAgreement1;
                    RspCheck = oReader.CheckPasswordTarjeta(SKey, 1);
                    RspLect = oReader.LeerTarjetaxSectorBloque(SKey, 1, 2, true);
                    View.Tarjeta.CodeAgreement1 = RspLect.Datos[1];

                    View.General_EventsBill = "Presenter-LeerTarjeta CONVENIO = " + View.Tarjeta.CodeAgreement1;

                    OK = true;

                    oResultadoOperacion = Model.DatosConvenio(Convert.ToInt64(View.Tarjeta.CodeAgreement1));

                    View.General_EventsBill = "Presenter-DatosConvenio";

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        View.General_EventsBill = "Presenter-DatosConvenio NombreConvenio " + oResultadoOperacion.EntidadDatos.ToString();

                        View.NombreConvenio = oResultadoOperacion.EntidadDatos.ToString();
                    }
                    else
                    {
                        View.General_EventsBill = "Presenter-Tarjeta sin convenio";

                    }

                }
                else
                {
                    if (RspLeer.Des_ErrorLECTOR != "No hay tarjeta...")
                    {
                        if (RspLeer.Des_ErrorLECTOR == "Datos de lectura no válidos...")
                        {
                            View.General_EventsBill = "Presenter-LeerTarjeta ERROR" + RspLeer.Des_ErrorLECTOR;
                            View.SetearPantalla(Pantalla.TarjetaInvalida);
                            oReader.ExpulsarTarjeta();
                        }
                        else
                        {
                            View.General_EventsBill = "Presenter-LeerTarjeta ERROR" + RspLeer.Des_ErrorLECTOR;
                            View.SetearPantalla(Pantalla.SistemaSuspendido);
                            oReader.ExpulsarTarjeta();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                View.General_EventsBill = ex.ToString();
                oReader.ExpulsarTarjeta();
            }


            return OK;
        }
        public bool ConectarLector()
        {
            bool OK = false;

            try
            {
                string COMLEC = Globales.sPuertoCRC;


                RspConexion = oReader.Conectar(COMLEC, true, true);
                if (RspConexion.Conectado)
                {
                    OK = true;
                    View.General_EventsBill = "Presenter-ConectarLector OK" + RspConexion.Des_ErrorLECTOR;
                }
                else
                {
                    View.General_EventsBill = "Presenter-ConectarLector ERROR" + RspConexion.Des_ErrorLECTOR;
                }
            }
            catch (Exception ex)
            {
                //View.General_EventsBill = "Presenter-LeerTarjeta " + ex.ToString();
            }

            return OK;
        }
        public bool DesConectarLector()
        {
            bool OK = false;

            try
            {
               // oReader.Desconectar();
                oCRT288Device.DesconectarLector();
            }
            catch (Exception ex)
            {
                //View.General_EventsBill = "Presenter-LeerTarjeta " + ex.ToString();
            }

            return OK;
        }
        public string HEX2ASCII(string hex)
        {

            string res = String.Empty;

            for (int a = 0; a < hex.Length; a = a + 2)
            {

                string Char2Convert = hex.Substring(a, 2);

                int n = Convert.ToInt32(Char2Convert, 16);

                char c = (char)n;

                res += c.ToString();

            }

            return res;

        }
        public bool EscribirTarjeta()
        {
            bool OK = false;

            try
            {
                #region OLD
                //ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                //string SKey = ObtenerValorParametro(Parametros.KeyCard);

                //SMARTCARD_PARKING_V1 sTarjeta = new SMARTCARD_PARKING_V1();
                //RspId = oReader.ObtenerIDTarjeta();


                //if (View.Tarjeta.Replacement == true)
                //{
                //    sTarjeta.Replacement = false;
                //    sTarjeta.ActiveCycle = true;
                //    sTarjeta.CodeCard = RspId.CodigoTarjeta;


                //    RspEscribir = oReader.EscribirTarjeta(sTarjeta, SKey, true, true);

                //    if (RspEscribir.TarjetaEscrita)
                //    {
                //        OK = true;
                //    }
                //    else
                //    {
                //        View.General_EventsBill = "Presenter-EscribirTarjeta ERROR" + RspEscribir.Des_ErrorLECTOR;
                //        View.SetearPantalla(Pantalla.SistemaSuspendido);
                //    }
                //}
                //else 
                //{
                //    OK = true;
                //}
                #endregion

                if (View.Tarjeta.Replacement == true)
                {
                    oCRT288Device.EscribirTarjeta(DATA);

                    if (View.Writeok)
                    {
                        OK = true;
                    }
                }
                else
                {
                    OK = true;
                }

            }
            catch (Exception ex)
            {
                View.General_EventsBill = "Presenter-EscribirTarjeta ERROR" + ex.ToString();
                ExpulsarTarjeta();
            }

            return OK;
        }
        public void ExpulsarTarjeta()
        {
            //oReader.ExpulsarTarjeta();
            oCRT288Device.ExpulsarTI();
        }

        #region EVENTOS
        void oCRT288Device_DeviceMessage(object sender, EventArgs e)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            var i = (EventArgsCRTDevice)e;

            switch (i.result)
            {
                #region ConexionExitosa
                case StatesLector.Conexion_Exitosa:
                    View.CRTReady = true;
                    //View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region ErrorConexion
                case StatesLector.Error_Conexion:
                    View.CRTReady = false;
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region Desconexion
                case StatesLector.Desconexion_Exitosa:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region ObtenerIdTarjeta
                case StatesLector.ObtenerIdTarjeta:
                    CODE = i.resultString.EntidadDatos.ToString();
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region LecturaOK
                case StatesLector.LecturaOK:
                    oResultadoOperacion = i.resultString;
                    View.Tarjeta = (Tarjeta)oResultadoOperacion.EntidadDatos;
                    View.General_EventsBill = i.result.ToString();
                    DATA = i.resultString.Mensaje;
                    View.Readok = true;
                    break;
                #endregion

                #region ErrorLectura
                case StatesLector.LecturaError:
                    View.General_EventsBill = i.result.ToString();
                    View.Readok = false;
                    break;
                #endregion

                #region CheckPassError
                case StatesLector.CheckPassError:
                    View.General_EventsBill = i.result.ToString();
                    View.Presentacion = Pantalla.TarjetaInvalida;
                    Expulsar();
                    break;
                #endregion

                #region ExpulsarTI
                case StatesLector.ExpulsarOK:
                    View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region CardIn
                case StatesLector.CardIn:
                    View.Status = true;
                    break;
                #endregion

                #region NoCard
                case StatesLector.NoCard:
                    View.Status = false;
                    break;
                #endregion

                #region ErrorEscribir
                case StatesLector.ErrorEscribir:
                    View.Writeok = false;
                    break;
                #endregion

                #region EscribirOK
                case StatesLector.EscribirOK:
                    View.Writeok = false;
                    break;
                #endregion

            }
        }
        #endregion

        #region FUNCIONES
        public void ConectarCRT()
        {
            oCRT288Device.ConectarLector(Globales.sPuertoCRC);
        }
        public void DesconectarCRT()
        {
            oCRT288Device.DesconectarLector();
        }
        public void ObtenerIdTarjeta()
        {
            oCRT288Device.ObtenerIdTarjeta();
        }
        public void CheckPass()
        {
            string SKey = ObtenerValorParametro(Parametros.KeyCard);

            oCRT288Device.CheckPass(SKey, 1);
        }
        public bool LeerTarjetaCRT()
        {
            bool ok = false;

            oCRT288Device.StatusTI();

            if (View.Status)
            {
                ObtenerIdTarjeta();
                string SKey = ObtenerValorParametro(Parametros.KeyCard);
                oCRT288Device.leerTarjeta(SKey, CODE);

                if (View.Readok)
                {
                    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                    View.General_EventsBill = "Presenter-LeerTarjeta CONVENIO = " + View.Tarjeta.CodeAgreement1;

                    oResultadoOperacion = Model.DatosConvenio(Convert.ToInt64(View.Tarjeta.CodeAgreement1));

                    View.General_EventsBill = "Presenter-DatosConvenio";

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        View.General_EventsBill = "Presenter-DatosConvenio NombreConvenio " + oResultadoOperacion.EntidadDatos.ToString();

                        View.NombreConvenio = oResultadoOperacion.EntidadDatos.ToString();
                    }
                    else
                    {
                        View.General_EventsBill = "Presenter-Tarjeta sin convenio";

                    }

                    ok = true;
                }
            }

            return ok;
        }
        public void Expulsar()
        {
            oCRT288Device.ExpulsarTI();
            //oReader.ExpulsarTarjeta();
        }
        #endregion


        #endregion

        #region Datafono Credibanco
       
        #region EVENTOS
        void oDatafonoDevice_DeviceMessage(object sender, EventArgs e)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            var i = (EventArgsDatafonoDevice)e;

            switch (i.result)
            {
                #region ConexionExitosa
                case StatesDatafono.Conexion_Exitosa:
                    View.DatafonoReady = true;
                    //View.General_EventsBill = i.result.ToString();
                    break;
                #endregion

                #region ErrorConexion
                case StatesDatafono.Error_Conexion:
                    View.DatafonoReady = false;
                    View.General_EventsBill = i.result.ToString();                    
                    break;
                #endregion

                #region IniciarTransaccion
                case StatesDatafono.Inicializarok:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.InserteTarjetaDatfono);
                    PagoNormalDatafono();
                    break;
                #endregion

                #region ErrorIniciar
                case StatesDatafono.ErrorIniciar:
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    View.General_EventsBill = i.result.ToString();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    ExpulsarTarjeta();
                    break;
                #endregion

                #region TarjetaLeida
                case StatesDatafono.TarjetaLeida:
                    if (i.resultString.Mensaje.ToString() == "Ingrese Cuotas")
                    {
                        View.SetearPantalla(Pantalla.DigiteCuotas);
                    }
                    else
                    {
                        View.SetearPantalla(Pantalla.TipoCuenta);
                        View.General_EventsBill = i.result.ToString();
                    }
                    break;
                #endregion

                #region ErrorConexion
                case StatesDatafono.ErrorLeerTarjeta:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    ExpulsarTarjeta();
                    break;
                #endregion

                #region ProcesoAhorros
                case StatesDatafono.ProcesoAhorros:
                    View.SetearPantalla(Pantalla.DigitePIN);
                    View.General_EventsBill = i.result.ToString();                    
                    RespuestaAhorros();
                    break;
                #endregion

                #region ErrorRespuestaAhorros
                case StatesDatafono.ErrorRespuestaAhorros:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    break;
                #endregion

                #region RespuestaAhorros
                case StatesDatafono.RespuestaAhorros:
                    View.General_EventsBill = i.result.ToString();
                    View.General_EventsBill = i.resultString.EntidadDatos.ToString(); 
                    if (i.resultString.EntidadDatos == "DATAFONO NO RESPONDE")
                    {
                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                        View.SetearPantalla(Pantalla.ConsultaFallida);
                        ExpulsarTarjeta();
                    }
                    else 
                    {
                        TipoCuenta = string.Empty;
                        string[] ResultEnd = i.resultString.EntidadDatos.ToString().Split(',');

                        if (ResultEnd[2].ToString() == "00")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono Aprobado";

                            View.Operacion.Pago.NoAutorizacion = ResultEnd[3].ToString();
                            View.Operacion.Pago.Franquicia = ResultEnd[13].ToString();
                            View.Operacion.Pago.CodigoBarras = ResultEnd[8].ToString();
                            View.Operacion.Pago.Referencia = string.Empty;
                            View.Operacion.Pago.NoTarjeta = ResultEnd[16].ToString();
                            View.Operacion.Pago.Factura = ResultEnd[7].ToString();

                            if(ResultEnd[14].ToString() == "DB")
                            {
                               View.Operacion.Pago.TipoPago = TipoPago.Ahorros;
                            }
                            else if(ResultEnd[14].ToString() == "EL")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.Corriente;
                            }
                            else if(ResultEnd[14].ToString() == "CR")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.Credito;
                            }
                            else if(ResultEnd[14].ToString() == "RO")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.CreditoRotativo;
                            }

                            TipoCuenta = View.Operacion.Pago.TipoPago.ToString();
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);

                            View.Operacion.Pago.TipoPago = TipoPago.Efectivo;

                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);


                            View.SetearPantalla(Pantalla.ImprimirFactura);
                        }
                        else if (ResultEnd[3].ToString() == "CAPTURE PIN EN DATAFONO]69")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono CAPTURE PIN EN DATAFONO";
                        }
                        else if (ResultEnd[3].ToString() == "LLAME A SU BANCO;;]4E")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono LLAME A SU BANCO";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                        }
                        else if (ResultEnd[2].ToString().Substring(0,2) == "02")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono Fondos Insuficientes – Clave Inválida – Tarjeta Vencida";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                        else if (ResultEnd[2].ToString().Substring(0, 2) == "05")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono (Error Apertura Puerto Serial – Problemas Comunicación";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                        else if (ResultEnd[3].ToString() == "PIN Incorrecto;;]60")
                        {
                            if (View.IntentoPin == 0)
                            {
                                View.General_EventsBill = "Presenter Respuesta Datafono PIN Incorrecto intento 1";
                                View.SetearPantalla(Pantalla.PinInvalidoTarjeta);
                                View.IntentoPin = 1;
                            }
                            else 
                            {
                                View.General_EventsBill = "Presenter Respuesta Datafono PIN Incorrecto intento 2";
                                ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                                View.SetearPantalla(Pantalla.ConsultaFallida);
                                ExpulsarTarjeta();
                            }

                        }
                        else
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono (Error Apertura Puerto Serial – Problemas Comunicación";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                    }
                    break;
                #endregion

                #region ErrorRespuestaAhorros
                case StatesDatafono.ErrorRespuestaFinal:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    break;
                #endregion

                #region ProcesoCredito
                case StatesDatafono.ProcesoCredito:
                    View.General_EventsBill = i.result.ToString();

                    if (i.resultString.Mensaje == "Ingrese Cuotas")
                    {
                        View.SetearPantalla(Pantalla.DigiteCuotas);
                    }
                    else 
                    {
                        View.SetearPantalla(Pantalla.DigiteCredito);
                    }
                    break;
                #endregion

                #region ErrorRespuestaCredito
                case StatesDatafono.ErrorRespuestaCredito:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    break;
                #endregion

                #region IngresoDigitos
                case StatesDatafono.IngresoDigitos:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.DigiteCuotas);
                    break;
                #endregion

                #region ErrorIngresoDigitos
                case StatesDatafono.ErrorIngresoDigitos:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    break;
                #endregion

                #region IngresoCuotas
                case StatesDatafono.IngresoCuotas:
                    View.General_EventsBill = i.result.ToString();
                    if (i.resultString.EntidadDatos == "DATAFONO NO RESPONDE")
                    {
                        ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                        View.SetearPantalla(Pantalla.ConsultaFallida);
                        ExpulsarTarjeta();
                    }
                    else 
                    {
                        TipoCuenta = string.Empty;
                        string[] ResultEnd = i.resultString.EntidadDatos.ToString().Split(',',';');

                        if (ResultEnd[2].ToString().Substring(0, 2) == "02")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono Fondos Insuficientes – Clave Inválida – Tarjeta Vencida";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                        else if (ResultEnd[2].ToString().Substring(0,2) == "00")
                        {
                            View.General_EventsBill = "Presenter Respuesta ";

                            View.Operacion.Pago.NoAutorizacion = ResultEnd[3].ToString();
                            View.Operacion.Pago.Franquicia = ResultEnd[13].ToString();
                            View.Operacion.Pago.CodigoBarras = ResultEnd[8].ToString();
                            View.Operacion.Pago.Referencia = string.Empty;
                            View.Operacion.Pago.NoTarjeta = ResultEnd[16].ToString();
                            View.Operacion.Pago.Factura = ResultEnd[7].ToString();

                            if (ResultEnd[14].ToString() == "DB")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.Ahorros;
                            }
                            else if (ResultEnd[14].ToString() == "EL")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.Corriente;
                            }
                            else if (ResultEnd[14].ToString() == "CR")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.Credito;
                            }
                            else if (ResultEnd[14].ToString() == "RO")
                            {
                                View.Operacion.Pago.TipoPago = TipoPago.CreditoRotativo;
                            }

                            TipoCuenta = View.Operacion.Pago.TipoPago.ToString();
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);

                            View.Operacion.Pago.TipoPago = TipoPago.Efectivo;

                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);


                            View.SetearPantalla(Pantalla.ImprimirFactura);


                           
                        }
                        else if (ResultEnd[2].ToString().Substring(0, 2) == "05")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono (Error Apertura Puerto Serial – Problemas Comunicación";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                        else if (ResultEnd[2].ToString() == "ERROR")
                        {
                            View.General_EventsBill = "Presenter Respuesta Datafono FONDOS INSUFICIENTES";
                            ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                            View.SetearPantalla(Pantalla.ConsultaFallida);
                            ExpulsarTarjeta();
                        }
                        
                    }
                    //View.SetearPantalla(Pantalla.ImprimirFactura);
                    break;
                #endregion

                #region ErrorIngresoCuotas
                case StatesDatafono.ErrorIngresoCuotas:
                    View.General_EventsBill = i.result.ToString();
                    View.SetearPantalla(Pantalla.ConsultaFallida);
                    break;
                #endregion

                #region CancelacionTipoCuenta
                case StatesDatafono.CancelacionTipoCuenta:
                    View.General_EventsBill = i.result.ToString();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    Expulsar();
                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                    break;
                #endregion

                #region ErrorCancelacionTipoCuenta
                case StatesDatafono.ErrorCancelacionTipoCuenta:
                    View.General_EventsBill = i.result.ToString();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    Expulsar();
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    break;
                #endregion

                #region CancelacionCuotas
                case StatesDatafono.CancelacionCuotas:
                    View.General_EventsBill = i.result.ToString();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    Expulsar();
                    View.SetearPantalla(Pantalla.PublicidadPrincipal);
                    break;
                #endregion

                #region ErrorCancelacionCuotas
                case StatesDatafono.ErrorCancelacionCuotas:
                    View.General_EventsBill = i.result.ToString();
                    ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);
                    Expulsar();
                    View.SetearPantalla(Pantalla.SistemaSuspendido);
                    break;
                #endregion
            }
        }
        #endregion

        #region FUNCIONES
        public void ConectarDatafono()
        {
            oDatafonoDevice.ConectarDatafono(Globales.sPuertoDTF);
        }
        public void DesconectarDatafono()
        {
            oDatafonoDevice.ConectarDatafono(Globales.sPuertoDTF);
        }
        public void PagoNormalDatafono()
        {
            oDatafonoDevice.InserteTarjeta();
        }
        public void InciarDispositivoDatafono(double ValorPago, double Iva, string IdModulo, string IdTransaccion)
        {
            string Terminal = Globales.sTerminal;
            oDatafonoDevice.IniciarTransaccion(ValorPago,Iva,IdModulo,IdTransaccion,Terminal);
        }
        public void SeleccionCredito()
        {
            oDatafonoDevice.ProcesoCredito();
        }
        public void SeleccionAhorros()
        {
            oDatafonoDevice.ProcesoAhorros();
        }
        public void SeleccionCorriente()
        {
            oDatafonoDevice.ProcesoCorriente();
        }
        public void RespuestaAhorros()
        {
            oDatafonoDevice.RespuestaAhorros();
        }
        public void NumeroTarjeta(string Num)
        {
            oDatafonoDevice.IngresoDigitos(Num);
        }
        public void IngresoCuotas(string Cuotas)
        {
            oDatafonoDevice.IngresoCuotas(Cuotas);
        }
        public void CancelacionTipoCuenta()
        {
            oDatafonoDevice.CanelarTipoCuenta();
        }
        public void CancelacionCuotas()
        {
            oDatafonoDevice.CanelarCuotas();
        }
        #endregion

        #endregion

        #region Datafono
        public bool InicializarDatafono()
        {
            bool ok = false;
            try
            {

                string PathArchivoRespuesta = "C:\\Datafono\\";
                string NombreArchivoRespuesta = "RESPUESTA.TXT";
                string PathArchivoSolicitud = "C:\\Datafono\\SOLICITUD.TXT";
                string ArchivoCajero = "C:\\Datafono\\Cajas.exe";

                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
                oResultadoOperacion = Model.IniciarDatafono(PathArchivoRespuesta, NombreArchivoRespuesta, PathArchivoSolicitud, ArchivoCajero);
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "(Presenter IniciarDatafono) Respuesta exitosa a Iniciar Datafono";
                    ok = true;
                }
                else
                {
                    View.General_EventsBill = "(Presenter IniciarDatafono) Respuesta error a Iniciar Datafono: " + oResultadoOperacion.Mensaje;
                    ok = false;
                }
            }
            catch (Exception ex)
            {
                ok = false;
            }
            return ok;
        }
        private void EventosDatafono(string t)
        {
            View.SetearPantalla(Pantalla.Procesando);

            string[] respuestaDatafono = t.Split(',');

            if (respuestaDatafono[0] == "00")
            {
                //if (RegistrarPagoDatafono(respuestaDatafono[0] + "," + respuestaDatafono[1] + "," + respuestaDatafono[2] + "," + respuestaDatafono[3] + "," + respuestaDatafono[4] + "," + respuestaDatafono[5] + "," + respuestaDatafono[6] + "," + respuestaDatafono[7] + "," + respuestaDatafono[8] + "," + respuestaDatafono[9]))
                //{
                //    View.Datafono = true;
                //    View.SetearPantalla(Pantalla.ImprimirFactura);

                //    try
                //    {
                //        File.Delete(Path.Combine("C:\\Datafono\\RESPUESTA.TXT"));
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}
                //else 
                //{
                //    View.SetearPantalla(Pantalla.TransaccionCancelada);
                //}
            }
            else
            {
                View.General_EventsBill = "Presenter Respuesta Datafono: " + respuestaDatafono[1];
                View.SetearPantalla(Pantalla.TransaccionCancelada);
            }

        }
        public bool PagarDatafono()
        {
            bool ok = false;
            try
            {
                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                oResultadoOperacion = Model.PagarDatafono(0, Convert.ToInt64(View.lstConsultarValorResult[0].valorAPagar),0, "0", 0, 0, "58");
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                ok = false;
            }
            return ok;
        }
        #endregion

        #region Service
        public bool ConsultaValor()
        {
            bool ok = false;
            int Carril = 0;
            try
            {

                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                string SecuenciaTransaccion = string.Empty;
                int TipoVehiculo = 0;
                string Codigo = View.Tarjeta.CodeAgreement1.ToString();

                bool REPO = false;

                if (View.Tarjeta.Replacement == true)
                {
                    REPO = true;
                }

                if (REPO)
                {
                    //obtener idtransaccion con id tarjeta

                    DateTime Entrada = Convert.ToDateTime(View.Tarjeta.DateTimeEntrance);
                    string año = Entrada.Year.ToString();
                    string MES = Entrada.Month.ToString();
                    string DIA = Entrada.Day.ToString();
                    string HORA = Entrada.Hour.ToString();
                    string MINU = Entrada.Minute.ToString();
                    string SEG = Entrada.Second.ToString();

                    //string IdTransaccion = Entrada.ToString("yyyyddMMHHmmss");
                    string IdTransaccion = año + MES.PadLeft(2, '0') + DIA.PadLeft(2, '0') + HORA.PadLeft(2, '0') + MINU.PadLeft(2, '0') + SEG.PadLeft(2, '0');

                    Carril = 0;

                    if (View.Tarjeta.EntranceModule.Length > 5)
                    {
                        string temp = View.Tarjeta.EntranceModule.Substring(4, 2);
                        Carril = Convert.ToInt32(temp);
                    }
                    else
                    {
                        string Modulo = View.Tarjeta.EntranceModule;
                        if (Modulo == "ADM01")
                        {
                            Carril = 30;
                        }
                    }

                    SecuenciaTransaccion = IdTransaccion + Carril + Globales.iCodigoEstacionamiento;

                    //SecuenciaTransaccion = "2020031608130311";
                    if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                    {
                        TipoVehiculo = 1;
                    }
                    else
                    {
                        TipoVehiculo = 2;
                    }
                    if (Codigo == string.Empty)
                    {
                        Codigo = "0";
                    }


                }
                else
                {
                    DateTime Entrada = Convert.ToDateTime(View.Tarjeta.DateTimeEntrance);
                    string año = Entrada.Year.ToString();
                    string MES = Entrada.Month.ToString();
                    string DIA = Entrada.Day.ToString();
                    string HORA = Entrada.Hour.ToString();
                    string MINU = Entrada.Minute.ToString();
                    string SEG = Entrada.Second.ToString();

                    //string IdTransaccion = Entrada.ToString("yyyyddMMHHmmss");
                    string IdTransaccion = año + MES.PadLeft(2, '0') + DIA.PadLeft(2, '0') + HORA.PadLeft(2, '0') + MINU.PadLeft(2, '0') + SEG.PadLeft(2, '0');

                    

                    if (View.Tarjeta.EntranceModule.Trim().Length > 5)
                    {
                        string temp = View.Tarjeta.EntranceModule.Substring(4, 2).Trim();
                        Carril = Convert.ToInt32(temp);
                    }
                    else
                    {
                        string Modulo = View.Tarjeta.EntranceModule.Trim();
                        if (Modulo == "ADM01")
                        {
                            Carril = 30;
                        }
                    }

                    SecuenciaTransaccion = IdTransaccion + Carril + Globales.iCodigoEstacionamiento;
                    View.IdTransaccion = SecuenciaTransaccion;

                    //SecuenciaTransaccion = "2022091314400011";
                    if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                    {
                        TipoVehiculo = 1;
                    }
                    else
                    {
                        TipoVehiculo = 2;
                    }
                    if (Codigo == string.Empty)
                    {
                        Codigo = "0";
                    }

                }

                //if vehiculo es mensual enviar idtarjeta en secuencia

                string SecuenciaTransaccionFinal = string.Empty;


                oResultadoOperacion = Model.ValidarTransaccion(View.Tarjeta.CodeCard, Convert.ToDateTime(View.Tarjeta.DateTimeEntrance));
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    SecuenciaTransaccionFinal = oResultadoOperacion.EntidadDatos.ToString();

                    if (SecuenciaTransaccionFinal == string.Empty)
                    {
                        //crea transaccion en bd

                        Transaccion oTransaccion = new Transaccion();

                        oTransaccion.IdTransaccion = Convert.ToInt64(SecuenciaTransaccion);
                        
                        //string temp = View.Tarjeta.EntranceModule.Substring(4, 2);
                        //int Carril = Convert.ToInt32(temp);
                        oTransaccion.CarrilEntrada = Carril;
                        oTransaccion.ModuloEntrada = View.Tarjeta.EntranceModule;
                        oTransaccion.IdEstacionamiento = Convert.ToInt64(Globales.iCodigoEstacionamiento);
                        oTransaccion.IdTarjeta = View.Tarjeta.CodeCard;
                        oTransaccion.PlacaEntrada = "------";
                        oTransaccion.IdTipoVehiculo = TipoVehiculo;

                        oResultadoOperacion = Model.RegistrarIngreso(oTransaccion);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            SecuenciaTransaccionFinal = oResultadoOperacion.EntidadDatos.ToString();
                        }
                    }

                }
                else 
                {
                    SecuenciaTransaccionFinal = SecuenciaTransaccion; 
                }
                
                // SecuenciaTransaccion = View.Tarjeta.CodeCard;

                View.General_EventsBill = "Presenter-ConsultaValor SecuenciaTransaccion =" + SecuenciaTransaccionFinal + " REPO = " + REPO + " TipoVehiculo = " + TipoVehiculo + " Codigo = " + Codigo;

                if (Codigo == "32")
                {
                    //Codigo = string.Empty;
                    Codigo = "0";
                }

                View.PagoEfectivo.ValorPago = 0;
                View.PagoEfectivo.Subtotal=0;
                View.PagoEfectivo.Iva = 0;

                oResultadoOperacion = Model.ConsultaValor(SecuenciaTransaccionFinal, TipoVehiculo, false, REPO, Codigo,0);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "Presenter-ConsultaValor ok ";

                    List<DatosLiquidacion> lstDatosLiquidacion = new List<DatosLiquidacion>();

                    if (oResultadoOperacion.ListaEntidadDatos != null)
                    {
                        string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        if (detalle.Length == 2)
                        {
                            if (detalle[1].ToString() == "5")
                            {
                                View.TipoPago = "EVENTO";
                            }
                            else if (detalle[1].ToString() == "3")
                            {
                                View.TipoPago = "REPOSICIÓN";
                            }
                        }
                        else
                        {
                            if (detalle[1].ToString() == "3")
                            {
                                View.TipoPago = "REPOSICIÓN";
                            }
                            else if (detalle[1].ToString() == "2")
                            {
                                View.TipoPago = "MENSUALIDAD";
                            }
                            else if (detalle[1].ToString() == "6")
                            {
                                View.TipoPago = "CASCOS";
                            }
                            else if (detalle[3].ToString() == "5")
                            {
                                View.TipoPago = "EVENTO";
                            }
                        }
                        lstDatosLiquidacion = (List<DatosLiquidacion>)oResultadoOperacion.ListaEntidadDatos;
                        View.lstDtoLiquidacion = lstDatosLiquidacion;

                        for (int i = 0; i < oResultadoOperacion.ListaEntidadDatos.Count; i++)
                        {
                            View.PagoEfectivo.ValorPago += Convert.ToInt64(lstDatosLiquidacion[i].Total);
                            View.PagoEfectivo.Subtotal += Convert.ToInt64(lstDatosLiquidacion[i].SubTotal);
                            View.PagoEfectivo.Iva += Convert.ToInt64(lstDatosLiquidacion[i].Iva);
                            if (lstDatosLiquidacion[i].Tipo == 3)
                            {
                                View.ValorTipoPago = lstDatosLiquidacion[i].Total.ToString();
                            }
                        }
                    }
                    else
                    {

                        lstDatosLiquidacion = (List<DatosLiquidacion>)oResultadoOperacion.ListaEntidadDatos;
                        View.lstDtoLiquidacion = lstDatosLiquidacion;

                        View.General_EventsBill = "Presenter-LISTA ENTIDAD NULL";
                        string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        View.PagoEfectivo.ValorPago = Convert.ToInt64(detalle[0]);
                        View.PagoEfectivo.Subtotal = Convert.ToInt64(detalle[1]);
                        View.PagoEfectivo.Iva = Convert.ToInt64(detalle[2]);
                    }

                    View.General_EventsBill = "Presenter-IVA " + View.PagoEfectivo.Iva + " SUBTOTAL " + View.PagoEfectivo.Subtotal + " TOTAL " + View.PagoEfectivo.ValorPago;

                    View.Operacion.Iva = View.PagoEfectivo.Iva;
                    View.Operacion.Total = View.PagoEfectivo.Subtotal;
                    View.Operacion.TotalPagado = View.PagoEfectivo.ValorPago;

                    if (View.PagoEfectivo.ValorPago <= 0)
                    {
                        
                    }
                    ok = true;
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_EventsBill = "Error Presenter-ConsultaValor: " + oResultadoOperacion.Mensaje;
                    oReader.ExpulsarTarjeta();
                }
            }
            catch (Exception ex)
            {
                View.General_EventsBill = ex.ToString();
                oReader.ExpulsarTarjeta();
            }
            return ok;
        }
        public bool ConsultaValorTarifaEspecial(int IdTarifa)
        {
            bool ok = false;
            int Carril = 0;
            try
            {

                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                SecuenciaTransaccion = string.Empty;
                int TipoVehiculo = 0;
                string Codigo = View.Tarjeta.CodeAgreement1.ToString();

                DateTime Entrada = Convert.ToDateTime(View.Tarjeta.DateTimeEntrance);
                string año = Entrada.Year.ToString();
                string MES = Entrada.Month.ToString();
                string DIA = Entrada.Day.ToString();
                string HORA = Entrada.Hour.ToString();
                string MINU = Entrada.Minute.ToString();
                string SEG = Entrada.Second.ToString();

                //string IdTransaccion = Entrada.ToString("yyyyddMMHHmmss");
                string IdTransaccion = año + MES.PadLeft(2, '0') + DIA.PadLeft(2, '0') + HORA.PadLeft(2, '0') + MINU.PadLeft(2, '0') + SEG.PadLeft(2, '0');



                if (View.Tarjeta.EntranceModule.Trim().Length > 5)
                {
                    string temp = View.Tarjeta.EntranceModule.Substring(4, 2).Trim();
                    Carril = Convert.ToInt32(temp);
                }
                else
                {
                    string Modulo = View.Tarjeta.EntranceModule.Trim();
                    if (Modulo == "ADM01")
                    {
                        Carril = 30;
                    }
                }

                SecuenciaTransaccion = IdTransaccion + Carril + Globales.iCodigoEstacionamiento;

                //SecuenciaTransaccion = "20200318111103471";
                if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                {
                    TipoVehiculo = 1;
                }
                else
                {
                    TipoVehiculo = 2;
                }
                if (Codigo == string.Empty)
                {
                    Codigo = "0";
                }



                //if vehiculo es mensual enviar idtarjeta en secuencia

                string SecuenciaTransaccionFinal = string.Empty;


                oResultadoOperacion = Model.ValidarTransaccion(View.Tarjeta.CodeCard, Convert.ToDateTime(View.Tarjeta.DateTimeEntrance));
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    SecuenciaTransaccionFinal = oResultadoOperacion.EntidadDatos.ToString();

                    if (SecuenciaTransaccionFinal == string.Empty)
                    {
                        //crea transaccion en bd

                        Transaccion oTransaccion = new Transaccion();

                        oTransaccion.IdTransaccion = Convert.ToInt64(SecuenciaTransaccion);

                        //string temp = View.Tarjeta.EntranceModule.Substring(4, 2);
                        //int Carril = Convert.ToInt32(temp);
                        oTransaccion.CarrilEntrada = Carril;
                        oTransaccion.ModuloEntrada = View.Tarjeta.EntranceModule;
                        oTransaccion.IdEstacionamiento = Convert.ToInt64(Globales.iCodigoEstacionamiento);
                        oTransaccion.IdTarjeta = View.Tarjeta.CodeCard;
                        oTransaccion.PlacaEntrada = "------";
                        oTransaccion.IdTipoVehiculo = TipoVehiculo;

                        oResultadoOperacion = Model.RegistrarIngreso(oTransaccion);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            SecuenciaTransaccionFinal = oResultadoOperacion.EntidadDatos.ToString();
                        }
                    }

                }
                else
                {
                    SecuenciaTransaccionFinal = SecuenciaTransaccion;
                }

                // SecuenciaTransaccion = View.Tarjeta.CodeCard;

                View.General_EventsBill = "Presenter-ConsultaValor SecuenciaTransaccion =" + SecuenciaTransaccionFinal + " REPO = " + false + " TipoVehiculo = " + TipoVehiculo + " Codigo = " + Codigo;

                if (Codigo == "32")
                {
                    Codigo = string.Empty;
                }

                oResultadoOperacion = Model.ConsultaValor(SecuenciaTransaccionFinal, TipoVehiculo, false, false, Codigo,IdTarifa);


                View.PagoEfectivo.ValorPago = 0;
                View.PagoEfectivo.Subtotal = 0;
                View.PagoEfectivo.Iva = 0;

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "Presenter-ConsultaValor ok ";

                    List<DatosLiquidacion> lstDatosLiquidacion = new List<DatosLiquidacion>();

                    if (oResultadoOperacion.ListaEntidadDatos != null)
                    {
                        string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        if (detalle.Length == 2)
                        {
                            if (detalle[1].ToString() == "5")
                            {
                                View.TipoPago = "EVENTO";
                            }
                            else if (detalle[1].ToString() == "3")
                            {
                                View.TipoPago = "REPOSICIÓN";
                            }
                        }
                        else
                        {
                            if (detalle[1].ToString() == "3")
                            {
                                View.TipoPago = "REPOSICIÓN";
                            }
                            else if (detalle[1].ToString() == "2")
                            {
                                View.TipoPago = "MENSUALIDAD";
                            }
                            else if (detalle[1].ToString() == "6")
                            {
                                View.TipoPago = "CASCOS";
                            }
                            else if (detalle[3].ToString() == "5")
                            {
                                View.TipoPago = "EVENTO";
                            }
                        }
                        lstDatosLiquidacion = (List<DatosLiquidacion>)oResultadoOperacion.ListaEntidadDatos;
                        View.lstDtoLiquidacion = lstDatosLiquidacion;

                        for (int i = 0; i < oResultadoOperacion.ListaEntidadDatos.Count; i++)
                        {
                            View.PagoEfectivo.ValorPago += Convert.ToInt64(lstDatosLiquidacion[i].Total);
                            View.PagoEfectivo.Subtotal += Convert.ToInt64(lstDatosLiquidacion[i].SubTotal);
                            View.PagoEfectivo.Iva += Convert.ToInt64(lstDatosLiquidacion[i].Iva);
                            if (lstDatosLiquidacion[i].Tipo == 3)
                            {
                                View.ValorTipoPago = lstDatosLiquidacion[i].Total.ToString();
                            }
                        }
                    }
                    else
                    {

                        lstDatosLiquidacion = (List<DatosLiquidacion>)oResultadoOperacion.ListaEntidadDatos;
                        View.lstDtoLiquidacion = lstDatosLiquidacion;

                        View.General_EventsBill = "Presenter-LISTA ENTIDAD NULL";
                        string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        View.PagoEfectivo.ValorPago = Convert.ToInt64(detalle[0]);
                        View.PagoEfectivo.Subtotal = Convert.ToInt64(detalle[1]);
                        View.PagoEfectivo.Iva = Convert.ToInt64(detalle[2]);
                    }

                    View.General_EventsBill = "Presenter-IVA " + View.PagoEfectivo.Iva + " SUBTOTAL " + View.PagoEfectivo.Subtotal + " TOTAL " + View.PagoEfectivo.ValorPago;

                    View.Operacion.Iva = View.PagoEfectivo.Iva;
                    View.Operacion.Total = View.PagoEfectivo.Subtotal;
                    View.Operacion.TotalPagado = View.PagoEfectivo.ValorPago;

                    if (View.PagoEfectivo.ValorPago <= 0)
                    {

                    }
                    ok = true;
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_EventsBill = "Error Presenter-ConsultaValor: " + oResultadoOperacion.Mensaje;
                    oReader.ExpulsarTarjeta();
                }
            }
            catch (Exception ex)
            {
                View.General_EventsBill = ex.ToString();
                oReader.ExpulsarTarjeta();
            }
            return ok;
        }
        public bool ConsultaValorMensualidad()
        {
            bool ok = false;

            try
            {
                View.RetornoCobro = 0;

                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                //if vehiculo es mensual enviar idtarjeta en secuencia
                bool REPO = false;
                string SecuenciaTransaccion = string.Empty;

                if (View.Tarjeta.Replacement == true)
                {
                    REPO = true;
                }


                SecuenciaTransaccion = View.lstDtoAutorizado[0].IdTarjeta;
                oResultadoOperacion = Model.ConsultaValor(SecuenciaTransaccion,0, true, REPO, "",0);



                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    View.General_EventsBill = "Presenter-ConsultaValor ok ";

                    List<DatosLiquidacion> lstDatosLiquidacion = new List<DatosLiquidacion>();

                    if (oResultadoOperacion.ListaEntidadDatos != null)
                    {
                        //string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        //if (detalle[3].ToString() == "3")
                        //{
                        //    View.TipoPago = "REPOSICIÓN";
                        //}
                        //else if (detalle[3].ToString() == "2")
                        //{
                        //    View.TipoPago = "MENSUALIDAD";
                        //}
                        //else if (detalle[3].ToString() == "7")
                        //{
                        //    View.TipoPago = "MENSUALIDAD";
                        //}
                       



                        lstDatosLiquidacion = (List<DatosLiquidacion>)oResultadoOperacion.ListaEntidadDatos;
                        View.lstDtoLiquidacion = lstDatosLiquidacion;

                        for (int i = 0; i < lstDatosLiquidacion.Count; i++)
                        {
                            if (lstDatosLiquidacion[i].Tipo == 3)
                            {
                                View.TipoPago = "REPOSICIÓN";
                            }
                            else if (lstDatosLiquidacion[i].Tipo == 2)
                            {
                                View.TipoPago = "MENSUALIDAD";
                            }
                            else if (lstDatosLiquidacion[i].Tipo == 7)
                            {
                                View.TipoPago = "MENSUALIDAD";
                            }

                            if (lstDatosLiquidacion[i].Total > 0)
                            {
                                View.RetornoCobro += lstDatosLiquidacion[i].Tipo;
                            }

                        }



                        View.CobroTarjetaMensual = false;

                        for (int i = 0; i < lstDatosLiquidacion.Count; i++)
                        {
                            if (lstDatosLiquidacion[i].Tipo == 7)
                            {
                                View.CobroTarjetaMensual = true;
                                break;
                            }
                        }

                        for (int i = 0; i < oResultadoOperacion.ListaEntidadDatos.Count; i++)
                        {
                            View.PagoEfectivo.ValorPago += Convert.ToInt64(lstDatosLiquidacion[i].Total);
                            View.PagoEfectivo.Subtotal += Convert.ToInt64(lstDatosLiquidacion[i].SubTotal);
                            View.PagoEfectivo.Iva += Convert.ToInt64(lstDatosLiquidacion[i].Iva);
                            if (lstDatosLiquidacion[i].Tipo == 3)
                            {
                                View.ValorTipoPago = lstDatosLiquidacion[i].Total.ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] detalle = oResultadoOperacion.EntidadDatos.ToString().Split(';');

                        if (detalle[3].ToString() == "2")
                        {
                            View.TipoPago = "MENSUALIDAD";
                            View.ValorTipoPago = detalle[0].ToString();
                        }

                        else if (detalle[3].ToString() == "3")
                        {
                            View.TipoPago = "REPOSICIÓN";
                            View.ValorTipoPago = detalle[0].ToString();
                        }

                        else if (detalle[3].ToString() == "7")
                        {
                            View.TipoPago = "MENSUALIDAD";
                            View.ValorTipoPago = detalle[0].ToString();
                        }



                        View.PagoEfectivo.ValorPago = Convert.ToInt64(detalle[0]);
                        View.PagoEfectivo.Subtotal = Convert.ToInt64(detalle[1]);
                        View.PagoEfectivo.Iva = Convert.ToInt64(detalle[2]);
                    }

                    View.Operacion.Iva = View.PagoEfectivo.Iva;
                    View.Operacion.Total = View.PagoEfectivo.Subtotal;
                    View.Operacion.TotalPagado = View.PagoEfectivo.ValorPago;

                    if (View.PagoEfectivo.ValorPago <= 0)
                    {
                        oReader.ExpulsarTarjeta();
                    }
                    ok = true;
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    View.General_EventsBill = "Error Presenter-ConsultaValor: " + oResultadoOperacion.Mensaje;
                    oReader.ExpulsarTarjeta();
                }

            }
            catch (Exception ex)
            {
                View.General_EventsBill = ex.ToString();
                oReader.ExpulsarTarjeta();
            }
            return ok;
        }
        public bool RegistrarPagoEfectivo()
        {
            bool ok = false;

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string IP = ObtenerValorParametro(Parametros.IPCity);
            View.ConsultarValorResult.serialMaquina = Globales.sSerial;

            oResultadoOperacion = Model.RegistrarPagoEfectivo(IP, View.ConsultarValorResult);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var Content = oResultadoOperacion.EntidadDatos.ToString();
                var ss = JsonConvert.DeserializeObject<RootObjectPagoEfectivo>(Content, jsonSettings);

                if (ss.registrarPagoEfectivoResult[0].estado)
                {
                    View.General_EventsBill = "Presenter-RegistrarPagoEfectivo OK ";
                    View.lstRegistrarPagoEfectivoResult = ss.registrarPagoEfectivoResult;
                    ok = true;
                }
                else
                {
                    View.General_EventsBill = "Error Presenter-RegistrarPagoEfectivo " + ss.registrarPagoEfectivoResult[0].error;
                }
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                View.General_EventsBill = "Error Presenter-RegistrarPagoEfectivo: " + oResultadoOperacion.Mensaje;
            }


            return ok;
        }        
        #endregion

        #region Printer
        private DataTable TicketArqu()
        {
            DataTable tableParte = new DataTable();

            //List<DtoArqueo> lstArqueo = new List<DtoArqueo>();
            tableParte.Columns.Add(InfoPresentacion.ColumnaTipoParte);
            tableParte.Columns.Add(InfoPresentacion.ColumnaDenominacion);
            tableParte.Columns.Add(InfoPresentacion.ColumnaCantidad);
            tableParte.Columns.Add(InfoPresentacion.ColumnaDinero);

            foreach (DtoArqueo item in View.DtoArqueo)
            {
                DataRow fila = tableParte.NewRow();
                fila[InfoPresentacion.ColumnaTipoParte] = item.Parte;
                fila[InfoPresentacion.ColumnaDenominacion] = item.Denominacion;
                fila[InfoPresentacion.ColumnaCantidad] = item.Cantidad;
                fila[InfoPresentacion.ColumnaDinero] = item.Valor;

                tableParte.Rows.Add(fila);
            }

            return tableParte;
        }
        public DataSetTicketArqueo GenerarTicketArqueo()
        {
            DataSetTicketArqueo facturacion = new DataSetTicketArqueo();
            if (ArqueoCoinBag.Rows.Count >= 1)
            {
                foreach (DataRow row in ArqueoCoinBag.Rows)
                {

                    DataSetTicketArqueo.TablaArqueoRow rowDatosArqueo = facturacion.TablaArqueo.NewTablaArqueoRow();

                    rowDatosArqueo.CodigoArqueo = oTicketArqueo.CodigoArqueo.ToString();
                    rowDatosArqueo.FechaArqueo = ObtenerFechaServer().ToString("yyyy/MM/dd HH:mm:ss");
                    rowDatosArqueo.ModuloArqueo = Globales.sSerial;
                    rowDatosArqueo.UsuarioArqueo = View.Usuario.IdCriptUsuario;
                    if (View.ProcesoArqueoParcial)
                    {
                        rowDatosArqueo.TipoArqueo = "PARCIAL";
                    }
                    else
                    {
                        rowDatosArqueo.TipoArqueo = "TOTAL";
                    }
                    rowDatosArqueo.Parte = row[0].ToString();
                    rowDatosArqueo.Denominacion = Convert.ToInt32(row[1].ToString());
                    rowDatosArqueo.Cantidad = Convert.ToInt32(row[2].ToString());
                    rowDatosArqueo.ValorArqueo = Convert.ToInt32(row[3].ToString());

                    facturacion.TablaArqueo.AddTablaArqueoRow(rowDatosArqueo);
                }
            }
            else
            {
                DataSetTicketArqueo.TablaArqueoRow rowDatosArqueo = facturacion.TablaArqueo.NewTablaArqueoRow();

                rowDatosArqueo.CodigoArqueo = oTicketArqueo.CodigoArqueo.ToString();
                rowDatosArqueo.FechaArqueo = ObtenerFechaServer().ToString("yyyy/MM/dd HH:mm:ss");
                rowDatosArqueo.ModuloArqueo = Globales.sSerial;
                rowDatosArqueo.UsuarioArqueo = View.Usuario.IdCriptUsuario;

                if (View.ProcesoArqueoParcial)
                {
                    rowDatosArqueo.TipoArqueo = "PARCIAL";
                }
                else
                {
                    rowDatosArqueo.TipoArqueo = "TOTAL";
                }

                facturacion.TablaArqueo.AddTablaArqueoRow(rowDatosArqueo);
            }


            return facturacion;
        }
        public DataSetTicketDevolucion GenerarTicketDevolucion()
        {
            DataSetTicketDevolucion facturacion = new DataSetTicketDevolucion();

            DataSetTicketDevolucion.TablaDevolucionRow rowDatosFactura = facturacion.TablaDevolucion.NewTablaDevolucionRow();

            rowDatosFactura.Fecha = ObtenerFechaServer().ToString();
            rowDatosFactura.IdTransaccion = View.IdTransaccion;

            rowDatosFactura.Modulo = Globales.sSerial;

            if (View.PagoEfectivo.ValorRecibido > View.PagoEfectivo.ValorPago)
            {
                rowDatosFactura.ValorEntregado = View.PagoEfectivo.ValorEntregado;
                rowDatosFactura.ValorFaltante = View.PagoEfectivo.ValorRecibido - View.PagoEfectivo.ValorPago;
                rowDatosFactura.TotalCliente = View.PagoEfectivo.ValorPago;
                rowDatosFactura.ValorTotal = View.PagoEfectivo.ValorPago;
                rowDatosFactura.ValorRecibido = View.PagoEfectivo.ValorRecibido;
            }
            else 
            {
                rowDatosFactura.ValorEntregado = View.PagoEfectivo.ValorEntregado;
                rowDatosFactura.ValorFaltante = View.PagoEfectivo.ValorPago - View.PagoEfectivo.ValorRecibido;
                rowDatosFactura.TotalCliente = View.PagoEfectivo.ValorPago;
                rowDatosFactura.ValorTotal = View.PagoEfectivo.ValorPago;
                rowDatosFactura.ValorRecibido = View.PagoEfectivo.ValorRecibido;
            }

            //double faltante = (View.PagoEfectivo.ValorRecibido - View.PagoEfectivo.ValorPago);

            //if (faltante < 0)
            //{
            //    rowDatosFactura.ValorEntregado = View.PagoEfectivo.ValorEntregado;
            //    rowDatosFactura.ValorFaltante = View.PagoEfectivo.ValorRecibido;
            //    rowDatosFactura.TotalCliente = View.PagoEfectivo.ValorPago;
            //    rowDatosFactura.ValorTotal = View.PagoEfectivo.ValorPago;
            //    rowDatosFactura.ValorRecibido = View.PagoEfectivo.ValorRecibido;
            //}
            //else
            //{
            //    rowDatosFactura.ValorEntregado = View.PagoEfectivo.ValorEntregado;
            //    rowDatosFactura.ValorFaltante = faltante - View.PagoEfectivo.ValorEntregado;
            //    rowDatosFactura.TotalCliente = View.PagoEfectivo.ValorPago;
            //    rowDatosFactura.ValorTotal = View.PagoEfectivo.ValorPago;
            //    rowDatosFactura.ValorRecibido = View.PagoEfectivo.ValorRecibido;
            //}


            facturacion.TablaDevolucion.AddTablaDevolucionRow(rowDatosFactura);

            return facturacion;
        }
        public DataSetTicketCarga GenerarTicketCarga()
        {
            DataSetTicketCarga facturacion = new DataSetTicketCarga();


            View.CargaMonedas = false;
            //View.CargaBilletes = true;

            ObtenerInfoPartes();


            if (View.CargaActualTemporal != null)
            {
                foreach (DataRow row in View.CargaActualTemporal.Rows)
                {
                    DataSetTicketCarga.TablaCargaRow rowDatosCarga = facturacion.TablaCarga.NewTablaCargaRow();
                    rowDatosCarga.Denominacion = Convert.ToDouble(row[1].ToString());
                    rowDatosCarga.Cantidad = row[2].ToString();
                    rowDatosCarga.Total = Convert.ToDouble(row[3].ToString());
                    rowDatosCarga.Parte = row[0].ToString();

                    rowDatosCarga.CodigoCarga = oTicketCarga.CodigoCarga.ToString();
                    rowDatosCarga.FechaCarga = ObtenerFechaServer().ToString();
                    rowDatosCarga.ModuloCarga = Globales.sSerial;
                    rowDatosCarga.UsuarioCarga = View.Usuario.IdCriptUsuario;

                    facturacion.TablaCarga.AddTablaCargaRow(rowDatosCarga);
                }
            }
            else
            {
                DataSetTicketCarga.TablaCargaRow rowDatosCarga = facturacion.TablaCarga.NewTablaCargaRow();
                rowDatosCarga.Denominacion = 0;
                rowDatosCarga.Cantidad = "0";
                rowDatosCarga.Total = 0;
                rowDatosCarga.Parte = string.Empty;

                rowDatosCarga.CodigoCarga = oTicketCarga.CodigoCarga.ToString();
                rowDatosCarga.FechaCarga = ObtenerFechaServer().ToString();
                rowDatosCarga.ModuloCarga = Globales.sSerial;
                rowDatosCarga.UsuarioCarga = View.Usuario.IdCriptUsuario;

                facturacion.TablaCarga.AddTablaCargaRow(rowDatosCarga);
            }

            View.CargaBilletesBB = false;

            View.CargaMonedas = true;
            ObtenerInfoPartes();

            //if (View.CargaActualTemporal != null)
            //{
            //    foreach (DataRow row in View.CargaActualTemporal.Rows)
            //    {
            //        DataSetTicketCarga.TablaCargaRow rowDatosCarga = facturacion.TablaCarga.NewTablaCargaRow();
            //        rowDatosCarga.Denominacion = Convert.ToDouble(row[1].ToString());
            //        rowDatosCarga.Cantidad = row[2].ToString();
            //        rowDatosCarga.Total = Convert.ToDouble(row[3].ToString());
            //        rowDatosCarga.Parte = row[0].ToString();

            //        rowDatosCarga.CodigoCarga = oTicketCarga.CodigoCarga.ToString();
            //        rowDatosCarga.FechaCarga = DateTime.Now.ToString();
            //        rowDatosCarga.ModuloCarga = Globales.sSerial;
            //        rowDatosCarga.UsuarioCarga = View.Usuario.IdCriptUsuario;

            //        facturacion.TablaCarga.AddTablaCargaRow(rowDatosCarga);
            //    }
            //}

            View.CargaBilletesBB = false;
            View.CargaMonedas = false;

            return facturacion;
        }
        public DataSetTicketPago GenerarTicketPago(string PIN)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();
            
            double total = 0;
            foreach (var item in View.lstDtoLiquidacion)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in View.lstDtoLiquidacion)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Cambio = View.PagoEfectivo.ValorCambio;
                rowDatosFactura.Direccion = "CALLE 157 19-55 Floridablanca";
                rowDatosFactura.Fecha = ObtenerFechaServer().ToString();
                rowDatosFactura.IdTransaccion = View.Operacion.ID_Transaccion.ToString();
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = Globales.sSerial;
                rowDatosFactura.Nombre = "UNAB BOSQUE";
                rowDatosFactura.NumeroFactura = View.DtoOperacion.DtoPago.Factura;
                rowDatosFactura.Placa = View.Tarjeta.EntrancePlate;
                rowDatosFactura.Recibido = View.PagoEfectivo.ValorRecibido;
                rowDatosFactura.Resolucion = View.DtoModulo.Factura.NumeroResolucion.ToString();
                rowDatosFactura.Rut = "NIT 900554696-8";
                rowDatosFactura.Telefono = "6520587";

                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = item.Total;
                rowDatosFactura.Subtotal = item.SubTotal;
                rowDatosFactura.Iva = item.Iva;

                string tp = string.Empty;
                if (item.Tipo == 1)
                {
                    tp = "Estacionamiento";
                }
                else if (item.Tipo == 2)
                {
                    tp = "Mensualidad";
                }
                else if (item.Tipo == 3)
                {
                    tp = "Reposicion";
                }
                else if (item.Tipo == 5)
                {
                    tp = "Evento";
                }
                else if (item.Tipo == 6)
                {
                    tp = "Cascos";
                }

                rowDatosFactura.TipoPago = tp;
                rowDatosFactura.Fecha2 = View.Tarjeta.DateTimeEntrance.ToString();
                string TIPO = string.Empty;
                if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                {
                    TIPO = "CARRO";
                }
                else
                {
                    TIPO = "MOTO";
                }
                rowDatosFactura.Vehiculo = TIPO;
                rowDatosFactura.Convenio = View.NombreConvenio;
                rowDatosFactura.VigenciaFactura = View.DtoModulo.Factura.FechaFinResolucion;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }
            
            return facturacion;
        }
        public DataSetTicketPago GenerarTicketPagoMensualidad()
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();


            double total = 0;
            foreach (var item in View.lstDtoLiquidacion)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in View.lstDtoLiquidacion)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Cambio = View.PagoEfectivo.ValorCambio;
                rowDatosFactura.Direccion = "CALLE 157  19-55 Floridablanca";
                rowDatosFactura.Fecha = ObtenerFechaServer().ToString();
                rowDatosFactura.IdTransaccion = View.Operacion.ID_Transaccion.ToString();
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = Globales.sSerial;
                rowDatosFactura.Nombre = "UNAB BOSQUE";
                rowDatosFactura.NumeroFactura = View.DtoOperacion.DtoPago.Factura;
                rowDatosFactura.Placa = View.Tarjeta.EntrancePlate;
                rowDatosFactura.Recibido = View.PagoEfectivo.ValorRecibido;
                rowDatosFactura.Resolucion = View.DtoModulo.Factura.NumeroResolucion.ToString();
                rowDatosFactura.Rut = "NIT 900003663-1";
                rowDatosFactura.Telefono = "6489090";


                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = item.Total;
                rowDatosFactura.Subtotal = item.SubTotal;
                rowDatosFactura.Iva = item.Iva;

                string tp = string.Empty;
                if (item.Tipo == 2)
                {
                    tp = "Mensualidad";
                }
                else if (item.Tipo == 7)
                {
                    tp = "Tarjeta";
                }

                rowDatosFactura.TipoPago = tp;




                rowDatosFactura.Fecha2 = View.Tarjeta.DateTimeEntrance.ToString();
                string TIPO = string.Empty;
                if (View.Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
                {
                    TIPO = "CARRO";
                }
                else
                {
                    TIPO = "MOTO";
                }
                rowDatosFactura.Vehiculo = TIPO;
                rowDatosFactura.Convenio = View.NombreConvenio;
                rowDatosFactura.VigenciaFactura = View.DtoModulo.Factura.FechaFinResolucion;

                rowDatosFactura.Fecha = ObtenerFechaServer().ToString();

                rowDatosFactura.NombreAutorizado = View.lstDtoAutorizado[0].NombresAutorizado;
                rowDatosFactura.Documento = View.lstDtoAutorizado[0].Documento;
                rowDatosFactura.NombreAutorizacion = View.lstDtoAutorizado[0].NombreAutorizacion;
                rowDatosFactura.Nit = View.lstDtoAutorizado[0].NIT;
                rowDatosFactura.NombreEmpresa = View.lstDtoAutorizado[0].NombreEmpresa;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        public DataSetValidacion GenerarTicketValidacion()
        {
            DataSetValidacion facturacion = new DataSetValidacion();

            DataSetValidacion.TablaValidacionRow rowDatosFactura = facturacion.TablaValidacion.NewTablaValidacionRow();

            rowDatosFactura.Convenio = View.NombreConvenio;
            rowDatosFactura.FechaEntrada = View.Tarjeta.DateTimeEntrance.ToString();
            DateTime FECH = ObtenerFechaConvenio();
            rowDatosFactura.FechaValidacion = FECH.ToString("dd/MM/yyyy HH:mm:ss");

            facturacion.TablaValidacion.AddTablaValidacionRow(rowDatosFactura);


            return facturacion;
        }
        #endregion

    }
}
