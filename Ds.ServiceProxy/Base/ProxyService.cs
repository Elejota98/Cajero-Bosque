using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ds.ServiceProxy
{
    public partial class ProxyService : IProxyService
    {
        //Definicion de los servicios
        private static Ds_ModuloComercialService.ModuloServiceClient _Ds_ModuloServices { get; set; }
        
        private Guid _NuevoRequestId;

        static ProxyService()
        {

            try
            {
                //Instanciación de los servicios
                _Ds_ModuloServices = new Ds_ModuloComercialService.ModuloServiceClient();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Obtiene Id Unico
        /// </summary>
        public Guid NuevoRequestId
        {
            get { return Guid.NewGuid(); }
            set { _NuevoRequestId = value; }
        }
    }
}
