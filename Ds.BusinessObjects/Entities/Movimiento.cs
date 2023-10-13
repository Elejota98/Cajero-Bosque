using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds.BusinessObjects.Enums;

namespace Ds.BusinessObjects.Entities
{
    public class Movimiento
    {
        private long _IdTransaccion;
        private long _IdSede;
        private long _IdCarga;
        private long _IdArqueo;
        private long _ID_Operacion;
        private string _ID_Modulo;
        private string _Parte;
        private int _Denominacion;
        private int _Cantidad;
        private TipoOperacion _TipoOperacion;
        private TipoMovimiento _TipoAccionMovimiento;
        private long _ID_Movimiento;


        
        public long IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }
        
        public long IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
       
        public long IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }


        
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        
        public long ID_Operacion
        {
            get { return _ID_Operacion; }
            set { _ID_Operacion = value; }
        }

       
        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

      
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
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

       
        public TipoOperacion TipoOperacion
        {
            get { return _TipoOperacion; }
            set { _TipoOperacion = value; }
        }

       
        public TipoMovimiento TipoAccionMovimiento
        {
            get { return _TipoAccionMovimiento; }
            set { _TipoAccionMovimiento = value; }
        }

       
        public long ID_Movimiento
        {
            get { return _ID_Movimiento; }
            set { _ID_Movimiento = value; }
        }
    }
}
