using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredibancoDevice
{
    public class DatafonoDeviceClass
    {
        public EventHandler DeviceMessageDatafonoState;
        StatesDatafono _StatesDatafono = new StatesDatafono();

        public ResultadoOperacion ConectarDatafono(string sPuerto)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                using (SerialPort port = new SerialPort(sPuerto))
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                    }

                    string Com = sPuerto;
                    port.PortName = Com;
                    port.BaudRate = 9600;
                    port.Parity = System.IO.Ports.Parity.None;
                    port.DataBits = 8;
                    port.Open();
                }

                _StatesDatafono = StatesDatafono.Conexion_Exitosa;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Conexion Exitosa Datafono";
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.Error_Conexion;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion DesconectarLector(string sPuerto)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                using (SerialPort port = new SerialPort(sPuerto))
                {
                    port.Close();
                }

                _StatesDatafono = StatesDatafono.Desconexion_Exitosa;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Desconexion Exitosa Datafono";
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion IniciarTransaccion(double ValorPago, double Iva, string IdModulo, string IdTransaccion, string Terminal)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
               // oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[I,01,10000,0,T0501,000CRE01,1000,0,010601557,0,PRU]46");

                string temp = string.Empty;

                //20210915051907132

                if (IdTransaccion.Length > 10)
                {
                    temp = IdTransaccion.Substring(2, 10);
                }
                else 
                {
                    temp = IdTransaccion;
                }
                
                int Valor = Convert.ToInt32(ValorPago);
                int Ivanew = Convert.ToInt32(Iva);

                char CC = CalculateLRC("I,01," + Valor + "," + Ivanew + "," + IdModulo + "," + Terminal + "," + temp + ",0,010601557,0," + IdModulo + "]");
                int Resl = Convert.ToInt32(CC);
                //CONVERTIR CC A HEXA
                string hexValue = Resl.ToString("X");

                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[I,01," + Valor + "," + Ivanew + "," + IdModulo + ",000CRE01," + temp + ",0,010601557,0," + IdModulo + "]" + hexValue + "");

               // [IdentificadorInicioTx,IdentificadoTipoperación,ValorTotalCompra,ValorIVA,NumeroKiosko,NmTerminal,NmTransacción,Propina,Codunico,ValorIAC,IdCajero] 

                if (oResultadoOperacion.EntidadDatos.ToString().Length >= 43)
                {
                    if (oResultadoOperacion.EntidadDatos.ToString().Substring(0, 43) == "No se encuentra el dispositivo en el puerto")
                    {
                        _StatesDatafono = StatesDatafono.ErrorIniciar;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
                    }
                    else
                    {
                        _StatesDatafono = StatesDatafono.Inicializarok;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Inicializacion Exitosa Datafono";
                    }
                }
                else 
                {
                    if (oResultadoOperacion.EntidadDatos.ToString() == "DATAFONO NO RESPONDE")
                    {
                        _StatesDatafono = StatesDatafono.ErrorIniciar;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorIniciar;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;
            
        }
        public ResultadoOperacion InserteTarjeta()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,64,1]3C");

                


                if (oResultadoOperacion.EntidadDatos.ToString() == "DATAFONO NO RESPONDE")
                {
                    _StatesDatafono = StatesDatafono.ErrorLeerTarjeta;
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
                }
                else
                {
                    string[] result = oResultadoOperacion.EntidadDatos.ToString().Split(',');

                    if (result[2] == "Tipo de Cuenta?")
                    {
                        _StatesDatafono = StatesDatafono.TarjetaLeida;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = result[3].ToString();
                    }
                    else if (result[3] == "Ingrese Cuotas")
                    {
                        _StatesDatafono = StatesDatafono.TarjetaLeida;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = result[3].ToString();
                    }
                    else if(result[2] == "00")
                    {
                        _StatesDatafono = StatesDatafono.IngresoCuotas;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Cobro Datafono ok";
                    }
                    else
                    {
                        _StatesDatafono = StatesDatafono.ErrorLeerTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorLeerTarjeta;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }


            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);


            return oResultadoOperacion;

        }
        public ResultadoOperacion ProcesoAhorros()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,61,1]39");

                string[] result = oResultadoOperacion.EntidadDatos.ToString().Split(',');


                if (result[2].ToString() == "00")
                {
                    _StatesDatafono = StatesDatafono.RespuestaAhorros;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Tarjeta leida Datafono";
                }
                else
                {
                    _StatesDatafono = StatesDatafono.ProcesoAhorros;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Tarjeta leida Datafono";
                }
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorRespuestaAhorros;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion ProcesoCorriente()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,61,2]3A");

                string[] result = oResultadoOperacion.EntidadDatos.ToString().Split(',');


                if (result[2].ToString() == "00")
                {
                    _StatesDatafono = StatesDatafono.RespuestaAhorros;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Tarjeta leida Datafono";
                }
                else
                {
                    _StatesDatafono = StatesDatafono.ProcesoAhorros;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Tarjeta leida Datafono";
                }
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorRespuestaAhorros;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion RespuestaAhorros()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("");



                _StatesDatafono = StatesDatafono.RespuestaAhorros;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Tarjeta leida Datafono";
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorRespuestaFinal;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion ProcesoCredito()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,61,3]3B");

                string Response = oResultadoOperacion.EntidadDatos.ToString();
                string[] Result = Response.Split(',');

                _StatesDatafono = StatesDatafono.ProcesoCredito;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = Result[3].ToString();
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorRespuestaCredito;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion IngresoDigitos(string Digitos)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {

                char CC = CalculateLRC("R,60,1," + Digitos + "]");
                int Resl = Convert.ToInt32(CC);
                //CONVERTIR CC A HEXA
                string hexValue = Resl.ToString("X");

                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,60,1," + Digitos + "]" + hexValue + "");
                

                _StatesDatafono = StatesDatafono.IngresoDigitos;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Ingreso Digitos Datafono ok";
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorIngresoDigitos;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion IngresoCuotas(string Cuotas)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();

                char CC = CalculateLRC("R,60,1," + Cuotas + "]");
                int Resl = Convert.ToInt32(CC);
                //CONVERTIR CC A HEXA
                string hexValue = Resl.ToString("X");

                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,60,1," + Cuotas + "]" + hexValue + "");



                _StatesDatafono = StatesDatafono.IngresoCuotas;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Ingreso Cuotas Datafono ok";
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorIngresoCuotas;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;

        }
        public ResultadoOperacion CanelarTipoCuenta()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                char CC = CalculateLRC("R,61,0]");
                int Resl = Convert.ToInt32(CC);
                string hexValue = Resl.ToString("X");
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();

                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,61,0]" + hexValue + "");

                _StatesDatafono = StatesDatafono.CancelacionTipoCuenta;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorCancelacionTipoCuenta;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion CanelarCuotas()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            try
            {
                char CC = CalculateLRC("R,60,0]");
                int Resl = Convert.ToInt32(CC);
                //CONVERTIR CC A HEXA
                string hexValue = Resl.ToString("X");
                
                trx.TEFTransactionManager oCLIENT = new trx.TEFTransactionManager();
                oResultadoOperacion.EntidadDatos = oCLIENT.getTEFAuthorization("[R,60,0]" + hexValue + "");

                _StatesDatafono = StatesDatafono.CancelacionCuotas;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = oResultadoOperacion.EntidadDatos.ToString();
            }
            catch (Exception ex)
            {
                _StatesDatafono = StatesDatafono.ErrorCancelacionCuotas;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsDatafonoDevice e = new EventArgsDatafonoDevice(_StatesDatafono, oResultadoOperacion);
            DeviceMessageDatafonoState(this, e);

            return oResultadoOperacion;
        }
        public static char CalculateLRC(string toEncode)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(toEncode);
            byte LRC = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                LRC ^= bytes[i];
            }
            return Convert.ToChar(LRC);
        }
    }

    public class EventArgsDatafonoDevice : EventArgs
    {
        private StatesDatafono _result;

        public StatesDatafono result
        {
            get { return _result; }
            set { _result = value; }
        }

        private ResultadoOperacion _resultString;

        public ResultadoOperacion resultString
        {
            get { return _resultString; }
            set { _resultString = value; }
        }

        public EventArgsDatafonoDevice(StatesDatafono oStatesDatafono, ResultadoOperacion oResultadoOperacion)
        {
            _result = oStatesDatafono;
            _resultString = oResultadoOperacion;
        }
    }
}
