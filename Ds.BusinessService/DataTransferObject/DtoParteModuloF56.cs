using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.DataTransferObject
{
    [DataContract(Name = "ServiceDtoParteModuloF56", Namespace = "http://www.dsystem.co/types/")]
    public class DtoParteModuloF56
    {
        private string _Nombre_Parte;
        private string _Tipo_Parte;
        private int _Num_Parte;
        private int _Denominacion;
        private int _Qty_Min;
        private int _Qty_Alarma;
        private int _Dinero_Actual;
        private int _Qty_Actual;
        private bool _Prioridad;
        private int _Qty_Max;

        [DataMember]
        public string Nombre_Parte
        {
            get { return _Nombre_Parte; }
            set { _Nombre_Parte = value; }
        }

        [DataMember]
        public string Tipo_Parte
        {
            get { return _Tipo_Parte; }
            set { _Tipo_Parte = value; }
        }

        [DataMember]
        public int Num_Parte
        {
            get { return _Num_Parte; }
            set { _Num_Parte = value; }
        }

        [DataMember]
        public int Denominacion
        {
            get { return _Denominacion; }
            set { _Denominacion = value; }
        }

        [DataMember]
        public int Qty_Min
        {
            get { return _Qty_Min; }
            set { _Qty_Min = value; }
        }

        [DataMember]
        public int Qty_Alarma
        {
            get { return _Qty_Alarma; }
            set { _Qty_Alarma = value; }
        }

        [DataMember]
        public int Dinero_Actual
        {
            get { return _Dinero_Actual; }
            set { _Dinero_Actual = value; }
        }

        [DataMember]
        public int Qty_Actual
        {
            get { return _Qty_Actual; }
            set { _Qty_Actual = value; }
        }

        [DataMember]
        public bool Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }

        [DataMember]
        public int Qty_Max
        {
            get { return _Qty_Max; }
            set { _Qty_Max = value; }
        }
    }
}
