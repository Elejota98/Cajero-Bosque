using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.SmartPayOut.Classes;
using Ds.SmartPayOut.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ds.SmartPayOut
{
    public class SmartPayOutDeviceClass
    {
        public EventHandler DeviceMessagePayOutState;
        public EventHandler DeviceMessageSerialPort;
        private StatesPayOutDevice _StatesPayOut = new StatesPayOutDevice();
        private SerialPort _ComPort = new SerialPort();
        private string _Puerto = string.Empty;
        public string Puerto
        {
            get { return _Puerto; }
            set { _Puerto = value; }
        }
        private List<TipoBillete> _Bills = new List<TipoBillete>();
        volatile public bool payoutConnecting = false;
        CPayout Payout = new CPayout();
        CValidator Validator = new CValidator();
        public bool payoutRunning = false;
        TextBox textBox1 = new TextBox();
        bool FormSetup = false;
        System.Windows.Forms.Timer reconnectionTimer = new System.Windows.Forms.Timer();
        int reconnectionAttempts = 10;
        bool Running = false;
        bool bFormSetup = false;
        bool bConexionF17 = false;
        Thread tHopRec, tSPRec;
        delegate void OutputMessage(string msg);
        int Bill = 0;
        int reconnectionInterval = 3;
        volatile bool Connected = false, ConnectionFail = false;
        Thread ConnectionThread;
        bool bVARIABLEAPAGADO = false;


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            int bytes = _ComPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            _ComPort.Read(comBuffer, 0, bytes);
            EventArgsPayOutSerialCommunicationDevice evento = new EventArgsPayOutSerialCommunicationDevice(TipoInsertEventoPayOut.Recepcion, comBuffer);
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
                    switch (_StatesPayOut)
                    {

                        default:
                            break;
                    }



                    //string recepcion = string.Format("{0:X02}", (byte)recibo[5]) + string.Format("{0:X02}", (byte)recibo[6]) + string.Format("{0:X02}", (byte)recibo[7]);
                    //MensajesEstado(recepcion);
                }
            }
            catch (Exception)
            {
                _StatesPayOut = StatesPayOutDevice.ErrorConexion;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error Conexion Hoppers";
            }

            EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, e);
        }


        public async void ConectarAceptador(string COM)
        {
            _StatesPayOut = StatesPayOutDevice.Nothing;
            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            //reconnectionTimer.Tick += new EventHandler(reconnectionTimer_Tick);
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            Payout.Evento = string.Empty;
            _Puerto = COM;

            Validator.CommandStructure.ComPort = _Puerto;
            Validator.CommandStructure.SSPAddress = 0;
            Validator.CommandStructure.Timeout = 3000;

            if (ConnectToValidator())
            {
                Running = true;
                textBox1.AppendText("\r\nPoll Loop\r\n*********************************\r\n");
                bConexionF17 = true;
                //// Enable validators
                //Payout.EnableValidator();
                //Validator.EnableValidator();
                _StatesPayOut = StatesPayOutDevice.ConexionExitosa;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Conexion Exitosa Aceptador";

                await Task.Run(() =>
                {
                    while (Running)
                    {

                        try
                        {

                            if (!Validator.DoPoll(textBox1))
                            {
                                if (!bVARIABLEAPAGADO)
                                {
                                    textBox1.AppendText("Poll failed, attempting to reconnect...\r\n");
                                    Connected = false;
                                    ConnectionThread = new Thread(ConnectToValidatorThreaded);
                                    ConnectionThread.Start();
                                    while (!Connected)
                                    {
                                        if (ConnectionFail)
                                        {
                                            textBox1.AppendText("Failed to reconnect to validator\r\n");
                                            return;
                                        }
                                        Application.DoEvents();
                                    }
                                    textBox1.AppendText("Reconnected successfully\r\n");
                                }
                            }


                            UpdateUI();

                            #region Eventos PayOut
                            switch (Validator.Evento)
                            {

                                #region SSP_POLL_SLAVE_RESET
                                case "SSP_POLL_SLAVE_RESET":
                                    //General_Events = "Front End SSP_POLL_SLAVE_RESET";
                                    //if (!bSuspendido)
                                    //{
                                    //    General_Events = "Front End ERROR  SSP_POLL_SLAVE_RESET";
                                    //    SetearPantalla(Pantalla.SistemaSuspendido);
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_SLAVE_RESET;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_DISABLED
                                case "SSP_POLL_DISABLED":
                                    //if (!bSuspendido)
                                    //{
                                    //    bSuspendido = true;
                                    //    General_Events = "Front End ERROR  SSP_POLL_DISABLED";
                                    //    SetearPantalla(Pantalla.SistemaSuspendido);
                                    //}
                                    //if (Presentacion == Pantalla.PublicidadPrincipal)
                                    //{
                                    //    Recepcion();
                                    //}






                                    //_StatesPayOut = StatesPayOutDevice.SSP_POLL_DISABLED;
                                    //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    //oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_READ_NOTE
                                case "SSP_POLL_READ_NOTE":
                                    //General_Events = "Front End SSP_POLL_READ_NOTE";
                                    //General_Events = "FrontEnd SSP_POLL_READ_NOTE";
                                    //bIntento = true;
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_READ_NOTE;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    //oResultadoOperacion.Mensaje = "Recibiendo billete de " + Validator.Rspt;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_CREDIT_NOTE
                                case "SSP_POLL_CREDIT_NOTE":
                                    // General_Events = "Front End SSP_POLL_CREDIT_NOTE";
                                    //if (_BanderaRecaudo)
                                    //{
                                    //_Bills = new List<TipoBillete>();
                                    //Bill = 0;
                                    //Bill = Validator.Rspt;
                                    //Validator.Rspt = 0;
                                    //if (Bill != 0)
                                    //{
                                    //    _Bills.Add(new TipoBillete(Bill, 1));
                                    //    _StatesPayOut = StatesPayOutDevice.SSP_POLL_CREDIT_NOTE;
                                    //    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    //    oResultadoOperacion.Mensaje = "SSP_POLL_CREDIT_NOTE";
                                    //}


                                    //    if (Bill != 0)
                                    //    {
                                    //        Cnt_Reinicio = 0;
                                    //        General_Events = "FrontEnd Principal RegistrarMovimiento Entrada Billetero" + " Denominacion " + Bill;


                                    //    }

                                    //    SetearPantalla(Pantalla.DetallePago);
                                    //}
                                    //else
                                    //{
                                    //    if (_CargaBilletesBB)
                                    //    {
                                    //        Bill = 0;
                                    //        Bill = Payout.Rspt;
                                    //        Payout.Rspt = 0;
                                    //        if (Bill != 0)
                                    //        {
                                    //            Cnt_Reinicio = 0;
                                    //            General_Events = "FrontEnd Principal RegistrarMovimiento Entrada Carga Billetero" + " Denominacion " + Bill;
                                    //        }
                                    //    }
                                    //}

                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_REJECTING
                                case "SSP_POLL_NOTE_REJECTING":
                                    //General_Events = "Front End SSP_POLL_NOTE_REJECTING";
                                    //if (!bSuspendido)
                                    //{
                                    //    General_Events = "Front End ERROR  SSP_POLL_NOTE_REJECTING";
                                    //    SetearPantalla(Pantalla.SistemaSuspendido);
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_REJECTING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    //oResultadoOperacion.Mensaje = "SSP_POLL_NOTE_REJECTING";
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_REJECTED
                                case "SSP_POLL_NOTE_REJECTED":
                                    //General_Events = "Front End SSP_POLL_NOTE_REJECTED";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_REJECTED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_STACKING
                                case "SSP_POLL_NOTE_STACKING":
                                    //General_Events = "Front End SSP_POLL_NOTE_STACKING";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_STACKING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_FLOATING
                                case "SSP_POLL_FLOATING":
                                    //General_Events = "Front End SSP_POLL_FLOATING";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_FLOATING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_STACKED
                                case "SSP_POLL_NOTE_STACKED":
                                    //General_Events = "Front End SSP_POLL_NOTE_STACKED";
                                    //if (_CargaBilletesBB)
                                    //{
                                    //    if (_frmPrincipal_Presenter.RegistrarMovimiento(TipoOperacion.Carga, TipoParte.Box, TipoMovimiento.Entrada, null, Bill, 1))
                                    //    {
                                    //        _frmPrincipal_Presenter.ObtenerInfoPartes();
                                    //        SetearPantalla(Pantalla.MenuCargBilletes);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    _frmPrincipal_Presenter.RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Box, TipoMovimiento.Entrada, null, Bill, 1);
                                    //}


                                    _Bills = new List<TipoBillete>();
                                    Bill = 0;
                                    Bill = Validator.Rspt;
                                    Validator.Rspt = 0;
                                    if (Bill != 0)
                                    {
                                        _Bills.Add(new TipoBillete(Bill, 1));
                                        _StatesPayOut = StatesPayOutDevice.SSP_POLL_CREDIT_NOTE;
                                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                        oResultadoOperacion.Mensaje = textBox1.Text;
                                    }


                                    //_StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_STACKED;
                                    //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    //oResultadoOperacion.Mensaje = "SSP_POLL_NOTE_STACKED";
                                    break;
                                #endregion

                                #region SSP_POLL_FLOATED
                                case "SSP_POLL_FLOATED":
                                    //General_Events = "Front End SSP_POLL_FLOATED";
                                    //if (!bSuspendido)
                                    //{
                                    //    General_Events = "Front End ERROR  SH SSP_POLL_INCOMPLETE_PAYOUT";
                                    //    SetearPantalla(Pantalla.SistemaSuspendido);
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_FLOATED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_STORED_IN_PAYOUT
                                case "SSP_POLL_NOTE_STORED_IN_PAYOUT":
                                    //General_Events = "Front End SSP_POLL_NOTE_STORED_IN_PAYOUT";
                                    //if (_CargaBilletesBB)
                                    //{
                                    //    if (_frmPrincipal_Presenter.RegistrarMovimiento(TipoOperacion.Carga, TipoParte.Cassette, TipoMovimiento.Entrada, null, Bill, 1))
                                    //    {
                                    //        _frmPrincipal_Presenter.ObtenerInfoPartes();
                                    //        SetearPantalla(Pantalla.MenuCargBilletes);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    _frmPrincipal_Presenter.RegistrarMovimiento(TipoOperacion.Pago, TipoParte.Cassette, TipoMovimiento.Entrada, null, Bill, 1);
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_STORED_IN_PAYOUT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_SAFE_NOTE_JAM
                                case "SSP_POLL_SAFE_NOTE_JAM":
                                    //General_Events = "Front End SSP_POLL_SAFE_NOTE_JAM";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_SAFE_NOTE_JAM;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_UNSAFE_NOTE_JAM
                                case "SSP_POLL_UNSAFE_NOTE_JAM":
                                    //General_Events = "Front End SSP_POLL_UNSAFE_NOTE_JAM";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_UNSAFE_NOTE_JAM;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_ERROR_DURING_PAYOUT
                                case "SSP_POLL_ERROR_DURING_PAYOUT":
                                    //General_Events = "Front End SSP_POLL_ERROR_DURING_PAYOUT";
                                    //Presentacion = Pantalla.SistemaSuspendido;
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_ERROR_DURING_PAYOUT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_FRAUD_ATTEMPT
                                case "SSP_POLL_FRAUD_ATTEMPT":
                                    //General_Events = "Front End SSP_POLL_FRAUD_ATTEMPT";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_FRAUD_ATTEMPT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_STACKER_FULL
                                case "SSP_POLL_STACKER_FULL":
                                    //General_Events = "Front End SSP_POLL_STACKER_FULL";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_STACKER_FULL;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_CLEARED_FROM_FRONT
                                case "SSP_POLL_NOTE_CLEARED_FROM_FRONT":
                                    //General_Events = "Front End SSP_POLL_NOTE_CLEARED_FROM_FRONT";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_CLEARED_FROM_FRONT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_CLEARED_TO_CASHBOX
                                case "SSP_POLL_NOTE_CLEARED_TO_CASHBOX":
                                    //General_Events = "Front End SSP_POLL_NOTE_CLEARED_TO_CASHBOX";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_CLEARED_TO_CASHBOX;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP
                                case "SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP":
                                    //General_Events = "Front End SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_PAID_INTO_STORE_AT_POWER_UP;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP
                                case "SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP":
                                    //General_Events = "Front End SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_PAID_INTO_STACKER_AT_POWER_UP;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_CASHBOX_REMOVED
                                case "SSP_POLL_CASHBOX_REMOVED":
                                    //General_Events = "Front End SSP_POLL_CASHBOX_REMOVED";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_CASHBOX_REMOVED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_CASHBOX_REPLACED
                                case "SSP_POLL_CASHBOX_REPLACED":
                                    //General_Events = "Front End SSP_POLL_CASHBOX_REPLACED";
                                    //btn_ConfirmarArqueo.Visible = true;
                                    //btn_ConfirmarArqueoTotal.Visible = true;
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_CASHBOX_REPLACED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion

                                #region SSP_POLL_DISPENSING
                                case "DISPENSING":
                                    //General_Events = "Front End SSP_POLL_DISPENSING";
                                    _StatesPayOut = StatesPayOutDevice.DISPENSING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "DISPENSING";
                                    break;
                                #endregion

                                #region SSP_POLL_DISPENSED
                                case "DISPENSED":
                                    //General_Events = "Front End SSP_POLL_DISPENSED";
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
                                    //        General_Events = "FrontEnd SSP_POLL_DISPENSED";
                                    //        bIntentob = true;
                                    //        _frmPrincipal_Presenter.ProcesoPagoBilletes(_PagoEfectivo, TipoMedioPago.Moneda);
                                    //    }
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.DISPENSED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "DISPENSED";
                                    break;
                                #endregion

                                #region SSP_POLL_EMPTYING
                                case "SSP_POLL_EMPTYING":
                                    //General_Events = "Front End SSP_POLL_EMPTYING";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_EMPTYING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_EMPTYING";
                                    break;
                                #endregion

                                #region SSP_POLL_EMPTIED
                                case "SSP_POLL_EMPTIED":
                                    //General_Events = "Front End SSP_POLL_EMPTIED";
                                    //Presentacion = Pantalla.ArqueoTotal;
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_EMPTIED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_EMPTIED";
                                    break;
                                #endregion

                                #region SSP_POLL_SMART_EMPTYING
                                case "SSP_POLL_SMART_EMPTYING":
                                    //General_Events = "Front End SSP_POLL_SMART_EMPTYING";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_SMART_EMPTYING;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_SMART_EMPTYING";
                                    break;
                                #endregion

                                #region SSP_POLL_SMART_EMPTIED
                                case "SSP_POLL_SMART_EMPTIED":
                                    //General_Events = "Front End SSP_POLL_SMART_EMPTIED";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_SMART_EMPTIED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_SMART_EMPTIED";
                                    break;
                                #endregion

                                #region SSP_POLL_JAMMED
                                case "SSP_POLL_JAMMED":
                                    //General_Events = "Front End SSP_POLL_JAMMED";
                                    //if (!bSuspendido)
                                    //{
                                    //    bSuspendido = true;
                                    //    General_Events = "Front End ERROR SSP_POLL_JAMMED";
                                    //    SetearPantalla(Pantalla.SistemaSuspendido);
                                    //}
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_JAMMED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_JAMMED";
                                    break;
                                #endregion

                                #region SSP_POLL_HALTED
                                case "SSP_POLL_HALTED":
                                    //General_Events = "Front End SSP_POLL_HALTED";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_HALTED;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_HALTED";
                                    break;
                                #endregion

                                #region SSP_POLL_INCOMPLETE_PAYOUT
                                case "SSP_POLL_INCOMPLETE_PAYOUT":
                                    //General_Events = "Front End SSP_POLL_INCOMPLETE_PAYOUT";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_INCOMPLETE_PAYOUT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_INCOMPLETE_PAYOUT";
                                    break;
                                #endregion

                                #region SSP_POLL_INCOMPLETE_FLOAT
                                case "SSP_POLL_INCOMPLETE_FLOAT":
                                    //General_Events = "Front End SSP_POLL_INCOMPLETE_FLOAT";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_INCOMPLETE_FLOAT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_INCOMPLETE_FLOAT";
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_TRANSFERED_TO_STACKER
                                case "SSP_POLL_NOTE_TRANSFERED_TO_STACKER":
                                    //General_Events = "Front End SSP_POLL_NOTE_TRANSFERED_TO_STACKER";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_TRANSFERED_TO_STACKER;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_NOTE_TRANSFERED_TO_STACKER";
                                    break;
                                #endregion

                                #region SSP_POLL_NOTE_HELD_IN_BEZEL
                                case "SSP_POLL_NOTE_HELD_IN_BEZEL":
                                    //General_Events = "Front End SSP_POLL_NOTE_HELD_IN_BEZEL";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_NOTE_HELD_IN_BEZEL;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_NOTE_HELD_IN_BEZEL";
                                    break;
                                #endregion

                                #region SSP_POLL_TIME_OUT
                                case "SSP_POLL_TIME_OUT":
                                    //General_Events = "Front End SSP_POLL_TIME_OUT";
                                    _StatesPayOut = StatesPayOutDevice.SSP_POLL_TIME_OUT;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.Mensaje = "SSP_POLL_TIME_OUT";
                                    break;
                                #endregion

                                #region Default
                                default:
                                    oResultadoOperacion.Mensaje = textBox1.Text;
                                    break;
                                #endregion
                            }
                            #endregion

                            Application.DoEvents();
                        }
                        catch (Exception ex)
                        {

                        }

                        EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
                        DeviceMessagePayOutState(this, e);
                    }

                });
            }
            else
            {
                //bConexionF17 = false;
            }

            EventArgsPayOutDevice ee = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, ee);
        }
        public bool ConnectToSMARTPayout(TextBox log = null)
        {
            payoutConnecting = true;
            // setup timer, timeout delay and number of attempts to connect
            System.Windows.Forms.Timer reconnectionTimer = new System.Windows.Forms.Timer();
            reconnectionTimer.Tick += new EventHandler(reconnectionTimer_Tick);
            reconnectionTimer.Interval = 1000; // ms
            int attempts = 10;

            // Setup connection info
            Payout.CommandStructure.ComPort = _Puerto;
            Payout.CommandStructure.SSPAddress = 0;
            Payout.CommandStructure.BaudRate = 9600;
            Payout.CommandStructure.Timeout = 1000;
            Payout.CommandStructure.RetryLevel = 3;

            // Run for number of attempts specified
            for (int i = 0; i < attempts; i++)
            {
                if (log != null) log.AppendText("Trying connection to SMART Payout\r\n");

                // turn encryption off for first stage
                Payout.CommandStructure.EncryptionStatus = false;

                // Open port first, if the key negotiation is successful then set the rest up
                if (Payout.OpenPort() && Payout.NegotiateKeys(log))
                {
                    Payout.CommandStructure.EncryptionStatus = true; // now encrypting
                    // find the max protocol version this validator supports
                    byte maxPVersion = FindMaxPayoutProtocolVersion();
                    if (maxPVersion >= 6)
                        Payout.SetProtocolVersion(maxPVersion, log);
                    else
                    {
                        //MessageBox.Show("This program does not support units under protocol 6!", "ERROR");
                        payoutConnecting = false;
                        return false;
                    }
                    // get info from the validator and store useful vars
                    Payout.PayoutSetupRequest(log);
                    // check the right unit is connected
                    if (!IsValidatorSupported(Payout.UnitType))
                    {
                        //MessageBox.Show("Unsupported type shown by SMART Payout, this SDK supports the SMART Payout and the SMART Hopper only");
                        payoutConnecting = false;
                        //Application.Exit();
                        return false;
                    }
                    // inhibits, this sets which channels can receive notes
                    Payout.SetInhibits(log);
                    // Get serial number.
                    Payout.GetSerialNumber(log);
                    // enable payout
                    Payout.EnablePayout(log);
                    // set running to true so the validator begins getting polled
                    payoutRunning = true;
                    payoutConnecting = false;
                    return true;
                }
                // reset timer
                reconnectionTimer.Enabled = true;
                while (reconnectionTimer.Enabled)
                {
                    if (CHelpers.Shutdown)
                    {
                        payoutConnecting = false;
                        return false;
                    }

                    Application.DoEvents();
                    Thread.Sleep(1);
                }
            }
            payoutConnecting = false;
            return false;
        }
        private bool ConnectToValidator()
        {
            // setup the timer
            reconnectionTimer.Interval = reconnectionInterval * 1000; // for ms

            // run for number of attempts specified
            for (int i = 0; i < reconnectionAttempts; i++)
            {
                // reset timer
                reconnectionTimer.Enabled = true;

                // close com port in case it was open
                Validator.SSPComms.CloseComPort();

                // turn encryption off for first stage
                Validator.CommandStructure.EncryptionStatus = false;

                // open com port and negotiate keys
                if (Validator.OpenComPort(textBox1) && Validator.NegotiateKeys(textBox1))
                {
                    Validator.CommandStructure.EncryptionStatus = true; // now encrypting
                    // find the max protocol version this validator supports
                    byte maxPVersion = FindMaxProtocolVersion();
                    if (maxPVersion > 6)
                    {
                        Validator.SetProtocolVersion(maxPVersion, textBox1);
                    }
                    else
                    {
                        //MessageBox.Show("This program does not support units under protocol version 6, update firmware.", "ERROR");
                        return false;
                    }
                    // get info from the validator and store useful vars
                    Validator.ValidatorSetupRequest(textBox1);
                    // Get Serial number
                    Validator.GetSerialNumber(textBox1);
                    // check this unit is supported by this program
                    if (!IsUnitTypeSupported(Validator.UnitType))
                    {
                        //MessageBox.Show("Unsupported unit type, this SDK supports the BV series and the NV series (excluding the NV11)");
                        Application.Exit();
                        return false;
                    }
                    // inhibits, this sets which channels can receive notes
                    Validator.SetInhibits(textBox1);
                    // enable, this allows the validator to receive and act on commands
                    //Validator.EnableValidator(textBox1);

                    return true;
                }
                while (reconnectionTimer.Enabled) Application.DoEvents(); // wait for reconnectionTimer to tick
            }
            return false;
        }
        public void Disable(string log)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                Validator.DisableValidator();
                Thread.Sleep(500);
                if (log == "OFF")
                {
                    bVARIABLEAPAGADO = true;
                }
                _StatesPayOut = StatesPayOutDevice.DisableValidator;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "DisableValidator";

            }
            catch (Exception)
            {
                _StatesPayOut = StatesPayOutDevice.DisableValidatorError;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error validador";
            }

            EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, e);
        }
        public void ACK()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {

                _StatesPayOut = StatesPayOutDevice.Nothing;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Nothing";
            }
            catch (Exception)
            {
                _StatesPayOut = StatesPayOutDevice.Nothing;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error validador";
            }

            EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, e);
        }
        public void Halt()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                textBox1.AppendText("Poll loop stopped\r\n");
                Running = false;
                _StatesPayOut = StatesPayOutDevice.Nothing;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Nothing";
            }
            catch (Exception)
            {
                _StatesPayOut = StatesPayOutDevice.Nothing;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error validador";
            }

            EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, e);
        }
        public void EnableValidator(string log)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {

                Validator.EnableValidator();
                if (log == "ON")
                {
                    bVARIABLEAPAGADO = false;
                }
                _StatesPayOut = StatesPayOutDevice.EnableValidator;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "EnableValidator ok";
            }
            catch (Exception)
            {
                _StatesPayOut = StatesPayOutDevice.ErrorValidador;
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Error validador";
            }

            EventArgsPayOutDevice e = new EventArgsPayOutDevice(_StatesPayOut, _Bills, oResultadoOperacion);
            DeviceMessagePayOutState(this, e);
        }

        private void ConnectToValidatorThreaded()
        {
            // setup the timer
            reconnectionTimer.Interval = reconnectionInterval * 1000; // for ms

            // run for number of attempts specified
            for (int i = 0; i < reconnectionAttempts; i++)
            {
                // reset timer
                reconnectionTimer.Enabled = true;

                // close com port in case it was open
                Validator.SSPComms.CloseComPort();

                // turn encryption off for first stage
                Validator.CommandStructure.EncryptionStatus = false;

                // open com port and negotiate keys
                if (Validator.OpenComPort() && Validator.NegotiateKeys())
                {
                    Validator.CommandStructure.EncryptionStatus = true; // now encrypting
                    // find the max protocol version this validator supports
                    byte maxPVersion = FindMaxProtocolVersion();
                    if (maxPVersion > 6)
                    {
                        Validator.SetProtocolVersion(maxPVersion);
                    }
                    else
                    {
                        //MessageBox.Show("This program does not support units under protocol version 6, update firmware.", "ERROR");
                        Connected = false;
                        return;
                    }
                    // get info from the validator and store useful vars
                    Validator.ValidatorSetupRequest();
                    // inhibits, this sets which channels can receive notes
                    Validator.SetInhibits();
                    // enable, this allows the validator to operate
                    Validator.EnableValidator(textBox1);

                    Connected = true;
                    return;
                }
                while (reconnectionTimer.Enabled) Application.DoEvents(); // wait for reconnectionTimer to tick
            }
            Connected = false;
            ConnectionFail = true;
        }
        private void reconnectionTimer_Tick(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.Timer)
            {
                System.Windows.Forms.Timer t = sender as System.Windows.Forms.Timer;
                t.Enabled = false;
            }
        }
        private byte FindMaxProtocolVersion()
        {
            // not dealing with protocol under level 6
            // attempt to set in validator
            byte b = 0x06;
            while (true)
            {
                Validator.SetProtocolVersion(b);
                if (Validator.CommandStructure.ResponseData[0] == CCommands.SSP_RESPONSE_FAIL)
                    return --b;
                b++;
                if (b > 20)
                    return 0x06; // return default if protocol 'runs away'
            }
        }
        private bool IsValidatorSupported(char type)
        {
            if (type == (char)0x06)
                return true;
            return false;
        }
        void UpdateUI()
        {
            string levelPayOut = Payout.GetChannelLevelInfo();
        }
        private bool IsUnitValid(char unitType)
        {
            if (unitType == (char)0x06) // 0x06 is Payout, no other types supported by this program
                return true;
            return false;
        }
        private byte FindMaxPayoutProtocolVersion()
        {
            // not dealing with protocol under level 6
            // attempt to set in validator
            byte b = 0x06;
            while (true)
            {
                Payout.SetProtocolVersion(b);
                // If it fails then it can't be set so fall back to previous iteration and return it
                if (Payout.CommandStructure.ResponseData[0] == CCommands.SSP_RESPONSE_FAIL)
                    return --b;
                b++;

                // If the protocol version 'runs away' because of a drop in comms. Return the default value.
                if (b > 20)
                    return 0x06;
            }
        }
        private void ReconnectPayout()
        {
            OutputMessage m = new OutputMessage(AppendToTextBox);
            while (!payoutRunning)
            {
                if (textBox1.InvokeRequired)
                    textBox1.Invoke(m, new object[] { "Attempting to reconnect to SMART Payout...\r\n" });
                else
                    textBox1.AppendText("Attempting to reconnect to SMART Payout...\r\n");

                ConnectToSMARTPayout(null); // Have to pass null as can't update text box from a different thread without invoking

                CHelpers.Pause(1000);
                if (CHelpers.Shutdown) return;
            }
            if (textBox1.InvokeRequired)
                textBox1.Invoke(m, new object[] { "Reconnected to SMART Payout\r\n" });
            else
                textBox1.AppendText("Reconnected to SMART Payout\r\n");
            //Payout.EnableValidator();
        }
        private void AppendToTextBox(string s)
        {
            textBox1.AppendText(s);
        }
        private bool IsUnitTypeSupported(char type)
        {
            if (type == (char)0x00)
                return true;
            return false;
        }
    }

    public class EventArgsPayOutDevice : EventArgs
    {
        private StatesPayOutDevice _result;

        public StatesPayOutDevice result
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

        private List<TipoBillete> _BilletesBox = new List<TipoBillete>();
        public List<TipoBillete> BilletesBox
        {
            get { return _BilletesBox; }
            set { _BilletesBox = value; }
        }

        public EventArgsPayOutDevice(StatesPayOutDevice oStatesPayOut, List<TipoBillete> lstTipoBillete, ResultadoOperacion oResultadoOperacion)
        {
            _result = oStatesPayOut;
            _resultString = oResultadoOperacion;
            _BilletesBox = lstTipoBillete;
        }
    }

    public class TipoBillete
    {
        public TipoBillete(int iDenominacion, int iCantidad)
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

    public class EventArgsPayOutSerialCommunicationDevice : EventArgs
    {
        private TipoInsertEventoPayOut _TipoInsertEvento;
        private byte[] _ByteArray;

        public TipoInsertEventoPayOut TipoInsertEvento
        {
            get { return _TipoInsertEvento; }
            set { _TipoInsertEvento = value; }
        }

        public byte[] ByteArray
        {
            get { return _ByteArray; }
            set { _ByteArray = value; }
        }

        public EventArgsPayOutSerialCommunicationDevice(TipoInsertEventoPayOut oTipoInsertEvento, byte[] oByteArray)
        {
            _TipoInsertEvento = oTipoInsertEvento;
            _ByteArray = oByteArray;
        }
    }
}
