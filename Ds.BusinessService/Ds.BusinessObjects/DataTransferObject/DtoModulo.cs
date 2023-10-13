using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoModulo
    {
        private string _IdModulo;
        private long _IdSede;
        private string _Nombre;
        private long _IdCiudad;
        private string _Ciudad;
        private long _IdPais;
        private string _Pais;
        private string _Ip;
        private string _Mac;
        private long _IdTipoModulo;
        private string _Extension;
        private bool _Estado;
        private string _Direccion;

        
        private List<DtoParteModulo> _Partes = new List<DtoParteModulo>();
        private List<DtoAccion> _Acciones = new List<DtoAccion>();
        private List<DtoParametro> _Parametros = new List<DtoParametro>();
        private List<DtoParteModuloF56> _PartesF56 = new List<DtoParteModuloF56>();
        private DtoFactura _Factura = new DtoFactura();

        public DtoFactura Factura
        {
            get { return _Factura; }
            set { _Factura = value; }
        }

        public List<DtoParteModuloF56> PartesF56
        {
            get { return _PartesF56; }
            set { _PartesF56 = value; }
        }

        public List<DtoParametro> Parametros
        {
            get { return _Parametros; }
            set { _Parametros = value; }
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

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        
        public long IdCiudad
        {
            get { return _IdCiudad; }
            set { _IdCiudad = value; }
        }
        
        public string Ciudad
        {
            get { return _Ciudad; }
            set { _Ciudad = value; }
        }
        
        public long IdPais
        {
            get { return _IdPais; }
            set { _IdPais = value; }
        }
        
        public string Pais
        {
            get { return _Pais; }
            set { _Pais = value; }
        }
        
        public string Ip
        {
            get { return _Ip; }
            set { _Ip = value; }
        }
        
        public string Mac
        {
            get { return _Mac; }
            set { _Mac = value; }
        }
        
        public long IdTipoModulo
        {
            get { return _IdTipoModulo; }
            set { _IdTipoModulo = value; }
        }
        
        public string Extension
        {
            get { return _Extension; }
            set { _Extension = value; }
        }
        
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }


        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }
        
        public List<DtoParteModulo> Partes
        {
            get { return _Partes; }
            set { _Partes = value; }
        }

        
        public List<DtoAccion> Acciones
        {
            get { return _Acciones; }
            set { _Acciones = value; }
        }
    }
}
