using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ds.CashPaymentDevice
{
    public class PaymentDevice : ContainerControl
    {
        public event EventHandler DeviceMessage;

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern int openpaymentmanager();

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern int closepaymentmanager();

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern int startpaymentmanager(IntPtr wid, int mid, int devices, int protocol, int message);

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern int stoppaymentmanager();

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern ulong setpaymentmanager(int cmd, int info1, int info2, int info3);

        [DllImport(@"Config Library/PaymentManager.dll")]
        public static extern ulong setoption(char cLetter, int iOption, int iValue1, int iValue2);

        /// <summary>
        /// CashCode Bill To Bill Functions
        /// </summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////

        public ResultadoOperacion Open_Pay_Manager()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = openpaymentmanager();

            if (result == (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Open OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Open Error - ";

                if (result == (int)Result.Not_USB_DLL_Found)
                    oResultadoOperacion.Mensaje += "Not USB DLL Found.";
                else if (result == (int)Result.PM_Busy)
                    oResultadoOperacion.Mensaje += "PM Already Busy.";
                else
                    oResultadoOperacion.Mensaje += "Unknown Error";

                string sMensajeError = oResultadoOperacion.Mensaje;
                // Generar LOG Device Exception
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Close_Pay_Manager()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = closepaymentmanager();

            if (result == (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Close OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Close Error - ";

                if (result == (int)Result.PM_Busy)
                    oResultadoOperacion.Mensaje += "PM Still Busy.";
                else
                    oResultadoOperacion.Mensaje += "Unknown Error.";

                string sMensajeError = oResultadoOperacion.Mensaje;
                // Generar LOG Device Exception
            }

            return oResultadoOperacion;
        }

        public async Task<ResultadoOperacion> Start_Pay_Manager()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            IntPtr hwnd = this.Handle;

            await Task.Run(() =>
            {
                int result = startpaymentmanager(hwnd, (int)CashCode_Handle.User, (int)Device.Hoppers_Coin, (int)Protocol.ALL, (int)Info.Zero);

                //if ((result >= (int)Result.Hoppers_Found) && (result < (int)Result.Payment_Manager_Unopened))
                if (result < (int)Result.Payment_Manager_Unopened)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Start OK.";
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Start Error - ";

                    if (result == (int)Result.Payment_Manager_Unopened)
                        oResultadoOperacion.Mensaje += "Payment Manager Unopened.";
                    else if (result == (int)Result.Unknown_Protocol_Selected)
                        oResultadoOperacion.Mensaje += "Unknown Protocol Selected.";
                    else if (result == (int)Result.Error_Adapter_Communication_A)
                        oResultadoOperacion.Mensaje += "Error Adapter Communication.";
                    else if (result == (int)Result.Error_Adapter_Communication_B)
                        oResultadoOperacion.Mensaje += "Error Adapter Communication.";
                    else if (result == (int)Result.No_Communication_With_Payment_Units)
                        oResultadoOperacion.Mensaje += "No Communication With_Payment Units.";
                    else if (result == (int)Result.Payment_Manager_Running)
                        oResultadoOperacion.Mensaje += "Payment Manager Running.";
                    else if (result == (int)Result.Payment_Manager_Not_Start_Thread)
                        oResultadoOperacion.Mensaje += "Payment Manager Not Start Thread.";
                    else
                        oResultadoOperacion.Mensaje += "Some Devices Not Found.";

                    string sMensajeError = oResultadoOperacion.Mensaje;
                    // Generar LOG Device Exception
                }
            });

            return oResultadoOperacion;
        }

        public ResultadoOperacion Stop_Pay_Manager()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = stoppaymentmanager();

            if (result == (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Stop OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Stop Error - ";

                if (result == (int)Result.PM_Busy)
                    oResultadoOperacion.Mensaje += "PM Still Busy.";
                else
                    oResultadoOperacion.Mensaje += "Unknown Error.";

                string sMensajeError = oResultadoOperacion.Mensaje;
                // Generar LOG Device Exception
            }

            return oResultadoOperacion;
        }

        private void SetResultMessages(int result, ref ResultadoOperacion oResultadoOperacion)
        {
            if (result == (int)Result.Device_Not_Found)
                oResultadoOperacion.Mensaje += "Device Not Found.";
            else if (result == (int)Result.Value_Too_Small)
                oResultadoOperacion.Mensaje += "Value Too Small.";
            else if (result == (int)Result.No_Device_Attached)
                oResultadoOperacion.Mensaje += "No Device Attached.";
            else if (result == (int)Result.Device_Error)
                oResultadoOperacion.Mensaje += "Device Error.";
            else if (result == (int)Result.Command_Unknowm)
                oResultadoOperacion.Mensaje += "Command Unknown.";
            else if (result == (int)Result.Payment_Manager_Not_Running)
                oResultadoOperacion.Mensaje += "Payment Manager Not Running.";
            else if (result == (int)Result.Device_Busy)
                oResultadoOperacion.Mensaje += "Device Busy.";
            else
                oResultadoOperacion.Mensaje += "Unknown Error.";

            string sMensajeError = oResultadoOperacion.Mensaje;
            // Generar LOG Device Exception
        }

        public ResultadoOperacion Enable_All_Cash_Items()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Inhibit, (int)Selection.Enable_All_Cash_Items, (int)Info.Zero, (int)Info.Zero);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Enable All Cash Items OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Enable All Cash Items Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Enable_Cash_Item_Specified(int value)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Inhibit, (int)Selection.Enable_Cash_Item_With_Specified_Value, value * (int)General.Factor_Denominacion, (int)Info.Zero);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Enable Cash Item OK " + value.ToString() + ".";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Enable Cash Item Error " + value.ToString() + " - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Disable_Cash_Item_Specified(int value)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Inhibit, (int)Selection.Disable_Cash_Item_With_Specified_Value, value * (int)General.Factor_Denominacion, (int)Info.Zero);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Disable Cash Item OK " + value.ToString() + ".";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Disable Cash Item Error " + value.ToString() + " - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Disable_All_Cash_Items()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Inhibit, (int)Selection.Disable_All_Cash_Items, (int)Info.Zero, (int)Info.Zero);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Disable All Cash Items OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Disable All Cash Items Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Enable_Escrow()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Escrow, (int)Selection.Enable_Disable_Escrow_All_Bills, (int)Info.Zero, (int)Info.Enable_Escrow);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Enable Escrow OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Enable Escrow Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Disable_Escrow()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Escrow, (int)Selection.Enable_Disable_Escrow_All_Bills, (int)Info.Zero, (int)Info.Disable_Escrow);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Disable Escrow OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Disable Escrow Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Accept_Bill()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Escrow, (int)Selection.Accept_Return_Timeout_Bill_From_Escrow, (int)Info.Zero, (int)Info.Accept_Bill);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Accept Bill OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Accept Bill Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
                oResultadoOperacion.IdeEntidad = result;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Return_Bill()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Escrow, (int)Selection.Accept_Return_Timeout_Bill_From_Escrow, (int)Info.Zero, (int)Info.Return_Bill);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Return Bill OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Return Bill Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
                oResultadoOperacion.IdeEntidad = result;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Set_Bill_Pay(int denominacion, int cnt)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Payout, (int)Selection.Manual_Payout, denominacion * (int)General.Factor_Denominacion, cnt);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Bill Pay OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Bill Pay Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Pay_All_Bills()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Payout, (int)Selection.Manual_Payout, (int)Info.Zero, (int)Info.Zero);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Pay All Bills OK.";
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Pay All Bills Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Set_Coin_Pay(int valor, Pay_Unit oPay_Unit)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Payout, (int)Selection.Payout_Specified_Value_Specified_Device, valor, (int)oPay_Unit);

            if (result >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "Coin Pay OK.";
                oResultadoOperacion.EntidadDatos = result;
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Coin Pay Error - ";
                SetResultMessages(result, ref oResultadoOperacion);
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Unload_All_Bills_To_Box(int cassette)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int cantidad = (int)setpaymentmanager((int)Command.Maintenance, (int)Selection.Unload_Bills, cassette, (int)General.Max_Unload_Bills);

            if (cantidad >= (int)Result.OK)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.Mensaje = "All Bills Unloaded To Box.";
                oResultadoOperacion.EntidadDatos = cantidad;
            }
            else
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Error;
                oResultadoOperacion.Mensaje = "Error Unloading All Bills To Box.";

                string sMensajeError = oResultadoOperacion.Mensaje;
                // Generar LOG Device Exception
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Get_Device_Level(int device)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int result = (int)setpaymentmanager((int)Command.Payout, (int)Selection.Get_Empty_Full_Status_Device, device, (int)Info.Zero);

            if ((result & (int)Result.Full_Level) > 0)
            {
                oResultadoOperacion.EntidadDatos = Result.Full_Level;
            }
            else if ((result & (int)Result.Empty_Level) > 0)
            {
                oResultadoOperacion.EntidadDatos = Result.Empty_Level;
            }
            else
            {
                oResultadoOperacion.EntidadDatos = Result.Normal_Level;
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion Assign_Bill_Denomination(int device_type, int device_number, int denominacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            char type = (char)device_type;

            if (type == (int)Device_Type.Cassette)
            {
                int result = (int)setoption(type, denominacion * (int)General.Factor_Denominacion, device_number, (int)Info.Zero);

                if (result >= (int)Result.OK)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Assign Bill Denomination OK in Cassette " + device_number.ToString() + ".";
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Assign Bill Denomination Error in Cassette " + device_number.ToString() + " - ";
                    SetResultMessages(result, ref oResultadoOperacion);
                }
            }
            else if (type == (int)Device_Type.Hopper)
            {
                int result = 0;

                if (device_number == 1)
                {
                    result = (int)setoption(type, 6, denominacion, 5);
                }
                else if (device_number == 2)
                {
                    result = (int)setoption(type, 4, denominacion, 3);
                }
                else if (device_number == 3)
                {
                    result = (int)setoption(type, 3, denominacion, 2);
                }
                else if (device_number == 4)
                {
                    result = (int)setoption(type, 5, denominacion, 4);
                }

                if (result >= (int)Result.OK)
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                    oResultadoOperacion.Mensaje = "Assign Coin Denomination OK in Hopper " + device_number.ToString() + ".";
                }
                else
                {
                    oResultadoOperacion.oEstado = TipoRespuesta.Error;
                    oResultadoOperacion.Mensaje = "Assign Coin Denomination Error in Hopper " + device_number.ToString() + " - ";
                    SetResultMessages(result, ref oResultadoOperacion);
                }
            }

            return oResultadoOperacion;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)CashCode_Handle.User)
            {
                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                oResultadoOperacion = CashCode_WindowsEvents(m);

                if (DeviceMessage != null)
                {
                    EventArgsPaymentDevice e = new EventArgsPaymentDevice(oResultadoOperacion);
                    DeviceMessage(this, e);
                }
            }
            else
                base.WndProc(ref m);
        }

        public ResultadoOperacion CashCode_WindowsEvents(Message m)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            PaymentEvent oPaymentEvent = new PaymentEvent();

            string view_message = string.Empty;

            int wParam = m.WParam.ToInt32();
            int lParam = m.LParam.ToInt32();

            int id_event = (wParam & (int)WParam_Message.Event_Range) >> 4;
            int device = wParam & (int)WParam_Message.Payment_Unit_Range;

            oPaymentEvent.id_event = id_event;
            oPaymentEvent.device = device;
            oPaymentEvent.message = lParam;
            oPaymentEvent.wm_event = m.ToString();

            oResultadoOperacion.EntidadDatos = oPaymentEvent;

            return oResultadoOperacion;
        }
    }

    public class EventArgsPaymentDevice : EventArgs
    {
        private ResultadoOperacion _result;

        public ResultadoOperacion result
        {
            get { return _result; }
            set { _result = value; }
        }

        public EventArgsPaymentDevice(ResultadoOperacion oResultadoOperacion)
        {
            _result = oResultadoOperacion;
        }
    }
}
