using ATM.WinForm.View.Base;
using Ds.BusinessObjects.DataTransferObject;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.View
{
    public interface IView_Principal : IView
    {
        bool bPagoSmart { get; set; }
        string General_Events { set; }
        int Cnt_Reinicio { get; set; }
        Tarjeta Tarjeta { get; set; }
        TarjetaSmart TarjetaSmart { get; set; }
        TicketCarga Carga { get; set; }
        DtoModulo DtoModulo { set; get; }
        DtoModulo DtoModuloF56 { set; get; }
        List<DtoTransacciones> DtoTransacciones { set; get; }
        List<DtoPago> DtoPago { set; get; }
        List<DtoLogMovimiento> DtoLogMovimiento { set; get; }
        List<DtoArqueo> DtoArqueo { set; get; }
        List<DatosLiquidacion> lstDtoLiquidacion { get; set; }
        DtoOperacion DtoOperacion { set; get; }
        Operacion Operacion { set; get; }
        Operacion OperacionCentral { set; get; }
        Usuario Usuario { set; get; }
        DtoUsuario DtoUsuario { set; get; }
        bool ProcesoPago { get; set; }
        double NumeroDocumentoOrigen { set; get; }
        bool ConfigModulo { set; get; }
        bool ProcesoArqueoParcial { set; get; }
        List<ListadoArqueo> lstArqueo { set; get; }
        PagoEfectivo PagoEfectivo { set; get; }
        void Imprimir();
        bool ProcesoCarga { set; get; }
        bool ProcesoParcialCambio { set; get; }
        bool Hopper1Ready { get; set; }
        bool Hopper2Ready { get; set; }
        bool Hopper3Ready { get; set; }
        bool Hopper4Ready { get; set; }
        bool BilleteroReady { get; set; }
        bool CtCoinReady { get; set; }
        bool Inicializando { set; get; }
        int DenomMenor { set; get; }
        int DenomMenorMonedas { set; get; }
        bool TicketDevolucion { set; get; }
        bool ComPrint { set; get; }
        bool CargaMonedas { set; get; }
        bool MonedasStart { set; get; }
        bool CargaBilletesBB { set; get; }
        DataTable CargaActualTemporal { set; get; }
        DataTable CargaTotalTemporal { set; get; }
        bool CargaBilletesF56 { set; get; }
        bool BilletesStart { set; get; }
        int oTimeOut { set; get; }
        void SetearPantalla(Pantalla ePantalla);
        //bool ProcesoLimpiezaManual { set; get; }
        bool BanderaRecaudo { set; get; }
        bool BanderaPresionado { set; get; }
        void FinalizarArqueo();
        List<ConsultarPagoCelularResult> lstConsultarPagoCelularResult { get; set; }
        List<RegistrarPagoCelularResult> lstRegistrarPagoCelularResult { get; set; }
        List<RegistrarPagoTarjetaResult> lstRegistrarPagoTarjetaResult { get; set; }
        List<RegistrarPagoSmartResult> lstRegistrarPagoSmartResult { get; set; }
        List<RegistrarPagoEfectivoResult> lstRegistrarPagoEfectivoResult { get; set; }
        List<ConsultarValorResult> lstConsultarValorResult { get; set; }
        void MensajesEstado(string sMensaje);
        ConsultarValorResult ConsultarValorResult { get; set; }
        //void AtascoEnReconteo();
        //void AlertaEnLimpiezaManual();
        bool CobroTarjetaMensual { get; set; }
        bool BanderaEstadoBilletero { set; get; }
        bool ProcesoEstados { get; set; }
        bool BanderaBox { get; set; }
        bool BanderaArqueoParcial { get; set; }
        bool BanderaArqueoTotal { get; set; }
        bool BanderaChasis { get; set; }
        bool BanderaJam { get; set; }
        bool BanderaCancelacion { get; set; }
        bool BanderaCargaBilletes { get; set; }
        bool BanderaJamArqueoT { get; set; }
        bool BanderaPagoFinal { get; set; }
        bool BanderaDispensing { get; set; }
        bool BanderaEsperaHabilitado { get; set; }
        int ContadorIDLING { get; set; }
        string EstadoSecuencia { get; set; }
        string EstadoSecuenciaDispensar { get; set; }
        string IdTransaccion { get; set; }
        string IdArqueo { get; set; }
        string IdCarga { get; set; }
        string IdEmpresa { get; set; }
        List<TransaccionEfectivo> Transacciones { get; set; }
        bool Efectivo { get; set; }
        bool Datafono { get; set; }
        bool Prepago { get; set; }
        bool Celular { get; set; }
        void DatosPantallaPago();
        string NombreConvenio { get; set; }
        string TipoPago { get; set; }
        string ValorTipoPago { get; set; }
        List<DtoAutorizado> lstDtoAutorizado { get; set; }
        int RetornoCobro { get; set; }
        Pantalla Presentacion { get; set; }
        List<DtoTarjetas> lstDtoTarjetas { get; set; }
    }
}
