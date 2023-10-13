using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class Modulo
    {
        private string _ID_Modulo;
        private List<ParteModulo> _Partes = new List<ParteModulo>();
        private Sede _Sede = new Sede();

        public string ID_Modulo
        {
            get { return _ID_Modulo; }
            set { _ID_Modulo = value; }
        }

        public List<ParteModulo> Partes
        {
            get { return _Partes; }
            set { _Partes = value; }
        }

        public Sede Sede
        {
            get { return _Sede; }
            set { _Sede = value; }
        }
    }
}
