using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceOperacion", Namespace = "http://www.dsystem.co/types/")]
    public class Operacion
    {
        private string _ID_Modulo;
        private string _ID_Usuario;
        private string _CodigoBarras;
        private long _ID_Transaccion;
        private TipoOperacion _TipoOperacion;
        private long _ID_Operacion;
        private Pago _Pago = new Pago();
        private Arqueo _Arqueo = new Arqueo();
        private long _ID_Pago;
        private long _IdSede;
        private string _Programa;
        private long _IdAutorizado;

        private int _ValidacionCobro;
        [DataMember]
        public int ValidacionCobro
        {
            get { return _ValidacionCobro; }
            set { _ValidacionCobro = value; }
        }

        [DataMember]
        public long IdAutorizado
        {
            get { return _IdAutorizado; }
            set { _IdAutorizado = value; }
        }
        private long _IdTipoPago;

        [DataMember]
        public long IdTipoPago
        {
            get { return _IdTipoPago; }
            set { _IdTipoPago = value; }
        }

        [DataMember]
        public string Programa
        {
            get { return _Programa; }
            set { _Programa = value; }
        }


        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
       
        [DataMember]
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }
        [DataMember]
        public long ID_Pago
        {
            get { return _ID_Pago; }
            set { _ID_Pago = value; }
        }
        private int _Estado;
        [DataMember]
        public int Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        private double _Total;
        [DataMember]
        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        private double _Donacion;
        [DataMember]
        public double Donacion
        {
            get { return _Donacion; }
            set { _Donacion = value; }
        }

        private double _Comision;
        [DataMember]
        public double Comision
        {
            get { return _Comision; }
            set { _Comision = value; }
        }
        private double _Redondeo;
        [DataMember]
        public double Redondeo
        {
            get { return _Redondeo; }
            set { _Redondeo = value; }
        }
        private double _Iva;
        [DataMember]
        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        [DataMember]
        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        [DataMember]
        public string ID_Usuario
        {
            get { return _ID_Usuario; }
            set { _ID_Usuario = value; }
        }

        [DataMember]
        public long ID_Transaccion
        {
            get { return _ID_Transaccion; }
            set { _ID_Transaccion = value; }
        }

        [DataMember]
        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

        [DataMember]
        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

        [DataMember]
        public Pago Pago
        {
            get { return _Pago; }
            set { _Pago = value; }
        }

        [DataMember]
        public Arqueo Arqueo
        {
            get { return _Arqueo; }
            set { _Arqueo = value; }
        }

        
        private double _TotalPagado;
        [DataMember]
        public double TotalPagado
        {
            get { return _TotalPagado; }
            set { _TotalPagado = value; }
        }

        private string _Fundacion;
        [DataMember]
        public string Fundacion
        {
            get { return _Fundacion; }
            set { _Fundacion = value; }
        }

        private double _ValorDonacion;
        [DataMember]
        public double ValorDonacion
        {
            get { return _ValorDonacion; }
            set { _ValorDonacion = value; }
        }

        private double _ValorRecarga;
        [DataMember]
        public double ValorRecarga
        {
            get { return _ValorRecarga; }
            set { _ValorRecarga = value; }
        }

        private string _Operador;
        [DataMember]
        public string Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }

        private string _Descripcion;
        [DataMember]
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private string _Linea;
        [DataMember]
        public string Linea
        {
            get { return _Linea; }
            set { _Linea = value; }
        }

    }
}
