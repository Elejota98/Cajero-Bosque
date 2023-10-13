using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class DispositivoPago
    {
        private int _Num_Cassettes = 0;
        private int _Num_Hoppers = 0;
        private int[] _Bill_Denom;
        private int[] _Coin_Denom;
        private int[] _Cassette_Denom;
        private int[] _Hopper_Denom;
        private int[] _Bill_Qty;
        private int[] _Coin_Qty;
        private int[] _Bill_Min;
        private int[] _Coin_Min;
        private int[] _Bill_Alarm;
        private int[] _Coin_Alarm;
        private int[] _Coin_Order;
        private int[] _Bill_Order;


        public int Num_Cassettes
        {
            get { return _Num_Cassettes; }
            set { _Num_Cassettes = value; }
        }

        public int Num_Hoppers
        {
            get { return _Num_Hoppers; }
            set { _Num_Hoppers = value; }
        }

        public int[] Bill_Denom
        {
            get { return _Bill_Denom; }
            set { _Bill_Denom = value; }
        }

        public int[] Coin_Denom
        {
            get { return _Coin_Denom; }
            set { _Coin_Denom = value; }
        }

        public int[] Cassette_Denom
        {
            get { return _Cassette_Denom; }
            set { _Cassette_Denom = value; }
        }

        public int[] Hopper_Denom
        {
            get { return _Hopper_Denom; }
            set { _Hopper_Denom = value; }
        }

        public int[] Bill_Qty
        {
            get { return _Bill_Qty; }
            set { _Bill_Qty = value; }
        }

        public int[] Coin_Qty
        {
            get { return _Coin_Qty; }
            set { _Coin_Qty = value; }
        }

        public int[] Bill_Min
        {
            get { return _Bill_Min; }
            set { _Bill_Min = value; }
        }

        public int[] Coin_Min
        {
            get { return _Coin_Min; }
            set { _Coin_Min = value; }
        }

        public int[] Bill_Alarm
        {
            get { return _Bill_Alarm; }
            set { _Bill_Alarm = value; }
        }

        public int[] Coin_Alarm
        {
            get { return _Coin_Alarm; }
            set { _Coin_Alarm = value; }
        }

        public int[] Coin_Order
        {
            get { return _Coin_Order; }
            set { _Coin_Order = value; }
        }

        public int[] Bill_Order
        {
            get { return _Bill_Order; }
            set { _Bill_Order = value; }
        }
    }
}
