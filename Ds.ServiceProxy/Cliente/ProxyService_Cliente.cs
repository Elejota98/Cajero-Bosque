using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Handlers;

namespace Ds.ServiceProxy
{
    public partial class ProxyService : IProxyService
    {
        //public ResultadoOperacion RegistrarEstado(EstadoModulo oModulo, string sUrl)
        //{
        //    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

        //    MonitoreoCentralService objecto = new MonitoreoCentralService();
        //    objecto.Timeout = 180000;

        //    objecto.Url = sUrl;

        //    bool recibido = false;

        //    try
        //    {
                
        //        setEstadoModulo_Request request = new setEstadoModulo_Request();
        //        request.oModulo = new ServiceEstadoModulo();
        //        request.oModulo.ID_Modulo = oModulo.ID_Modulo;
        //        request.oModulo.Nombre_Pantalla = oModulo.Nombre_Pantalla;
        //        request.oModulo.Version = oModulo.Version;
        //        request.RequestId = NuevoRequestId.ToString();

        //        setEstadoModulo_Response response  = objecto.setEstadoModulo(request);
        //        recibido = response.oEstadoRegistro;

        //        if (recibido)
        //        {
        //            oResultadoOperacion.EntidadDatos = recibido;
        //            oResultadoOperacion.oEstado = TipoRespuesta.Exito;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oResultadoOperacion.oEstado = TipoRespuesta.Error;
        //        oResultadoOperacion.Mensaje = "Error servicio RegistrarEstado: " + ex.Message;
        //        return oResultadoOperacion;
        //    }

        //    oResultadoOperacion.EntidadDatos = recibido;

        //    return oResultadoOperacion;
        //}
    }
}
