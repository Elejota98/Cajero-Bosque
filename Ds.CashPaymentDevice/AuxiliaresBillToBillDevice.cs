using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.CashPaymentDevice
{
    public sealed class ComandosEnvioDispositivo
    {
        private ComandosEnvioDispositivo() { }

        public const string BB = "01";
        public const string BV = "03";
        public const string DB = "05";

    }

    public sealed class ComandosEnvioDropBox
    {
        private ComandosEnvioDropBox() { }

        public const string Reset = "10";
        public const string Drop = "20";
        public const string Poll = "33";
        public const string OpenCloseDrop = "F0";
    }

    public sealed class ComandosRecepcionDropBox
    {
        private ComandosRecepcionDropBox() { }

        public const string ACK = "00";
        public const string PowerUp = "10";
        public const string Initializing = "11";
        public const string Idling = "12";
        public const string Failure = "13";
        public const string Busy = "19";
        public const string Dropping = "20";
        public const string DropOk = "21";
        public const string DropFailed = "22";
        public const string Standby = "44";
    }


    public sealed class ComandosEnvio
    {
        private ComandosEnvio() { }

        public const string ACK = "00";
        public const string Reset = "30";
        public const string ObtenerEstatus = "31";
        public const string EnviarSeguridad = "32";
        public const string Poll = "33";
        public const string HabilitarDeshabilitar = "34";
        public const string Almacenar = "35";
        public const string RetornarDeScrow = "36";
        public const string Identificacion = "37";
        public const string Hold = "38";
        public const string EnviarParametrosCodigoBarras = "39";
        public const string ExtraerDatosCodigoBarras = "3A";
        public const string ObtenerEstatusCassettes = "3B";
        public const string Dispensar = "3C";
        public const string DescargarCassette = "3D";
        public const string IdentificacionExtendida = "3E";
        public const string ConfigurarCassette = "40";
        public const string ObtenerTablaBilletes = "41";
        public const string ObtenerCapacidadCassette = "43";
        public const string AjustarCapacidadCassette = "44";
        public const string DownloadMode = "50";
        public const string ObtenerCRC = "51";
        public const string DownloadModule = "52";
        public const string ModuleIdentificationRequest = "53";
        public const string ValidationModuleIdentification = "54";
        public const string ObtenerCRC2 = "56";
        public const string ObtenerEstadisticas = "60";
        public const string SolicitarAjustarHoraFecha = "62";
        public const string PowerRecovery = "66";
        public const string VaciarDispensador = "67";
        public const string EnviarOpciones = "68";
        public const string ObtenerOpciones = "69";
        public const string ObtenerEstadoExtendidoCassettes = "70";
        public const string DiagnosticoAjuste = "F0";
    }

    public sealed class ComandosRecepcion
    {
        private ComandosRecepcion() { }

        //
        public const string ACK = "00";
        public const string Encendiendo = "10";
        public const string EncendiendoBilleteEnValidador = "11";
        public const string EncendiendoBilleteEnChasis = "12";
        public const string Inicializando = "13";
        public const string Recibiendo = "14";
        public const string Aceptando = "15";
        public const string Almacenando = "17";
        public const string DevolviendoDeEscrow = "18";
        public const string Deshabilitado = "19";
        public const string Holding = "1A";
        public const string Ocupado = "1B"; //Y en siguiente byte simboliza tiempo para poll en multiplos de 100ms
        public const string Rechazando = "1C";
        public const string Dispensando = "1D"; // 00 moving to recycle -- 01 waiting to remove
        public const string DescargandoCassette = "1E"; // 00 cantidad suficiente de billetes --  01 Cantidad insuficiente de billetes
        public const string ConfigurandoCassette = "21";
        public const string BilletesDispensados = "25";
        public const string BilletesDescargados = "26"; //Numero billetes descargados
        public const string CantidadInvalidaBilletes = "28";
        public const string CassetteConfigurado = "29";
        public const string ComandoInvalido = "30";
        public const string BoxLleno = "41";
        public const string BoxAfuera = "42";
        public const string AtascoValidador = "43";
        public const string AtascoBox = "44";
        public const string Cheated = "45";
        //public const string Pausa                                 = "46";
        public const string FallaGenerica = "47";
        public const string BilleteEnEscrow = "80";
        public const string BilleteAlmacenado = "81";
        public const string BilleteDevuelto = "82";
        public const string EsperandoDecision = "83";
        //
    }

    public sealed class ComandosGenericosRechazo
    {
        private ComandosGenericosRechazo() { }

        public const string Insercion = "60";
        public const string Magnetismo = "61";
        public const string PermaneceValidador = "62";
        public const string Multiple = "63";
        public const string ErrorTransporte = "64";
        public const string Identificacion1 = "65";
        public const string Verificacion = "66";
        public const string Optico = "67";
        public const string DenominacionInhabilitada = "68";
        public const string Capacidad = "69";
        public const string Operacion = "6A";
        public const string Tamano = "6C";
        public const string UV = "6D";
        public const string CodigoBarras = "92";
        public const string NumeroCaracteresCodigoBarras = "93";
        public const string InicioSecuenciaCodigoBarras = "94";
        public const string FinSecuenciaCodigoBarras = "95";

    }

    public sealed class ComandosGenericosFallas
    {
        private ComandosGenericosFallas() { }

        public const string MotorAlmacenador = "50";
        public const string VelocidadMotorTransportador = "51";
        public const string MotorTransportador = "52";
        public const string MotorAlineacion = "53";
        public const string StatusInicialCassette = "54";
        public const string CanalOptico = "55";
        public const string CanalMagnetico = "56";
        public const string SensorCapacitivo = "5F";

    }

    public enum DeviceTypeEnum
    {
        BB,
        BV,
    }

    public enum StatesBillToBillDevice
    {
        //PreNothing,
        Nothing,
        Busy,
        PowerUp,
        Inicializing,
        SettingCassette1,
        SettingCassette2,
        SettingCassette3,
        SettingCassetteCapacity1,
        SettingCassetteCapacity2,
        SettingCassetteCapacity3,
        SettingCassetteType,
        Disabling,
        Disable,
        Idling,
        Jam,
        WaitingRemove,
        PreAceptingAll,
        PreAcepting,
        Acepting,
        Storing,
        BillAcepted,
        Rejecting,
        UnloadingCassette,
        CassetteUnloaded,
        CassettesUnloaded,
        Dispensed,
        ErrorDispensing,
        BoxFull,
        BoxOut,
        ValidatorJam,
        BoxJam,
        EndInicialization,
        EndDispensing,
        PollingSecuense,
        BoxIn,
        Falla,
        Dispensing,
        SendingOptions,
        StartSendingOptions,
    }

    public enum StatesDropBoxDevice
    {
        Nothing,
        Inicializing,
        EndInicialization,
        Idling,
        OpeningDropControl,
        EndOpenDropControl,
        ClosingDropControl,
        EndCloseDropControl,
        Dropping,
        EndDrop,
        DropError,
    }

    public enum TipoInsertEvento
    {
        Ninguno,
        Envio,
        Recepcion,
    }
}
