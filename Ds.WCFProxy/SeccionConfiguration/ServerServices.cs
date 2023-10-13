using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ds.WCFProxy.SeccionConfiguration
{
    public class ServerServices : ConfigurationElement
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
