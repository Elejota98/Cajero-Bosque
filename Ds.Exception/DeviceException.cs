﻿using Ds.Global;
using Ds.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ds.Exception
{
    public class DeviceException : System.Exception
    {
        public DeviceException(string sMensajeError, System.Exception eException)
            : base(sMensajeError, eException)
        {
            //Escribe en el Log
            TraceHandler.WriteLine(LOG.NombreArchivoLogDevice, sMensajeError, TipoLog.ERROR);
        }
    }
}
