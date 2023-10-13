using Ds.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Ds.CashPaymentDevice
{
    public class BillToBillDevice
    {
        public EventHandler DeviceMessageBillToBillState;
        public EventHandler DeviceMessageSerialPort;
        private SerialPort _ComPort = new SerialPort();
        private List<string> _BillTableList = new List<string>();
        public List<string> BillTableList
        {
            get { return _BillTableList; }
            set { _BillTableList = value; }
        }
        private List<string> _Denominaciones = new List<string>();
        public List<string> Denominaciones
        {
            get { return _Denominaciones; }
            set { _Denominaciones = value; }
        }
        public string _Puerto = string.Empty;
        private StatesBillToBillDevice _CurrentState = StatesBillToBillDevice.Nothing;
        public StatesBillToBillDevice CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                EventArgsBillToBillEventsDevice evento = new EventArgsBillToBillEventsDevice(value, _ValorAdicional, _Parte, _Dispensado, _TipoFalla, _TipoRechazo);
                DeviceMessageBillToBillState(this, evento);
            }
        }
        private int _IndiceDispensar = 0;
        private bool _Disable = false;
        private Int64 _ValorAdicional = 0;
        private string _Parte = string.Empty;
        private List<int> _Dispensado = new List<int>();
        List<int[]> _AcumuladoPagos = new List<int[]>();
        private bool _DescargaTodos = false;
        private int _IndiceDescargaCassette = 0;
        private bool _Enable = false;
        public bool Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }
        private string _TipoFalla;
        public string TipoFalla
        {
            get { return _TipoFalla; }
            set { _TipoFalla = value; }
        }
        private string _TipoRechazo;
        public string TipoRechazo
        {
            get { return _TipoRechazo; }
            set { _TipoRechazo = value; }
        }
        private string _DeviceType = string.Empty;
        public string DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }
        private int _SecondDisable = 1;
        private List<int> _DenominacionTemporal = new List<int>();

        public BillToBillDevice()
        {
            //_ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }
        public bool Conectar(List<string> lsDenominaciones)
        {
            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            //_DeviceType = dt;
            _Enable = true;
            _DescargaTodos = false;
            _IndiceDescargaCassette = 0;
            CurrentState = StatesBillToBillDevice.Nothing;
            //_Puerto = sPuerto;
            Denominaciones = lsDenominaciones;
            byte[] comBuffer = new byte[2];

            bool bAbrePuerto = false;

            for (int i = 1; i < 10; i++)
            {
                if (OpenPort("COM" + i))
                {
                    try
                    {
                        _DeviceType = "BB";
                        Poll();
                        Thread.Sleep(200);
                        _ComPort.Read(comBuffer, 0, 2);
                        _ComPort.DiscardInBuffer();
                        if (string.Format("{0:X02}", (byte)comBuffer[0]) == "02" && string.Format("{0:X02}", (byte)comBuffer[1]) == "01")
                        {
                            bAbrePuerto = true;
                            _Puerto = "COM" + i;
                            _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            Thread.Sleep(500);
                            Poll();
                            break;
                        }
                        else
                        {
                            bAbrePuerto = false;
                            ClosePort();
                        }
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            _DeviceType = "BV";
                            Poll();
                            Thread.Sleep(200);
                            _ComPort.Read(comBuffer, 0, 2);
                            _ComPort.DiscardInBuffer();
                            if (string.Format("{0:X02}", (byte)comBuffer[0]) == "02" && string.Format("{0:X02}", (byte)comBuffer[1]) == "03")
                            {
                                bAbrePuerto = true;
                                _Puerto = "COM" + i;
                                _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                Thread.Sleep(500);
                                Poll();
                                break;
                            }
                            else
                            {
                                bAbrePuerto = false;
                                ClosePort();
                            }
                        }
                        catch (Exception e2)
                        {
                            bAbrePuerto = false;
                            ClosePort();
                        }
                    }
                }
                else
                {
                    bAbrePuerto = false;
                }
            }

            return bAbrePuerto;
        }
        public bool Conectar()
        {
            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            _Enable = true;
            _DescargaTodos = false;
            _IndiceDescargaCassette = 0;
            CurrentState = StatesBillToBillDevice.Nothing;
            byte[] comBuffer = new byte[2];

            bool bAbrePuerto = false;

            for (int i = 1; i < 10; i++)
            {
                if (OpenPort("COM" + i))
                {
                    try
                    {
                        _DeviceType = "BV";
                        Poll();
                        Thread.Sleep(200);
                        _ComPort.Read(comBuffer, 0, 2);
                        _ComPort.DiscardInBuffer();
                        if (string.Format("{0:X02}", (byte)comBuffer[0]) == "02" && string.Format("{0:X02}", (byte)comBuffer[1]) == "03")
                        {
                            bAbrePuerto = true;
                            _Puerto = "COM" + i;
                            _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            Thread.Sleep(500);
                            Poll();
                            break;
                        }
                        else
                        {
                            bAbrePuerto = false;
                            ClosePort();
                        }
                    }
                    catch (Exception e2)
                    {
                        bAbrePuerto = false;
                        ClosePort();
                    }
                }
                else
                {
                    bAbrePuerto = false;
                }
            }

            return bAbrePuerto;
        }
        public bool Desconectar()
        {
            bool bCierraPuerto = false;

            if (ClosePort())
            {
                bCierraPuerto = true;
            }
            else
            {
                bCierraPuerto = false;
            }

            return bCierraPuerto;
        }
        private void Actuar(byte[] recibo)
        {
            lock (this)
            {
                if (recibo.Length > 4)
                {
                    string recepcion = string.Format("{0:X02}", (byte)recibo[3]);

                    if (recepcion != ComandosRecepcion.ACK)
                    {
                        ACK();
                    }

                    switch (recepcion)
                    {
                        case ComandosRecepcion.ACK:
                            if (CurrentState == StatesBillToBillDevice.Nothing || CurrentState == StatesBillToBillDevice.PowerUp)
                            {
                                Thread.Sleep(2000);
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Inicializing)
                            {
                                if (_DeviceType == DeviceTypeEnum.BB.ToString())
                                {
                                    CurrentState = StatesBillToBillDevice.SettingCassette1;
                                    ConfigurarCassettes(1);
                                }
                                else if (_DeviceType == DeviceTypeEnum.BV.ToString())
                                {
                                    CurrentState = StatesBillToBillDevice.EndInicialization;
                                    Poll();
                                }
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassette1 || CurrentState == StatesBillToBillDevice.SettingCassette2 || CurrentState == StatesBillToBillDevice.SettingCassette3)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Disable)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Disabling)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Dispensed)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.CassetteUnloaded)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassetteCapacity1 || CurrentState == StatesBillToBillDevice.SettingCassetteCapacity2 || CurrentState == StatesBillToBillDevice.SettingCassetteCapacity3)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Idling)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Dispensing)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.PreAcepting || CurrentState == StatesBillToBillDevice.PreAceptingAll)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.StartSendingOptions)
                            {
                                CurrentState = StatesBillToBillDevice.SendingOptions;
                                EnviarOpciones();
                            }
                            else if (CurrentState == StatesBillToBillDevice.SendingOptions)
                            {
                                CurrentState = StatesBillToBillDevice.EndInicialization;
                                Poll();
                            }
                            else
                            {

                            }
                            break;
                        case ComandosRecepcion.Encendiendo:
                            Reset();
                            CurrentState = StatesBillToBillDevice.PowerUp;
                            break;
                        case ComandosRecepcion.EncendiendoBilleteEnValidador:
                            break;
                        case ComandosRecepcion.EncendiendoBilleteEnChasis:
                            break;
                        case ComandosRecepcion.Inicializando:
                            Poll();
                            CurrentState = StatesBillToBillDevice.Inicializing;
                            break;
                        case ComandosRecepcion.Recibiendo:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Deshabilitar();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Disabling)
                            {
                                Deshabilitar();
                            }
                            else
                            {
                                CurrentState = StatesBillToBillDevice.Idling;
                                Poll();
                                if (_Disable)
                                {
                                    CurrentState = StatesBillToBillDevice.Disabling;
                                }
                            }
                            break;
                        case ComandosRecepcion.Aceptando:
                            CurrentState = StatesBillToBillDevice.Acepting;
                            Poll();
                            break;
                        case ComandosRecepcion.Almacenando:
                            CurrentState = StatesBillToBillDevice.Storing;
                            Poll();
                            break;
                        case ComandosRecepcion.DevolviendoDeEscrow:
                            Poll();
                            break;
                        case ComandosRecepcion.Deshabilitado:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else if (CurrentState == StatesBillToBillDevice.Inicializing)
                            {
                                ObtenerTablaBilletes();
                                Thread.Sleep(2000);
                            }
                            else if (CurrentState == StatesBillToBillDevice.Dispensed)
                            {
                                DispensarMultiple(_AcumuladoPagos[_IndiceDispensar]);
                            }
                            else if (CurrentState == StatesBillToBillDevice.PollingSecuense)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.BoxOut)
                            {
                                CurrentState = StatesBillToBillDevice.BoxIn;
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassette3)
                            {
                                CurrentState = StatesBillToBillDevice.SettingCassetteCapacity1;
                                ConfigurarCapacidadCassette(1, 80);
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassetteCapacity1)
                            {
                                CurrentState = StatesBillToBillDevice.SettingCassetteCapacity2;
                                ConfigurarCapacidadCassette(2, 80);
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassetteCapacity2)
                            {
                                CurrentState = StatesBillToBillDevice.SettingCassetteCapacity3;
                                ConfigurarCapacidadCassette(3, 80);
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassetteCapacity3)
                            {
                                CurrentState = StatesBillToBillDevice.EndInicialization;
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.PreAcepting)
                            {
                                Habilitar(_DenominacionTemporal);
                            }
                            else if (CurrentState == StatesBillToBillDevice.PreAceptingAll)
                            {
                                Habilitar();
                            }
                            else
                            {
                                _SecondDisable++;
                                if (_SecondDisable >= 3)
                                {
                                    CurrentState = StatesBillToBillDevice.Disable;
                                }
                                else
                                {
                                    CurrentState = StatesBillToBillDevice.Disable;
                                    Poll();
                                }
                            }
                            break;
                        case ComandosRecepcion.Holding:
                            break;
                        case ComandosRecepcion.Ocupado:
                            CurrentState = StatesBillToBillDevice.Busy;
                            int tiempoEspera = Convert.ToInt32(recibo[4]);
                            Thread.Sleep(tiempoEspera * 1000);
                            Poll();
                            break;
                        case ComandosRecepcion.DescargandoCassette:
                            CurrentState = StatesBillToBillDevice.UnloadingCassette;
                            Poll();
                            break;
                        case ComandosRecepcion.Rechazando:
                            _TipoRechazo = string.Format("{0:X02}", (byte)recibo[4]);
                            CurrentState = StatesBillToBillDevice.Rejecting;
                            Poll();
                            break;
                        case ComandosRecepcion.Dispensando:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else
                            {
                                if (recibo[4] == 00)
                                {
                                    CurrentState = StatesBillToBillDevice.Dispensing;
                                }
                                else
                                {
                                    CurrentState = StatesBillToBillDevice.WaitingRemove;
                                }
                                Poll();
                            }
                            break;
                        case ComandosRecepcion.ConfigurandoCassette:
                            CurrentState = StatesBillToBillDevice.SettingCassetteType;
                            Poll();
                            break;
                        case ComandosRecepcion.BilletesDispensados:
                            if (_AcumuladoPagos.Count > 0)
                            {
                                _IndiceDispensar++;
                                _Dispensado = _AcumuladoPagos[_IndiceDispensar - 1].ToList();
                                if (_IndiceDispensar < _AcumuladoPagos.Count)
                                {
                                    Poll();
                                    CurrentState = StatesBillToBillDevice.Dispensed;
                                }
                                else
                                {
                                    Poll();
                                    CurrentState = StatesBillToBillDevice.Dispensed;
                                    CurrentState = StatesBillToBillDevice.EndDispensing;
                                }
                            }
                            else
                            {
                                Poll();
                                CurrentState = StatesBillToBillDevice.Dispensed;
                                CurrentState = StatesBillToBillDevice.EndDispensing;
                            }
                            break;
                        case ComandosRecepcion.BilletesDescargados:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else
                            {
                                if (_DescargaTodos)
                                {
                                    _IndiceDescargaCassette++;
                                    if (_IndiceDescargaCassette < 4)
                                    {
                                        CurrentState = StatesBillToBillDevice.CassetteUnloaded;
                                        VaciarCassette(_IndiceDescargaCassette, 120);
                                    }
                                    else
                                    {
                                        CurrentState = StatesBillToBillDevice.CassetteUnloaded;
                                        CurrentState = StatesBillToBillDevice.CassettesUnloaded;
                                        Poll();
                                    }
                                }
                                else
                                {
                                    _DescargaTodos = false;
                                    _IndiceDescargaCassette = 0;
                                    CurrentState = StatesBillToBillDevice.CassetteUnloaded;
                                    CurrentState = StatesBillToBillDevice.CassettesUnloaded;
                                    Poll();
                                }
                            }
                            break;
                        case ComandosRecepcion.CantidadInvalidaBilletes:
                            Poll();
                            for (int i = _IndiceDispensar; i < _AcumuladoPagos.Count; i++)
                            {
                                _ValorAdicional += _AcumuladoPagos[i][0] * Convert.ToInt32(_Denominaciones[0]);
                                _ValorAdicional += _AcumuladoPagos[i][1] * Convert.ToInt32(_Denominaciones[1]);
                                _ValorAdicional += _AcumuladoPagos[i][2] * Convert.ToInt32(_Denominaciones[2]);
                            }
                            CurrentState = StatesBillToBillDevice.ErrorDispensing;
                            break;
                        case ComandosRecepcion.CassetteConfigurado:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Poll();
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassette1)
                            {
                                CurrentState = StatesBillToBillDevice.SettingCassette2;
                                ConfigurarCassettes(2);
                            }
                            else if (CurrentState == StatesBillToBillDevice.SettingCassette2)
                            {
                                CurrentState = StatesBillToBillDevice.SettingCassette3;
                                ConfigurarCassettes(3);
                            }
                            else
                            {
                                Poll();
                            }
                            break;
                        case ComandosRecepcion.BoxLleno:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else
                            {
                                CurrentState = StatesBillToBillDevice.BoxFull;
                            }
                            break;
                        case ComandosRecepcion.BoxAfuera:
                            Poll();
                            if (CurrentState != StatesBillToBillDevice.Nothing)
                            {
                                CurrentState = StatesBillToBillDevice.BoxOut;
                            }
                            break;
                        case ComandosRecepcion.AtascoValidador:
                            CurrentState = StatesBillToBillDevice.ValidatorJam;
                            break;
                        case ComandosRecepcion.AtascoBox:
                            CurrentState = StatesBillToBillDevice.BoxJam;
                            break;
                        case ComandosRecepcion.Cheated:
                            break;
                        case ComandosRecepcion.FallaGenerica:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else
                            {
                                _TipoFalla = string.Format("{0:X02}", (byte)recibo[4]);
                                CurrentState = StatesBillToBillDevice.Falla;
                            }
                            break;
                        case ComandosRecepcion.BilleteEnEscrow:
                            Poll();
                            break;
                        case ComandosRecepcion.BilleteAlmacenado:
                            if (CurrentState == StatesBillToBillDevice.Nothing)
                            {
                                Reset();
                            }
                            else
                            {
                                if (recibo[5] == 1 || recibo[5] == 2 || recibo[5] == 3)
                                {
                                    _Parte = "Cassette";
                                }
                                else
                                {
                                    _Parte = "Box";
                                }

                                _ValorAdicional = Convert.ToInt32(_BillTableList[recibo[4]]);

                                CurrentState = StatesBillToBillDevice.BillAcepted;
                                Poll();
                            }
                            break;
                        case ComandosRecepcion.BilleteDevuelto:
                            Poll();
                            break;
                        case ComandosRecepcion.ComandoInvalido:

                            break;
                        default:
                            if (CurrentState == StatesBillToBillDevice.Inicializing)
                            {
                                BillTableList = new List<string>();
                                int Indice = 0;
                                for (int i = 0; i < 33; i += 5)
                                {
                                    BillTableList.Add(((Convert.ToInt32(recibo[(i + 3)]) * (int)System.Math.Pow(10, Convert.ToDouble(recibo[i + 7])))).ToString());
                                    Indice += 1;
                                }
                                BillTableList.Remove("0");
                                if (_DeviceType == DeviceTypeEnum.BB.ToString())
                                {
                                    EnviarOpciones();
                                }
                                else if (_DeviceType == DeviceTypeEnum.BV.ToString())
                                {
                                    CurrentState = StatesBillToBillDevice.StartSendingOptions;
                                    Deshabilitar();
                                    //CurrentState = StatesBillToBillDevice.EndInicialization;
                                    //Poll();
                                }
                            }
                            break;
                    }
                }
            }
        }
        public void CancelarRecepcion()
        {
            _Disable = true;
        }
        public void HabilitarRecepcion()
        {
            CurrentState = StatesBillToBillDevice.PreAceptingAll;
            Poll();
        }
        public void HabilitarRecepcion(List<int> denominaciones)
        {
            _DenominacionTemporal = denominaciones;
            CurrentState = StatesBillToBillDevice.PreAcepting;
            Poll();
        }

        #region BillToBillCommands
        public void PollingSecuense()
        {
            CurrentState = StatesBillToBillDevice.PollingSecuense;
            Poll();
        }
        private void Habilitar()
        {
            _SecondDisable = 1;

            _Disable = false;

            byte[] paquete = new byte[12];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = StringToByteArray("0C")[0];
            paquete[3] = StringToByteArray(ComandosEnvio.HabilitarDeshabilitar.ToString())[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = StringToByteArray("FF")[0];
            paquete[6] = StringToByteArray("FF")[0];
            paquete[7] = 0;
            paquete[8] = 0;
            paquete[9] = 0;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void Habilitar(List<int> denominaciones)
        {
            _SecondDisable = 1;

            _Disable = false;
            List<int> denomIndice = new List<int>();
            int i = 0;
            foreach (string item in BillTableList)
            {
                foreach (int item2 in denominaciones)
                {
                    if (item2 == Convert.ToInt32(item))
                    {
                        denomIndice.Add(i);
                        break;
                    }
                }
                i++;
            }

            bool[] arrayBit = new bool[24];
            for (int j = 0; j < arrayBit.Length; j++)
            {
                if (denomIndice.Contains(j))
                {
                    arrayBit[j] = true;
                }
                else
                {
                    arrayBit[j] = false;
                }
            }

            string bitArray = string.Empty;
            foreach (bool item in arrayBit)
            {
                bitArray = Convert.ToInt32(item).ToString() + bitArray;
            }

            byte[] byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(k => k * 8)    // get the starting index of which char segment
                          .TakeWhile(k => k < bitArray.Length)
                          .Select(k => bitArray.Substring(k, 8)) // get the binary string segments
                          .Select(s => Convert.ToByte(s, 2)) // convert to byte
                          .ToArray();

            byte[] paquete = new byte[12];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = StringToByteArray("0C")[0];
            paquete[3] = StringToByteArray(ComandosEnvio.HabilitarDeshabilitar.ToString())[0];
            paquete[4] = byteArray[0];
            paquete[5] = byteArray[1];
            paquete[6] = byteArray[2];
            paquete[7] = 0;
            paquete[8] = 0;
            paquete[9] = 0;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void Deshabilitar()
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[12];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = StringToByteArray("0C")[0];
            paquete[3] = StringToByteArray(ComandosEnvio.HabilitarDeshabilitar.ToString())[0];
            paquete[4] = 0;
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = 0;
            paquete[8] = 0;
            paquete[9] = 0;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void VaciarCassette(int nCassette, int nBills)
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[8];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 8;
            paquete[3] = StringToByteArray(ComandosEnvio.DescargarCassette.ToString())[0];
            paquete[4] = (byte)nCassette;
            paquete[5] = (byte)nBills;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void VaciarCassettesAll()
        {
            _SecondDisable = 1;

            _DescargaTodos = true;
            _IndiceDescargaCassette = 1;
            byte[] paquete = new byte[8];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 8;
            paquete[3] = StringToByteArray(ComandosEnvio.DescargarCassette.ToString())[0];
            paquete[4] = (byte)_IndiceDescargaCassette;
            paquete[5] = 150;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void Dispensar(int nCassette, int nBills)
        {
            _SecondDisable = 1;

            //cassette a denominacion
            int billNumber = 0;
            int i = 0;
            foreach (string item in BillTableList)
            {
                if (item == Denominaciones[nCassette - 1])
                {
                    billNumber = i;
                    break;
                }
                i++;
            }


            byte[] paquete = new byte[12];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 12;
            paquete[3] = StringToByteArray(ComandosEnvio.Dispensar.ToString())[0];
            paquete[4] = (byte)billNumber;
            paquete[5] = (byte)nBills;
            paquete[6] = 0;
            paquete[7] = 0;
            paquete[8] = 0;
            paquete[9] = 0;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void Almacenar()
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[6];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 6;
            paquete[3] = StringToByteArray(ComandosEnvio.Almacenar.ToString())[0];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void ConfigurarCassettes(int nCassette)
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[8];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 8;
            paquete[3] = StringToByteArray(ComandosEnvio.ConfigurarCassette.ToString())[0];
            paquete[4] = (byte)nCassette;

            int d1 = 31;
            int i = 0;
            if (Convert.ToInt32(Denominaciones[nCassette - 1]) != 0)
            {
                foreach (string item in BillTableList)
                {
                    if (item == Denominaciones[nCassette - 1])
                    {
                        d1 = i;
                        break;
                    }
                    i++;
                }
            }
            else
            {
                d1 = 31;
            }

            paquete[5] = (byte)(d1 + 128);
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void ConfigurarCapacidadCassette(int nCassette, int nMax)
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[8];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 8;
            paquete[3] = StringToByteArray(ComandosEnvio.AjustarCapacidadCassette.ToString())[0];
            paquete[4] = (byte)nCassette;
            paquete[5] = (byte)nMax;
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void EnviarOpciones()
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[10];
            paquete[0] = 02;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = StringToByteArray("0A")[0];
            //if (_DeviceType == DeviceTypeEnum.BV.ToString())
            //{
            //    paquete[3] = StringToByteArray(ComandosEnvio.ObtenerEstatus.ToString())[0];
            //}
            //else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            //{
            paquete[3] = StringToByteArray(ComandosEnvio.EnviarOpciones.ToString())[0];
            //}
            paquete[4] = StringToByteArray("60")[0];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void Poll()
        {
            if (_Enable)
            {
                byte[] paquete = new byte[6];
                paquete[0] = 02;
                if (_DeviceType == DeviceTypeEnum.BV.ToString())
                {
                    paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
                }
                else if (_DeviceType == DeviceTypeEnum.BB.ToString())
                {
                    paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
                }
                paquete[2] = 06;
                paquete[3] = StringToByteArray(ComandosEnvio.Poll.ToString())[0];
                byte[] envio = CrearPaquete(paquete);
                WriteData(envio);
            }
        }
        private void ACK()
        {
            byte[] paquete = new byte[6];
            paquete[0] = 02;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 06;
            paquete[3] = StringToByteArray(ComandosEnvio.ACK.ToString())[0];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void Reset()
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[6];
            paquete[0] = 02;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 06;
            paquete[3] = StringToByteArray(ComandosEnvio.Reset.ToString())[0];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        private void ObtenerTablaBilletes()
        {
            _SecondDisable = 1;

            byte[] paquete = new byte[6];
            paquete[0] = 02;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 06;
            paquete[3] = StringToByteArray(ComandosEnvio.ObtenerTablaBilletes.ToString())[0];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void DispensarMultiple(int[] valoresDispensar)
        {
            _SecondDisable = 1;

            //cassette a denominacion
            int billNumber1 = 0;
            int billNumber2 = 0;
            int billNumber3 = 0;
            int i = 0;
            foreach (string item in BillTableList)
            {
                if (item == Denominaciones[0])
                {
                    billNumber1 = i;
                }
                else if (item == Denominaciones[1])
                {
                    billNumber2 = i;
                }
                else if (item == Denominaciones[2])
                {
                    billNumber3 = i;
                }
                i++;
            }


            byte[] paquete = new byte[12];
            paquete[0] = 2;
            if (_DeviceType == DeviceTypeEnum.BV.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BV.ToString())[0];
            }
            else if (_DeviceType == DeviceTypeEnum.BB.ToString())
            {
                paquete[1] = StringToByteArray(ComandosEnvioDispositivo.BB.ToString())[0];
            }
            paquete[2] = 12;
            paquete[3] = StringToByteArray(ComandosEnvio.Dispensar.ToString())[0];
            paquete[4] = (byte)billNumber1;
            paquete[5] = (byte)valoresDispensar[0];
            paquete[6] = (byte)billNumber2;
            paquete[7] = (byte)valoresDispensar[1];
            paquete[8] = (byte)billNumber3;
            paquete[9] = (byte)valoresDispensar[2];
            byte[] envio = CrearPaquete(paquete);
            WriteData(envio);
        }
        public void Pagar(Int64 iValorPagar, List<int> lstValoresDisponibles)
        {
            //TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: Entra a funcion pagar en device bill to bill valor a pagar = " + iValorPagar, TipoLog.TRAZA);  
            _SecondDisable = 1;

            _ValorAdicional = 0;
            _IndiceDispensar = 0;
            int[] t = { 0, 0, 0 };
            int[] b = { 0, 0, 0 };
            b[0] = lstValoresDisponibles[0];
            b[1] = lstValoresDisponibles[1];
            b[2] = lstValoresDisponibles[2];

            calcular(iValorPagar, b, t);

            _ValorAdicional = iValorPagar - (t[0] * Convert.ToInt32(Denominaciones[0]) + t[1] * Convert.ToInt32(Denominaciones[1]) + t[2] * Convert.ToInt32(Denominaciones[2]));

            FormarGruposPago(t.ToList());

            DispensarMultiple(_AcumuladoPagos[_IndiceDispensar]);

        }
        private bool calcular(Int64 cantidad, int[] b, int[] t)
        {
            _SecondDisable = 1;

            Int64 mC = cantidad - (t[0] * Convert.ToInt64(Denominaciones[0]) + t[1] * Convert.ToInt64(Denominaciones[1]) + t[2] * Convert.ToInt64(Denominaciones[2]));

            if (mC == 0)
            {
                return true;
            }
            else
            {
                if (mC >= Convert.ToInt32(Denominaciones[2]) && b[2] > 0)
                {
                    b[2]--;
                    t[2]++;
                    mC = cantidad - (t[0] * Convert.ToInt64(Denominaciones[0]) + t[1] * Convert.ToInt64(Denominaciones[1]) + t[2] * Convert.ToInt64(Denominaciones[2]));
                    if (mC < ((Convert.ToInt32(Denominaciones[2]) / Convert.ToInt32(Denominaciones[1])) * Convert.ToInt32(Denominaciones[1]) + Convert.ToInt32(Denominaciones[1]) - Convert.ToInt32(Denominaciones[2])) || mC >= Convert.ToInt32(Denominaciones[2]))
                    {
                        calcular(cantidad, b, t);
                    }
                    else
                    {
                        t[2]--;
                        t[1]++;
                        b[1]--;
                        b[2]++;
                        calcular(cantidad, b, t);
                    }
                }
                else if (mC >= Convert.ToInt32(Denominaciones[1]) && b[1] > 0)
                {
                    b[1]--;
                    t[1]++;
                    calcular(cantidad, b, t);
                }
                else if (mC >= Convert.ToInt32(Denominaciones[0]) && b[0] > 0)
                {
                    b[0]--;
                    t[0]++;
                    calcular(cantidad, b, t);
                }
                else
                {
                    //      cout<<mC;
                    if (mC >= ((Convert.ToInt32(Denominaciones[1]) / Convert.ToInt32(Denominaciones[0])) * Convert.ToInt32(Denominaciones[0]) + Convert.ToInt32(Denominaciones[0]) - Convert.ToInt32(Denominaciones[1])))
                    {
                        if (t[1] > 0 && b[0] > 0)
                        {
                            t[1]--;
                            t[0]++;
                            b[0]--;
                            b[1]++;
                            calcular(cantidad, b, t);
                        }
                        else if (t[2] > 0 && b[1] > 0)
                        {
                            t[2]--;
                            t[1]++;
                            b[1]--;
                            b[2]++;
                            calcular(cantidad, b, t);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        private void FormarGruposPago(List<int> lsPagos)
        {
            _SecondDisable = 1;

            _AcumuladoPagos = new List<int[]>();
            int[] _Pagos = { 0, 0, 0 };

            int i = 0;
            int suma = 0;
            while (i <= 3)
            {
                if (i == 3)
                {
                    _AcumuladoPagos.Add(_Pagos);
                    i++;
                }
                else
                {
                    int posibleInsertar = 20 - suma;
                    if (posibleInsertar != 0)
                    {
                        if (lsPagos[i] <= posibleInsertar)
                        {
                            _Pagos[i] = lsPagos[i];
                            suma += lsPagos[i];
                            lsPagos[i] = 0;
                            i++;
                        }
                        else
                        {
                            //int insertar = lsPagos[i] - posibleInsertar;
                            _Pagos[i] = posibleInsertar;
                            lsPagos[i] -= posibleInsertar;
                            _AcumuladoPagos.Add(_Pagos);
                            _Pagos = new int[3]
                                {
                                    0,0,0
                                };
                            suma = 0;
                        }
                    }
                    else
                    {
                        _AcumuladoPagos.Add(_Pagos);
                        suma = 0;
                        _Pagos = new int[3]
                        {
                            0,0,0
                        };
                    }
                }
            }
        }
        #endregion

        #region SerialFunctions
        private byte[] CrearPaquete(byte[] Paquete)
        {
            try
            {
                ushort crc = 0;
                ushort L = (byte)(Paquete.Length - 2);
                string PaqueteString = string.Empty;

                for (int j = 0; j < L; j++)
                {
                    ushort b = Convert.ToChar(Paquete[j]);
                    for (int i = 0; i < 8; i++)
                    {
                        crc = ((b ^ crc) & 1) > 0 ? (ushort)((crc >> 1) ^ 0x8408) : (ushort)(crc >> 1);
                        b >>= 1;
                    }
                }
                Paquete[L] = (byte)(crc & 255);
                Paquete[L + 1] = (byte)(crc / 256);

                for (int i = 0; i < L + 2; i++)
                {
                    PaqueteString += string.Format("{0:X02} ", Paquete[i]);
                }

                return Paquete;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");

            byte[] comBuffer = new byte[msg.Length / 2];

            for (int i = 0; i < msg.Length; i += 2)

                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);

            return comBuffer;
        }
        private string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            foreach (byte data in comByte)
            {
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return builder.ToString().ToUpper();
        }
        public void WriteData(byte[] msg)
        {
            string ExtraccionlogBits = BitConverter.ToString(msg);
            TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Billetero"), "msg: " + ExtraccionlogBits, TipoLog.TRAZA);
            EventArgsBillToBillSerialCommunicationDevice evento = new EventArgsBillToBillSerialCommunicationDevice(TipoInsertEvento.Envio, msg);
            DeviceMessageSerialPort(this, evento);
            //InsertEvent(TipoInsertEvento.Envio, msg);
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
        public bool OpenPort(string puerto)
        {
            try
            {
                string sPuerto = string.Empty;

                sPuerto = puerto;

                if (_ComPort.IsOpen == true)
                {
                    _ComPort.Close();
                }

                _ComPort.ReadTimeout = 5000;
                _ComPort.WriteTimeout = 5000;
                _ComPort.BaudRate = 9600;
                _ComPort.DataBits = 8;
                _ComPort.StopBits = StopBits.One;
                _ComPort.Parity = Parity.None;
                _ComPort.Handshake = Handshake.None;
                _ComPort.PortName = sPuerto;

                _ComPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ClosePort()
        {
            try
            {

                if (_ComPort.IsOpen == true)
                {
                    _ComPort.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region SerialEvents
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(200);
            int bytes = _ComPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            _ComPort.Read(comBuffer, 0, bytes);
            string ExtraccionlogBits = BitConverter.ToString(comBuffer);
            TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Billetero"), "comBuffer: " + ExtraccionlogBits, TipoLog.TRAZA);
            EventArgsBillToBillSerialCommunicationDevice evento = new EventArgsBillToBillSerialCommunicationDevice(TipoInsertEvento.Recepcion, comBuffer);
            DeviceMessageSerialPort(this, evento);
            Actuar(comBuffer);
        }
        #endregion

    }

    public class EventArgsBillToBillSerialCommunicationDevice : EventArgs
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

        public EventArgsBillToBillSerialCommunicationDevice(TipoInsertEvento oTipoInsertEvento, byte[] oByteArray)
        {
            _TipoInsertEvento = oTipoInsertEvento;
            _ByteArray = oByteArray;
        }
    }

    public class EventArgsBillToBillEventsDevice : EventArgs
    {
        private StatesBillToBillDevice _State = StatesBillToBillDevice.Nothing;
        public StatesBillToBillDevice StatesBillToBillDevice
        {
            get { return _State; }
            set { _State = value; }
        }

        private Int64 _Valor = 0;
        public Int64 Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        private string _Parte = string.Empty;
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }

        private List<int> _Dispensado = new List<int>();
        public List<int> Dispensado
        {
            get { return _Dispensado; }
            set { _Dispensado = value; }
        }

        private string _Falla;
        public string Falla
        {
            get { return _Falla; }
            set { _Falla = value; }
        }

        private string _Rechazo;
        public string Rechazo
        {
            get { return _Rechazo; }
            set { _Rechazo = value; }
        }

        public EventArgsBillToBillEventsDevice(StatesBillToBillDevice oStatesBillToBillDevice, Int64 iValor, string sParte, List<int> oDispensado, string oFalla, string oRechazo)
        {
            _State = oStatesBillToBillDevice;
            _Valor = iValor;
            _Parte = sParte;
            _Dispensado = oDispensado;
            _Falla = oFalla;
            _Rechazo = oRechazo;
        }
    }
}
