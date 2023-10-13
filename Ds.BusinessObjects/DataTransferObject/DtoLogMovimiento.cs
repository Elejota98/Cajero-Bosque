using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoLogMovimiento
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

        
        public long IdMovimiento
        {
            get { return _IdMovimiento; }
            set { _IdMovimiento = value; }
        }
        public string IdCajero
        {
            get { return _IdCajero; }
            set { _IdCajero = value; }
        }
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }
        public string Accion
        {
            get { return _Accion; }
            set { _Accion = value; }
        }
        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }
        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }
        public int Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        public int Acumulado
        {
            get { return _Acumulado; }
            set { _Acumulado = value; }
        }
    }
}
