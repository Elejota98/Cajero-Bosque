using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.WinForm.Model
{
    public partial class Model : IModel
    {

        private static Ds.ServiceProxy.IProxyService _ProxyServicios;

        static Model()
        {
            //Inicializador de Servicios
            _ProxyServicios = new Ds.ServiceProxy.ProxyService();
        }
    }
}
