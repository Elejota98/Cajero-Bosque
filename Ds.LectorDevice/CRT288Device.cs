using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using EGlobalT.Device.SmartCard;
using GS.Util.Hex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.LectorDevice
{
    public class CRT288DeviceClass
    {
        UInt32 Hdle = 0;
        byte CPUType = 2;

        public EventHandler DeviceMessageCRTState;
        StatesLector _StatesLector = new StatesLector();

        public ResultadoOperacion ConectarLector(string sPuerto)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                Hdle = DllClass.CommOpen(sPuerto);
                if (Hdle != 0)
                {
                    _StatesLector = StatesLector.Conexion_Exitosa;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Conexion Exitosa Lector";
                }
                else
                {
                    _StatesLector = StatesLector.Error_Conexion;
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error de conexion";
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion DesconectarLector()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    int i = DllClass.CommClose(Hdle);
                    Hdle = 0;
                    _StatesLector = StatesLector.Desconexion_Exitosa;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Desconexion Exitosa Lector";
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }

        public ResultadoOperacion ObtenerIdTarjeta()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    string RfSNlABEL = "";
                    byte[] _CardID = new byte[4];
                    byte _CardIDLen = 0;
                    int i = -1;
                    i = DllClass.RF_GetCardID(Hdle, ref _CardIDLen, _CardID);

                    if (i == 0)
                    {
                        int n;
                        string StrBuf = "";

                        for (n = 0; n < 4; n++)
                        {
                            StrBuf += _CardID[n].ToString("X2");
                        }

                        RfSNlABEL = StrBuf;

                        _StatesLector = StatesLector.ObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = RfSNlABEL;
                        oResultadoOperacion.EntidadDatos = RfSNlABEL;
                        //MessageBox.Show("Card S/N OK", "Card S/N");
                    }
                    else if (i == 69)
                    {
                        _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Caution");
                    }
                    else if (i == 87)
                    {
                        _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                        //MessageBox.Show("The card is not on the card operation position", "Caution");
                    }
                    else if (i == 78)
                    {
                        _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = "Excute Command Error";
                        //MessageBox.Show("Excute Command Error", "Caution");
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                        oResultadoOperacion.oEstado = TipoRespuesta.Error;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
                else
                {
                    _StatesLector = StatesLector.ErrorObtenerIdTarjeta;
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Comm. port is not Opened";
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion CheckPass(string Pass, byte SecNo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    byte[] PassWordData = new byte[6];
                    string PasswordStr = Pass;
                    byte RfType = 0;
                    byte KEYLen = 6;

                    PassWordData[0] = (byte)Convert.ToInt32(PasswordStr.Substring(0, 2), 16);
                    PassWordData[1] = (byte)Convert.ToInt32(PasswordStr.Substring(3, 2), 16);
                    PassWordData[2] = (byte)Convert.ToInt32(PasswordStr.Substring(6, 2), 16);
                    PassWordData[3] = (byte)Convert.ToInt32(PasswordStr.Substring(9, 2), 16);
                    PassWordData[4] = (byte)Convert.ToInt32(PasswordStr.Substring(12, 2), 16);
                    PassWordData[5] = (byte)Convert.ToInt32(PasswordStr.Substring(15, 2), 16);

                    RfType = 0;
                    int i = DllClass.RF_LoadSecKey(Hdle, SecNo, RfType, KEYLen, PassWordData);

                    if (i == 0)
                    {
                        _StatesLector = StatesLector.CheckPassOK;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Verify Password OK";
                        //MessageBox.Show("Verify Password OK", "Verify Password");
                    }
                    else if (i == 0x30)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No RF Card In";
                        //MessageBox.Show("No RF Card In", "Verify Password");
                    }
                    else if (i == 0x31)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Sector Error";
                        //MessageBox.Show("Sector Error", "Verify Password");
                    }
                    else if (i == 0x32)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "S/N Error";
                        //MessageBox.Show("S/N Error", "Verify Password");
                    }
                    else if (i == 0x33)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Password Error";
                        //MessageBox.Show("Password Error", "Verify Password");
                    }
                    else if (i == 0x34)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Block Error";
                        //MessageBox.Show("Block Error", "Verify Password");
                    }
                    else if (i == 0x35)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Value overflow";
                        //MessageBox.Show("Value overflow", "Verify Password");
                    }
                    else if (i == 69)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Caution");
                    }
                    else if (i == 87)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                        //MessageBox.Show("The card is not on the card operation position", "Caution");
                    }
                    else if (i == 78)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Excute Command Error";
                        //MessageBox.Show("Excute Command Error", "Caution");
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
                else
                {
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion leerTarjetaIndividual(byte SecNo, byte BlockNo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    byte[] DataBlock = new byte[16];
                    byte DataBlockLen = 0;
                    string RftextBox1 = "";
                    //RF_ReadBlock(UInt32 ComHandle, byte _Sec, byte _Block, ref byte _BlockDataLen, byte[] _BlockData);
                    int i = DllClass.RF_ReadBlock(Hdle, SecNo, BlockNo, ref DataBlockLen, DataBlock);

                    if (i == 0)
                    {
                        int n;
                        string StrBuf = "";

                        for (n = 0; n < 16; n++)
                        {
                            StrBuf += DataBlock[n].ToString("X2") + " ";
                        }

                        RftextBox1 = StrBuf;

                        _StatesLector = StatesLector.LecturaOK;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = RftextBox1;
                        //MessageBox.Show("Read Data OK", "Read Data");
                    }
                    else if (i == 0x30)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No RF Card In";
                        //MessageBox.Show("No RF Card In", "Read Data");
                    }
                    else if (i == 0x31)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Sector Error";
                        //MessageBox.Show("Sector Error", "Read Data");
                    }
                    else if (i == 0x32)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "S/N Error";
                        //MessageBox.Show("S/N Error", "Read Data");
                    }
                    else if (i == 0x33)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Password Error";
                        //MessageBox.Show("Password Error", "Read Data");
                    }
                    else if (i == 0x34)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Block Error";
                        //MessageBox.Show("Block Error", "Read Data");
                    }
                    else if (i == 0x35)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Value overflow";
                        //MessageBox.Show("Value overflow", "Read Data");
                    }
                    else if (i == 69)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Caution");
                    }
                    else if (i == 87)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                        //MessageBox.Show("The card is not on the card operation position", "Caution");
                    }
                    else if (i == 78)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Excute Command Error";
                        //MessageBox.Show("Excute Command Error", "Caution");
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.LecturaError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
                else
                {
                    _StatesLector = StatesLector.LecturaError;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Comm. port is not Opened";
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion leerTarjeta(string Pass, string code)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    byte[] PassWordData = new byte[6];
                    string PasswordStr = Pass;
                    byte RfType = 0;
                    byte KEYLen = 6;

                    PassWordData[0] = (byte)Convert.ToInt32(PasswordStr.Substring(0, 2), 16);
                    PassWordData[1] = (byte)Convert.ToInt32(PasswordStr.Substring(3, 2), 16);
                    PassWordData[2] = (byte)Convert.ToInt32(PasswordStr.Substring(6, 2), 16);
                    PassWordData[3] = (byte)Convert.ToInt32(PasswordStr.Substring(9, 2), 16);
                    PassWordData[4] = (byte)Convert.ToInt32(PasswordStr.Substring(12, 2), 16);
                    PassWordData[5] = (byte)Convert.ToInt32(PasswordStr.Substring(15, 2), 16);

                    RfType = 0;
                    int i = DllClass.RF_LoadSecKey(Hdle, 1, RfType, KEYLen, PassWordData);

                    if (i == 0)
                    {
                        byte[] DataBlock = new byte[16];
                        byte DataBlockLen = 0;
                        string RftextBox1 = "";
                        int a = DllClass.RF_ReadBlock(Hdle, 1, 0, ref DataBlockLen, DataBlock);
                        if (a == 0)
                        {
                            int n;
                            string StrBuf = "";

                            for (n = 0; n < 16; n++)
                            {
                                StrBuf += DataBlock[n].ToString("X2") + " ";
                            }

                            //bloque 0 sector 1
                            RftextBox1 = StrBuf;
                            string[] Sector1Bloque0 = RftextBox1.Split(' ');


                            byte Byte0 = Convert.ToByte(Sector1Bloque0[0], 16);
                            byte Byte1 = Convert.ToByte(Sector1Bloque0[1], 16);
                            byte Byte2 = Convert.ToByte(Sector1Bloque0[2], 16);
                            byte Byte3 = Convert.ToByte(Sector1Bloque0[3], 16);
                            byte Byte4 = Convert.ToByte(Sector1Bloque0[4], 16);
                            byte Byte5 = Convert.ToByte(Sector1Bloque0[5], 16);
                            byte Byte6 = Convert.ToByte(Sector1Bloque0[6], 16);
                            byte Byte7 = Convert.ToByte(Sector1Bloque0[7], 16);
                            byte Byte8 = Convert.ToByte(Sector1Bloque0[8], 16);
                            byte Byte9 = Convert.ToByte(Sector1Bloque0[9], 16);
                            byte Byte10 = Convert.ToByte(Sector1Bloque0[10], 16);
                            byte Byte11 = Convert.ToByte(Sector1Bloque0[11], 16);
                            byte Byte12 = Convert.ToByte(Sector1Bloque0[12], 16);
                            byte Byte13 = Convert.ToByte(Sector1Bloque0[13], 16);
                            byte Byte14 = Convert.ToByte(Sector1Bloque0[14], 16);
                            byte Byte15 = Convert.ToByte(Sector1Bloque0[15], 16);

                            Tarjeta oTarjeta = new Tarjeta();

                            string INPUT = Convert.ToString(Byte1, 2);
                            INPUT = INPUT.PadLeft(8, '0');

                            string[] OUTPUT = INPUT.ToCharArray().Select(c => c.ToString()).ToArray();


                            string SEXO = OUTPUT[0];
                            string ACTUALIZACIONTARJETA = OUTPUT[1];
                            string REPAGO = OUTPUT[2];
                            string MOTO = OUTPUT[3];
                            string PAGO = OUTPUT[4];
                            string CORTESIA = OUTPUT[5];
                            string REPOSICION = OUTPUT[6];
                            string CILCOACTIVO = OUTPUT[7];

                            if (Byte0 == 1)
                            {

                                string ModuloEntrada = HexAsciiConvert(Sector1Bloque0[8].ToString() + Sector1Bloque0[9].ToString() + Sector1Bloque0[10].ToString() + Sector1Bloque0[11].ToString() + Sector1Bloque0[12].ToString() + Sector1Bloque0[13].ToString() + Sector1Bloque0[14].ToString() + Sector1Bloque0[15].ToString());

                                string AÑO = "20" + Byte2;
                                string MES = Byte3.ToString();
                                string DIA = Byte4.ToString();

                                string Ho = Byte5.ToString();
                                string Min = Byte6.ToString();
                                string Segu = Byte7.ToString();



                                string Fecha = AÑO + "/" + MES.PadLeft(2, '0') + "/" + DIA.PadLeft(2, '0') + " " + Ho.PadLeft(2, '0') + ":" + Min.PadLeft(2, '0') + ":" + Segu.PadLeft(2, '0');

                                if (Fecha != "0/0/0 0:0:0")
                                {
                                    oTarjeta.DateTimeEntrance = Convert.ToDateTime(Fecha);
                                }

                                oTarjeta.EntranceModule = ModuloEntrada;
                            }



                            oTarjeta.ActiveCycle = Convert.ToBoolean(Convert.ToInt32(CILCOACTIVO));
                            oTarjeta.Courtesy = Convert.ToBoolean(Convert.ToInt32(CORTESIA));
                            oTarjeta.Replacement = Convert.ToBoolean(Convert.ToInt32(REPOSICION));
                            
                            oTarjeta.CodeCard = code;



                            string RftextBox12 = "";
                            int b = DllClass.RF_ReadBlock(Hdle, 1, 2, ref DataBlockLen, DataBlock);
                            if (b == 0)
                            {
                                int n2;
                                string StrBuf2 = "";

                                for (n2 = 0; n2 < 16; n2++)
                                {
                                    StrBuf2 += DataBlock[n2].ToString("X2") + " ";
                                }

                                //bloque 0 sector 1
                                RftextBox12 = StrBuf2;
                                string[] Sector1Bloque2 = RftextBox12.Split(' ');

                                byte Byte20 = Convert.ToByte(Sector1Bloque2[0], 16);
                                byte Byte21 = Convert.ToByte(Sector1Bloque2[1], 16);
                                byte Byte22 = Convert.ToByte(Sector1Bloque2[2], 16);
                                byte Byte23 = Convert.ToByte(Sector1Bloque2[3], 16);
                                byte Byte24 = Convert.ToByte(Sector1Bloque2[4], 16);
                                byte Byte25 = Convert.ToByte(Sector1Bloque2[5], 16);
                                byte Byte26 = Convert.ToByte(Sector1Bloque2[6], 16);
                                byte Byte27 = Convert.ToByte(Sector1Bloque2[7], 16);
                                byte Byte28 = Convert.ToByte(Sector1Bloque2[8], 16);
                                byte Byte29 = Convert.ToByte(Sector1Bloque2[9], 16);
                                byte Byte210 = Convert.ToByte(Sector1Bloque2[10], 16);
                                byte Byte211 = Convert.ToByte(Sector1Bloque2[11], 16);
                                byte Byte212 = Convert.ToByte(Sector1Bloque2[12], 16);
                                byte Byte213 = Convert.ToByte(Sector1Bloque2[13], 16);
                                byte Byte214 = Convert.ToByte(Sector1Bloque2[14], 16);
                                byte Byte215 = Convert.ToByte(Sector1Bloque2[15], 16);

                                oTarjeta.CodeAgreement1 = Byte21;


                                string RftextBox13 = "";
                                int c = DllClass.RF_ReadBlock(Hdle, 1, 1, ref DataBlockLen, DataBlock);
                                if (c == 0)
                                {
                                    int n3;
                                    string StrBuf3 = "";

                                    for (n3 = 0; n3 < 16; n3++)
                                    {
                                        StrBuf3 += DataBlock[n3].ToString("X2") + " ";
                                    }

                                    //bloque 0 sector 1
                                    RftextBox13 = StrBuf3;
                                    string[] Sector1Bloque1 = RftextBox13.Split(' ');

                                    byte Byte30 = Convert.ToByte(Sector1Bloque1[0], 16);
                                    byte Byte31 = Convert.ToByte(Sector1Bloque1[1], 16);
                                    byte Byte32 = Convert.ToByte(Sector1Bloque1[2], 16);
                                    byte Byte33 = Convert.ToByte(Sector1Bloque1[3], 16);
                                    byte Byte34 = Convert.ToByte(Sector1Bloque1[4], 16);
                                    byte Byte35 = Convert.ToByte(Sector1Bloque1[5], 16);
                                    byte Byte36 = Convert.ToByte(Sector1Bloque1[6], 16);
                                    byte Byte37 = Convert.ToByte(Sector1Bloque1[7], 16);
                                    byte Byte38 = Convert.ToByte(Sector1Bloque1[8], 16);
                                    byte Byte39 = Convert.ToByte(Sector1Bloque1[9], 16);
                                    byte Byte310 = Convert.ToByte(Sector1Bloque1[10], 16);
                                    byte Byte311 = Convert.ToByte(Sector1Bloque1[11], 16);
                                    byte Byte312 = Convert.ToByte(Sector1Bloque1[12], 16);
                                    byte Byte313 = Convert.ToByte(Sector1Bloque1[13], 16);
                                    byte Byte314 = Convert.ToByte(Sector1Bloque1[14], 16);
                                    byte Byte315 = Convert.ToByte(Sector1Bloque1[15], 16);

                                    if (Byte38 == 1)
                                    {
                                        oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                    }
                                    else
                                    {
                                        oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                    }


                                    _StatesLector = StatesLector.LecturaOK;
                                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                    oResultadoOperacion.EntidadDatos = oTarjeta;
                                    oResultadoOperacion.Mensaje = RftextBox1;

                                }



                                _StatesLector = StatesLector.LecturaOK;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.EntidadDatos = oTarjeta;
                                oResultadoOperacion.Mensaje = RftextBox1;

                            }
                            else if (b == 0x30)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "No RF Card In";
                                //MessageBox.Show("No RF Card In", "Read Data");
                            }
                            else if (b == 0x31)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Sector Error";
                                //MessageBox.Show("Sector Error", "Read Data");
                            }
                            else if (b == 0x32)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "S/N Error";
                                //MessageBox.Show("S/N Error", "Read Data");
                            }
                            else if (b == 0x33)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Password Error";
                                //MessageBox.Show("Password Error", "Read Data");
                            }
                            else if (b == 0x34)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Block Error";
                                //MessageBox.Show("Block Error", "Read Data");
                            }
                            else if (b == 0x35)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Value overflow";
                                //MessageBox.Show("Value overflow", "Read Data");
                            }
                            else if (b == 69)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "No Card In";
                                //MessageBox.Show("No Card In", "Caution");
                            }
                            else if (b == 87)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                                //MessageBox.Show("The card is not on the card operation position", "Caution");
                            }
                            else if (b == 78)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Excute Command Error";
                                //MessageBox.Show("Excute Command Error", "Caution");
                            }
                            else if (b == -1)
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "Communication Error";
                                //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                            }
                            else
                            {
                                _StatesLector = StatesLector.LecturaError;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "unknow Error";
                                //MessageBox.Show("unknow Error", "Caution");
                            }

                        }
                        else if (a == 0x30)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "No RF Card In";
                            //MessageBox.Show("No RF Card In", "Read Data");
                        }
                        else if (a == 0x31)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Sector Error";
                            //MessageBox.Show("Sector Error", "Read Data");
                        }
                        else if (a == 0x32)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "S/N Error";
                            //MessageBox.Show("S/N Error", "Read Data");
                        }
                        else if (a == 0x33)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Password Error";
                            //MessageBox.Show("Password Error", "Read Data");
                        }
                        else if (a == 0x34)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Block Error";
                            //MessageBox.Show("Block Error", "Read Data");
                        }
                        else if (a == 0x35)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Value overflow";
                            //MessageBox.Show("Value overflow", "Read Data");
                        }
                        else if (a == 69)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "No Card In";
                            //MessageBox.Show("No Card In", "Caution");
                        }
                        else if (a == 87)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                            //MessageBox.Show("The card is not on the card operation position", "Caution");
                        }
                        else if (a == 78)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Excute Command Error";
                            //MessageBox.Show("Excute Command Error", "Caution");
                        }
                        else if (a == -1)
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "Communication Error";
                            //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                        }
                        else
                        {
                            _StatesLector = StatesLector.LecturaError;
                            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                            oResultadoOperacion.Mensaje = "unknow Error";
                            //MessageBox.Show("unknow Error", "Caution");
                        }
                    }
                    else if (i == 0x30)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No RF Card In";
                        //MessageBox.Show("No RF Card In", "Verify Password");
                    }
                    else if (i == 0x31)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Sector Error";
                        //MessageBox.Show("Sector Error", "Verify Password");
                    }
                    else if (i == 0x32)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "S/N Error";
                        //MessageBox.Show("S/N Error", "Verify Password");
                    }
                    else if (i == 0x33)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Password Error";
                        //MessageBox.Show("Password Error", "Verify Password");
                    }
                    else if (i == 0x34)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Block Error";
                        //MessageBox.Show("Block Error", "Verify Password");
                    }
                    else if (i == 0x35)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Value overflow";
                        //MessageBox.Show("Value overflow", "Verify Password");
                    }
                    else if (i == 69)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Caution");
                    }
                    else if (i == 87)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                        //MessageBox.Show("The card is not on the card operation position", "Caution");
                    }
                    else if (i == 78)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Excute Command Error";
                        //MessageBox.Show("Excute Command Error", "Caution");
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.CheckPassError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion ExpulsarTI()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    int i = DllClass.CRT288_Eject(Hdle);
                    if (i == 0)
                    {
                        _StatesLector = StatesLector.ExpulsarOK;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Eject Card OK";
                        //MessageBox.Show("Eject Card OK", "Eject Card");
                    }
                    else if (i == 69)
                    {
                        _StatesLector = StatesLector.ExpulsarError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Eject Card");
                    }
                    else if (i == 78)
                    {
                        _StatesLector = StatesLector.ExpulsarError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Eject Card Error";
                        //MessageBox.Show("Eject Card Error", "Eject Card");
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.ExpulsarError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.ExpulsarError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Eject Card");
                    }
                }
                else
                {
                    _StatesLector = StatesLector.ExpulsarError;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Comm. port is not Opened";
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public ResultadoOperacion StatusTI()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    byte _CardStatus;
                    _CardStatus = 0;
                    int i = DllClass.CRT288_GetStatus(Hdle, ref _CardStatus);
                    if (i == 0)
                    {
                        switch (_CardStatus)
                        {
                            case 1:
                                _StatesLector = StatesLector.CardIn;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "card in the reader";
                                //MessageBox.Show("Card Status:  There is card in the reader", "Status");
                                break;
                            case 0:
                                _StatesLector = StatesLector.NoCard;
                                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                                oResultadoOperacion.Mensaje = "No Card";
                                //MessageBox.Show("Card Status:  No Card In The Reader", "Status");
                                break;
                        }
                    }
                    else if (i == -1)
                    {
                        _StatesLector = StatesLector.StatusError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        _StatesLector = StatesLector.StatusError;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "unknow Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
                else
                {
                    _StatesLector = StatesLector.StatusError;
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Comm. port is not Opened";
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
        public string HexAsciiConvert(string hex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),
                System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        public ResultadoOperacion EscribirTarjeta(string DATA)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                if (Hdle != 0)
                {
                    string[] DATAFINAL = DATA.Split(' ');
                    string write = DATAFINAL[0] + " 01 " + DATAFINAL[2] + " " + DATAFINAL[3] + " " + DATAFINAL[4] + " " + DATAFINAL[5] + " " + DATAFINAL[6] + " " + DATAFINAL[7] + " " + DATAFINAL[8] + " " + DATAFINAL[9] + " " + DATAFINAL[10] + " " + DATAFINAL[11] + " " + DATAFINAL[12] + " " + DATAFINAL[13] + " " + DATAFINAL[14] + " " + DATAFINAL[15] + " " + DATAFINAL[16];

                    byte[] BlockData = new byte[16];
                    string BlockDataStr = write;
                    byte BlockDataLen = 16;
                    byte SecNo = 1;
                    byte BlockNo = 0;

                    for (int n = 0; n < 16; n++)
                    {
                        BlockData[n] = (byte)Convert.ToInt32(BlockDataStr.Substring(n * 3, 2), 16);
                    }


                    //RF_WriteBlock(UInt32 ComHandle, byte _Sec, byte _Block, byte _BlockDataLen, byte[] _BlockData);
                    int i = DllClass.RF_WriteBlock(Hdle, SecNo, BlockNo, BlockDataLen, BlockData);

                    if (i == 0)
                    {
                        _StatesLector = StatesLector.EscribirOK;
                        oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        oResultadoOperacion.Mensaje = "Write Data OK";
                        //MessageBox.Show("Write Data OK", "Write Data");
                    }
                    else if (i == 0x30)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "No RF Card In";
                        //MessageBox.Show("No RF Card In", "Write Data");
                    }
                    else if (i == 0x31)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Sector Error";
                        //MessageBox.Show("Sector Error", "Write Data");
                    }
                    else if (i == 0x32)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "S/N Error";
                        //MessageBox.Show("S/N Error", "Write Data");
                    }
                    else if (i == 0x33)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Password Error";
                        //MessageBox.Show("Password Error", "Write Data");
                    }
                    else if (i == 0x34)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Block Error";
                        //MessageBox.Show("Block Error", "Write Data");
                    }
                    else if (i == 0x35)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Value overflow";
                        //MessageBox.Show("Value overflow", "Write Data");
                    }
                    else if (i == 69)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "No Card In";
                        //MessageBox.Show("No Card In", "Caution");
                    }
                    else if (i == 87)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "The card is not on the card operation position";
                        //MessageBox.Show("The card is not on the card operation position", "Caution");
                    }
                    else if (i == 78)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Excute Command Error";
                        //MessageBox.Show("Excute Command Error", "Caution");
                    }
                    else if (i == -1)
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("Communication Error\r\nPossible causes:\r\n1>Communication setup error\r\n2>Wrong model selected\r\n3>No connected\r\n4>No power on unit", "Caution");
                    }
                    else
                    {
                        //_StatesLector = StatesLector.ErrorEscribir;
                        //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                        //oResultadoOperacion.Mensaje = "Communication Error";
                        //MessageBox.Show("unknow Error", "Caution");
                    }
                }
                else
                {
                    //_StatesLector = StatesLector.ErrorEscribir;
                    //oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    //oResultadoOperacion.Mensaje = "Comm. port is not Opened";
                    //MessageBox.Show("Comm. port is not Opened", "Caution");
                }

            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = ex.ToString();
            }

            EventArgsCRTDevice e = new EventArgsCRTDevice(_StatesLector, oResultadoOperacion);
            DeviceMessageCRTState(this, e);

            return oResultadoOperacion;
        }
    }

    public class EventArgsCRTDevice : EventArgs
    {
        private StatesLector _result;

        public StatesLector result
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

        public EventArgsCRTDevice(StatesLector oStatesLector, ResultadoOperacion oResultadoOperacion)
        {
            _result = oStatesLector;
            _resultString = oResultadoOperacion;
        }
    }
}
