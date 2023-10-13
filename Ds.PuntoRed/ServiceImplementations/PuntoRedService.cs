using Ds.BaseService;
using Ds.BaseService.MessageBase;
using Ds.BusinessService.Entities;
using Ds.PuntoRed.Messages;
using Ds.PuntoRed.ServiceContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ds.PuntoRed.ServiceImplementations
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PuntoRedService : ServiceBase, IPuntoRedService
    {
        public getPuntoRed_Response getOperadorRecarga(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Recargas");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Recargas");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");

                    ////PRODUCCION
                    string json = "{\"proceso\":\"01001\"," +
                                  "\"usuarioHost\":\"SMARTCOIN\"," +
                                  "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                                  "\"comercio\":\"374178\"," +
                                  "\"puntoVenta\":\"188140\"}";


                    ////PRUEBAS
                    //string json = "{\"proceso\":\"01001\"," +
                    //             "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //             "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //             "\"comercio\":\"352621\"," +
                    //             "\"puntoVenta\":\"176629\"}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getPuntoRed_Response setRecarga(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Recargas");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Recargas");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");


                    ////PRODUCCION
                    string json = "{\"proceso\":\"01002\"," +
                                  "\"usuarioHost\":\"SMARTCOIN\"," +
                                  "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                                  "\"comercio\":\"374178\"," +
                                  "\"puntoVenta\":\"188140\"," +
                                  "\"datos\":{" +
                                  "\"numero\":\"" + request.sLinea + "\"," +
                                  "\"valor\":\"" + request.sValor + "\"," +
                                  "\"terminal\":\"285499\"," +
                                   "\"trace\":\"" + request.sTransaccion + "\"," +// numero de transaccion
                                  "\"codigoProveedor\":\"" + request.sOperador + "\"," +
                                  "\"claveCXR\":\"21069102\"}" + "}";//clave

                    ////PRUBEAS
                    //string json = "{\"proceso\":\"01002\"," +
                    //              "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //              "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //              "\"comercio\":\"352621\"," +
                    //              "\"puntoVenta\":\"176629\"," +
                    //              "\"datos\":{" +
                    //              "\"numero\":\"" + request.sLinea + "\"," +
                    //              "\"valor\":\"" + request.sValor + "\"," +
                    //              "\"terminal\":\"265342\"," +
                    //              "\"trace\":\"" + request.sTransaccion + "\"," +// numero de transaccion 
                    //              "\"codigoProveedor\":\"" + request.sOperador + "\"," +
                    //              "\"claveCXR\":\"25808102\"}" + "}";//clave

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getPuntoRed_Response getOperadorPaquete(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Paquetes");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Paquetes");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");

                    ////PRODUCCION
                    string json = "{\"proceso\":\"02001\"," +
                                       "\"usuarioHost\":\"SMARTCOIN\"," +
                                       "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                                       "\"comercio\":\"374178\"," +
                                       "\"puntoVenta\":\"188140\"}";


                    ////PRUEBAS
                    //string json = "{\"proceso\":\"02001\"," +
                    //            "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //            "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //            "\"comercio\":\"352621\"," +
                    //            "\"puntoVenta\":\"176629\"}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getPuntoRed_Response getCategoriaPaquete(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Paquetes");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Paquetes");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");

                    ////PRODUCCION
                    string json = "{\"proceso\":\"02002\"," +
                              "\"usuarioHost\":\"SMARTCOIN\"," +
                              "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                              "\"comercio\":\"374178\"," +
                              "\"puntoVenta\":\"188140\"," +
                              "\"datos\":{" +
                              "\"codigoProveedor\":\"" + request.sOperador + "\"}" + "}";


                    ////PRUEBAS
                    //string json = "{\"proceso\":\"02002\"," +
                    //          "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //          "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //          "\"comercio\":\"352621\"," +
                    //          "\"puntoVenta\":\"176629\"," +
                    //          "\"datos\":{" +
                    //          "\"codigoProveedor\":\"" + request.sOperador + "\"}" + "}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getPuntoRed_Response getPaquetesXCategoriaPaquete(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Paquetes");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Paquetes");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");

                    ////PRODUCCION
                    string json = "{\"proceso\":\"02003\"," +
                              "\"usuarioHost\":\"SMARTCOIN\"," +
                              "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                              "\"comercio\":\"374178\"," +
                              "\"puntoVenta\":\"188140\"," +
                              "\"datos\":{" +
                              "\"codigoCategoria\":\"" + request.sCategoria + "\"}" + "}";


                    ////PRUEBAS
                    //string json = "{\"proceso\":\"02003\"," +
                    //          "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //          "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //          "\"comercio\":\"352621\"," +
                    //          "\"puntoVenta\":\"176629\"," +
                    //          "\"datos\":{" +
                    //          "\"codigoCategoria\":\"" + request.sCategoria + "\"}" + "}";


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public getPuntoRed_Response setComprarPaquete(getPuntoRed_Request request)
        {

            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

            getPuntoRed_Response response = new getPuntoRed_Response();

            try
            {

                response.CorrelationId = request.RequestId;

                if (!ValidRequest(request, response))
                    return response;


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.125.15.4:17201/serviciosIntegracionHost/api/host/Paquetes");
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://201.234.66.17:17201/serviciosIntegracionHost/api/host/Paquetes");


                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string cadenaEncriptada = GetSHA256("dN8W2E*Vh@gtvzcw");


                    ////PRODUCCION
                    string json = "{\"proceso\":\"02004\"," +
                            "\"usuarioHost\":\"SMARTCOIN\"," +
                            "\"claveHost\":\"" + cadenaEncriptada + "\"," +
                            "\"comercio\":\"374178\"," +
                            "\"puntoVenta\":\"188140\"," +
                            "\"datos\":{" +
                            "\"codigoProveedor\":\"" + request.sOperador + "\"," +
                            "\"codigoCategoria\":\"" + request.sCategoria + "\"," +
                            "\"codigoPaquete\":\"" + request.sPaquete + "\"," +
                            "\"numero\":\"" + request.sLinea + "\"," +
                            "\"trace\":\"" + request.sTransaccion + "\"," +// numero de transaccion 
                            "\"sku\":\"" + request.sSku + "\"," +
                            "\"valor\":\"" + request.sValor + "\"," +
                            "\"terminal\":\"285499\"," +
                            "\"claveCXR\":\"21069102\"}" + "}";//clave

                    ////PRUBEAS
                    //string json = "{\"proceso\":\"02004\"," +
                    //          "\"usuarioHost\":\"PRUEBASSMARTCOIN\"," +
                    //          "\"claveHost\":\"0CA7DF2AC642E57683D2B33D40DA67448AC943BBA02EF7627BCF43BE5361C89B\"," +
                    //          "\"comercio\":\"352621\"," +
                    //          "\"puntoVenta\":\"176629\"," +
                    //          "\"datos\":{" +
                    //          "\"codigoProveedor\":\"" + request.sOperador + "\"," +
                    //          "\"codigoCategoria\":\"" + request.sCategoria + "\"," +
                    //          "\"codigoPaquete\":\"" + request.sPaquete + "\"," +
                    //          "\"numero\":\"" + request.sLinea + "\"," +
                    //          "\"trace\":\"" + request.sTransaccion + "\"," +// numero de transaccion 
                    //          "\"sku\":\"" + request.sSku + "\"," +
                    //          "\"valor\":\"" + request.sValor + "\"," +
                    //          "\"terminal\":\"265342\"," +
                    //          "\"claveCXR\":\"25808102\"}" + "}";//clave

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var result = streamReader.ReadToEnd();
                    response.oResult = result;
                }


            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.ToString();
            }


            return response;
        }

        public string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
