using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class TransaccionEfectivo
    {
        private TipoParte _TipoParte;
        private int _Denominacion;
        private int _Cantidad;
        private string _IdParte;

        public TipoParte TipoParte
        {
            get { return _TipoParte; }
            set { _TipoParte = value; }
        }

        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }

        public string IdParte
        {
            get { return _IdParte; }
            set { _IdParte = value; }
        }
    }
}
