using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class MyRegistro
    {
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _version = string.Empty;

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private string _unistallKey = string.Empty;

        public string UnistallKey
        {
            get { return _unistallKey; }
            set { _unistallKey = value; }
        }

    }
}
