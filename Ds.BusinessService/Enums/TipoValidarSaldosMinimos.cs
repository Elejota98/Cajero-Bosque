using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessService.Enums
{
    [DataContract(Name = "ServiceTipoValidarSaldosMinimos", Namespace = "http://www.eglobalt.com/types/")]
    public enum TipoValidarSaldosMinimos
    {
        True = 1,
        False = 0,
    }
}
