using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;


namespace Ds.PrinterDevice
{
    public class Printer
    {
        [DllImport(@"Config Library/nano_com.dll")]
        public static extern int nanoptixPrinterOpen(string prmDeviceName, byte prmDeviceType);

        [DllImport(@"Config Library/nano_com.dll")]
        public static extern int nanoptixPrinterClose();

        [DllImport(@"Config Library/nano_com.dll")]
        public static extern int nanoptixPrinterStatus(byte[] varString, ref long varLengthRead);

        [DllImport(@"Config Library/nano_com.dll")]
        public static extern int nanoptixProcessStatusString(ref string prmStatusString, long prmStatusLength, ref long prmStatus);

        long varLengthRead;
        int varResult;
        byte[] varString = new byte[29];
        string puerto;

        public ResultadoOperacion ObtenerEstadoDispositivoImpresora()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion.Mensaje = string.Empty;
            oResultadoOperacion.oEstado = TipoRespuesta.Error;

            try
            {

                for (int i = 1; i < 9; i++)
                {
                    puerto = ("USB00" + i.ToString()).Trim();
                    varResult = nanoptixPrinterOpen(puerto, 0);
                    if (varResult == 0)
                    {
                        break;
                    }
                }

                varResult = nanoptixPrinterStatus(varString, ref varLengthRead);
                varResult = nanoptixPrinterClose();

                if (varResult != 0)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    if (varResult > 171 && varResult < 187)
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.ImpresoraDesconectada.ToString();
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.ImpresoraDesconectada;
                    }
                    else if (varResult == 170)
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.ModoDispositivoInvalido + " - " + varResult;
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.ModoDispositivoInvalido;
                    }
                    else if (varResult == 203)
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.ErrorLecturaEstadoDispositivo + " - " + varResult;
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.ErrorLecturaEstadoDispositivo;
                    }
                    else
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.ErrorDesconocido + " - " + varResult;
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.ErrorDesconocido;
                    }
                }
                else
                {
                    if (varString[15] == 84 && varString[19] == 66 && varString[21] == 72 && varString[23] == 64) //no hay papel en boca pero hay en el eje
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.AtascamientoPapel.ToString();
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.AtascamientoPapel;
                        
                    }
                    else if (varString[15] == 84 && varString[19] == 66 && varString[21] == 73 && varString[23] == 64) //no hay papel 
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.ImpresoraSinPapel.ToString();
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.ImpresoraSinPapel;
                       
                    }
                    else if (varString[15] == 64 && varString[19] == 64 && varString[21] == 73 && varString[23] == 64) // papel bajo o fuera del eje
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.NivelBajoPapel.ToString();
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.NivelBajoPapel;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    }
                    else
                    {
                        oResultadoOperacion.Mensaje = EstadoImpresora.OK.ToString();
                        oResultadoOperacion.EntidadDatos = EstadoImpresora.OK;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    }
                }

            }
            catch (Exception)
            {
                oResultadoOperacion.Mensaje = "Error, No disponible";
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

    }
}
