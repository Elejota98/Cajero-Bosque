using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.Utilidades;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ds.AzcoyenDevice
{
    public class AzcoyenDeviceClass
    {
        public EventHandler DeviceMessageAzkState;
        public EventHandler DeviceMessageSerialPort;
        private StatesAzkDevice _StatesAzkoyen = new StatesAzkDevice();
        private Pay_Unit _Hopper = new Pay_Unit();
        private SerialPort _ComPort = new SerialPort();
        private string _Puerto = string.Empty;
        public string Puerto
        {
            get { return _Puerto; }
            set { _Puerto = value; }
        }
        private string _IdVlocal = string.Empty;
        private int Denominacion = 0;
        private List<TipoMoneda> _Coins = new List<TipoMoneda>();
        private string Ingresadas = string.Empty;
        private string AntesIngresadas = string.Empty;
        public ResultadoOperacion Conectar(string COM)
        {
            _StatesAzkoyen = StatesAzkDevice.Nothing;
            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                string sPuerto = string.Empty;

                sPuerto = COM;

                if (_ComPort.IsOpen == true)
                {
                    _ComPort.Close();
                }

                _ComPort.ReadTimeout = 5000;
                _ComPort.WriteTimeout = 5000;
                _ComPort.BaudRate = 9600;
                _ComPort.DataBits = 8;
                _ComPort.StopBits = StopBits.One;
                _ComPort.Parity = Parity.Mark;
                _ComPort.PortName = sPuerto;
                _ComPort.Open();

                _StatesAzkoyen = StatesAzkDevice.ConexionExitosa;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Conexion Exitosa Lector";
            }
            catch (Exception ex)
            {
                _StatesAzkoyen = StatesAzkDevice.ErrorConexion;
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error de conexion";
            }


            EventArgsAzkDevice e = new EventArgsAzkDevice(_StatesAzkoyen, _Coins, oResultadoOperacion);
            DeviceMessageAzkState(this, e);

            return oResultadoOperacion;
        }
        public void ConectarValidador(string IdV)
        {
            byte[] comBuffer = new byte[10];

            try
            {
                _IdVlocal = IdV;
                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdV)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("01")[0];

                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];


                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.Validador;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void Reset(string IdH, int H)
        {
            byte[] comBuffer = new byte[10];

            try
            {

                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdH)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("01")[0];

                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];
                if (H == 1)
                {
                    _Hopper = Pay_Unit.Hopper1;
                }
                else if (H == 2)
                {
                    _Hopper = Pay_Unit.Hopper2;
                }
                else if (H == 3)
                {
                    _Hopper = Pay_Unit.Hopper3;
                }
                else if (H == 4)
                {
                    _Hopper = Pay_Unit.Hopper4;
                }

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.Reset;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void ACK(string IdH, int H)
        {
            byte[] comBuffer = new byte[10];

            try
            {

                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdH)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("A3")[0];

                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];
                if (H == 1)
                {
                    _Hopper = Pay_Unit.Hopper1;
                }
                else if (H == 2)
                {
                    _Hopper = Pay_Unit.Hopper2;
                }
                else if (H == 3)
                {
                    _Hopper = Pay_Unit.Hopper3;
                }
                else if (H == 4)
                {
                    _Hopper = Pay_Unit.Hopper4;
                }

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.Test;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void HABILITAR(string IdH, int H)
        {
            byte[] comBuffer = new byte[10];

            try
            {

                byte[] paquete = new byte[6];
                paquete[0] = StringToByteArray(IdH)[0];
                paquete[1] = StringToByteArray("01")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("A4")[0];
                paquete[4] = StringToByteArray("A5")[0];

                string CHS = Checksum(paquete);

                paquete[5] = StringToByteArray(CHS)[0];
                if (H == 1)
                {
                    _Hopper = Pay_Unit.Hopper1;
                }
                else if (H == 2)
                {
                    _Hopper = Pay_Unit.Hopper2;
                }
                else if (H == 3)
                {
                    _Hopper = Pay_Unit.Hopper3;
                }
                else if (H == 4)
                {
                    _Hopper = Pay_Unit.Hopper4;
                }

                _StatesAzkoyen = StatesAzkDevice.Habilitar;

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.Habilitar;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void HabilitarRecepcion(string IdV)
        {
            byte[] comBuffer = new byte[20];

            try
            {

                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdV)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("E5")[0];
                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 20);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.HabilitarRecepcion;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void DesHabilitarRecepcion(string IdV)
        {
            byte[] comBuffer = new byte[20];
            _StatesAzkoyen = StatesAzkDevice.DesHabilitarRecepcion;
            try
            {

                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdV)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("01")[0];
                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 20);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.DesHabilitarRecepcion;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void WriteData(byte[] msg)
        {

            EventArgsAzkSerialCommunicationDevice evento = new EventArgsAzkSerialCommunicationDevice(TipoInsertEvento.Envio, msg);
            DeviceMessageSerialPort(this, evento);

            try
            {
                if (!(_ComPort.IsOpen == true))
                {
                    _ComPort.Open();
                }
                _ComPort.Write(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {

            }
        }
        public void ETD(string IdV)
        {
            byte[] comBuffer = new byte[10];

            try
            {

                byte[] paquete = new byte[5];
                paquete[0] = StringToByteArray(IdV)[0];
                paquete[1] = StringToByteArray("00")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("F8")[0];

                string CHS = Checksum(paquete);

                paquete[4] = StringToByteArray(CHS)[0];
                _StatesAzkoyen = StatesAzkDevice.EstadoValidador;

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.EstadoValidador;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }
        public void HTM(string IdV)
        {
            byte[] comBuffer = new byte[10];

            try
            {

                byte[] paquete = new byte[7];
                paquete[0] = StringToByteArray(IdV)[0];
                paquete[1] = StringToByteArray("02")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("E7")[0];
                paquete[4] = StringToByteArray("FF")[0];
                paquete[5] = StringToByteArray("FF")[0];


                string CHS = Checksum(paquete);

                paquete[6] = StringToByteArray(CHS)[0];
                _StatesAzkoyen = StatesAzkDevice.HabilitarMonedas;

                WriteData(paquete);
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.HabilitarMonedas;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {

            }
        }

        public ResultadoOperacion PayOut(int Denominacion, int valor, Pay_Unit oPay_Unit)
        {
            byte[] comBuffer = new byte[10];

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            string IdH = string.Empty;
            string sSerial = string.Empty;
            int Cantidad = 0;

            try
            {

                if (oPay_Unit == Pay_Unit.Hopper1)
                {
                    IdH = Globales.sIdH1;
                    sSerial = Globales.sSerialH1;

                }
                else if (oPay_Unit == Pay_Unit.Hopper2)
                {
                    IdH = Globales.sIdH2;
                    sSerial = Globales.sSerialH2;
                }
                else if (oPay_Unit == Pay_Unit.Hopper3)
                {
                    IdH = Globales.sIdH3;
                    sSerial = Globales.sSerialH3;
                }
                else if (oPay_Unit == Pay_Unit.Hopper4)
                {
                    //IdH = Globales.sIdH4;
                    //sSerial = Globales.sSerialH4;
                }

                Cantidad = valor / Denominacion;

                byte[] paquete = new byte[9];
                paquete[0] = StringToByteArray(IdH)[0];/// ID HOPPER
                paquete[1] = StringToByteArray("04")[0];
                paquete[2] = StringToByteArray("01")[0];
                paquete[3] = StringToByteArray("A7")[0];
                paquete[4] = StringToByteArray(sSerial.Substring(0, 2))[0];//SERIAL EQUIPO
                paquete[5] = StringToByteArray(sSerial.Substring(2, 2))[0];//SERIAL EQUIPO
                paquete[6] = StringToByteArray(sSerial.Substring(4, 2))[0];//SERIAL EQUIPO
                paquete[7] = StringToByteArray(Cantidad.ToString().PadLeft(2, '0'))[0]; // CANTIDAD DE MONEDAS

                string CHS = Checksum(paquete);

                paquete[8] = StringToByteArray(CHS)[0];

                #region TEST
                //byte[] paquete = new byte[9];
                //paquete[0] = StringToByteArray("04")[0];/// ID HOPPER
                //paquete[1] = StringToByteArray("04")[0];
                //paquete[2] = StringToByteArray("01")[0];
                //paquete[3] = StringToByteArray("A7")[0];
                //paquete[4] = StringToByteArray("CB")[0];//SERIAL EQUIPO
                //paquete[5] = StringToByteArray("6D")[0];//SERIAL EQUIPO
                //paquete[6] = StringToByteArray("24")[0];//SERIAL EQUIPO
                //paquete[7] = StringToByteArray("01")[0]; // CANTIDAD DE MONEDAS

                //string CHS = Checksum(paquete);

                //paquete[8] = StringToByteArray(CHS)[0];
                #endregion

                _StatesAzkoyen = StatesAzkDevice.Pagar;

                WriteData(paquete);
                oResultadoOperacion.EntidadDatos = valor;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "PayOut Exitoso";
                Thread.Sleep(200);
                _ComPort.Read(comBuffer, 0, 10);
                _ComPort.DiscardInBuffer();
                _StatesAzkoyen = StatesAzkDevice.Pagar;
                Actuar(comBuffer);
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "PayOut Error";
            }

            return oResultadoOperacion;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private string Checksum(byte[] Data)
        {
            string RData = string.Empty;

            try
            {
                byte[] buf = Data;

                int chkSum = buf.Aggregate(0, (s, b) => s += b) & 0xff;
                chkSum = (0x100 - chkSum) & 0xff;

                var str = chkSum.ToString("X2"); // <-- D9
                RData = str;


            }
            catch (Exception)
            {

            }

            return RData;
        }


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            int bytes = _ComPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            _ComPort.Read(comBuffer, 0, bytes);
            EventArgsAzkSerialCommunicationDevice evento = new EventArgsAzkSerialCommunicationDevice(TipoInsertEvento.Recepcion, comBuffer);
            DeviceMessageSerialPort(this, evento);
            //Actuar(comBuffer);
        }
        private void Actuar(byte[] recibo)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                string recepcion = string.Empty;

                if (recibo.Length >= 2)
                {
                    switch (_StatesAzkoyen)
                    {
                        #region Nothing
                        case StatesAzkDevice.Nothing:
                            break;
                        #endregion
                        #region Test
                        case StatesAzkDevice.Test:

                            bool bconexion = false;

                            for (int i = 0; i < recibo.Length; i++)
                            {
                                recepcion = string.Format("{0:X02}", (byte)recibo[i]);

                                switch (_Hopper)
                                {
                                    case Pay_Unit.Hopper1:
                                        if (recepcion == "80")
                                        {
                                            _StatesAzkoyen = StatesAzkDevice.Hopper1Ready;
                                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                            oResultadoOperacion.Mensaje = "Conexion Exitosa Hopper1";

                                            bconexion = true;
                                        }
                                        break;
                                    case Pay_Unit.Hopper2:
                                        if (recepcion == "80")
                                        {
                                            _StatesAzkoyen = StatesAzkDevice.Hopper2Ready;
                                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                            oResultadoOperacion.Mensaje = "Conexion Exitosa Hopper2";

                                            bconexion = true;
                                        }
                                        break;
                                    case Pay_Unit.Hopper3:
                                        if (recepcion == "80")
                                        {
                                            _StatesAzkoyen = StatesAzkDevice.Hopper3Ready;
                                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                            oResultadoOperacion.Mensaje = "Conexion Exitosa Hopper3";

                                            bconexion = true;
                                        }
                                        break;
                                    case Pay_Unit.Hopper4:
                                        if (recepcion == "80")
                                        {
                                            _StatesAzkoyen = StatesAzkDevice.Hopper4Ready;
                                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                            oResultadoOperacion.Mensaje = "Conexion Exitosa Hopper4";

                                            bconexion = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (!bconexion)
                            {
                                _StatesAzkoyen = StatesAzkDevice.ErrorConexionHopper;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Error Conexion Hoppers";
                            }



                            break;
                        #endregion
                        #region Reset
                        case StatesAzkDevice.Reset:
                            switch (_Hopper)
                            {
                                case Pay_Unit.Hopper1:
                                    _StatesAzkoyen = StatesAzkDevice.TestHopper1;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "TestHopper1 ok";
                                    break;
                                case Pay_Unit.Hopper2:
                                    _StatesAzkoyen = StatesAzkDevice.TestHopper2;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "TestHopper2 ok";
                                    break;
                                case Pay_Unit.Hopper3:
                                    _StatesAzkoyen = StatesAzkDevice.TestHopper3;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "TestHopper3 ok";
                                    break;
                                case Pay_Unit.Hopper4:
                                    _StatesAzkoyen = StatesAzkDevice.TestHopper4;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "TestHopper4 ok";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        #endregion
                        #region Habilitar
                        case StatesAzkDevice.Habilitar:
                            switch (_Hopper)
                            {
                                case Pay_Unit.Hopper1:
                                    _StatesAzkoyen = StatesAzkDevice.Hopper1Habilitado;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Hopper1 Habilitado";
                                    break;
                                case Pay_Unit.Hopper2:
                                    _StatesAzkoyen = StatesAzkDevice.Hopper2Habilitado;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Hopper2 Habilitado";
                                    break;
                                case Pay_Unit.Hopper3:
                                    _StatesAzkoyen = StatesAzkDevice.Hopper3Habilitado;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Hopper3 Habilitado";
                                    break;
                                case Pay_Unit.Hopper4:
                                    _StatesAzkoyen = StatesAzkDevice.Hopper4Habilitado;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "Hopper4 Habilitado";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        #endregion
                        #region Validador
                        case StatesAzkDevice.Validador:
                            ETD(_IdVlocal);
                            break;
                        #endregion
                        #region Estado Validador
                        case StatesAzkDevice.EstadoValidador:
                            HTM(_IdVlocal);
                            break;
                        #endregion
                        #region Habilitar Monedas
                        case StatesAzkDevice.HabilitarMonedas:
                            _StatesAzkoyen = StatesAzkDevice.ValidadorReady;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Conexion Exitosa ValidadorReady";
                            break;
                        #endregion
                        #region ValidadorReady
                        case StatesAzkDevice.ValidadorReady:
                            _StatesAzkoyen = StatesAzkDevice.Nothing;
                            break;
                        #endregion
                        #region Habilitar Recepcion
                        case StatesAzkDevice.HabilitarRecepcion:
                            _StatesAzkoyen = StatesAzkDevice.HabilitarRecepcion;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "HabilitarRecepcion ok";
                            _Coins = new List<TipoMoneda>();
                            Denominacion = 0;
                            recepcion = string.Format("{0:X02}", (byte)recibo[5]);
                            Ingresadas = string.Format("{0:X02}", (byte)recibo[4]);


                            if (recepcion == "03" || recepcion == "04" || recepcion == "0A")//$50
                            {
                                Denominacion = 50;
                            }
                            else if (recepcion == "05" || recepcion == "0B")//$100
                            {
                                Denominacion = 100;
                            }
                            else if (recepcion == "06" || recepcion == "0C")//$200
                            {
                                Denominacion = 200;
                            }
                            else if (recepcion == "07" || recepcion == "0D")//$500
                            {
                                Denominacion = 500;
                            }
                            else if (recepcion == "08" || recepcion == "0E")//$1.000
                            {
                                Denominacion = 1000;
                            }

                            if (Denominacion > 0)
                            {
                                if (Ingresadas != "00")
                                {
                                    if (Ingresadas != AntesIngresadas)
                                    {
                                        AntesIngresadas = Ingresadas;
                                        _Coins.Add(new TipoMoneda(Denominacion, 1));
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region DesHabilitarRecepcion
                        case StatesAzkDevice.DesHabilitarRecepcion:
                            _StatesAzkoyen = StatesAzkDevice.DesHabilitarRecepcion;
                            break;
                        #endregion
                        default:
                            break;
                    }



                    //string recepcion = string.Format("{0:X02}", (byte)recibo[5]) + string.Format("{0:X02}", (byte)recibo[6]) + string.Format("{0:X02}", (byte)recibo[7]);
                    //MensajesEstado(recepcion);
                }
            }
            catch (Exception)
            {
                _StatesAzkoyen = StatesAzkDevice.ErrorConexionHopper;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error Conexion Hoppers";
            }

            EventArgsAzkDevice e = new EventArgsAzkDevice(_StatesAzkoyen, _Coins, oResultadoOperacion);
            DeviceMessageAzkState(this, e);
        }
    }

    public class EventArgsAzkDevice : EventArgs
    {
        private StatesAzkDevice _result;

        public StatesAzkDevice result
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

        private List<TipoMoneda> _CoinsCtCoin = new List<TipoMoneda>();
        public List<TipoMoneda> CoinsCtCoin
        {
            get { return _CoinsCtCoin; }
            set { _CoinsCtCoin = value; }
        }

        public EventArgsAzkDevice(StatesAzkDevice oStatesAzkoyen, List<TipoMoneda> lstTipoMoneda, ResultadoOperacion oResultadoOperacion)
        {
            _result = oStatesAzkoyen;
            _resultString = oResultadoOperacion;
            _CoinsCtCoin = lstTipoMoneda;
        }
    }

    public class TipoMoneda
    {
        public TipoMoneda(int iDenominacion, int iCantidad)
        {
            _Denominacion = iDenominacion;
            _Cantidad = iCantidad;
        }

        private int _Denominacion = 0;

        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        private int _Cantidad = 0;

        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }
    }

    public class EventArgsAzkSerialCommunicationDevice : EventArgs
    {
        private TipoInsertEvento _TipoInsertEvento;
        private byte[] _ByteArray;

        public TipoInsertEvento TipoInsertEvento
        {
            get { return _TipoInsertEvento; }
            set { _TipoInsertEvento = value; }
        }

        public byte[] ByteArray
        {
            get { return _ByteArray; }
            set { _ByteArray = value; }
        }

        public EventArgsAzkSerialCommunicationDevice(TipoInsertEvento oTipoInsertEvento, byte[] oByteArray)
        {
            _TipoInsertEvento = oTipoInsertEvento;
            _ByteArray = oByteArray;
        }
    }
}
