using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.DataTransferObject
{
    public class DtoConsultarCiudades
    {
        private int _IdCiudad;
        private string _NombreCiudad;
        private string _OrigenDatos;
        
        public int IdCiudad
        {
            get { return _IdCiudad; }
            set { _IdCiudad = value; }
        }
        public string NombreCiudad
        {
            get { return _NombreCiudad; }
            set { _NombreCiudad = value; }
        }
        public string OrigenDatos
        {
            get { return _OrigenDatos; }
            set { _OrigenDatos = value; }
        }


        
    }
}
