using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.CashPaymentDevice
{
    public enum F56Style
    {
        Rear,
        Front,
    }
    public enum StatesF56Device
    {
        Nothing,
        Inicializing,
        EndInicialization,
        Disable,
        InicializingTransporting,
        EndTransport,
        InicializingDispensing,
        Dispensing,
        EndDispense,
        EndPartialDispense,
        EndDispensePack,
        ErrorDeviceInformation,
        AbnormalInitialization,
        AbnormalBillCount,
        AbnormalBillTransportation,
        AbnormalBillCountAndTransportFront,
        WaitingRemoveBills,
        EndWaitingRemoveBills,
        ClosingShutter,
        Transporting,
        AbnormalBillCountAndTransportRear,
    }

    public enum F56ShutterAction
    {
        Both = 0,       //00H
        OnlyRear = 1,   //01H
        OnlyFront = 2,  //02H
        NoShutter = 3,  //03H
    }

    public sealed class ComandosRecepcionBasicoF56Device
    {
        private ComandosRecepcionBasicoF56Device() { }

        public const string STX = "1002";
        public const string ENQ = "1005";
        public const string ACK = "1006";
    }

    public sealed class ComandosRecepcionDH0F56Device
    {
        private ComandosRecepcionDH0F56Device() { }

        public const string Positive = "E0";
        public const string Negative = "F0";
    }

    public sealed class ComandosRecepcionDH1F56Device
    {
        private ComandosRecepcionDH1F56Device() { }

        public const string StatusInformation = "01";
        public const string Initialization = "02";
        public const string BillCount = "03";
        public const string BillTransportation = "05";
        public const string BillRetrival = "06";
        public const string BillCountTransportationFront = "09";
        public const string BillCountTransportationRear = "0A";
    }

    public class F56Bill
    {
        public F56Bill()
        {

        }

        public F56Bill(int iBillLengthDown, int iBillLengthUp, double iBillThickness)
        {
            _BillLengthDown = iBillLengthDown;
            _BillLengthUp = iBillLengthUp;
            setBillThickness(iBillThickness);
        }

        private int _BillLengthDown = 0;

        public int BillLengthDown
        {
            get { return _BillLengthDown; }
            set { _BillLengthDown = value; }
        }

        private int _BillLengthUp = 0;

        public int BillLengthUp
        {
            get { return _BillLengthUp; }
            set { _BillLengthUp = value; }
        }

        private int _BillThickness = 0;

        public int BillThickness
        {
            get { return _BillThickness; }
        }

        public void setBillThickness(double value)
        {
            _BillThickness = Convert.ToInt32(value / 0.1);
        }
    }

    public class F56PayParameter
    {
        public F56PayParameter()
        {

        }

        public F56PayParameter(int iNumeroCassette)
        {
            _NumeroCassette = iNumeroCassette;
            _Cantidad = 0;
        }

        public F56PayParameter(int iNumeroCassette, int iDenominacion, int iCantidad)
        {
            _Denominacion = iDenominacion;
            _NumeroCassette = iNumeroCassette;
            _Cantidad = iCantidad;
        }

        private int _NumeroCassette = 0;

        public int NumeroCassette
        {
            get { return _NumeroCassette; }
            set { _NumeroCassette = value; }
        }

        private int _Denominacion = 0;

        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        private int _Cantidad = 0;

        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }
    }
}
