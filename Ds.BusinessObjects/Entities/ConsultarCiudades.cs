using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class ConsultarCiudades
    {
        private int _IdCountry;
        private int _IdCity;
        private int _IdProducto;

        public int IdCountry
        {
            get { return _IdCountry; }
            set { _IdCountry = value; }
        }
        public int IdCity
        {
            get { return _IdCity; }
            set { _IdCity = value; }
        }
        public int IdProducto
        {
            get { return _IdProducto; }
            set { _IdProducto = value; }
        }


    }
}
