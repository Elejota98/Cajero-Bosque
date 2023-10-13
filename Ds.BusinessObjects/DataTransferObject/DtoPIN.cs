using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoPIN
    {
        private string _PIN;
        private DateTime _FechaValidacion;
        private int _TipoVehiculo;

        public int TipoVehiculo
        {
            get { return _TipoVehiculo; }
            set { _TipoVehiculo = value; }
        }

        public DateTime FechaValidacion
        {
            get { return _FechaValidacion; }
            set { _FechaValidacion = value; }
        }

        public string PIN
        {
            get { return _PIN; }
            set { _PIN = value; }
        }
    }
}
