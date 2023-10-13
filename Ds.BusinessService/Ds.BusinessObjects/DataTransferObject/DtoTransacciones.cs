using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoTransacciones
    {
        private long  _IdTransaccion;
        private string _IdCajero;
        private string _IdDocumento;
        private DateTime _FechaTransaccion;
              
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
    
        public string IdDocumento
        {
            get { return _IdDocumento; }
            set { _IdDocumento = value; }
        }
      
        public DateTime FechaTransaccion
        {
            get { return _FechaTransaccion; }
            set { _FechaTransaccion = value; }
        }
    }
}
