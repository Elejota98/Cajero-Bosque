﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Ds.Exception;

namespace Ds.WCFProxy
{
    /// <summary>
    /// ProxySeguro para el consumo de servicios
    /// </summary>
    public class SafeProxy
    {

        /// <summary>
        /// Used in environments where WCF proxies are created and closed all the time.
        /// In case something goes wrong it aborts (immediately closes) the communications.
        /// </summary>
        /// <remarks>
        /// Design Pattern: Proxy
        /// 
        /// As an alternative one could implement proxy pooling to increase performance.
        /// </remarks>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="action">The typed action.</param>
        public static void DoActionAndClose<T>(T client, Action<T> action) where T : ICommunicationObject
        {
            try
            {
                action(client);
                client.Close();
            }
            catch
            {
                client.Abort();
            }
        }

        /// <summary>
        /// Perform action without closing it. This is for long running client proxies.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="action">The typed action.</param>
        public static void DoAction<T>(T client, Action<T> action) where T : ICommunicationObject
        {
            try
            {
                if (client.State == CommunicationState.Faulted || client.State == CommunicationState.Closed)
                {
                    client = (T)Activator.CreateInstance(client.GetType(), null);
                }
                action(client);
                client.Close();
            }
            catch (System.Exception oException)
            {
                client.Abort();
                client.Close();
                throw new ServiceException(oException.Message, oException);
            }
        }


    }
}
