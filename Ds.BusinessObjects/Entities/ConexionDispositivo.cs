using Ds.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ConexionDispositivo
    {
        private TipoConexionDispositivo _TipoConexionDispositivo;
        private string _sPuertoMedioComunicacion = string.Empty;
        private int _iTimeOut = 0;
        private int _iNumeroIntentos = 0;

        public TipoConexionDispositivo oTipoConexionDispositivo
        {
            get { return _TipoConexionDispositivo; }
            set { _TipoConexionDispositivo = value; }
        }
        public string sPuertoMedioComunicacion
        {
            get { return _sPuertoMedioComunicacion; }
            set { _sPuertoMedioComunicacion = value; }
        }
        public int iTimeOut
        {
            get { return _iTimeOut; }
            set { _iTimeOut = value; }
        }
        public int iNumeroIntentos
        {
            get { return _iNumeroIntentos; }
            set { _iNumeroIntentos = value; }
        }
    }
}
