using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.CashPaymentDevice
{
    public class Comunicacion
    {
        public ArrayList Datos = new ArrayList();
        private string texto;

        #region  Enums

        public enum TransmissionType { Text, Hex }

        public enum MessageType { Incoming, Outgoing, Normal, Warning, Error };

        #endregion  Enums

        #region  Variables

        private string _baudRate = string.Empty;
        private string _parity = string.Empty;
        private string _stopBits = string.Empty;
        private string _dataBits = string.Empty;
        private string _portName = string.Empty;
        private int _BytesThreshold = 1;
        private TransmissionType _transType;
        private SerialPort comPort = new SerialPort();

        #endregion  Variables

        #region  Properties

        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        public string StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public string DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        public TransmissionType CurrentTransmissionType
        {
            get { return _transType; }
            set { _transType = value; }
        }

        public int BytesThreshold
        {
            get { return _BytesThreshold; }
            set { comPort.ReceivedBytesThreshold = value; }
        }

        #endregion  Properties

        #region  Constructors

        public Comunicacion()
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _portName = "COM2";
            comPort.ReceivedBytesThreshold = BytesThreshold;
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        #endregion  Constructors

        #region WriteData

        public void WriteData(string msg)
        {
            switch (TransmissionType.Hex)
            {
                case TransmissionType.Text:

                    if (!(comPort.IsOpen == true))
                    {
                        comPort.PortName = "COM5";
                        comPort.Open();
                    }
                    comPort.Write(msg);

                    break;

                case TransmissionType.Hex:
                    try
                    {
                        if (!(comPort.IsOpen == true)) comPort.Open(); //ojo

                        byte[] newMsg = HexToByte(msg);

                        comPort.Write(newMsg, 0, newMsg.Length);
                    }
                    catch (Exception ex)
                    {
                        //Trace
                    }
                    finally
                    {
                        //Trace
                    }
                    break;

                default:

                    if (!(comPort.IsOpen == true)) comPort.Open();

                    comPort.Write(msg);

                    break;
            }
        }

        #endregion WriteData

        #region comPort_DataReceived

        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes = comPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            comPort.Read(comBuffer, 0, bytes);
            Datos.Clear();
            for (int i = 0; i < bytes; i++)
            {
                Datos.Add(comBuffer[i]);
            }
        }

        #endregion comPort_DataReceived

        public void LimpiarBuffer()
        {
            string str = string.Empty;
            try
            { str = comPort.ReadExisting(); }
            catch (Exception) { str = string.Empty; }
        }

        #region HexToByte

        private byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");

            byte[] comBuffer = new byte[msg.Length / 2];

            for (int i = 0; i < msg.Length; i += 2)

                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);

            return comBuffer;
        }

        #endregion HexToByte

        #region ByteToHex

        private string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            foreach (byte data in comByte)

                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

            return builder.ToString().ToUpper();
        }

        #endregion ByteToHex

        #region OpenPort

        public bool OpenPort(string puerto)
        {
            try
            {
                string sPuerto = string.Empty;

                sPuerto = puerto;

                if (comPort.IsOpen == true)
                {
                    comPort.Close();
                }

                comPort.BaudRate = 9600;
                comPort.DataBits = 8;
                comPort.StopBits = (StopBits)1;
                comPort.Parity = 0;
                comPort.PortName = sPuerto;
                comPort.ReceivedBytesThreshold = 8;

                //comPort.BaudRate = Convert.ToInt32(BaudRate);
                //comPort.DataBits = Convert.ToInt32(DataBits);
                //comPort.StopBits = (StopBits)Convert.ToInt32(StopBits);
                //comPort.Parity = (Parity)Convert.ToInt32(Parity);
                //comPort.PortName = PortName;
                //comPort.ReceivedBytesThreshold = BytesThreshold;

                comPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion OpenPort

        #region SetParityValues

        public void SetParityValues(object obj)
        {
        }

        #endregion SetParityValues

        #region SetStopBitValues

        public void SetStopBitValues(object obj)
        {
        }

        #endregion SetStopBitValues

        #region SetPortNameValues

        public void SetPortNameValues(object obj)
        {
        }

        #endregion SetPortNameValues
    }
}
