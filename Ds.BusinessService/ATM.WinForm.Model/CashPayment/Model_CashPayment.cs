using Ds.BusinessObjects.DataTransferObject;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.CashPaymentDevice;
using Ds.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial class Model : IModel
    {

        #region Billetero serial

        public bool InicializarBilletero(DtoModulo oEoModulo, string sPuerto)
        {
            bool ok = false;

            int d1 = 0;
            int d2 = 0;
            int d3 = 0;

            foreach (var p in oEoModulo.Partes)
            {
                if (p.Nombre == "Cass1")
                {
                    d1 = Convert.ToInt32(p.Denominacion);
                }
                else if (p.Nombre == "Cass2")
                {
                    d2 = Convert.ToInt32(p.Denominacion);
                }
                else if (p.Nombre == "Cass3")
                {
                    d3 = Convert.ToInt32(p.Denominacion);
                }
            }


            if (BilleteroSerial.Instancia.Iniciar(d1, d2, d3, sPuerto) == true)
            {
                ok = true;
            }


            return ok;
        }

        public ResultadoOperacion HabilitarSecuenciaRecibir()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            
            BilleteroSerial.Instancia.Recibir();

            PaymentDevice oPaymentDevice = new PaymentDevice();

            oResultadoOperacion = oPaymentDevice.Enable_All_Cash_Items();

            return oResultadoOperacion;
        }

        public ResultadoOperacion HabilitarSecuenciaRecibirDenominacionEspecifica(int bills, int scrow)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //BilleteroSerial.Instancia.RecibirIndependiente(bills, scrow);
            return oResultadoOperacion;
        }

        public bool HabilitarSecuenciaArqueoTotal(DtoModulo oEoModulo)
        {
            BilleteroSerial.Instancia.Arqueo(oEoModulo);
            return true;
        }

        public void DetenerSecuencias()
        {
            BilleteroSerial.Instancia.DetenerSecuencias();
        }

        public void HabilitarSecuenciaMonitor()
        {
            BilleteroSerial.Instancia.Monitor();
        }

        public void DeshabilitarRecepcion()
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            BilleteroSerial.Instancia.DeshabilitarRecepcion();

            PaymentDevice oPaymentDevice = new PaymentDevice();
            oResultadoOperacion = oPaymentDevice.Disable_All_Cash_Items();
        }

        public void DispensarBilletes(int Valor, DtoModulo oEoModulo)
        {
            BilleteroSerial.Instancia.Dispensar(Valor, oEoModulo);
        }

        #endregion Billetero serial


        #region Billetero usb

        public async Task<ResultadoOperacion> StartCashPayment(PaymentDevice oPaymentDevice)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            var t1 = await oPaymentDevice.Start_Pay_Manager();
            oResultadoOperacion = t1;
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                oResultadoOperacion.Mensaje = "Conexión Exitosa Cash Payment Device";
            }
            else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                string sMensajeError = oResultadoOperacion.Mensaje;
            }

            return oResultadoOperacion;
        }

        public async Task<ResultadoOperacion> AdministrarConexionCashPayment(DtoModulo oEoModulo, PaymentDevice oPaymentDevice, ConexionDispositivo oConexionDispositivo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oConexionDispositivo.oTipoConexionDispositivo == TipoConexionDispositivo.Abrir)
            {
                oResultadoOperacion = oPaymentDevice.Open_Pay_Manager();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = AsignarDenominaciones(oEoModulo, oPaymentDevice);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        var t1 = await oPaymentDevice.Start_Pay_Manager();
                        oResultadoOperacion = t1;
                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            oResultadoOperacion.Mensaje = "Conexión Exitosa Cash Payment Device";
                        }
                        else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                        {
                            string sMensajeError = oResultadoOperacion.Mensaje;
                        }
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    string sMensajeError = oResultadoOperacion.Mensaje;
                    // Reportar Alarma a Base de Datos
                }
            }
            else if (oConexionDispositivo.oTipoConexionDispositivo == TipoConexionDispositivo.Cerrar)
            {
                oResultadoOperacion = oPaymentDevice.Stop_Pay_Manager();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oResultadoOperacion = oPaymentDevice.Close_Pay_Manager();

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        oResultadoOperacion.Mensaje = "Desconexión Exitosa";
                    }
                    else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
                else if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    string sMensajeError = oResultadoOperacion.Mensaje;
                    // Reportar Alarma a Base de Datos
                }
            }

            return oResultadoOperacion;
        }

        private ResultadoOperacion AsignarDenominaciones(DtoModulo oEoModulo, PaymentDevice oPaymentDevice)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            for (int i = 0; i < oEoModulo.Partes.Count; i++)
            {
                if (oEoModulo.Partes[i].TipoParte == TipoParte.Hopper.ToString())
                {
                    oResultadoOperacion = oPaymentDevice.Assign_Bill_Denomination((int)Device_Type.Hopper, Convert.ToInt32(oEoModulo.Partes[i].NumParte), Convert.ToInt32(oEoModulo.Partes[i].Denominacion));

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        return oResultadoOperacion;
                    }
                }
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion AdministrarDenominaciones(PaymentDevice oPaymentDevice, TipoAdministracionDenominacion oTipoAdministracionDenominacion, TipoDenominacion oDenominacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oTipoAdministracionDenominacion == TipoAdministracionDenominacion.Habilitar)
            {
                if (oDenominacion == TipoDenominacion.ALL)
                {
                    oResultadoOperacion = oPaymentDevice.Enable_All_Cash_Items();

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
                else
                {
                    oResultadoOperacion = oPaymentDevice.Enable_Cash_Item_Specified((int)oDenominacion);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
            }
            else if (oTipoAdministracionDenominacion == TipoAdministracionDenominacion.Deshabilitar)
            {
                if (oDenominacion == TipoDenominacion.ALL)
                {
                    oResultadoOperacion = oPaymentDevice.Disable_All_Cash_Items();

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
                else
                {
                    oResultadoOperacion = oPaymentDevice.Disable_Cash_Item_Specified((int)oDenominacion);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                    {
                        string sMensajeError = oResultadoOperacion.Mensaje;
                        // Reportar Alarma a Base de Datos
                    }
                }
            }

            return oResultadoOperacion;
        }

        public ResultadoOperacion AdministrarProcesoPagoEfectivo(PaymentDevice oPaymentDevice, DispositivoPago oDispositivoPago, PagoEfectivo oPagoEfectivo, TipoMedioPago oTipoDenominacion)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            if (oTipoDenominacion == TipoMedioPago.Moneda)
            {
                oResultadoOperacion = GestionarPagoEfectivoMonedas(oPaymentDevice, oDispositivoPago, oPagoEfectivo);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
                {
                    string sMensajeError = oResultadoOperacion.Mensaje;
                    // Reportar Alarma a Base de Datos
                }
            }

            return oResultadoOperacion;
        }

        private ResultadoOperacion GestionarPagoEfectivoMonedas(PaymentDevice oPaymentDevice, DispositivoPago oDispositivoPago, PagoEfectivo oPagoEfectivo)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            int[] coin_cnt_denom = { 0, 0, 0, 0 };
            int coin_index = 0;
            Int64 valor_temp = 0;

            List<TransaccionEfectivo> Transacciones = new List<TransaccionEfectivo>();

            while ((oPagoEfectivo.ValorProcesoCambio >= 0) && (coin_index < oDispositivoPago.Num_Hoppers))
            {
                while (oDispositivoPago.Coin_Qty[coin_index] <= oDispositivoPago.Coin_Min[coin_index])
                {
                    coin_index++;
                    if (coin_index == oDispositivoPago.Num_Hoppers)
                        break;
                }

                if (coin_index <= oDispositivoPago.Num_Hoppers)
                {
                    valor_temp = oPagoEfectivo.ValorProcesoCambio - oDispositivoPago.Coin_Denom[coin_index];

                    if (valor_temp >= 0)
                    {
                        coin_cnt_denom[coin_index]++;
                        oPagoEfectivo.ValorProcesoCambio = valor_temp;
                    }
                    else
                    {
                        if (coin_cnt_denom[coin_index] > 0)
                        {
                            int pago_parcial_hopper = (oDispositivoPago.Coin_Denom[coin_index]) * (coin_cnt_denom[coin_index]);
                            int payout = 0;

                            for (int i = 0; i < oDispositivoPago.Num_Hoppers; i++)
                            {
                                if ((oDispositivoPago.Coin_Denom[coin_index] == oDispositivoPago.Hopper_Denom[i]) && (payout == 0))
                                {
                                    Pay_Unit oPay_Unit = new Pay_Unit();

                                    if (i == 0)
                                        oPay_Unit = Pay_Unit.Hopper1;
                                    else if (i == 1)
                                        oPay_Unit = Pay_Unit.Hopper2;
                                    else if (i == 2)
                                        oPay_Unit = Pay_Unit.Hopper3;
                                    else if (i == 3)
                                        oPay_Unit = Pay_Unit.Hopper4;

                                    ResultadoOperacion oResultParcial = new ResultadoOperacion();
                                    TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "Set_Coin_Pay: pago_parcial_hopper: " + pago_parcial_hopper + " oPay_Unit " + oPay_Unit, TipoLog.TRAZA);
                                    oResultParcial = oPaymentDevice.Set_Coin_Pay(pago_parcial_hopper, oPay_Unit);

                                    if (oResultParcial.oEstado == TipoRespuesta.Exito)
                                    {
                                        payout = (int)oResultParcial.EntidadDatos;

                                        if (payout > 0)
                                        {
                                            TransaccionEfectivo oTransaccionEfectivo = new TransaccionEfectivo();
                                            oTransaccionEfectivo.TipoParte = TipoParte.Hopper;
                                            oTransaccionEfectivo.Denominacion = oDispositivoPago.Hopper_Denom[i];
                                            oTransaccionEfectivo.Cantidad = payout / oDispositivoPago.Hopper_Denom[i];
                                            //oTransaccionEfectivo.IdParte = IdParte;
                                            Transacciones.Add(oTransaccionEfectivo);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        oResultadoOperacion.Mensaje = oResultParcial.Mensaje;
                                    }
                                }
                            }

                            oPagoEfectivo.ValorProcesoCambio += (pago_parcial_hopper - payout);
                        }

                        coin_index++;
                    }
                }
                else
                    break;
            }

            if (Transacciones.Count > 0)
            {
                oResultadoOperacion.oEstado = TipoRespuesta.Exito;
                oResultadoOperacion.ListaEntidadDatos = Transacciones;
            }
            else
                oResultadoOperacion.oEstado = TipoRespuesta.Error;

            return oResultadoOperacion;
        }

        public ResultadoOperacion AdministrarDescargaMonedas(PaymentDevice oPaymentDevice, Pay_Unit oPay_Unit, int valor)
        {
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            oResultadoOperacion = oPaymentDevice.Set_Coin_Pay(valor, oPay_Unit);

            if (oResultadoOperacion.oEstado == TipoRespuesta.Error)
            {
                string sMensajeError = oResultadoOperacion.Mensaje;
                // Reportar Alarma a Base de Datos
            }

            return oResultadoOperacion;
        }

        #endregion

    }
}
