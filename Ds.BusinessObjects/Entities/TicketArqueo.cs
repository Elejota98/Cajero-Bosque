using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class TicketArqueo
    {
        private string _CodigoArqueo;
        private string _ModuloArqueo;
        private string _UsuarioArqueo;
        private int _ValorArqueo;
        private int _ProducidoArqueo;
        private DateTime _FechaArqueo;

        public DateTime FechaArqueo
        {
            get { return _FechaArqueo; }
            set { _FechaArqueo = value; }
        }

        public int ProducidoArqueo
        {
            get { return _ProducidoArqueo; }
            set { _ProducidoArqueo = value; }
        }

        public int ValorArqueo
        {
            get { return _ValorArqueo; }
            set { _ValorArqueo = value; }
        }

        public string UsuarioArqueo
        {
            get { return _UsuarioArqueo; }
            set { _UsuarioArqueo = value; }
        }

        public string ModuloArqueo
        {
            get { return _ModuloArqueo; }
            set { _ModuloArqueo = value; }
        }

        public string CodigoArqueo
        {
            get { return _CodigoArqueo; }
            set { _CodigoArqueo = value; }
        }
    }
}
