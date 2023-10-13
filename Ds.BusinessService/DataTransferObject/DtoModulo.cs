using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoModulo", Namespace = "http://www.dsystem.co/types/")]
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
        private DtoFactura _Factura = new DtoFactura();

        

        [DataMember]
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }

        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }

        [DataMember]
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        [DataMember]
        public long IdCiudad
        {
            get { return _IdCiudad; }
            set { _IdCiudad = value; }
        }
        [DataMember]
        public string Ciudad
        {
            get { return _Ciudad; }
            set { _Ciudad = value; }
        }
        [DataMember]
        public long IdPais
        {
            get { return _IdPais; }
            set { _IdPais = value; }
        }
        [DataMember]
        public string Pais
        {
            get { return _Pais; }
            set { _Pais = value; }
        }
        [DataMember]
        public string Ip
        {
            get { return _Ip; }
            set { _Ip = value; }
        }
        [DataMember]
        public string Mac
        {
            get { return _Mac; }
            set { _Mac = value; }
        }
        [DataMember]
        public long IdTipoModulo
        {
            get { return _IdTipoModulo; }
            set { _IdTipoModulo = value; }
        }
        [DataMember]
        public string Extension
        {
            get { return _Extension; }
            set { _Extension = value; }
        }
        [DataMember]
        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        [DataMember]
        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }

        [DataMember]
        public List<DtoParteModulo> Partes
        {
            get { return _Partes; }
            set { _Partes = value; }
        }

        [DataMember]
        public List<DtoAccion> Acciones
        {
            get { return _Acciones; }
            set { _Acciones = value; }
        }

        [DataMember]
        public DtoFactura Factura
        {
            get { return _Factura; }
            set { _Factura = value; }
        }
    }
}
