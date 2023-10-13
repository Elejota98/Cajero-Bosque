using Ds.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoArqueo
    {
        private long _ID_Arqueo;
        private DateTime _Fecha;
        private int _Valor;
        private int _Producido;
        private int _Denominacion;
        private int _Cantidad;
        private string _Parte;

        
        public string Parte
        {
            get { return _Parte; }
            set { _Parte = value; }
        }

        
        public int Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }
       
        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

       
        public long ID_Arqueo
        {
            get { return _ID_Arqueo; }
            set { _ID_Arqueo = value; }
        }

        
        public int Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

       
        public int Producido
        {
            get { return _Producido; }
            set { _Producido = value; }
        }

        
        public DateTime Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }
    }
}
