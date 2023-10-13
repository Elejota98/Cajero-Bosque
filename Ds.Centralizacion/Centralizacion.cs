using Ds.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Ds.Centralizacion
{
    public partial class Centralizacion : ServiceBase
    {
        #region Definiciones
        private Timer oTimer;

        private static object objLock = new object();
        private static Centralizacion Agente = new Centralizacion();
        private static ServiceProxy.IProxyService _ProxyServicios;

        private static int _PeriodoEjecucionSegundos
        {
            get
            {
                string sPeriodoEjecucionSegundos = ConfigurationManager.AppSettings["PeriodoEjecucionSegundos"];
                if (string.IsNullOrEmpty(sPeriodoEjecucionSegundos))
                {
                    return 10;
                }
                else
                {
                    return Convert.ToInt32(sPeriodoEjecucionSegundos);
                }
            }
        }
        public static string sSerial
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["serial"];
                if (!string.IsNullOrEmpty(sIdeModulo))
                {
                    return sIdeModulo;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sSerialLocal
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["serialLocal"];
                if (!string.IsNullOrEmpty(sIdeModulo))
                {
                    return sIdeModulo;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sIdModulo
        {
            get
            {
                string sIdModulo = ConfigurationManager.AppSettings["IdModulo"];
                if (!string.IsNullOrEmpty(sIdModulo))
                {
                    return sIdModulo;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion
       
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().Exited += new EventHandler(Centralizacion_Exited);

            try
            {
                if (System.Diagnostics.Process.GetProcessesByName
                    (System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
                    throw new ApplicationException("Existe otra instancia del servicio en ejecución.");

                if (!Environment.UserInteractive)
                    Centralizacion.Run(Agente);
                else
                {
                    if (Environment.UserInteractive)
                        Console.ForegroundColor = ConsoleColor.Green;

                    Agente.OnStart(null);

                    if (Environment.UserInteractive)
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (Environment.UserInteractive)
                        Console.WriteLine("El servicio se inicio correctamente y queda en espera de atender solicitudes.");

                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ocurrió un error iniciando el servicio.");
                    Console.WriteLine(ex.Message);
                    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                }
            }
        }

        public Centralizacion()
        {
            _ProxyServicios = new ServiceProxy.ProxyService();
            oTimer = new Timer(_PeriodoEjecucionSegundos * 1000);
            oTimer.Elapsed += new ElapsedEventHandler(oTimer_Elapsed);

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            oTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            oTimer.Enabled = false;
        }

        void oTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                oTimer.Enabled = false;

                if (Environment.UserInteractive)
                    Console.WriteLine("El servicio inicia revision");

                lock (objLock)
                {

                    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                    #region T_Transacciones

                    //OBTENER DATOS SINCRONIZACION TRANSACCIONES

                    oResultadoOperacion = _ProxyServicios.GenerarSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo registro ok");
                    }

                    //OBTENER DATOS SINCRONIZACIONPAGO TRANSACCIONES
                    oResultadoOperacion = _ProxyServicios.GenerarSincronizacionPago(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo registro transaccion pago ok");
                    }

                    #endregion

                    #region T_Cambio
                    oResultadoOperacion = _ProxyServicios.GenerarCambioSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    #region T_Recargas
                    oResultadoOperacion = _ProxyServicios.GenerarRecargasSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    #region T_Donacion
                    oResultadoOperacion = _ProxyServicios.GenerarDonacionSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    #region T_Arqueos
                    oResultadoOperacion = _ProxyServicios.GenerarArqueosSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    #region T_Cargas
                    oResultadoOperacion = _ProxyServicios.GenerarCargasSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    #region T_Partes
                    oResultadoOperacion = _ProxyServicios.GenerarPartesSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo partes ok");
                    }
                    #endregion
                    
                    #region T_Movimientos
                    oResultadoOperacion = _ProxyServicios.GenerarMovimientosSincronizacion(sSerial);
                    if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    {
                        if (Environment.UserInteractive)
                            Console.WriteLine("Sincronizo Movimiento ok");
                    }
                    #endregion

                    ////////////////////////////////////////////

                    #region ValidacionRed
                    //oResultadoOperacion = _ProxyServicios.ValidacionAutorizadosSincronizacion(sSerial,Convert.ToInt64(sIdEstacionamiento));
                    //if (oResultadoOperacion.oEstado == BusinessObjects.Enums.TipoRespuesta.Exito)
                    //{
                    //    if (Environment.UserInteractive)
                    //        Console.WriteLine("Sincronizo Autorizaciones ok");
                    //}
                    #endregion

                    oTimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                string sFechaFile = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                //TraceHandler.WriteLine(LOG.NombreArchivoLogRegistraArchivos + sFechaFile, "SERVICIO WINDOWS: excepcion servicio: " + ex.Source + " " + ex.StackTrace + " " + ex.Message, TipoLog.TRAZA);

                oTimer.Enabled = true;
            }
        }


        static void Centralizacion_Exited(object sender, EventArgs e)
        {
            try
            {
                if (!Environment.UserInteractive)
                {
                    Agente.OnStop();
                    Agente = null;
                }
            }
            catch { }
        }
    }
}
