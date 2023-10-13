using Ds.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ds.CashPaymentDevice
{

    public class F56Device
    {
        private SerialPort _ComPort = new SerialPort();
        private string _Puerto = string.Empty;
        public string Puerto
        {
            get { return _Puerto; }
            set { _Puerto = value; }
        }
        public EventHandler DeviceMessageSerialPort;
        public EventHandler DeviceMessageF56State;
        private List<F56Bill> _BillSizes = new List<F56Bill>();
        public List<F56Bill> BillSizes
        {
            get { return _BillSizes; }
            set { _BillSizes = value; }
        }
        private F56ShutterAction _ShutterAction = F56ShutterAction.Both;
        public F56ShutterAction ShutterAction
        {
            get { return _ShutterAction; }
            set { _ShutterAction = value; }
        }
        private StatesF56Device _CurrentState = StatesF56Device.Nothing;
        public StatesF56Device CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                EventArgsF56EventsDevice evento = new EventArgsF56EventsDevice(value, lstDispensar, lstRechazar, _ValorAdicional);
                DeviceMessageF56State(this, evento);
            }
        }

        private List<F56PayParameter> lstDispensar = new List<F56PayParameter>();
        private List<F56PayParameter> lstRechazar = new List<F56PayParameter>();
        private List<F56PayParameter> lstReferencia = new List<F56PayParameter>();

        #region VariablesPagar
        private List<List<F56PayParameter>> lstDispensarMultiple = new List<List<F56PayParameter>>();
        List<List<F56PayParameter>> _AcumuladoPagos = new List<List<F56PayParameter>>();
        private Int64 _ValorAdicional = 0;
        private int _IndiceDispensar = 0;
        List<F56PayParameter> t = new List<F56PayParameter>
        {
                new F56PayParameter(1),
                new F56PayParameter(2),
                new F56PayParameter(3),
                new F56PayParameter(4),
                new F56PayParameter(5),
                new F56PayParameter(6),
        };
        private bool _PartialDispense = false;
        private F56Style stiloF56 = F56Style.Front;
        #endregion

        public F56Device()
        {

        }

        public bool Conectar(List<F56Bill> lsBillsSizes, F56ShutterAction shutterAction, bool forcePay)
        {
            CurrentState = StatesF56Device.Nothing;
            _ComPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
            BillSizes = lsBillsSizes;
            ShutterAction = shutterAction;

            byte[] comBuffer = new byte[2];

            bool bAbrePuerto = false;

            for (int i = 1; i < 10; i++)
            {
                if (OpenPort("COM" + i))
                {
                    try
                    {
                        ENQ();
                        Thread.Sleep(200);
                        _ComPort.Read(comBuffer, 0, 2);
                        _ComPort.DiscardInBuffer();
                        if (string.Format("{0:X02}", (byte)comBuffer[0]) == "10" && string.Format("{0:X02}", (byte)comBuffer[1]) == "06")
                        {
                            bAbrePuerto = true;
                            _Puerto = "COM" + i;
                            _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            Thread.Sleep(500);
                            StatusInformation();
                            break;
                        }
                        else
                        {
                            bAbrePuerto = false;
                            ClosePort();
                        }
                    }
                    catch (Exception e)
                    {
                        bAbrePuerto = false;
                        ClosePort();
                    }
                }
                else
                {
                    bAbrePuerto = false;
                }
            }

            return bAbrePuerto;
        }
        public bool Desconectar()
        {
            bool bCierraPuerto = false;

            if (ClosePort())
            {
                bCierraPuerto = true;
            }
            else
            {
                bCierraPuerto = false;
            }

            return bCierraPuerto;
        }
        public void Pagar(List<F56PayParameter> listaPagar, Int64 valorAPagar, F56Style estiloDispensado)
        {
            stiloF56 = estiloDispensado;
            _PartialDispense = false;
            _ValorAdicional = 0;
            _IndiceDispensar = 0;
            lstReferencia = listaPagar;
            int lastLista = listaPagar.Count - 1;

            for (int i = listaPagar.Count; i < 6; i++)
            {
                F56PayParameter parametro = new F56PayParameter(i + 1, listaPagar[lastLista].Denominacion, 0);
                listaPagar.Add(parametro);
            }

            int j = 0;
            foreach (var item in t)
            {
                item.Denominacion = listaPagar[j].Denominacion;
                j++;
            }

            bool falta = AntesDeCalcular(valorAPagar, listaPagar);

            _ValorAdicional = valorAPagar
                - ((t[0].Cantidad * Convert.ToInt64(t[0].Denominacion))
                + (t[1].Cantidad * Convert.ToInt64(t[1].Denominacion))
                + (t[2].Cantidad * Convert.ToInt64(t[2].Denominacion))
                + (t[3].Cantidad * Convert.ToInt64(t[3].Denominacion))
                + (t[4].Cantidad * Convert.ToInt64(t[4].Denominacion))
                + (t[5].Cantidad * Convert.ToInt64(t[5].Denominacion)));

            FormarGruposPago(t);


            DispensarBilletesMultiple(_AcumuladoPagos);

        }
        public List<List<F56PayParameter>> CantidadPagar(List<F56PayParameter> listaPagar, Int64 valorAPagar, F56Style estiloDispensado)
        {

            stiloF56 = estiloDispensado;
            _PartialDispense = false;
            _ValorAdicional = 0;
            _IndiceDispensar = 0;
            lstReferencia = listaPagar;
            int lastLista = listaPagar.Count - 1;

            for (int i = listaPagar.Count; i < 6; i++)
            {
                F56PayParameter parametro = new F56PayParameter(i + 1, listaPagar[lastLista].Denominacion, 0);
                listaPagar.Add(parametro);
            }

            int j = 0;
            foreach (var item in t)
            {
                item.Denominacion = listaPagar[j].Denominacion;
                j++;
            }

            bool falta = AntesDeCalcular(valorAPagar, listaPagar);

            _ValorAdicional = valorAPagar
                - ((t[0].Cantidad * Convert.ToInt64(t[0].Denominacion))
                + (t[1].Cantidad * Convert.ToInt64(t[1].Denominacion))
                + (t[2].Cantidad * Convert.ToInt64(t[2].Denominacion))
                + (t[3].Cantidad * Convert.ToInt64(t[3].Denominacion))
                + (t[4].Cantidad * Convert.ToInt64(t[4].Denominacion))
                + (t[5].Cantidad * Convert.ToInt64(t[5].Denominacion)));

            FormarGruposPago(t);

            List<List<F56PayParameter>> list = (List<List<F56PayParameter>>)_AcumuladoPagos;

            return list;
        }

        #region NuevoCalculo
        private bool AntesDeCalcular(Int64 cantidad, List<F56PayParameter> listaPagar)
        {
            List<List<F56PayParameter>> lstTodasLasCombinaciones = new List<List<F56PayParameter>>();

            bool parcial = true;
            List<F56PayParameter> listaPagarOriginal = new List<F56PayParameter>(listaPagar);
            List<F56PayParameter> listaPagar2 = new List<F56PayParameter>(listaPagar);

            List<List<F56PayParameter>> ListaProbarFinal = GetCombination(listaPagar);

            foreach (var item in ListaProbarFinal)
            {
                listaPagar2 = new List<F56PayParameter>(listaPagar);
                tToZero();
                Calcular(cantidad, item);
                var aderir2 = t.Select(o =>
                        new F56PayParameter
                        {
                            NumeroCassette = o.NumeroCassette,
                            Denominacion = o.Denominacion,
                            Cantidad = o.Cantidad
                        }).ToList();
                lstTodasLasCombinaciones.Add(aderir2);
            }

            Int64 maximoFinal = 0;
            int cantidadBilletesFinal = 10000;
            foreach (var item in lstTodasLasCombinaciones)
            {
                Int64 maximoParcial = 0;
                int cantidadBilletesParcial = 0;
                foreach (var item2 in item)
                {
                    maximoParcial += item2.Cantidad * item2.Denominacion;
                    cantidadBilletesParcial += item2.Cantidad;
                }

                if (maximoFinal <= maximoParcial)
                {
                    maximoFinal = maximoParcial;
                    cantidadBilletesFinal = cantidadBilletesParcial;
                    t = new List<F56PayParameter>(item);
                }
            }

            return parcial;
        }
        private void tToZero()
        {
            foreach (var item in t)
            {
                item.Cantidad = 0;
            }
        }
        private List<List<F56PayParameter>> GetCombination(List<F56PayParameter> list)
        {
            List<List<F56PayParameter>> ListaProbarFinal = new List<List<F56PayParameter>>();
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                List<F56PayParameter> tem = new List<F56PayParameter>();
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        tem.Add(new F56PayParameter
                        {
                            NumeroCassette = list[j].NumeroCassette,
                            Denominacion = list[j].Denominacion,
                            Cantidad = list[j].Cantidad
                        });
                        //Console.Write(list[j].Denominacion);
                    }
                    //Console.Write("-");
                }
                ListaProbarFinal.Add(tem);
                //Console.WriteLine();
            }
            return ListaProbarFinal;
        }
        private Int64 Calcular(Int64 cantidad, List<F56PayParameter> listaPagar)
        {
            if (cantidad == 0)
            {
                return cantidad;
            }
            else if (listaPagar.Count == 1 && cantidad < listaPagar[listaPagar.Count - 1].Denominacion)
            {
                return cantidad;
            }
            else if (cantidad >= listaPagar[listaPagar.Count - 1].Denominacion && listaPagar[listaPagar.Count - 1].Cantidad > 0)
            {
                cantidad = cantidad - listaPagar[listaPagar.Count - 1].Denominacion;
                listaPagar[listaPagar.Count - 1].Cantidad = listaPagar[listaPagar.Count - 1].Cantidad - 1;

                t.Find(x => x.NumeroCassette == listaPagar[listaPagar.Count - 1].NumeroCassette).Cantidad++;

                return Calcular(cantidad, listaPagar);
            }
            else if (listaPagar.Count == 1)
            {
                return cantidad;
            }
            else
            {
                listaPagar.RemoveAt(listaPagar.Count - 1);
                return Calcular(cantidad, listaPagar);
            }
        }
        #endregion

        private void DispensarBilletesMultiple(List<List<F56PayParameter>> listaDispensar)
        {
            lstDispensar = new List<F56PayParameter>();
            lstRechazar = new List<F56PayParameter>();
            CurrentState = StatesF56Device.InicializingDispensing;
            lstDispensarMultiple = listaDispensar;
            ENQ();
        }
        //private bool calcular2(Int64 cantidad, List<F56PayParameter> listaPagar)
        //{

        //    int multi = (listaPagar[1].Denominacion / listaPagar[0].Denominacion) * listaPagar[0].Denominacion + listaPagar[0].Denominacion - listaPagar[1].Denominacion;
        //    int multi2 = (listaPagar[2].Denominacion / listaPagar[1].Denominacion) * listaPagar[1].Denominacion + listaPagar[1].Denominacion - listaPagar[2].Denominacion;
        //    int multi3 = (listaPagar[3].Denominacion / listaPagar[2].Denominacion) * listaPagar[2].Denominacion + listaPagar[2].Denominacion - listaPagar[3].Denominacion;
        //    int multi4 = (listaPagar[4].Denominacion / listaPagar[3].Denominacion) * listaPagar[3].Denominacion + listaPagar[3].Denominacion - listaPagar[4].Denominacion;
        //    int multi5 = (listaPagar[5].Denominacion / listaPagar[4].Denominacion) * listaPagar[4].Denominacion + listaPagar[4].Denominacion - listaPagar[5].Denominacion;
        //    Int64 mC = cantidad
        //        - ((t[0].Cantidad * Convert.ToInt64(listaPagar[0].Denominacion))
        //        + (t[1].Cantidad * Convert.ToInt64(listaPagar[1].Denominacion))
        //        + (t[2].Cantidad * Convert.ToInt64(listaPagar[2].Denominacion))
        //        + (t[3].Cantidad * Convert.ToInt64(listaPagar[3].Denominacion))
        //        + (t[4].Cantidad * Convert.ToInt64(listaPagar[4].Denominacion))
        //        + (t[5].Cantidad * Convert.ToInt64(listaPagar[5].Denominacion)));
        //    if (mC == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (mC >= listaPagar[5].Denominacion && listaPagar[5].Cantidad > 0)
        //        {
        //            listaPagar[5].Cantidad--;
        //            t[5].Cantidad++;
        //            mC = cantidad - (t[0].Cantidad * listaPagar[0].Denominacion + t[1].Cantidad * listaPagar[1].Denominacion + t[2].Cantidad * listaPagar[2].Denominacion + t[3].Cantidad * listaPagar[3].Denominacion + t[4].Cantidad * listaPagar[4].Denominacion + t[5].Cantidad * listaPagar[5].Denominacion);
        //            if (mC < multi5 || mC >= listaPagar[5].Denominacion)
        //            {
        //                return calcular2(cantidad, listaPagar);
        //            }
        //            else
        //            {
        //                t[5].Cantidad--;
        //                t[4].Cantidad++;
        //                listaPagar[4].Cantidad--;
        //                listaPagar[5].Cantidad++;
        //                return calcular2(cantidad, listaPagar);
        //            }
        //        }
        //        else if (mC >= listaPagar[4].Denominacion && listaPagar[4].Cantidad > 0)
        //        {
        //            listaPagar[4].Cantidad--;
        //            t[4].Cantidad++;
        //            mC = cantidad - (t[0].Cantidad * listaPagar[0].Denominacion + t[1].Cantidad * listaPagar[1].Denominacion + t[2].Cantidad * listaPagar[2].Denominacion + t[3].Cantidad * listaPagar[3].Denominacion + t[4].Cantidad * listaPagar[4].Denominacion + t[5].Cantidad * listaPagar[5].Denominacion);
        //            if (mC < multi4 || mC >= listaPagar[2].Denominacion)
        //            {
        //                return calcular2(cantidad, listaPagar);
        //            }
        //            else
        //            {
        //                t[4].Cantidad--;
        //                t[3].Cantidad++;
        //                listaPagar[3].Cantidad--;
        //                listaPagar[4].Cantidad++;
        //                return calcular2(cantidad, listaPagar);
        //            }
        //        }
        //        else if (mC >= listaPagar[3].Denominacion && listaPagar[3].Cantidad > 0)
        //        {
        //            listaPagar[3].Cantidad--;
        //            t[3].Cantidad++;
        //            mC = cantidad - (t[0].Cantidad * listaPagar[0].Denominacion + t[1].Cantidad * listaPagar[1].Denominacion + t[2].Cantidad * listaPagar[2].Denominacion + t[3].Cantidad * listaPagar[3].Denominacion + t[4].Cantidad * listaPagar[4].Denominacion + t[5].Cantidad * listaPagar[5].Denominacion);
        //            if (mC < multi3 || mC >= listaPagar[2].Denominacion)
        //            {
        //                return calcular2(cantidad, listaPagar);
        //            }
        //            else
        //            {
        //                t[3].Cantidad--;
        //                t[2].Cantidad++;
        //                listaPagar[2].Cantidad--;
        //                listaPagar[3].Cantidad++;
        //                return calcular2(cantidad, listaPagar);
        //            }
        //        }
        //        else if (mC >= listaPagar[2].Denominacion && listaPagar[2].Cantidad > 0)
        //        {
        //            listaPagar[2].Cantidad--;
        //            t[2].Cantidad++;
        //            mC = cantidad - (t[0].Cantidad * listaPagar[0].Denominacion + t[1].Cantidad * listaPagar[1].Denominacion + t[2].Cantidad * listaPagar[2].Denominacion + t[3].Cantidad * listaPagar[3].Denominacion + t[4].Cantidad * listaPagar[4].Denominacion + t[5].Cantidad * listaPagar[5].Denominacion);
        //            if (mC < multi2 || mC >= listaPagar[2].Denominacion)
        //            {
        //                return calcular2(cantidad, listaPagar);
        //            }
        //            else
        //            {
        //                t[2].Cantidad--;
        //                t[1].Cantidad++;
        //                listaPagar[1].Cantidad--;
        //                listaPagar[2].Cantidad++;
        //                return calcular2(cantidad, listaPagar);
        //            }
        //        }
        //        else if (mC >= listaPagar[1].Denominacion && listaPagar[1].Cantidad > 0)
        //        {
        //            listaPagar[1].Cantidad--;
        //            t[1].Cantidad++;
        //            return calcular2(cantidad, listaPagar);
        //        }
        //        else if (mC >= listaPagar[0].Denominacion && listaPagar[0].Cantidad > 0)
        //        {
        //            listaPagar[0].Cantidad--;
        //            t[0].Cantidad++;
        //            return calcular2(cantidad, listaPagar);
        //        }
        //        else
        //        {
        //            if (mC >= multi)
        //            {
        //                if (t[1].Cantidad > 0 && listaPagar[0].Cantidad > 0)
        //                {
        //                    t[1].Cantidad--;
        //                    t[0].Cantidad++;
        //                    listaPagar[0].Cantidad--;
        //                    listaPagar[1].Cantidad++;
        //                    return calcular2(cantidad, listaPagar);
        //                }
        //                else if (t[2].Cantidad > 0 && listaPagar[1].Cantidad > 0)
        //                {
        //                    t[2].Cantidad--;
        //                    t[1].Cantidad++;
        //                    listaPagar[1].Cantidad--;
        //                    listaPagar[2].Cantidad++;
        //                    return calcular2(cantidad, listaPagar);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //}
        //private bool calcular(Int64 cantidad, List<F56PayParameter> listaPagar)
        //{

        //    Int64 mC = cantidad
        //        - ((t[0].CassetteQuantity * Convert.ToInt64(listaPagar[0].Denominacion))
        //        + (t[1].CassetteQuantity * Convert.ToInt64(listaPagar[1].Denominacion))
        //        + (t[2].CassetteQuantity * Convert.ToInt64(listaPagar[2].Denominacion))
        //        + (t[3].CassetteQuantity * Convert.ToInt64(listaPagar[3].Denominacion))
        //        + (t[4].CassetteQuantity * Convert.ToInt64(listaPagar[4].Denominacion))
        //        + (t[5].CassetteQuantity * Convert.ToInt64(listaPagar[5].Denominacion)));

        //    if (mC == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (mC >= Convert.ToInt32(listaPagar[3].Denominacion) && listaPagar[3].Cantidad > 0)
        //        {
        //            listaPagar[3].Cantidad--;
        //            t[3].CassetteQuantity++;
        //            mC = cantidad - (t[0].CassetteQuantity * Convert.ToInt64(listaPagar[0].Denominacion) + t[1].CassetteQuantity * Convert.ToInt64(listaPagar[1].Denominacion) + t[2].CassetteQuantity * Convert.ToInt64(listaPagar[2].Denominacion) + t[3].CassetteQuantity * Convert.ToInt64(listaPagar[3].Denominacion));
        //            if (mC < ((Convert.ToInt32(listaPagar[3].Denominacion) / Convert.ToInt32(listaPagar[2].Denominacion)) * Convert.ToInt32(listaPagar[2].Denominacion) + Convert.ToInt32(listaPagar[2].Denominacion) - Convert.ToInt32(listaPagar[1].Denominacion)) || mC >= Convert.ToInt32(listaPagar[3].Denominacion))
        //            {
        //                calcular(cantidad, listaPagar);
        //            }
        //            else
        //            {
        //                t[3].CassetteQuantity--;
        //                t[2].CassetteQuantity++;
        //                listaPagar[2].Cantidad--;
        //                listaPagar[3].Cantidad++;
        //                calcular(cantidad, listaPagar);
        //            }
        //        }
        //        else if (mC >= Convert.ToInt32(listaPagar[2].Denominacion) && listaPagar[2].Cantidad > 0)
        //        {
        //            listaPagar[2].Cantidad--;
        //            t[2].CassetteQuantity++;
        //            calcular(cantidad, listaPagar);
        //        }
        //        else if (mC >= Convert.ToInt32(listaPagar[1].Denominacion) && listaPagar[1].Cantidad > 0)
        //        {
        //            listaPagar[1].Cantidad--;
        //            t[1].CassetteQuantity++;
        //            calcular(cantidad, listaPagar);
        //        }
        //        else if (mC >= Convert.ToInt32(listaPagar[0].Denominacion) && listaPagar[0].Cantidad > 0)
        //        {
        //            listaPagar[0].Cantidad--;
        //            t[0].CassetteQuantity++;
        //            calcular(cantidad, listaPagar);
        //        }
        //        else
        //        {
        //            //      cout<<mC;
        //            if (mC >= ((Convert.ToInt32(listaPagar[2].Denominacion) / Convert.ToInt32(listaPagar[1].Denominacion)) * Convert.ToInt32(listaPagar[1].Denominacion + Convert.ToInt32(listaPagar[1].Denominacion) - Convert.ToInt32(listaPagar[2].Denominacion))))
        //            {
        //                if (t[1].CassetteQuantity > 0 && listaPagar[0].Cantidad > 0)
        //                {
        //                    t[1].CassetteQuantity--;
        //                    t[0].CassetteQuantity++;
        //                    listaPagar[0].Cantidad--;
        //                    listaPagar[1].Cantidad++;
        //                    calcular(cantidad, listaPagar);
        //                }
        //                else if (t[2].CassetteQuantity > 0 && listaPagar[1].Cantidad > 0)
        //                {
        //                    t[2].CassetteQuantity--;
        //                    t[1].CassetteQuantity++;
        //                    listaPagar[1].Cantidad--;
        //                    listaPagar[2].Cantidad++;
        //                    calcular(cantidad, listaPagar);
        //                }
        //                else if (t[3].CassetteQuantity > 0 && listaPagar[2].Cantidad > 0)
        //                {
        //                    t[3].CassetteQuantity--;
        //                    t[2].CassetteQuantity++;
        //                    listaPagar[2].Cantidad--;
        //                    listaPagar[3].Cantidad++;
        //                    calcular(cantidad, listaPagar);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return false;
        //}
        private void FormarGruposPago(List<F56PayParameter> lsPagos)
        {
            _AcumuladoPagos = new List<List<F56PayParameter>>();
            List<F56PayParameter> _Pagos = new List<F56PayParameter>
            {
                new F56PayParameter(1),
                new F56PayParameter(2),
                new F56PayParameter(3),
                new F56PayParameter(4),
                new F56PayParameter(5),
                new F56PayParameter(6),
            };

            int i = 0;
            int suma = 0;
            while (i <= 6)
            {
                if (i == 6)
                {
                    _AcumuladoPagos.Add(_Pagos);
                    i++;
                }
                else
                {
                    int posibleInsertar = 20 - suma;
                    if (posibleInsertar != 0)
                    {
                        if (lsPagos[i].Cantidad <= posibleInsertar)
                        {
                            _Pagos[i].Cantidad = lsPagos[i].Cantidad;
                            suma += lsPagos[i].Cantidad;
                            lsPagos[i].Cantidad = 0;
                            i++;
                        }
                        else
                        {
                            //int insertar = lsPagos[i] - posibleInsertar;
                            _Pagos[i].Cantidad = posibleInsertar;
                            lsPagos[i].Cantidad -= posibleInsertar;
                            _AcumuladoPagos.Add(_Pagos);
                            _Pagos = new List<F56PayParameter>
                            {
                                new F56PayParameter(1),
                                new F56PayParameter(2),
                                new F56PayParameter(3),
                                new F56PayParameter(4),
                                new F56PayParameter(5),
                                new F56PayParameter(6),
                            };
                            suma = 0;
                        }
                    }
                    else
                    {
                        _AcumuladoPagos.Add(_Pagos);
                        suma = 0;
                        _Pagos = new List<F56PayParameter>
                        {
                            new F56PayParameter(1),
                            new F56PayParameter(2),
                            new F56PayParameter(3),
                            new F56PayParameter(4),
                            new F56PayParameter(5),
                            new F56PayParameter(6),
                        };
                    }
                }
            }
        }
        private void Transportar()
        {
            CurrentState = StatesF56Device.InicializingTransporting;
            ENQ();
        }

        private int CalcularDenominacion(int numerocass)
        {
            int cassette = 0;
            foreach (F56PayParameter item in lstReferencia)
            {
                if (item.NumeroCassette == numerocass)
                {
                    return item.Denominacion;
                }
            }
            return cassette;
        }

        private void Actuar(byte[] recibo)
        {
            lock (this)
            {
                if (recibo.Length >= 2)
                {
                    string recepcion = string.Format("{0:X02}", (byte)recibo[0]) + string.Format("{0:X02}", (byte)recibo[1]);

                    switch (recepcion)
                    {
                        #region ACK
                        case ComandosRecepcionBasicoF56Device.ACK:
                            if (recibo.Length >= 4)
                            {
                                recepcion = string.Format("{0:X02}", (byte)recibo[2]) + string.Format("{0:X02}", (byte)recibo[3]);
                                if (recepcion == ComandosRecepcionBasicoF56Device.ENQ)
                                {
                                    ACK();
                                }
                                else
                                {
                                    //Caso desconocido
                                }
                            }
                            else
                            {
                                if (CurrentState == StatesF56Device.Nothing)
                                {
                                    CurrentState = StatesF56Device.Inicializing;
                                    Initialization();
                                }
                                else if (CurrentState == StatesF56Device.EndInicialization)
                                {
                                    StatusInformation();
                                }
                                else if (CurrentState == StatesF56Device.WaitingRemoveBills)
                                {
                                    StatusInformation();
                                }
                                else if (CurrentState == StatesF56Device.EndDispense)
                                {
                                    StatusInformation();
                                }
                                else if (CurrentState == StatesF56Device.EndWaitingRemoveBills)
                                {
                                    CurrentState = StatesF56Device.ClosingShutter;
                                    DemandBillTransportationCloseFrontShutter();
                                }
                                else if (CurrentState == StatesF56Device.EndTransport)
                                {
                                    StatusInformation();
                                }
                                else if (CurrentState == StatesF56Device.InicializingDispensing)
                                {
                                    CurrentState = StatesF56Device.Dispensing;
                                    if (stiloF56 == F56Style.Rear)
                                    {
                                        BillCountAndTransportRear();
                                    }
                                    else
                                    {
                                        BillCountAndTransportFront();
                                    }
                                }
                                else if (CurrentState == StatesF56Device.InicializingTransporting)
                                {
                                    CurrentState = StatesF56Device.Transporting;
                                    DemandBillTransportation();
                                }
                            }
                            break;
                        #endregion
                        #region STX
                        case ComandosRecepcionBasicoF56Device.STX:
                            ACK();
                            string DH0 = string.Format("{0:X02}", (byte)recibo[4]);
                            string DH1 = string.Format("{0:X02}", (byte)recibo[5]);
                            string DH2 = string.Format("{0:X02}", (byte)recibo[6]);
                            string DH3 = string.Format("{0:X02}", (byte)recibo[7]);
                            switch (DH0)
                            {
                                case ComandosRecepcionDH0F56Device.Positive:
                                    switch (DH1)
                                    {
                                        #region DeviceStatusInfo
                                        case "01":
                                            if (CurrentState == StatesF56Device.Nothing)
                                            {
                                                ENQ();
                                            }
                                            else if (CurrentState == StatesF56Device.EndInicialization)
                                            {
                                                CurrentState = StatesF56Device.Disable;
                                            }
                                            else if (CurrentState == StatesF56Device.EndTransport)
                                            {
                                                CurrentState = StatesF56Device.Disable;
                                            }
                                            else if (CurrentState == StatesF56Device.WaitingRemoveBills)
                                            {
                                                if (string.Format("{0:X02}", (byte)recibo[20]) == "08")
                                                {
                                                    ENQ();
                                                }
                                                else if (string.Format("{0:X02}", (byte)recibo[20]) == "10")
                                                {
                                                    ENQ();
                                                }
                                                else if (string.Format("{0:X02}", (byte)recibo[20]) == "00")
                                                {
                                                    CurrentState = StatesF56Device.EndWaitingRemoveBills;
                                                    ENQ();
                                                }
                                            }
                                            else if (CurrentState == StatesF56Device.EndDispense)
                                            {
                                                CurrentState = StatesF56Device.Disable;
                                            }
                                            break;
                                        #endregion
                                        #region NormalDeviceInicialization
                                        case "02":
                                            if (CurrentState == StatesF56Device.Inicializing)
                                            {
                                                CurrentState = StatesF56Device.EndInicialization;
                                                ENQ();
                                            }
                                            break;
                                        #endregion
                                        #region NormalBillCount
                                        case "03":
                                            break;
                                        #endregion
                                        #region NormalBillTransportation
                                        case "05":
                                            if (CurrentState == StatesF56Device.ClosingShutter)
                                            {
                                                if (_AcumuladoPagos.Count > 0)
                                                {
                                                    _IndiceDispensar++;
                                                    if (_IndiceDispensar < _AcumuladoPagos.Count)
                                                    {
                                                        CurrentState = StatesF56Device.InicializingDispensing;
                                                        ENQ();
                                                    }
                                                    else
                                                    {
                                                        if (_PartialDispense)
                                                        {
                                                            CurrentState = StatesF56Device.EndPartialDispense;
                                                        }
                                                        else
                                                        {
                                                            CurrentState = StatesF56Device.EndDispense;
                                                        }
                                                        ENQ();
                                                    }
                                                }
                                                else
                                                {
                                                    if (_PartialDispense)
                                                    {
                                                        CurrentState = StatesF56Device.EndPartialDispense;
                                                    }
                                                    else
                                                    {
                                                        CurrentState = StatesF56Device.EndDispense;
                                                    }
                                                    ENQ();
                                                }
                                            }
                                            else if (CurrentState == StatesF56Device.Transporting)
                                            {
                                                CurrentState = StatesF56Device.EndDispensePack;
                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                CurrentState = StatesF56Device.WaitingRemoveBills;
                                                ENQ();
                                            }
                                            break;
                                        #endregion
                                        #region NormalBillRetrival
                                        case "06":
                                            break;
                                        #endregion
                                        #region NormalBillCountTransportFront
                                        case "09":
                                            if (CurrentState == StatesF56Device.Dispensing)
                                            {
                                                string contados1 = string.Format("{0:X02}", (byte)recibo[94]) + string.Format("{0:X02}", (byte)recibo[95]);
                                                string contados2 = string.Format("{0:X02}", (byte)recibo[96]) + string.Format("{0:X02}", (byte)recibo[97]);
                                                string contados3 = string.Format("{0:X02}", (byte)recibo[98]) + string.Format("{0:X02}", (byte)recibo[99]);
                                                string contados4 = string.Format("{0:X02}", (byte)recibo[100]) + string.Format("{0:X02}", (byte)recibo[101]);
                                                string contados5 = string.Format("{0:X02}", (byte)recibo[211]) + string.Format("{0:X02}", (byte)recibo[212]);
                                                string contados6 = string.Format("{0:X02}", (byte)recibo[213]) + string.Format("{0:X02}", (byte)recibo[214]);
                                                string rechazados1 = string.Format("{0:X02}", (byte)recibo[102]) + string.Format("{0:X02}", (byte)recibo[103]);
                                                string rechazados2 = string.Format("{0:X02}", (byte)recibo[104]) + string.Format("{0:X02}", (byte)recibo[105]);
                                                string rechazados3 = string.Format("{0:X02}", (byte)recibo[106]) + string.Format("{0:X02}", (byte)recibo[107]);
                                                string rechazados4 = string.Format("{0:X02}", (byte)recibo[108]) + string.Format("{0:X02}", (byte)recibo[109]);
                                                string rechazados5 = string.Format("{0:X02}", (byte)recibo[219]) + string.Format("{0:X02}", (byte)recibo[220]);
                                                string rechazados6 = string.Format("{0:X02}", (byte)recibo[221]) + string.Format("{0:X02}", (byte)recibo[222]);

                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                lstDispensar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(contados1)));
                                                lstDispensar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(contados2)));
                                                lstDispensar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(contados3)));
                                                lstDispensar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(contados4)));
                                                lstDispensar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(contados5)));
                                                lstDispensar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(contados6)));
                                                lstRechazar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(rechazados1)));
                                                lstRechazar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(rechazados2)));
                                                lstRechazar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(rechazados3)));
                                                lstRechazar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(rechazados4)));
                                                lstRechazar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(rechazados5)));
                                                lstRechazar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(rechazados6)));
                                                CurrentState = StatesF56Device.EndDispensePack;
                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                CurrentState = StatesF56Device.WaitingRemoveBills;
                                                ENQ();
                                            }
                                            break;
                                        #endregion
                                        #region NormalBillCountTransportRear
                                        case "0A":
                                            if (CurrentState == StatesF56Device.Dispensing)
                                            {
                                                string contados1 = string.Format("{0:X02}", (byte)recibo[94]) + string.Format("{0:X02}", (byte)recibo[95]);
                                                string contados2 = string.Format("{0:X02}", (byte)recibo[96]) + string.Format("{0:X02}", (byte)recibo[97]);
                                                string contados3 = string.Format("{0:X02}", (byte)recibo[98]) + string.Format("{0:X02}", (byte)recibo[99]);
                                                string contados4 = string.Format("{0:X02}", (byte)recibo[100]) + string.Format("{0:X02}", (byte)recibo[101]);
                                                string contados5 = string.Format("{0:X02}", (byte)recibo[211]) + string.Format("{0:X02}", (byte)recibo[212]);
                                                string contados6 = string.Format("{0:X02}", (byte)recibo[213]) + string.Format("{0:X02}", (byte)recibo[214]);
                                                string rechazados1 = string.Format("{0:X02}", (byte)recibo[102]) + string.Format("{0:X02}", (byte)recibo[103]);
                                                string rechazados2 = string.Format("{0:X02}", (byte)recibo[104]) + string.Format("{0:X02}", (byte)recibo[105]);
                                                string rechazados3 = string.Format("{0:X02}", (byte)recibo[106]) + string.Format("{0:X02}", (byte)recibo[107]);
                                                string rechazados4 = string.Format("{0:X02}", (byte)recibo[108]) + string.Format("{0:X02}", (byte)recibo[109]);
                                                string rechazados5 = string.Format("{0:X02}", (byte)recibo[219]) + string.Format("{0:X02}", (byte)recibo[220]);
                                                string rechazados6 = string.Format("{0:X02}", (byte)recibo[221]) + string.Format("{0:X02}", (byte)recibo[222]);

                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                lstDispensar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(contados1)));
                                                lstDispensar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(contados2)));
                                                lstDispensar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(contados3)));
                                                lstDispensar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(contados4)));
                                                lstDispensar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(contados5)));
                                                lstDispensar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(contados6)));
                                                lstRechazar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(rechazados1)));
                                                lstRechazar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(rechazados2)));
                                                lstRechazar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(rechazados3)));
                                                lstRechazar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(rechazados4)));
                                                lstRechazar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(rechazados5)));
                                                lstRechazar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(rechazados6)));
                                                CurrentState = StatesF56Device.EndDispensePack;
                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                CurrentState = StatesF56Device.WaitingRemoveBills;
                                                ENQ();
                                            }
                                            break;
                                        #endregion
                                        case "0E":
                                            break;
                                        case "0F":
                                            break;
                                        case "10":
                                            break;
                                        case "11":
                                            break;
                                        case "12":
                                            break;
                                        case "13":
                                            break;
                                        case "14":
                                            break;
                                        case "D1":
                                            break;
                                        case "D2":
                                            break;
                                        case "D4":
                                            break;
                                    }
                                    break;
                                case ComandosRecepcionDH0F56Device.Negative:
                                    string ErrorCode1 = string.Format("{0:X02}", (byte)recibo[10]);
                                    string ErrorCode2 = string.Format("{0:X02}", (byte)recibo[11]);
                                    switch (DH1)
                                    {
                                        case "01":
                                            CurrentState = StatesF56Device.ErrorDeviceInformation;
                                            break;
                                        case "02":
                                            CurrentState = StatesF56Device.AbnormalInitialization;
                                            break;
                                        case "03":
                                            CurrentState = StatesF56Device.AbnormalBillCount;
                                            break;
                                        case "05":
                                            CurrentState = StatesF56Device.AbnormalBillTransportation;
                                            break;
                                        case "06":
                                            break;
                                        #region AbnormalBillCountTransportFront
                                        case "09":
                                            if (CurrentState == StatesF56Device.Dispensing)
                                            {
                                                string contados1 = string.Format("{0:X02}", (byte)recibo[94]) + string.Format("{0:X02}", (byte)recibo[95]);
                                                string contados2 = string.Format("{0:X02}", (byte)recibo[96]) + string.Format("{0:X02}", (byte)recibo[97]);
                                                string contados3 = string.Format("{0:X02}", (byte)recibo[98]) + string.Format("{0:X02}", (byte)recibo[99]);
                                                string contados4 = string.Format("{0:X02}", (byte)recibo[100]) + string.Format("{0:X02}", (byte)recibo[101]);
                                                string contados5 = string.Format("{0:X02}", (byte)recibo[211]) + string.Format("{0:X02}", (byte)recibo[212]);
                                                string contados6 = string.Format("{0:X02}", (byte)recibo[213]) + string.Format("{0:X02}", (byte)recibo[214]);
                                                string rechazados1 = string.Format("{0:X02}", (byte)recibo[102]) + string.Format("{0:X02}", (byte)recibo[103]);
                                                string rechazados2 = string.Format("{0:X02}", (byte)recibo[104]) + string.Format("{0:X02}", (byte)recibo[105]);
                                                string rechazados3 = string.Format("{0:X02}", (byte)recibo[106]) + string.Format("{0:X02}", (byte)recibo[107]);
                                                string rechazados4 = string.Format("{0:X02}", (byte)recibo[108]) + string.Format("{0:X02}", (byte)recibo[109]);
                                                string rechazados5 = string.Format("{0:X02}", (byte)recibo[219]) + string.Format("{0:X02}", (byte)recibo[220]);
                                                string rechazados6 = string.Format("{0:X02}", (byte)recibo[221]) + string.Format("{0:X02}", (byte)recibo[222]);

                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                lstDispensar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(contados1)));
                                                lstDispensar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(contados2)));
                                                lstDispensar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(contados3)));
                                                lstDispensar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(contados4)));
                                                lstDispensar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(contados5)));
                                                lstDispensar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(contados6)));
                                                lstRechazar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(rechazados1)));
                                                lstRechazar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(rechazados2)));
                                                lstRechazar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(rechazados3)));
                                                lstRechazar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(rechazados4)));
                                                lstRechazar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(rechazados5)));
                                                lstRechazar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(rechazados6)));

                                                if (HexF56NumberOfBillsValueToInt(contados1) + HexF56NumberOfBillsValueToInt(contados2) + HexF56NumberOfBillsValueToInt(contados3) + HexF56NumberOfBillsValueToInt(contados4) + HexF56NumberOfBillsValueToInt(contados5) + HexF56NumberOfBillsValueToInt(contados6) > 0)
                                                {
                                                    int i = 0;
                                                    foreach (var item in _AcumuladoPagos[_IndiceDispensar])
                                                    {
                                                        if (item.Cantidad > 0)
                                                        {
                                                            if (item.NumeroCassette == lstDispensar[i].NumeroCassette)
                                                            {
                                                                if (item.Cantidad > lstDispensar[i].Cantidad)
                                                                {
                                                                    _ValorAdicional += (CalcularDenominacion(item.NumeroCassette) * (item.Cantidad - lstDispensar[i].Cantidad));
                                                                }
                                                            }
                                                        }
                                                        i++;
                                                    }

                                                    _PartialDispense = true;
                                                    _IndiceDispensar = _AcumuladoPagos.Count;
                                                    Transportar();
                                                }
                                                else
                                                {
                                                    CurrentState = StatesF56Device.AbnormalBillCountAndTransportFront;
                                                    lstDispensar = new List<F56PayParameter>();
                                                    lstRechazar = new List<F56PayParameter>();
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region AbnormalBillCountTransportRear
                                        case "0A":
                                            if (CurrentState == StatesF56Device.Dispensing)
                                            {
                                                string contados1 = string.Format("{0:X02}", (byte)recibo[94]) + string.Format("{0:X02}", (byte)recibo[95]);
                                                string contados2 = string.Format("{0:X02}", (byte)recibo[96]) + string.Format("{0:X02}", (byte)recibo[97]);
                                                string contados3 = string.Format("{0:X02}", (byte)recibo[98]) + string.Format("{0:X02}", (byte)recibo[99]);
                                                string contados4 = string.Format("{0:X02}", (byte)recibo[100]) + string.Format("{0:X02}", (byte)recibo[101]);
                                                string contados5 = string.Format("{0:X02}", (byte)recibo[211]) + string.Format("{0:X02}", (byte)recibo[212]);
                                                string contados6 = string.Format("{0:X02}", (byte)recibo[213]) + string.Format("{0:X02}", (byte)recibo[214]);
                                                string rechazados1 = string.Format("{0:X02}", (byte)recibo[102]) + string.Format("{0:X02}", (byte)recibo[103]);
                                                string rechazados2 = string.Format("{0:X02}", (byte)recibo[104]) + string.Format("{0:X02}", (byte)recibo[105]);
                                                string rechazados3 = string.Format("{0:X02}", (byte)recibo[106]) + string.Format("{0:X02}", (byte)recibo[107]);
                                                string rechazados4 = string.Format("{0:X02}", (byte)recibo[108]) + string.Format("{0:X02}", (byte)recibo[109]);
                                                string rechazados5 = string.Format("{0:X02}", (byte)recibo[219]) + string.Format("{0:X02}", (byte)recibo[220]);
                                                string rechazados6 = string.Format("{0:X02}", (byte)recibo[221]) + string.Format("{0:X02}", (byte)recibo[222]);

                                                lstDispensar = new List<F56PayParameter>();
                                                lstRechazar = new List<F56PayParameter>();
                                                lstDispensar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(contados1)));
                                                lstDispensar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(contados2)));
                                                lstDispensar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(contados3)));
                                                lstDispensar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(contados4)));
                                                lstDispensar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(contados5)));
                                                lstDispensar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(contados6)));
                                                lstRechazar.Add(new F56PayParameter(1, CalcularDenominacion(1), HexF56NumberOfBillsValueToInt(rechazados1)));
                                                lstRechazar.Add(new F56PayParameter(2, CalcularDenominacion(2), HexF56NumberOfBillsValueToInt(rechazados2)));
                                                lstRechazar.Add(new F56PayParameter(3, CalcularDenominacion(3), HexF56NumberOfBillsValueToInt(rechazados3)));
                                                lstRechazar.Add(new F56PayParameter(4, CalcularDenominacion(4), HexF56NumberOfBillsValueToInt(rechazados4)));
                                                lstRechazar.Add(new F56PayParameter(5, CalcularDenominacion(5), HexF56NumberOfBillsValueToInt(rechazados5)));
                                                lstRechazar.Add(new F56PayParameter(6, CalcularDenominacion(6), HexF56NumberOfBillsValueToInt(rechazados6)));

                                                if (HexF56NumberOfBillsValueToInt(contados1) + HexF56NumberOfBillsValueToInt(contados2) + HexF56NumberOfBillsValueToInt(contados3) + HexF56NumberOfBillsValueToInt(contados4) + HexF56NumberOfBillsValueToInt(contados5) + HexF56NumberOfBillsValueToInt(contados6) > 0)
                                                {
                                                    int i = 0;
                                                    foreach (var item in _AcumuladoPagos[_IndiceDispensar])
                                                    {
                                                        if (item.Cantidad > 0)
                                                        {
                                                            if (item.NumeroCassette == lstDispensar[i].NumeroCassette)
                                                            {
                                                                if (item.Cantidad > lstDispensar[i].Cantidad)
                                                                {
                                                                    _ValorAdicional += (CalcularDenominacion(item.NumeroCassette) * (item.Cantidad - lstDispensar[i].Cantidad));
                                                                }
                                                            }
                                                        }
                                                        i++;
                                                    }

                                                    _PartialDispense = true;
                                                    _IndiceDispensar = _AcumuladoPagos.Count;
                                                    Transportar();
                                                }
                                                else
                                                {
                                                    CurrentState = StatesF56Device.AbnormalBillCountAndTransportRear;
                                                    lstDispensar = new List<F56PayParameter>();
                                                    lstRechazar = new List<F56PayParameter>();
                                                }
                                            }
                                            break;
                                        #endregion
                                        case "0E":
                                            break;
                                        case "0F":
                                            break;
                                        case "10":
                                            break;
                                        case "11":
                                            break;
                                        case "12":
                                            break;
                                        case "13":
                                            break;
                                        case "14":
                                            break;
                                        case "D1":
                                            break;
                                        case "D2":
                                            break;
                                        case "D4":
                                            break;
                                    }
                                    break;
                            }
                            break;
                        #endregion
                        #region ENQ
                        case ComandosRecepcionBasicoF56Device.ENQ:
                            ACK();
                            break;
                        #endregion
                        default:
                            //Caso error de recepcion
                            break;
                    }
                }
            }
        }

        #region F56DeviceCommands
        private void ENQ()
        {
            byte[] paquete = new byte[2];
            paquete[0] = StringToByteArray("10")[0];
            paquete[1] = StringToByteArray("05")[0];
            WriteData(paquete);
        }
        private void ACK()
        {
            byte[] paquete = new byte[2];
            paquete[0] = StringToByteArray("10")[0];
            paquete[1] = StringToByteArray("06")[0];
            WriteData(paquete);
        }
        private void StatusInformation()
        {
            byte[] paquete = new byte[14];
            byte[] paquetepre = new byte[2];

            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 8; //tamaño
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = 1;
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = 1;
            paquete[8] = 0;
            paquete[9] = StringToByteArray("1C")[0];
            paquete[10] = StringToByteArray("10")[0];
            paquete[11] = StringToByteArray("03")[0];
            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private void Initialization()
        {
            byte[] paquete = new byte[39];
            byte[] paquetepre = new byte[2];

            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 33;                                                           //Size
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = StringToByteArray("02")[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;                                                            //RSV
            paquete[6] = 0;
            paquete[7] = StringToByteArray("1A")[0];
            paquete[8] = 0;                                                            //ODR
            paquete[9] = StringToByteArray("0" + Convert.ToInt32(ShutterAction))[0];   //ODR

            int j = 0;
            for (int i = 0; i < BillSizes.Count; i++)
            {
                if (i < 4)
                {
                    paquete[j + 10] = StringToByteArray(BillSizes[i].BillLengthUp.ToString("X"))[0];
                    paquete[j + 11] = StringToByteArray(BillSizes[i].BillLengthDown.ToString("X"))[0];
                    string a = BillSizes[i].BillThickness.ToString("X");
                    if (a.Length == 1)
                    {
                        paquete[i + 18] = StringToByteArray("0" + a)[0];
                    }
                    else
                    {
                        paquete[i + 18] = StringToByteArray(a)[0];
                    }
                }
                else
                {
                    paquete[j + 22] = StringToByteArray(BillSizes[i].BillLengthUp.ToString("X"))[0];
                    paquete[j + 23] = StringToByteArray(BillSizes[i].BillLengthDown.ToString("X"))[0];
                    string a = BillSizes[i].BillThickness.ToString("X");
                    if (a.Length == 1)
                    {
                        paquete[i + 30 - 4] = StringToByteArray("0" + a)[0];
                    }
                    else
                    {
                        paquete[i + 30 - 4] = StringToByteArray(a)[0];
                    }
                }
                if (i == 3)
                {
                    j = 0;
                }
                else
                {
                    j = j + 2;
                }
            }


            //--
            //paquete[10] = StringToByteArray("92")[0];
            //paquete[11] = StringToByteArray("82")[0];
            //paquete[12] = 0;
            //paquete[13] = 0;
            //paquete[14] = 0;
            //paquete[15] = 0;
            //paquete[16] = 0;
            //paquete[17] = 0;
            //paquete[18] = StringToByteArray("0C")[0];
            //paquete[19] = 0;
            //paquete[20] = 0;
            //paquete[21] = 0;
            //paquete[22] = 0;
            //paquete[23] = 0;
            //paquete[24] = 0;
            //paquete[25] = 0;
            //paquete[26] = 0;
            //paquete[27] = 0;
            //paquete[28] = 0;
            //paquete[29] = 0;
            //paquete[30] = 0;
            //paquete[31] = 0;
            //paquete[32] = 0;
            //paquete[33] = 0;
            //--

            paquete[34] = StringToByteArray("1C")[0];
            paquete[35] = StringToByteArray("10")[0];
            paquete[36] = StringToByteArray("03")[0];
            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private void DemandBillTransportationCloseFrontShutter()
        {
            byte[] paquete = new byte[14];
            byte[] paquetepre = new byte[2];

            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 8;
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = StringToByteArray("05")[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = 1;
            if (stiloF56 == F56Style.Rear)
            {
                paquete[8] = StringToByteArray("82")[0];
            }
            else
            {
                paquete[8] = 2;
            }
            paquete[9] = StringToByteArray("1C")[0];
            paquete[10] = StringToByteArray("10")[0];
            paquete[11] = StringToByteArray("03")[0];
            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private void BillCountAndTransportFront()
        {
            byte[] paquete = new byte[57];
            byte[] paquetepre = new byte[2];
            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 51;
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = StringToByteArray("09")[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = StringToByteArray("2C")[0];

            //Orden de conteo->Cassette menor a mayor
            paquete[8] = StringToByteArray("FE")[0];
            paquete[9] = StringToByteArray("DC")[0];
            paquete[10] = StringToByteArray("BA")[0];
            paquete[11] = StringToByteArray("98")[0];

            //Valores a dispensar - > limpiar valores
            paquete[12] = StringToByteArray("30")[0];
            paquete[13] = StringToByteArray("30")[0];
            paquete[14] = StringToByteArray("30")[0];//
            paquete[15] = StringToByteArray("30")[0];
            paquete[16] = StringToByteArray("30")[0];//
            paquete[17] = StringToByteArray("30")[0];
            paquete[18] = StringToByteArray("30")[0];//
            paquete[19] = StringToByteArray("30")[0];
            paquete[20] = StringToByteArray("30")[0];
            paquete[21] = StringToByteArray("30")[0];
            paquete[22] = StringToByteArray("30")[0];
            paquete[23] = StringToByteArray("30")[0];
            paquete[24] = StringToByteArray("30")[0];
            paquete[25] = StringToByteArray("30")[0];
            paquete[26] = StringToByteArray("30")[0];
            paquete[27] = StringToByteArray("30")[0];
            paquete[28] = 0;
            paquete[29] = 0;
            paquete[30] = 0;
            paquete[31] = 0;

            paquete[32] = StringToByteArray("30")[0];
            paquete[33] = StringToByteArray("30")[0];
            paquete[34] = StringToByteArray("30")[0];
            paquete[35] = StringToByteArray("30")[0];
            paquete[36] = StringToByteArray("30")[0];
            paquete[37] = StringToByteArray("30")[0];
            paquete[38] = StringToByteArray("30")[0];
            paquete[39] = StringToByteArray("30")[0];
            paquete[40] = StringToByteArray("30")[0];
            paquete[41] = StringToByteArray("30")[0];
            paquete[42] = StringToByteArray("30")[0];
            paquete[43] = StringToByteArray("30")[0];
            paquete[44] = StringToByteArray("30")[0];
            paquete[45] = StringToByteArray("30")[0];
            paquete[46] = StringToByteArray("30")[0];
            paquete[47] = StringToByteArray("30")[0];
            paquete[48] = 0;
            paquete[49] = 0;
            paquete[50] = 0;
            paquete[51] = 0;
            //



            for (int i = 0; i < lstDispensarMultiple[_IndiceDispensar].Count; i++)
            {

                int adicional = 0;
                switch (lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette)
                {
                    case 1:
                        adicional = 0;
                        break;
                    case 2:
                        adicional = 1;
                        break;
                    case 3:
                        adicional = 2;
                        break;
                    case 4:
                        adicional = 3;
                        break;
                    case 5:
                        adicional = 0;
                        break;
                    case 6:
                        adicional = 1;
                        break;
                    case 7:
                        adicional = 2;
                        break;
                    case 8:
                        adicional = 3;
                        break;
                }

                if (lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette <= 4)
                {
                    string cantidadBilletes = IntToHexF56NumberOfBillsValue(lstDispensarMultiple[_IndiceDispensar][i].Cantidad);
                    paquete[adicional + 11 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[0];
                    paquete[adicional + 12 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[1];
                    paquete[adicional + 19 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("B1")[0];
                    paquete[adicional + 20 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("30")[0];
                }
                else
                {
                    string cantidadBilletes = IntToHexF56NumberOfBillsValue(lstDispensarMultiple[_IndiceDispensar][i].Cantidad);
                    paquete[adicional + 27 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[0];
                    paquete[adicional + 28 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[1];
                    paquete[adicional + 35 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("B1")[0];
                    paquete[adicional + 36 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("30")[0];
                }

            }

            paquete[28] = StringToByteArray("0A")[0];
            paquete[29] = StringToByteArray("0A")[0];
            paquete[30] = StringToByteArray("0A")[0];
            paquete[31] = StringToByteArray("0A")[0];
            paquete[48] = StringToByteArray("0A")[0];
            paquete[49] = StringToByteArray("0A")[0];
            paquete[50] = StringToByteArray("0A")[0];
            paquete[51] = StringToByteArray("0A")[0];

            //Fin
            paquete[52] = StringToByteArray("1C")[0];
            paquete[53] = StringToByteArray("10")[0];
            paquete[54] = StringToByteArray("03")[0];

            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private void BillCountAndTransportRear()
        {
            byte[] paquete = new byte[57];
            byte[] paquetepre = new byte[2];
            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 51;
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = StringToByteArray("0A")[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = StringToByteArray("2C")[0];

            //Orden de conteo->Cassette menor a mayor
            paquete[8] = StringToByteArray("FE")[0];
            paquete[9] = StringToByteArray("DC")[0];
            paquete[10] = StringToByteArray("BA")[0];
            paquete[11] = StringToByteArray("98")[0];

            //Valores a dispensar - > limpiar valores
            paquete[12] = StringToByteArray("30")[0];
            paquete[13] = StringToByteArray("30")[0];
            paquete[14] = StringToByteArray("30")[0];//
            paquete[15] = StringToByteArray("30")[0];
            paquete[16] = StringToByteArray("30")[0];//
            paquete[17] = StringToByteArray("30")[0];
            paquete[18] = StringToByteArray("30")[0];//
            paquete[19] = StringToByteArray("30")[0];
            paquete[20] = StringToByteArray("30")[0];
            paquete[21] = StringToByteArray("30")[0];
            paquete[22] = StringToByteArray("30")[0];
            paquete[23] = StringToByteArray("30")[0];
            paquete[24] = StringToByteArray("30")[0];
            paquete[25] = StringToByteArray("30")[0];
            paquete[26] = StringToByteArray("30")[0];
            paquete[27] = StringToByteArray("30")[0];
            paquete[28] = 0;
            paquete[29] = 0;
            paquete[30] = 0;
            paquete[31] = 0;

            paquete[32] = StringToByteArray("30")[0];
            paquete[33] = StringToByteArray("30")[0];
            paquete[34] = StringToByteArray("30")[0];
            paquete[35] = StringToByteArray("30")[0];
            paquete[36] = StringToByteArray("30")[0];
            paquete[37] = StringToByteArray("30")[0];
            paquete[38] = StringToByteArray("30")[0];
            paquete[39] = StringToByteArray("30")[0];
            paquete[40] = StringToByteArray("30")[0];
            paquete[41] = StringToByteArray("30")[0];
            paquete[42] = StringToByteArray("30")[0];
            paquete[43] = StringToByteArray("30")[0];
            paquete[44] = StringToByteArray("30")[0];
            paquete[45] = StringToByteArray("30")[0];
            paquete[46] = StringToByteArray("30")[0];
            paquete[47] = StringToByteArray("30")[0];
            paquete[48] = 0;
            paquete[49] = 0;
            paquete[50] = 0;
            paquete[51] = 0;
            //



            for (int i = 0; i < lstDispensarMultiple[_IndiceDispensar].Count; i++)
            {

                int adicional = 0;
                switch (lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette)
                {
                    case 1:
                        adicional = 0;
                        break;
                    case 2:
                        adicional = 1;
                        break;
                    case 3:
                        adicional = 2;
                        break;
                    case 4:
                        adicional = 3;
                        break;
                    case 5:
                        adicional = 0;
                        break;
                    case 6:
                        adicional = 1;
                        break;
                    case 7:
                        adicional = 2;
                        break;
                    case 8:
                        adicional = 3;
                        break;
                }

                if (lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette <= 4)
                {
                    string cantidadBilletes = IntToHexF56NumberOfBillsValue(lstDispensarMultiple[_IndiceDispensar][i].Cantidad);
                    paquete[adicional + 11 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[0];
                    paquete[adicional + 12 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[1];
                    paquete[adicional + 19 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("B1")[0];
                    paquete[adicional + 20 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("30")[0];
                }
                else
                {
                    string cantidadBilletes = IntToHexF56NumberOfBillsValue(lstDispensarMultiple[_IndiceDispensar][i].Cantidad);
                    paquete[adicional + 27 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[0];
                    paquete[adicional + 28 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray(cantidadBilletes)[1];
                    paquete[adicional + 35 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("B1")[0];
                    paquete[adicional + 36 + lstDispensarMultiple[_IndiceDispensar][i].NumeroCassette] = StringToByteArray("30")[0];
                }

            }

            paquete[28] = StringToByteArray("0A")[0];
            paquete[29] = StringToByteArray("0A")[0];
            paquete[30] = StringToByteArray("0A")[0];
            paquete[31] = StringToByteArray("0A")[0];
            paquete[48] = StringToByteArray("0A")[0];
            paquete[49] = StringToByteArray("0A")[0];
            paquete[50] = StringToByteArray("0A")[0];
            paquete[51] = StringToByteArray("0A")[0];

            //Fin
            paquete[52] = StringToByteArray("1C")[0];
            paquete[53] = StringToByteArray("10")[0];
            paquete[54] = StringToByteArray("03")[0];

            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private void DemandBillTransportation()
        {
            byte[] paquete = new byte[14];
            byte[] paquetepre = new byte[2];

            paquetepre[0] = StringToByteArray("10")[0];
            paquetepre[1] = StringToByteArray("02")[0];

            paquete[0] = StringToByteArray("00")[0];
            paquete[1] = 8;
            paquete[2] = StringToByteArray("60")[0];
            paquete[3] = StringToByteArray("05")[0];
            paquete[4] = StringToByteArray("FF")[0];
            paquete[5] = 0;
            paquete[6] = 0;
            paquete[7] = 1;
            if (stiloF56 == F56Style.Rear)
            {
                paquete[8] = StringToByteArray("80")[0];
            }
            else
            {
                paquete[8] = 0;
            }
            paquete[9] = StringToByteArray("1C")[0];
            paquete[10] = StringToByteArray("10")[0];
            paquete[11] = StringToByteArray("03")[0];
            byte[] envio = CrearPaquete(paquete);

            byte[] rv = new byte[paquetepre.Length + paquete.Length];
            System.Buffer.BlockCopy(paquetepre, 0, rv, 0, paquetepre.Length);
            System.Buffer.BlockCopy(paquete, 0, rv, paquetepre.Length, paquete.Length);
            WriteData(rv);
        }
        private string IntToHexF56NumberOfBillsValue(int valorCantidad)
        {
            string resultado = string.Empty;

            if (valorCantidad == 0)
            {
                resultado = "3030";
            }
            if (valorCantidad == 1)
            {
                resultado = "30B1";
            }
            if (valorCantidad == 2)
            {
                resultado = "30B2";
            }
            if (valorCantidad == 3)
            {
                resultado = "3033";
            }
            if (valorCantidad == 4)
            {
                resultado = "30B4";
            }
            if (valorCantidad == 5)
            {
                resultado = "3035";
            }
            if (valorCantidad == 6)
            {
                resultado = "3036";
            }
            if (valorCantidad == 7)
            {
                resultado = "30B7";
            }
            if (valorCantidad == 8)
            {
                resultado = "30B8";
            }
            if (valorCantidad == 9)
            {
                resultado = "3039";
            }
            if (valorCantidad == 10)
            {
                resultado = "B130";
            }
            if (valorCantidad == 11)
            {
                resultado = "B1B1";
            }
            if (valorCantidad == 12)
            {
                resultado = "B1B2";
            }
            if (valorCantidad == 13)
            {
                resultado = "B133";
            }
            if (valorCantidad == 14)
            {
                resultado = "B1B4";
            }
            if (valorCantidad == 15)
            {
                resultado = "B135";
            }
            if (valorCantidad == 16)
            {
                resultado = "B136";
            }
            if (valorCantidad == 17)
            {
                resultado = "B1B7";
            }
            if (valorCantidad == 18)
            {
                resultado = "B1B8";
            }
            if (valorCantidad == 19)
            {
                resultado = "B139";
            }
            if (valorCantidad == 20)
            {
                resultado = "B230";
            }

            return resultado;
        }
        private int HexF56NumberOfBillsValueToInt(string valorCantidad)
        {
            int resultado = 0;

            if (valorCantidad == "3030")
            {
                resultado = 0;
            }
            if (valorCantidad == "30B1")
            {
                resultado = 1;
            }
            if (valorCantidad == "30B2")
            {
                resultado = 2;
            }
            if (valorCantidad == "3033")
            {
                resultado = 3;
            }
            if (valorCantidad == "30B4")
            {
                resultado = 4;
            }
            if (valorCantidad == "3035")
            {
                resultado = 5;
            }
            if (valorCantidad == "3036")
            {
                resultado = 6;
            }
            if (valorCantidad == "30B7")
            {
                resultado = 7;
            }
            if (valorCantidad == "30B8")
            {
                resultado = 8;
            }
            if (valorCantidad == "3039")
            {
                resultado = 9;
            }
            if (valorCantidad == "B130")
            {
                resultado = 10;
            }
            if (valorCantidad == "B1B1")
            {
                resultado = 11;
            }
            if (valorCantidad == "B1B2")
            {
                resultado = 12;
            }
            if (valorCantidad == "B133")
            {
                resultado = 13;
            }
            if (valorCantidad == "B1B4")
            {
                resultado = 14;
            }
            if (valorCantidad == "B135")
            {
                resultado = 15;
            }
            if (valorCantidad == "B136")
            {
                resultado = 16;
            }
            if (valorCantidad == "B1B7")
            {
                resultado = 17;
            }
            if (valorCantidad == "B1B8")
            {
                resultado = 18;
            }
            if (valorCantidad == "B139")
            {
                resultado = 19;
            }
            if (valorCantidad == "B230")
            {
                resultado = 20;
            }

            return resultado;
        }
        #endregion

        #region SerialFunctions
        public bool OpenPort(string puerto)
        {
            try
            {
                string sPuerto = string.Empty;

                sPuerto = puerto;

                if (_ComPort.IsOpen == true)
                {
                    _ComPort.Close();
                }

                _ComPort.ReadTimeout = 5000;
                _ComPort.WriteTimeout = 5000;
                _ComPort.BaudRate = 9600;
                _ComPort.DataBits = 8;
                _ComPort.StopBits = StopBits.One;
                _ComPort.Parity = Parity.Even;
                //_ComPort.Handshake = Handshake.None;
                _ComPort.PortName = sPuerto;

                _ComPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ClosePort()
        {
            try
            {

                if (_ComPort.IsOpen == true)
                {
                    _ComPort.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void WriteData(byte[] msg)
        {
            EventArgsF56SerialCommunicationDevice evento = new EventArgsF56SerialCommunicationDevice(TipoInsertEvento.Envio, msg);
            DeviceMessageSerialPort(this, evento);
            try
            {
                if (!(_ComPort.IsOpen == true))
                {
                    _ComPort.Open();
                }
                _ComPort.Write(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {

            }
        }
        private byte[] CrearPaquete(byte[] Paquete)
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

                return Paquete;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        #endregion

        #region SerialEvents
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(400);
            int bytes = _ComPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            _ComPort.Read(comBuffer, 0, bytes);
            EventArgsF56SerialCommunicationDevice evento = new EventArgsF56SerialCommunicationDevice(TipoInsertEvento.Recepcion, comBuffer);
            DeviceMessageSerialPort(this, evento);
            Actuar(comBuffer);
        }
        #endregion
    }

    public class EventArgsF56SerialCommunicationDevice : EventArgs
    {
        private TipoInsertEvento _TipoInsertEvento;
        private byte[] _ByteArray;

        public TipoInsertEvento TipoInsertEvento
        {
            get { return _TipoInsertEvento; }
            set { _TipoInsertEvento = value; }
        }

        public byte[] ByteArray
        {
            get { return _ByteArray; }
            set { _ByteArray = value; }
        }

        public EventArgsF56SerialCommunicationDevice(TipoInsertEvento oTipoInsertEvento, byte[] oByteArray)
        {
            _TipoInsertEvento = oTipoInsertEvento;
            _ByteArray = oByteArray;
        }
    }

    public class EventArgsF56EventsDevice : EventArgs
    {
        private StatesF56Device _State = StatesF56Device.Nothing;
        public StatesF56Device StatesF56Device
        {
            get { return _State; }
            set { _State = value; }
        }

        private List<F56PayParameter> _Dispensado = new List<F56PayParameter>();
        public List<F56PayParameter> Dispensado
        {
            get { return _Dispensado; }
            set { _Dispensado = value; }
        }

        private List<F56PayParameter> _Rechazado = new List<F56PayParameter>();
        public List<F56PayParameter> Rechazado
        {
            get { return _Rechazado; }
            set { _Rechazado = value; }
        }

        private Int64 _Resto = 0;

        public Int64 Resto
        {
            get { return _Resto; }
            set { _Resto = value; }
        }

        public EventArgsF56EventsDevice(StatesF56Device oStatesF56Device, List<F56PayParameter> oLstDispenseValue, List<F56PayParameter> oLstRechazadoValue, Int64 elresto)
        {
            _State = oStatesF56Device;
            _Dispensado = oLstDispenseValue;
            _Rechazado = oLstRechazadoValue;
            _Resto = elresto;
        }
    }

}
