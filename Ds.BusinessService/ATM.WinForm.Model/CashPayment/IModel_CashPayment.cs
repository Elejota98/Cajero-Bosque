using Ds.BusinessObjects.DataTransferObject;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.CashPaymentDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial interface IModel
    {
        //Funciones Hoppers
        Task<ResultadoOperacion> StartCashPayment(PaymentDevice oPaymentDevice);
        Task<ResultadoOperacion> AdministrarConexionCashPayment(DtoModulo oModulo, PaymentDevice oPaymentDevice, ConexionDispositivo oConexionDispositivo);
        ResultadoOperacion AdministrarDenominaciones(PaymentDevice oPaymentDevice, TipoAdministracionDenominacion oTipoAdministracionDenominacion, TipoDenominacion oDenominacion);
        ResultadoOperacion AdministrarDescargaMonedas(PaymentDevice oPaymentDevice, Pay_Unit oPay_Unit, int valor);
        ResultadoOperacion AdministrarProcesoPagoEfectivo(PaymentDevice oPaymentDevice, DispositivoPago oDispositivoPago, PagoEfectivo oPagoEfectivo, TipoMedioPago oTipoDenominacion);

        //Funciones Billetero
        void DeshabilitarRecepcion();
        //void DetenerSecuencias();
        //void DispensarBilletes(int Valor, DtoModulo oEoModulo);
        //bool HabilitarSecuenciaArqueoTotal(DtoModulo oEoModulo);
        //void HabilitarSecuenciaMonitor();
        ResultadoOperacion HabilitarSecuenciaRecibir();
        //bool InicializarBilletero(DtoModulo oEoModulo, string sPuerto);
        //ResultadoOperacion HabilitarSecuenciaRecibirDenominacionEspecifica(int bills, int scrow);


        ///////////////////////////////////////////////////////////////////
        bool InicializarBilletero(DtoModulo oEoModulo, string sPuerto);
        void DetenerSecuencias();
        void HabilitarSecuenciaMonitor();
        bool HabilitarSecuenciaArqueoTotal(DtoModulo oEoModulo);
        void DispensarBilletes(int Valor, DtoModulo oEoModulo);

    }
}
