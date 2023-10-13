using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds.BusinessObjects.Enums;

namespace Ds.BusinessObjects.Entities
{
    public class Operacion
    {
        private string _ID_Modulo;
        private string _CodigoBarras;
        private string _ID_Usuario;
        private long _ID_Transaccion;
        private TipoOperacion _TipoOperacion;
        private long _ID_Operacion;
        private string _ID_Fake_Operacion;
        private Pago _Pago = new Pago();
        private Arqueo _Arqueo = new Arqueo();
        private long _IdSede;
        private string _Programa;
        private int _ValidacionCobro;

        public int ValidacionCobro
        {
            get { return _ValidacionCobro; }
            set { _ValidacionCobro = value; }
        }



        public string Programa
        {
            get { return _Programa; }
            set { _Programa = value; }
        }
        
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        public string CodigoBarras
        {
            get { return _CodigoBarras; }
            set { _CodigoBarras = value; }
        }

        private long _ID_Pago;

        public long ID_Pago
        {
            get { return _ID_Pago; }
            set { _ID_Pago = value; }
        }
        private int _Estado;

        public int Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        private double _Total;

        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        private double _Donacion;
        public double Donacion
        {
            get { return _Donacion; }
            set { _Donacion = value; }
        }

        private double _Comision;

        public double Comision
        {
            get { return _Comision; }
            set { _Comision = value; }
        }
        private double _Redondeo;

        public double Redondeo
        {
            get { return _Redondeo; }
            set { _Redondeo = value; }
        }
        private double _Iva;

        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        public string ID_Usuario
        {
            get { return _ID_Usuario; }
            set { _ID_Usuario = value; }
        }

        public long ID_Transaccion
        {
            get { return _ID_Transaccion; }
            set { _ID_Transaccion = value; }
        }

        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

        public string ID_Fake_Operacion
        {
            get { return _ID_Fake_Operacion; }
            set { _ID_Fake_Operacion = value; }
        }

        public Pago Pago
        {
            get { return _Pago; }
            set { _Pago = value; }
        }

        public Arqueo Arqueo
        {
            get { return _Arqueo; }
            set { _Arqueo = value; }
        }
        private double _TotalPagado;

        public double TotalPagado
        {
            get { return _TotalPagado; }
            set { _TotalPagado = value; }
        }

        private string _Fundacion;

        public string Fundacion
        {
            get { return _Fundacion; }
            set { _Fundacion = value; }
        }

        private double _ValorDonacion;

        public double ValorDonacion
        {
            get { return _ValorDonacion; }
            set { _ValorDonacion = value; }
        }

        private double _ValorRecarga;

        public double ValorRecarga
        {
            get { return _ValorRecarga; }
            set { _ValorRecarga = value; }
        }

        private string _Operador;

        public string Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }

        private string _Descripcion;

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private string _Linea;

        public string Linea
        {
            get { return _Linea; }
            set { _Linea = value; }
        }
    

    }
}
