using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Items
    {
        private int _Denominacion;

        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }
        private int _Cantidad;

        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }
        private double _Valor;

        public double Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }


    }
}
