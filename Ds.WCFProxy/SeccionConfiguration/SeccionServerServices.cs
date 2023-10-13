using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ds.WCFProxy.SeccionConfiguration
{

    using System;
    using System.Configuration;



    public class SeccionServerServices : ConfigurationSection
    {

        private static ConfigurationPropertyCollection _propiedades;
        private static ConfigurationProperty _ServerServiceConfigurados;

        static SeccionServerServices()
        {
            _ServerServiceConfigurados = new ConfigurationProperty
                (
                 "",
                 typeof(ColeccionServerServices),
                 null,
                 ConfigurationPropertyOptions.IsRequired |
                 ConfigurationPropertyOptions.IsDefaultCollection |
                 ConfigurationPropertyOptions.IsKey
                );
            _propiedades = new ConfigurationPropertyCollection();
            _propiedades.Add(_ServerServiceConfigurados);
        }



        public ColeccionServerServices ServidoresServicios
        {
            get
            {
                return (ColeccionServerServices)base[_ServerServiceConfigurados];
            }
        }


        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _propiedades;
            }
        }

    }
}
       

      
 

