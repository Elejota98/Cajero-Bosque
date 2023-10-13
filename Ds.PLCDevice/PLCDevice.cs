using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;
using System.IO.Ports;
using Modbus.Device;
using Ds.BusinessObjects.Enums;
using Ds.BusinessObjects.Entities;


namespace Ds.PLCDevice
{
    public class PLCDeviceClass
    {
        public EventHandler DeviceMessage;

        public OPCAutomation.OPCServer AnOPCServer;
        public OPCServer ConnectedOPCServer;
        public OPCAutomation.OPCGroups ConnectedServerGroup;
        public OPCGroup ConnectedGroup;
        public string Groupname;

        int ItemCount;
        Array OPCItemIDs = Array.CreateInstance(typeof(string), 10);
        Array ItemServerHandles = Array.CreateInstance(typeof(Int32), 10);
        Array ItemServerErrors = Array.CreateInstance(typeof(Int32), 10);
        Array ClientHandles = Array.CreateInstance(typeof(Int32), 10);
        Array RequestedDataTypes = Array.CreateInstance(typeof(Int16), 10);
        Array AccessPaths = Array.CreateInstance(typeof(string), 10);
        Array WriteItems = Array.CreateInstance(typeof(object), 10);

        bool banderaBit1 = true;

        public ResultadoOperacion GestionarConexionPLC(TipoConexionDispositivo oTipo, string server, string varibleEstado, string variableLuz, string variableAlarma)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oTipo == TipoConexionDispositivo.Abrir)
            {
                try
                {
                    string IOServer = server;
                    string IOGroup = "OPCGroup1";

                    ConnectedOPCServer = new OPCAutomation.OPCServer();
                    ConnectedOPCServer.Connect(IOServer, "");
                    ConnectedGroup = ConnectedOPCServer.OPCGroups.Add(IOGroup);
                    ConnectedGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(ObjOPCGroup_DataChange);
                    ConnectedGroup.UpdateRate = 1000;
                    ConnectedGroup.IsSubscribed = ConnectedGroup.IsActive;


                    ItemCount = 3;
                    OPCItemIDs.SetValue(varibleEstado, 1);
                    ClientHandles.SetValue(1, 1);
                    OPCItemIDs.SetValue(variableLuz, 2);
                    ClientHandles.SetValue(2, 2);
                    OPCItemIDs.SetValue(variableAlarma, 3);
                    ClientHandles.SetValue(3, 3);

                    ConnectedGroup.OPCItems.DefaultIsActive = true;
                    ConnectedGroup.OPCItems.AddItems(ItemCount, ref OPCItemIDs, ref ClientHandles, out ItemServerHandles, out ItemServerErrors, RequestedDataTypes, AccessPaths);
                }
                catch (Exception ex)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error conectar PLC";
                }
            }
            else
            {
                try
                {
                    ConnectedOPCServer.Disconnect();
                }
                catch (Exception e)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Error desconectar PLC";
                }
            }

            return oResultadoOperacion;
        }

        private void ObjOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;

                for (int i = 1; i <= ClientHandles.Length; i++)
                {
                    if ((Convert.ToInt32(ClientHandles.GetValue(i)) == 1))
                    {
                        if (Convert.ToBoolean(ItemValues.GetValue(i)) != banderaBit1)
                        {
                            if (banderaBit1 == true)
                            {
                                banderaBit1 = false;
                            }
                            else
                            {
                                banderaBit1 = true;
                            }

                            oResultadoOperacion.EntidadDatos = ItemValues.GetValue(i);
                            EventArgsPLCDevice e = new EventArgsPLCDevice(oResultadoOperacion);
                            DeviceMessage(this, e);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }
        }

        public ResultadoOperacion GestionarLuzMonedas(bool bEstado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                WriteItems.SetValue(null, 1);
                WriteItems.SetValue(null, 3);
                WriteItems.SetValue(bEstado, 2);
                ConnectedGroup.SyncWrite(ItemCount, ref ItemServerHandles, ref WriteItems, out ItemServerErrors);
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error gestionar luz PLC";
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion GestionarAlarma(bool bEstado)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                WriteItems.SetValue(null, 1);
                WriteItems.SetValue(null, 2);
                WriteItems.SetValue(bEstado, 3);
                ConnectedGroup.SyncWrite(ItemCount, ref ItemServerHandles, ref WriteItems, out ItemServerErrors);
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error gestionar alarma PLC";
            }

            return oResultadoOperacion;
        }

        /// <summary>
        /// New PLC
        /// </summary>

        public static ResultadoOperacion Escribir(string sPuertoPLC, ushort usDireccion, bool bComando)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            try
            {
                using (SerialPort port = new SerialPort(sPuertoPLC))
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                    }

                    string Com = "COM5";
                    port.PortName = Com;
                    port.BaudRate = 9600;
                    port.Parity = System.IO.Ports.Parity.None;
                    port.DataBits = 8;
                    port.Open();                    
                }

                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }


            return oResultadoOperacion;
        }

        public static ResultadoOperacion Leer(string sPuertoPLC, ushort usDireccion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            bool[] bEstado = null;
            try
            {
                using (SerialPort port = new SerialPort(sPuertoPLC))
                {
                    // configure serial port
                    port.BaudRate = 38400;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.ReadTimeout = 500;
                    port.Open();

                    // create modbus master
                    IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                    master.Transport.WriteTimeout = 300;
                    master.Transport.ReadTimeout = 300;
                    master.Transport.Retries = 0;

                    byte slaveId = 1;
                    ushort startAddress = usDireccion;

                    bEstado = master.ReadCoils(slaveId, startAddress, 1);
                    port.Close();
                }
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.ListaEntidadDatos = bEstado;
            }
            catch (Exception ex)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
            }

            return oResultadoOperacion;
        }

    }

    public class EventArgsPLCDevice : EventArgs
    {
        private ResultadoOperacion _result;

        public ResultadoOperacion result
        {
            get { return _result; }
            set { _result = value; }
        }

        public EventArgsPLCDevice(ResultadoOperacion oResultadoOperacion)
        {
            _result = oResultadoOperacion;
        }
    }
}
