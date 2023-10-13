using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ds.WCFProxy.SeccionConfiguration
{
    using System;
    using System.Configuration;
    public class ConfiguracionServerService : ConfigurationElement
    {


        [ConfigurationProperty("Valor", IsRequired = true, IsKey = true)]
        public string Valor
        {
            get
            {
                return (string)this["Valor"];
            }
            set
            {
                this["Valor"] = value;
            }
        }



    }
}
