using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Entities
{
    [DataContract(Name = "ServiceMovimiento", Namespace = "http://www.dsystem.co/types/")]
    public class Movimiento
    {
        private long? _IdTransaccion;
        private long _IdSede;
        private long? _IdCarga;
        private long? _IdArqueo;
        private long _ID_Operacion;
        private string _ID_Modulo;
        private string _Parte;
        private int _Denominacion;
        private int _Cantidad;
        private TipoOperacion _TipoOperacion;
        private TipoMovimiento _TipoAccionMovimiento;
        private long _ID_Movimiento;
        private string _Accion;
        [DataMember]
        public string Accion
        {
            get { return _Accion; }
            set { _Accion = value; }
        }

        private int _Valor;
        [DataMember]
        public int Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        private DateTime _FechaMovimiento;
        [DataMember]
        public DateTime FechaMovimiento
        {
            get { return _FechaMovimiento; }
            set { _FechaMovimiento = value; }
        }
        private bool _Sincronizacion;
        [DataMember]
        public bool Sincronizacion
        {
            get { return _Sincronizacion; }
            set { _Sincronizacion = value; }
        }
        
        
        [DataMember]
        public long? IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }
        [DataMember]
        public long? IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }


        [DataMember]
        public long? IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        [DataMember]
        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

        [DataMember]
        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        [DataMember]
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }

        [DataMember]
        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        [DataMember]
        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }

        [DataMember]
        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

        [DataMember]
        public TipoMovimiento TipoAccionMovimiento
        {
            get { return _TipoAccionMovimiento; }
            set { _TipoAccionMovimiento = value; }
        }

        [DataMember]
        public long ID_Movimiento
        {
            get { return _ID_Movimiento; }
            set { _ID_Movimiento = value; }
        }
    }
}
