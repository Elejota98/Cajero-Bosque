using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class TicketCarga
    {
        private DateTime _FechaCarga;

        public DateTime FechaCarga
        {
            get { return _FechaCarga; }
            set { _FechaCarga = value; }
        }
        private string _CodigoCarga;

        public string CodigoCarga
        {
            get { return _CodigoCarga; }
            set { _CodigoCarga = value; }
        }
        private string _CodigoCargaCentral;

        public string CodigoCargaCentral
        {
            get { return _CodigoCargaCentral; }
            set { _CodigoCargaCentral = value; }
        }
        private string _ModuloCarga;

        public string ModuloCarga
        {
            get { return _ModuloCarga; }
            set { _ModuloCarga = value; }
        }
        private string _UsuarioCarga;

        public string UsuarioCarga
        {
            get { return _UsuarioCarga; }
            set { _UsuarioCarga = value; }
        }


    }
}
