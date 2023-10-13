using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ProductoTicket
    {
        private string _PRDITM = string.Empty;

        public string PRDITM
        {
            get { return _PRDITM; }
            set { _PRDITM = value; }
        }

        private string _QTY = string.Empty;

        public string QTY
        {
            get { return _QTY; }
            set { _QTY = value; }
        }

        private string _UNIT = string.Empty;

        public string UNIT
        {
            get { return _UNIT; }
            set { _UNIT = value; }
        }

        private double _PRICE = 0;

        public double PRICE
        {
            get { return _PRICE; }
            set { _PRICE = value; }
        }

        private string _DISCOUNT1 = string.Empty;

        public string DISCOUNT1
        {
            get { return _DISCOUNT1; }
            set { _DISCOUNT1 = value; }
        }

        private string _PERCENT = string.Empty;

        public string PERCENT
        {
            get { return _PERCENT; }
            set { _PERCENT = value; }
        }

        private double _AMOUNT = 0;

        public double AMOUNT
        {
            get { return _AMOUNT; }
            set { _AMOUNT = value; }
        }

        private string _WHM = string.Empty;

        public string WHM
        {
            get { return _WHM; }
            set { _WHM = value; }
        }

        private string _DESC = string.Empty;

        public string DESC
        {
            get { return _DESC; }
            set { _DESC = value; }
        }
    }
}
