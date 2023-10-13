using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Ds.BusinessObjects.DataTransferObject;
using Ds.Utilidades;
using Ds.BusinessObjects.Enums;
using Ds.BusinessObjects.Entities;
using System.Diagnostics;

namespace Ds.CashPaymentDevice
{
    public class BilleteroSerial : Comunicacion
    {
        #region Definiciones

        private bool banderaDesahabilitar = false;

        private int denomBills = 0;
        private int denomScrow = 0;

        private static BilleteroSerial _instancia;

        public BilleteroSerial()
        {
            #region Manejadores de Secuencias

            SecuenciaMonitor.Elapsed += new ElapsedEventHandler(SecuenciaMonitor_Elapsed);
            SecuenciaMonitor.Interval = Tbase;
            SecuenciaInicio.Elapsed += new ElapsedEventHandler(SecuenciaInicio_Elapsed);
            SecuenciaInicio.Interval = Tbase;
            SecuenciaRecibir.Elapsed += new ElapsedEventHandler(SecuenciaRecibir_Elapsed);
            SecuenciaRecibir.Interval = Tbase;
            SecuenciaDispensar.Elapsed += new ElapsedEventHandler(SecuenciaDispensar_Elapsed);
            SecuenciaDispensar.Interval = Tbase;
            SecuenciaArqueo.Elapsed += new ElapsedEventHandler(SecuenciaArqueo_Elapsed);
            SecuenciaArqueo.Interval = Tbase;

            #endregion Manejadores de Secuencias
        }

        public delegate void Enviar(string Lugar, int Denominacion);

        public delegate void Funcion(string e);

        public delegate void Funcion2(string estado, int den1, int cant1, int den2, int cant2, int den3, int cant3);

        public event Funcion2 EventoDispensar;

        public event Enviar EventoLectura;

        public event Funcion EventoSecuencia;

        public static BilleteroSerial Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new BilleteroSerial();

