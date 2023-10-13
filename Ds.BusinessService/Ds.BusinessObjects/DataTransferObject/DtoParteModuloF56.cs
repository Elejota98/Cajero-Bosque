using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoParteModuloF56
    {
        private string _Nombre_Parte = string.Empty;
        private string _Tipo_Parte = string.Empty;
        private int _Num_Parte = 0;
        private int _Denominacion = 0;
        private int _Qty_Min = 0;
        private int _Qty_Alarma = 0;
        private int _Dinero_Actual = 0;
        private int _Qty_Actual = 0;
        private int _Qty_Max = 0;
        private bool _Prioridad = false;


        public string Nombre_Parte
        {
            get { return _Nombre_Parte; }
            set { _Nombre_Parte = value; }
        }

        public string Tipo_Parte
        {
            get { return _Tipo_Parte; }
            set { _Tipo_Parte = value; }
        }

        public int Num_Parte
        {
            get { return _Num_Parte; }
            set { _Num_Parte = value; }
        }

        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        public int Qty_Min
        {
            get { return _Qty_Min; }
            set { _Qty_Min = value; }
        }

        public int Qty_Alarma
        {
            get { return _Qty_Alarma; }
            set { _Qty_Alarma = value; }
        }

        public int Dinero_Actual
        {
            get { return _Dinero_Actual; }
            set { _Dinero_Actual = value; }
        }

        public int Qty_Actual
        {
            get { return _Qty_Actual; }
            set { _Qty_Actual = value; }
        }

        public bool Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }

        public int Qty_Max
        {
            get { return _Qty_Max; }
            set { _Qty_Max = value; }
        }
    }
}
