using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoFactura
    {
        private long _IdFacturacion;
        private string _IdModulo;
        private long _IdSede;
        private string _Prefijo;
        private int _FacturaInicial;
        private int _FacturaFinal;
        private int _FacturaActual;
        private long _NumeroResolucion;
        private string _FechaResolucion;
        private string _FechaFinResolucion;
        private bool _Estado;



        
        public long IdFacturacion
        {
            get { return _IdFacturacion; }
            set { _IdFacturacion = value; }
        }
        
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }
        
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
       
        public string Prefijo
        {
            get { return _Prefijo; }
            set { _Prefijo = value; }
        }
        
        public int FacturaInicial
        {
            get { return _FacturaInicial; }
            set { _FacturaInicial = value; }
        }
        
        public int FacturaFinal
        {
            get { return _FacturaFinal; }
            set { _FacturaFinal = value; }
        }
        
        public int FacturaActual
        {
            get { return _FacturaActual; }
            set { _FacturaActual = value; }
        }
        
        public long NumeroResolucion
        {
            get { return _NumeroResolucion; }
            set { _NumeroResolucion = value; }
        }
        
        public string FechaResolucion
        {
            get { return _FechaResolucion; }
            set { _FechaResolucion = value; }
        }
       
        public string FechaFinResolucion
        {
            get { return _FechaFinResolucion; }
            set { _FechaFinResolucion = value; }
        }
       
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

    }
}
