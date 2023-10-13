using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Timers;
using System.Diagnostics;

using System.IO;
using Ds.Utilidades;

namespace Ds.CtCoin
{
    public class PelikanCtCoinDevice
    {
        public EventHandler DeviceMessagePelikanCtCoinSaved;
        public EventHandler DeviceMessagePelikanCtCoinState;
        public EventHandler DeviceMessageSerialPort;
        private SerialPort _ComPort = new SerialPort();
        private StatesPelicanDevice _CurrentState = StatesPelicanDevice.Nothing;
        public StatesPelicanDevice CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                EventArgsPelikanCtCoinEventsDevice evento = new EventArgsPelikanCtCoinEventsDevice(value, _States, _Error, _Coins, _Last);
                DeviceMessagePelikanCtCoinState(this, evento);
            }
        }
        private string _Puerto = string.Empty;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        private List<int> _Denominations = new List<int>();
        public List<int> Denominations
        {
            get { return _Denominations; }
            set { _Denominations = value; }
        }
        private int _SalvadaMayor = 0;
        private int _ConteoSalvadas = 0;
        private List<SavedTransactions> _LstTransaccionesSalvadas = new List<SavedTransactions>();

        public string Puerto
        {
            get { return _Puerto; }
            set { _Puerto = value; }
        }

        private int tickFinalizar = 0;

        private TipoEstadoCtCoin _States = new TipoEstadoCtCoin();
        private TipoErrorCtCoin _Error = TipoErrorCtCoin.No_error;
        private List<TipoMoneda> _Coins = new List<TipoMoneda>();
        private List<TipoMoneda> _Last = new List<TipoMoneda>();
        private int conteoPendiente = 0;

        private void Actuar(byte[] recibo)
        {
            foreach (byte item in recibo)
            {
                Debug.Write(string.Format("{0:X02}", (byte)item));
            }

            Debug.WriteLine("");
            
            lock (this)
            {
                if (recibo.Length > 1)
                {
                    if (VerificarCRC(recibo))
                    {
                        ACK();

                        string recepcion = string.Format("{0:X02}", (byte)recibo[3]);
                        string recepcion2 = string.Format("{0:X02}", (byte)recibo[4]);
                        string recepcion3 = string.Format("{0:X02}", (byte)recibo[5]);

                        switch (recepcion)
                        {
                            #region Link
                            case "02":
                                switch (CurrentState)
                                {
                                    case StatesPelicanDevice.Nothing:
                                        if (recepcion2 == "00")
                                        {
                                            CurrentState = StatesPelicanDevice.Inicilizing;
                                            GetState();
                                        }
                                        else
                                        {
                                            CurrentState = StatesPelicanDevice.NoLink;
                                        }
                                        break;
                                }
                                break;
                            #endregion
                            #region GetValues
                            case "12":
                                switch (recepcion3)
                                {
                                    #region ValuesState
                                    case "33":
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.Inicilizing:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    CoinDenomValues();
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorInitializing;
                                                }
                                                break;
                                            case StatesPelicanDevice.StartCleaning:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else if (_States.BowlOpen)
                                                    {
                                                        conteoPendiente = 0;
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else
                                                    {
                                                        if (!_States.TransaccionPendiente)
                                                        {
                                                            GetRead();
                                                            //CurrentState = StatesPelicanDevice.EndClean;
                                                        }
                                                        else
                                                        {
                                                            if (conteoPendiente > 5)
                                                            {
                                                                CurrentState = StatesPelicanDevice.PendingCount;
                                                            }
                                                            else
                                                            {
                                                                conteoPendiente++;
                                                                CurrentState = StatesPelicanDevice.StartCleaning;
                                                                GetState();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (_Error == TipoErrorCtCoin.Coins_left_in_bowl)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Enter;
                                                        Enter();
                                                    }
                                                    else
                                                    {
                                                        CurrentState = StatesPelicanDevice.ErrorCleaning;
                                                    }
                                                }
                                                break;
                                            case StatesPelicanDevice.StartCounting:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Counting;
                                                        GetRead();
                                                    }
                                                    else
                                                    {
                                                        CurrentState = StatesPelicanDevice.ErrorCounting;
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorCounting;
                                                }
                                                break;
                                            case StatesPelicanDevice.Counting:
                                                _TempRespuesta = 0;
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        tickFinalizar = 0;
                                                        CurrentState = StatesPelicanDevice.Counting;
                                                        GetRead();
                                                    }
                                                    else
                                                    {
                                                        if (!_States.TransaccionPendiente)
                                                        {
                                                            CurrentState = StatesPelicanDevice.StartCleaning;
                                                            conteoPendiente = 0;
                                                            Thread.Sleep(2000);
                                                            GetState();
                                                        }
                                                        else
                                                        {
                                                            if (tickFinalizar >= 5)
                                                            {
                                                                tickFinalizar = 0;
                                                                CurrentState = StatesPelicanDevice.EndCount;
                                                            }
                                                            else
                                                            {
                                                                tickFinalizar++;
                                                                GetState();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorCounting;
                                                }
                                                break;
                                            case StatesPelicanDevice.Cleaning:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else if (_States.BowlOpen)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else
                                                    {
                                                        if (!_States.TransaccionPendiente)
                                                        {
                                                            GetRead();
                                                            //CurrentState = StatesPelicanDevice.EndClean;
                                                        }
                                                        else
                                                        {
                                                            CurrentState = StatesPelicanDevice.Cleaning;
                                                            GetState();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorCleaning;
                                                }
                                                break;
                                            case StatesPelicanDevice.StartCountAndClean:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.CountingAndCleaning;
                                                        GetRead();
                                                    }
                                                    else
                                                    {
                                                        CurrentState = StatesPelicanDevice.ErrorCountAndClean;
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorCountAndClean;
                                                }
                                                break;
                                            case StatesPelicanDevice.CountingAndCleaning:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.CountingAndCleaning;
                                                        GetRead();
                                                    }
                                                    else
                                                    {
                                                        if (!_States.TransaccionPendiente)
                                                        {
                                                            CurrentState = StatesPelicanDevice.StartCleaning;
                                                            conteoPendiente = 0;
                                                            Thread.Sleep(2000);
                                                            GetState();
                                                        }
                                                        else
                                                        {
                                                            CurrentState = StatesPelicanDevice.StartCleaningCountClean;
                                                            Clean();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorCounting;
                                                }
                                                break;
                                            case StatesPelicanDevice.StartCleaningCountClean:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    if (_States.MotorMovimiento)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else if (_States.BowlOpen)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Cleaning;
                                                        GetState();
                                                    }
                                                    else
                                                    {
                                                        if (!_States.TransaccionPendiente)
                                                        {
                                                            GetRead();
                                                            //CurrentState = StatesPelicanDevice.EndClean;
                                                        }
                                                        else
                                                        {
                                                            CurrentState = StatesPelicanDevice.Cleaning;
                                                            GetState();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (_Error == TipoErrorCtCoin.Coins_left_in_bowl)
                                                    {
                                                        CurrentState = StatesPelicanDevice.Enter;
                                                        Enter();
                                                    }
                                                }
                                                break;
                                            case StatesPelicanDevice.GettingState:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error == TipoErrorCtCoin.No_error)
                                                {
                                                    CurrentState = StatesPelicanDevice.EndGettingState;
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.ErrorGettingState;
                                                }
                                                break;
                                            case StatesPelicanDevice.EnterSolo:
                                                _Error = GetError(recibo);
                                                _States = GetStates(recibo);
                                                if (_Error != TipoErrorCtCoin.No_error)
                                                {
                                                    Enter();
                                                }
                                                else
                                                {
                                                    CurrentState = StatesPelicanDevice.EndEnterSolo;
                                                }
                                                break;
                                        }
                                        break;
                                    #endregion
                                    #region ValuesRead
                                    case "16":
                                        int sum = 0;
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.Inicilizing:
                                                _Coins = GetCoinValues(recibo);
                                                CurrentState = StatesPelicanDevice.Inicialized;
                                                break;
                                            case StatesPelicanDevice.Counting:
                                                _TempRespuesta = 0;
                                                _Coins = GetCoinValues(recibo);
                                                CurrentState = StatesPelicanDevice.Counting;
                                                GetState();
                                                break;
                                            case StatesPelicanDevice.StartCleaning:
                                                sum = 0;
                                                _Coins = GetCoinValues(recibo);
                                                foreach (var item in _Coins)
                                                {
                                                    sum += item.Cantidad * item.Denominacion;
                                                }
                                                if (sum == 0)
                                                {
                                                    CurrentState = StatesPelicanDevice.EndClean;
                                                }
                                                else
                                                {
                                                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "ACA HAY UN ERROR SI LLEGA // StartCleaning", TipoLog.TRAZA);
                                                }
                                                break;
                                            case StatesPelicanDevice.Cleaning:
                                                sum = 0;
                                                _Coins = GetCoinValues(recibo);
                                                foreach (var item in _Coins)
                                                {
                                                    sum += item.Cantidad * item.Denominacion;
                                                }
                                                if (sum == 0)
                                                {
                                                    CurrentState = StatesPelicanDevice.EndClean;
                                                }
                                                else
                                                {
                                                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "ACA HAY UN ERROR SI LLEGA // Cleaning", TipoLog.TRAZA);
                                                }
                                                break;
                                            case StatesPelicanDevice.CountingAndCleaning:
                                                _Coins = GetCoinValues(recibo);
                                                CurrentState = StatesPelicanDevice.CountingAndCleaning;
                                                GetState();
                                                break;
                                        }
                                        break;
                                    #endregion
                                    #region ValuesDenominatios
                                    case "1F":
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.Inicilizing:
                                                int coinValue = 0;
                                                for (int i = 0; i < 64; i += 4)
                                                {
                                                    coinValue = int.Parse(string.Format("{0:X02}", (byte)recibo[i + 6]) + string.Format("{0:X02}", (byte)recibo[i + 7]) + string.Format("{0:X02}", (byte)recibo[i + 8]) + string.Format("{0:X02}", (byte)recibo[i + 9]), System.Globalization.NumberStyles.HexNumber);
                                                    if (coinValue > 0)
                                                    {
                                                        _Denominations.Add(coinValue);
                                                    }
                                                }

                                                GetRead();
                                                break;
                                        }
                                        break;
                                    #endregion
                                    #region ValuesSavedTransactions
                                    case "22":
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.GettingSavedTransactions:
                                                _LstTransaccionesSalvadas = new List<SavedTransactions>();
                                                int cantidadTransaccionesSalvadas = int.Parse(string.Format("{0:X02}", (byte)recibo[6]) + string.Format("{0:X02}", (byte)recibo[7]), System.Globalization.NumberStyles.HexNumber);
                                                if (cantidadTransaccionesSalvadas > 0)
                                                {
                                                    int idPrimeraTransaccion = int.Parse(string.Format("{0:X02}", (byte)recibo[10]) + string.Format("{0:X02}", (byte)recibo[11]), System.Globalization.NumberStyles.HexNumber);
                                                    int idUltimaTransaccion = int.Parse(string.Format("{0:X02}", (byte)recibo[12]) + string.Format("{0:X02}", (byte)recibo[13]), System.Globalization.NumberStyles.HexNumber);

                                                    
                                                    _ConteoSalvadas = idPrimeraTransaccion;
                                                    _SalvadaMayor = idUltimaTransaccion;

                                                    GetTransactionData(_ConteoSalvadas);
                                                }
                                                else
                                                {
                                                    EventArgsPelikanCtCoinSavedTransactions evento = new EventArgsPelikanCtCoinSavedTransactions(_LstTransaccionesSalvadas);
                                                    DeviceMessagePelikanCtCoinSaved(this, evento);
                                                    CurrentState = StatesPelicanDevice.EndGettingSavedTransactions;
                                                }

                                                break;
                                        }
                                        break;
                                    #endregion
                                    #region SpecificTransaction
                                    case "23":
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.GettingSavedTransactions:
                                                SavedTransactions oSavedTransactions = new SavedTransactions();
                                                oSavedTransactions.Valor = int.Parse(string.Format("{0:X02}", (byte)recibo[24]) + string.Format("{0:X02}", (byte)recibo[25]) + string.Format("{0:X02}", (byte)recibo[26]) + string.Format("{0:X02}", (byte)recibo[27]), System.Globalization.NumberStyles.HexNumber);
                                                oSavedTransactions.Fecha = string.Format("{0:X02}", (byte)recibo[11]) + ":" + string.Format("{0:X02}", (byte)recibo[12])
                                                                            + " " + string.Format("{0:X02}", (byte)recibo[15])
                                                                            + "/" + string.Format("{0:X02}", (byte)recibo[14])
                                                                            + "/" + string.Format("{0:X02}", (byte)recibo[13]);
                                                oSavedTransactions.Id = int.Parse(string.Format("{0:X02}", (byte)recibo[6]) + string.Format("{0:X02}", (byte)recibo[7]), System.Globalization.NumberStyles.HexNumber);
                                                _LstTransaccionesSalvadas.Add(oSavedTransactions);
                                                _ConteoSalvadas++;
                                                if (_ConteoSalvadas <= _SalvadaMayor)
                                                {
                                                    Thread.Sleep(500);
                                                    GetTransactionData(_ConteoSalvadas);
                                                }
                                                else
                                                {
                                                    EventArgsPelikanCtCoinSavedTransactions evento = new EventArgsPelikanCtCoinSavedTransactions(_LstTransaccionesSalvadas);
                                                    DeviceMessagePelikanCtCoinSaved(this, evento);
                                                    CurrentState = StatesPelicanDevice.EndGettingSavedTransactions;
                                                }
                                                break;
                                        }
                                        break;
                                    #endregion
                                }
                                break;
                            #endregion
                            #region SetValues
                            case "22":
                                switch (recepcion3)
                                {
                                    case "5E":
                                        switch (CurrentState)
                                        {
                                            case StatesPelicanDevice.StartCleaning:
                                                _Last = _Coins;
                                                GetState();
                                                break;
                                            case StatesPelicanDevice.StartCounting:
                                                _Last = new List<TipoMoneda>();
                                                GetState();
                                                break;
                                            case StatesPelicanDevice.StartCountAndClean:
                                                _Last = new List<TipoMoneda>();
                                                GetState();
                                                break;
                                            case StatesPelicanDevice.StartCleaningCountClean:
                                                _Last = _Coins;
                                                GetState();
                                                break;
                                            case StatesPelicanDevice.Enter:
                                                CurrentState = StatesPelicanDevice.Enter2;
                                                Enter();
                                                break;
                                            case StatesPelicanDevice.Enter2:
                                                CurrentState = StatesPelicanDevice.StartCountAndClean;
                                                Count();
                                                break;
                                            case StatesPelicanDevice.StartReseting:
                                                CurrentState = StatesPelicanDevice.Reseting;
                                                CLR();
                                                break;
                                            case StatesPelicanDevice.Reseting:
                                                CurrentState = StatesPelicanDevice.ContinueReseting;
                                                Com1();
                                                break;
                                            case StatesPelicanDevice.ContinueReseting:
                                                CurrentState = StatesPelicanDevice.EnterReseting;
                                                Enter();
                                                break;
                                            case StatesPelicanDevice.EnterReseting:
                                                CurrentState = StatesPelicanDevice.Enter2Reseting;
                                                Enter();
                                                break;
                                            case StatesPelicanDevice.Enter2Reseting:
                                                CurrentState = StatesPelicanDevice.EndReseting;
                                                break;
                                            case StatesPelicanDevice.EnterSolo:
                                                GetState();
                                                break;
                                        }
                                        break;
                                }
                                break;
                            #endregion
                        }
                    }
                    else
                    {
                        NACK();
                        TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "No pasa ACK, se envia NACK // Recibido -> ", TipoLog.TRAZA);
                        foreach (byte item in recibo)
                        {
                            TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), string.Format("{0:X02}", (byte)item), TipoLog.TRAZA);
                        }
                        _TempRespuesta = 0;
                        _Waiting = true;
                    }
                }
                else
                {
                    if (string.Format("{0:X02}", (byte)recibo[0]) == "15")
                    {
                        WriteData(_LastMsg);
                    }

                    _TempRespuesta = 0;
                    _Waiting = true;

                }
            }
        }

        private bool VerificarCRC(byte[] aVerificar)
        {
            bool bVerificado = true;
            byte[] sinCRC = new byte[aVerificar.Length - 6];

            Array.Copy(aVerificar, 3, sinCRC, 0, aVerificar.Length - 6);

            CRC DD = new CRC();
            byte[] aComparar = DD.CalculateCrc(sinCRC);

            if (aComparar[1] == aVerificar[aVerificar.Length - 2] && aComparar[0] == aVerificar[aVerificar.Length - 3])
            {
                bVerificado = true;
            }
            else
            {
                bVerificado = false;
            }

            return bVerificado;
        }
        private byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");

            byte[] comBuffer = new byte[msg.Length / 2];

            for (int i = 0; i < msg.Length; i += 2)

                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);

            return comBuffer;
        }
        private byte[] CRC16(string Comando)
        {
            byte[] HInicioLinea = new byte[1];
            HInicioLinea[0] = 02;
            byte[] HFinLinea = new byte[1];
            HFinLinea[0] = 03;

            byte[] Msg = HexToByte(Comando);
            int tam = Msg.Length;
            byte[] bytes = BitConverter.GetBytes(tam);
            byte[] tamaño = new byte[1];
            tamaño[0] = bytes[0];
            CRC DD = new CRC();
            byte[] Resul = DD.CalculateCrc(Msg);

            List<byte> list1 = new List<byte>(HInicioLinea);
            List<byte> list2 = new List<byte>(tamaño);
            List<byte> list3 = new List<byte>(Msg);
            List<byte> list4 = new List<byte>(Resul);
            list1.AddRange(list2);
            list3.AddRange(list4);

            byte[] Track1 = list1.ToArray();
            byte[] Track2 = list3.ToArray();

            List<byte> list5 = new List<byte>(Track1);
            List<byte> list6 = new List<byte>(Track2);

            list5.AddRange(list6);
            byte[] Track3 = list5.ToArray();

            List<byte> list7 = new List<byte>(Track3);
            List<byte> list8 = new List<byte>(HFinLinea);

            list7.AddRange(list8);
            byte[] Resultado = list7.ToArray();

            return Resultado;
        }
        private int _TempRespuesta = 0;
        private bool _Waiting = false;
        private byte[] _LastMsg;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _TempRespuesta++;
            if (_TempRespuesta > 5 && _Waiting)
            {
                _Waiting = false;
                _TempRespuesta = 0;
                GetRead();
            }
        }

        public PelikanCtCoinDevice()
        {
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
        }

        public bool Conectar()
        {
            _Denominations = new List<int>();
            _States = new TipoEstadoCtCoin();
            _Error = TipoErrorCtCoin.No_error;
            _Coins = new List<TipoMoneda>();
            _Last = new List<TipoMoneda>();

            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            CurrentState = StatesPelicanDevice.Nothing;
            byte[] comBuffer = new byte[7];

            bool bAbrePuerto = false;

            for (int i = 1; i <= 15; i++)
            {
                if (OpenPort("COM" + i))
                {
                    //TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: Entra a funcion pagar en device bill to bill valor a pagar = " + iValorPagar, TipoLog.TRAZA); 
                    try
                    {
                        DestrucLink();
                        Thread.Sleep(500);
                        _ComPort.Read(comBuffer, 0, 7);
                        _ComPort.DiscardInBuffer();
                        if (string.Format("{0:X02}", (byte)comBuffer[3]) == "04")
                        {
                            bAbrePuerto = true;
                            _Puerto = "COM" + i;
                            _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            Thread.Sleep(500);
                            Link();
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

        public TipoErrorCtCoin GetError(byte[] Rept)
        {
            TipoErrorCtCoin resultado = TipoErrorCtCoin.No_error;

            byte ListadoError = Rept[11];

            if (ListadoError > 0)
            {
                if (ListadoError == (int)TipoErrorCtCoin.All_bags_are_filled_up)
                {
                    resultado = TipoErrorCtCoin.All_bags_are_filled_up;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.At_least_one_full_bag)
                {
                    resultado = TipoErrorCtCoin.At_least_one_full_bag;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Bowl_not_closed)
                {
                    resultado = TipoErrorCtCoin.Bowl_not_closed;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Check_Sensor)
                {
                    resultado = TipoErrorCtCoin.Check_Sensor;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Coins_left_in_bowl)
                {
                    resultado = TipoErrorCtCoin.Coins_left_in_bowl;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Error_on_printer_1)
                {
                    resultado = TipoErrorCtCoin.Error_on_printer_1;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Error_on_printer_2)
                {
                    resultado = TipoErrorCtCoin.Error_on_printer_2;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.HOST_error)
                {
                    resultado = TipoErrorCtCoin.HOST_error;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Lost_funnel_position)
                {
                    resultado = TipoErrorCtCoin.Lost_funnel_position;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Machine_is_turned_on_with_an_unfinished_transaction)
                {
                    resultado = TipoErrorCtCoin.Machine_is_turned_on_with_an_unfinished_transaction;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.MASTER_error)
                {
                    resultado = TipoErrorCtCoin.MASTER_error;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Maximum_of_500_transactions_reached)
                {
                    resultado = TipoErrorCtCoin.Maximum_of_500_transactions_reached;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Overflow_maximum_of_amount_reached)
                {
                    resultado = TipoErrorCtCoin.Overflow_maximum_of_amount_reached;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Printer_1_out_of_paper)
                {
                    resultado = TipoErrorCtCoin.Printer_1_out_of_paper;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Printer_2_out_of_paper)
                {
                    resultado = TipoErrorCtCoin.Printer_2_out_of_paper;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Railstop)
                {
                    resultado = TipoErrorCtCoin.Railstop;
                }
                else if (ListadoError == (int)TipoErrorCtCoin.Sensor_in_right_position)
                {
                    resultado = TipoErrorCtCoin.Sensor_in_right_position;
                }

            }

            return resultado;
        }
        public TipoEstadoCtCoin GetStates(byte[] Rept)
        {
            TipoEstadoCtCoin resultado = new TipoEstadoCtCoin();

            byte ListadoEstado = Rept[7];
            byte ListadoEstado2 = Rept[9];
            string bitsFlag1 = Convert.ToString(ListadoEstado, 2).PadLeft(8, '0');
            string bitsFlag2 = Convert.ToString(ListadoEstado2, 2).PadLeft(8, '0');

            resultado.BowlOpen = bitsFlag2[7] == '0';
            resultado.MotorMovimiento = bitsFlag1[7] == '1';
            resultado.MiddleDrawer = bitsFlag1[5] == '1';
            resultado.TransaccionPendiente = bitsFlag1[1] == '1';
            resultado.ErrorSensorHead = bitsFlag1[3] == '1';
            resultado.ErrorRailStop = bitsFlag1[4] == '1';
            resultado.SensorOutOfCalibration = bitsFlag1[2] == '1';

            return resultado;
        }
        public List<TipoMoneda> GetCoinValues(byte[] Rept)
        {
            List<TipoMoneda> lstTipoMoneda = new List<TipoMoneda>();

            try
            {

                if (Rept.Length == 74)
                {
                    string Extraccion80PrimerosBits = BitConverter.ToString(Rept);
                    Extraccion80PrimerosBits = Extraccion80PrimerosBits.Substring(18, 203);

                    #region Procesosubstring
                    string MonedasT1 = Extraccion80PrimerosBits.Substring(0, 11);
                    string MonedasT2 = Extraccion80PrimerosBits.Substring(12, 11);
                    string MonedasT3 = Extraccion80PrimerosBits.Substring(24, 11);
                    string MonedasT4 = Extraccion80PrimerosBits.Substring(36, 11);
                    string MonedasT5 = Extraccion80PrimerosBits.Substring(48, 11);
                    string MonedasT6 = Extraccion80PrimerosBits.Substring(60, 11);
                    string MonedasT7 = Extraccion80PrimerosBits.Substring(72, 11);
                    string MonedasT8 = Extraccion80PrimerosBits.Substring(84, 11);
                    string MonedasT9 = Extraccion80PrimerosBits.Substring(96, 11);
                    string MonedasT10 = Extraccion80PrimerosBits.Substring(108, 11);
                    string MonedasT11 = Extraccion80PrimerosBits.Substring(120, 11);
                    string MonedasT12 = Extraccion80PrimerosBits.Substring(132, 11);
                    string MonedasT13 = Extraccion80PrimerosBits.Substring(144, 11);
                    string MonedasT14 = Extraccion80PrimerosBits.Substring(156, 11);
                    string MonedasT15 = Extraccion80PrimerosBits.Substring(168, 11);
                    string MonedasT16 = Extraccion80PrimerosBits.Substring(180, 11);
                    string MonedasT17 = Extraccion80PrimerosBits.Substring(192, 11);
                    #endregion

                    #region Replace
                    MonedasT1 = MonedasT1.Replace("-", "").Trim();
                    MonedasT2 = MonedasT2.Replace("-", "").Trim();
                    MonedasT3 = MonedasT3.Replace("-", "").Trim();
                    MonedasT4 = MonedasT4.Replace("-", "").Trim();
                    MonedasT5 = MonedasT5.Replace("-", "").Trim();
                    MonedasT6 = MonedasT6.Replace("-", "").Trim();
                    MonedasT7 = MonedasT7.Replace("-", "").Trim();
                    MonedasT8 = MonedasT8.Replace("-", "").Trim();
                    MonedasT9 = MonedasT9.Replace("-", "").Trim();
                    MonedasT10 = MonedasT10.Replace("-", "").Trim();
                    MonedasT11 = MonedasT11.Replace("-", "").Trim();
                    MonedasT12 = MonedasT12.Replace("-", "").Trim();
                    MonedasT13 = MonedasT13.Replace("-", "").Trim();
                    MonedasT14 = MonedasT14.Replace("-", "").Trim();
                    MonedasT15 = MonedasT15.Replace("-", "").Trim();
                    MonedasT16 = MonedasT16.Replace("-", "").Trim();
                    MonedasT17 = MonedasT17.Replace("-", "").Trim();
                    #endregion

                    List<int> monedas = new List<int>();
                    monedas.Add(Convert.ToInt32(MonedasT1, 16));
                    monedas.Add(Convert.ToInt32(MonedasT2, 16));
                    monedas.Add(Convert.ToInt32(MonedasT3, 16));
                    monedas.Add(Convert.ToInt32(MonedasT4, 16));
                    monedas.Add(Convert.ToInt32(MonedasT5, 16));
                    monedas.Add(Convert.ToInt32(MonedasT6, 16));
                    monedas.Add(Convert.ToInt32(MonedasT7, 16));
                    monedas.Add(Convert.ToInt32(MonedasT8, 16));
                    monedas.Add(Convert.ToInt32(MonedasT9, 16));
                    monedas.Add(Convert.ToInt32(MonedasT10, 16));
                    monedas.Add(Convert.ToInt32(MonedasT11, 16));
                    monedas.Add(Convert.ToInt32(MonedasT12, 16));
                    monedas.Add(Convert.ToInt32(MonedasT13, 16));
                    monedas.Add(Convert.ToInt32(MonedasT14, 16));
                    monedas.Add(Convert.ToInt32(MonedasT15, 16));
                    monedas.Add(Convert.ToInt32(MonedasT16, 16));
                    monedas.Add(Convert.ToInt32(MonedasT17, 16));

                    bool find = false;
                    int i = 0;
                    foreach (int item in _Denominations)
                    {
                        foreach (TipoMoneda item2 in lstTipoMoneda)
                        {
                            if (item2.Denominacion == item)
                            {
                                find = true;
                                item2.Cantidad += monedas[i];
                            }
                        }

                        if (!find)
                        {
                            lstTipoMoneda.Add(new TipoMoneda(item, monedas[i]));
                        }
                        else
                        {
                            find = false;
                        }
                        i++;
                    }

                    //lstTipoMoneda.Add(new TipoMoneda(1000,Convert.ToInt32(MonedasT1, 16)));

                    //lstTipoMoneda.Add(new TipoMoneda(500,Convert.ToInt32(MonedasT2, 16)));

                    //lstTipoMoneda.Add(new TipoMoneda(200, Convert.ToInt32(MonedasT3, 16) + Convert.ToInt32(MonedasT4, 16)));

                    //lstTipoMoneda.Add(new TipoMoneda(100, Convert.ToInt32(MonedasT5, 16)+Convert.ToInt32(MonedasT6, 16)));

                    //lstTipoMoneda.Add(new TipoMoneda(50, Convert.ToInt32(MonedasT7, 16) + Convert.ToInt32(MonedasT8, 16) + Convert.ToInt32(MonedasT9, 16)));
                }
                else
                {
                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "Rept lenght != 74 / Rept lenght = " + Rept.Length, TipoLog.TRAZA);
                    foreach (byte item in Rept)
                    {
                        TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), string.Format("{0:X02}", (byte)item), TipoLog.TRAZA);
                    }
                }
            }
            catch (Exception e)
            {
                TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "Exception " + e.GetType().FullName + " / " + e.Message + " / " + e.Source + " / " + e.StackTrace, TipoLog.TRAZA);
                foreach (byte item in Rept)
                {
                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), string.Format("{0:X02}", (byte)item), TipoLog.TRAZA);
                }
            }

            return lstTipoMoneda;
        }

        public void ContarHopper()
        {
            _TempRespuesta = 0;
            _CurrentState = StatesPelicanDevice.StartCounting;
            Count();
        }
        public void LimpiarHopper()
        {
            _TempRespuesta = 0;
            _CurrentState = StatesPelicanDevice.StartCleaning;
            conteoPendiente = 0;
            Clean();
        }
        public void ContarLimpiar()
        {
            _TempRespuesta = 0;
            _CurrentState = StatesPelicanDevice.StartCountAndClean;
            Count();
        }
        public void ArqueoPelican()
        {
            _CurrentState = StatesPelicanDevice.StartReseting;
            MR();
        }
        public void EnterSolo()
        {
            _CurrentState = StatesPelicanDevice.EnterSolo;
            Enter();
        }
        //public void FinalizarConteo()
        //{
        //    _TempRespuesta = 0;
        //    CurrentState = StatesPelicanDevice.StartEndCount;
        //    GetState();
        //}
        public void ObtenerEstado()
        {
            _TempRespuesta = 0;
            _CurrentState = StatesPelicanDevice.GettingState;
            GetState();
        }
        public void ObtenerTransaccionesSalvadas()
        {
            //_TempRespuesta = 0;
            _CurrentState = StatesPelicanDevice.GettingSavedTransactions;
            InformationSavedTransactions();
        }

        private void Link()
        {
            WriteData(CRC16("013639333930323734"));
        }
        private void DestrucLink()
        {
            WriteData(CRC16("03"));
        }
        private void ACK()
        {
            byte[] paquete = new byte[1];
            paquete[0] = 6;
            WriteData(paquete);
        }
        private void NACK()
        {
            byte[] paquete = new byte[1];
            paquete[0] = HexToByte("15")[0];
            WriteData(paquete);
        }
        private void GetState()
        {
            _TempRespuesta = 0;
            _Waiting = true;
            WriteData(CRC16("1133"));
        }
        private void GetRead()
        {
            _TempRespuesta = 0;
            _Waiting = true;
            WriteData(CRC16("1116"));
        }
        private void Clean()
        {
            WriteData(CRC16("215E50"));
        }
        private void Count()
        {
            WriteData(CRC16("215E53"));
        }
        private void Enter()
        {
            WriteData(CRC16("215E0D"));
        }
        private void MR()
        {
            WriteData(CRC16("215E4D"));
        }
        private void CLR()
        {
            WriteData(CRC16("215E18"));
        }
        private void Com1()
        {
            WriteData(CRC16("215E31"));
        }
        private void CoinDenomValues()
        {
            WriteData(CRC16("111F"));
        }
        private void InformationSavedTransactions()
        {
            WriteData(CRC16("1122"));
        }
        private void GetTransactionData(int idTransaccion)
        {
            WriteData(CRC16("1123" + idTransaccion.ToString("X")));
        }

        public void WriteData(byte[] msg)
        {
            _LastMsg = msg;
            string ExtraccionlogBits = BitConverter.ToString(msg);
            EventArgsPelikanCtCoinSerialCommunicationDevice evento = new EventArgsPelikanCtCoinSerialCommunicationDevice(TipoInsertEvento.Envio, msg);
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

        #region SerialEvents
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            _Waiting = false;
            Thread.Sleep(800);
            int bytes = _ComPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            _ComPort.Read(comBuffer, 0, bytes);
            string ExtraccionlogBits = BitConverter.ToString(comBuffer);
            EventArgsPelikanCtCoinSerialCommunicationDevice evento = new EventArgsPelikanCtCoinSerialCommunicationDevice(TipoInsertEvento.Recepcion, comBuffer);
            DeviceMessageSerialPort(this, evento);
            if (comBuffer.Length > 0)
            {
                Actuar(comBuffer);
            }
            else
            {
                Thread.Sleep(500);
                bytes = _ComPort.BytesToRead;
                comBuffer = new byte[bytes];
                _ComPort.Read(comBuffer, 0, bytes);
                ExtraccionlogBits = BitConverter.ToString(comBuffer);
                evento = new EventArgsPelikanCtCoinSerialCommunicationDevice(TipoInsertEvento.Recepcion, comBuffer);
                DeviceMessageSerialPort(this, evento);
                if (comBuffer.Length > 0)
                {
                    Actuar(comBuffer);
                }
                else
                {
                    _TempRespuesta = 0;
                    _Waiting = true;
                }
            }
        }
        #endregion
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

    public class EventArgsPelikanCtCoinSerialCommunicationDevice : EventArgs
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

        public EventArgsPelikanCtCoinSerialCommunicationDevice(TipoInsertEvento oTipoInsertEvento, byte[] oByteArray)
        {
            _TipoInsertEvento = oTipoInsertEvento;
            _ByteArray = oByteArray;
        }
    }

    public class EventArgsPelikanCtCoinEventsDevice : EventArgs
    {
        private StatesPelicanDevice _State = StatesPelicanDevice.Nothing;
        public StatesPelicanDevice StatesPelicanDevice
        {
            get { return _State; }
            set { _State = value; }
        }
        private TipoEstadoCtCoin _States = new TipoEstadoCtCoin();
        public TipoEstadoCtCoin States
        {
            get { return _States; }
            set { _States = value; }
        }
        private TipoErrorCtCoin _Error = TipoErrorCtCoin.No_error;
        public TipoErrorCtCoin Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private List<TipoMoneda> _CoinsCtCoin = new List<TipoMoneda>();
        public List<TipoMoneda> CoinsCtCoin
        {
            get { return _CoinsCtCoin; }
            set { _CoinsCtCoin = value; }
        }
        private List<TipoMoneda> _LastCoinsCtCoin = new List<TipoMoneda>();
        public List<TipoMoneda> LastCoinsCtCoin
        {
            get { return _LastCoinsCtCoin; }
            set { _LastCoinsCtCoin = value; }
        }

        public EventArgsPelikanCtCoinEventsDevice(StatesPelicanDevice oStatesBillToBillDevice, TipoEstadoCtCoin lstTipoEstadoCtCoin, TipoErrorCtCoin oTipoErrorCtCoin, List<TipoMoneda> lstTipoMoneda, List<TipoMoneda> lastCount)
        {
            _State = oStatesBillToBillDevice;
            _States = lstTipoEstadoCtCoin;
            _Error = oTipoErrorCtCoin;
            _CoinsCtCoin = lstTipoMoneda;
            _LastCoinsCtCoin = lastCount;
        }
    }

    public class EventArgsPelikanCtCoinSavedTransactions : EventArgs
    {
        private List<SavedTransactions> _LstSaved = new List<SavedTransactions>();

        public List<SavedTransactions> LstSaved
        {
            get { return _LstSaved; }
            set { _LstSaved = value; }
        }

        public EventArgsPelikanCtCoinSavedTransactions(List<SavedTransactions> lstTransac)
        {
            _LstSaved = lstTransac;
        }
    }

    public class SavedTransactions
    {
        private Int64 _Valor = 0;

        public Int64 Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        private int _Id = 0;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Fecha = string.Empty;

        public string Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }
    }

    public enum TipoInsertEvento
    {
        Ninguno,
        Envio,
        Recepcion,
    }

    public enum StatesPelicanDevice
    {
        //Errores
        NoLink,
        ErrorInitializing,
        //Normal
        Nothing,
        Inicilizing,
        Inicialized,
        StartCleaning,
        ErrorCleaning,
        StartCounting,
        ErrorCounting,
        Counting,
        Cleaning,
        //Ok,
        EndCount,
        EndClean,

        StartCountAndClean,
        CountingAndCleaning,
        ErrorCountAndClean,

        Enter,
        Enter2,
        StartCleaningCountClean,

        StartReseting,
        Reseting,
        ContinueReseting,
        EnterReseting,
        Enter2Reseting,
        EndReseting,

        EnterSolo,
        EndEnterSolo,

        GettingState,
        EndGettingState,
        ErrorGettingState,

        PendingCount,

        GettingSavedTransactions,
        EndGettingSavedTransactions,
    }

    public class TipoEstadoCtCoin
    {
        private bool _MotorMovimiento = false;

        public bool MotorMovimiento
        {
            get { return _MotorMovimiento; }
            set { _MotorMovimiento = value; }
        }
        private bool _MiddleDrawer = false;

        public bool MiddleDrawer
        {
            get { return _MiddleDrawer; }
            set { _MiddleDrawer = value; }
        }
        private bool _TransaccionPendiente = false;

        public bool TransaccionPendiente
        {
            get { return _TransaccionPendiente; }
            set { _TransaccionPendiente = value; }
        }
        private bool _ErrorSensorHead = false;

        public bool ErrorSensorHead
        {
            get { return _ErrorSensorHead; }
            set { _ErrorSensorHead = value; }
        }
        private bool _ErrorRailStop = false;

        public bool ErrorRailStop
        {
            get { return _ErrorRailStop; }
            set { _ErrorRailStop = value; }
        }
        private bool _SensorOutOfCalibration = false;

        public bool SensorOutOfCalibration
        {
            get { return _SensorOutOfCalibration; }
            set { _SensorOutOfCalibration = value; }
        }
        private bool _BowlOpen = false;

        public bool BowlOpen
        {
            get { return _BowlOpen; }
            set { _BowlOpen = value; }
        }
    }

    public enum TipoErrorCtCoin
    {
        No_error = 0,
        Printer_1_out_of_paper = 1,
        Printer_2_out_of_paper = 2,
        Error_on_printer_1 = 3,
        Error_on_printer_2 = 4,
        Maximum_of_500_transactions_reached = 5,
        Overflow_maximum_of_amount_reached = 6,
        At_least_one_full_bag = 7,
        Coins_left_in_bowl = 8,
        Railstop = 9,
        Machine_is_turned_on_with_an_unfinished_transaction = 10,
        Bowl_not_closed = 11,
        All_bags_are_filled_up = 12,
        Check_Sensor = 13,
        Sensor_in_right_position = 14,
        MASTER_error = 15,
        HOST_error = 16,
        Lost_funnel_position = 17,
    }

    public class CRC
    {

        static ushort[] crc_table = {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
            0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef
        };

        public byte[] CalculateCrc(byte[] data)
        {
            int i;
            ushort crc = 0;
            int len = data.Length;

            for (int j = 0; j < len; j++)
            {
                i = (crc >> 12) ^ (data[j] >> 4);
                crc = (ushort)(crc_table[i & 0x0F] ^ (crc << 4));
                i = (crc >> 12) ^ (data[j] >> 0);
                crc = (ushort)(crc_table[i & 0x0F] ^ (crc << 4));
            }



            byte[] bytes = BitConverter.GetBytes(crc);
            Array.Reverse(bytes);

            return bytes;


        }
    }

}
