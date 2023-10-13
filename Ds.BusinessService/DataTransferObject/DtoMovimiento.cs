using Ds.BusinessService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoMovimiento", Namespace = "http://www.dsystem.co/types/")]
    public class DtoMovimiento
    {
        private long _IdMovimiento;
        private string _IdCajero;
        private long _Id;
        private string _Parte;
        private string _Accion;
        private int _Denominacion;
        private int _Cantidad;
        private int _Valor;       
        private int _Acumulado;
        private TipoMovimiento _TipoAccionMovimiento;

        [DataMember]
        public TipoMovimiento TipoAccionMovimiento
        {
            get { return _TipoAccionMovimiento; }
            set { _TipoAccionMovimiento = value; }
        }

        private long? _IdTransaccion;
        [DataMember]
        public long? IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        private long _IdSede;
        [DataMember]
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        private long? _IdCarga;
        [DataMember]
        public long? IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
        private long? _IdArqueo;
        [DataMember]
        public long? IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
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
        public long IdMovimiento
        {
            get { return _IdMovimiento; }
            set { _IdMovimiento = value; }
        }
        [DataMember]
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
        [DataMember]
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [DataMember]
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }
        [DataMember]
        public string Accion
        {
            get { return _Accion; }
            set { _Accion = value; }
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
        public int Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        [DataMember]
        public int Acumulado
        {
            get { return _Acumulado; }
            set { _Acumulado = value; }
        }
    }
}
