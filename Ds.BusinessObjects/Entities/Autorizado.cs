using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Autorizado
    {
        private string _Documento;
        private long _IdAutorizacion;
        private bool _Estado;
        private bool _EstadoAutorizacion;
        private string _IdTarjeta;
        private string _NombresAutorizado;
        private DateTime? _FechaInicial;
        private DateTime? _FechaFinal;
        private long _IdEstacionamiento;
        private string _NombreAutorizacion;
        private int _IdTipo;
        private DateTime? _FechaCreacion;
        private long _DocumentoCreador;
        private string _Telefono;
        private string _Email;
        private string _Placa1;
        private string _Placa2;
        private int? _Regla;
        private bool _Sincronizacion;


        public string Documento
        {
            get { return _Documento; }
            set { _Documento = value; }
        }

        public long IdAutorizacion
        {
            get { return _IdAutorizacion; }
            set { _IdAutorizacion = value; }
        }

        public bool Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        public bool EstadoAutorizacion
        {
            get { return _EstadoAutorizacion; }
            set { _EstadoAutorizacion = value; }
        }

        public string IdTarjeta
        {
            get { return _IdTarjeta; }
            set { _IdTarjeta = value; }
        }

        public string NombresAutorizado
        {
            get { return _NombresAutorizado; }
            set { _NombresAutorizado = value; }
        }

        public DateTime? FechaInicial
        {
            get { return _FechaInicial; }
            set { _FechaInicial = value; }
        }

        public DateTime? FechaFinal
        {
            get { return _FechaFinal; }
            set { _FechaFinal = value; }
        }

        public long IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        public string NombreAutorizacion
        {
            get { return _NombreAutorizacion; }
            set { _NombreAutorizacion = value; }
        }

        public int IdTipo
        {
            get { return _IdTipo; }
            set { _IdTipo = value; }
        }

        public DateTime? FechaCreacion
        {
            get { return _FechaCreacion; }
            set { _FechaCreacion = value; }
        }

        public long DocumentoCreador
        {
            get { return _DocumentoCreador; }
            set { _DocumentoCreador = value; }
        }

        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Placa1
        {
            get { return _Placa1; }
            set { _Placa1 = value; }
        }

        public string Placa2
        {
            get { return _Placa2; }
            set { _Placa2 = value; }
        }

        public int? Regla
        {
            get { return _Regla; }
            set { _Regla = value; }
        }

        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }

    }
}
