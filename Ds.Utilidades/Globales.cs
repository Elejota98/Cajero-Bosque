using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ds.Utilidades
{
    public class Globales
    {

        public static int bth
        {
            get
            {
                string _bth = ConfigurationManager.AppSettings["bth"];
                if (!string.IsNullOrEmpty(_bth))
                {
                    return Convert.ToInt32(_bth);
                }
                else
                {
                    return 8;
                }
            }
        }

        public static string Nombre
        {
            get
            {
                string _nombre = ConfigurationManager.AppSettings["nombre"];
                if (!string.IsNullOrEmpty(_nombre))
                {
                    return _nombre;
                }
                else
                {
                    return "COM2";
                }
            }
        }

        public static string Par
        {
            get
            {
                string _Paridad = ConfigurationManager.AppSettings["par"];
                if (!string.IsNullOrEmpty(_Paridad))
                {
                    return _Paridad;
                }
                else
                {
                    return "0";
                }
            }
        }

        public static string sBits
        {
            get
            {
                string _sBits = ConfigurationManager.AppSettings["sBits"];
                if (!string.IsNullOrEmpty(_sBits))
                {
                    return _sBits;
                }
                else
                {
                    return "1";
                }
            }
        }

        public static string dBits
        {
            get
            {
                string _dBits = ConfigurationManager.AppSettings["dBits"];
                if (!string.IsNullOrEmpty(_dBits))
                {
                    return _dBits;
                }
                else
                {
                    return "8";
                }
            }
        }

        public static string baud
        {
            get
            {
                string _baud = ConfigurationManager.AppSettings["baud"];
                if (!string.IsNullOrEmpty(_baud))
                {
                    return _baud;
                }
                else
                {
                    return "9600";
                }
            }
        }

        public static string sDispenseOption
        {
            get
            {
                string sDispenseOption = ConfigurationManager.AppSettings["DispenseOption"];
                if (!string.IsNullOrEmpty(sDispenseOption))
                {
                    return sDispenseOption;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sTestMode
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["TestMode"];
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

        public static string sIdCajeroAgro
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["IdCajeroAgro"];
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

        public static string sRutaFotos
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["rutaFotos"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static int iEspera
        {
            get
            {
                string iTiempo = ConfigurationManager.AppSettings["TiempoEspera"];
                if (!string.IsNullOrEmpty(iTiempo))
                {
                    return Convert.ToInt32(iTiempo);
                }
                else
                {
                    return 0;
                }
            }
        } 

        public static string sRegion
        {
            get
            {
                string sRegion = ConfigurationManager.AppSettings["ID_Region"];
                if (!string.IsNullOrEmpty(sRegion))
                {
                    return sRegion;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sCentro
        {
            get
            {
                string sCentro = ConfigurationManager.AppSettings["ID_Centro"];
                if (!string.IsNullOrEmpty(sCentro))
                {
                    return sCentro;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sSEC
        {
            get
            {
                string sSEC = ConfigurationManager.AppSettings["SEC"];
                if (!string.IsNullOrEmpty(sSEC))
                {
                    return sSEC;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sSecuencia_Virtual
        {
            get
            {
                string sSecuencia_Virtual = ConfigurationManager.AppSettings["Secuencia_Virtual"];
                if (!string.IsNullOrEmpty(sSecuencia_Virtual))
                {
                    return sSecuencia_Virtual;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public static string sPuertoAzk
        {
            get
            {
                string sPuertoAzk = ConfigurationManager.AppSettings["PuertoAzk"];
                if (!string.IsNullOrEmpty(sPuertoAzk))
                {
                    return sPuertoAzk;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sIdH1
        {
            get
            {
                string sIdH1 = ConfigurationManager.AppSettings["IdH1"];
                if (!string.IsNullOrEmpty(sIdH1))
                {
                    return sIdH1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sIdH2
        {
            get
            {
                string sIdH2 = ConfigurationManager.AppSettings["IdH2"];
                if (!string.IsNullOrEmpty(sIdH2))
                {
                    return sIdH2;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sIdH3
        {
            get
            {
                string sIdH3 = ConfigurationManager.AppSettings["IdH3"];
                if (!string.IsNullOrEmpty(sIdH3))
                {
                    return sIdH3;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sSerialH1
        {
            get
            {
                string sSerialH1 = ConfigurationManager.AppSettings["SerialH1"];
                if (!string.IsNullOrEmpty(sSerialH1))
                {
                    return sSerialH1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sSerialH2
        {
            get
            {
                string sSerialH2 = ConfigurationManager.AppSettings["SerialH2"];
                if (!string.IsNullOrEmpty(sSerialH2))
                {
                    return sSerialH2;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sSerialH3
        {
            get
            {
                string sSerialH3 = ConfigurationManager.AppSettings["SerialH3"];
                if (!string.IsNullOrEmpty(sSerialH3))
                {
                    return sSerialH3;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sIdV
        {
            get
            {
                string sIdV = ConfigurationManager.AppSettings["IdV"];
                if (!string.IsNullOrEmpty(sIdV))
                {
                    return sIdV;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Establece si está habilitado el logging de la aplicación
        /// </summary>
        public static bool bEnabledTracking
        {
            get
            {
                string sEnabledTracking = ConfigurationManager.AppSettings["EnabledTracking"];
                if (!string.IsNullOrEmpty(sEnabledTracking))
                {
                    return Convert.ToBoolean(sEnabledTracking);
                }
                else
                {
                    return false;
                }
            }
        }

        public static string sRutaLog
        {
            get { return ConfigurationManager.AppSettings["LogFilePath"]; }
        }

        public static string sBilleteroSerialPuerto
        {
            get
            {
                string sIdePortSerial = ConfigurationManager.AppSettings["BilleteroSerialPort"];
                if (!string.IsNullOrEmpty(sIdePortSerial))
                {
                    return sIdePortSerial;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sOPCServer
        {
            get
            {
                string sDir = ConfigurationManager.AppSettings["opcServerName"];
                if (!string.IsNullOrEmpty(sDir))
                {
                    return sDir;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sOPCEstado
        {
            get
            {
                string sloc = ConfigurationManager.AppSettings["opcVarReadEstado"];
                if (!string.IsNullOrEmpty(sloc))
                {
                    return sloc;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sOPCLuz
        {
            get
            {
                string sloc = ConfigurationManager.AppSettings["opcVarWriteLuz"];
                if (!string.IsNullOrEmpty(sloc))
                {
                    return sloc;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sOPCAlarma
        {
            get
            {
                string sloc = ConfigurationManager.AppSettings["opcVarWriteAlarma"];
                if (!string.IsNullOrEmpty(sloc))
                {
                    return sloc;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sSerial
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["IdCajero"];
                if (!string.IsNullOrEmpty(sSerial))
                {
                    return sSerial;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sPuertoBill
        {
            get
            {
                string sPuertoBill = ConfigurationManager.AppSettings["PuertoBill"];
                if (!string.IsNullOrEmpty(sPuertoBill))
                {
                    return sPuertoBill;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sPuertoDTF
        {
            get
            {
                string sPuertoDTF = ConfigurationManager.AppSettings["PuertoDTF"];
                if (!string.IsNullOrEmpty(sPuertoDTF))
                {
                    return sPuertoDTF;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sPuertoCRC
        {
            get
            {
                string sPuertoCRC = ConfigurationManager.AppSettings["PuertoCRC"];
                if (!string.IsNullOrEmpty(sPuertoCRC))
                {
                    return sPuertoCRC;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sPuertoPLC
        {
            get
            {
                string sPuertoPLC = ConfigurationManager.AppSettings["PuertoPLC"];
                if (!string.IsNullOrEmpty(sPuertoPLC))
                {
                    return sPuertoPLC;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sTerminal
        {
            get
            {
                string sTerminal = ConfigurationManager.AppSettings["Terminal"];
                if (!string.IsNullOrEmpty(sTerminal))
                {
                    return sTerminal;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sNumeroCajero
        {
            get
            {
                string sNumeroCajero = ConfigurationManager.AppSettings["NumeroCajero"];
                if (!string.IsNullOrEmpty(sNumeroCajero))
                {
                    return sNumeroCajero;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sDisplayName
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["DisplayName"];
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
        public static string sUrlMonitoreo
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["UrlMonitoreo"];
                if (!string.IsNullOrEmpty(sIdeModulo))
                {
                    return sIdeModulo;
                }
                else
                {
                    return string.Empty; ;
                }
            }
        }
        public static string sTiempoSincronizacion
        {
            get
            {
                string sTiempoSincronizacion = ConfigurationManager.AppSettings["TiempoSincronizacionMilisegundos"];
                if (!string.IsNullOrEmpty(sTiempoSincronizacion))
                {
                    return sTiempoSincronizacion;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sPuertoArduino
        {
            get
            {
                string sPuertoArduino = ConfigurationManager.AppSettings["PuertoArduino"];
                if (!string.IsNullOrEmpty(sPuertoArduino))
                {
                    return sPuertoArduino;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public static string iCodigoEstacionamiento
        {
            get
            {
                string iCodigoEstacionamiento = ConfigurationManager.AppSettings["CodigoEstacionamiento"];
                if (!string.IsNullOrEmpty(iCodigoEstacionamiento))
                {
                    return iCodigoEstacionamiento;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sPuertoCtCoin
        {
            get
            {
                string sPuertoCtCoin = ConfigurationManager.AppSettings["PuertoCtCoin"];
                if (!string.IsNullOrEmpty(sPuertoCtCoin))
                {
                    return sPuertoCtCoin;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sNombreImpresoraSticker
        {
            get
            {
                string sNombreImpresoraSticker = ConfigurationManager.AppSettings["NombreImpresoraSticker"];
                if (!string.IsNullOrEmpty(sNombreImpresoraSticker))
                {
                    return sNombreImpresoraSticker;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sNombreImpresoraTickets
        {
            get
            {
                string sNombreImpresoraTickets = ConfigurationManager.AppSettings["NombreImpresoraTickets"];
                if (!string.IsNullOrEmpty(sNombreImpresoraTickets))
                {
                    return sNombreImpresoraTickets;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sCountry
        {
            get
            {
                string sCountry = ConfigurationManager.AppSettings["IdCountry"];
                if (!string.IsNullOrEmpty(sCountry))
                {
                    return sCountry;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sCity
        {
            get
            {
                string sCity = ConfigurationManager.AppSettings["IdCity"];
                if (!string.IsNullOrEmpty(sCity))
                {
                    return sCity;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string sProducto
        {
            get
            {
                string sProducto = ConfigurationManager.AppSettings["IdProducto"];
                if (!string.IsNullOrEmpty(sProducto))
                {
                    return sProducto;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public static string sRutaConfirmar
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["RutaConfirmar"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sRutaArchivoImprimir
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["RutaArchivoImprimir"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sRutaCopiar
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["RutaCopiar"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sQuery
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["Query"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sQueryFactura
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["QueryFactura"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string sConnection
        {
            get
            {
                string sRuta = ConfigurationManager.AppSettings["ConnectionString"];
                if (!string.IsNullOrEmpty(sRuta))
                {
                    return sRuta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public static bool bPrinterRequired
        {
            get
            {
                string sPrinter = ConfigurationManager.AppSettings["printerRequired"];
                if (!string.IsNullOrEmpty(sPrinter))
                {
                    return Convert.ToBoolean(sPrinter);
                }
                else
                {
                    return false;
                }
            }
        }

        public static string sRutaAlmacenamiento
        {
            get { return ConfigurationManager.AppSettings["PathServer"]; }
        }

      

    }
}