                return _instancia;
            }
        }
        public int[] RegistroEstado = new int[10];
        private CCassette _CCassette = new CCassette();
        private List<CCassette> _CDisponible = new List<CCassette>();
        private Comando _Comando;
        private Estado _Estado;
        private List<CPagos> _Pagos = new List<CPagos>();
        private List<CPagos> _PagosAgrupados = new List<CPagos>();
        private Parte _Parte;
        private Secuencia _Secuencia;
        private bool BanderaFinArqueo = false;
        private bool banderaFinSecuencia;
        private int[] Cantidad = new int[15];
        private int ContadorReset = 0;
        private int[] CRC = new int[2];
        private ArrayList Data = new ArrayList();
        private List<string> Data3 = new List<string>();
        private string dataString;
        private int DenCass1 = 0;
        private int DenCass2 = 0;
        private int DenCass3 = 0;
        private int hp;
        private int Indice = 0;
        private int IndiceComando = 0;
        private int IndicePagos = 0;
        private int MontoDisponible = 0;
        private Timer Periodo = new Timer();
        private Timer SecuenciaArqueo = new Timer();
        private Timer SecuenciaDispensar = new Timer();
        private Timer SecuenciaInicio = new Timer();
        private Timer SecuenciaMonitor = new Timer();
        private Timer SecuenciaRecibir = new Timer();
        private int[] TablaBilletes = new int[24];
        private int Tbase = 10;
        private byte TbaseContador = 0;
        private int TEnvio = 25;
        private Timer TPoll = new Timer();
        private string transType = string.Empty;
        private int TRecepcion = 10;
        private int ValorBilletes = 0;
        private int ValorDispensar = 0;
        private int ValorMonedas = 0;
        private Int32 y;

        #endregion Definiciones

        #region Enumeradores

        public enum Comando
        {
            Poll,
            Reset,
            Seguridad,
            ObtenerEstado,
            FormatoCodigoBarras,
            EnviarOpciones,
            ObtenerEstadoCassetes,
            ObtenerTablaBilletes,
            Habilitar,
            Deshabilitar,
            DispensarBillete,
            DescargarCassette,
            ConfigurarCassette,
            Ninguno
        }

        public enum Estado
        {
            Inicializacion,
            Inicializando,
            Habilitado,
            Deshabilitado,
            Atascado,
            Lleno,
            Vacio,
            Dispensando,
            EsperandoRemocion,

            DevolviendoInhibitet,
            DevolviendoVerification,

            Afuera,

            Descargando,
            Descargado,
            Cerrado,
            Abierto,
            Monitoriando,
            Disponible,
            ErrorDispensando
        }

        public enum Mensaje
        {
            NoEnviado,
            Enviado,
            recibido
        }

        /// <summary>
        /// /no mover el orden de este enum Cassette1=0,Cassette2=1,Cassette3=2,Box=3
        /// afectaria  el calculo de dinero disponible y la recion de billetes, es posible agregar
        /// otra parte despues del Box;
        /// </summary>
        public enum Parte
        {
            Cassette1,
            Cassette2,
            Cassette3,
            Box,
            Cassettes,
            Dispensador,
            Validador,
            Puerto
        }
        public enum Secuencia
        {
            Monitor,
            Inicio,
            Recibir,
            Dispensar,
            Arqueo,
            Periodo
        }

        #endregion Enumeradores

        #region Secuencias

        private void SecuenciaArqueo_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Secuencia Arqueo

            TbaseContador++;
            if (TbaseContador == TRecepcion / 2)
            {
                foreach (byte item in Datos)
                {
                    dataString += string.Format("{0:X02} ", item);
                    Data.Add(item);
                }

                Data3.Add(dataString);
                Datos.Clear();

                if (Data.Count > 0)
                {
                    if (CalcularCRC())
                    {
                        WriteData("020106007A37");

                        switch (_Comando)
                        {
                            case Comando.Poll:
                                try
                                {
                                    if (string.Format("{0:X02}", (byte)Data[3]) == "19")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Deshabilitado;
                                        IndiceComando = !banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                        this.EventoSecuencia("Estable");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "14")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Habilitado;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "E2")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DB")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DC")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "F4")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno;
                                        this.EventoSecuencia("BoxLLeno");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "12")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1E")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Descargando;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "42")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Descargando;
                                        this.EventoSecuencia("ArqueoBoxRemovido");
                                        this.EventoSecuencia("ArqueoBoxRemovido");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "26")
                                    {
                                        if (IndiceComando > 0)
                                        {
                                            Data.Clear();
                                            RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Descargado;
                                            IndiceComando = banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                        }
                                        else
                                        {
                                            Data.Clear();
                                            RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                                            RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                        }
                                    }
                                    else
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Data.Clear();
                                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                }
                                break;

                            case Comando.ObtenerEstadoCassetes:
                                IndiceComando = !banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                                DescargarCassette(1, _CDisponible[0].CantidadBillete);
                                break;

                            case Comando.DescargarCassette:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                                break;

                            case Comando.Deshabilitar:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            default:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                                break;
                        }
                    }
                    else
                    {
                        IndiceComando = (IndiceComando > 0) ? IndiceComando -= 1 : IndiceComando;
                        Data.Clear();
                        RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                    }
                }
                else
                {
                    Data.Clear();
                    RegistroEstado[(int)Parte.Cassettes] = (int)Estado.Monitoriando;
                }
            }
            else if (TbaseContador == TEnvio)
            {
                if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado && !banderaFinSecuencia)
                {
                    switch (IndiceComando)
                    {
                        case 0: Poll(); break;
                        case 1: ObtenerEstadoCassetes(); break;
                        default:
                            banderaFinSecuencia = true;
                            TbaseContador = 0;
                            break;
                    }
                }
                else if (RegistroEstado[(int)Parte.Cassettes] == (int)Estado.Descargando && RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado)
                {
                    Poll();
                }

                else if (RegistroEstado[(int)Parte.Cassettes] == (int)Estado.Descargado && !BanderaFinArqueo)
                {
                    switch (IndiceComando)
                    {
                        case 2: DescargarCassette(1, _CDisponible[0].CantidadBillete); break;
                        case 3: DescargarCassette(2, _CDisponible[1].CantidadBillete); break;
                        case 4: DescargarCassette(3, _CDisponible[2].CantidadBillete); break;
                        default:
                            RegistroEstado[(int)Parte.Cassettes] = 0;
                            IndiceComando = 0;
                            BanderaFinArqueo = true;
                            this.EventoSecuencia("FinArqueo");
                            break;
                    }
                }
                else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Habilitado)
                {
                    Deshabilitar();
                }
                else
                {
                    Poll();
                }
            }

            #endregion Secuencia Arqueo
        }

        private void SecuenciaDispensar_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Secuencia de Dispensar

            TbaseContador++;
            if (TbaseContador == TRecepcion)
            {
                foreach (byte item in Datos)
                {
                    dataString += string.Format("{0:X02} ", item);
                    Data.Add(item);
                }

                Data3.Add(dataString);
                Datos.Clear();

                if (Data.Count > 0)
                {
                    if (CalcularCRC())
                    {
                        WriteData("020106007A37");

                        switch (_Comando)
                        {
                            case Comando.Poll:
                                try
                                {
                                    if (string.Format("{0:X02}", (byte)Data[3]) == "19")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Deshabilitado;
                                        IndiceComando = !banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "14")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Habilitado;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "E2")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DB")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "F4")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno; ;
                                        this.EventoSecuencia("BoxLLeno");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DC")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "ED")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "12")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1D" && string.Format("{0:X02}", (byte)Data[4]) == "00")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Dispensador] = (int)Estado.Dispensando;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "25")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Dispensador] = (int)Estado.Afuera;
                                        this.EventoDispensar("Dispensado", TablaBilletes[_PagosAgrupados[IndicePagos].Denominacion1], _PagosAgrupados[IndicePagos].Cantidad1, TablaBilletes[_PagosAgrupados[IndicePagos].Denominacion2], _PagosAgrupados[IndicePagos].Cantidad2, TablaBilletes[_PagosAgrupados[IndicePagos].Denominacion3], _PagosAgrupados[IndicePagos].Cantidad3);
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "28")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Dispensador] = (int)Estado.ErrorDispensando;
                                        this.EventoSecuencia("ErrorDispensar");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1D" && string.Format("{0:X02}", (byte)Data[4]) == "01")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Dispensador] = (int)Estado.EsperandoRemocion;
                                        this.EventoDispensar("ERemocion", 0, 0, 0, 0, 0, 0);
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "42")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Afuera;
                                        this.EventoSecuencia("BoxRemovido");
                                    }
                                    else
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Data.Clear();
                                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                }
                                break;

                            case Comando.Deshabilitar:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.ObtenerEstadoCassetes:
                                DispensarBillete(_PagosAgrupados[IndicePagos].Denominacion1, _PagosAgrupados[IndicePagos].Cantidad1, _PagosAgrupados[IndicePagos].Denominacion2, _PagosAgrupados[IndicePagos].Cantidad2, _PagosAgrupados[IndicePagos].Denominacion3, _PagosAgrupados[IndicePagos].Cantidad3);
                                break;

                            case Comando.DispensarBillete:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            default:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;
                        }
                    }
                    else
                    {
                        IndiceComando = (IndiceComando > 0) ? IndiceComando -= 1 : IndiceComando;
                        Data.Clear();
                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                    }
                }
                else
                {
                    Data.Clear();
                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                }
            }
            else if (TbaseContador == TEnvio)
            {
                if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado && !banderaFinSecuencia)
                {
                    switch (IndiceComando)
                    {
                        case 0: Poll(); break;
                        case 1: ObtenerEstadoCassetes(); break;
                        default:
                            banderaFinSecuencia = true;
                            IndiceComando = 0;
                            break;
                    }
                }
                else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Habilitado)
                {
                    Deshabilitar();
                    TbaseContador = 0;
                }
                else if (RegistroEstado[(int)Parte.Dispensador] == (int)Estado.Dispensando) //ojo
                {
                    RegistroEstado[(int)Parte.Dispensador] = 0;
                    Poll();
                }
                else if (RegistroEstado[(int)Parte.Dispensador] == (int)Estado.Afuera)
                {
                    RegistroEstado[(int)Parte.Dispensador] = 0;
                    IndicePagos += 1;
                    if (IndicePagos < _PagosAgrupados.Count)
                    {
                        DispensarBillete(_PagosAgrupados[IndicePagos].Denominacion1, _PagosAgrupados[IndicePagos].Cantidad1, _PagosAgrupados[IndicePagos].Denominacion2, _PagosAgrupados[IndicePagos].Cantidad2, _PagosAgrupados[IndicePagos].Denominacion3, _PagosAgrupados[IndicePagos].Cantidad3);
                    }
                    else
                    {
                        this.EventoDispensar("Resto", ValorBilletes, ValorMonedas, 0, 0, 0, 0);
                        banderaFinSecuencia = true;
                        IndiceComando = 0;
                        IndicePagos = 0;
                        ValorBilletes = 0;
                        ValorMonedas = 0;
                        DetenerSecuencias();
                    }
                }
                else
                {
                    Poll();
                }
            }

            #endregion Secuencia de Dispensar
        }

        private void SecuenciaInicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Secuencia de Inicio

            TbaseContador++;
            if (TbaseContador == TRecepcion)
            {
                foreach (byte item in Datos)
                {
                    dataString += string.Format("{0:X02} ", item);
                    Data.Add(item);
                }

                Datos.Clear();

                if (Data.Count > 0)
                {
                    if (CalcularCRC())
                    {
                        WriteData("020106007A37");

                        switch (_Comando)
                        {
                            case Comando.Poll:
                                try
                                {
                                    if (string.Format("{0:X02}", (byte)Data[3]) == "19")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Deshabilitado;
                                        IndiceComando = !banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "14")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Habilitado;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "E2")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DB")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DC")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "ED")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "F4")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno;
                                        this.EventoSecuencia("BoxLLeno");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "42")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Afuera;
                                        this.EventoSecuencia("BoxRemovido");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "12")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "13")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Inicializando;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "10")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Inicializacion;
                                    }
                                    else
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Data.Clear();
                                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                }
                                break;

                            case Comando.Reset:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.Seguridad:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.FormatoCodigoBarras:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.EnviarOpciones:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.ObtenerTablaBilletes:
                                for (int i = 0; i < 33; i += 5)
                                {
                                    TablaBilletes[Indice] = Convert.ToInt32(Data[(i + 3)]) * (int)System.Math.Pow(10, Convert.ToDouble(Data[i + 7]));
                                    Indice += 1;
                                }
                                Indice = 0;
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.ConfigurarCassette:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            default:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;
                        }
                    }
                    else
                    {
                        IndiceComando = (IndiceComando > 0) ? IndiceComando -= 1 : IndiceComando;
                        Data.Clear();
                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                    }
                }
                else
                {
                    Data.Clear();
                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                }
            }
            else if (TbaseContador == TEnvio)
            {
                if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado && !banderaFinSecuencia)
                {
                    switch (IndiceComando)
                    {
                        case 0: Poll(); break;
                        case 1: if (RegistroEstado[(int)Parte.Validador] != (int)Estado.Deshabilitado) { Reset(); }
                            else { Poll(); } break;
                        case 2: Seguridad(); break;
                        case 3: FormatoCodigoBarras(); break;
                        case 4: EnviarOpciones(); break;
                        case 5: ObtenerTablaBilletes(); break;
                        //case 6: ConfigurarCassette(1, DenCass1); break;
                        //case 7: ConfigurarCassette(2, DenCass2); break;
                        //case 8: ConfigurarCassette(3, DenCass3); break;

                        default:
                            banderaFinSecuencia = true;
                            IndiceComando = 0;
                            DetenerSecuencias();
                            this.EventoSecuencia("FinInicio");
                            break;
                    }
                }
                else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Inicializacion || RegistroEstado[(int)Parte.Validador] == (int)Estado.Habilitado || RegistroEstado[(int)Parte.Validador] == (int)Estado.Atascado)
                {
                    Reset();
                }
                else
                {
                    Poll();
                }
            }

            #endregion Secuencia de Inicio
        }

        private void SecuenciaMonitor_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Secuencia Monitor

            TbaseContador++;
            if (TbaseContador == TRecepcion)
            {
                foreach (byte item in Datos)
                {
                    dataString += string.Format("{0:X02} ", item);
                    Data.Add(item);
                }
                Datos.Clear();

                if (Data.Count > 0)
                {
                    if (CalcularCRC())
                    {
                        WriteData("020106007A37");

                        switch (_Comando)
                        {
                            case Comando.Poll:
                                try
                                {
                                    if (string.Format("{0:X02}", (byte)Data[3]) == "19")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Deshabilitado;
                                        this.EventoSecuencia("Estable");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "E2")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DB")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DC")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "ED")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "F4")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno;
                                        this.EventoSecuencia("BoxLLeno");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "12")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "14")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Inicializando;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "13")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Inicializando;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "10")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Inicializacion;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "42")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Afuera;
                                        this.EventoSecuencia("BoxRemovido");
                                    }
                                    else
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Data.Clear();
                                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                }
                                break;

                            case Comando.Reset:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.Seguridad:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.ObtenerEstado:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.FormatoCodigoBarras:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.EnviarOpciones:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            case Comando.ObtenerEstadoCassetes:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            default:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;
                        }
                    }
                    else
                    {
                        IndiceComando = (IndiceComando > 0) ? IndiceComando -= 1 : IndiceComando;
                        Data.Clear();
                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                    }
                }
                else
                {
                    Data.Clear();
                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                }
            }
            else if (TbaseContador == TEnvio)
            {
                Poll();
            }

            #endregion Secuencia Monitor
        }

        private void SecuenciaRecibir_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Secuencia de Recepcion

            TbaseContador++;
            if (TbaseContador == TRecepcion / 2)
            {
                foreach (byte item in Datos)
                {
                    dataString += string.Format("{0:X02} ", item);
                    Data.Add(item);
                }

                Datos.Clear();
                WriteData("020106007A37");

                if (Data.Count > 0)
                {
                    if (CalcularCRC())
                    {
                        //WriteData("020106007A37");

                        switch (_Comando)
                        {
                            case Comando.Poll:
                                try
                                {
                                    if (string.Format("{0:X02}", (byte)Data[3]) == "19")
                                    {
                                        Data.Clear();
                                        this.EventoSecuencia("Deshabilitado");
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Deshabilitado;
                                        IndiceComando = !banderaFinSecuencia ? IndiceComando += 1 : IndiceComando = 0;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "14")
                                    {
                                        Data.Clear();
                                        this.EventoSecuencia("Habilitado");
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Habilitado;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "E2")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DB")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "DC")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "ED")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47" && string.Format("{0:X02}", (byte)Data[4]) == "F4")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Lleno;
                                        this.EventoSecuencia("BoxLLeno");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "47")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "11")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "12")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                        this.EventoSecuencia("Atascado");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "15")
                                    {
                                        this.EventoSecuencia("BilleteBoca");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "17")
                                    {
                                        this.EventoSecuencia("Enrutando");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1C" && string.Format("{0:X02}", (byte)Data[4]) == "68")
                                    {
                                        //Data.Clear();
                                        this.EventoSecuencia("DevolviendoInhibitet");
                                        //RegistroEstado[(int)Parte.Validador] = (int)Estado.DevolviendoInhibitet;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1C" && string.Format("{0:X02}", (byte)Data[4]) == "66")
                                    {
                                        //Data.Clear();
                                        this.EventoSecuencia("DevolviendoVerification");
                                        //RegistroEstado[(int)Parte.Validador] = (int)Estado.DevolviendoVerification;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "1C")
                                    {
                                        this.EventoSecuencia("Devolviendo");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "43")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Atascado;
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "42")
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Box] = (int)Estado.Afuera;
                                        this.EventoSecuencia("BoxRemovido");
                                    }
                                    else if (string.Format("{0:X02}", (byte)Data[3]) == "81")
                                    {
                                        if ((byte)Data[5] == (byte)Parte.Box - 3)
                                        {
                                            this.EventoLectura("Box", TablaBilletes[(byte)Data[4]]);
                                        }
                                        else if ((byte)Data[5] == (byte)Parte.Cassette1 + 1)
                                        {
                                            this.EventoLectura("Cassette", TablaBilletes[(byte)Data[4]]);
                                        }
                                        else if ((byte)Data[5] == (byte)Parte.Cassette1 + 2)
                                        {
                                            this.EventoLectura("Cassette", TablaBilletes[(byte)Data[4]]);
                                        }
                                        else if ((byte)Data[5] == (byte)Parte.Cassette1 + 3)
                                        {
                                            this.EventoLectura("Cassette", TablaBilletes[(byte)Data[4]]);
                                        }
                                        else
                                        {
                                            //trace
                                        }
                                    }
                                    else
                                    {
                                        Data.Clear();
                                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Data.Clear();
                                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                }
                                break;

                            case Comando.Habilitar:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;
                            case Comando.Deshabilitar:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;
                            case Comando.ObtenerEstado:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;

                                break;

                            case Comando.ObtenerEstadoCassetes:
                                Data.Clear();
                                RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        IndiceComando = (IndiceComando > 0) ? IndiceComando -= 1 : IndiceComando;
                        Data.Clear();
                        RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                    }
                }
                else
                {
                    Data.Clear();
                    RegistroEstado[(int)Parte.Validador] = (int)Estado.Monitoriando;
                }
            }
            else if (TbaseContador == TEnvio)
            {
                if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado && !banderaFinSecuencia)
                {
                    switch (IndiceComando)
                    {
                        case 0: Poll(); break;
                        case 1:
                            if (denomBills == 0)
                            {
                                Habilitar();
                            }
                            else
                            {
                                HabilitarDenomIndependiente();
                            }
                            break;
                        default:
                            banderaFinSecuencia = true;
                            IndiceComando = 0;
                            break;
                    }
                }
                //else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Habilitado && banderaDesahabilitar)
                //{
                //    Deshabilitar();
                //}
                //else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Deshabilitado && banderaDesahabilitar)
                //{
                //    DetenerSecuencias();
                //    banderaDesahabilitar = false;
                //}
                else if (RegistroEstado[(int)Parte.Validador] == (int)Estado.Inicializacion || RegistroEstado[(int)Parte.Validador] == (int)Estado.Atascado)
                {
                    if (ContadorReset < 3)
                    {
                        ContadorReset++;
                        Reset();
                    }
                    else
                    {
                        this.EventoSecuencia("Atascamiento");
                        SecuenciaRecibir.Stop();
                        ContadorReset = 0;
                    }
                }
                else
                {
                    Poll();
                }
            }

            #endregion Secuencia de Recepcion
        }

        #endregion Secuencias

        #region Reset

        private void Reset()
        {
            _Comando = Comando.Reset;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            BytesThreshold = 6;
            WriteData("02010630F906");
        }

        #endregion Reset

        #region funciones

        public bool AbrirPuerto(string sPuerto)
        {
            return OpenPort(sPuerto);
        }

        private void ConfigurarCassette(int c1, int d1)
        {
            _Comando = Comando.ConfigurarCassette;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            byte[] Paquete = new byte[8];
            Paquete[0] = 2; Paquete[1] = 1; Paquete[2] = 8; Paquete[3] = 64; Paquete[4] = (byte)c1; Paquete[5] = (byte)(d1 + 128);
            string PaqueteString = CrearPaquete(Paquete);
            WriteData(PaqueteString);
            TbaseContador = 0;
        }

        private void FormatoCodigoBarras()
        {
            _Comando = Comando.FormatoCodigoBarras;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            WriteData("02 01 08 39 01 12 31 D6");
        }

        private void ObtenerEstado()
        {
            _Comando = Comando.ObtenerEstado;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 14;
            WriteData("02 01 06 31 70 17");
        }

        private void ObtenerTablaBilletes()
        {
            _Comando = Comando.ObtenerTablaBilletes;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 125;
            WriteData("02 01 06 41 F7 64");
        }

        private void Poll()
        {

            _Comando = Comando.Poll;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 8;
            WriteData("02 01 06 33 62 34");
        }

        private void Seguridad()
        {
            _Comando = Comando.Seguridad;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            WriteData("02 01 09 32 00 00 00 70 17");
        }

        #region funciones privadas

        public void DetenerSecuencias()
        {
            SecuenciaMonitor.Stop();
            SecuenciaInicio.Stop();
            SecuenciaRecibir.Stop();
            SecuenciaDispensar.Stop();
            SecuenciaArqueo.Stop();
            Periodo.Stop();
            banderaFinSecuencia = false;
            IndiceComando = 0;
            IndicePagos = 0;
        }

        private void CalcularCantidadBilletes(int monto)
        {
            int[] billetes = new int[50];
            int[] denominacion = new int[50];
            for (int i = 0; i < 50; i++)
            {
                billetes[i] = monto / denominacion[i];
                monto = monto % denominacion[i];
            }
        }

        private bool CalcularCRC()
        {
            try
            {
                ushort crc = 0;
                ushort L = (byte)(Data[2]);
                if (L > 2)
                {
                    for (int j = 0; j < L - 2; j++)
                    {
                        ushort b = Convert.ToChar(Data[j]);
                        for (int i = 0; i < 8; i++)
                        {
                            crc = ((b ^ crc) & 1) > 0 ? (ushort)((crc >> 1) ^ 0x8408) : (ushort)(crc >> 1);
                            b >>= 1;
                        }
                    }
                    return (byte)(Data[L - 2]) == (crc & 255) && (byte)(Data[L - 1]) == (crc / 256) ? true : false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool CalcularPago(int MontoSolicitado)
        {
            try
            {
                List<CPagos> _PagosAux = new List<CPagos>();
                List<int> _Denominacion = new List<int>();
                int MontoAuxiliar = MontoSolicitado;
                int MontoRestante = MontoSolicitado;
                int j = 0, N = 0, i = 0;
                _Pagos.Clear();
                _PagosAux.Clear();
                _Denominacion.Clear();
                _PagosAgrupados.Clear();

                Array.Clear(Cantidad, 0, Cantidad.Length);

                var list = from b in _CDisponible
                           orderby b.TipoBillete descending, b.TipoBillete
                           select b.TipoBillete;

                foreach (int item in list)
                    _Denominacion.Add(item);

                if (MontoDisponible >= MontoSolicitado)
                {
                    do
                    {
                        N = MontoAuxiliar / _Denominacion[j];
                        int x6 = ObtenerCantidadXtipo(_Denominacion[j]);
                        Cantidad[i] += N;
                        if (Cantidad[i] > 20 || N > ObtenerCantidadXtipo(_Denominacion[j]))
                        {
                            MontoAuxiliar -= _Denominacion[j];
                            MontoAuxiliar = MontoAuxiliar < 0 ? MontoAuxiliar = 0 : MontoAuxiliar;
                            Cantidad[i] = 0;
                            N = 0;
                        }
                        else
                        {
                            if (N != 0)
                            {
                                MontoRestante -= N * _Denominacion[j];
                                MontoAuxiliar = MontoRestante;
                                for (int w = 0; w < _CDisponible.Count; w++)
                                {
                                    if (_CDisponible[w].TipoBillete == _Denominacion[j])
                                    {
                                        _CDisponible[w].CantidadBillete -= Cantidad[i];
                                        break;
                                    }
                                }
                                // _CDisponible[j].CantidadBillete -= Cantidad[i];
                                _Pagos.Add(new CPagos(_Denominacion[j], Cantidad[i]));
                            }

                            if (MontoRestante > _Denominacion[j] && ObtenerCantidadXtipo(_Denominacion[j]) > 0)
                            { i++; }
                            else
                            {
                                if (MontoRestante == 0)
                                    break;
                                i++; j++; N = 0; MontoAuxiliar = MontoRestante;
                            }
                        }
                    } while (j < 3);

                    #region Organizar lista

                    var listPagos = from p in _Pagos
                                    orderby p.Cantidad descending, p.Cantidad
                                    select new { p.Denominacion, p.Cantidad };

                    foreach (var group in listPagos)
                    {
                        Console.WriteLine(group);
                        _PagosAux.Add(new CPagos(ObtenerId((int)group.Denominacion), (int)group.Cantidad));
                    }

                    if (_PagosAux.Count > 0)
                    {
                        for (i = 0; i < _PagosAux.Count; i++)
                        {
                            if (_PagosAux[i].Cantidad == 20)
                            {
                                _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, _PagosAux[i].Denominacion, 0, _PagosAux[i].Denominacion, 0));
                                _PagosAux[i].Denominacion = 0;
                                _PagosAux[i].Cantidad = 0;
                            }
                            else
                            {
                                if (i + 1 < _PagosAux.Count)
                                {
                                    if ((_PagosAux[i].Cantidad + _PagosAux[i + 1].Cantidad) <= 20)
                                    {
                                        if (i + 2 < _PagosAux.Count)
                                        {
                                            if ((_PagosAux[i].Cantidad + _PagosAux[i + 1].Cantidad + _PagosAux[i + 2].Cantidad) <= 20)
                                            {
                                                _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, _PagosAux[i + 1].Denominacion, _PagosAux[i + 1].Cantidad, _PagosAux[i + 2].Denominacion, _PagosAux[i + 2].Cantidad));
                                                _PagosAux[i].Denominacion = 0;
                                                _PagosAux[i].Cantidad = 0;
                                                _PagosAux[i + 1].Denominacion = 0;
                                                _PagosAux[i + 1].Cantidad = 0;
                                                _PagosAux[i + 2].Denominacion = 0;
                                                _PagosAux[i + 2].Cantidad = 0;
                                            }
                                            else
                                            {
                                                _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, _PagosAux[i + 1].Denominacion, _PagosAux[i + 1].Cantidad, 0, 0));
                                                _PagosAux[i].Denominacion = 0;
                                                _PagosAux[i].Cantidad = 0;
                                                _PagosAux[i + 1].Denominacion = 0;
                                                _PagosAux[i + 1].Cantidad = 0;
                                            }
                                        }
                                        else
                                        {
                                            if (_PagosAux[i].Cantidad != 0)
                                                _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, _PagosAux[i + 1].Denominacion, _PagosAux[i + 1].Cantidad, 0, 0));
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, 0, 0, 0, 0));
                                        _PagosAux[i].Denominacion = 0;
                                        _PagosAux[i].Cantidad = 0;
                                    }
                                }
                                else
                                {
                                    if (_PagosAux[i].Cantidad != 0)
                                        _PagosAgrupados.Add(new CPagos(_PagosAux[i].Denominacion, _PagosAux[i].Cantidad, 0, 0, 0, 0));
                                    break;
                                }
                            }
                        }

                        ValorBilletes = MontoSolicitado - MontoRestante;
                        ValorMonedas = MontoRestante;
                        TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "CalcularPago Billetero Serial: Denominacion1: " + _PagosAgrupados[0].Denominacion1 + " Cantidad1: " + _PagosAgrupados[0].Cantidad1 + " Denominacion2: " + _PagosAgrupados[0].Denominacion2 + " Cantidad2: " + _PagosAgrupados[0].Cantidad2 + " Denominacion3: " + _PagosAgrupados[0].Denominacion3 + " Cantidad3: " + _PagosAgrupados[0].Cantidad3, TipoLog.TRAZA);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    #endregion Organizar lista
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string CrearPaquete(byte[] Paquete)
        {
            try
            {
                ushort crc = 0;
                ushort L = (byte)(Paquete.Length - 2);
                string PaqueteString = string.Empty;

                for (int j = 0; j < L; j++)
                {
                    ushort b = Convert.ToChar(Paquete[j]);
                    for (int i = 0; i < 8; i++)
                    {
                        crc = ((b ^ crc) & 1) > 0 ? (ushort)((crc >> 1) ^ 0x8408) : (ushort)(crc >> 1);
                        b >>= 1;
                    }
                }
                Paquete[L] = (byte)(crc & 255);
                Paquete[L + 1] = (byte)(crc / 256);

                for (int i = 0; i < L + 2; i++)
                {
                    PaqueteString += string.Format("{0:X02} ", Paquete[i]);
                }

                return PaqueteString;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private void DescargarCassette(int d1, int c1)
        {
            _Comando = Comando.DescargarCassette;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            byte[] Paquete = new byte[8];
            Paquete[0] = 2; Paquete[1] = 1; Paquete[2] = 8; Paquete[3] = 61; Paquete[4] = (byte)d1; Paquete[5] = (byte)c1;
            string PaqueteString = CrearPaquete(Paquete);
            WriteData(PaqueteString);
            TbaseContador = 0;
        }

        private void Deshabilitar()
        {
            _Comando = Comando.Deshabilitar; ;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            WriteData("02 01 0C 34 00 00 00 00 00 00 ED 97");
            TbaseContador = 0;
        }

        private void DispensarBillete(int d1, int c1, int d2, int c2, int d3, int c3)
        {
            if (d1 != 0 && d2 == 0 && d3 == 0)
            {
                _Comando = Comando.DispensarBillete;
                Data.Clear();
                Datos.Clear();
                dataString = string.Empty;
                LimpiarBuffer();
                BytesThreshold = 6;
                byte[] Paquete = new byte[8];

                Paquete[0] = 2; Paquete[1] = 1; Paquete[2] = 8; Paquete[3] = 60; Paquete[4] = (byte)d1; Paquete[5] = (byte)c1;

                string PaqueteString = CrearPaquete(Paquete);
                WriteData(PaqueteString);
                TbaseContador = 0;
            }
            else if (d1 != 0 && d2 != 0 && d3 == 0)
            {
                _Comando = Comando.DispensarBillete;
                Data.Clear();
                Datos.Clear();
                dataString = string.Empty;
                LimpiarBuffer();
                BytesThreshold = 6;
                byte[] Paquete = new byte[10];

                Paquete[0] = 2; Paquete[1] = 1; Paquete[2] = 10; Paquete[3] = 60; Paquete[4] = (byte)d1; Paquete[5] = (byte)c1;
                Paquete[6] = (byte)d2; Paquete[7] = (byte)c2;

                string PaqueteString = CrearPaquete(Paquete);
                WriteData(PaqueteString);
                TbaseContador = 0;
            }
            else if (d1 != 0 && d2 != 0 && d3 != 0)
            {
                _Comando = Comando.DispensarBillete;
                Data.Clear();
                Datos.Clear();
                dataString = string.Empty;
                LimpiarBuffer();
                BytesThreshold = 6;
                byte[] Paquete = new byte[12];
                Paquete[0] = 2; Paquete[1] = 1; Paquete[2] = 12; Paquete[3] = 60; Paquete[4] = (byte)d1; Paquete[5] = (byte)c1;
                Paquete[6] = (byte)d2; Paquete[7] = (byte)c2; Paquete[8] = (byte)d3; Paquete[9] = (byte)c3;
                string PaqueteString = CrearPaquete(Paquete);
                WriteData(PaqueteString);
                TbaseContador = 0;
            }

            
        }

        private void EnviarOpciones()
        {
            _Comando = Comando.EnviarOpciones;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            WriteData("02 01 0A 68 60 00 00 00 C3 DF");
        }

        private void Habilitar()
        {
            _Comando = Comando.Habilitar;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;
            WriteData("02 01 0C 34 FF FF FF 00 00 00 4F 5A");
        }

        private void HabilitarDenomIndependiente()
        {

            string hexValueBill = denomBills.ToString("X");
            hexValueBill = hexValueBill.PadLeft(2, '0');

            string hexValueScrow = denomScrow.ToString("X");
            hexValueScrow = hexValueScrow.PadLeft(2, '0');


            string trama = "02010C340000" + hexValueBill + "0000" + hexValueScrow + "0000";

            byte[] Paquete = new byte[12];

            Paquete = StringToByteArray(trama);

            string paqueteEnviar = CrearPaquete(Paquete);

            _Comando = Comando.Habilitar;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 6;

            WriteData(paqueteEnviar);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private void HabilitarSecuencia()
        {
            switch (_Secuencia)
            {
                case Secuencia.Monitor:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaMonitor.Start();
                    break;

                case Secuencia.Inicio:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaInicio.Start();
                    break;

                case Secuencia.Recibir:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaRecibir.Start();
                    break;

                case Secuencia.Dispensar:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaDispensar.Start();
                    break;

                case Secuencia.Arqueo:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaArqueo.Start();
                    break;

                default:
                    DetenerSecuencias();
                    Poll();
                    SecuenciaMonitor.Start();
                    break;
            }
        }

        private int ObtenerCantidadXtipo(int d)
        {
            int Cantidad = 0;

            foreach (var item in _CDisponible)
            {
                if (item.TipoBillete == d)
                {
                    Cantidad = item.CantidadBillete;
                    break;
                }
            }

            return Cantidad;
        }

        private void ObtenerEstadoCassetes()
        {
            //_Comando = Comando.ObtenerEstadoCassetes;
            //TbaseContador = 0;
            //Data.Clear();
            //Datos.Clear();
            //dataString = string.Empty;
            //LimpiarBuffer();
            //BytesThreshold = 11;
            //WriteData("02 01 06 3B 2A B8");

            _Comando = Comando.ObtenerEstadoCassetes;
            TbaseContador = 0;
            Data.Clear();
            Datos.Clear();
            dataString = string.Empty;
            LimpiarBuffer();
            BytesThreshold = 8;
            WriteData("02 01 06 33 62 34");
        }
        private int ObtenerId(int d)
        {
            int Indice = 125;

            for (int i = 0; i < TablaBilletes.Length; i++)
            {
                if (TablaBilletes[i] == d)
                {
                    Indice = i;
                    break;
                }
            }
            return Indice;
        }
        private int ObtenerTipoBillete(int id)
        {
            int Tipo = 0;
            for (int i = 0; i < TablaBilletes.Length; i++)
            {
                if (id == i)
                {
                    Tipo = TablaBilletes[i];
                    break;
                }
            }
            return Tipo;
        }

        #endregion funciones privadas

        #region Funciones Publicas

        public void Arqueo(DtoModulo oEoModulo)
        {
            _CDisponible.Clear();
            MontoDisponible = 0;
            BanderaFinArqueo = false;
            RegistroEstado[(int)Parte.Cassettes] = 0;
            Datos.Clear();

            for (int i = 0; i < oEoModulo.Partes.Count; i++)
            {
                if (oEoModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                {
                    if (oEoModulo.Partes[i].Nombre == "Cass1")
                        _CDisponible.Add(new CCassette(0, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), 200));
                    else if (oEoModulo.Partes[i].Nombre == "Cass2")
                        _CDisponible.Add(new CCassette(1, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), 200));
                    else if (oEoModulo.Partes[i].Nombre == "Cass3")
                        _CDisponible.Add(new CCassette(2, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), 200));
                }
            }

            _Secuencia = Secuencia.Arqueo;
            HabilitarSecuencia();
        }

        public void DeshabilitarRecepcion()
        {
            //banderaDesahabilitar = true;
            DetenerSecuencias();
            Deshabilitar();
        }

        public void Dispensar(int Valor, DtoModulo oEoModulo)
        {
            ValorDispensar = Valor;
            _CDisponible.Clear();
            MontoDisponible = 0;
            IndicePagos = 0;
            Datos.Clear();
            RegistroEstado[(int)Parte.Dispensador] = 0;

            for (int i = 0; i < oEoModulo.Partes.Count; i++)
            {
                if (oEoModulo.Partes[i].TipoParte == TipoParte.Cassette.ToString())
                {
                    if (oEoModulo.Partes[i].Nombre == "Cass1")
                        _CDisponible.Add(new CCassette(0, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), Convert.ToInt32(oEoModulo.Partes[i].CantidadActual)));
                    else if (oEoModulo.Partes[i].Nombre == "Cass2")
                        _CDisponible.Add(new CCassette(1, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), Convert.ToInt32(oEoModulo.Partes[i].CantidadActual)));
                    else if (oEoModulo.Partes[i].Nombre == "Cass3")
                        _CDisponible.Add(new CCassette(2, Convert.ToInt32(oEoModulo.Partes[i].Denominacion), Convert.ToInt32(oEoModulo.Partes[i].CantidadActual)));
                }
            }

            for (int i = 0; i < 3; i++)
                MontoDisponible += _CDisponible[i].TipoBillete * _CDisponible[i].CantidadBillete;

            if (CalcularPago(ValorDispensar))
            {
                _Secuencia = Secuencia.Dispensar;
                HabilitarSecuencia();
            }
            else
            {
                this.EventoDispensar("ErrorDispensar", 0, 0, 0, 0, 0, 0);
            }
        }

        public bool Iniciar(int d1, int d2, int d3, string sPuerto)
        {
            Datos.Clear();
            DenCass1 = d1;
            DenCass2 = d2;
            DenCass3 = d3;

            if (OpenPort(sPuerto))
            {
                RegistroEstado[(int)Parte.Validador] = 0;
                _Secuencia = Secuencia.Inicio;
                HabilitarSecuencia();
            }
            else
            {
                return false;
            }
            return true;
        }

        public void Monitor()
        {
            _Secuencia = Secuencia.Monitor;
            HabilitarSecuencia();
        }
        public void Recibir()
        {
            denomBills = 0;
            denomScrow = 0;
            Datos.Clear();
            _Secuencia = Secuencia.Recibir;
            HabilitarSecuencia();
        }
        public void RecibirIndependiente(int bills, int scrows)
        {
            denomBills = bills;
            denomScrow = scrows;
            Datos.Clear();
            _Secuencia = Secuencia.Recibir;
            HabilitarSecuencia();
        }

        #endregion Funciones Publicas

        #endregion funciones
    }

    #region Clases Auxiliares

    #region Clase Cpagos

    public class CPagos
    {
        private int _cantidad;
        private int _cantidad1;
        private int _cantidad2;
        private int _cantidad3;
        private int _denominacion;
        private int _denominacion1;
        private int _denominacion2;
        private int _denominacion3;
        public CPagos(int d1, int c1)
        {
            Denominacion = d1;
            Cantidad = c1;
        }

        public CPagos(int d1, int c1, int d2, int c2, int d3, int c3)
        {
            Denominacion1 = d1;
            Cantidad1 = c1;
            Denominacion2 = d2;
            Cantidad2 = c2;
            Denominacion3 = d3;
            Cantidad3 = c3;
        }

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public int Cantidad1
        {
            get { return _cantidad1; }
            set { _cantidad1 = value; }
        }

        public int Cantidad2
        {
            get { return _cantidad2; }
            set { _cantidad2 = value; }
        }

        public int Cantidad3
        {
            get { return _cantidad3; }
            set { _cantidad3 = value; }
        }

        public int Denominacion
        {
            get { return _denominacion; }
            set { _denominacion = value; }
        }
        public int Denominacion1
        {
            get { return _denominacion1; }
            set { _denominacion1 = value; }
        }
        public int Denominacion2
        {
            get { return _denominacion2; }
            set { _denominacion2 = value; }
        }
        public int Denominacion3
        {
            get { return _denominacion3; }
            set { _denominacion3 = value; }
        }
    }

    #endregion Clase Cpagos

    #region Clase CCassette

    public class CCassette
    {
        private int _CantidadBillete;
        private int _Nombre;
        private int _TipoBillete;
        public CCassette()
        {
        }

        public CCassette(int N, int T, int C)
        {
            Nombre = N;
            TipoBillete = T;
            CantidadBillete = C;
        }

        public int CantidadBillete
        {
            get { return _CantidadBillete; }
            set { _CantidadBillete = value; }
        }

        public int Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public int TipoBillete
        {
            get { return _TipoBillete; }
            set { _TipoBillete = value; }
        }
    }

    #endregion Clase CCassette

    #endregion Clases Auxiliares
}
