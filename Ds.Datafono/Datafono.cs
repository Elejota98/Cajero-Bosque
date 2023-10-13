using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Datafono
{
    public class Datafono
    {
        System.IO.FileSystemWatcher m_Watcher = new System.IO.FileSystemWatcher();

        public delegate void Funcion(string e);

        public event Funcion EventoRespuesta;

        private static Datafono _instancia;

        public static Datafono Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Datafono();
                }

                return _instancia;
            }
        }

        private string _ArchivoRespuesta = string.Empty;
        private string _ArchivoSolicitud = string.Empty;
        private string _ArchivoCajero = string.Empty;

        /// <summary>
        /// Funcion para abrir la app cajero Exe (Esta es la aplicacion que tiene comunicacion con el datafono al momento de pagar)
        /// </summary>
        /// <param name="sPathArchivoRespuesta">Path de la ubicacion del archivo de respuesta (c:\\Datafono\\)</param>
        /// <param name="sNombreArchivoRespuesta">Nombre del archivo donde se encontrara la respuesta RESPUESTA.txt</param>
        /// <param name="sArchivoSolicitud">Path donde se deja el archivo de solicitud (c:\\Datafono\\SOLICITUD.txt)</param>
        /// <param name="sArchivoCajero">Path del software de redeban (c:\\Datafono\\Cajas.exe)</param>
        /// <returns>Retorna resultado operacion exito si logra iniciar correctamente el software de redeban</returns>
        public ResultadoOperacion IniciarDatafono(string sPathArchivoRespuesta, string sNombreArchivoRespuesta, string sArchivoSolicitud, string sArchivoCajero)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                //Quitar dato quemado
                m_Watcher.Path = sPathArchivoRespuesta;
                //m_Watcher.Path = "c:\\Datafono\\";
                m_Watcher.Filter = "*.txt";
                m_Watcher.NotifyFilter = NotifyFilters.LastAccess |
                             NotifyFilters.LastWrite |
                             NotifyFilters.FileName |
                             NotifyFilters.DirectoryName;
                m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                _ArchivoRespuesta = sNombreArchivoRespuesta;
                _ArchivoSolicitud = sArchivoSolicitud;
                _ArchivoCajero = sArchivoCajero;
                //_PathArchivoSolicitud = sPathArchivoSolicitud;
                //m_Watcher.EnableRaisingEvents = true;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Datafono inicializado correctamente";
            }
            catch (Exception e)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = e.Message;
            }
            return oResultadoOperacion;
        }

        /// <summary>
        /// Evento que monitorea que el archivo de respuesta haya sido escrito por el software de redeban (verifica que si haya algo escrito) retorna la respuesta por el evento: EventoRespuesta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name == _ArchivoRespuesta)
            {
                FileInfo fFile = new FileInfo(e.FullPath);
                while (IsFileLocked(fFile)) ;
                System.IO.StreamReader file = new System.IO.StreamReader(e.FullPath);
                string line = file.ReadLine();
                file.Close();
                if (line != null && line != string.Empty)
                {
                    this.EventoRespuesta(line);
                    m_Watcher.EnableRaisingEvents = false;
                }
            }
        }

        /// <summary>
        /// Evento auxiliar para verificar que los archivos se puedan leer y escribir
        /// </summary>
        /// <param name="file">Archivo a verificar</param>
        /// <returns></returns>
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Funcion que se encarga de escribir el archivo de solicitud con los datos del pago
        /// </summary>
        /// <param name="iOperacion">idOperacion</param>
        /// <param name="iMonto">Monto de la operacion</param>
        /// <param name="iIva">Iva de la operacion</param>
        /// <param name="sFactura">Numero de factura</param>
        /// <param name="iBaseDev">Vaslor de la base</param>
        /// <param name="iImpConsumidor">Valor del impuesto al consumidor</param>
        /// <param name="sCodigoCajero">Codigo del cajero</param>
        /// <returns>Retorna resultado operacion exito si escribe exitosamente el archivo</returns>
        public ResultadoOperacion PagarDatafono(int iOperacion, Int64 iMonto, Int64 iIva, string sFactura, Int64 iBaseDev, Int64 iImpConsumidor, string sCodigoCajero)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                //Quitar dato quemado
                string lines = iOperacion + ", " + iMonto + ", " + iIva + ", " + sFactura + ", " + iBaseDev + ", " + iImpConsumidor + ", " + sCodigoCajero;
                System.IO.StreamWriter file = new System.IO.StreamWriter(_ArchivoSolicitud);
                //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Datafono\\SOLICITUD.txt");
                file.WriteLine(lines);
                file.Close();
                m_Watcher.EnableRaisingEvents = true;
                //Quitar dato quemado
                Process.Start(_ArchivoCajero);
                //Process.Start("c:\\Datafono\\Cajas.exe");
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Datáfono habilitado";
            }
            catch (Exception e)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = e.Message;
            }
            return oResultadoOperacion;
        }

        public ResultadoOperacion AnularPagoDatafono(int Operacion, string Recibo, string Factura, string Clave, string CodigoCajero)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                string lines = Operacion + ", " + Recibo + ", " + Factura + ", " + Clave + ", " + CodigoCajero;
                System.IO.StreamWriter file = new System.IO.StreamWriter(_ArchivoSolicitud);
                file.WriteLine(lines);
                file.Close();
                m_Watcher.EnableRaisingEvents = true;
                Process.Start(_ArchivoCajero);
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Datáfono habilitado para anulaciones";
            }
            catch (Exception e)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = e.Message;
            }
            return oResultadoOperacion;
        }

    }
}
