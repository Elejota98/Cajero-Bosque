using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoPIN", Namespace = "http://www.dsystem.co/types/")]
    public class DtoPIN
    {
        private string _PIN;
        private DateTime _FechaValidacion;
        private int _TipoVehiculo;

        [DataMember]
        public int TipoVehiculo
        {
            get { return _TipoVehiculo; }
            set { _TipoVehiculo = value; }
        }

        [DataMember]
        public DateTime FechaValidacion
        {
            get { return _FechaValidacion; }
            set { _FechaValidacion = value; }
        }

        [DataMember]
        public string PIN
        {
            get { return _PIN; }
            set { _PIN = value; }
        }
    }
}
