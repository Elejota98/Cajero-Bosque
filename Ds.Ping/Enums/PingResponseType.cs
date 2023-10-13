using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ds.Ping
{
    public enum PingResponseType
    {
        Ok = 0,
        CouldNotResolveHost,
        RequestTimedOut,
        ConnectionError,
        InternalError,
        Canceled
    }
}
