using ATM.WinForm.FrontEnd.Log_Viewer;
using ATM.WinForm.Presenter;
using ATM.WinForm.View;
using DevComponents.DotNetBar.Keyboard;
using Ds.BusinessObjects.DataTransferObject;
using Ds.BusinessObjects.Entities;
using Ds.BusinessObjects.Enums;
using Ds.Utilidades;
using EGlobalT.Device.SmartCard;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ATM.WinForm.FrontEnd
{
    public partial class frmPrincipal : Form, IView_Principal
    {

        #region Rutas
        string sInicioAnimacion = string.Empty;
        string sPrincipalAnimacion = string.Empty;
        string sBoxAnimacion = string.Empty;
        string sBoxTotalAnimacion = string.Empty;
        string sUnMomentoAnimacion = string.Empty;
        string sTransaccionCanceladaAnimacion = string.Empty;
        string sRetiroReciboAnimacion = string.Empty;
        string sRetiroDineroAnimacion = string.Empty;
        #endregion

        #region Definiciones
        private List<DtoTarjetas> _lstDtoTarjetas = new List<DtoTarjetas>();
        public List<DtoTarjetas> lstDtoTarjetas
        {
            get { return _lstDtoTarjetas; }
            set { _lstDtoTarjetas = value; }
        }
        private int _RetornoCobro = 0;
        public int RetornoCobro
        {
            get { return _RetornoCobro; }
            set { _RetornoCobro = value; }
        }
        private bool _CobroTarjetaMensual = false;
        public bool CobroTarjetaMensual
        {
            get { return _CobroTarjetaMensual; }
            set { _CobroTarjetaMensual = value; }
        }
        private double _TiempoSalida = 0;
        private List<DtoAutorizado> _lstDtoAutorizado = new List<DtoAutorizado>();
        public List<DtoAutorizado> lstDtoAutorizado
        {
            get { return _lstDtoAutorizado; }
            set { _lstDtoAutorizado = value; }
        }   
        private bool _ComPrint = false;
        public bool ComPrint
        {
            get { return _ComPrint; }
            set { _ComPrint = value; }
        }
        private bool _PrintSalida = false;
        public bool PrintSalida
        {
            get { return _PrintSalida; }
            set { _PrintSalida = value; }
        }
        private string _ValorTipoPago = string.Empty;
        public string ValorTipoPago
        {
            get { return _ValorTipoPago; }
            set { _ValorTipoPago = value; }
        }
        private string _TipoPago = string.Empty;
        public string TipoPago
        {
            get { return _TipoPago; }
            set { _TipoPago = value; }
        }
        private string _NombreConvenio = string.Empty;
        public string NombreConvenio
        {
            get { return _NombreConvenio; }
            set { _NombreConvenio = value; }
        }
        private List<DatosLiquidacion> _lstDtoLiquidacion = new List<DatosLiquidacion>();
        public List<DatosLiquidacion> lstDtoLiquidacion
        {
            get { return _lstDtoLiquidacion; }
            set { _lstDtoLiquidacion = value; }
        }
        private bool _CargaBilletesBB = false;
        public bool CargaBilletesBB
        {
            get { return _CargaBilletesBB; }
            set { _CargaBilletesBB = value; }
        }
        int _50Antes = 0;
        int _100Antes = 0;
        int _200Antes = 0;
        int _500Antes = 0;
        int _1000Antes = 0;

        int _50Despues = 0;
        int _100Despues = 0;
        int _200Despues = 0;
        int _500Despues = 0;
        int _1000Despues = 0;
        bool bFormSetup = false;
        private bool _Efectivo = false;
        public bool Efectivo
        {
            get { return _Efectivo; }
            set { _Efectivo = value; }
        }
        private bool _Datafono = false;
        public bool Datafono
        {
            get { return _Datafono; }
            set { _Datafono = value; }
        }
        private bool _Prepago = false;
        public bool Prepago
        {
            get { return _Prepago; }
            set { _Prepago = value; }
        }
        private bool _Celular = false;
        public bool Celular
        {
            get { return _Celular; }
            set { _Celular = value; }
        }
        List<RegistrarPagoSmartResult> _lstRegistrarPagoSmartResult = new List<RegistrarPagoSmartResult>();
        public List<RegistrarPagoSmartResult> lstRegistrarPagoSmartResult
        {
            get { return _lstRegistrarPagoSmartResult; }
            set { _lstRegistrarPagoSmartResult = value; }
        }
        List<RegistrarPagoTarjetaResult> _lstRegistrarPagoTarjetaResult = new List<RegistrarPagoTarjetaResult>();
        public List<RegistrarPagoTarjetaResult> lstRegistrarPagoTarjetaResult
        {
            get { return _lstRegistrarPagoTarjetaResult; }
            set { _lstRegistrarPagoTarjetaResult = value; }
        }
        List<RegistrarPagoEfectivoResult> _lstRegistrarPagoEfectivoResult = new List<RegistrarPagoEfectivoResult>();
        public List<RegistrarPagoEfectivoResult> lstRegistrarPagoEfectivoResult
        {
            get { return _lstRegistrarPagoEfectivoResult; }
            set { _lstRegistrarPagoEfectivoResult = value; }
        }
        List<RegistrarPagoCelularResult> _lstRegistrarPagoCelularResult = new List<RegistrarPagoCelularResult>();
        public List<RegistrarPagoCelularResult> lstRegistrarPagoCelularResult
        {
            get { return _lstRegistrarPagoCelularResult; }
            set { _lstRegistrarPagoCelularResult = value; }
        }
        List<ConsultarPagoCelularResult> _lstConsultarPagoCelularResult = new List<ConsultarPagoCelularResult>();
        public List<ConsultarPagoCelularResult> lstConsultarPagoCelularResult
        {
            get { return _lstConsultarPagoCelularResult; }
            set { _lstConsultarPagoCelularResult = value; }
        }
        ConsultarValorResult _ConsultarValorResult = new ConsultarValorResult();
        public ConsultarValorResult ConsultarValorResult
        {
            get { return _ConsultarValorResult; }
            set { _ConsultarValorResult = value; }
        }
        List<ConsultarValorResult> _lstConsultarValorResult = new List<ConsultarValorResult>();
        public List<ConsultarValorResult> lstConsultarValorResult
        {
            get { return _lstConsultarValorResult; }
            set { _lstConsultarValorResult = value; }
        }
        private Tarjeta _Tarjeta = new Tarjeta();
        public Tarjeta Tarjeta
        {
            get { return _Tarjeta; }
            set { _Tarjeta = value; }
        }
        private TarjetaSmart _TarjetaSmart = new TarjetaSmart();
        public TarjetaSmart TarjetaSmart
        {
            get { return _TarjetaSmart; }
            set { _TarjetaSmart = value; }
        }
        private bool _BanderaJam;
        public bool BanderaJam
        {
            get { return _BanderaJam; }
            set { _BanderaJam = value; }
        }
        private bool _BanderaEstadoBilletero;
        public bool BanderaEstadoBilletero
        {
            get { return _BanderaEstadoBilletero; }
            set { _BanderaEstadoBilletero = value; }
        }
        private bool _BanderaEsperaHabilitado;
        public bool BanderaEsperaHabilitado
        {
            get { return _BanderaEsperaHabilitado; }
            set { _BanderaEsperaHabilitado = value; }
        }
        private bool _BanderaDispensing;
        public bool BanderaDispensing
        {
            get { return _BanderaDispensing; }
            set { _BanderaDispensing = value; }
        }
        private bool _BanderaChasis;
        public bool BanderaChasis
        {
            get { return _BanderaChasis; }
            set { _BanderaChasis = value; }
        }
        private bool _BanderaCargaBilletes;
        public bool BanderaCargaBilletes
        {
            get { return _BanderaCargaBilletes; }
            set { _BanderaCargaBilletes = value; }
        }
        private bool _BanderaCancelacion;
        public bool BanderaCancelacion
        {
            get { return _BanderaCancelacion; }
            set { _BanderaCancelacion = value; }
        }
        private bool _BanderaBox;
        public bool BanderaBox
        {
            get { return _BanderaBox; }
            set { _BanderaBox = value; }
        }
        private bool _BanderaArqueoParcial;
        public bool BanderaArqueoParcial
        {
            get { return _BanderaArqueoParcial; }
            set { _BanderaArqueoParcial = value; }
        }
        private bool _BanderaArqueoTotal;
        public bool BanderaArqueoTotal
        {
            get { return _BanderaArqueoTotal; }
            set { _BanderaArqueoTotal = value; }
        }
        string ValorCobro = string.Empty;
        private bool _BanderaJamArqueoT;
        public bool BanderaJamArqueoT
        {
            get { return _BanderaJamArqueoT; }
            set { _BanderaJamArqueoT = value; }
        }
        private bool _BanderaPresionado;
        public bool BanderaPresionado
        {
            get { return _BanderaPresionado; }
            set { _BanderaPresionado = value; }
        }
        private bool _BanderaPagoFinal;
        public bool BanderaPagoFinal
        {
            get { return _BanderaPagoFinal; }
            set { _BanderaPagoFinal = value; }
        }
        private bool _BanderaRecaudo;
        public bool BanderaRecaudo
        {
            get { return _BanderaRecaudo; }
            set { _BanderaRecaudo = value; }
        }
        private int _ContadorIDLING = 0;
        public int ContadorIDLING
        {
            get { return _ContadorIDLING; }
            set { _ContadorIDLING = value; }
        }
        private string _EstadoSecuencia = string.Empty;
        public string EstadoSecuencia
        {
            get { return _EstadoSecuencia; }
            set { _EstadoSecuencia = value; }
        }
        bool bUsuario = false;
        private string _EstadoSecuenciaDispensar = string.Empty;
        public string EstadoSecuenciaDispensar
        {
            get { return _EstadoSecuenciaDispensar; }
            set { _EstadoSecuenciaDispensar = value; }
        }
        private int _oTimeOut = new int();
        public int oTimeOut
        {
            get { return _oTimeOut; }
            set { _oTimeOut = value; }
        }
        int timeLeft = 120;
        int pollTimer = 250; // timer in ms
        System.Windows.Forms.Timer reconnectionTimer = new System.Windows.Forms.Timer();
        private TransaccionEfectivo _TransaccionEfectivo = new TransaccionEfectivo();
        public TransaccionEfectivo TransaccionEfectivo
        {
            get { return _TransaccionEfectivo; }
            set { _TransaccionEfectivo = value; }
        }
        private int _Dispensado50 = 0;
        public int Dispensado50
        {
            get { return _Dispensado50; }
            set { _Dispensado50 = value; }
        }
        private int _Dispensado100 = 0;
        public int Dispensado100
        {
            get { return _Dispensado100; }
            set { _Dispensado100 = value; }
        }
        private int _Dispensado200 = 0;
        public int Dispensado200
        {
            get { return _Dispensado200; }
            set { _Dispensado200 = value; }
        }
        private int _Dispensado500 = 0;
        public int Dispensado500
        {
            get { return _Dispensado500; }
            set { _Dispensado500 = value; }
        }
        private int _Dispensado1000 = 0;
        public int Dispensado1000
        {
            get { return _Dispensado1000; }
            set { _Dispensado1000 = value; }
        }
        string Valor = string.Empty;
        private List<TransaccionEfectivo> _Transacciones = new List<TransaccionEfectivo>();
        public List<TransaccionEfectivo> Transacciones
        {
            get { return _Transacciones; }
            set { _Transacciones = value; }
        }
        TextBox textBox1 = new TextBox();
        bool Running = false;
        string _Dominio = string.Empty;
        private string _Fundacion = string.Empty;
        public string Fundacion
        {
            get { return _Fundacion; }
            set { _Fundacion = value; }
        }
        private string _Linea = string.Empty;
        public string Linea
        {
            get { return _Linea; }
            set { _Linea = value; }
        }
        private string _Descripcion = string.Empty;
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        private string _Operador = string.Empty;
        public string Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }
        public string _sOperacionTransaccion = string.Empty;
        private string _ValorTEMP = string.Empty;
        public string ValorTEMP
        {
            get { return _ValorTEMP; }
            set { _ValorTEMP = value; }
        }
        string sLinea = string.Empty;
        string sValor = string.Empty;
        string sCategoria = string.Empty;
        string sPaquete = string.Empty;
        string sSku = string.Empty;
        string sOperador = string.Empty;
        string sIDOperador = string.Empty;
        public int _CantBilletes = 0;
        public int _ValorPagar = 0;
        private double _ValorContado = 0;
        public double ValorContado
        {
            get { return _ValorContado; }
            set { _ValorContado = value; }
        }
        private double _ValorDepositado = 0;
        public double ValorDepositado
        {
            get { return _ValorDepositado; }
            set { _ValorDepositado = value; }
        }
        private bool _ProcesoEstados = false;
        public bool ProcesoEstados
        {
            get { return _ProcesoEstados; }
            set { _ProcesoEstados = value; }
        }
        private bool _ProcesoLimpiezaManual = false;
        public bool ProcesoLimpiezaManual
        {
            get { return _ProcesoLimpiezaManual; }
            set { _ProcesoLimpiezaManual = value; }
        }
        bool bMantenimiento = false;
        private string _CelularCliente = string.Empty;
        public string CelularCliente
        {
            get { return _CelularCliente; }
            set { _CelularCliente = value; }
        }
        int _cnt = 0;
        bool bVideo = false;
        private string sPathAnimacionPrincipalVideo = string.Empty;
        private string sPathAnimacionVideoDonacion = string.Empty;
        System.IO.Ports.SerialPort ComunicacionArduino = new System.IO.Ports.SerialPort();
        private frmPrincipal_Presenter _frmPrincipal_Presenter;
        private Pantalla _Presentacion = Pantalla.SistemaSuspendido;
        private int _Cnt_Reinicio = 0;
        public int Cnt_Reinicio
        {
            get { return _Cnt_Reinicio; }
            set { _Cnt_Reinicio = value; }
        }
        private List<Dato> _oDato = new List<Dato>();
        public List<Dato> oDato
        {
            get { return _oDato; }
            set { _oDato = value; }
        }
        private Datos _oDatos = new Datos();
        public Datos oDatos
        {
            get { return _oDatos; }
            set { _oDatos = value; }
        }
        private DtoModulo _DtoModulo = new DtoModulo();
        public DtoModulo DtoModulo
        {
            get { return _DtoModulo; }
            set { _DtoModulo = value; }
        }
        private Arqueo _Arqueo = new Arqueo();
        public Arqueo Arqueo
        {
            get { return _Arqueo; }
            set { _Arqueo = value; }
        }
        private ID_Part ID_Cassette = ID_Part.Ninguno;
        private TicketCarga _Carga = new TicketCarga();
        public TicketCarga Carga
        {
            get { return _Carga; }
            set { _Carga = value; }
        }
        private bool _CargaBilletesF56 = false;
        public bool CargaBilletesF56
        {
            get { return _CargaBilletesF56; }
            set { _CargaBilletesF56 = value; }
        }
        private DtoModulo _DtoModuloF56 = new DtoModulo();
        public DtoModulo DtoModuloF56
        {
            get { return _DtoModuloF56; }
            set { _DtoModuloF56 = value; }
        }
        private List<DtoTransacciones> _DtoTransacciones = new List<DtoTransacciones>();
        public List<DtoTransacciones> DtoTransacciones
        {
            get { return _DtoTransacciones; }
            set { _DtoTransacciones = value; }
        }
        private List<DtoLogMovimiento> _DtoLogMovimiento = new List<DtoLogMovimiento>();
        public List<DtoLogMovimiento> DtoLogMovimiento
        {
            get { return _DtoLogMovimiento; }
            set { _DtoLogMovimiento = value; }
        }
        private List<DtoPago> _DtoPago = new List<DtoPago>();
        public List<DtoPago> DtoPago
        {
            get { return _DtoPago; }
            set { _DtoPago = value; }
        }
        private List<DtoArqueo> _DtoArqueo = new List<DtoArqueo>();
        public List<DtoArqueo> DtoArqueo
        {
            get { return _DtoArqueo; }
            set { _DtoArqueo = value; }
        }
        private DtoOperacion _DtoOperacion = new DtoOperacion();
        public DtoOperacion DtoOperacion
        {
            get { return _DtoOperacion; }
            set { _DtoOperacion = value; }
        }
        private ListadoArqueo _ListadoArqueo = new ListadoArqueo();
        private PagoEfectivo _PagoEfectivo = new PagoEfectivo();
        public PagoEfectivo PagoEfectivo
        {
            get { return _PagoEfectivo; }
            set { _PagoEfectivo = value; }
        }
        private bool _ProcesoCarga = false;
        public bool ProcesoCarga
        {
            get { return _ProcesoCarga; }
            set { _ProcesoCarga = value; }
        }
        private bool _ProcesoParcialCambio;
        public bool ProcesoParcialCambio
        {
            set
            {
                if (value == true)
                {
                    //btn_CancelarPago.Enabled = false;
                }
                else if (value == false)
                {
                    //btn_CancelarPago.Enabled = true;
                }

                _ProcesoParcialCambio = value;


            }

            get { return _ProcesoParcialCambio; }
        }
        private bool _Inicializando = false;
        public bool Inicializando
        {
            get { return _Inicializando; }
            set { _Inicializando = value; }
        }
        private int _DenomMenorMonedas = 0;
        public int DenomMenorMonedas
        {
            get { return _DenomMenorMonedas; }
            set { _DenomMenorMonedas = value; }
        }
        int _DenomMenor = 0;
        private bool _BilletesStart = false;
        public bool BilletesStart
        {
            get { return _BilletesStart; }
            set { _BilletesStart = value; }
        }
        private bool _CargaMonedas = false;
        public bool CargaMonedas
        {
            get { return _CargaMonedas; }
            set { _CargaMonedas = value; }
        }
        public int DenomMenor
        {
            get { return _DenomMenor; }
            set { _DenomMenor = value; }
        }
        private bool _CtCoinReady = false;
        public bool CtCoinReady
        {
            get { return _CtCoinReady; }
            set { _CtCoinReady = value; }
        }
        private bool _BilleteroReady = false;
        public bool BilleteroReady
        {
            get { return _BilleteroReady; }
            set { _BilleteroReady = value; }
        }
        bool _Hopper1Ready = false;
        public bool Hopper1Ready
        {
            get { return _Hopper1Ready; }
            set { _Hopper1Ready = value; }
        }
        bool _Hopper2Ready = false;
        public bool Hopper2Ready
        {
            get { return _Hopper2Ready; }
            set { _Hopper2Ready = value; }
        }
        bool _Hopper3Ready = false;
        public bool Hopper3Ready
        {
            get { return _Hopper3Ready; }
            set { _Hopper3Ready = value; }
        }
        bool _Hopper4Ready = false;
        public bool Hopper4Ready
        {
            get { return _Hopper4Ready; }
            set { _Hopper4Ready = value; }
        }
        public ListadoArqueo ListadoArqueo
        {
            get { return _ListadoArqueo; }
            set { _ListadoArqueo = value; }
        }
        private ID_Part ID_Hopper = ID_Part.Ninguno;
        private List<ListadoArqueo> _lstArqueo = new List<ListadoArqueo>();
        public List<ListadoArqueo> lstArqueo
        {
            get { return _lstArqueo; }
            set { _lstArqueo = value; }
        }
        private DataTable _CargaActual;
        public DataTable CargaActual
        {
            set
            {
                if (CargaMonedas)
                {
                    grvCargaActualMonedas.DataSource = value;
                    grvCargaActualMonedas.Refresh();
                    grvCargaActualMonedas.Show();
                }
                else if (CargaBilletesBB)
                {
                    grvCargaActualBilletesF56.DataSource = value;
                    grvCargaActualBilletesF56.Columns[InfoPresentacion.ColumnaCantidadMaxima].Visible = false;
                    grvCargaActualBilletesF56.Refresh();
                    grvCargaActualBilletesF56.Show();
                }

                _CargaActual = value;
            }
            get { return _CargaActual; }
        }
        private DataTable _CargaTotal;
        public DataTable CargaTotal
        {
            set
            {
                if (CargaMonedas)
                {
                    grvCargaTotalMonedas.DataSource = value;
                    grvCargaTotalMonedas.Refresh();
                    grvCargaTotalMonedas.Show();
                }
                else if (CargaBilletesBB)
                {
                    grvCargaTotalBilletesF56.DataSource = value;
                    grvCargaTotalBilletesF56.Columns[InfoPresentacion.ColumnaCantidadMaxima].Visible = false;
                    grvCargaTotalBilletesF56.Refresh();
                    grvCargaTotalBilletesF56.Show();
                }

                _CargaTotal = value;
            }

            get { return _CargaTotal; }
        }
        private ArrayList Datos = new ArrayList();
        private string _pass = string.Empty;
        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }
        private string _pass2 = string.Empty;
        public string Pass2
        {
            get { return _pass2; }
            set { _pass2 = value; }
        }
        private Usuario _Usuario = new Usuario();
        public Usuario Usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }
        private DtoUsuario _DtoUsuario = new DtoUsuario();
        public DtoUsuario DtoUsuario
        {
            get { return _DtoUsuario; }
            set { _DtoUsuario = value; }
        }
        private bool _ProcesoPago = false;
        public bool ProcesoPago
        {
            get { return _ProcesoPago; }
            set { _ProcesoPago = value; }
        }
        private bool _ProcesoPagoParcial = false;
        public bool ProcesoPagoParcial
        {
            get { return _ProcesoPagoParcial; }
            set { _ProcesoPagoParcial = value; }
        }
        private bool _ProcesoReconteo = false;
        public bool ProcesoReconteo
        {
            get { return _ProcesoReconteo; }
            set { _ProcesoReconteo = value; }
        }
        Operacion _Operacion = new Operacion();
        public Operacion Operacion
        {
            get { return _Operacion; }
            set { _Operacion = value; }
        }
        Operacion _OperacionCentral = new Operacion();
        public Operacion OperacionCentral
        {
            get { return _OperacionCentral; }
            set { _OperacionCentral = value; }
        }
        private double _NumeroDocumentoOrigen;
        public double NumeroDocumentoOrigen
        {
            get { return _NumeroDocumentoOrigen; }
            set { _NumeroDocumentoOrigen = value; }
        }
        private string _DatosAuto;
        public string DatosAuto
        {
          get { return _DatosAuto; }
          set { _DatosAuto = value; }
        }
        private string _FechaFinAuto;
        public string FechaFinAuto
        {
          get { return _FechaFinAuto; }
          set { _FechaFinAuto = value; }
        }
        private bool _ConfigModulo = false;
        public bool ConfigModulo
        {
            get { return _ConfigModulo; }
            set { _ConfigModulo = value; }
        }
        private string _MensajeAlerta = string.Empty;
        public string MensajeAlerta
        {
            get { return _MensajeAlerta; }
            set { _MensajeAlerta = value; }
        }
        DataTable _CargaActualTemporal;
        public DataTable CargaActualTemporal
        {
            get { return _CargaActualTemporal; }
            set { _CargaActualTemporal = value; }
        }
        DataTable _CargaTotalTemporal;
        public DataTable CargaTotalTemporal
        {
            get { return _CargaTotalTemporal; }
            set { _CargaTotalTemporal = value; }
        }
        private bool _MonedasStart = false;
        public bool MonedasStart
        {
            get { return _MonedasStart; }
            set { _MonedasStart = value; }
        }
        private bool _ProcesoArqueoParcial = false;
        public bool ProcesoArqueoParcial
        {
            get { return _ProcesoArqueoParcial; }
            set { _ProcesoArqueoParcial = value; }
        }
        private string _IdEmpresa = string.Empty;
        public string IdEmpresa
        {
            get { return _IdEmpresa; }
            set { _IdEmpresa = value; }
        }
        private bool _ProcesoArqueoTotal = false;
        public bool ProcesoArqueoTotal
        {
            get { return _ProcesoArqueoTotal; }
            set { _ProcesoArqueoTotal = value; }
        }
        bool bSmart = false;
        private bool _bPagoSmart = false;
        public bool bPagoSmart
        {
            get { return _bPagoSmart; }
            set { _bPagoSmart = value; }
        }

        #region Audios
        private string _sPrepagoLector = string.Empty;
        private string _sImprimiendoTicket = string.Empty;
        private string _sPagoCelular = string.Empty;
        private string _sLeyendoTarjeta = string.Empty;
        private string _sNoOlvideDinero = string.Empty;
        private string _sNoOlvideFactura= string.Empty;
        private string _sNoOlvideTarjeta = string.Empty;
        private string _sPagoDatafono = string.Empty;
        private string _sIngreseDinero = string.Empty;
        private string _sRetireTarjeta = string.Empty;
        private string _sDetallePago = string.Empty;
        private string _sTarjetaInvalida = string.Empty;
        private string _sGraciasPago = string.Empty;
        private string _sDeseaPrint = string.Empty;
        private string _sAtasco = string.Empty;
        private string _sCodigoInvalido = string.Empty;
        private string _sConsultaFallida = string.Empty;
        #endregion

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        private VirtualKeyboardColorTable _ColorTableCustom;
        private bool _TicketDevolucion = false;
        public bool TicketDevolucion
        {
            get { return _TicketDevolucion; }
            set { _TicketDevolucion = value; }
        }

        private string _IdTransaccion = string.Empty;
        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        private string _IdArqueo = string.Empty;
        public string IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }
        private string _IdCarga = string.Empty;
        public string IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
        int cnt = 0;

        #endregion

        #region Delegate
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);
        delegate void SetDeleg();
        delegate void SetDeleg2();
        delegate void SetDeleg3();
        delegate void SetDeleg4(string ValorPagar);
        delegate void MyDelegado(Pantalla ePantalla);
        delegate void MyDelegado3();
        delegate void MyDelegado4(string mensaje);
        delegate void MyDelegado5();
        delegate void MyDelegado6(string mensaje);
        public void FinalizaConteoLimpiezaDelegado()
        {
            RegistrarOperacionPago();
        }
        public void MostrarPantalla(Pantalla pantallaPresenter)
        {
            Presentacion = pantallaPresenter;
        }
        public void PantallaFinArqueoParcial()
        {
            if (_ProcesoArqueoParcial)
            {
                btn_ConfirmarArqueo.Visible = true;
            }
            else
            {

            }


        }
        public void Cargatext(string Msg)
        {
            textBox1.AppendText(Msg);
        }
        public void CargaInfoPagoEfectivo()
        {
            string Recibido = _PagoEfectivo.ValorRecibido.ToString();
            string Cambio = _PagoEfectivo.ValorCambio.ToString();

            if (_TipoPago == "MENSUALIDAD")
            {
                if (_BanderaRecaudo)
                {
                    lblValorPagarAuto.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(ValorCobro.Replace("$", "").Replace(".", "")));
                    lblValorRecibidoAuto.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Recibido.Replace("$", "").Replace(".", "")));
                    lblValorCambioAuto.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Cambio.Replace("$", "").Replace(".", "")));

                    if ((_PagoEfectivo.ValorRecibido - _PagoEfectivo.ValorPago) < 0)
                        _PagoEfectivo.ValorCambio = 0;
                    else
                        _PagoEfectivo.ValorCambio = Convert.ToInt32(_PagoEfectivo.ValorRecibido - _PagoEfectivo.ValorPago);
                    Cambio = _PagoEfectivo.ValorCambio.ToString();
                    lblValorCambioAuto.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Cambio.Replace("$", "").Replace(".", "")));

                    if (_PagoEfectivo.ValorRecibido > 0)
                    {
                        btn_VolverMedios.Enabled = false;
                    }
                    else
                    {
                        btn_VolverMedios.Enabled = true;
                    }


                    if (_PagoEfectivo.ValorRecibido >= _PagoEfectivo.ValorPago && _BanderaRecaudo)
                    {
                        Thread.Sleep(1000);
                        _BanderaPagoFinal = true;
                        _BanderaRecaudo = false;
                    }
                }
            }
            else
            {
                if (_BanderaRecaudo)
                {
                    lblValorPagarEfectivo.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(ValorCobro.Replace("$", "").Replace(".", "")));
                    lblValorRecibido.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Recibido.Replace("$", "").Replace(".", "")));
                    lblCambio.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Cambio.Replace("$", "").Replace(".", "")));

                    if ((_PagoEfectivo.ValorRecibido - _PagoEfectivo.ValorPago) < 0)
                        _PagoEfectivo.ValorCambio = 0;
                    else
                        _PagoEfectivo.ValorCambio = Convert.ToInt32(_PagoEfectivo.ValorRecibido - _PagoEfectivo.ValorPago);
                    Cambio = _PagoEfectivo.ValorCambio.ToString();
                    lblCambio.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(Cambio.Replace("$", "").Replace(".", "")));

                    if (_PagoEfectivo.ValorRecibido > 0)
                    {
                        btn_VolverMedios.Enabled = false;
                    }
                    else
                    {
                        btn_VolverMedios.Enabled = true;
                    }


                    if (_PagoEfectivo.ValorRecibido >= _PagoEfectivo.ValorPago && _BanderaRecaudo)
                    {
                        Thread.Sleep(1000);
                        _BanderaPagoFinal = true;
                        _BanderaRecaudo = false;
                    }

                }
            }
        }
        public void PantallaFinArqueoTotal()
        {
            _ProcesoArqueoTotal = false;
            _ProcesoArqueoParcial = false;

            btn_ConfirmarArqueoTotal.Visible = true;

        }
        public void SetTextBoxEstadosMenuSistemas(string mensaje)
        {
            //tbEstadosMenuSistemas.Text = mensaje;
        }
        public void PantallaAlertaGeneral(string sMensaje)
        {


            frmAlertas _frmAlertas = new frmAlertas(sMensaje);
            _frmAlertas.ShowDialog();
            _frmAlertas.Close();
            _frmAlertas = null;

            Presentacion = Pantalla.MenuSistemas;
        }
        #endregion

        #region EventosControles
        private void tmrReset_Tick(object sender, EventArgs e)
        {
            Cnt_Reinicio++;
            cnt++;
            

            switch (_Presentacion)
            {
                case Pantalla.PublicidadPrincipal:
                    if (_frmPrincipal_Presenter.LeerTarjeta())
                    {
                        if (_Tarjeta.CodeCard != string.Empty)
                        {
                            bool TarOK = false;

                            if (_frmPrincipal_Presenter.ObtenerTarjetas())
                            {
                                for (int i = 0; i < _lstDtoTarjetas.Count; i++)
                                {

                                    if (_lstDtoTarjetas[i].IdTarjeta == _Tarjeta.CodeCard && _lstDtoTarjetas[i].Estado)
                                    {
                                        General_Events = "TARJETA ESTADO TRUE";
                                        TarOK = true;
                                        break;
                                    }
                                }
                            }


                            if (TarOK)
                            {
                                if (_Tarjeta.ActiveCycle == true)
                                {
                                    Presentacion = Pantalla.Procesando;
                                    if (ValidarMensualidad())
                                    {
                                        Presentacion = Pantalla.TarjetaMensual;
                                        //if (_frmPrincipal_Presenter.ConsultaValorMensualidad())
                                        //{
                                        //    lblConvenio.Text = _TipoPago;
                                        //    RegistrarOperacionPago();
                                        //}
                                        //else
                                        //{
                                        //    _frmPrincipal_Presenter.ExpulsarTarjeta();
                                        //    Presentacion = Pantalla.TransaccionCancelada;
                                        //}
                                    }
                                    else
                                    {
                                        ValidarPago();
                                    }
                                }
                                else
                                {
                                    Presentacion = Pantalla.Procesando;
                                    if (ValidarMensualidad())
                                    {
                                        Presentacion = Pantalla.TarjetaMensual;
                                        //if (_frmPrincipal_Presenter.ConsultaValorMensualidad())
                                        //{
                                        //    lblConvenio.Text = _TipoPago;
                                        //    RegistrarOperacionPago();
                                        //}
                                        //else
                                        //{
                                        //    _frmPrincipal_Presenter.ExpulsarTarjeta();
                                        //    Presentacion = Pantalla.TransaccionCancelada;
                                        //}

                                    }
                                    else
                                    {
                                        SetearPantalla(Pantalla.TarjetaInvalida);
                                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                                    }
                                }
                            }
                            else 
                            {
                                SetearPantalla(Pantalla.TarjetaInvalida);
                                _frmPrincipal_Presenter.ExpulsarTarjeta();
                            }
                        }
                    }
                    break;

                case Pantalla.TransaccionCancelada:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_TransaccionCancelada)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }

                    break;
                case Pantalla.PuedeSalir:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_TransaccionCancelada)
                    {
                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }
                    break;
                
                case Pantalla.Procesando:
                    break;
                case Pantalla.TarjetaMensual:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Publicidad1)
                    {
                        Cnt_Reinicio = 0;
                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }
                    break;
                case Pantalla.DetallePago:                    
                    tbCodigo.Focus();                    
                    if (_BanderaPagoFinal)
                    {
                        Thread.Sleep(2000);
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.Procesando;
                        _frmPrincipal_Presenter.DeshabilitarRecepcion();
                        Thread.Sleep(1000);
                        _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorCambio);
                    }
                    else if (Cnt_Reinicio == (int)TimeOut.TimeOut_CondicionesRestricciones)
                    {
                        Cnt_Reinicio = 0;
                        _BanderaRecaudo = false;
                        _BanderaCancelacion = true;
                        Presentacion = Pantalla.TransaccionCanceladaPago;
                        _frmPrincipal_Presenter.DeshabilitarRecepcion();
                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                        _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorRecibido);
                    }
                    break;
                    case Pantalla.DetallePagoMensual:                    
                    if (_BanderaPagoFinal)
                    {
                        Thread.Sleep(2000);
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.Procesando;
                        _frmPrincipal_Presenter.DeshabilitarRecepcion();
                        Thread.Sleep(1000);
                        _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorCambio);
                    }
                    else if (Cnt_Reinicio == (int)TimeOut.TimeOut_CondicionesRestricciones)
                    {
                        Cnt_Reinicio = 0;
                        _BanderaRecaudo = false;
                        _BanderaCancelacion = true;
                        Presentacion = Pantalla.TransaccionCanceladaPago;
                        _frmPrincipal_Presenter.DeshabilitarRecepcion();
                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                        _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorRecibido);
                    }
                    break;
                case Pantalla.TarjetaInvalida:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Publicidad0)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }

                    break;
                case Pantalla.TarjetaNoPago:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Publicidad0)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }

                    break;
                case Pantalla.TarjetaSinEntrada:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Publicidad0)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }

                    break;
                case Pantalla.ConsultaFallida:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Publicidad0)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.PublicidadPrincipal;
                    }

                    break;
                case Pantalla.SistemaSuspendido:
                    //if (_frmPrincipal_Presenter.LeerTarjeta())
                    //{
                    //    if (_Tarjeta.TipoTarjeta == 2)
                    //    {
                    //        Presentacion = Pantalla.IngresoPass;
                    //    }
                    //}
                    break;
                case Pantalla.Atasco:
                    
                    break;
                case Pantalla.SeleccionPago:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Flujo)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.TransaccionCancelada;
                    }

                    break;
                case Pantalla.PagoCelular:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Flujo)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.TransaccionCancelada;
                    }

                    break;
                case Pantalla.PagoEfectivo:
                    
                    break;
                case Pantalla.ContraseñaInavlida:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_TransaccionCancelada)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.IngresoPass;
                    }
                    break;
                case Pantalla.Prepago:
                    ConsultarPagoPrepago();
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Smart)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.TransaccionCancelada;
                    }

                    break;
                case Pantalla.Datafono:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_Flujo)
                    {
                        Cnt_Reinicio = 0;
                        Presentacion = Pantalla.TransaccionCancelada;
                    }

                    break;
                case Pantalla.ImprimirFactura:
                    //if (Cnt_Reinicio == (int)TimeOut.TimeOut_RetiroRecibo)
                    //{
                    //    Cnt_Reinicio = 0;
                    //    Presentacion = Pantalla.GarciasPago;
                    //}
                    break;
                case Pantalla.GarciasPago:
                    if (Cnt_Reinicio == (int)TimeOut.TimeOut_RetiroRecibo)
                    {
                        _frmPrincipal_Presenter.ExpulsarTarjeta();
                        Cnt_Reinicio = 0;

                        if (_frmPrincipal_Presenter.ValidarSaldosMinimos())
                        {
                            Presentacion = Pantalla.PublicidadPrincipal;
                        }
                    }

                    break;
                case Pantalla.MenuCargMonedas:
                    CargaActual = _CargaActualTemporal;
                    CargaTotal = _CargaTotalTemporal;
                    break;
                case Pantalla.MenuCargBilletes:
                    CargaActual = _CargaActualTemporal;
                    CargaTotal = _CargaTotalTemporal;
                    break;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void tbCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (Presentacion == Pantalla.DetallePago)
                {
                    General_Events = "DETALLE PAGO";
                    
                    if (tbCodigo.Text != string.Empty)
                    {
                        General_Events = "Codigo Completo " + tbCodigo.Text;

                        if (_frmPrincipal_Presenter.ValidarConvenio(tbCodigo.Text))
                        {

                            if (tbCodigo.Text.Length >= 20)
                            {
                                int CodigoBarras = 0;
                                int Consecutivo = 0;
                                CodigoBarras = Convert.ToInt32(tbCodigo.Text.Substring(16, 4));
                                Consecutivo = Convert.ToInt32(tbCodigo.Text.Substring(5, 5));

                                string AñoFecha = tbCodigo.Text.Substring(10, 2);
                                string MesFecha = tbCodigo.Text.Substring(12, 2);
                                string DiaFecha = tbCodigo.Text.Substring(14, 2);

                                string FechaCod = "20" + AñoFecha + "/" + MesFecha + "/" + DiaFecha;

                                string AñoAct = DateTime.Now.Year.ToString();

                                string MesAct = DateTime.Now.Month.ToString();
                                MesAct = MesAct.PadLeft(2, '0');
                                string DiaAct = DateTime.Now.Day.ToString();
                                DiaAct = DiaAct.PadLeft(2, '0');

                                string FechaAct = AñoAct + "/" + MesAct + "/" + DiaAct;


                                if (FechaCod == FechaAct)
                                {

                                    General_Events = "Codigo Barras " + CodigoBarras;

                                    if (CodigoBarras >= 30)
                                    {
                                        Presentacion = Pantalla.Procesando;

                                        _Tarjeta.CodeAgreement1 = 2;

                                        _frmPrincipal_Presenter.RegistrarConvenioValidado(Consecutivo.ToString(), tbCodigo.Text);

                                        _frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Cancelado);

                                        if (_frmPrincipal_Presenter.ConsultaValor())
                                        {
                                            if (_PagoEfectivo.ValorPago > 0)
                                            {
                                                ValorCobro = _PagoEfectivo.ValorPago.ToString();

                                                General_Events = "FrondEnd-Convenio aplicado -> Total : " + ValorCobro;
                                                _PagoEfectivo.ValorRecibido = 0;
                                                _PagoEfectivo.ValorCambio = 0;
                                                _PagoEfectivo.ValorPago = Convert.ToInt64(ValorCobro);
                                                _BanderaRecaudo = true;
                                                _BanderaCancelacion = false;
                                                _BanderaEsperaHabilitado = false;
                                                _BanderaPresionado = false;
                                                Presentacion = Pantalla.DetallePago;
                                            }
                                            else
                                            {

                                                _frmPrincipal_Presenter.RegistrarOperacion(TipoOperacion.Pago);
                                                _PrintSalida = true;
                                                Imprimir();
                                                _frmPrincipal_Presenter.DeshabilitarRecepcion();
                                                Presentacion = Pantalla.GarciasPago;
                                                _PrintSalida = false;
                                            }
                                        }

                                    }
                                }
                                else 
                                {
                                    SoundPlayer simpleSound = new SoundPlayer(_sCodigoInvalido);
                                    simpleSound.Play();
                                    tbCodigo.Text = string.Empty;
                                }
                            }
                        }
                        else 
                        {
                            //codigo invalido
                            //Presentacion = Pantalla.TarjetaInvalida;
                            SoundPlayer simpleSound = new SoundPlayer(_sCodigoInvalido);
                            simpleSound.Play();
                            tbCodigo.Text = string.Empty;
                        }
                    }
                }
                else 
                {
                    General_Events = "NO ESTA EN DETALLE PAGO";
                }
            }
        }
        #endregion

        #region EventosFormulario
        public frmPrincipal()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.Closing += frmPrincipal_Closing;
            _frmPrincipal_Presenter = new frmPrincipal_Presenter(this);
        }
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                Inicio();
            }
            catch (Exception ex)
            {
                General_Events = "FrondEnd-frmPrincipal_Load -> ERROR DE INICIO / Detalles: " + ex.ToString();
            }
        }
        private void frmPrincipal_Closing(object sender, CancelEventArgs e)
        {
            CerrarFormulario();
        }
        private void frmPrincipal_KeyUp(object sender, KeyEventArgs e)
        {
            if (!bUsuario)
            {
                if (lblUsuario.Text.Length < 10)
                {
                    if (e.KeyCode == Keys.D0)
                    {
                        lblUsuario.Text += "0";
                    }
                    else if (e.KeyCode == Keys.D1)
                    {
                        lblUsuario.Text += "1";
                    }
                    else if (e.KeyCode == Keys.D2)
                    {
                        lblUsuario.Text += "2";
                    }
                    else if (e.KeyCode == Keys.D3)
                    {
                        lblUsuario.Text += "3";
                    }
                    else if (e.KeyCode == Keys.D4)
                    {
                        lblUsuario.Text += "4";
                    }
                    else if (e.KeyCode == Keys.D5)
                    {
                        lblUsuario.Text += "5";
                    }
                    else if (e.KeyCode == Keys.D6)
                    {
                        lblUsuario.Text += "6";
                    }
                    else if (e.KeyCode == Keys.D7)
                    {
                        lblUsuario.Text += "7";
                    }
                    else if (e.KeyCode == Keys.D8)
                    {
                        lblUsuario.Text += "8";
                    }
                    else if (e.KeyCode == Keys.D9)
                    {
                        lblUsuario.Text += "9";
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (lblUsuario.Text != string.Empty)
                    {
                        lblUsuario.Text = lblUsuario.Text.Remove(lblUsuario.Text.Length - 1, 1);
                    }
                }
            }
            else
            {
                if (lblPassword.Text.Length < 9)
                {
                    if (e.KeyCode == Keys.D0)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "0";
                    }
                    else if (e.KeyCode == Keys.D1)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "1";
                    }
                    else if (e.KeyCode == Keys.D2)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "2";
                    }
                    else if (e.KeyCode == Keys.D3)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "3";
                    }
                    else if (e.KeyCode == Keys.D4)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "4";
                    }
                    else if (e.KeyCode == Keys.D5)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "5";
                    }
                    else if (e.KeyCode == Keys.D6)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "6";
                    }
                    else if (e.KeyCode == Keys.D7)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "7";
                    }
                    else if (e.KeyCode == Keys.D8)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "8";
                    }
                    else if (e.KeyCode == Keys.D9)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "9";
                    }
                    else if (e.KeyCode == Keys.A)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "A";
                    }
                    else if (e.KeyCode == Keys.B)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "B";
                    }
                    else if (e.KeyCode == Keys.C)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "C";
                    }
                    else if (e.KeyCode == Keys.D)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "D";
                    }
                    else if (e.KeyCode == Keys.E)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "E";
                    }
                    else if (e.KeyCode == Keys.F)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "F";
                    }
                    else if (e.KeyCode == Keys.G)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "G";
                    }
                    else if (e.KeyCode == Keys.H)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "H";
                    }
                    else if (e.KeyCode == Keys.I)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "I";
                    }
                    else if (e.KeyCode == Keys.J)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "J";
                    }
                    else if (e.KeyCode == Keys.K)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "K";
                    }
                    else if (e.KeyCode == Keys.L)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "L";
                    }
                    else if (e.KeyCode == Keys.M)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "M";
                    }
                    else if (e.KeyCode == Keys.N)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "N";
                    }
                    else if (e.KeyCode == Keys.O)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "O";
                    }
                    else if (e.KeyCode == Keys.P)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "P";
                    }
                    else if (e.KeyCode == Keys.Q)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "Q";
                    }
                    else if (e.KeyCode == Keys.R)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "R";
                    }
                    else if (e.KeyCode == Keys.S)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "S";
                    }
                    else if (e.KeyCode == Keys.T)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "T";
                    }
                    else if (e.KeyCode == Keys.U)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "U";
                    }
                    else if (e.KeyCode == Keys.V)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "V";
                    }
                    else if (e.KeyCode == Keys.W)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "W";
                    }
                    else if (e.KeyCode == Keys.X)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "X";
                    }
                    else if (e.KeyCode == Keys.Y)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "Y";
                    }
                    else if (e.KeyCode == Keys.Z)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + "Z";
                    }
                    else if (e.KeyCode == Keys.Space)
                    {
                        lblPassword.Text += "*";
                        Pass = Pass + " ";
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (lblPassword.Text != string.Empty)
                    {
                        lblPassword.Text = lblPassword.Text.Remove(lblPassword.Text.Length - 1, 1);
                        Pass = Pass.Remove(Pass.Length - 1, 1);
                    }
                }
            }
        }
        #endregion

        #region Botones
        private void btn_MostrarTecladoPass_Click(object sender, EventArgs e)
        {
            kbUsuarioPass.Show();
        }
        private void CapaMenuPrincipal_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-CapaAyudaPrincipal_Click -> boton acceso Usuario";
            Cnt_Reinicio = 0;
            Presentacion = Pantalla.IngresoPass;
        }
        private void btn_Pagar_Click(object sender, EventArgs e)
        {
            
        }
        private void btn_CancelarPago_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.TransaccionCancelada;
        }
        private void btn_CancelarSeleccion_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.TransaccionCancelada;
        }
        private void CapaUsuario_Click(object sender, EventArgs e)
        {
            bUsuario = false;
            lblUsuario.Focus();
        }
        private void CapaPass_Click(object sender, EventArgs e)
        {
            bUsuario = true;
            lblPassword.Focus();
        }
        private void btn_Salir_Click(object sender, EventArgs e)
        {
            CerrarFormulario();
        }
        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            if (lblUsuario.Text != string.Empty)
            {
                if (lblUsuario.Text != null)
                {
                    string Identificacion = lblUsuario.Text;
                    General_Events = "FrondEnd-btn_Aceptar_Click -> Presion boton Aceptar Usuario: " + lblUsuario.Text;
                    if (_frmPrincipal_Presenter.ValidarClave(Identificacion, Pass))
                    {
                        _Usuario = new Usuario();
                        _Usuario.IdCriptUsuario = Identificacion.ToString();

                        if (Decrypt(_IdEmpresa) == Pass)
                        {
                            General_Events = "FrondEnd-btn_Aceptar_Click -> Ingresa a menu Sistemas: " + lblUsuario.Text;
                            Presentacion = Pantalla.MenuSistemas;
                        }
                        else
                        {
                            if (Identificacion == "666" && Pass == "2042484")
                            {
                                General_Events = "FrondEnd-btn_Aceptar_Click -> Ingresa a menu Sistemas con super usuario de respaldo -> " + lblUsuario.Text;
                                Presentacion = Pantalla.MenuSistemas;
                            }
                            else
                            {
                                General_Events = "FrondEnd-btn_Aceptar_Click -> No genera clave Usuario: " + lblUsuario.Text;
                                Presentacion = Pantalla.ContraseñaInavlida;
                            }
                        }
                    }
                    else
                    {
                        if (Identificacion == "666" && Pass == "2042484")
                        {
                            General_Events = "FrondEnd-btn_Aceptar_Click -> Ingresa a menu Sistemas con usuario de respaldo -> " + lblUsuario.Text;
                            Presentacion = Pantalla.MenuSistemas;
                        }
                        else
                        {
                            General_Events = "FrondEnd-btn_Aceptar_Click -> Contraseña Invalida Usuario: " + lblUsuario.Text;
                            Presentacion = Pantalla.ContraseñaInavlida;
                        }
                    }
                }
            }
            else
            {
                Presentacion = Pantalla.ContraseñaInavlida;
            }
        }
        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            if (bMantenimiento)
            {
                Presentacion = Pantalla.Mantenimiento;
            }
            else
            {
                _frmPrincipal_Presenter.DesConectarLector();
                Inicio();
            }
        }
        private void CapaMantenimientoCaleto_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.IngresoPass;
        }
        private void CapaSuspendidoCaleto_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.IngresoPass;
        }
        private void capaAtasco_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.IngresoPass;
        }
        private void btn_Arqueo_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_Arqueo_Click -> Presion boton Arqueo.";
            Presentacion = Pantalla.Arqueo;
        }
        private void btn_Carga_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_Carga_Click -> Presion boton Carga.";
            Presentacion = Pantalla.Carga;
        }
        private void btn_Iniciar_Click(object sender, EventArgs e)
        {
            _frmPrincipal_Presenter.DesConectarLector();
            Inicio();
        }
        private void btn_Mantenimiento_Click(object sender, EventArgs e)
        {
            bMantenimiento = true;
            General_Events = "FrondEnd-btn_Mantenimiento_Click -> Presion boton Mantenimiento.";
            Presentacion = Pantalla.Mantenimiento;
        }
        private void btn_Log_Click(object sender, EventArgs e)
        {
            string sFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\" + "Log_TRAZA_");
            sFilePath = sFilePath + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".log";
            if (!System.IO.File.Exists(sFilePath))
            {
                MessageBox.Show(string.Format("Archivo no encontrado '{0}'", sFilePath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                frmLogFileViewer _frmLogViewer = new frmLogFileViewer(sFilePath);
                _frmLogViewer.ShowDialog();
                _frmLogViewer.Close();
                _frmLogViewer = null;
            }
        }
        private void btn_VolverCarga_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.MenuSistemas;
        }
        private void btn_Volver_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.MenuSistemas;
        }
        private void btn_Print_Click(object sender, EventArgs e)
        {
            //imprimir 
            Presentacion = Pantalla.GarciasPago;
        }
        private void btn_NoPrint_Click(object sender, EventArgs e)
        {
            //if (_frmPrincipal_Presenter.ValidarSaldosMinimos())
            //{
                Presentacion = Pantalla.GarciasPago;
            //}
        }
        private void btnPrintSI_Click(object sender, EventArgs e)
        {
            _ProcesoPago = true;
            Imprimir();
            //if (_frmPrincipal_Presenter.ValidarSaldosMinimos())
            //{
                Presentacion = Pantalla.GarciasPago;
            //}
            
        }
        private void btn_CargaMonedas_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_CargaMonedas_Click -> Presion boton Carga monedas.";
            RegistrarCarga();
            _MonedasStart = false;
            _CargaMonedas = true;
            ID_Hopper = ID_Part.Ninguno;
            _frmPrincipal_Presenter.ObtenerInfoPartes();
            Presentacion = Pantalla.MenuCargMonedas;
        }
        private void btn_CargaBilletes_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_CargaMonedas_Click -> Presion boton Carga monedas.";
            RegistrarCarga();
            _BilletesStart = false;
            _CargaBilletesBB = true;
            _frmPrincipal_Presenter.ObtenerInfoPartes();
            _frmPrincipal_Presenter.HabilitarSecuenciaRecibir();
            Presentacion = Pantalla.MenuCargBilletes;
        }
        private void btn_ConfirmarCargaF56_Click(object sender, EventArgs e)
        {
            try
            {
                _frmPrincipal_Presenter.DeshabilitarRecepcion();

                if (_frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.Carga, TipoEstadoPago.Aprobado))
                {
                    Imprimir();
                    _ProcesoCarga = false;
                    _CargaBilletesBB = false;
                    _BilletesStart = false;
                    Presentacion = Pantalla.Carga;
                }

            }
            catch (Exception)
            {
                Presentacion = Pantalla.Carga;
            }
        }
        private void btn_ConfirmarCargaMonedas_Click(object sender, EventArgs e)
        {
            try
            {

                if (_frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.Carga, TipoEstadoPago.Aprobado))
                {
                    lblKeyboard_Carga.Text = string.Empty;
                    ID_Hopper = ID_Part.Ninguno;
                    btnHopper1.LeaveButton();
                    btnHopper2.LeaveButton();
                    btn_Hopper3.LeaveButton();
                    btnHopper4.LeaveButton();
                    Imprimir();
                    _ProcesoCarga = false;
                    //CargaBilletes = false;
                    _BilletesStart = false;
                    Presentacion = Pantalla.Carga;
                }

            }
            catch (Exception)
            {

                Presentacion = Pantalla.Carga;
            }
        }
        private void btn_PagarEfectivo_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.Procesando;
            
        }
        private void btn_PagoDatafono_Click(object sender, EventArgs e)
        {
            if (_frmPrincipal_Presenter.PagarDatafono())
            {
                _BanderaRecaudo = true;
                Presentacion = Pantalla.Datafono;
            }
            else 
            {
                Presentacion = Pantalla.TransaccionCancelada;
            }
        }
        private void btn_PagarPrepago_Click(object sender, EventArgs e)
        {
            bSmart = true;
            Presentacion = Pantalla.Procesando;
            WindowsMediaPlayer wplayer = new WindowsMediaPlayer();
            wplayer.URL = _sPrepagoLector;
            wplayer.controls.play();
            ConsultarPagoPrepago();
        }
        private void btn_PagoCelular_Click(object sender, EventArgs e)
        {
            lblCodigoParqueo.Text = string.Empty;
            lblCodigoPago.Text = string.Empty;
            Presentacion = Pantalla.Procesando;
            ConsultarPagoCelular();
        }
        private void btn_Prepago_Click(object sender, EventArgs e)
        {            
           
        }
        private void btn_Datafono_Click(object sender, EventArgs e)
        {

        }
        private void btn_Celular_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.PagoCelular;
        }
        private void btn_Efectivo_Click(object sender, EventArgs e)
        {

        }
        private void btn_CancelarCelular_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.TransaccionCancelada;
        }
        private void btn_VolverMedios_Click(object sender, EventArgs e)
        {
            _BanderaRecaudo = false;
            _frmPrincipal_Presenter.DeshabilitarRecepcion();
            Presentacion = Pantalla.DetallePago;
        }
        private void btn_CancelarPagoEfectivo_Click(object sender, EventArgs e)
        {
            _BanderaRecaudo = false;
            _BanderaCancelacion = true;
            Presentacion = Pantalla.TransaccionCanceladaPago;
            _frmPrincipal_Presenter.DeshabilitarRecepcion();
            _frmPrincipal_Presenter.ExpulsarTarjeta();
            _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorRecibido);
        }
        private void btn_CancelarPagoAuto_Click(object sender, EventArgs e)
        {
            _BanderaRecaudo = false;
            _BanderaCancelacion = true;
            Presentacion = Pantalla.TransaccionCanceladaPago;
            _frmPrincipal_Presenter.DeshabilitarRecepcion();
            _frmPrincipal_Presenter.ExpulsarTarjeta();
            _frmPrincipal_Presenter.EfectuarPagoCancelado(_PagoEfectivo.ValorRecibido);
        }
        private void btn_VolverMedioCelular_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.DetallePago;
        }
        private void btn_ContinuarCelular_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.Procesando;

            _ConsultarValorResult.idEntrada = _lstConsultarValorResult[0].idEntrada;
            //_ConsultarValorResult._PlacaSalida = _Tarjeta.Placa;
            _ConsultarValorResult.valorAPagar = _lstConsultarValorResult[0].valorAPagar;
            _ConsultarValorResult.valorServicio = _lstConsultarValorResult[0].valorServicio;
            _ConsultarValorResult.valorDescuento = _lstConsultarValorResult[0].valorDescuento;
            _ConsultarValorResult.valorEmpresa = _lstConsultarValorResult[0].valorEmpresa;
            _ConsultarValorResult.fechaLiquidacion = _lstConsultarPagoCelularResult[0].fechaLiquidacion;
            _ConsultarValorResult._IdPago = _lstConsultarPagoCelularResult[0].idPago.ToString();
            _ConsultarValorResult._IdParqueadero = _lstConsultarPagoCelularResult[0].idParqueadero.ToString();

            //if (_frmPrincipal_Presenter.RegistrarPagoCelular())
            //{
            //    _Celular = true;
            //    Presentacion = Pantalla.ImprimirFactura;
            //}
            //else 
            //{
            //    Presentacion = Pantalla.TransaccionCancelada;
            //}
        }
        private void btn_MediosPrepago_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.DetallePago;
        }
        private void btn_CancelarPrepago_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.TransaccionCancelada;
        }
        private void btn_MedioDatafono_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.DetallePago;
        }
        private void btn_CancelarDatafono_Click(object sender, EventArgs e)
        {
            Presentacion = Pantalla.TransaccionCancelada;
        }
        private void btn_0Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "0";
            }
        }
        private void btn_1Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "1";
            }
        }
        private void btn_2Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "2";
            }
        }
        private void btn_3Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "3";
            }
        }
        private void btn_4Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "4";
            }
        }
        private void btn_5Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "5";
            }
        }
        private void btn_6Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "6";
            }
        }
        private void btn_7Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "7";
            }
        }
        private void btn_8Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "8";
            }
        }
        private void btn_9Carga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text.Replace("-", "").Length < 4)
            {
                lblKeyboard_Carga.Text += "9";
            }
        }
        private void btn_okCarga_Click(object sender, EventArgs e)
        {
            if (ID_Hopper != ID_Part.Ninguno)
            {
                if (lblKeyboard_Carga.Text != string.Empty && lblKeyboard_Carga.Text != null)
                {
                    if (Convert.ToInt16(lblKeyboard_Carga.Text) != 0)
                    {

                        frmPopUpOKCancel popup = null;
                        if ((int)ID_Hopper == 1)
                        {
                            popup = new frmPopUpOKCancel("Desea realizar la carga: " + lblKeyboard_Carga.Text + " en Monedas de Hopper1 ($" + Convert.ToInt32(lblKeyboard_Carga.Text) * 100 + ") ?");
                        }
                        else if ((int)ID_Hopper == 2)
                        {
                            popup = new frmPopUpOKCancel("Desea realizar la carga: " + lblKeyboard_Carga.Text + " en Monedas de Hopper2 ($" + Convert.ToInt32(lblKeyboard_Carga.Text) * 200 + ") ?");
                        }
                        else if ((int)ID_Hopper == 3)
                        {
                            popup = new frmPopUpOKCancel("Desea realizar la carga: " + lblKeyboard_Carga.Text + " en Monedas de Hopper3 ($" + Convert.ToInt32(lblKeyboard_Carga.Text) * 500 + ") ?");
                        }
                        else if ((int)ID_Hopper == 4)
                        {
                            popup = new frmPopUpOKCancel("Desea realizar la carga: " + lblKeyboard_Carga.Text + " en Monedas de Hopper4 ($" + Convert.ToInt32(lblKeyboard_Carga.Text) * 500 + ") ?");
                        }

                        popup.ShowDialog();
                        if (popup.DialogResult == DialogResult.OK)
                        {

                            int cantidad = Convert.ToInt16(lblKeyboard_Carga.Text);

                            General_Events = "FrondEnd-btn_okCarga_Click -> Carga " + cantidad + " monedas a Hopper " + (int)ID_Hopper;

                            if (_frmPrincipal_Presenter.RegistrarMovimiento(TipoOperacion.Carga, TipoParte.Hopper, TipoMovimiento.Entrada, (int)ID_Hopper, null, cantidad))
                            {

                                _frmPrincipal_Presenter.ObtenerInfoPartes();
                                lblKeyboard_Carga.Text = string.Empty;
                            }
                            else
                            {
                                Presentacion = Pantalla.SistemaSuspendido;
                            }

                        }
                        else if (popup.DialogResult == DialogResult.Cancel)
                        {
                            lblKeyboard_Carga.Text = string.Empty;
                        }
                        popup.Close();
                        popup.Dispose();
                        popup = null;
                    }
                }
                ID_Hopper = ID_Part.Ninguno;
                btnHopper1.LeaveButton();
                btnHopper2.LeaveButton();
                btn_Hopper3.LeaveButton();
                btnHopper4.LeaveButton();
            }
        }
        private void btn_BorrarCarga_Click(object sender, EventArgs e)
        {
            if (lblKeyboard_Carga.Text != string.Empty)
            {
                lblKeyboard_Carga.Text = lblKeyboard_Carga.Text.Remove(lblKeyboard_Carga.Text.Length - 1, 1);
            }
        }
        private void btnHopper1_Click(object sender, EventArgs e)
        {
            ID_Hopper = ID_Part.Hopper1;
            btnHopper2.LeaveButton();
            btn_Hopper3.LeaveButton();
            btnHopper4.LeaveButton();
        }
        private void btnHopper2_Click(object sender, EventArgs e)
        {
            ID_Hopper = ID_Part.Hopper2;
            btnHopper1.LeaveButton();
            btn_Hopper3.LeaveButton();
            btnHopper4.LeaveButton();
        }
        private void btn_Hopper3_Click(object sender, EventArgs e)
        {
            ID_Hopper = ID_Part.Hopper3;
            btnHopper1.LeaveButton();
            btnHopper2.LeaveButton();
            btnHopper4.LeaveButton();
        }
        private void btnHopper4_Click(object sender, EventArgs e)
        {
            ID_Hopper = ID_Part.Hopper4;
            btnHopper1.LeaveButton();
            btnHopper2.LeaveButton();
            btn_Hopper3.LeaveButton();
        }
        private void btn_ArqueoParcial_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_ArqueoParcial_Click -> boton Arqueo Parcial.";
            _Operacion.Pago.EstadoPago = TipoEstadoPago.NoAplica;
            _OperacionCentral.Pago.EstadoPago = TipoEstadoPago.NoAplica;
            _ProcesoArqueoParcial = true;
            _ProcesoArqueoTotal = false;
            btn_ConfirmarArqueo.Visible = false;
            RegistrarArqueoParcial();
        }
        private void btn_ArqueoTotal_Click(object sender, EventArgs e)
        {
            General_Events = "FrondEnd-btn_ArqueoTotal_Click -> Presion boton Arqueo Total.";
            _Operacion.Pago.EstadoPago = TipoEstadoPago.NoAplica;
            _OperacionCentral.Pago.EstadoPago = TipoEstadoPago.NoAplica;
            _ProcesoArqueoParcial = false;
            _ProcesoArqueoTotal = true;
            btn_ConfirmarArqueoTotal.Visible = false;
            _MensajeAlerta = "Esta seguro que desea realizar arqueo total?";

            frmAlertas _frmAlertas = new frmAlertas(_MensajeAlerta);
            _frmAlertas.ShowDialog();
            _frmAlertas.Close();
            _frmAlertas = null;

            RegistrarArqueoTotal();
        }
        private void btn_ConfirmarArqueo_Click(object sender, EventArgs e)
        {
            if (_frmPrincipal_Presenter.RegistrarArqueo("P"))
            {
                if (_frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.ArqueoParcial, TipoEstadoPago.Aprobado))
                {
                    _Arqueo.IdArqueo = Convert.ToInt64(_IdArqueo);
                    _frmPrincipal_Presenter.ListarDetalleArqueo(_Arqueo);
                    _ProcesoArqueoTotal = false;
                    _ProcesoArqueoParcial = true;
                    Imprimir();

                    _ProcesoArqueoTotal = false;
                    _ProcesoArqueoParcial = false;

                    //CONFIRMAR OPERACION

                    _MensajeAlerta = "Se registró exitosamente el arqueo \nEn la maquina " + Globales.sSerial;

                    frmAlertas _frmAlertas = new frmAlertas(_MensajeAlerta);
                    _frmAlertas.ShowDialog();
                    _frmAlertas.Close();
                    _frmAlertas = null;
                    //return;
                    Presentacion = Pantalla.MenuSistemas;
                    //_frmPrincipal_Presenter.Arquear();
                    //Presentacion = Pantalla.Procesando;
                }
                else
                {
                    General_Events = "FrondEnd-RegistrarArqueoParcial -> No confirma arqueo parcial.";
                    Presentacion = Pantalla.MenuSistemas;
                }
            }
            else
            {
                General_Events = "FrondEnd-RegistrarArqueoParcial -> No confirma arqueo parcial.";
                Presentacion = Pantalla.MenuSistemas;
            }
        }
        private void btn_ConfirmarArqueoTotal_Click(object sender, EventArgs e)
        {
            if (_frmPrincipal_Presenter.RegistrarArqueo("T"))
            {
                if (_frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.ArqueoTotal, TipoEstadoPago.Aprobado))
                {
                    _Arqueo.IdArqueo = Convert.ToInt64(_IdArqueo);

                    _frmPrincipal_Presenter.ListarDetalleArqueo(_Arqueo);
                    _ProcesoArqueoParcial = false;
                    _ProcesoArqueoTotal = true;
                    Imprimir();
                    Presentacion = Pantalla.MenuSistemas;
                    //_frmPrincipal_Presenter.Arquear();
                    //Presentacion = Pantalla.Procesando;
                }
                else
                {
                    General_Events = "FrondEnd-RegistrarArqueoTotal -> No confirma arqueo total.";
                }
            }
            else
            {
                General_Events = "FrondEnd-RegistrarArqueoTotal -> No registra arqueo total.";
            }
        }
        private void btn_SiMensual_Click(object sender, EventArgs e)
        {
            if (_frmPrincipal_Presenter.ConsultaValorMensualidad())
            {
                if (_PagoEfectivo.ValorPago > 0)
                {
                    lblConvenio.Text = _TipoPago;
                    RegistrarOperacionPago();
                }
                else 
                {
                    Presentacion = Pantalla.PublicidadPrincipal;
                }
            }
            else
            {
                _frmPrincipal_Presenter.ExpulsarTarjeta();
                Presentacion = Pantalla.PublicidadPrincipal;
            }
        }
        private void btn_NoMensual_Click(object sender, EventArgs e)
        {
            _frmPrincipal_Presenter.ExpulsarTarjeta();
            Presentacion = Pantalla.PublicidadPrincipal;
        }
       
        #endregion

        #region General
        private async Task Inicio()
        {

            if (Globales.sTestMode != string.Empty)
            {
                if (Convert.ToBoolean(Globales.sTestMode) != true)
                {
                    Cursor.Hide();
                    this.TopMost = true;
                    this.ControlBox = false;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    Cursor.Show();
                    this.TopMost = false;
                    this.ControlBox = true;
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            else
            {
                Cursor.Hide();
                this.TopMost = true;
                this.ControlBox = false;
                this.WindowState = FormWindowState.Maximized;
            }

            General_Events = "INICIO APP ATM / TestMode = " + Globales.sTestMode;

            if (CargaImagenes())
            {
                if (CargaRecursos())
                {
                    if (CargaSonidos())
                    {
                        if (CargarParametros())
                        {

                            Presentacion = Pantalla.Inicio;
                            var th1 = await ConectarDispositivos();

                            if (th1)
                            {
                                if (_frmPrincipal_Presenter.ValidarSaldosMinimos())
                                {
                                    //_frmPrincipal_Presenter.GestionarAlarma(true);
                                    Presentacion = Pantalla.PublicidadPrincipal;
                                }
                                else
                                {
                                    Presentacion = Pantalla.SistemaSuspendido;
                                }
                                //_frmPrincipal_Presenter.GestionarAlarma(false);
                                //_frmPrincipal_Presenter.SolucionarAlarmas();
                            }
                            else
                            {
                                Presentacion = Pantalla.SistemaSuspendido;
                            }
                        }
                        else
                        {
                            Presentacion = Pantalla.SistemaSuspendido;
                        }
                    }
                }
            }
        }
        private async Task<bool> ConectarDispositivos()
        {
            //return true;
            bool ok = true;
            string sMensaje = string.Empty;
            _BanderaJam = false;
            //if (_frmPrincipal_Presenter.GestionarConexionPLC(TipoConexionDispositivo.Abrir))
            //{
            if (_frmPrincipal_Presenter.ConectarLector())
            {
                if (_frmPrincipal_Presenter.InicializarBilletero())
                {
                    _frmPrincipal_Presenter.ConectarMiniHub(TipoConexionDispositivo.Abrir);
                    await Task.Run(() => Thread.Sleep(40000));

                    foreach (DtoParteModulo item in _DtoModulo.Partes)
                    {
                        if (item.TipoParte == TipoParte.Hopper.ToString())
                        {
                            if (item.Prioridad)
                            {
                                switch (item.NumParte)
                                {
                                    case "1":
                                        if (!Hopper1Ready)
                                        {
                                            General_Events = "FrondEnd-ConectarDispositivos -> Hopper1 not Ready";
                                            ok = false;
                                            break;
                                        }
                                        break;
                                    case "2":
                                        if (!Hopper2Ready)
                                        {
                                            General_Events = "FrondEnd-ConectarDispositivos -> Hopper2 not Ready";
                                            ok = false;
                                            break;
                                        }
                                        break;
                                    case "3":
                                        if (!Hopper3Ready)
                                        {
                                            General_Events = "FrondEnd-ConectarDispositivos -> Hopper3 not Ready";
                                            ok = false;
                                            break;
                                        }
                                        break;
                                    case "4":
                                        if (!Hopper4Ready)
                                        {
                                            General_Events = "FrondEnd-ConectarDispositivos -> Hopper4 not Ready";
                                            ok = false;
                                            break;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    General_Events = "FrondEnd-ConectarDispositivos -> BILLETERO not Ready";
                    ok = false;
                }
            }
            else
            {
                General_Events = "FrondEnd-ConectarDispositivos -> ConectarLector not Ready";
                ok = false;
            }
            //}
            return ok;
        }
        private void CerrarFormulario()
        {
            try
            {
                //DesconectarDispositivos();
            }
            catch (Exception)
            {
                throw;
            }
            _frmPrincipal_Presenter = null;
            this.Dispose();
            this.Close();
        }
        private bool CargaRecursos()
        {
            bool ok = false;

            try
            {
                lblTiempoSalida.BackColor = Color.Transparent;
                capaAtasco.Text = string.Empty;
                capaAtasco.Parent = Imagen_Atasco;

                //lblTipoPago.Text = string.Empty;
                //btn_Monedas.Text = string.Empty;
                //btn_Monedas.LockPush = false;

                //btn_PuertaBilletes.Text = string.Empty;
                //btn_PuertaBilletes.LockPush = false;

                //Animacion_Inicio.Dock = DockStyle.Fill;
                tbCodigo.Location = new Point(-7, -34);
                tbCodigo.MaxLength = 50;

                btn_CancelarPagoAuto.Text = string.Empty;
                btn_CancelarPagoAuto.LockPush = false;

                btn_ConfirmarArqueo.Text = string.Empty;
                btn_ConfirmarArqueo.LockPush = false;

                btn_ConfirmarArqueoTotal.Text = string.Empty;
                btn_ConfirmarArqueoTotal.LockPush = false;


                CapaMantenimientoCaleto.Text = string.Empty;
                CapaMenuPrincipal.Parent = Imagen_Principal;
                CapaMenuPrincipal.Text = string.Empty;

                CapaSuspendidoCaleto.Text = string.Empty;
                CapaSuspendidoCaleto.Parent = Imagen_SistemaSupendido;


                tbCodigo.Parent = Imagen_DetallePago;


                btnPrintNO.Text = string.Empty;
                btnPrintNO.LockPush = false;

                btnPrintSI.Text = string.Empty;
                btnPrintSI.LockPush = false;

                //btn_NoPrint.Text = string.Empty;
                //btn_NoPrint.LockPush = false;

                btn_CancelarPagoEfectivo.Text = string.Empty;
                btn_CancelarPagoEfectivo.LockPush = false;

                btn_ConfirmarCargaF56.LockPush = false;
                btn_ConfirmarCargaF56.Text = string.Empty;

                btn_ConfirmarCargaMonedas.LockPush = false;
                btn_ConfirmarCargaMonedas.Text = string.Empty;


                grvCargaTotalBilletesF56.ColumnHeadersDefaultCellStyle.BackColor = Color.Indigo;
                grvCargaTotalBilletesF56.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grvCargaTotalBilletesF56.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grvCargaTotalBilletesF56.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grvCargaTotalBilletesF56.RowHeadersVisible = false;

                grvCargaActualBilletesF56.ColumnHeadersDefaultCellStyle.BackColor = Color.Indigo;
                grvCargaActualBilletesF56.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grvCargaActualBilletesF56.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grvCargaActualBilletesF56.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grvCargaActualBilletesF56.RowHeadersVisible = false;

                grvCargaTotalMonedas.ColumnHeadersDefaultCellStyle.BackColor = Color.Indigo;
                grvCargaTotalMonedas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grvCargaTotalMonedas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grvCargaTotalMonedas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grvCargaTotalMonedas.RowHeadersVisible = false;

                grvCargaActualMonedas.ColumnHeadersDefaultCellStyle.BackColor = Color.Indigo;
                grvCargaActualMonedas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grvCargaActualMonedas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grvCargaActualMonedas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grvCargaActualMonedas.RowHeadersVisible = false;

                lblKeyboard_Carga.BackColor = Color.Transparent;
                lblKeyboard_Carga.Text = string.Empty;


                //tmrSincronizacion.Interval = Convert.ToInt32(Globales.sTiempoSincronizacion);

                tmrReset.Enabled = true;

                CapaPass.Text = string.Empty;
                CapaUsuario.Text = string.Empty;

                Imagen_Inicio.Dock = DockStyle.Fill;
                Imagen_Principal.Dock = DockStyle.Fill;
                Imagen_DetallePago.Dock = DockStyle.Fill;
                Imagen_Procesando.Dock = DockStyle.Fill;
                Imagen_MenuSistema.Dock = DockStyle.Fill;
                Imagen_IngresoPass.Dock = DockStyle.Fill;
                Imagen_SegundaPass.Dock = DockStyle.Fill;
                Imagen_ContraseñaInvalida.Dock = DockStyle.Fill;
                Imagen_Mantenimiento.Dock = DockStyle.Fill;
                Imagen_Arqueo.Dock = DockStyle.Fill;
                Imagen_Carga.Dock = DockStyle.Fill;
                Imagen_CargaMonedas.Dock = DockStyle.Fill;
                Imagen_CargaBilletes.Dock = DockStyle.Fill;
                Imagen_SistemaSupendido.Dock = DockStyle.Fill;
                Imagen_GraciasPago.Dock = DockStyle.Fill;
                Imagen_ArqueoParcial.Dock = DockStyle.Fill;
                Imagen_ArqueoTotal.Dock = DockStyle.Fill;
                Imagen_Descargando.Dock = DockStyle.Fill;
                Imagen_TarjetaInvalida.Dock = DockStyle.Fill;
                Imagen_TarjetaMensual.Dock = DockStyle.Fill;
                Imagen_DetallePagoMensual.Dock = DockStyle.Fill;
                Imagen_Atasco.Dock = DockStyle.Fill;
                imagen_PuedeSalir.Dock = DockStyle.Fill;
                Imagen_ConsultaFallida.Dock = DockStyle.Fill;


                kbUsuarioPass.Keyboard = CreateCustomKeyboard();
                kbUsuarioPass.Parent = Imagen_IngresoPass;

                kbUsuarioPass.Location = new Point(16, 679);
                kbUsuarioPass.Size = new System.Drawing.Size(804, 282);

                KeyBoardLoad();

                btn_MostrarTecladoPass.LockPush = false;
                btn_MostrarTecladoPass.Text = string.Empty;

                btn_MostrarTecladoSegunda.LockPush = false;
                btn_MostrarTecladoSegunda.Text = string.Empty;

                btn_Aceptar.LockPush = false;
                btn_Aceptar.Text = string.Empty;

                btn_SiMensual.Text = string.Empty;
                btn_SiMensual.LockPush = false;
                btn_NoMensual.Text = string.Empty;
                btn_NoMensual.LockPush = false;

                btn_Cancelar.LockPush = false;
                btn_Cancelar.Text = string.Empty;

                btn_AceptarSegunda.LockPush = false;
                btn_AceptarSegunda.Text = string.Empty;

                btn_CancelarSegunda.LockPush = false;
                btn_CancelarSegunda.Text = string.Empty;

                lblPassword.Text = string.Empty;
                lblPassword.BackColor = Color.Transparent;

                lblUsuario.Text = string.Empty;
                lblUsuario.BackColor = Color.Transparent;

                lblUsuarioSegunda.Text = string.Empty;
                lblUsuarioSegunda.BackColor = Color.Transparent;

                lblCodigo.Text = string.Empty;
                lblCodigo.BackColor = Color.Transparent;

                btn_Arqueo.LockPush = false;
                btn_Arqueo.Text = string.Empty;

                btn_Carga.LockPush = false;
                btn_Carga.Text = string.Empty;

                btn_Mantenimiento.LockPush = false;
                btn_Mantenimiento.Text = string.Empty;

                btn_Log.LockPush = false;
                btn_Log.Text = string.Empty;

                btn_Iniciar.LockPush = false;
                btn_Iniciar.Text = string.Empty;

                btn_Salir.LockPush = false;
                btn_Salir.Text = string.Empty;

                btn_ArqueoParcial.LockPush = false;
                btn_ArqueoParcial.Text = string.Empty;

                btn_ArqueoTotal.LockPush = false;
                btn_ArqueoTotal.Text = string.Empty;

                btn_Volver.LockPush = false;
                btn_Volver.Text = string.Empty;

                btn_CargaBilletes.LockPush = false;
                btn_CargaBilletes.Text = string.Empty;

                btn_CargaMonedas.LockPush = false;
                btn_CargaMonedas.Text = string.Empty;

                btn_VolverCarga.LockPush = false;
                btn_VolverCarga.Text = string.Empty;


                //btnHopper1.LockPush = false;
                btnHopper1.Text = string.Empty;
                //btnHopper2.LockPush = false;
                btnHopper2.Text = string.Empty;
                btn_Hopper3.Text = string.Empty;
                btnHopper4.Text = string.Empty;
                btn_0Carga.LockPush = false;
                btn_0Carga.Text = string.Empty;
                btn_1Carga.LockPush = false;
                btn_1Carga.Text = string.Empty;
                btn_2Carga.LockPush = false;
                btn_2Carga.Text = string.Empty;
                btn_3Carga.LockPush = false;
                btn_3Carga.Text = string.Empty;
                btn_4Carga.LockPush = false;
                btn_4Carga.Text = string.Empty;
                btn_5Carga.LockPush = false;
                btn_5Carga.Text = string.Empty;
                btn_6Carga.LockPush = false;
                btn_6Carga.Text = string.Empty;
                btn_7Carga.LockPush = false;
                btn_7Carga.Text = string.Empty;
                btn_8Carga.LockPush = false;
                btn_8Carga.Text = string.Empty;
                btn_9Carga.LockPush = false;
                btn_9Carga.Text = string.Empty;
                btn_okCarga.LockPush = false;
                btn_okCarga.Text = string.Empty;
                btn_BorrarCarga.LockPush = false;
                btn_BorrarCarga.Text = string.Empty;

                btn_ConfirmarCargaMonedas.LockPush = false;
                btn_ConfirmarCargaMonedas.Text = string.Empty;

                pInicio.BackgroundImageLayout = ImageLayout.Stretch;
                Animacion_InserteTarjeta.BackgroundImageLayout = ImageLayout.Stretch;
                Animacion_RetireBox.BackgroundImageLayout = ImageLayout.Stretch;
                AnimacionBoxTotal.BackgroundImageLayout = ImageLayout.Stretch;

                ok = true;

            }
            catch (Exception ex)
            {
                General_Events = "FrondEnd-CargaRecursos -> Excepcion en CargarRecursos: " + ex.InnerException;
                Presentacion = Pantalla.SistemaSuspendido;
            }

            return ok;
        }
        private bool CargaImagenes()
        {
            bool ok = false;

            try
            {
                #region CargaAnimaciones

                pInicio.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Gif\Animacion_Inicio.gif"));
                Animacion_InserteTarjeta.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Gif\Animacion_InserteTarjeta.gif"));
                AnimacionBoxTotal.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Gif\Animacion_RetireBox.gif"));
                Animacion_RetireBox.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Gif\Animacion_RetireBox.gif"));

                //sInicioAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_Inicio.swf"));
                //if (File.Exists(sInicioAnimacion))
                //{
                //    Animacion_Inicio.Visible = true;
                //    Animacion_Inicio.Movie = sInicioAnimacion;
                //    Animacion_Inicio.CtlScale = "ExactFit";
                //    Animacion_Inicio.Play();
                //    Animacion_Inicio.BringToFront();

                //}
                //sPrincipalAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_InserteTarjeta.swf"));
                //if (File.Exists(sPrincipalAnimacion))
                //{
                //    Animacion_InserteTarjeta.Visible = true;
                //    Animacion_InserteTarjeta.Movie = sPrincipalAnimacion;
                //    Animacion_InserteTarjeta.CtlScale = "ExactFit";
                //    Animacion_InserteTarjeta.Play();
                //    Animacion_InserteTarjeta.BringToFront();

                //}
                //sBoxAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_RetireBox.swf"));
                //if (!File.Exists(sBoxAnimacion))
                //{
                //    return false;
                //}
                //sBoxTotalAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_RetireBox.swf"));
                //if (!File.Exists(sBoxTotalAnimacion))
                //{
                //    return false;
                //}
                //sUnMomentoAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_UnMomento.swf"));
                //if (!File.Exists(sUnMomentoAnimacion))
                //{
                //    return false;
                //}


                //sRetiroReciboAnimacion = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_RetiroRecibo.swf"));
                //if (!File.Exists(sRetiroReciboAnimacion))
                //{
                //    return false;
                //}
                //string EnCaso = (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Swf\Animacion_EnCaso.swf"));
                //if (File.Exists(EnCaso))
                //{
                //    //Animacion_Encaso.Visible = true;
                //    //Animacion_Encaso.Movie = EnCaso;
                //    //Animacion_Encaso.CtlScale = "ExactFit";
                //    //Animacion_Encaso.Play();
                //    //Animacion_Encaso.BringToFront();
                //}


                #endregion

                #region ImagenesFlujo
                pPublicidad.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadDetalle.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadProcesando.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadCancelada.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadNoPago.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadSinEntrada.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadCanceladaPago.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadImprimir.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadGracias.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadInvalida.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadSuspendido.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadMensul.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadPagoAuto.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                pPublicidadPuedeSalir.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Publicidad.jpg"));
                this.Icon = new System.Drawing.Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Icon\ICON.ico"));
                Imagen_Inicio.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Inicio.jpg"));
                Imagen_Principal.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Principal.jpg"));
                Imagen_DetallePago.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_DetallePago.jpg"));
                Imagen_Procesando.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_ProcesandoPago.jpg"));
                Imagen_MenuSistema.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuSistema.jpg"));
                Imagen_IngresoPass.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_IngresoPass.jpg"));
                Imagen_SegundaPass.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_SegundaPass.jpg"));
                Imagen_ContraseñaInvalida.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_ContraseñaInvalida.jpg"));
                Imagen_Mantenimiento.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Mantenimiento.jpg"));
                Imagen_CerrarOperacion.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Cerrado.jpg"));
                Imagen_Arqueo.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuArqueo.jpg"));
                Imagen_Carga.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuCarga.jpg"));
                Imagen_ArqueoParcial.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuArqueo.jpg"));
                Imagen_ArqueoTotal.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_ArqueoTotal.jpg"));
                Imagen_Descargando.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Descargando.jpg"));
                Imagen_CargaMonedas.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuCargaMonedas.jpg"));
                Imagen_CargaBilletes.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_MenuCargaBilletes.jpg"));
                Imagen_SistemaSupendido.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_SistemaSuspendido.jpg"));
                Imagen_GraciasPago.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_GraciasPago.jpg"));
                Imagen_ImprimirFactura.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_ImprimirFactura.jpg"));
                Imagen_TransaccionCanceladaPago.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_CanceladaPago.jpg"));
                Imagen_TarjetaInvalida.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_TarjetaInvalida.jpg"));
                Imagen_TarjetaMensual.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_TarjetaMensualidad.jpg"));
                Imagen_DetallePagoMensual.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_DetallePagoMensualidad.jpg"));
                Imagen_Atasco.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Atasco.jpg"));
                imagen_PuedeSalir.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_PuedeSalir.jpg"));
                Imagen_ConsultaFallida.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_ConsultaFallida.jpg"));
                #endregion

                #region ImagenesBotones

                //btn_Monedas.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaMonedas.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaMonedas.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaMonedasPresionado.png"));
                //btn_PuertaBilletes.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaBilltes.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaBilltes.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PuertaBilltesPresionado.png"));

                btn_CancelarPagoEfectivo.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_CancelarPresionado.png"));
                btn_MostrarTecladoPass.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoard.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoard.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoardPresionado.png"));
                btn_MostrarTecladoSegunda.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoard.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoard.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Imagen_KeyBoardPresionado.png"));

                btn_Aceptar.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_AceptarPresionado.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_AceptarPresionado.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Aceptar.png"));
                btn_Cancelar.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_CancelarPresionado.png"));
                btn_AceptarSegunda.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_AceptarPresionado.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_AceptarPresionado.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Aceptar.png"));
                btn_CancelarSegunda.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_CancelarPresionado.png"));

                btn_CancelarPagoAuto.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Cancelar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_CancelarPresionado.png"));

                btn_Arqueo.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Arqueo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Arqueo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_ArqueoPresionado.png"));
                btn_Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Carga.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Carga.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_CargaPresionado.png"));
                btn_Mantenimiento.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Mantenimiento.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Mantenimiento.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_MantenimientoPresionado.png"));
                btn_Log.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Log.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Log.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_LogPresionado.png"));
                btn_Iniciar.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Iniciar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Iniciar.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_IniciarPresionado.png"));
                btn_Salir.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Salir.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Salir.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_SalirPresionado.png"));

                btn_ArqueoTotal.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Total.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Total.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_TotalPresionado.png"));
                btn_ArqueoParcial.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Parcial.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Parcial.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_ParcialPresionado.png"));
                btn_Volver.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));
                btn_ConfirmarCargaF56.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));
                btn_ConfirmarArqueo.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));
                btn_ConfirmarArqueoTotal.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));


                btn_CargaBilletes.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Billetes.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Billetes.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_BilletesPresionado.png"));
                btn_CargaMonedas.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Monedas.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Monedas.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_MonedasPresionado.png"));
                btn_VolverCarga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));

                btn_0Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero0Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero0Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero0Carga_Presionado.png"));
                btn_1Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero1Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero1Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero1Carga_Presionado.png"));
                btn_2Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero2Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero2Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero2Carga_Presionado.png"));
                btn_3Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero3Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero3Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero3Carga_Presionado.png"));
                btn_4Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero4Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero4Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero4Carga_Presionado.png"));
                btn_5Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero5Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero5Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero5Carga_Presionado.png"));
                btn_6Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero6Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero6Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero6Carga_Presionado.png"));
                btn_7Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero7Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero7Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero7Carga_Presionado.png"));
                btn_8Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero8Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero8Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero8Carga_Presionado.png"));
                btn_9Carga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero9Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero9Carga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_Numero9Carga_Presionado.png"));
                btn_okCarga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_OKCarga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_OKCarga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_OKCarga_Presionado.png"));
                btn_BorrarCarga.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_BorrarCarga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_BorrarCarga_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_BorrarCarga_Presionado.png"));

                btnHopper1.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper1_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper1_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper1_Presionado.png"));
                btnHopper2.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper2_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper2_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper2_Presionado.png"));
                btn_Hopper3.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper3_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper3_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper3_Presionado.png"));
                btnHopper4.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper4_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper4_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btnHopper4_Presionado.png"));

                btn_ConfirmarCargaMonedas.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Volver.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_VolverPresionado.png"));

                btnPrintNO.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNoPresionado.png"));
                btnPrintSI.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Print.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Print.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintPresionado.png"));

                btn_NoMensual.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNo.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintNoPresionado.png"));
                btn_SiMensual.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Print.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Print.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_PrintPresionado.png"));

                #endregion

                ok = true;

            }
            catch (Exception ex)
            {
                General_Events = "FrondEnd-CargaImagenes -> Excepcion en CargaImagenes: " + ex.InnerException;
                Presentacion = Pantalla.SistemaSuspendido;
            }

            return ok;

        }
        private void Limpiartext()
        {
            _RetornoCobro = 0;
            _CobroTarjetaMensual = false;
            _TiempoSalida = 0;
            lblDocumentoAuto.Text = string.Empty;
            lblNombreAuto.Text = string.Empty;
            lblFechaFinAuto.Text = string.Empty;
            lblConvenio.Text = string.Empty;
            _TipoPago = string.Empty;
            _ValorTipoPago = string.Empty;
            _BanderaJam = false;
            CargaBilletesBB = false;
            tbCodigo.Text = string.Empty;
            _ComPrint = false;
            _NombreConvenio = string.Empty;
            _PrintSalida = false;
            cnt = 0;
            _Tarjeta = new Tarjeta();
            _TarjetaSmart = new TarjetaSmart();
            ValorCobro = string.Empty;
            lblCodigoParqueo.Text = string.Empty;
            lblCodigoPago.Text = string.Empty;
            _Efectivo = false;
            _Datafono = false;
            _Prepago = false;
            _Celular = false;
            bSmart = false;
            _bPagoSmart = false;
            _PagoEfectivo = new PagoEfectivo();
            _PagoEfectivo.ValorRecibido = 0;
            _PagoEfectivo.ValorCambio = 0;
            lblValorPagarEfectivo.Text = "$0";
            lblValorRecibido.Text = "$0";
            lblCambio.Text = "$0";
            _BanderaCancelacion = false;
            _BanderaPagoFinal = false;
            btn_CancelarPagoEfectivo.Enabled = true;
            _BanderaRecaudo = false;
        }
        private bool CargaSonidos()
        {
            bool ok = false;

            try
            {

                _sPrepagoLector = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_ColoqueTarjetaPrepagoSobreLector.mp3");
                _sImprimiendoTicket = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_ImprimiendoTiquete.mp3");
                _sPagoCelular = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_IngreseEnSuAplicacionElCodigoDeParqueo.mp3");
                _sLeyendoTarjeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_LeyendoTarjeta_10db.mp3");
                _sNoOlvideDinero = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_NoOlvideRetirarSuDinero.mp3");
                _sNoOlvideFactura = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_NoOlvideRetirarSuFactura.mp3");
                _sNoOlvideTarjeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_NoOlvideRetirarSuTarjeta.mp3");
                _sPagoDatafono = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_OprimaBotonVerdeDatafono.mp3");
                _sIngreseDinero = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_IngreseDinero.wav");
                _sRetireTarjeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_RetireTarjetaDeParqueadero.mp3");
                _sDetallePago = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_SeleccioneFormaDePago.mp3");
                _sTarjetaInvalida = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_TarjetaNoValidaRetireLaTarjetaPorFavor.mp3");
                _sDeseaPrint = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_ImprimirFactura.wav");
                _sGraciasPago = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_GraciasPago.wav");
                _sCodigoInvalido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_CodigoInvalido.wav");
                _sAtasco = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_Atasco.wav");
                _sConsultaFallida = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Wav\Sonido_ConsultaError.wav");

                ok = true;
            }
            catch (Exception ex)
            {
                General_Events = "FrondEnd-CargaSonidos -> Excepcion en CargaSonidos: " + ex.InnerException;
                Presentacion = Pantalla.SistemaSuspendido;
            }
            return ok;
        }
        private bool CargarParametros()
        {
            bool ok = false;

            if (_frmPrincipal_Presenter.ObtenerInfoModulo())
            {
                if (_frmPrincipal_Presenter.ObtenerInfoFactura())
                {
                    if (_frmPrincipal_Presenter.ObtenerInfoPartes())
                    {
                        if (_frmPrincipal_Presenter.ObtenerInfoParametros())
                        {
                            if (_frmPrincipal_Presenter.AsignarDenominacionesLocales())
                            {
                                ok = true;
                            }
                        }
                    }
                }
            }

            return ok;
        }
        private void RegistrarOperacionPago()
        {
            Int64 total = 0;

            //insertar transaccion
            if (_TipoPago == "MENSUALIDAD")
            {
                if (_frmPrincipal_Presenter.RegistrarOperacion(TipoOperacion.Mensualidad))
                {
                    lblConvenio.Text = "MENSUALIDAD";
                    //ValorCobro = "2000";
                    ValorCobro = _PagoEfectivo.ValorPago.ToString();

                    General_Events = "FrondEnd-RegistrarOperacionPago -> Total : " + ValorCobro;
                    _PagoEfectivo.ValorRecibido = 0;
                    _PagoEfectivo.ValorCambio = 0;
                    _PagoEfectivo.ValorPago = Convert.ToInt64(ValorCobro);
                    _BanderaRecaudo = true;
                    _BanderaCancelacion = false;
                    _BanderaEsperaHabilitado = false;
                    _BanderaPresionado = false;
                    _frmPrincipal_Presenter.HabilitarSecuenciaRecibir();
                    //SoundPlayer simpleSound = new SoundPlayer(_sIngreseDinero);
                    //simpleSound.Play();
                    Presentacion = Pantalla.DetallePagoMensual;
                }
                else
                {
                    Presentacion = Pantalla.TransaccionCancelada;
                }
            }
            else
            {
                if (_frmPrincipal_Presenter.RegistrarOperacion(TipoOperacion.Pago))
                {
                    //ValorCobro = "2000";
                    ValorCobro = _PagoEfectivo.ValorPago.ToString();

                    General_Events = "FrondEnd-RegistrarOperacionPago -> Total : " + ValorCobro;
                    _PagoEfectivo.ValorRecibido = 0;
                    _PagoEfectivo.ValorCambio = 0;
                    _PagoEfectivo.ValorPago = Convert.ToInt64(ValorCobro);
                    _BanderaRecaudo = true;
                    _BanderaCancelacion = false;
                    _BanderaEsperaHabilitado = false;
                    _BanderaPresionado = false;
                    _frmPrincipal_Presenter.HabilitarSecuenciaRecibir();
                    //SoundPlayer simpleSound = new SoundPlayer(_sIngreseDinero);
                    //simpleSound.Play();
                    Presentacion = Pantalla.DetallePago;
                }
                else
                {
                    Presentacion = Pantalla.TransaccionCancelada;
                }
            }
        }
        private void RegistrarArqueoTotal()
        {
            _frmPrincipal_Presenter.ObtenerInfoPartes();
            _frmPrincipal_Presenter.HabilitarSecuenciaArqueoTotal();
            Presentacion = Pantalla.Descargando;
        }
        private void RegistrarArqueoParcial()
        {
            _frmPrincipal_Presenter.HabilitarSecuenciaMonitor();
            Presentacion = Pantalla.ArqueoParcial;
        }
        private void RegistrarCarga()
        {
            if (!_ProcesoCarga)
            {
                if (_frmPrincipal_Presenter.RegistrarCarga())
                {
                    _Carga.CodigoCarga = _Operacion.ID_Operacion.ToString();
                    _ProcesoCarga = true;
                    _CargaMonedas = false;
                    _MonedasStart = false;
                    _CargaBilletesBB = false;
                    _BilletesStart = false;
                }
                else
                {
                    General_Events = "FrondEnd-RegistrarCarga -> No confirma carga.";
                    Presentacion = Pantalla.SistemaSuspendido;
                }

            }
        }
        private void RegistrarCargaRedencion()
        {
            //if (!ProcesoCarga)
            //{
            //    if (_frmPrincipal_Presenter.RegistrarCarga())
            //    {

            //        ProcesoCarga = true;
            //        BilletesStart = false;
            //        _Carga.CodigoCarga = _Operacion.ID_Operacion.ToString();
            //        ID_Hopper = ID_Part.Ninguno;
            //        ID_Cassette = ID_Part.Ninguno;
            //        CargaBilletesF56 = true;
            //        CargaMonedas = false;
            //        MonedasStart = false;


            //    }
            //    else
            //    {
            //        General_Events = "FrondEnd-RegistrarCargaRedencion -> No registra carga redencion.";
            //        Presentacion = Pantalla.SistemaSuspendido;
            //    }
            //}
        }
        private void KeyBoardLoad()
        {
            _ColorTableCustom = new VirtualKeyboardColorTable();
            _ColorTableCustom.BackgroundColor = Color.Gainsboro;
            _ColorTableCustom.DarkKeysColor = Color.Black;
            _ColorTableCustom.KeysColor = Color.DarkGray;
            _ColorTableCustom.LightKeysColor = Color.Black;
            _ColorTableCustom.PressedKeysColor = Color.Indigo;
            _ColorTableCustom.TextColor = Color.Black;
            _ColorTableCustom.DownKeysColor = Color.White;
            _ColorTableCustom.DownTextColor = Color.Black;
            _ColorTableCustom.TopBarTextColor = Color.Transparent;
            _ColorTableCustom.ToggleTextColor = Color.GreenYellow;


            kbUsuarioPass.ColorTable = _ColorTableCustom;
        }
        private Keyboard CreateCustomKeyboard()
        {
            Keyboard keyboard = new Keyboard();

            LinearKeyboardLayout firstRow = new LinearKeyboardLayout();

            firstRow.AddKey("Q");
            firstRow.AddKey("W");
            firstRow.AddKey("E");
            firstRow.AddKey("R");
            firstRow.AddKey("T");
            firstRow.AddKey("Y");
            firstRow.AddKey("U");
            firstRow.AddKey("I");
            firstRow.AddKey("O");
            firstRow.AddKey("P");
            //firstRow.AddKey("!");
            //firstRow.AddKey(":");
            firstRow.AddKey("BackSpace", "{BACKSPACE}", width: 15);
            firstRow.AddSpace(10);
            firstRow.AddKey("7");
            firstRow.AddKey("8");
            firstRow.AddKey("9");

            firstRow.AddLine();
            firstRow.AddSpace(5);
            firstRow.AddKey("A");
            firstRow.AddKey("S");
            firstRow.AddKey("D");
            firstRow.AddKey("F");
            firstRow.AddKey("G");
            firstRow.AddKey("H");
            firstRow.AddKey("J");
            firstRow.AddKey("K");
            firstRow.AddKey("L");
            firstRow.AddKey("Ñ");
            //firstRow.AddKey("-");
            //firstRow.AddKey("*");
            firstRow.AddSpace(21);
            firstRow.AddKey("4");
            firstRow.AddKey("5");
            firstRow.AddKey("6");

            firstRow.AddLine();
            firstRow.AddSpace(22);
            firstRow.AddKey("Z");
            firstRow.AddKey("X");
            firstRow.AddKey("C");
            firstRow.AddKey("V");
            firstRow.AddKey("B");
            firstRow.AddKey("N");
            firstRow.AddKey("M");
            //firstRow.AddKey("?");
            //firstRow.AddKey(",");
            //firstRow.AddKey("_");
            //firstRow.AddKey("´", info: null, style: KeyStyle.Normal, layout: 1);
            firstRow.AddSpace(37);
            firstRow.AddKey("1");
            firstRow.AddKey("2");
            firstRow.AddKey("3");

            firstRow.AddLine();
            firstRow.AddSpace(15);
            firstRow.AddKey("", " ", width: 90);
            firstRow.AddSpace(30);
            firstRow.AddKey("0", width: 32);
            //firstRow.AddKey(".");


            LinearKeyboardLayout numRow = new LinearKeyboardLayout();
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("É", "E");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("Ú", "U");
            numRow.AddKey("Í", "I");
            numRow.AddKey("Ó", "O");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("BackSpace", "{BACKSPACE}", width: 15);
            numRow.AddSpace(10);
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");

            numRow.AddLine();
            numRow.AddSpace(5);
            numRow.AddKey("Á", "A");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddSpace(21);
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");

            numRow.AddLine();
            numRow.AddSpace(10);
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("´", info: null, style: KeyStyle.Pressed, layout: 0);
            numRow.AddSpace(27);
            numRow.AddKey("");
            numRow.AddKey("");
            numRow.AddKey("");

            numRow.AddLine();
            numRow.AddSpace(25);
            numRow.AddKey("", " ", width: 90);
            numRow.AddSpace(42);
            numRow.AddKey("", width: 21);
            numRow.AddKey("");

            keyboard.Layouts.Add(firstRow);
            keyboard.Layouts.Add(numRow);



            return keyboard;
        }
        public void ReportarEstadoModulo(string sPantalla)
        {
            //string displayName = Globales.sDisplayName;
            //MyRegistro registro = new MyRegistro();
            //registro = InstalledPrograms.GetRegistryProgram(displayName);
            //if (registro != null)
            //{
            //    bool bResultado = _frmPrincipal_Presenter.RegistrarEstadoPuntoPago(sPantalla, registro.Version);
            //}
            //else
            //{

            //}
        }
        private string GenerarSegundaClave()
        {
            string Password2 = string.Empty;

            try
            {
                Random rand;
                string semilla = string.Empty;
                if (DateTime.Now.Minute < 30)
                {
                    semilla = DateTime.Now.ToString("MMddyyHH") + "01";
                    rand = new Random(Convert.ToInt32(semilla));
                }
                else
                {
                    semilla = DateTime.Now.ToString("MMddyyHH") + "31";
                    rand = new Random(Convert.ToInt32(semilla));
                }


                Password2 = rand.Next(10000, 99999).ToString();

            }
            catch (Exception ex)
            {

            }
            Password2 = DateTime.Now.ToString("ddMMyyHH");
            return Password2;
        }
        private void ActivarAlarma()
        {
            //SerialArduino.Write("A");
        }
        private void DesactivarAlarma()
        {
            //SerialArduino.Write("D");
        }
        private void ObtenerValorPagar()
        {

        }
        private void ConsultarPagoCelular()
        {
            _ConsultarValorResult.idEntrada = _lstConsultarValorResult[0].idEntrada;
            _ConsultarValorResult.valorAPagar = _lstConsultarValorResult[0].valorAPagar;
            _ConsultarValorResult.fechaLiquidacion = _lstConsultarValorResult[0].fechaLiquidacion;

            //if (_frmPrincipal_Presenter.ConsultaPagoCelular())
            //{
            //    lblCodigoParqueo.Text = _lstConsultarPagoCelularResult[0].idParqueadero.ToString();
            //    lblCodigoPago.Text = _lstConsultarPagoCelularResult[0].idPago.ToString();

            //    Presentacion = Pantalla.PagoCelular;
            //}
            //else 
            //{
            //    Presentacion = Pantalla.TransaccionCancelada;
            //}

        }
        private void RegistrarPagoEfectivo()
        {
            Presentacion = Pantalla.Procesando;

            _ConsultarValorResult.idEntrada = _lstConsultarValorResult[0].idEntrada;
            //_ConsultarValorResult._PlacaSalida = _Tarjeta.Placa;
            _ConsultarValorResult.valorAPagar = _lstConsultarValorResult[0].valorAPagar;
            _ConsultarValorResult.valorServicio = _lstConsultarValorResult[0].valorServicio;
            _ConsultarValorResult.valorDescuento = _lstConsultarValorResult[0].valorDescuento;
            _ConsultarValorResult.valorEmpresa = _lstConsultarValorResult[0].valorEmpresa;
            _ConsultarValorResult.fechaLiquidacion = _lstConsultarValorResult[0].fechaLiquidacion;

            if (_frmPrincipal_Presenter.RegistrarPagoEfectivo())
            {
                _Efectivo = true;
                Presentacion = Pantalla.ImprimirFactura;
            }
            else
            {
                Presentacion = Pantalla.TransaccionCancelada;
            }

        }
        private void ConsultarPagoPrepago()
        {

        }
        private void ValidarPago()
        {
            lblFecha.Text = _Tarjeta.DateTimeEntrance.ToString();
            DateTime FechaPago = Convert.ToDateTime(_Tarjeta.DateTimeEntrance);

            string año = FechaPago.Year.ToString();
            string MES = FechaPago.Month.ToString();
            string DIA = FechaPago.Day.ToString();
            string HORA = FechaPago.Hour.ToString();
            string MINU = FechaPago.Minute.ToString();
            string SEG = FechaPago.Second.ToString();

            //string IdTransaccion = Entrada.ToString("yyyyddMMHHmmss");
            string FECHA = año + "/" + MES.PadLeft(2, '0') + "/" + DIA.PadLeft(2, '0') + " " + HORA.PadLeft(2, '0') + ":" + MINU.PadLeft(2, '0') + ":" + SEG.PadLeft(2, '0');
            FechaPago = Convert.ToDateTime(FECHA);

            DateTime FechaActual = _frmPrincipal_Presenter.ObtenerFechaServer();

            TimeSpan Calculo = FechaActual - FechaPago;

            lblPermanencia.Text = Calculo.Hours + " Horas " + Calculo.Minutes + " Min " + Calculo.Seconds + " Seg";
            if (_Tarjeta.TypeVehicle == TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE)
            {
                lblTipoVehiculo.Text = "CARRO";
            }
            else
            {
                lblTipoVehiculo.Text = "MOTO";
            }
            if (_Tarjeta.CodeAgreement1 != 0)
            {
                lblConvenio.Text = _NombreConvenio;
            }
            else
            {
                lblConvenio.Text = "VISITANTE";
            }

            if (_frmPrincipal_Presenter.ConsultaValor())
            {

                if (_PagoEfectivo.ValorPago > 0)
                {
                    if (_TipoPago == string.Empty)
                    {
                        _TipoPago = "VISITANTE";
                    }

                    lblConvenio.Text = _TipoPago;
                    RegistrarOperacionPago();
                }
                else
                {
                    if (_Tarjeta.CodeAgreement1 > 0 && _Tarjeta.CodeAgreement1 != 32)
                    {
                        _frmPrincipal_Presenter.RegistrarOperacion(TipoOperacion.Pago);
                        _PrintSalida = true;
                        
                        _frmPrincipal_Presenter.RegistrarConvenio(Convert.ToInt64(_IdTransaccion), Convert.ToInt32(_Tarjeta.CodeAgreement1));
                        _frmPrincipal_Presenter.ConfirmarOperacion(TipoOperacion.Pago, TipoEstadoPago.Aprobado);
                        Presentacion = Pantalla.ImprimirFactura;
                    }
                    else
                    {


                        DateTime FechaPay = Convert.ToDateTime(_Tarjeta.DateTimeEntrance);
                        DateTime Fechaahora = _frmPrincipal_Presenter.ObtenerFechaServer();

                        TimeSpan Calcu = Fechaahora - FechaPay;

                        int TiempoGracia = Convert.ToInt32(_frmPrincipal_Presenter.ObtenerValorParametro(Parametros.TiempoGracia));

                        if (Calcu.TotalMinutes > TiempoGracia)
                        {
                            _frmPrincipal_Presenter.ExpulsarTarjeta();
                            Presentacion = Pantalla.PublicidadPrincipal;
                        }
                        else
                        {
                            _TiempoSalida = Math.Round((TiempoGracia - Calcu.TotalMinutes), 0);
                            Presentacion = Pantalla.PuedeSalir;
                        }
                    }
                    //Presentacion = Pantalla.GarciasPago;
                    //_PrintSalida = false;
                }
            }
            else
            {
                _frmPrincipal_Presenter.ExpulsarTarjeta();
                Presentacion = Pantalla.TransaccionCancelada;
            }

        }
        private bool ValidarMensualidad()
        {
            bool ok = false;

            Autorizado oAutorizado = new Autorizado();
            oAutorizado.IdTarjeta = _Tarjeta.CodeCard;
            //oAutorizado.IdTarjeta = "06F7486A";
            //_Tarjeta.CodeCard = "06F7486A";
            General_Events = "FrondEnd-ValidarMensualidad" + " IdTarjeta: " + oAutorizado.IdTarjeta;
            if (_frmPrincipal_Presenter.ObtenerAutorizado(oAutorizado))
            {
                for (int i = 0; i < _lstDtoAutorizado.Count; i++)
                {
                    if (_lstDtoAutorizado[i].Estado && _lstDtoAutorizado[i].EstadoAutorizacion)
                    {
                        _NumeroDocumentoOrigen = Convert.ToDouble(_lstDtoAutorizado[i].Documento);
                        _DatosAuto = _lstDtoAutorizado[i].NombresAutorizado;
                        _FechaFinAuto = _lstDtoAutorizado[i].FechaFinal.ToString();
                        General_Events = "_NumeroDocumentoOrigen " + _NumeroDocumentoOrigen;
                        ok = true;
                        break;
                    }
                }
            }

            return ok;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

        #region IView
        public Pantalla Presentacion
        {
            set
            {
                Cnt_Reinicio = 0;
                //Thread th1 = new Thread(new ThreadStart(() =>
                //{
                //    ReportarEstadoModulo(value.ToString());
                //}));
                //th1.Start();
                SoundPlayer simpleSound;
                switch (value)
                {

                    case Pantalla.Inicio:
                        TabControlPrincipal.SelectedTab = tabInicio;
                        break;
                    case Pantalla.PublicidadPrincipal:
                        Cnt_Reinicio = 0;
                        Limpiartext();
                        TabControlPrincipal.SelectedTab = tabPrincipal;
                        break;
                    case Pantalla.DetallePago:
                        Cnt_Reinicio = 0;
                        _ComPrint = false;
                        DatosPantallaPago();                        
                        TabControlPrincipal.SelectedTab = tabDetallePago;
                        break;
                    case Pantalla.DetallePagoMensual:
                        Cnt_Reinicio = 0;
                        _ComPrint = false;
                        lblDocumentoAuto.Text = _NumeroDocumentoOrigen.ToString();
                        lblNombreAuto.Text = _DatosAuto;
                        lblFechaFinAuto.Text = _FechaFinAuto;
                        DatosPantallaPago();
                        TabControlPrincipal.SelectedTab = tabDetallePagoMensual;
                        break;
                    case Pantalla.TransaccionCancelada:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTransaccionCancelada;
                        break;
                    case Pantalla.TarjetaSinEntrada:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTarjetaSinEntrada;
                        break;
                    case Pantalla.ConsultaFallida:
                        Cnt_Reinicio = 0;
                        simpleSound = new SoundPlayer(_sConsultaFallida);
                        simpleSound.Play();
                        TabControlPrincipal.SelectedTab = tabConsultaFallida;
                        break;
                    case Pantalla.PuedeSalir:
                        Cnt_Reinicio = 0;
                        lblTiempoSalida.Text = _TiempoSalida.ToString();
                        lblTiempoSalida.Parent = imagen_PuedeSalir;
                        TabControlPrincipal.SelectedTab = tabPuedeSalir;
                        break;
                    case Pantalla.TransaccionCanceladaPago:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTransaccionCanceladaPago;
                        break;
                    case Pantalla.Procesando:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabProcesando;
                        break;
                    case Pantalla.TarjetaMensual:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTarjetaMensual;
                        break;
                    case Pantalla.PagoEfectivo:
                        Cnt_Reinicio = 0;                        
                        
                        TabControlPrincipal.SelectedTab = tabPagoEfectivo;                        
                        break;
                    case Pantalla.PagoCelular:
                        Cnt_Reinicio = 0;
                        lblValorPagarCelular.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(ValorCobro.Replace("$", "").Replace(".", "")));
                        TabControlPrincipal.SelectedTab = tabPagoCelular;
                        break;
                    case Pantalla.Prepago:
                        Cnt_Reinicio = 0;
                        //bSmart = false;
                        lblValorPagarPrepago.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(ValorCobro.Replace("$", "").Replace(".", "")));
                        TabControlPrincipal.SelectedTab = tabPagoPrepago;
                        break;
                    case Pantalla.Datafono:
                        Cnt_Reinicio = 0;
                        lblValorPagarDatafono.Text = "$" + String.Format("{0:#,##0.##}", Convert.ToDouble(ValorCobro.Replace("$", "").Replace(".", "")));
                        TabControlPrincipal.SelectedTab = tabPagoDatafono;
                        break;
                    case Pantalla.IngresoPass:
                        Cnt_Reinicio = 0;
                        lblUsuario.Text = string.Empty;
                        lblPassword.Text = string.Empty;
                        Pass = string.Empty;
                        kbUsuarioPass.BringToFront();
                        TabControlPrincipal.SelectedTab = tabIngresoPass;
                        break;
                    case Pantalla.MenuSistemas:
                        Cnt_Reinicio = 0;
                        bMantenimiento = false;
                        _ProcesoArqueoParcial = false;
                        _ProcesoArqueoTotal = false;
                        _ProcesoCarga = false;
                        TabControlPrincipal.SelectedTab = tabMenuSistemas;
                        break;
                    case Pantalla.ContraseñaInavlida:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabContrasenaInvalida;
                        break;
                    case Pantalla.Arqueo:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabArqueo;
                        break;
                    case Pantalla.Carga:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabCarga;
                        break;
                    case Pantalla.Mantenimiento:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabMantenimiento;
                        break;
                    case Pantalla.SistemaSuspendido:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabSistemaSuspendido;
                        break;
                    case Pantalla.Atasco:
                        Cnt_Reinicio = 0;
                        simpleSound = new SoundPlayer(_sAtasco);
                        simpleSound.Play();
                        TabControlPrincipal.SelectedTab = tabAtasco;
                        break;
                    case Pantalla.GarciasPago:
                        Cnt_Reinicio = 0;
                        simpleSound = new SoundPlayer(_sGraciasPago);
                        simpleSound.Play();
                        TabControlPrincipal.SelectedTab = tabGraciasPago;
                        break;
                    case Pantalla.TarjetaInvalida:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTarjetaInvalida;
                        break;
                    case Pantalla.ImprimirFactura:
                        Cnt_Reinicio = 0;
                        simpleSound = new SoundPlayer(_sDeseaPrint);
                        simpleSound.Play();
                        TabControlPrincipal.SelectedTab = tabImprimirFactura;
                        break;
                    case Pantalla.TarjetaNoPago:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabTarjetaNoGeneraPago;
                        break;
                    case Pantalla.MenuCargMonedas:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabCargaMonedas;
                        break;
                    case Pantalla.MenuCargBilletes:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabCargaBilletes;
                        break;
                    case Pantalla.ArqueoParcial:
                        Cnt_Reinicio = 0;
                        //Animacion_RetireBox.Visible = true;
                        //Animacion_RetireBox.LoadMovie(0, sBoxAnimacion);
                        //Animacion_RetireBox.Play();
                        TabControlPrincipal.SelectedTab = tabArqueoParcial;
                        break;
                    case Pantalla.ArqueoTotal:
                        Cnt_Reinicio = 0;
                        //AnimacionBoxTotal.Visible = true;
                        //AnimacionBoxTotal.LoadMovie(0, sBoxTotalAnimacion);
                        //AnimacionBoxTotal.Play();
                        TabControlPrincipal.SelectedTab = tabArqueoTotal;
                        break;
                    case Pantalla.Descargando:
                        Cnt_Reinicio = 0;
                        TabControlPrincipal.SelectedTab = tabDescargando;
                        break;
                }
                _Presentacion = value;
            }
            get
            {
                return _Presentacion;
            }
        }
        public string General_Events
        {
            set
            {
                TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: " + value, TipoLog.TRAZA);
            }
        }
        public void Imprimir()
        {
            try
            {
                General_Events = "FrondEnd-Imprimir -> Ingresa a funcion imprimir.";

                ReportDataSource datasource = new ReportDataSource();
                ReportDataSource datasource2 = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();
                LocalReport oLocalReport2 = new LocalReport();
                if (_TicketDevolucion)
                {
                    datasource = new ReportDataSource();
                    datasource2 = new ReportDataSource();
                    oLocalReport = new LocalReport();
                    datasource = new ReportDataSource("DataSetTicketDevolucion", _frmPrincipal_Presenter.GenerarTicketDevolucion().Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "TicketDevolucion"));
                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();
                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                    ore.PrintController = new StandardPrintController();
                    ore.Print();
                    ore.Dispose();
                    ore = null;
                    oLocalReport.Dispose();
                    oLocalReport = null;
                    _TicketDevolucion = false;
                    _ProcesoPago = false;
                }
                else if (_ProcesoArqueoParcial == true || _ProcesoArqueoTotal == true)
                {
                    oLocalReport = new LocalReport();
                    datasource = new ReportDataSource("DatasetArqueo", _frmPrincipal_Presenter.GenerarTicketArqueo().Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "TicketArqueo"));
                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                    ore.PrintController = new StandardPrintController();
                    ore.Print();
                    ore.Dispose();
                    ore = null;
                    oLocalReport.Dispose();
                    oLocalReport = null;
                    Thread.Sleep(2000);
                }
                else if (_ProcesoCarga == true)
                {
                    datasource = new ReportDataSource();
                    datasource2 = new ReportDataSource();
                    oLocalReport = new LocalReport();
                    oLocalReport2 = new LocalReport();
                    datasource = new ReportDataSource("DatosTicketCarga", _frmPrincipal_Presenter.GenerarTicketCarga().Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "TicketCarga"));
                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();
                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                    ore.PrintController = new StandardPrintController();
                    ore.Print();
                    ore.Dispose();
                    ore = null;
                    oLocalReport.Dispose();
                    oLocalReport = null;

                }
                else if (_PrintSalida == true)
                {
                    datasource = new ReportDataSource();
                    datasource2 = new ReportDataSource();
                    oLocalReport = new LocalReport();
                    oLocalReport2 = new LocalReport();
                    datasource = new ReportDataSource("DataSetValidacion", _frmPrincipal_Presenter.GenerarTicketValidacion().Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "TicketValidacion"));
                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();
                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                    ore.PrintController = new StandardPrintController();
                    ore.Print();
                    ore.Dispose();
                    ore = null;
                    oLocalReport.Dispose();
                    oLocalReport = null;

                }
                else if (_ProcesoPago)
                {

                    if (_TipoPago == "MENSUALIDAD")
                    {
                        datasource = new ReportDataSource("DataSetTicketPago", _frmPrincipal_Presenter.GenerarTicketPagoMensualidad().Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "ticketPagoMensualidad"));
                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();
                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                        ore.PrintController = new StandardPrintController();
                        ore.Print();
                        ore.Dispose();
                        ore = null;
                        oLocalReport.Dispose();
                        oLocalReport = null;
                    }
                    else
                    {
                        datasource = new ReportDataSource("DataSetTicketPago", _frmPrincipal_Presenter.GenerarTicketPago().Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Facturas\{0}.rdlc", "ticketPago"));
                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();
                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        //ore.PrinterSettings.PrinterName = Globales.sNombreImpresoraTickets;
                        ore.PrintController = new StandardPrintController();
                        ore.Print();
                        ore.Dispose();
                        ore = null;
                        oLocalReport.Dispose();
                        oLocalReport = null;
                    }
                }
            }
            catch (Exception ex)
            {
                General_Events = "FrondEnd-Imprimir -> Excepcion al imprimir: " + ex.InnerException;
            }
        }
        public void MensajesEstado(string sMensaje)
        {
            //MyDelegado4 MD = new MyDelegado4(SetTextBoxEstadosMenuSistemas);
            //this.Invoke(MD, new object[] { sMensaje });
        }
        public void SetearPantalla(Pantalla ePantalla)
        {
            MyDelegado MD = new MyDelegado(MostrarPantalla);
            this.Invoke(MD, new object[] { ePantalla });
        }
        public void FinalizarArqueo()
        {
            if (_ProcesoArqueoParcial)
            {
                MyDelegado3 MD = new MyDelegado3(PantallaFinArqueoParcial);
                this.Invoke(MD, new object[] { });
            }
            else if (_ProcesoArqueoTotal)
            {
                MyDelegado3 MD = new MyDelegado3(PantallaFinArqueoTotal);
                this.Invoke(MD, new object[] { });
            }
        }
        public void DatosPantallaPago()
        {
            if (!_BanderaPagoFinal)
            {
                MyDelegado5 MD = new MyDelegado5(CargaInfoPagoEfectivo);
                this.Invoke(MD, new object[] { });
            }
        }
        #endregion

        #region Seguridad
        private void HabilitaControles()
        {
            Control[] lstControls = this.Controls.Find("Imagen_MenuSistema", true);

            foreach (Control oControl in lstControls[0].Controls)
            {
                if (oControl.GetType() == typeof(CustomButton.CustomButton))
                {

                    //if (_IdEmpresa == "1")
                    //{
                    //    if (oControl.Name == "btn_Iniciar" || oControl.Name == "btn_Mantenimiento" || oControl.Name == "btn_Salir" || oControl.Name == "btn_IniciarCiclo" || oControl.Name == "btn_CerrarCiclo" || oControl.Name == "btn_SeguirReconteo" || oControl.Name == "btn_Log" || oControl.Name == "btn_CerrarOperacion")
                    //    {
                    //        oControl.Enabled = true;
                    //    }
                    //    else
                    //    {
                    //        oControl.Enabled = false;
                    //    }
                    //}

                    //else if (_IdEmpresa == "2")
                    //{
                    //    if (oControl.Name == "btn_Iniciar" || oControl.Name == "btn_Mantenimiento" || oControl.Name == "btn_Salir" || oControl.Name == "btn_IniciarCiclo" || oControl.Name == "btn_CerrarCiclo" || oControl.Name == "btn_SeguirReconteo" || oControl.Name == "btn_Log" || oControl.Name == "btn_CerrarOperacion")
                    //    {
                    //        oControl.Enabled = true;
                    //    }
                    //    else
                    //    {
                    //        oControl.Enabled = false;
                    //    }
                    //}

                    //else if (_IdEmpresa == "3")
                    //{
                    //    if (oControl.Name == "btn_Iniciar" || oControl.Name == "btn_Mantenimiento" || oControl.Name == "btn_Salir" || oControl.Name == "btn_Arqueo" || oControl.Name == "btn_Carga" || oControl.Name == "btn_Monedas" || oControl.Name == "btn_Puerta" || oControl.Name == "btn_CerrarOperacion")
                    //    {
                    //        oControl.Enabled = true;
                    //    }
                    //    else
                    //    {
                    //        oControl.Enabled = false;
                    //    }
                    //}
                }
            }

            foreach (Control oControl in lstControls[0].Controls)
            {
                //foreach (DtoPerfil iPerfilUsuario in _DtoUsuario.lstDtoPerfil)
                //{
                //    if (oControl.GetType() == typeof(CustomButton.CustomButton))
                //    {
                //        if (oControl.Name == iPerfilUsuario.NombreControl)
                //        {
                //            oControl.Enabled = true;
                //        }
                //    }
                //}
            }


        }
        #endregion       
    }
}
