using Ds.SincronizacionService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ds.SincronizacionService.ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface ISincronizacionService
    {
        [OperationContract]
        Sincronizacion_Response getDatosSincronizacion(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosSincronizacionPago(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosArqueos(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosCargas(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosMovimientos(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosPartes(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosCambio(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosRecargas(Sincronizacion_Request request);

        [OperationContract]
        Sincronizacion_Response getDatosDonacion(Sincronizacion_Request request);
    }
}
