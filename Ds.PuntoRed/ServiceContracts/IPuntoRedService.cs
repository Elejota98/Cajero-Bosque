using Ds.PuntoRed.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ds.PuntoRed.ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IPuntoRedService
    {

        [OperationContract]
        getPuntoRed_Response getOperadorRecarga(getPuntoRed_Request request);

        [OperationContract]
        getPuntoRed_Response setRecarga(getPuntoRed_Request request);

        [OperationContract]
        getPuntoRed_Response getOperadorPaquete(getPuntoRed_Request request);

        [OperationContract]
        getPuntoRed_Response getCategoriaPaquete(getPuntoRed_Request request);

        [OperationContract]
        getPuntoRed_Response getPaquetesXCategoriaPaquete(getPuntoRed_Request request);

        [OperationContract]
        getPuntoRed_Response setComprarPaquete(getPuntoRed_Request request);
    }
}
